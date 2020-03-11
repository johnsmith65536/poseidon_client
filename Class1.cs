using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Poseidon.infra.sqlite;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Collections;
using System.Net;
using Newtonsoft.Json;
using Poseidon.infra.redis;
using CCWin.SkinControl;
using System.Drawing;
using System.Threading;

namespace Poseidon
{
    public static class Class1
    {
        public const string Ip = "192.168.6.128";

        public const string HttpPort = ":8081";

        public const string EndPoint = "oss-cn-shenzhen.aliyuncs.com";
        public const string BucketName = "poseidon-data";

        public static long UserId;
        public static string Password;

        public static string AccessToken;

        public static bool IsOnline;

        public static frm_main frm_main;

        public static SQLiteDB sql = new SQLiteDB();

        public static Dictionary<long, frm_chat> formChatPool = new Dictionary<long, frm_chat>();

        public static Dictionary<long, ChatListSubItem> chatListSubItemPool = new Dictionary<long, ChatListSubItem>();
        public static HashSet<long> onlineUserId = new HashSet<long>();
        public static HashSet<long> offlineUserId = new HashSet<long>();

        public static Dictionary<long, ChatListSubItem> unReadMsgItemPool = new Dictionary<long, ChatListSubItem>();


        public enum AddFriendStatus
        {
            Pending = 0,
            Rejected = 1,
            Accepted = 2
        }

        public enum ContentType
        {
            Text = 0,
            Object = 1,
            Vibration = 2
        }

        public enum UnReadMsgType
        {
            Message = 0,
            AddFriend = 1,
            ReplyAddFriend = 2
        }

        delegate void MenuSetCheckCallBack(ToolStripMenuItem mnu, bool value);
        delegate void ToolStripStatusLabelCallBack(ToolStripStatusLabel obj, string value);
        public static void InvokeToolStripStatusLabel(ToolStripStatusLabel obj, string value)
        {
            if (frm_main.InvokeRequired)
            {
                ToolStripStatusLabelCallBack cb = new ToolStripStatusLabelCallBack(InvokeToolStripStatusLabel);
                frm_main.Invoke(cb, new object[] { obj, value });
            }
            else
                obj.Text = value;
        }
        public static void InvokeMenuSetCheck(ToolStripMenuItem mnu, bool value)
        {
            if (frm_main.menuStrip1.InvokeRequired)
            {
                MenuSetCheckCallBack cb = new MenuSetCheckCallBack(InvokeMenuSetCheck);
                frm_main.Invoke(cb, new object[] { mnu, value });
            }
            else
                mnu.Checked = value;
        }


        public static void GridAdd(DataGridView dgv, Dictionary<string, object> dict)
        {
            var idx = dgv.Rows.Add(new DataGridViewRow());
            foreach (var item in dict)
                dgv.Rows[idx].Cells[item.Key].Value = item.Value;
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

        public static string GenerateMD5WithFilePath(string filePath)
        {
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash_byte = md5.ComputeHash(file);
            string str = System.BitConverter.ToString(hash_byte);
            str = str.Replace("-", "");
            file.Close();
            return str;
        }

        public static DateTime StampToDateTime(long timestamp)
        {
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            return dateTime.AddSeconds(timestamp).ToLocalTime();
        }
        public static long DateTimeToStamp(DateTime dataTime)
        {
            return (long)(dataTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }
        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static void InsertObject(long id,string eTag,string name)
        {
            DataTable dt = Class1.sql.SqlTable($"SELECT * FROM `object` WHERE id = {id}");
            if (dt.Rows.Count != 0)
                return;
            bool ret = Class1.sql.ExecuteNonQuery($"INSERT INTO `object`(id, e_tag, name) VALUES({id},'{eTag}', '{name}')");
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO object失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        public static string DoHttpRequest(string url, string method, Dictionary<string,string> header, string data = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Class1.Ip + Class1.HttpPort + url);
            request.Method = method;
            request.ContentType = "application/json;charset=utf-8";
            if (header != null)
                foreach (var item in header)
                    request.Headers.Add(item.Key, item.Value);
            if (!string.IsNullOrEmpty(data))
            {
                Stream RequestStream = request.GetRequestStream();
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                RequestStream.Write(bytes, 0, bytes.Length);
                RequestStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream ResponseStream = response.GetResponseStream();
            StreamReader StreamReader = new StreamReader(ResponseStream, Encoding.GetEncoding("utf-8"));
            string re = StreamReader.ReadToEnd();
            StreamReader.Close();
            ResponseStream.Close();
            var status = GetStatus(re);
            var statusCode = status.Item1;
            var statusMessage = status.Item2;
            switch(statusCode)
            {
                case 254:
                    {
                        redis.UnSubscribe("poseidon_heart_beat_channel");
                        redis.UnSubscribe("poseidon_message_channel_" + UserId);
                        IsOnline = false;
                        AccessToken = "";
                        UpdateStatusCheckBox(false);
                        InvokeToolStripStatusLabel(frm_main.toolStripStatusLabel1, "离线");
                        foreach (ChatListSubItem item in frm_main.ChatListBox.Items[0].SubItems)
                            frm_main.Invoke(new Action(() => {
                                frm_main.ChatListBox.Items[1].SubItems.Add(item);
                                frm_main.ChatListBox.Items[0].SubItems.Remove(item);
                            }));
                        MessageBox.Show(statusMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                case 255:
                    {
                        MessageBox.Show("server error, message: " + statusMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception("server error, message: " + statusMessage);
                    }
            }
            return re;
        }

        public static Tuple<int,string> GetStatus(string jsonString)
        {
            Console.WriteLine(jsonString);
            var obj = JsonConvert.DeserializeObject<dynamic>(jsonString);
            return new Tuple<int,string>((int)obj.StatusCode, (string)obj.StatusMessage);
        }

        public static void UpdateStatusCheckBox(bool isOnline)
        {
            InvokeMenuSetCheck(frm_main.mnu_online, isOnline);
            InvokeMenuSetCheck(frm_main.mnu_offline, !isOnline);
            //frm_main.mnu_online.Checked = isOnline;
            //frm_main.mnu_offline.Checked = !isOnline;
        }

        public static bool Login(long userId, string password)
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
                sql.CreateDBFile(sqlFile);
                sql.Connection(sqlFile);
                var ret = sql.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS `message` (
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

                ret = sql.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS `user_relation_request` (
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

                ret = sql.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS `object` (
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

                UserId = userId;
                Password = password;
                AccessToken = resp.AccessToken;
                IsOnline = true;
                return true;
            }
            return false;
        }
        public static void Logout(long userId, string accessToken)
        {
            var req = new http._Login.LogoutReq()
            {
                UserId = userId,
                AccessToken = accessToken
            };

            http._Login.Logout(req);
            redis.UnSubscribe("poseidon_heart_beat_channel");
            redis.UnSubscribe("poseidon_message_channel_" + UserId);
            IsOnline = false;
            InvokeToolStripStatusLabel(frm_main.toolStripStatusLabel1, "离线");
            AccessToken = "";
            foreach(ChatListSubItem item in frm_main.ChatListBox.Items[0].SubItems)
                frm_main.Invoke(new Action(() => {
                    frm_main.ChatListBox.Items[1].SubItems.Add(item);
                    frm_main.ChatListBox.Items[0].SubItems.Remove(item);
                }));
            
        }

        public static void appendRtfToMsgBox(frm_chat frmChat,string name, DateTime time, string content)
        {
            if (!frmChat.IsHandleCreated)
            {
                frmChat.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                     new Font(frmChat.Font, FontStyle.Regular), Color.Green);
                //frmChat.rtxt_message.AppendText("   ");
                frmChat.rtxt_message.AppendRtf(content);

            } else
            {

                frmChat.Invoke(new Action(() => {
                    frmChat.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                         new Font(frmChat.Font, FontStyle.Regular), Color.Green);
                    //frmChat.rtxt_message.AppendText("   ");
                    frmChat.rtxt_message.AppendRtf(content);
                }));
            }

        }
        public static void appendFileToMsgBox(frm_chat frmChat, string name, DateTime time, string fileName, long objId)
        {
            if (!frmChat.IsHandleCreated)
            {
                frmChat.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                     new Font(frmChat.Font, FontStyle.Regular), Color.Green);
                //frmChat.rtxt_message.AppendText("   ");
                frmChat.rtxt_message.AppendText(fileName);
                frmChat.rtxt_message.AppendText("\n");
                frmChat.rtxt_message.InsertLink("下载文件[" + objId + "]");
                frmChat.rtxt_message.AppendText("\n");
            }
            else
            {
                frmChat.Invoke(new Action(() => {
                    frmChat.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                         new Font(frmChat.Font, FontStyle.Regular), Color.Green);
                    //frmChat.rtxt_message.AppendText("   ");
                    frmChat.rtxt_message.AppendText(fileName);
                    frmChat.rtxt_message.AppendText("\n");
                    frmChat.rtxt_message.InsertLink("下载文件[" + objId + "]");
                    frmChat.rtxt_message.AppendText("\n");
                }));
            }
        }
        public static void appendVibrationToMsgBox(frm_chat frmChat, string name, DateTime time)
        {
            string str;
            if (long.Parse(name) == Class1.UserId)
                str = "发送";
            else
                str = "收到";
            if (!frmChat.IsHandleCreated)
            {
                frmChat.rtxt_message.AppendRichText(Class1.UserId + "  " + time.ToLongTimeString() + "\r\n",
                new Font(frmChat.Font, FontStyle.Regular), Color.Green);
                //rtxt_message.SelectionColor = Color.Red;
                frmChat.rtxt_message.AppendText("您" + str + "了一个窗口抖动。\r\n");
                frmChat.rtxt_message.ForeColor = Color.Black;
            }
            else
            {
                frmChat.Invoke(new Action(() => {
                    frmChat.rtxt_message.AppendRichText(Class1.UserId + "  " + time.ToLongTimeString() + "\r\n",
                new Font(frmChat.Font, FontStyle.Regular), Color.Green);
                    //rtxt_message.SelectionColor = Color.Red;
                    frmChat.rtxt_message.AppendText("您" + str + "了一个窗口抖动。\r\n");
                    frmChat.rtxt_message.ForeColor = Color.Black;
                }));
            }
        }
        public static void Vibration(frm_chat frmChat)
        {
            frmChat.Invoke(new Action(() => {
                Point pOld = frmChat.Location;//原来的位置
                int radius = 3;//半径
                for (int n = 0; n < 3; n++) //旋转圈数
                {
                    //右半圆逆时针
                    for (int i = -radius; i <= radius; i++)
                    {
                        int x = Convert.ToInt32(Math.Sqrt(radius * radius - i * i));
                        int y = -i;

                        frmChat.Location = new Point(pOld.X + x, pOld.Y + y);
                        Thread.Sleep(10);
                    }
                    //左半圆逆时针
                    for (int j = radius; j >= -radius; j--)
                    {
                        int x = -Convert.ToInt32(Math.Sqrt(radius * radius - j * j));
                        int y = -j;

                        frmChat.Location = new Point(pOld.X + x, pOld.Y + y);
                        Thread.Sleep(10);
                    }
                }
                //抖动完成，恢复原来位置
                frmChat.Location = pOld;
            }));
        }
        public static void appendPersonalMsgToUnReadBox(long id, long userId, string content)
        {
            ChatListSubItem subItem;
            if(unReadMsgItemPool.ContainsKey(userId))
                subItem = unReadMsgItemPool[userId];
            else
            {
                subItem = new ChatListSubItem("");
                subItem.DisplayName = userId.ToString();
                subItem.Tag = new Dictionary<string, object>{ { "type", (long)UnReadMsgType.Message },{"user_id_send",userId },{ "ids",new List<long>()} };
                unReadMsgItemPool.Add(userId, subItem);
                frm_main.clb_unread_msg.Items[0].SubItems.Add(subItem);
            }
            subItem.ID++;
            subItem.NicName = subItem.ID + "条未读消息";
            subItem.PersonalMsg = content;
            ((List<long>)(((Dictionary<string, object>)subItem.Tag)["ids"])).Add(id);
        }
        public static void appendSystemMsgToUnReadBox(long id, long userId, string content, UnReadMsgType msgType)
        {
            var subItem = new ChatListSubItem(content);
            subItem.Tag = new Dictionary<string, object> { { "type", (long)msgType }, { "user_id_send", userId }, { "id", id } };
            frm_main.clb_unread_msg.Items[1].SubItems.Add(subItem);
        }
        public static string rtfToText(string rtfContent)
        {
            RichTextBox rtb = new RichTextBox();
            rtb.Rtf = rtfContent;
            return rtb.Text;
        }
    }
}
