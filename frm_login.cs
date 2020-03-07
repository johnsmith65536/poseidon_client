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
        private bool login(long userId, string password)
        {
            var req = new http._Login.LoginReq()
            {
                UserId = userId,
                Password = password
            };

            var resp = http._Login.Login(req);
            if (resp.Success)
            {
                string sqlFile = $"{System.Environment.CurrentDirectory}\\{userId}\\profile.db";
                Class1.sql.CreateDBFile(sqlFile);
                Class1.sql.Connection(sqlFile);
                var ret = Class1.sql.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS `message` (
                  `id` INTEGER NOT NULL,
                  `user_id_send` INTEGER NOT NULL,
                  `user_id_recv` INTEGER NOT NULL,
                  `group_id` INTEGER NOT NULL,
                  `content` TEXT,
                  `create_time` INTEGER NOT NULL,
                  `content_type` INTEGER NOT NULL,
                  `msg_type` INTEGER NOT NULL,
                  `is_read` INTEGER DEFAULT NULL,
                  PRIMARY KEY(`id`)
                ) ; ");
                if (!ret)
                {
                    MessageBox.Show("DB错误，建表失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                ret = Class1.sql.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS `user_relation_request` (
                  `id` INTEGER NOT NULL,
                  `user_id_send` INTEGER NOT NULL,
                  `user_id_recv` INTEGER NOT NULL,
                  `create_time` INTEGER NOT NULL,
                  `status` INTEGER NOT NULL,
                  `parent_id` INTEGER NOT NULL,
                  PRIMARY KEY (`id`)
                ) ;");
                if (!ret)
                {
                    MessageBox.Show("DB错误，建表失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                ret = Class1.sql.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS `object` (
                  `id` INTEGER NOT NULL,
                  `e_tag` TEXT NOT NULL,
                  `name` TEXT NOT NULL,
                  PRIMARY KEY (`id`)
                ) ;");
                if (!ret)
                {
                    MessageBox.Show("DB错误，建表失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                Class1.UserId = userId;
                return true;
            }
            return false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var userId = Convert.ToInt64(txt_user_id.Text);
            var password = Class1.GenerateMD5(txt_password.Text);
            var ok = login(userId, password);
            if (!ok)
            {
                MessageBox.Show("登陆失败，账号或密码有误", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            frm_main frm_main = new frm_main();
            this.Hide();
            frm_main.Show();
        }

        private void frm_login_Load(object sender, EventArgs e)
        {
            //rpc._Init.InitRpcClient();
        }
    }
}
