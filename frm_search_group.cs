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
    public partial class frm_search_group : Form
    {
        public frm_search_group()
        {
            InitializeComponent();
        }
        public long selectGroupId;

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var req = new http._Group.SearchGroupReq()
            {
                UserId = Class1.UserId,
                Data = txt_search.Text
            };
            var resp = http._Group.SearchGroup(req);
            dataGridView1.Rows.Clear();
            foreach (var group in resp.Groups)
            {
                var index = dataGridView1.Rows.Add(new DataGridViewRow());
                dataGridView1.Rows[index].Cells["group_id"].Value = group.Id.ToString();
                dataGridView1.Rows[index].Cells["group_name"].Value = group.Name;
                dataGridView1.Rows[index].Cells["create_time"].Value = Class1.FormatDateTime(Class1.StampToDateTime(group.CreateTime));
                dataGridView1.Rows[index].Cells["is_member"].Value = group.IsMember;
            }
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    selectGroupId = long.Parse(dataGridView1.Rows[e.RowIndex].Cells["group_id"].Value.ToString());
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    mnu_add_group.Enabled = !Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells["is_member"].Value.ToString());
                    mnu_strip.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void mnu_add_group_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var req = new http._Group_User.AddGroupReq()
            {
                UserId = Class1.UserId,
                GroupId = selectGroupId
            };
            var resp = http._Group_User.AddGroup(req);
            var id = resp.Id;
            var createTime = resp.CreateTime;
            var statusCode = resp.StatusCode;

            switch (statusCode)

            {
                case 1:
                    MessageBox.Show("重复的请求", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                case 2:
                    MessageBox.Show("已是群成员", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
            }

            bool ret = Class1.sql.ExecuteNonQuery($"INSERT INTO `group_user_request`(id, user_id_send, user_id_recv, group_id, create_time, status, parent_id, type) VALUES({id}, " +
                            $"{Class1.UserId}, 0, {selectGroupId}, {createTime}, 0, -1, {(int)Class1.GroupUserRequestType.AddGroup})");
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO group_user_request失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show("加群请求已发送", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
