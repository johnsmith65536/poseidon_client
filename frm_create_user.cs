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
    public partial class frm_create_user : Form
    {
        public frm_create_user()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var req = new http._User.CreateUserReq()
            {
                NickName = txt_nick_name.Text,
                Password = Class1.GenerateMD5(txt_password.Text)
            };
            var resp = http._User.CreateUser(req);
            MessageBox.Show("用户创建成功，用户ID为" + resp.UserId, "", MessageBoxButtons.OK);
            this.Close();
        }
    }
}
