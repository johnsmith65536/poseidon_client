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
    public partial class frm_invite_group : Form
    {
        public long GroupId;
        public frm_invite_group()
        {
            InitializeComponent();
        }
        public frm_invite_group(long groupId)
        {
            InitializeComponent();
            GroupId = groupId;
        }

        private void frm_invite_group_Load(object sender, EventArgs e)
        {
            var req = new http._Group_User.InviteGroupFriendListReq()
            {
                UserId = Class1.UserId,
                GroupId = GroupId
            };
            var resp = http._Group_User.InviteGroupFriendList(req);
            foreach (var user in resp.NotInGroupUsers)
                clb_friend.Items.Add($"{user.NickName}({user.Id})", false);
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
            {
                if (clb_friend.GetItemChecked(i))
                {
                    var text = clb_friend.GetItemText(clb_friend.Items[i]);
                    var l = text.LastIndexOf('(');
                    var r = text.LastIndexOf(')');
                    userIds.Add(long.Parse(text.Substring(l + 1, r - l - 1)));
                }
            }
            var req = new http._Group_User.InviteGroupReq()
            {
                UserIds = userIds,
                GroupId = GroupId
            };
            http._Group_User.InviteGroup(req);
            this.Close();
        }
    }
}
