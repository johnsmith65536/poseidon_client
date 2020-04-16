using Aliyun.OSS;
using Aliyun.OSS.Common;
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
    public partial class frm_msg_view : Form
    {
        public long UserId = -1;
        public long GroupId = -1;
        public long MsgCount;
        public long PageCount;
        public long CurPage;
        public const int PageSize = 20;
        public string WhereClause;
        HashSet<Image> imagePool = new HashSet<Image>();
        public frm_msg_view(long id,string idType)
        {
            InitializeComponent();
            switch(idType)
            { 
                case "private":
                    {
                        UserId = id;
                        WhereClause = $"(`user_id_send` = {Class1.UserId} AND `user_id_recv` = {UserId}) OR (`user_id_send` = {UserId} AND `user_id_recv` = {Class1.UserId})";
                        this.Text = $"私聊会话 {Class1.GetUserInfo(UserId).NickName}({UserId}) 的消息记录";
                        break;
                    }
                case "group":
                    {
                        GroupId = id;
                        WhereClause = $"`group_id` = {GroupId}";
                        this.Text = $"群聊会话 {Class1.GetGroupInfo(GroupId).Name}({GroupId}) 的消息记录";
                        break;
                    }
                default:
                    throw new Exception("unknown id_type");
            }
            var dt = Class1.sql.SqlTable($"SELECT count(*) as count FROM `message` WHERE {WhereClause}");
            if (dt == null || dt.Rows.Count != 1)
                throw new Exception("select count(*) from message failed");
            MsgCount = (long)dt.Rows[0]["count"];
            PageCount = MsgCount / PageSize + (MsgCount % PageSize == 0 ? 0 : 1);
        }
        public void LoadMessage(long pageIndex)
        {
            rtxt_message.Clear();
            var dt = Class1.sql.SqlTable($"SELECT id, user_id_send, content, create_time, content_type FROM `message` WHERE {WhereClause} LIMIT {pageIndex*PageSize},{Math.Min(MsgCount - pageIndex * PageSize, PageSize)}");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var id = (long)dt.Rows[i]["id"];
                var userIdSend = (long)dt.Rows[i]["user_id_send"];
                var content = System.Text.Encoding.Default.GetString(Class1.UnGzip((byte[])dt.Rows[i]["content"]));
                var createTime = (long)dt.Rows[i]["create_time"];
                var contentType = (long)dt.Rows[i]["content_type"];
                switch (contentType)
                {
                    case (int)Class1.ContentType.Text:
                        {
                            appendRtfToMsgBox($"{Class1.GetUserInfo(userIdSend).NickName}({userIdSend})", Class1.StampToDateTime(createTime), content);
                            break;
                        }
                    case (int)Class1.ContentType.Object:
                        {
                            var objId = long.Parse(content);
                            DataTable dt1 = Class1.sql.SqlTable($"SELECT e_tag, name FROM `object` WHERE `id` = {objId}");
                            if (dt1 == null || dt1.Rows.Count != 1)
                                throw new Exception("rowCount != 1, rowCount = " + dt1.Rows.Count);
                            appendFileToMsgBox($"{Class1.GetUserInfo(userIdSend).NickName}({userIdSend})", Class1.StampToDateTime(createTime), "[文件]" + dt1.Rows[0]["name"].ToString(), objId);
                            break;
                        }
                    case (int)Class1.ContentType.Vibration:
                        {
                            appendSysMsgToMsgBox("你" + (userIdSend == Class1.UserId ? "发送" : "收到") + "了一个窗口抖动。\r\n", Class1.StampToDateTime(createTime));
                            break;
                        }
                    case (int)Class1.ContentType.Image:
                        {
                            var eTag = content;
                            DataTable dt1 = Class1.sql.SqlTable($"SELECT content FROM `image` WHERE `e_tag` = \"{eTag}\"");
                            if (dt1 == null || dt1.Rows.Count != 1)
                                throw new Exception("rowCount != 1, rowCount = " + dt1.Rows.Count);
                            var imageData = Class1.UnGzip((byte[])dt1.Rows[0]["content"]);
                            var stream = appendImageToMsgBox($"{Class1.GetUserInfo(userIdSend).NickName}({userIdSend})", Class1.StampToDateTime(createTime), imageData);
                            imagePool.Add(stream);
                            break;
                        }
                    default:
                        throw new Exception("unknown content_type, content_type = " + contentType);
                }
            }
            CurPage = pageIndex;
            label1.Text = $"{CurPage+1}/{PageCount}";
        }
        public void appendRtfToMsgBox(string name, DateTime time, string content)
        {
            rtxt_message.AppendRichText(name + "  " + time.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n",
                    new Font(Font, FontStyle.Regular), Color.Green);
            rtxt_message.AppendRtf(content);
            rtxt_message.Select(rtxt_message.Text.Length, 0);
            rtxt_message.ScrollToCaret();
        }
        public void appendFileToMsgBox(string name, DateTime time, string fileName, long objId)
        {
            rtxt_message.AppendRichText(name + "  " + time.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n",
                    new Font(Font, FontStyle.Regular), Color.Green);
            rtxt_message.AppendText(fileName);
            rtxt_message.AppendText("\n");
            rtxt_message.InsertLink("下载文件[" + objId + "]");
            rtxt_message.AppendText("\n");
            rtxt_message.Select(rtxt_message.Text.Length, 0);
            rtxt_message.ScrollToCaret();

        }
        public void appendSysMsgToMsgBox(string content, DateTime time)
        {
            rtxt_message.AppendRichText("[系统消息]" + "  " + time.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n",
            new Font(Font, FontStyle.Regular), Color.Blue);
            rtxt_message.AppendText(content + "\r\n");
            rtxt_message.ForeColor = Color.Black;
            rtxt_message.Select(rtxt_message.Text.Length, 0);
            rtxt_message.ScrollToCaret();
        }
        public Image appendImageToMsgBox(string name, DateTime time, byte[] imageData)
        {
            var image = Image.FromStream(new MemoryStream(imageData));
            rtxt_message.AppendRichText(name + "  " + time.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n",
                    new Font(Font, FontStyle.Regular), Color.Green);
            rtxt_message.InsertImage(image);
            rtxt_message.AppendText("\n");
            rtxt_message.Select(rtxt_message.Text.Length, 0);
            rtxt_message.ScrollToCaret();
            return image;
        }
        private void frm_msg_view_Load(object sender, EventArgs e)
        {
            LoadMessage(PageCount - 1);
        }

        private void frm_msg_view_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(UserId != -1)
                Class1.formMsgViewPrivatePool.Remove(UserId);
            else
                Class1.formMsgViewGroupPool.Remove(GroupId);
            foreach (var image in imagePool)
                image.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CurPage == 0)
                return;
            LoadMessage(--CurPage);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CurPage == PageCount - 1)
                return;
            LoadMessage(++CurPage);
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
            if (dt == null || dt.Rows.Count != 1)
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
        private void DownloadProgressCallback(object sender, StreamTransferProgressArgs args)
        {
            Invoke(new Action(() =>
            {
                pgb_download.Value = (int)(args.TransferredBytes * 100 / args.TotalBytes);
            }));
        }
    }
}
