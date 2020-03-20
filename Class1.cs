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
using System.IO.Compression;
using Aliyun.OSS.Common;
using Aliyun.OSS;

namespace Poseidon
{
    public static class Class1
    {
        public const string Ip = "192.168.6.128";

        public const string HttpPort = ":8081";

        public const string EndPoint = "oss-cn-shenzhen.aliyuncs.com";
        public const string BucketName = "poseidon-data";

        public const int PageSize = 20;

        public static long UserId;
        public static string Password;

        public static string AccessToken;

        public static bool IsOnline;

        public static frm_main frm_main;

        public static frm_msg_box frmMsgBox;

        public static SQLiteDB sql = new SQLiteDB();

        public static Dictionary<long, frm_chat> formChatPool = new Dictionary<long, frm_chat>();
        public static Dictionary<long, frm_group> formGroupPool = new Dictionary<long, frm_group>();

        public static Dictionary<long, ChatListSubItem> chatListSubItemPool = new Dictionary<long, ChatListSubItem>();
        public static HashSet<long> onlineUserId = new HashSet<long>();
        public static HashSet<long> offlineUserId = new HashSet<long>();

        public static Dictionary<long, ChatListSubItem> unReadPrivateMsgItemPool = new Dictionary<long, ChatListSubItem>();
        public static Dictionary<long, ChatListSubItem> unReadGroupMsgItemPool = new Dictionary<long, ChatListSubItem>();
        

        public static Dictionary<long, ChatListSubItem> groupItemPool = new Dictionary<long, ChatListSubItem>();
        


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
            Vibration = 2,
            Image = 3
        }

        public enum MsgType
        {
            PrivateChat = 0,
            GroupChat = 1
        }

        public enum UnReadMsgType
        {
            Message = 0,
            AddFriend = 1,
            ReplyAddFriend = 2,
            AddGroup = 3,
            ReplyAddGroup = 4,
            InviteAddGroup = 5,
            ReplyInviteAddGroup = 6,
        }

        public enum GroupUserRequestType
        {
            AddGroup = 0,
            InviteAddGroup = 1
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

        public static void InsertObject(long id, string eTag, string name)
        {
            DataTable dt = sql.SqlTable($"SELECT * FROM `object` WHERE id = {id}");
            if (dt != null && dt.Rows.Count != 0)
                return;
            bool ret = sql.ExecuteNonQuery($"INSERT INTO `object`(id, e_tag, name) VALUES({id},'{eTag}', '{name}')");
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO object失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        public static void UploadImage(string localFileName, long idRecv, frm_chat frmChat)
        {
            var eTag = Class1.GenerateMD5WithFilePath(localFileName);
            var req = new http._Oss.GetSTSInfoReq()
            {
                UserId = Class1.UserId
            };
            var resp = http._Oss.GetSTSInfo(req);

            Thread t = new Thread(new ThreadStart(() =>
            {
                // 拿到STS临时凭证后，通过其中的安全令牌（SecurityToken）和临时访问密钥（AccessKeyId和AccessKeySecret）生成OSSClient。
                var client = new OssClient(Class1.EndPoint, resp.AccessKeyId, resp.AccessKeySecret, resp.SecurityToken);
                if (!client.DoesObjectExist(Class1.BucketName, eTag))
                {
                    try
                    {
                        using (var fs = File.Open(localFileName, FileMode.Open))
                        {
                            var putObjectRequest = new PutObjectRequest(Class1.BucketName, eTag, fs);
                            //putObjectRequest.StreamTransferProgress += UploadProgressCallback;
                            client.PutObject(putObjectRequest);
                        }
                        Console.WriteLine("Put object:{0} succeeded", eTag);
                    }
                    catch (OssException ex)
                    {
                        Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID: {2}\tHostID: {3}",
                            ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed with error info: {0}", ex.Message);
                    }
                }

                var name = Path.GetFileName(localFileName);

                var sendMessageReq = new http._Message.SendMessageReq()
                {
                    UserIdSend = Class1.UserId,
                    IdRecv = idRecv,
                    Content = eTag,
                    ContentType = (int)Class1.ContentType.Image,
                    MessageType = 0
                };
                var sendMessageResp = http._Message.SendMessage(sendMessageReq);
                var statusCode = sendMessageResp.StatusCode;

                switch (statusCode)
                {
                    case 1:
                        {
                            Class1.appendSysMsgToMsgBox(frmChat, "你与" + idRecv + "未成为好友，无法发送消息", DateTime.Now);
                            return;
                        }
                }


                var messageId = sendMessageResp.Id;
                var createTime = sendMessageResp.CreateTime;
                var param = Class1.Gzip(System.Text.Encoding.Default.GetBytes(eTag));
                var ret = Class1.sql.ExecuteNonQueryWithBinary($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type, is_read) VALUES({messageId}, " +
                            $"{Class1.UserId}, {idRecv}, 0, @param, {createTime}, {(int)Class1.ContentType.Image}, 0, 1)", param);
                if (!ret)
                {
                    MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var imageData = Class1.LoadFile(localFileName);
                var imageParam = Class1.Gzip(imageData);
                Class1.InsertImage(eTag, imageParam);
                Class1.appendImageToMsgBox(frmChat, Class1.UserId.ToString(), Class1.StampToDateTime(createTime), imageData);
            }));
            t.Start();
        }
        public static byte[] FetchImage(string eTag)
        {
            //先从本地加载，没有再从云端下载
            DataTable dt = Class1.sql.SqlTable($"SELECT content FROM `image` WHERE `e_tag` = \"{eTag}\"");
            if (dt != null && dt.Rows.Count != 0)
                return Class1.UnGzip((byte[])dt.Rows[0]["content"]);
            else
            {
                byte[] imageData = new byte[0];
                var req = new http._Oss.GetSTSInfoReq()
                {
                    UserId = Class1.UserId
                };
                var resp = http._Oss.GetSTSInfo(req);
                // 拿到STS临时凭证后，通过其中的安全令牌（SecurityToken）和临时访问密钥（AccessKeyId和AccessKeySecret）生成OSSClient。
                var client = new OssClient(Class1.EndPoint, resp.AccessKeyId, resp.AccessKeySecret, resp.SecurityToken);
                try
                {
                    var getObjectRequest = new GetObjectRequest(Class1.BucketName, eTag);
                    //getObjectRequest.StreamTransferProgress += DownloadProgressCallback;
                    // 下载文件。
                    var ossObject = client.GetObject(getObjectRequest);
                    using (var stream = ossObject.Content)
                    {
                        var buffer = new byte[1024 * 1024];
                        var bytesRead = 0;
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                            imageData = imageData.Concat(buffer.Take(bytesRead)).ToArray();
                    }
                    Console.WriteLine("Get object:{0} succeeded", eTag);
                    InsertImage(eTag, Gzip(imageData)); //持久化
                    return imageData;
                }
                catch (OssException ex)
                {
                    Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed with error info: {0}", ex.Message);
                    return null;
                }
            }

        }
        public static void InsertImage(string eTag, byte[] imageParam)
        {
            DataTable dt = sql.SqlTable($"SELECT * FROM `image` WHERE e_tag = \"{eTag}\"");
            if (dt != null && dt.Rows.Count != 0)
                return;
            bool ret = sql.ExecuteNonQueryWithBinary($"INSERT INTO `image`(e_tag, content) VALUES(\"{eTag}\", @param)", imageParam);
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO image666", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        public static string DoHttpRequest(string url, string method, Dictionary<string, string> header, string data = null)
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
            switch (statusCode)
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
                            frm_main.Invoke(new Action(() =>
                            {
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

        public static Tuple<int, string> GetStatus(string jsonString)
        {
            Console.WriteLine(jsonString);
            var obj = JsonConvert.DeserializeObject<dynamic>(jsonString);
            return new Tuple<int, string>((int)obj.StatusCode, (string)obj.StatusMessage);
        }

        public static void UpdateStatusCheckBox(bool isOnline)
        {
            InvokeMenuSetCheck(frm_main.mnu_online, isOnline);
            InvokeMenuSetCheck(frm_main.mnu_offline, !isOnline);
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
                  `content` BLOB NOT NULL,
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

                ret = sql.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS `image` (
                  `e_tag` TEXT NOT NULL,
                  `content` BLOB NOT NULL,
                  PRIMARY KEY (`e_tag`)
                ); ");
                if (!ret)
                {
                    MessageBox.Show("DB错误，建表失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                ret = sql.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS `group_user_request` (
                  `id` INTEGER NOT NULL,
                  `user_id_send` INTEGER NOT NULL,
                  `user_id_recv` INTEGER NOT NULL,
                  `group_id` INTEGER NOT NULL,
                  `create_time` INTEGER NOT NULL,
                  `status` INTEGER NOT NULL,
                  `parent_id` INTEGER NOT NULL,
                  `type` INTEGER NOT NULL,
                  PRIMARY KEY (`id`)
                ); ");
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
            foreach (ChatListSubItem item in frm_main.ChatListBox.Items[0].SubItems)
                frm_main.Invoke(new Action(() =>
                {
                    frm_main.ChatListBox.Items[1].SubItems.Add(item);
                    frm_main.ChatListBox.Items[0].SubItems.Remove(item);
                }));

        }

        public static void appendRtfToMsgBox(frm_chat frmChat, string name, DateTime time, string content)
        {
            if (!frmChat.IsHandleCreated)
            {
                frmChat.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                     new Font(frmChat.Font, FontStyle.Regular), Color.Green);
                frmChat.rtxt_message.AppendRtf(content);
                frmChat.rtxt_message.Select(frmChat.rtxt_message.Text.Length, 0);
                frmChat.rtxt_message.ScrollToCaret();
                frmChat.rtxt_send.Focus();
            }
            else
            {

                frmChat.Invoke(new Action(() =>
                {
                    frmChat.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                         new Font(frmChat.Font, FontStyle.Regular), Color.Green);
                    frmChat.rtxt_message.AppendRtf(content);
                    frmChat.rtxt_message.Select(frmChat.rtxt_message.Text.Length, 0);
                    frmChat.rtxt_message.ScrollToCaret();
                    frmChat.rtxt_send.Focus();
                }));
            }
        }
        public static void appendFileToMsgBox(frm_chat frmChat, string name, DateTime time, string fileName, long objId)
        {
            if (!frmChat.IsHandleCreated)
            {
                frmChat.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                     new Font(frmChat.Font, FontStyle.Regular), Color.Green);
                frmChat.rtxt_message.AppendText(fileName);
                frmChat.rtxt_message.AppendText("\n");
                frmChat.rtxt_message.InsertLink("下载文件[" + objId + "]");
                frmChat.rtxt_message.AppendText("\n");
                frmChat.rtxt_message.Select(frmChat.rtxt_message.Text.Length, 0);
                frmChat.rtxt_message.ScrollToCaret();
                frmChat.rtxt_send.Focus();
            }
            else
            {
                frmChat.Invoke(new Action(() =>
                {
                    frmChat.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                         new Font(frmChat.Font, FontStyle.Regular), Color.Green);
                    frmChat.rtxt_message.AppendText(fileName);
                    frmChat.rtxt_message.AppendText("\n");
                    frmChat.rtxt_message.InsertLink("下载文件[" + objId + "]");
                    frmChat.rtxt_message.AppendText("\n");
                    frmChat.rtxt_message.Select(frmChat.rtxt_message.Text.Length, 0);
                    frmChat.rtxt_message.ScrollToCaret();
                    frmChat.rtxt_send.Focus();
                }));
            }
        }
        public static void appendSysMsgToMsgBox(frm_chat frmChat, string content, DateTime time)
        {
            if (!frmChat.IsHandleCreated)
            {
                frmChat.rtxt_message.AppendRichText("[系统消息]" + "  " + time.ToLongTimeString() + "\r\n",
                new Font(frmChat.Font, FontStyle.Regular), Color.Blue);
                frmChat.rtxt_message.AppendText(content + "\r\n");
                frmChat.rtxt_message.ForeColor = Color.Black;
                frmChat.rtxt_message.Select(frmChat.rtxt_message.Text.Length, 0);
                frmChat.rtxt_message.ScrollToCaret();
                frmChat.rtxt_send.Focus();
            }
            else
            {
                frmChat.Invoke(new Action(() =>
                {
                    frmChat.rtxt_message.AppendRichText("[系统消息]" + "  " + time.ToLongTimeString() + "\r\n",
                new Font(frmChat.Font, FontStyle.Regular), Color.Blue);
                    frmChat.rtxt_message.AppendText(content + "\r\n");
                    frmChat.rtxt_message.ForeColor = Color.Black;
                    frmChat.rtxt_message.Select(frmChat.rtxt_message.Text.Length, 0);
                    frmChat.rtxt_message.ScrollToCaret();
                    frmChat.rtxt_send.Focus();
                }));
            }
        }
        public static Image appendImageToMsgBox(frm_chat frmChat, string name, DateTime time, byte[] imageData)
        {
            var image = Image.FromStream(new MemoryStream(imageData));
            if (!frmChat.IsHandleCreated)
            {
                frmChat.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                     new Font(frmChat.Font, FontStyle.Regular), Color.Green);
                frmChat.rtxt_message.InsertImage(image);
                frmChat.rtxt_message.AppendText("\n");
                frmChat.rtxt_message.Select(frmChat.rtxt_message.Text.Length, 0);
                frmChat.rtxt_message.ScrollToCaret();
                frmChat.rtxt_send.Focus();
            }
            else
            {
                frmChat.Invoke(new Action(() =>
                {
                    frmChat.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                        new Font(frmChat.Font, FontStyle.Regular), Color.Green);
                    frmChat.rtxt_message.InsertImage(image);
                    frmChat.rtxt_message.AppendText("\n");
                    frmChat.rtxt_message.Select(frmChat.rtxt_message.Text.Length, 0);
                    frmChat.rtxt_message.ScrollToCaret();
                    frmChat.rtxt_send.Focus();
                }));
            }
            return image;
        }
        public static void appendCenteralText(frm_chat frmChat, string msg)
        {
            var len = frmChat.rtxt_message.TextLength;
            frmChat.rtxt_message.AppendRichText(msg, new Font(frmChat.Font, FontStyle.Regular), Color.Red);
            frmChat.rtxt_message.Select(len, 0);
            frmChat.rtxt_message.SelectionAlignment = HorizontalAlignment.Center;
            frmChat.rtxt_message.AppendText("\n");
            frmChat.rtxt_message.SelectionAlignment = HorizontalAlignment.Left;
        }
        public static void Vibration(frm_chat frmChat)
        {
            frmChat.Invoke(new Action(() =>
            {
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
            if (unReadPrivateMsgItemPool.ContainsKey(userId))
                subItem = unReadPrivateMsgItemPool[userId];
            else
            {
                subItem = new ChatListSubItem("");
                subItem.DisplayName = userId.ToString();
                subItem.Tag = new Dictionary<string, object> { { "type", (long)UnReadMsgType.Message }, { "user_id_send", userId }, { "msg_type", (int)Class1.MsgType.PrivateChat },{ "ids", new List<long>() } };
                unReadPrivateMsgItemPool.Add(userId, subItem);
                frmMsgBox.clb_unread_msg.Items[0].SubItems.Add(subItem);
            }
            subItem.ID++;
            subItem.NicName = subItem.ID + "条未读消息";
            subItem.PersonalMsg = content;
            ((List<long>)(((Dictionary<string, object>)subItem.Tag)["ids"])).Add(id);
        }
        public static void appendSystemMsgToUnReadBox(long id, long userId, string content, UnReadMsgType msgType, Dictionary<string,object> extra)
        {
            var subItem = new ChatListSubItem(content);
            subItem.Tag = new Dictionary<string, object> { { "type", (long)msgType }, { "user_id_send", userId }, { "id", id } };
            foreach (var item in extra)
                ((Dictionary<string, object>)subItem.Tag).Add(item.Key, item.Value);
            frmMsgBox.clb_unread_msg.Items[1].SubItems.Add(subItem);
        }
        public static void appendGroupMsgToUnReadBox(long id,long userIdSend, long groupId, string content)
        {
            ChatListSubItem subItem;
            if (unReadGroupMsgItemPool.ContainsKey(groupId))
                subItem = unReadGroupMsgItemPool[groupId];
            else
            {
                subItem = new ChatListSubItem("");
                subItem.DisplayName = groupId.ToString();
                subItem.Tag = new Dictionary<string, object> { { "type", (long)UnReadMsgType.Message }, { "group_id", groupId }, { "msg_type", (int)Class1.MsgType.GroupChat} ,{ "max_id", 0 } };
                unReadGroupMsgItemPool.Add(groupId, subItem);
                frmMsgBox.clb_unread_msg.Items[2].SubItems.Add(subItem);
            }
            subItem.ID++;
            subItem.NicName = subItem.ID + "条未读消息";
            subItem.PersonalMsg = content;
            var maxId = long.Parse(((Dictionary<string, object>)subItem.Tag)["max_id"].ToString());
            ((Dictionary<string, object>)subItem.Tag)["max_id"] = Math.Max(maxId, id); ;
        }
        public static string rtfToText(string rtfContent)
        {
            RichTextBox rtb = new RichTextBox();
            rtb.Rtf = rtfContent;
            return rtb.Text;
        }

        public static void LoadUnReadMessage()
        {
            foreach (ChatListItem item in Class1.frmMsgBox.clb_unread_msg.Items)
                item.SubItems.Clear();
            Class1.unReadPrivateMsgItemPool.Clear();
            Class1.unReadGroupMsgItemPool.Clear();

            var readMessage = new Dictionary<long, int>();

            //private_chat
            DataTable dt = Class1.sql.SqlTable($"SELECT id, user_id_send, content, create_time, content_type FROM `message` WHERE `user_id_recv` = {Class1.UserId} AND `is_read` = 0");
            if(dt != null && dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var id = long.Parse(row["id"].ToString());
                    var userIdSend = long.Parse(row["user_id_send"].ToString());
                    var content = System.Text.Encoding.Default.GetString(Class1.UnGzip((byte[])row["content"]));
                    var createTime = Class1.FormatDateTime(Class1.StampToDateTime(long.Parse(row["create_time"].ToString())));
                    var contentType = long.Parse(row["content_type"].ToString());

                    if (Class1.formChatPool.ContainsKey(userIdSend))
                    {
                        var frm_chat = Class1.formChatPool[userIdSend];
                        switch (contentType)
                        {
                            case (int)Class1.ContentType.Text:
                                {
                                    Class1.appendRtfToMsgBox(frm_chat, userIdSend.ToString(), Class1.StampToDateTime(long.Parse(row["create_time"].ToString())), content);
                                    break;
                                }
                            case (int)Class1.ContentType.Object:
                                {
                                    var objId = long.Parse(content);
                                    DataTable dt1 = Class1.sql.SqlTable($"SELECT name FROM `object` WHERE `id` = {objId}");
                                    if (dt1 == null || dt1.Rows.Count != 1)
                                        throw new Exception("object not found, objId: " + objId);
                                    var objName = dt1.Rows[0]["name"].ToString();
                                    Class1.appendFileToMsgBox(frm_chat, userIdSend.ToString(), Class1.StampToDateTime(long.Parse(row["create_time"].ToString())), "[文件]" + objName, objId);
                                    break;
                                }
                            case (int)Class1.ContentType.Vibration:
                                {
                                    Class1.appendSysMsgToMsgBox(frm_chat, "你" + (userIdSend == UserId ? "发送" : "收到") + "了一个窗口抖动。\r\n", Class1.StampToDateTime(long.Parse(row["create_time"].ToString())));
                                    Class1.Vibration(frm_chat);
                                    break;
                                }
                            case (int)Class1.ContentType.Image:
                                {
                                    var eTag = content;
                                    DataTable dt1 = sql.SqlTable($"SELECT content FROM `image` WHERE `e_tag` = \"{eTag}\"");
                                    if (dt1 == null || dt1.Rows.Count != 1)
                                        throw new Exception("image not found, e_tag: " + eTag);
                                    var imageData = UnGzip((byte[])dt1.Rows[0]["content"]);
                                    Class1.appendImageToMsgBox(frm_chat, userIdSend.ToString(), Class1.StampToDateTime(long.Parse(row["create_time"].ToString())), imageData);
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("unknown content_type content_type = ", contentType);
                                    break;
                                }
                        }
                        readMessage.Add(id, 1);
                    }
                    else
                    {
                        switch (contentType)
                        {
                            case (int)ContentType.Text:
                                {
                                    appendPersonalMsgToUnReadBox(id, userIdSend, Class1.rtfToText(content));
                                    break;
                                }
                            case (int)ContentType.Object:
                                {
                                    var objId = long.Parse(content);
                                    DataTable dt1 = sql.SqlTable($"SELECT name FROM `object` WHERE `id` = {objId}");
                                    if (dt1 == null || dt1.Rows.Count != 1)
                                    {
                                        Console.WriteLine("rowCount != 1");
                                        return;
                                    }
                                    appendPersonalMsgToUnReadBox(id, userIdSend, "[文件]" + dt1.Rows[0]["name"].ToString());
                                    break;
                                }
                            case (int)ContentType.Vibration:
                                {
                                    appendPersonalMsgToUnReadBox(id, userIdSend, "你收到了一个窗口抖动");
                                    break;
                                }
                            case (int)ContentType.Image:
                                {
                                    appendPersonalMsgToUnReadBox(id, userIdSend, "[图片]");
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("unknown content_type " + contentType);
                                    break;
                                }
                        }
                    }
                }
            }

            //group_chat
            var req = new http._Group_User.GetLastReadMsgIdReq()
            {
                UserId = Class1.UserId
            };
            var resp = http._Group_User.GetLastReadMsgId(req);
            var lastReadMsgId = resp.LastReadMsgId;

            var updateLastReadMsgIdReq = new http._Group_User.UpdateLastReadMsgIdReq()
            {
                UserId = Class1.UserId,
                LastReadMsgId = new Dictionary<long, long>()
            };

            foreach(var item in lastReadMsgId)
            {
                var groupId = item.Key;
                var msgId = item.Value;


                dt = Class1.sql.SqlTable($"SELECT id, user_id_send, content, create_time, content_type FROM `message` WHERE `id` > {msgId} AND `group_id` = {groupId} AND `msg_type` = {(int)Class1.MsgType.GroupChat}");
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var id = long.Parse(row["id"].ToString());
                        var userIdSend = long.Parse(row["user_id_send"].ToString());
                        var content = System.Text.Encoding.Default.GetString(Class1.UnGzip((byte[])row["content"]));
                        var createTime = Class1.FormatDateTime(Class1.StampToDateTime(long.Parse(row["create_time"].ToString())));
                        var contentType = long.Parse(row["content_type"].ToString());

                        if (Class1.formGroupPool.ContainsKey(groupId))
                        {
                            var frmGroup = Class1.formGroupPool[groupId];
                            switch (contentType)
                            {
                                case (int)Class1.ContentType.Text:
                                    {
                                        cls_group.appendRtfToMsgBox(frmGroup, userIdSend.ToString(), Class1.StampToDateTime(long.Parse(row["create_time"].ToString())), content);
                                        break;
                                    }
                                case (int)Class1.ContentType.Object:
                                    {
                                        var objId = long.Parse(content);
                                        DataTable dt1 = Class1.sql.SqlTable($"SELECT name FROM `object` WHERE `id` = {objId}");
                                        if (dt1 == null || dt1.Rows.Count != 1)
                                            throw new Exception("object not found, objId: " + objId);
                                        var objName = dt1.Rows[0]["name"].ToString();
                                        cls_group.appendFileToMsgBox(frmGroup, userIdSend.ToString(), Class1.StampToDateTime(long.Parse(row["create_time"].ToString())), "[文件]" + objName, objId);
                                        break;
                                    }
                                case (int)Class1.ContentType.Image:
                                    {
                                        var eTag = content;
                                        DataTable dt1 = sql.SqlTable($"SELECT content FROM `image` WHERE `e_tag` = \"{eTag}\"");
                                        if (dt1 == null || dt1.Rows.Count != 1)
                                            throw new Exception("image not found, e_tag: " + eTag);
                                        var imageData = UnGzip((byte[])dt1.Rows[0]["content"]);
                                        cls_group.appendImageToMsgBox(frmGroup, userIdSend.ToString(), Class1.StampToDateTime(long.Parse(row["create_time"].ToString())), imageData);
                                        break;
                                    }
                                default:
                                        throw new Exception("unknown content_type content_type = " + contentType);
                            }
                            if (updateLastReadMsgIdReq.LastReadMsgId.ContainsKey(groupId))
                                updateLastReadMsgIdReq.LastReadMsgId[groupId] = Math.Max(updateLastReadMsgIdReq.LastReadMsgId[groupId], id);
                            else
                                updateLastReadMsgIdReq.LastReadMsgId.Add(groupId, id);

                        }
                        else
                        {
                            switch (contentType)
                            {
                                case (int)ContentType.Text:
                                    {
                                        appendGroupMsgToUnReadBox(id, userIdSend, groupId, Class1.rtfToText(content));
                                        break;
                                    }
                                case (int)ContentType.Object:
                                    {
                                        var objId = long.Parse(content);
                                        DataTable dt1 = sql.SqlTable($"SELECT name FROM `object` WHERE `id` = {objId}");
                                        if (dt1 == null || dt1.Rows.Count != 1)
                                        {
                                            Console.WriteLine("rowCount != 1");
                                            return;
                                        }
                                        appendGroupMsgToUnReadBox(id, userIdSend, groupId, "[文件]" + dt1.Rows[0]["name"].ToString());
                                        break;
                                    }
                                case (int)ContentType.Image:
                                    {
                                        appendGroupMsgToUnReadBox(id, userIdSend, groupId, "[图片]");
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("unknown content_type " + contentType);
                                        break;
                                    }
                            }
                        }
                    }
                }
            }


            //进入聊天框的消息已读
            UpdateMessageStatus(readMessage, new Dictionary<long, int>(), new Dictionary<long, int>());
            http._Group_User.UpdateLastReadMsgId(updateLastReadMsgIdReq);

            dt = Class1.sql.SqlTable($"SELECT id, user_id_send, create_time, parent_id FROM `user_relation_request` WHERE `user_id_recv` = {Class1.UserId} AND `status` = 0");
            foreach (DataRow row in dt.Rows)
            {
                var id = long.Parse(row["id"].ToString());
                var userIdSend = long.Parse(row["user_id_send"].ToString());
                var createTime = Class1.FormatDateTime(Class1.StampToDateTime(long.Parse(row["create_time"].ToString())));
                var parentId = long.Parse(row["parent_id"].ToString());
                if (parentId == -1)
                    Class1.appendSystemMsgToUnReadBox(id, userIdSend, "来自" + userIdSend + "的好友请求", Class1.UnReadMsgType.AddFriend, new Dictionary<string, object>());
                else
                {
                    var dt1 = Class1.sql.SqlTable($"SELECT status FROM `user_relation_request` WHERE `id` = {parentId}");
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                        Console.WriteLine("parentId 不存在");
                        return;
                    }
                    var status = long.Parse(dt1.Rows[0]["status"].ToString());
                    string statusString;
                    if (status == (long)Class1.AddFriendStatus.Accepted)
                        statusString = "请求通过";
                    else if (status == (long)Class1.AddFriendStatus.Rejected)
                        statusString = "请求被拒绝";
                    else
                    {
                        Console.WriteLine("无效的status, status = " + status);
                        return;
                    }
                    Class1.appendSystemMsgToUnReadBox(id, userIdSend, userIdSend + (status == (long)Class1.AddFriendStatus.Accepted ? "通过" : "拒绝") + "了好友请求", Class1.UnReadMsgType.ReplyAddFriend, new Dictionary<string, object>());
                }
            }

            dt = Class1.sql.SqlTable($"SELECT id, user_id_send, create_time, parent_id, group_id FROM `group_user_request` WHERE `user_id_send` != {Class1.UserId} AND `status` = 0");
            foreach (DataRow row in dt.Rows)
            {
                var id = long.Parse(row["id"].ToString());
                var userIdSend = long.Parse(row["user_id_send"].ToString());
                var createTime = Class1.FormatDateTime(Class1.StampToDateTime(long.Parse(row["create_time"].ToString())));
                var parentId = long.Parse(row["parent_id"].ToString());
                var groupId = long.Parse(row["group_id"].ToString());
                if (parentId == -1)
                    Class1.appendSystemMsgToUnReadBox(id, userIdSend, userIdSend + "请求加入群聊" + groupId, Class1.UnReadMsgType.AddGroup, new Dictionary<string, object>() { { "group_id", groupId } });
                else
                {
                    var dt1 = Class1.sql.SqlTable($"SELECT status FROM `group_user_request` WHERE `id` = {parentId}");
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                        Console.WriteLine("parentId 不存在");
                        return;
                    }
                    var status = long.Parse(dt1.Rows[0]["status"].ToString());
                    /*
                    string statusString;
                    if (status == (long)Class1.AddFriendStatus.Accepted)
                        statusString = "请求通过";
                    else if (status == (long)Class1.AddFriendStatus.Rejected)
                        statusString = "请求被拒绝";
                    else
                    {
                        Console.WriteLine("无效的status, status = " + status);
                        return;
                    }*/
                    Class1.appendSystemMsgToUnReadBox(id, userIdSend, userIdSend + (status == (long)Class1.AddFriendStatus.Accepted ? "通过" : "拒绝") + "了加群请求", Class1.UnReadMsgType.ReplyAddGroup, new Dictionary<string, object>());
                }
            }

            icon.ChangeIconState();
        }

        public static void UpdateMessageStatus(Dictionary<long, int> message, Dictionary<long, int> userRelationRequest, Dictionary<long, int> groupUserRequest)
        {
            if (!Class1.IsOnline) //离线时，为保证本地和远程数据一致性，不做已读状态更新
                return;
            foreach (var item in message)
            {
                bool ret = Class1.sql.ExecuteNonQuery($"UPDATE `message` SET `is_read` = {item.Value} WHERE `id` = {item.Key}");
                if (!ret)
                {
                    MessageBox.Show("DB错误，UPDATE message", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            foreach (var item in userRelationRequest)
            {
                bool ret = Class1.sql.ExecuteNonQuery($"UPDATE `user_relation_request` SET `status` = {item.Value} WHERE `id` = {item.Key}");
                if (!ret)
                {
                    MessageBox.Show("DB错误，UPDATE user_relation_request", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            foreach (var item in groupUserRequest)
            {
                bool ret = Class1.sql.ExecuteNonQuery($"UPDATE `group_user_request` SET `status` = {item.Value} WHERE `id` = {item.Key}");
                if (!ret)
                {
                    MessageBox.Show("DB错误，UPDATE group_user_request", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            var req = new http._Message.UpdateMessageStatusReq()
            {
                MessageIds = message,
                UserRelationRequestIds = userRelationRequest,
                GroupUserRequestIds = groupUserRequest
              
            };
            http._Message.UpdateMessageStatus(req);
        }
        public static byte[] Gzip(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    zipStream.Write(data, 0, data.Length);
                    zipStream.Close();
                    return compressedStream.ToArray();
                }
            }
        }
        public static byte[] UnGzip(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        zipStream.Close();
                        return resultStream.ToArray();
                    }
                }
            }
        }
        public static byte[] LoadFile(string localFileName)
        {
            byte[] data = new byte[0];
            try
            {
                FileStream fs = new FileStream(localFileName, FileMode.Open);
                if (File.Exists(localFileName))
                {
                    using (BinaryReader br = new BinaryReader(fs))//将文件流内容存放到BinaryReader的对象br中
                    {
                        var buf = new byte[1024 * 1024];
                        int count;
                        while ((count = br.Read(buf, 0, 1024 * 1024)) > 0)
                            data = data.Concat(buf.Take(count)).ToArray();
                        br.Close();
                    }
                    fs.Close();
                }
                else
                {
                    MessageBox.Show("[" + localFileName + "]不存在.", "读取失败");
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("错误消息:" + ex.Message, "IOException异常");
            }
            return data;
        }
    }
}
