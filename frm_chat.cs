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
    public partial class frm_chat : Form
    {
        public static long userIdChat;
        public frm_chat()
        {
            InitializeComponent();
        }
        public frm_chat(long userId)
        {
            InitializeComponent();
            userIdChat = userId;
            this.Text = "与" + userId + "的会话";
            DataTable dt = Class1.sql.SqlTable($"SELECT user_id_send, content, create_time FROM `message` WHERE (`user_id_send` = {Class1.UserId} AND `user_id_recv` = {userId}) OR (`user_id_send` = {userId} AND `user_id_recv` = {Class1.UserId})");
            for (int i=0;i<dt.Rows.Count;i++)
            {
                var userIdSend = (long)dt.Rows[i]["user_id_send"];
                var content = (string)dt.Rows[i]["content"];
                var createTime = (long)dt.Rows[i]["create_time"];

                if (userIdSend == Class1.UserId)
                {
                    //listBox1.Items.Add("我:" + content + " " + Class1.FormatDateTime(Class1.StampToDateTime(createTime)));
                    Class1.GridAdd(dgv_msg, new Dictionary<string, object> {
            {"user_id",userIdSend},
            {"content",content},
            {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(createTime))}
        });
                }
                else
                {

                    //listBox1.Items.Add("对方:" + content + " " + Class1.FormatDateTime(Class1.StampToDateTime(createTime)));
                    Class1.GridAdd(dgv_msg, new Dictionary<string, object> {
            {"user_id",userIdSend},
            {"content",content},
            {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(createTime))}
        });
                }
            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            var content = txt_send.Text;
            Tuple<long, long> resp = rpc._Message.SendMessage(Class1.UserId, userIdChat, content, 0, 0);
            //txt_status.Text += "消息发送成功, msgId = " + resp.Item1 + Environment.NewLine;
            bool ret = Class1.sql.ExecuteNonQuery($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, msg_type, is_read) VALUES({resp.Item1}, " +
                            $"{Class1.UserId}, {userIdChat}, 0, '{content}', {resp.Item2}, 0, 0)");
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //listBox1.Items.Add("我:" + content + " " + Class1.FormatDateTime(Class1.StampToDateTime(resp.Item2)));
            Class1.GridAdd(dgv_msg, new Dictionary<string, object> {
            {"user_id",Class1.UserId},
            {"content",content},
            {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(resp.Item2))}
        });
            txt_send.Text = "";
        }

        private void frm_chat_Load(object sender, EventArgs e)
        {

        }

        private void frm_chat_FormClosed(object sender, FormClosedEventArgs e)
        {
            Class1.formChatPool.Remove(userIdChat);
        }

        private void dgv_msg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
