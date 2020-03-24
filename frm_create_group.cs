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
    public partial class frm_create_group : Form
    {
        public frm_create_group()
        {
            InitializeComponent();
        }

        private void frm_create_group_Load(object sender, EventArgs e)
        {
            var req = new http._User_Relation.FetchFriendListReq()
            {
                UserId = Class1.UserId
            };
            var resp = http._User_Relation.FetchFriendList(req);
            foreach (var userId in resp.OnlineUserIds)
                clb_friend.Items.Add(userId, false);
            foreach (var userId in resp.OfflineUserIds)
                clb_friend.Items.Add(userId, false);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            List<long> userIds = new List<long>();
            for (int i = 0; i < clb_friend.Items.Count; i++)
                if (clb_friend.GetItemChecked(i))
                    userIds.Add(long.Parse(clb_friend.GetItemText(clb_friend.Items[i])));
            var req = new http._Group.CreateGroupReq()
            {
                Owner = Class1.UserId,
                Name = txt_group_name.Text,
                UserIds = userIds.ToArray()
            };
            http._Group.CreateGroup(req);
            MessageBox.Show("群组已建立", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
