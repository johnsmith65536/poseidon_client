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
    public partial class frm_search_user : Form
    {
        long userIdRecv;
        public frm_search_user()
        {
            InitializeComponent();
        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            //var users = rpc._User.SearchUser(Class1.UserId,txt_search.Text);
            var req = new http._User.SearchUserReq()
            {
                UserId = Class1.UserId,
                Data = txt_search.Text
            };
            var resp = http._User.SearchUser(req);
            dataGridView1.Rows.Clear();
            foreach (var user in resp.Users)
            {
                var index = dataGridView1.Rows.Add(new DataGridViewRow());
                dataGridView1.Rows[index].Cells["user_id"].Value = user.Id.ToString();
                dataGridView1.Rows[index].Cells["nick_name"].Value = user.NickName;
                if (Class1.DateTimeToStamp(DateTime.UtcNow) - user.LastOnlineTime < 30)
                    dataGridView1.Rows[index].Cells["last_online_time"].Value = "在线";
                else
                    dataGridView1.Rows[index].Cells["last_online_time"].Value = Class1.FormatDateTime(Class1.StampToDateTime(user.LastOnlineTime));
                dataGridView1.Rows[index].Cells["is_friend"].Value = user.IsFriend;

            }
        }

        private void frm_search_user_Load(object sender, EventArgs e)
        {
        }


        private void mnu_add_friend_Click(object sender, EventArgs e)
        {
            var req = new http._Relation.AddFriendReq()
            {
                UserIdSend = Class1.UserId,
                UserIdRecv = userIdRecv
            };
            var resp = http._Relation.AddFriend(req);
            //var resp = rpc._Relation.AddFriend(Class1.UserId,userIdRecv);
            var id = resp.Id;
            var createTime = resp.CreateTime;
            var statusCode = resp.StatusCode;

            switch (statusCode)
            
            {
                case 1:
                    MessageBox.Show("重复的好友请求", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                case 2:
                    MessageBox.Show("已为好友", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
            }

            bool ret = Class1.sql.ExecuteNonQuery($"INSERT INTO `user_relation_request`(id, user_id_send, user_id_recv, create_time, status, parent_id) VALUES({id}, " +
                            $"{Class1.UserId}, {userIdRecv}, {createTime}, 0, -1)");
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO user_relation_request失败777", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show("好友请求已发送666", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    Console.WriteLine(dataGridView1.Rows[e.RowIndex].Cells["user_id"].Value);
                    userIdRecv = long.Parse(dataGridView1.Rows[e.RowIndex].Cells["user_id"].Value.ToString());
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    mnu_add_friend.Enabled = !Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells["is_friend"].Value.ToString()) && userIdRecv != Class1.UserId;
                    mnu_strip.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}