using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poseidon
{
    public static class cls_group
    {
        public static void appendRtfToMsgBox(frm_group frmGroup, string name, DateTime time, string content)
        {
            if (!frmGroup.IsHandleCreated)
            {
                frmGroup.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                     new Font(frmGroup.Font, FontStyle.Regular), Color.Green);
                frmGroup.rtxt_message.AppendRtf(content);
                frmGroup.rtxt_message.Select(frmGroup.rtxt_message.Text.Length, 0);
                frmGroup.rtxt_message.ScrollToCaret();
                frmGroup.rtxt_send.Focus();
            }
            else
            {
                frmGroup.Invoke(new Action(() =>
                {
                    frmGroup.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                         new Font(frmGroup.Font, FontStyle.Regular), Color.Green);
                    frmGroup.rtxt_message.AppendRtf(content);
                    frmGroup.rtxt_message.Select(frmGroup.rtxt_message.Text.Length, 0);
                    frmGroup.rtxt_message.ScrollToCaret();
                    frmGroup.rtxt_send.Focus();
                }));
            }
        }
        public static void appendFileToMsgBox(frm_group frmGroup, string name, DateTime time, string fileName, long objId)
        {
            if (!frmGroup.IsHandleCreated)
            {
                frmGroup.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                     new Font(frmGroup.Font, FontStyle.Regular), Color.Green);
                frmGroup.rtxt_message.AppendText(fileName);
                frmGroup.rtxt_message.AppendText("\n");
                frmGroup.rtxt_message.InsertLink("下载文件[" + objId + "]");
                frmGroup.rtxt_message.AppendText("\n");
                frmGroup.rtxt_message.Select(frmGroup.rtxt_message.Text.Length, 0);
                frmGroup.rtxt_message.ScrollToCaret();
                frmGroup.rtxt_send.Focus();
            }
            else
            {
                frmGroup.Invoke(new Action(() =>
                {
                    frmGroup.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                         new Font(frmGroup.Font, FontStyle.Regular), Color.Green);
                    frmGroup.rtxt_message.AppendText(fileName);
                    frmGroup.rtxt_message.AppendText("\n");
                    frmGroup.rtxt_message.InsertLink("下载文件[" + objId + "]");
                    frmGroup.rtxt_message.AppendText("\n");
                    frmGroup.rtxt_message.Select(frmGroup.rtxt_message.Text.Length, 0);
                    frmGroup.rtxt_message.ScrollToCaret();
                    frmGroup.rtxt_send.Focus();
                }));
            }
        }
        
        public static void appendSysMsgToMsgBox(frm_group frmGroup, string content, DateTime time)
        {
            if (!frmGroup.IsHandleCreated)
            {
                frmGroup.rtxt_message.AppendRichText("[系统消息]" + "  " + time.ToLongTimeString() + "\r\n",
                new Font(frmGroup.Font, FontStyle.Regular), Color.Blue);
                frmGroup.rtxt_message.AppendText(content + "\r\n");
                frmGroup.rtxt_message.ForeColor = Color.Black;
                frmGroup.rtxt_message.Select(frmGroup.rtxt_message.Text.Length, 0);
                frmGroup.rtxt_message.ScrollToCaret();
                frmGroup.rtxt_send.Focus();
            }
            else
            {
                frmGroup.Invoke(new Action(() =>
                {
                    frmGroup.rtxt_message.AppendRichText("[系统消息]" + "  " + time.ToLongTimeString() + "\r\n",
                new Font(frmGroup.Font, FontStyle.Regular), Color.Blue);
                    frmGroup.rtxt_message.AppendText(content + "\r\n");
                    frmGroup.rtxt_message.ForeColor = Color.Black;
                    frmGroup.rtxt_message.Select(frmGroup.rtxt_message.Text.Length, 0);
                    frmGroup.rtxt_message.ScrollToCaret();
                    frmGroup.rtxt_send.Focus();
                }));
            }
        }
        public static Image appendImageToMsgBox(frm_group frmGroup, string name, DateTime time, byte[] imageData)
        {
            var image = Image.FromStream(new MemoryStream(imageData));
            if (!frmGroup.IsHandleCreated)
            {
                frmGroup.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                     new Font(frmGroup.Font, FontStyle.Regular), Color.Green);
                frmGroup.rtxt_message.InsertImage(image);
                frmGroup.rtxt_message.AppendText("\n");
                frmGroup.rtxt_message.Select(frmGroup.rtxt_message.Text.Length, 0);
                frmGroup.rtxt_message.ScrollToCaret();
                frmGroup.rtxt_send.Focus();
            }
            else
            {
                frmGroup.Invoke(new Action(() =>
                {
                    frmGroup.rtxt_message.AppendRichText(name + "  " + time.ToLongTimeString() + "\r\n",
                        new Font(frmGroup.Font, FontStyle.Regular), Color.Green);
                    frmGroup.rtxt_message.InsertImage(image);
                    frmGroup.rtxt_message.AppendText("\n");
                    frmGroup.rtxt_message.Select(frmGroup.rtxt_message.Text.Length, 0);
                    frmGroup.rtxt_message.ScrollToCaret();
                    frmGroup.rtxt_send.Focus();
                }));
            }
            return image;
        }
        
        public static void appendCenteralText(frm_group frmGroup, string msg)
        {
            var len = frmGroup.rtxt_message.TextLength;
            frmGroup.rtxt_message.AppendRichText(msg, new Font(frmGroup.Font, FontStyle.Regular), Color.Red);
            frmGroup.rtxt_message.Select(len, 0);
            frmGroup.rtxt_message.SelectionAlignment = HorizontalAlignment.Center;
            frmGroup.rtxt_message.AppendText("\n");
            frmGroup.rtxt_message.SelectionAlignment = HorizontalAlignment.Left;
        }

        public static void UploadImage(string localFileName, long groupId, frm_group frmGroup)
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
                    IdRecv = groupId,
                    Content = eTag,
                    ContentType = (int)Class1.ContentType.Image,
                    MessageType = (int)Class1.MsgType.GroupChat
                };
                var sendMessageResp = http._Message.SendMessage(sendMessageReq);
                var statusCode = sendMessageResp.StatusCode;

                switch (statusCode)
                {
                    case 1:
                        {
                            cls_group.appendSysMsgToMsgBox(frmGroup, "你未加入群聊，无法发送消息", DateTime.Now);
                            return;
                        }
                }


                var messageId = sendMessageResp.Id;
                var createTime = sendMessageResp.CreateTime;
                var param = Class1.Gzip(System.Text.Encoding.Default.GetBytes(eTag));
                var ret = Class1.sql.ExecuteNonQueryWithBinary($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type, is_read) VALUES({messageId}, " +
                            $"{Class1.UserId}, 0, {groupId}, @param, {createTime}, {(int)Class1.ContentType.Image}, {(int)Class1.MsgType.GroupChat}, 1)", param);
                if (!ret)
                {
                    MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var imageData = Class1.LoadFile(localFileName);
                var imageParam = Class1.Gzip(imageData);
                Class1.InsertImage(eTag, imageParam);
                cls_group.appendImageToMsgBox(frmGroup, Class1.UserId.ToString(), Class1.StampToDateTime(createTime), imageData);
            }));
            t.Start();
        }
    }
}
