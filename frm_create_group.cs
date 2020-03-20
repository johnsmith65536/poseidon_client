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
            var req = new http._Relation.FetchFriendListReq()
            {
                UserId = Class1.UserId
            };
            var resp = http._Relation.FetchFriendList(req);
            foreach (var userId in resp.OnlineUserIds)
                clb_friend.Items.Add(userId, false);
            foreach (var userId in resp.OfflineUserIds)
                clb_friend.Items.Add(userId, false);

        }

        private void button1_Click(object sender, EventArgs e)
        {
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
