using Aliyun.OSS;
using Aliyun.OSS.Common;
using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poseidon
{
    public partial class frm_chat : Form
    {
        public long userIdChat;
        HashSet<Image> imagePool = new HashSet<Image>();
        delegate void ProgressBarSetValueCallBackCallBack(ProgressBar pgb, int value); 
        private void InvokeProgressBarSetValue(ProgressBar pgb, int value)
        {
            if (pgb.InvokeRequired)
            {
                ProgressBarSetValueCallBackCallBack cb = new ProgressBarSetValueCallBackCallBack(InvokeProgressBarSetValue);
                this.Invoke(cb, new object[] { pgb, value });
            }
            else
                pgb.Value = value;
        }
        public frm_chat(long userId)
        {
            InitializeComponent();
            this.Text = $"私聊会话 {Class1.UserId2User[userId].NickName}({userId})";
            userIdChat = userId;
            DataTable dt;
            long lastReadMsgId = long.MaxValue;

            if (Class1.IsOnline)
            {
                var getFriendLastReadMsgIdReq = new http._User_Relation.GetFriendLastReadMsgIdReq()
                {
                    UserId = Class1.UserId
                };
                var getFriendLastReadMsgIdResp = http._User_Relation.GetFriendLastReadMsgId(getFriendLastReadMsgIdReq);
                if (getFriendLastReadMsgIdResp.LastReadMsgId.ContainsKey(userId))
                    lastReadMsgId = getFriendLastReadMsgIdResp.LastReadMsgId[userId];
                else
                {
                    //未成为好友，无法发送消息，会在SendMessage中拒绝，这里放行
                }
            }

            dt = Class1.sql.SqlTable($"SELECT count(*) as count FROM `message` WHERE `id` > {lastReadMsgId} AND `user_id_send` = {userId} AND `user_id_recv` = {Class1.UserId}");
            if (dt == null || dt.Rows.Count != 1)
                throw new Exception("select count(*) from message failed");

            var unReadCount = long.Parse(dt.Rows[0]["count"].ToString());
            //加载历史消息，数量为page_size和未读消息数的较大者
            dt = Class1.sql.SqlTable($"SELECT id, user_id_send, content, create_time, content_type FROM `message` WHERE (`user_id_send` = {Class1.UserId} AND `user_id_recv` = {userId}) OR (`user_id_send` = {userId} AND `user_id_recv` = {Class1.UserId}) ORDER BY `id` DESC LIMIT 0,{Math.Max(unReadCount, Class1.PageSize)}");
            dt.DefaultView.Sort = "id ASC";
            dt = dt.DefaultView.ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var id = (long)dt.Rows[i]["id"];
                var userIdSend = (long)dt.Rows[i]["user_id_send"];
                var content = System.Text.Encoding.Default.GetString(Class1.UnGzip((byte[])dt.Rows[i]["content"]));
                var createTime = (long)dt.Rows[i]["create_time"];
                var contentType = (long)dt.Rows[i]["content_type"];
                if (id > lastReadMsgId && (i == 0 || (long)dt.Rows[i - 1]["id"] <= lastReadMsgId))
                    Class1.appendCenteralText(this, "以下为新消息");
                switch (contentType)
                {
                    case (int)Class1.ContentType.Text:
                        {
                            Class1.appendRtfToMsgBox(this, $"{Class1.UserId2User[userIdSend].NickName}({userIdSend})", Class1.StampToDateTime(createTime), content);
                            break;
                        }
                    case (int)Class1.ContentType.Object:
                        {
                            var objId = long.Parse(content);
                            DataTable dt1 = Class1.sql.SqlTable($"SELECT e_tag, name FROM `object` WHERE `id` = {objId}");
                            if (dt1 == null || dt1.Rows.Count != 1)
                                throw new Exception("rowCount != 1, rowCount = " + dt1.Rows.Count);
                            Class1.appendFileToMsgBox(this, $"{Class1.UserId2User[userIdSend].NickName}({userIdSend})", Class1.StampToDateTime(createTime), "[文件]" + dt1.Rows[0]["name"].ToString(), objId);
                            break;
                        }
                    case (int)Class1.ContentType.Vibration:
                        {
                            Class1.appendSysMsgToMsgBox(this, "你" + (userIdSend == Class1.UserId ? "发送" : "收到") + "了一个窗口抖动。\r\n", Class1.StampToDateTime(createTime));
                            break;
                        }
                    case (int)Class1.ContentType.Image:
                        {
                            var eTag = content;
                            DataTable dt1 = Class1.sql.SqlTable($"SELECT content FROM `image` WHERE `e_tag` = \"{eTag}\"");
                            if (dt1 == null || dt1.Rows.Count != 1)
                                throw new Exception("rowCount != 1, rowCount = " + dt1.Rows.Count);
                            var imageData = Class1.UnGzip((byte[])dt1.Rows[0]["content"]);
                            var stream = Class1.appendImageToMsgBox(this, $"{Class1.UserId2User[userIdSend].NickName}({userIdSend})", Class1.StampToDateTime(createTime), imageData);
                            imagePool.Add(stream);
                            break;
                        }
                    default:
                            throw new Exception("unknown content_type, content_type = " + contentType);
                }
            }
        }
        private void frm_chat_FormClosed(object sender, FormClosedEventArgs e)
        {
            Class1.formChatPool.Remove(userIdChat);
            foreach (var image in imagePool)
                image.Dispose();
        }
        private void UploadProgressCallback(object sender, StreamTransferProgressArgs args)
        {
            Invoke(new Action(() =>
            {
                pgb_upload.Value = (int)(args.TransferredBytes * 100 / args.TotalBytes);
            }));
        }
        private void UploadFile(string localFileName)
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
                            putObjectRequest.StreamTransferProgress += UploadProgressCallback;
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
                Invoke(new Action(() =>
                {
                    pgb_upload.Value = 100;
                    pgb_upload.Visible = false;
                }));

                var name = Path.GetFileName(localFileName);
                var createObjectReq = new http._Object.CreateObjectReq()
                {
                    ETag = eTag,
                    Name = name
                };
                var createObjectResp = http._Object.CreateObject(createObjectReq);
                var objId = createObjectResp.Id;
                Class1.InsertObjectIfNotExists(objId, eTag, name);

                var sendMessageReq = new http._Message.SendMessageReq()
                {
                    UserIdSend = Class1.UserId,
                    IdRecv = userIdChat,
                    Content = objId.ToString() ,
                    ContentType = (int)Class1.ContentType.Object,
                    MessageType = 0
                };
                var sendMessageResp = http._Message.SendMessage(sendMessageReq);
                var statusCode = sendMessageResp.StatusCode;

                switch (statusCode)
                {
                    case 1:
                        {
                            Class1.appendSysMsgToMsgBox(this, "你与" + $"{Class1.UserId2User[userIdChat].NickName}({userIdChat})" + "未成为好友，无法发送消息", DateTime.Now);
                            return;
                        }
                }

                var messageId = sendMessageResp.Id;
                var createTime = sendMessageResp.CreateTime;
                var param = Class1.Gzip(System.Text.Encoding.Default.GetBytes(objId.ToString()));
                var ret = Class1.sql.ExecuteNonQueryWithBinary($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type) VALUES({messageId}, " +
                            $"{Class1.UserId}, {userIdChat}, 0, @param, {createTime}, {(int)Class1.ContentType.Object}, 0)", param);
                if (!ret)
                {
                    MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Class1.appendFileToMsgBox(this, $"{Class1.UserId2User[Class1.UserId].NickName}({Class1.UserId})", Class1.StampToDateTime(createTime), "[文件]" + name, objId);
            }));
            t.Start();
        }
        private void DownloadProgressCallback(object sender, StreamTransferProgressArgs args)
        {
            Invoke(new Action(() =>
            {
                pgb_download.Value = (int)(args.TransferredBytes * 100 / args.TotalBytes);
            }));
        }
        private void DownloadFile(string eTag, string localFileName)
        {
            var req = new http._Oss.GetSTSInfoReq()
            {
                UserId = Class1.UserId
            };
            var resp = http._Oss.GetSTSInfo(req);
            Thread t = new Thread(new ThreadStart(() =>
            {
                // 拿到STS临时凭证后，通过其中的安全令牌（SecurityToken）和临时访问密钥（AccessKeyId和AccessKeySecret）生成OSSClient。
                var client = new OssClient(Class1.EndPoint, resp.AccessKeyId, resp.AccessKeySecret, resp.SecurityToken);
                try
                {
                    var getObjectRequest = new GetObjectRequest(Class1.BucketName, eTag);
                    getObjectRequest.StreamTransferProgress += DownloadProgressCallback;
                    // 下载文件。
                    var ossObject = client.GetObject(getObjectRequest);
                    using (var stream = ossObject.Content)
                    {
                        var buffer = new byte[1024 * 1024];
                        var bytesRead = 0;
                        var bw = new BinaryWriter(new FileStream(localFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite));
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            bw.Write(buffer.Take(bytesRead).ToArray());
                        }
                        bw.Close();
                    }
                    Console.WriteLine("Get object:{0} succeeded", eTag); 
                    Invoke(new Action(() =>
                    {
                        pgb_download.Value = 100;
                        pgb_download.Visible = false;
                    }));
                }
                catch (OssException ex)
                {
                    Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message + "\n Cannot create file.");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed with error info: {0}", ex.Message);
                }
            }));
            t.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var content = rtxt_send.Rtf;
            var req = new http._Message.SendMessageReq()
            {
                UserIdSend = Class1.UserId,
                IdRecv = userIdChat,
                Content = content,
                ContentType = (int)Class1.ContentType.Text,
                MessageType = (int)Class1.MsgType.PrivateChat
            };
            var resp = http._Message.SendMessage(req);
            var statusCode = resp.StatusCode;

            switch (statusCode)
            {
                case 1:
                    {
                        Class1.appendSysMsgToMsgBox(this, "你与" + $"{Class1.UserId2User[userIdChat].NickName}({userIdChat})" + "未成为好友，无法发送消息", DateTime.Now);
                        return;
                    }
            }

            var param = Class1.Gzip(System.Text.Encoding.Default.GetBytes(content));
            bool ret = Class1.sql.ExecuteNonQueryWithBinary($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type) VALUES({resp.Id}, " +
                            $"{Class1.UserId}, {userIdChat}, 0, @param, {resp.CreateTime}, {(int)Class1.ContentType.Text}, 0)", param);
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Class1.appendRtfToMsgBox(this, $"{Class1.UserId2User[Class1.UserId].NickName}({Class1.UserId})", DateTime.Now,content);
            rtxt_send.Rtf = string.Empty;
        }
        private void rtxt_message_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var text = e.LinkText;
            var left = text.LastIndexOf("[");
            var right = text.LastIndexOf(']');
            var objId = long.Parse(text.Substring(left + 1, right - left - 1));


            var dt = Class1.sql.SqlTable($"SELECT e_tag, name FROM `object` WHERE `id` = {objId}");
            if(dt == null || dt.Rows.Count != 1) 
                throw new Exception("rowCount != 1");
            var eTag = dt.Rows[0]["e_tag"].ToString();
            var name = dt.Rows[0]["name"].ToString();

            saveFileDialog1.FileName = name;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var localFileName = saveFileDialog1.FileName;
                pgb_download.Value = 0;
                pgb_download.Visible = true;
                DownloadFile(eTag, localFileName);
            }
        }

        private void toolFont_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == fontDialog1.ShowDialog())
            {
                rtxt_send.Font = fontDialog1.Font;
                rtxt_send.ForeColor = fontDialog1.Color;
            }
        }

        private void toolVibration_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var req = new http._Message.SendMessageReq()
            {
                UserIdSend = Class1.UserId,
                IdRecv = userIdChat,
                Content = "vibration",
                ContentType = (int)Class1.ContentType.Vibration
            };
            var resp = http._Message.SendMessage(req);
            var statusCode = resp.StatusCode;

            switch (statusCode)
            {
                case 1:
                    {
                        Class1.appendSysMsgToMsgBox(this, "你与" + $"{Class1.UserId2User[userIdChat].NickName}({userIdChat})" + "未成为好友，无法发送消息", DateTime.Now);
                        return;
                    }
            }

            var param = Class1.Gzip(System.Text.Encoding.Default.GetBytes(""));
            bool ret = Class1.sql.ExecuteNonQueryWithBinary($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type) VALUES({resp.Id}, " +
                            $"{Class1.UserId}, {userIdChat}, 0, @param, {resp.CreateTime}, {(int)Class1.ContentType.Vibration}, 0)", param);
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Class1.appendSysMsgToMsgBox(this, "你发送了一个窗口抖动。\r\n", DateTime.Now);
            rtxt_message.Select(rtxt_message.Text.Length, 0);
            rtxt_message.ScrollToCaret();
            rtxt_send.Focus();
            Class1.Vibration(this);
        }

        private void rtxt_send_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                // suspend layout to avoid blinking
                rtxt_send.SuspendLayout();

                // get insertion point
                int insPt = rtxt_send.SelectionStart;

                // preserve text from after insertion pont to end of RTF content
                string postRTFContent = rtxt_send.Text.Substring(insPt);

                // remove the content after the insertion point
                rtxt_send.Text = rtxt_send.Text.Substring(0, insPt);

                // add the clipboard content and then the preserved postRTF content
                rtxt_send.Text += (string)Clipboard.GetData("Text") + postRTFContent;

                // adjust the insertion point to just after the inserted text
                rtxt_send.SelectionStart = rtxt_send.TextLength - postRTFContent.Length;

                // restore layout
                rtxt_send.ResumeLayout();

                // cancel the paste
                e.Handled = true;
            }
        }

        private void toolImgFile_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            openFileDialog1.Filter = "图片文件|*.jpg;*.bmp;*.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var localFileName = openFileDialog1.FileName;
                Class1.UploadImage(localFileName, userIdChat, this);
            }
        }
        private void toolfile_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            openFileDialog1.Filter = "所有文件|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var localFileName = openFileDialog1.FileName;
                pgb_upload.Value = 0;
                pgb_upload.Visible = true;
                UploadFile(localFileName);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
