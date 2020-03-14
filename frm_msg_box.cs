using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poseidon
{
    public partial class frm_msg_box : Form
    {
        public frm_msg_box()
        {
            InitializeComponent();
        }

        private void clb_unread_msg_ClickSubItem(object sender, CCWin.SkinControl.ChatListClickEventArgs e, MouseEventArgs es)
        {
            if (es.Button != MouseButtons.Left)
                return;
            var dict = (Dictionary<string, object>)e.SelectSubItem.Tag;
            var type = (long)dict["type"];
            var userIdSend = (long)dict["user_id_send"];


            switch (type)
            {
                case (long)Class1.UnReadMsgType.Message:
                    {
                        var ids = (List<long>)dict["ids"];
                        frm_chat frm_chat;
                        if (Class1.formChatPool.ContainsKey(userIdSend))
                        {
                            frm_chat = Class1.formChatPool[userIdSend];
                            frm_chat.Activate();
                        }
                        else
                        {
                            frm_chat = new frm_chat(userIdSend);
                            Class1.formChatPool.Add(userIdSend, frm_chat);
                            frm_chat.Show();
                        }
                        Dictionary<long, int> readMessage = new Dictionary<long, int>();
                        foreach (var msgId in ids)
                            readMessage.Add(msgId, 1);
                        Class1.UpdateMessageStatus(readMessage, new Dictionary<long, int>());
                        Class1.LoadUnReadMessage();
                        break;
                    }
                case (long)Class1.UnReadMsgType.AddFriend:
                    {
                        if (!Class1.IsOnline)
                        {
                            MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        var id = (long)dict["id"];
                        var ret = MessageBox.Show(userIdSend + "请求添加为好友，是否接受？", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                        int status;
                        if (ret == DialogResult.Yes)
                            status = (int)Class1.AddFriendStatus.Accepted;
                        else if (ret == DialogResult.No)
                            status = (int)Class1.AddFriendStatus.Rejected;
                        else
                            return;

                        var req = new http._Relation.ReplyAddFriendReq()
                        {
                            Id = id,
                            Status = status
                        };
                        var resp = http._Relation.ReplyAddFriend(req);
                        var replyId = resp.Id;
                        var createTime = resp.CreateTime;
                        bool ok = Class1.sql.ExecuteNonQuery($"UPDATE `user_relation_request` SET `status` = {status} WHERE `id` = {id}");
                        if (!ok)
                        {
                            MessageBox.Show("DB错误，UPDATE user_relation_request", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        ok = Class1.sql.ExecuteNonQuery($"INSERT INTO `user_relation_request`(id, user_id_send, user_id_recv, create_time, status, parent_id) VALUES({replyId}, " +
                            $"{Class1.UserId}, {userIdSend}, {createTime}, 0, {id})");
                        if (!ok)
                        {
                            MessageBox.Show("DB错误，INSERT INTO user_relation_request", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        Class1.LoadUnReadMessage();
                        break;
                    }
                case (long)Class1.UnReadMsgType.ReplyAddFriend:
                    {
                        var id = (long)dict["id"];
                        Class1.UpdateMessageStatus(new Dictionary<long, int>(), new Dictionary<long, int> { { id, 1 } });
                        Class1.LoadUnReadMessage();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("unknown type");
                        break;
                    }
            }
        }
    }
}
