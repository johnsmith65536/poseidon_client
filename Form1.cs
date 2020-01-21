using Poseidon.infra.redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thrift.Protocol;
using Thrift.Transport;
using StackExchange.Redis;
using System.Threading;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Poseidon.infra.sqlite;

namespace Poseidon
{
    unsafe public partial class Form1 : Form
    {
        public static Int64 UserId;
        SQLiteDB sql = new SQLiteDB();
        public Form1()
        {
            InitializeComponent();
        }
        public enum BroadcastMsgType
        {
            Chat = 0,
            AddFriend = 1
        }
        public enum AddFriendStatus
        {
            Pending = 0,
            Rejected = 1,
            Accepted = 2
        }
        struct Message
        {
            public long Id;
            public long UserIdSend;
            public long UserIdRecv;
            public long GroupId;
            public string Content;
            public long CreateTime;
            public int MsgType;
            public bool IsRead;
        };
        struct AddFriend
        {
            public long Id;
            public long UserIdSend;
            public long UserIdRecv;
            public long CreateTime;
        };
        public static BroadcastMsgType GetBroadcastMsgType(string jsonString)
        {
            var obj = JsonConvert.DeserializeObject<dynamic>(jsonString);
            BroadcastMsgType type = obj.BroadcastMsgType;
            return type;
        }
        public static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("a");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserId = Convert.ToInt64(txt_user_id.Text);
            var password = GenerateMD5(txt_password.Text);
            var ok = rpc.Loginout.Login(UserId, password);
            if (ok)
            {
                string sqlFile = $"{System.Environment.CurrentDirectory}\\{UserId}\\profile.db";
                sql.CreateDBFile(sqlFile);
                sql.Connection(sqlFile);
                var ret = sql.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS `message` (
                  `id` INTEGER NOT NULL,
                  `user_id_send` INTEGER NOT NULL,
                  `user_id_recv` INTEGER NOT NULL,
                  `group_id` INTEGER NOT NULL,
                  `content` TEXT,
                  `create_time` INTEGER NOT NULL,
                  `msg_type` INTEGER NOT NULL,
                  `is_read` INTEGER DEFAULT NULL,
                  PRIMARY KEY(`id`)
                ) ; ");
                if (!ret)
                {
                    MessageBox.Show("DB错误，建表失败","信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                ret = sql.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS `user_relation_request` (
                  `id` INTEGER NOT NULL,
                  `user_id_send` INTEGER NOT NULL,
                  `user_id_recv` INTEGER NOT NULL,
                  `create_time` INTEGER NOT NULL,
                  `status` INTEGER NOT NULL,
                  PRIMARY KEY (`id`)
                ) ;");
                if (!ret)
                {
                    MessageBox.Show("DB错误，建表失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }



                Thread t1 = new Thread(new ThreadStart(() =>
                {
                    ConnectionMultiplexer redisCli = redis.GetRedisConn();
                    var sub = redisCli.GetSubscriber();
                    var chn = "poseidon_heart_beat_channel";
                    sub.Subscribe(chn, HeartBeatChannel);
                }));
                t1.Start();

                Thread t2 = new Thread(new ThreadStart(() =>
                {
                    ConnectionMultiplexer redisCli = redis.GetRedisConn();
                    var sub = redisCli.GetSubscriber();
                    var chn = "poseidon_message_channel_" + UserId;
                    sub.Subscribe(chn, MessageChannel);
                }));
                t2.Start();

                Thread t3 = new Thread(new ThreadStart(() =>    //异步拉取离线消息
                {
                    long messageId = -1, userRelationId = -1;
                    DataTable dt = sql.SqlTable($"SELECT count(*) as count, MAX(id) as id FROM `message` WHERE `user_id_recv` = {UserId}");
                    if (dt != null && dt.Rows.Count == 1 && (long)dt.Rows[0]["count"] > 0)
                        messageId = (long)dt.Rows[0]["id"];
                    dt = sql.SqlTable($"SELECT count(*) as count, MAX(id) as id FROM `user_relation_request` WHERE `user_id_recv` = {UserId}");
                    if (dt != null && dt.Rows.Count == 1 && (long)dt.Rows[0]["count"] > 0)
                        userRelationId = (long)dt.Rows[0]["id"];
                    var resp = rpc.Messages.FetchOfflineMessage(UserId, messageId, userRelationId);
                    var messages = resp.Item1; 
                    var userRelations = resp.Item2;

                    //离线消息落库
                    foreach (var msg in messages)
                    {
                        bool ret1 = sql.ExecuteNonQuery($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, msg_type, is_read) VALUES({msg.Id}, " +
                            $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.GroupId}, '{msg.Content}', {msg.CreateTime}, {msg.MsgType}, {msg.IsRead})");
                        if (!ret1)
                        {
                            MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        AddList(listBox1, $"[新消息]来自{UserId}，内容:{msg.Content}");
                    }

                    foreach (var msg in userRelations)
                    {
                        bool ret1 = sql.ExecuteNonQuery($"INSERT INTO `user_relation_request`(id, user_id_send, user_id_recv, create_time, status) VALUES({msg.Id}, " +
                            $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.CreateTime}, {msg.Status})");
                        if (!ret1)
                        {
                            MessageBox.Show("DB错误，INSERT INTO user_relation_request失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        AddList(listBox1, $"[好友请求]来自{UserId}的好友请求");
                    }


                }));
                t3.Start();

            }
            Console.WriteLine(ok);
        }
        public void HeartBeatChannel(RedisChannel cnl, RedisValue val)
        {
            //ConnectionMultiplexer redisCli = redis.GetRedisConn();
            rpc.Heart_Beat.HeartBeat(UserId);
            Console.WriteLine("Send HeartBeat");
            //Console.WriteLine("频道：" + cnl + "\t收到消息:" + val); ;
            // Console.WriteLine("线程：" + Thread.CurrentThread.ManagedThreadId + ",是否线程池：" + Thread.CurrentThread.IsThreadPoolThread);
            // if (val == "close")
            //      redisCli.GetSubscriber().Unsubscribe(cnl);
            //  if (val == "closeall")
            //     redisCli.GetSubscriber().UnsubscribeAll();
        }
        public void MessageChannel(RedisChannel cnl, RedisValue val)
        {
            ConnectionMultiplexer redisCli = redis.GetRedisConn();
            //rpc.Heart_Beat.HeartBeat(UserId);
            //Console.WriteLine("Send HeartBeat");
            Console.WriteLine("频道：" + cnl + "\t收到消息:" + val);

            var msgType = GetBroadcastMsgType(val.ToString());
            switch (msgType)
            {
                case BroadcastMsgType.Chat:
                    {
                        Message msg = JsonConvert.DeserializeObject<Message>(val.ToString());
                        var newText = txt_status.Text + "消息接受成功, msgId = " + msg.Id + ", content = " + msg.Content + Environment.NewLine;
                        SetText(txt_status,newText);
                        rpc.Messages.MessageDelivered(msg.Id);
                        bool ret = sql.ExecuteNonQuery($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, msg_type, is_read) VALUES({msg.Id}, " +
                            $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.GroupId}, '{msg.Content}', {msg.CreateTime}, {msg.MsgType}, {msg.IsRead})");
                        if (!ret)
                        {
                            MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        break;
                    }
                case BroadcastMsgType.AddFriend:
                    {
                        AddFriend msg = JsonConvert.DeserializeObject<AddFriend>(val.ToString());
                        var newText = txt_status.Text + "用户" + msg.UserIdSend + "请求添加你为好友, msgId = " + msg.Id + Environment.NewLine;
                        SetText(txt_status,newText);
                        SetText(txt_add_friend_id, msg.Id.ToString());
                        bool ret = sql.ExecuteNonQuery($"INSERT INTO `user_relation_request`(id, user_id_send, user_id_recv, create_time, status) VALUES({msg.Id}, " +
                            $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.CreateTime}, 0)");
                        if (!ret)
                        {
                            MessageBox.Show("DB错误，INSERT INTO user_relation_request失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Unknown BroadcastMsgType, type: " + msgType);
                        break;
                    }
            }


            // Console.WriteLine("线程：" + Thread.CurrentThread.ManagedThreadId + ",是否线程池：" + Thread.CurrentThread.IsThreadPoolThread);
            // if (val == "close")
            //      redisCli.GetSubscriber().Unsubscribe(cnl);
            //  if (val == "closeall")
            //     redisCli.GetSubscriber().UnsubscribeAll();
        }
        delegate void SetTextCallBack(TextBox txt_box, string text);
        private void SetText(TextBox txt_box,string text)
        {
            if (txt_box.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(SetText);
                this.Invoke(stcb, new object[] { txt_box, text });
            }
            else
            {
                txt_box.Text = text;
            }
        }

        delegate void AddListCallBack(ListBox list_box, Object item);
        private void AddList(ListBox list_box, Object item)
        {
            if (list_box.InvokeRequired)
            {
                AddListCallBack stcb = new AddListCallBack(AddList);
                this.Invoke(stcb, new object[] { list_box, item });
            }
            else
            {
                list_box.Items.Add(item);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            TTransport transport = new TSocket("192.168.6.128", 8080);
            TProtocol protocol = new TBinaryProtocol(transport);
            Server.Client client = new Server.Client(protocol);
            transport.Open();
            var userId = Convert.ToInt64(txt_user_id.Text);
            LogoutReq req = new LogoutReq()
            {
                UserId = userId,
            };
            LogoutResp resp = client.Logout(req);
            Console.WriteLine(resp);
            transport.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rpc.Init.InitRpcClient();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var idRecv = Convert.ToInt64(txt_id_recv.Text);
            Tuple<long,long> resp = rpc.Messages.SendMessage(UserId, idRecv, txt_send.Text, 0, 0);
            txt_status.Text += "消息发送成功, msgId = " + resp.Item1 + Environment.NewLine;
            bool ret = sql.ExecuteNonQuery($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, msg_type, is_read) VALUES({resp.Item1}, " +
                            $"{UserId}, {idRecv}, 0, '{txt_send.Text}', {resp.Item2}, 0, 0)");
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            var userIdRecv = Convert.ToInt64(txt_friend.Text);
            Tuple<long,long> resp = rpc.Relation.AddFriend(UserId, userIdRecv);
            bool ret = sql.ExecuteNonQuery($"INSERT INTO `user_relation_request`(id, user_id_send, user_id_recv, create_time, status) VALUES({resp.Item1}, " +
                            $"{UserId}, {userIdRecv}, {resp.Item2}, 0)");
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO user_relation_request失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt64(txt_add_friend_id.Text);
            rpc.Relation.ReplyAddFriend(id, (int)AddFriendStatus.Accepted);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt64(txt_add_friend_id.Text);
            rpc.Relation.ReplyAddFriend(id, (int)AddFriendStatus.Rejected);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            txt_friend_list.Text = "";
            var resp = rpc.Relation.FetchFriendList(UserId);
            foreach (long userId in resp.Item1)
                txt_friend_list.Text = txt_friend_list.Text + userId + "(online)" + Environment.NewLine;
            foreach (long userId in resp.Item2)
                txt_friend_list.Text = txt_friend_list.Text + userId + "(offline)" + Environment.NewLine;
        }
    }
}
