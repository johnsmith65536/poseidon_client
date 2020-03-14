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
    public partial class frm_reply_add_friend : Form
    {
        public long id;
        public frm_reply_add_friend()
        {
            InitializeComponent();
        }
        public frm_reply_add_friend(long requestId)
        {
            InitializeComponent();
            id = requestId;
        }

        private void frm_reply_add_friend_Load(object sender, EventArgs e)
        {

        }

        private void btn_accept_Click(object sender, EventArgs e)
        {
            var req = new http._Relation.ReplyAddFriendReq()
            {
                Id = id,
                Status = (int)Class1.AddFriendStatus.Accepted
            };
            http._Relation.ReplyAddFriend(req);
        }

        private void btn_reject_Click(object sender, EventArgs e)
        {
            var req = new http._Relation.ReplyAddFriendReq()
            {
                Id = id,
                Status = (int)Class1.AddFriendStatus.Rejected
            };
            http._Relation.ReplyAddFriend(req);
        }
    }
}
