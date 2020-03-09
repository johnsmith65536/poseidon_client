using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Poseidon.infra.sqlite;
using System.Threading;
using StackExchange.Redis;
using Poseidon.infra.redis;
using Newtonsoft.Json;

namespace Poseidon
{
    public partial class frm_login : Form
    {
        public frm_login()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var userId = Convert.ToInt64(txt_user_id.Text);
            var password = Class1.GenerateMD5(txt_password.Text);
            var ok = Class1.Login(userId, password);
            if (!ok)
            {
                MessageBox.Show("登陆失败，账号或密码有误", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Class1.frm_main = new frm_main();
            this.Hide();
            Class1.frm_main.Show();
        }

        private void frm_login_Load(object sender, EventArgs e)
        {
            //rpc._Init.InitRpcClient();
        }
    }
}
