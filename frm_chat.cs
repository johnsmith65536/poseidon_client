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
        public static long userIdChat;
        public frm_chat()
        {
            InitializeComponent();
        }

        delegate void ProgressBarSetValueCallBackCallBack(ProgressBar pgb, int value); 
        delegate void GridAddCallBack(DataGridView dgv, Dictionary<string, object> dict);
        delegate void GridClearCallBack(DataGridView dgv);

        private void InvokeGridAdd(DataGridView dgv, Dictionary<string, object> dict)
        {
            if (dgv.InvokeRequired)
            {
                GridAddCallBack stcb = new GridAddCallBack(InvokeGridAdd);
                this.Invoke(stcb, new object[] { dgv, dict });
            }
            else
                Class1.GridAdd(dgv, dict);
        }
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
            userIdChat = userId;
            this.Text = "与" + userId + "的会话";
            DataTable dt = Class1.sql.SqlTable($"SELECT user_id_send, content, create_time, content_type FROM `message` WHERE (`user_id_send` = {Class1.UserId} AND `user_id_recv` = {userId}) OR (`user_id_send` = {userId} AND `user_id_recv` = {Class1.UserId})");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var userIdSend = (long)dt.Rows[i]["user_id_send"];
                var content = System.Text.Encoding.Default.GetString(Class1.UnGzip((byte[])dt.Rows[i]["content"]));
                var createTime = (long)dt.Rows[i]["create_time"];
                var contentType = (long)dt.Rows[i]["content_type"];
                Console.WriteLine("content_type = " + contentType);

                switch (contentType)
                {
                    case (int)Class1.ContentType.Text:
                        {
                            /*Class1.GridAdd(dgv_msg, new Dictionary<string, object> {
        {"user_id",userIdSend},
        {"content",content},
        {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(createTime))}
    });*/
                            Class1.appendRtfToMsgBox(this, userIdSend.ToString(), Class1.StampToDateTime(createTime), content);
                            break;
                        }
                    case (int)Class1.ContentType.Object:
                        {
                            var objId = long.Parse(content);
                            DataTable dt1 = Class1.sql.SqlTable($"SELECT e_tag, name FROM `object` WHERE `id` = {objId}");
                            if (dt1.Rows.Count != 1)
                            {
                                Console.WriteLine("rowCount != 1, rowCount = " +dt1.Rows.Count);
                                return;
                            }
                            /*Class1.GridAdd(dgv_msg, new Dictionary<string, object> {
        {"user_id",userIdSend},
        {"content","[文件]" + dt1.Rows[0]["name"].ToString()},
        {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(createTime))},
        {"e_tag",dt1.Rows[0]["e_tag"].ToString()},
    });*/
                            Class1.appendFileToMsgBox(this, userIdSend.ToString(), Class1.StampToDateTime(createTime), "[文件]" + dt1.Rows[0]["name"].ToString(), objId);
                            break;
                        }
                    case (int)Class1.ContentType.Vibration:
                        {
                            /*Class1.GridAdd(dgv_msg, new Dictionary<string, object> {
        {"user_id",userIdSend},
        {"content",content},
        {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(createTime))}
    });*/
                            Class1.appendVibrationToMsgBox(this, userIdSend.ToString(), Class1.StampToDateTime(createTime));
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("unknown content_type, content_type = ", contentType);
                            break;
                        }
                }   
            }
        }

        /*private void btn_send_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var content = txt_send.Text;
            var req = new http._Message.SendMessageReq()
            {
                UserIdSend = Class1.UserId,
                IdRecv = userIdChat,
                Content = content,
                ContentType = (int)Class1.ContentType.Text,
                MessageType = 0
            };
            var resp = http._Message.SendMessage(req);
            bool ret = Class1.sql.ExecuteNonQuery($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type, is_read) VALUES({resp.Id}, " +
                            $"{Class1.UserId}, {userIdChat}, 0, \"{content}\", {resp.CreateTime}, {(int)Class1.ContentType.Text}, 0, 0)");
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Class1.GridAdd(dgv_msg, new Dictionary<string, object> {
            {"user_id",Class1.UserId},
            {"content",content},
            {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(resp.CreateTime))}
        });
            txt_send.Text = "";
        }*/
        private void frm_chat_FormClosed(object sender, FormClosedEventArgs e)
        {
            Class1.formChatPool.Remove(userIdChat);
        }

        private void btn_send_file_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var localFileName = openFileDialog1.FileName;
                InvokeProgressBarSetValue(pgb_upload, 0);
                UploadFile(localFileName);
                InvokeProgressBarSetValue(pgb_upload, 100);

            }
        }
        private void UploadProgressCallback(object sender, StreamTransferProgressArgs args)
        {
            //System.Console.WriteLine("ProgressCallback - Progress: {0}%, TotalBytes:{1}, TransferredBytes:{2} ",
            //args.TransferredBytes * 100 / args.TotalBytes, args.TotalBytes, args.TransferredBytes);
            InvokeProgressBarSetValue(pgb_upload, (int)(args.TransferredBytes * 100 / args.TotalBytes));
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
                
                var name = Path.GetFileName(localFileName);
               var createObjectReq = new http._Object.CreateObjectReq()
                {
                    ETag = eTag,
                    Name = name
                };
                var createObjectResp = http._Object.CreateObject(createObjectReq);
                var objId = createObjectResp.Id;

                /*bool ret = Class1.sql.ExecuteNonQuery($"INSERT INTO `object`(id, e_tag, name) VALUES({objId}, " +
                            $"'{eTag}', '{name}')");
                if (!ret)
                {
                    //MessageBox.Show("DB错误，INSERT INTO object失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //return;
                }*/
                Class1.InsertObject(objId, eTag, name);

                var sendMessageReq = new http._Message.SendMessageReq()
                {
                    UserIdSend = Class1.UserId,
                    IdRecv = userIdChat,
                    Content = objId.ToString() ,
                    ContentType = (int)Class1.ContentType.Object,
                    MessageType = 0
                };
                var sendMessageResp = http._Message.SendMessage(sendMessageReq);

                //var sendMessageResp = rpc._Message.SendMessage(Class1.UserId, userIdChat, objId.ToString(), Class1.ContentType.Object, 0);
                var messageId = sendMessageResp.Id;
                var createTime = sendMessageResp.CreateTime;
                var param = Class1.Gzip(System.Text.Encoding.Default.GetBytes(objId.ToString()));
                var ret = Class1.sql.ExecuteNonQueryWithBinary($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type, is_read) VALUES({messageId}, " +
                            $"{Class1.UserId}, {userIdChat}, 0, @param, {createTime}, {(int)Class1.ContentType.Object}, 0, 0)", param);
                if (!ret)
                {
                    MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

           /*     InvokeGridAdd(dgv_msg, new Dictionary<string, object> {
            {"user_id",Class1.UserId},
            {"content","[文件]" + name},
            {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(createTime))},
            {"e_tag",eTag}
        });*/
                Class1.appendFileToMsgBox(this, Class1.UserId.ToString(), Class1.StampToDateTime(createTime), "[文件]" + name, objId);
            }));
            t.Start();
        }

        private void dgv_msg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           /* if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var eTag = dgv_msg.Rows[e.RowIndex].Cells["e_tag"].Value;
                var objectName = dgv_msg.Rows[e.RowIndex].Cells["content"].Value.ToString();
                dgv_msg.ClearSelection();
                dgv_msg.Rows[e.RowIndex].Selected = true;
                dgv_msg.CurrentCell = dgv_msg.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (eTag == null)
                    return;
                if (!Class1.IsOnline)
                {
                    MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                saveFileDialog1.FileName = objectName.Substring(4);
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var localFileName = saveFileDialog1.FileName;
                    InvokeProgressBarSetValue(pgb_download, 0);
                    DownloadFile(eTag.ToString(),localFileName);
                    InvokeProgressBarSetValue(pgb_download, 100);
                }
            }*/
        }
        private void DownloadProgressCallback(object sender, StreamTransferProgressArgs args)
        {
            //System.Console.WriteLine("ProgressCallback - Progress: {0}%, TotalBytes:{1}, TransferredBytes:{2} ",
            //args.TransferredBytes * 100 / args.TotalBytes, args.TotalBytes, args.TransferredBytes);
            InvokeProgressBarSetValue(pgb_download, (int)(args.TransferredBytes * 100 / args.TotalBytes));
        }
        private void DownloadFile(string eTag, string localFileName)
        {
            var req = new http._Oss.GetSTSInfoReq()
            {
                UserId = Class1.UserId
            };
            var resp = http._Oss.GetSTSInfo(req);
            //var resp = rpc._Oss.GetSTSInfo(Class1.UserId);
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
                MessageType = 0
            };
            var resp = http._Message.SendMessage(req);
            var param = Class1.Gzip(System.Text.Encoding.Default.GetBytes(content));
            bool ret = Class1.sql.ExecuteNonQueryWithBinary($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type, is_read) VALUES({resp.Id}, " +
                            $"{Class1.UserId}, {userIdChat}, 0, @param, {resp.CreateTime}, {(int)Class1.ContentType.Text}, 0, 0)", param);
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            /*Class1.GridAdd(dgv_msg, new Dictionary<string, object> {
            {"user_id",Class1.UserId},
            {"content",content},
            {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(resp.CreateTime))}
        });*/

            Class1.appendRtfToMsgBox(this, Class1.UserId.ToString(),DateTime.Now,content);
            rtxt_send.Rtf = string.Empty;
        }
        

        private void frm_chat_Load(object sender, EventArgs e)
        {

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
            if(dt.Rows.Count != 1) 
                throw new Exception("rowCount != 1");
            var eTag = dt.Rows[0]["e_tag"].ToString();
            var name = dt.Rows[0]["name"].ToString();

            saveFileDialog1.FileName = name;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var localFileName = saveFileDialog1.FileName;
                InvokeProgressBarSetValue(pgb_download, 0);
                DownloadFile(eTag, localFileName);
                InvokeProgressBarSetValue(pgb_download, 100);
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
            /*var param = Class1.Gzip(System.Text.Encoding.Default.GetBytes(content));
            bool ret = Class1.sql.ExecuteNonQueryWithBinary($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type, is_read) VALUES({resp.Id}, " +
                            $"{Class1.UserId}, {userIdChat}, 0, @param, {resp.CreateTime}, {(int)Class1.ContentType.Text}, 0, 0)", param);*/
            var param = Class1.Gzip(System.Text.Encoding.Default.GetBytes(""));
            bool ret = Class1.sql.ExecuteNonQueryWithBinary($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type, is_read) VALUES({resp.Id}, " +
                            $"{Class1.UserId}, {userIdChat}, 0, @param, {resp.CreateTime}, {(int)Class1.ContentType.Vibration}, 0, 0)", param);
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            /*    Class1.GridAdd(dgv_msg, new Dictionary<string, object> {
                {"user_id",Class1.UserId},
                {"content",content},
                {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(resp.CreateTime))}
            });*/

            /*  Class1.appendRtfToMsgBox(this, Class1.UserId.ToString(), DateTime.Now, content);
              rtxt_message.Select(rtxt_message.Text.Length, 0);
              rtxt_message.ScrollToCaret();
              rtxt_send.Text = string.Empty;
              rtxt_send.Focus();*/


            Class1.appendVibrationToMsgBox(this, Class1.UserId.ToString(), DateTime.Now);
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
    }
}
