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

            switch (type)
            {
                case (long)Class1.UnReadMsgType.Message:
                    {
                        var msgType = (int)dict["msg_type"];
                        switch (msgType)
                        {
                            case (int)Class1.MsgType.PrivateChat:
                                {
                                    
                                    var ids = (List<long>)dict["ids"];
                                    var userIdSend = (long)dict["user_id_send"];
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
                                    var updateFriendLastReadMsgIdReq = new http._User_Relation.UpdateFriendLastReadMsgIdReq()
                                    {
                                        UserId = Class1.UserId,
                                        LastReadMsgId = new Dictionary<long, long>()
                                    };
                                    foreach (var id in ids)
                                    {
                                        if (updateFriendLastReadMsgIdReq.LastReadMsgId.ContainsKey(userIdSend))
                                            updateFriendLastReadMsgIdReq.LastReadMsgId[userIdSend] = Math.Max(updateFriendLastReadMsgIdReq.LastReadMsgId[userIdSend], id);
                                        else
                                            updateFriendLastReadMsgIdReq.LastReadMsgId.Add(userIdSend, id);
                                    }
                                    http._User_Relation.UpdateFriendLastReadMsgId(updateFriendLastReadMsgIdReq);


                                    var subItem = Class1.unReadPrivateMsgItemPool[userIdSend];
                                    clb_unread_msg.Items[0].SubItems.Remove(subItem);
                                    Class1.unReadPrivateMsgItemPool.Remove(userIdSend);
                                    break;
                                }
                            case (int)Class1.MsgType.GroupChat:
                                {
                                    var maxId = (long)dict["max_id"];
                                    var groupId = (long)dict["group_id"];
                                    frm_group frmGroup;
                                    if (Class1.formGroupPool.ContainsKey(groupId))
                                    {
                                        frmGroup = Class1.formGroupPool[groupId];
                                        frmGroup.Activate();
                                    }
                                    else
                                    {
                                        frmGroup = new frm_group(Class1.GroupId2Group[groupId]);
                                        Class1.formGroupPool.Add(groupId, frmGroup);
                                        frmGroup.Show();
                                    }
                                    //Class1.UpdateMessageStatus(readMessage, new Dictionary<long, int>());
                                    var req = new http._Group_User.UpdateGroupLastReadMsgIdReq()
                                    {
                                        UserId = Class1.UserId,
                                        LastReadMsgId = new Dictionary<long, long>() { { groupId, maxId } }
                                    };
                                    http._Group_User.UpdateGroupLastReadMsgId(req);
                                    var subItem = Class1.unReadGroupMsgItemPool[groupId];
                                    clb_unread_msg.Items[2].SubItems.Remove(subItem);
                                    Class1.unReadGroupMsgItemPool.Remove(groupId);
                                    break;
                                }
                            default:
                                throw new Exception("unknown msg_type");
                        }
                        break;
                    }
                case (long)Class1.UnReadMsgType.AddFriend:
                    {
                        var userIdSend = (long)dict["user_id_send"];
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

                        var req = new http._User_Relation.ReplyAddFriendReq()
                        {
                            Id = id,
                            Status = status
                        };
                        var resp = http._User_Relation.ReplyAddFriend(req);
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
                        clb_unread_msg.Items[1].SubItems.Remove(e.SelectSubItem);
                        break;
                    }
                case (long)Class1.UnReadMsgType.ReplyAddFriend:
                    {
                        var id = (long)dict["id"];
                        Class1.UpdateMessageStatus(new Dictionary<long, int> { { id, 1 } }, new Dictionary<long, int>());
                        clb_unread_msg.Items[1].SubItems.Remove(e.SelectSubItem);
                        break;
                    }
                case (long)Class1.UnReadMsgType.AddGroup:
                    {
                        var userIdSend = (long)dict["user_id_send"];
                        if (!Class1.IsOnline)
                        {
                            MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        var id = (long)dict["id"];
                        var groupId = (long)dict["group_id"];
                        var ret = MessageBox.Show(userIdSend + "请求加入群聊" + groupId + "，是否接受？", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                        int status;
                        if (ret == DialogResult.Yes)
                            status = (int)Class1.AddFriendStatus.Accepted;
                        else if (ret == DialogResult.No)
                            status = (int)Class1.AddFriendStatus.Rejected;
                        else
                            return;

                        var req = new http._Group_User.ReplyAddGroupReq()
                        {
                            Id = id,
                            Status = status
                        };
                        var resp = http._Group_User.ReplyAddGroup(req);
                        var replyId = resp.Id;
                        var createTime = resp.CreateTime;
                        bool ok = Class1.sql.ExecuteNonQuery($"UPDATE `group_user_request` SET `status` = {status} WHERE `id` = {id}");
                        if (!ok)
                        {
                            MessageBox.Show("DB错误，UPDATE group_user_request", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        ok = Class1.sql.ExecuteNonQuery($"INSERT INTO `group_user_request`(id, user_id_send, user_id_recv, group_id, create_time, status, parent_id, type) VALUES({replyId}, " +
                            $"{Class1.UserId}, {userIdSend}, {groupId}, {createTime}, 0, {id}, {(int)Class1.GroupUserRequestType.AddGroup})");
                        if (!ok)
                        {
                            MessageBox.Show("DB错误，INSERT INTO group_user_request", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        clb_unread_msg.Items[1].SubItems.Remove(e.SelectSubItem);
                        break;
                    }
                case (long)Class1.UnReadMsgType.ReplyAddGroup:
                    {
                        var id = (long)dict["id"];
                        Class1.UpdateMessageStatus(new Dictionary<long, int>(), new Dictionary<long, int> { { id, 1 } });
                        clb_unread_msg.Items[1].SubItems.Remove(e.SelectSubItem);
                        break;
                    }
                case (long)Class1.UnReadMsgType.InviteAddGroup:
                    {

                        break;
                    }
                case (long)Class1.UnReadMsgType.ReplyInviteAddGroup:
                    {

                        break;
                    }
                default:
                        throw new Exception("unknown UnReadMsgType");
            }
            icon.ChangeIconState();
        }
    }
}
