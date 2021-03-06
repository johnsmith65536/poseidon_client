﻿using Aliyun.OSS;
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
    public partial class frm_group : Form
    {
        //public static long groupIdChat;
        private Class1.Group groupChat;
        private Dictionary<long, ChatListSubItem> memberPool = new Dictionary<long, ChatListSubItem>();
        private HashSet<long> onlineUserId = new HashSet<long>();
        private HashSet<long> offlineUserId = new HashSet<long>();
        private long selectId;

        HashSet<Image> imagePool = new HashSet<Image>();
        public void login()
        {
            memberPool.Clear();
            onlineUserId.Clear();
            offlineUserId.Clear();
            clb_member.Items.Clear();
            clb_member.Items.Add(new ChatListItem("群成员", true));
            Thread t = new Thread(new ThreadStart(() =>
            {
                while (Class1.IsOnline && !this.IsDisposed)
                {
                    var req = new http._Group_User.FetchMemberListReq()
                    {
                        GroupId = groupChat.Id
                    };
                    var resp = http._Group_User.FetchMemberList(req);
                    if (resp.StatusCode == 254)
                        return;

                    foreach (var user in resp.OnlineUsers)
                        Class1.updateUserId2User(user.Id, new Class1.User() { Id = user.Id, NickName = user.NickName });
                    foreach (var user in resp.OfflineUsers)
                        Class1.updateUserId2User(user.Id, new Class1.User() { Id = user.Id, NickName = user.NickName });

                    var oldOnlineUserId = onlineUserId;
                    var oldOfflineUserId = offlineUserId;
                    var oldUserId = new HashSet<long>();
                    oldUserId.UnionWith(oldOnlineUserId);
                    oldUserId.UnionWith(oldOfflineUserId);

                    var newOnlineUserId = new HashSet<long>();
                    var newOfflineUserId = new HashSet<long>();
                    foreach (var user in resp.OnlineUsers)
                        newOnlineUserId.Add(user.Id);
                    foreach (var user in resp.OfflineUsers)
                        newOfflineUserId.Add(user.Id);
                    var newUserId = new HashSet<long>();
                    newUserId.UnionWith(newOnlineUserId);
                    newUserId.UnionWith(newOfflineUserId);

                    foreach (var userId in oldUserId)
                    {
                        if (!newUserId.Contains(userId))
                        {
                            var subItem = memberPool[userId];
                            clb_member.Items[0].SubItems.Remove(subItem);
                            memberPool.Remove(userId);
                        }
                    }

                    foreach (var userId in newUserId)
                    {
                        if (!oldUserId.Contains(userId))
                        {
                            var subItem = new ChatListSubItem(userId.ToString());
                            subItem.ID = (uint)userId;
                            subItem.DisplayName = Class1.UserId2User[userId].NickName;
                            subItem.NicName = userId.ToString();
                            clb_member.Items[0].SubItems.Add(subItem);
                            memberPool.Add(userId, subItem);
                        }
                    }

                    foreach (var user in resp.OnlineUsers)
                        memberPool[user.Id].Status = ChatListSubItem.UserStatus.Online;
                    foreach (var user in resp.OfflineUsers)
                        memberPool[user.Id].Status = ChatListSubItem.UserStatus.OffLine;

                    onlineUserId = newOnlineUserId;
                    offlineUserId = newOfflineUserId;

                    Thread.Sleep(2000);
                }
            }));
            t.Start();
        }
        public frm_group(Class1.Group group)
        {
            InitializeComponent();
            this.Text = $"群聊会话 {group.Name}({group.Id})";
            groupChat = group;
            login();
            LoadMessage();
        }
        private void LoadMessage()
        {
            long lastReadMsgId = long.MaxValue;
            if(Class1.IsOnline)
            {
                var req = new http._Group_User.GetGroupLastReadMsgIdReq()
                {
                    UserId = Class1.UserId
                };
                var resp = http._Group_User.GetGroupLastReadMsgId(req);
                lastReadMsgId = resp.LastReadMsgId[groupChat.Id];
            }

            DataTable dt;
            dt = Class1.sql.SqlTable($"SELECT count(*) as count FROM `message` WHERE `id` > {lastReadMsgId} AND `group_id` = {groupChat.Id}");
            if (dt == null || dt.Rows.Count != 1)
                throw new Exception("select count(*) from message failed");
            var unReadCount = long.Parse(dt.Rows[0]["count"].ToString());

            //加载历史消息，数量为page_size和未读消息数的较大者
            dt = Class1.sql.SqlTable($"SELECT id, user_id_send, content, create_time, content_type FROM `message` WHERE `group_id` = {groupChat.Id} ORDER BY `id` DESC LIMIT 0,{Math.Max(unReadCount, Class1.PageSize)}");
            dt.DefaultView.Sort = "id ASC";
            dt = dt.DefaultView.ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var id = (long)dt.Rows[i]["id"];
                var userIdSend = (long)dt.Rows[i]["user_id_send"];
                var content = System.Text.Encoding.Default.GetString(Class1.UnGzip((byte[])dt.Rows[i]["content"]));
                var createTime = (long)dt.Rows[i]["create_time"];
                var contentType = (long)dt.Rows[i]["content_type"];
                if (id > lastReadMsgId && (i == 0 || (long)dt.Rows[i-1]["id"] <= lastReadMsgId))
                    cls_group.appendCenteralText(this, "以下为新消息");
                switch (contentType)
                {
                    case (int)Class1.ContentType.Text:
                        {
                            cls_group.appendRtfToMsgBox(this, userIdSend.ToString(), Class1.StampToDateTime(createTime), content);
                            break;
                        }
                    case (int)Class1.ContentType.Object:
                        {
                            var objId = long.Parse(content);
                            DataTable dt1 = Class1.sql.SqlTable($"SELECT e_tag, name FROM `object` WHERE `id` = {objId}");
                            if (dt1 == null || dt1.Rows.Count != 1)
                                throw new Exception("rowCount != 1, rowCount = " + dt1.Rows.Count);
                            cls_group.appendFileToMsgBox(this, userIdSend.ToString(), Class1.StampToDateTime(createTime), "[文件]" + dt1.Rows[0]["name"].ToString(), objId);
                            break;
                        }
                    case (int)Class1.ContentType.Image:
                        {
                            var eTag = content;
                            DataTable dt1 = Class1.sql.SqlTable($"SELECT content FROM `image` WHERE `e_tag` = \"{eTag}\"");
                            if (dt1 == null || dt1.Rows.Count != 1)
                                throw new Exception("rowCount != 1, rowCount = " + dt1.Rows.Count);
                            var imageData = Class1.UnGzip((byte[])dt1.Rows[0]["content"]);
                            var stream = cls_group.appendImageToMsgBox(this, userIdSend.ToString(), Class1.StampToDateTime(createTime), imageData);
                            imagePool.Add(stream);
                            break;
                        }
                    default:
                            throw new Exception("unknown content_type, content_type = " + contentType);
                }
            }
        }
        private void frm_group_Load(object sender, EventArgs e)
        {

        }

        private void frm_group_FormClosed(object sender, FormClosedEventArgs e)
        {
            Class1.formGroupPool.Remove(groupChat.Id);
            foreach (var image in imagePool)
                image.Dispose();
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
                IdRecv = groupChat.Id,
                Content = content,
                ContentType = (int)Class1.ContentType.Text,
                MessageType = (int)Class1.MsgType.GroupChat
            };
            var resp = http._Message.SendMessage(req);
            var statusCode = resp.StatusCode;

            switch (statusCode)
            {
                case 1:
                    {
                        cls_group.appendSysMsgToMsgBox(this, "你尚未加入群聊，无法发送消息", DateTime.Now);
                        return;
                    }
            }

            var param = Class1.Gzip(System.Text.Encoding.Default.GetBytes(content));
            bool ret = Class1.sql.ExecuteNonQueryWithBinary($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type) VALUES({resp.Id}, " +
                            $"{Class1.UserId}, 0, {groupChat.Id}, @param, {resp.CreateTime}, {(int)Class1.ContentType.Text}, {(int)Class1.MsgType.GroupChat})", param);
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            cls_group.appendRtfToMsgBox(this, Class1.UserId.ToString(), DateTime.Now, content);
            rtxt_send.Rtf = string.Empty;
        }

        private void toolFont_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == fontDialog1.ShowDialog())
            {
                rtxt_send.Font = fontDialog1.Font;
                rtxt_send.ForeColor = fontDialog1.Color;
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
                //InvokeProgressBarSetValue(pgb_upload, 0);
                cls_group.UploadImage(localFileName, groupChat.Id, this);
                //InvokeProgressBarSetValue(pgb_upload, 100);
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
                    IdRecv = groupChat.Id,
                    Content = objId.ToString(),
                    ContentType = (int)Class1.ContentType.Object,
                    MessageType = (int)Class1.MsgType.GroupChat
                };
                var sendMessageResp = http._Message.SendMessage(sendMessageReq);
                var statusCode = resp.StatusCode;

                switch (statusCode)
                {
                    case 1:
                        {
                            cls_group.appendSysMsgToMsgBox(this, "你尚未加入群聊，无法发送消息", DateTime.Now);
                            return;
                        }
                }

                var messageId = sendMessageResp.Id;
                var createTime = sendMessageResp.CreateTime;
                var param = Class1.Gzip(System.Text.Encoding.Default.GetBytes(objId.ToString()));
                var ret = Class1.sql.ExecuteNonQueryWithBinary($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type) VALUES({messageId}, " +
                            $"{Class1.UserId}, 0, {groupChat.Id}, @param, {createTime}, {(int)Class1.ContentType.Object}, {(int)Class1.MsgType.GroupChat})", param);
                if (!ret)
                {
                    MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                cls_group.appendFileToMsgBox(this, Class1.UserId.ToString(), Class1.StampToDateTime(createTime), "[文件]" + name, objId);
            }));
            t.Start();
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
                Invoke(new Action(() =>
                {
                    pgb_download.Value = 0;
                    pgb_download.Visible = true;
                }));
                DownloadFile(eTag, localFileName);
            }
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

        private void clb_member_DoubleClickSubItem(object sender, ChatListEventArgs e, MouseEventArgs es)
        {
            var userId = (long)e.SelectSubItem.ID;
            if (userId == Class1.UserId)
                return;
            frm_chat frm_chat;
            if (Class1.formChatPool.ContainsKey(userId))
            {
                frm_chat = Class1.formChatPool[userId];
                frm_chat.Activate();
            }
            else
            {
                frm_chat = new frm_chat(userId);
                Class1.formChatPool.Add(userId, frm_chat);
                frm_chat.Show();
            }
            if (Class1.unReadPrivateMsgItemPool.ContainsKey(userId))
            {
                var updateFriendLastReadMsgIdReq = new http._User_Relation.UpdateFriendLastReadMsgIdReq()
                {
                    UserId = Class1.UserId,
                    LastReadMsgId = new Dictionary<long, long>()
                };
                var subItem = Class1.unReadPrivateMsgItemPool[userId];
                var ids = ((List<long>)(((Dictionary<string, object>)subItem.Tag)["ids"]));
                foreach (var id in ids)
                {
                    if (updateFriendLastReadMsgIdReq.LastReadMsgId.ContainsKey(userId))
                        updateFriendLastReadMsgIdReq.LastReadMsgId[userId] = Math.Max(updateFriendLastReadMsgIdReq.LastReadMsgId[userId], id);
                    else
                        updateFriendLastReadMsgIdReq.LastReadMsgId.Add(userId, id);
                }
                http._User_Relation.UpdateFriendLastReadMsgId(updateFriendLastReadMsgIdReq);

                Class1.frmMsgBox.clb_unread_msg.Items[0].SubItems.Remove(subItem);
                Class1.unReadPrivateMsgItemPool.Remove(userId);
                icon.ChangeIconState();
            }
        }

        private void clb_member_UpSubItem(object sender, ChatListClickEventArgs e, MouseEventArgs es)
        {
            if (es.Button != MouseButtons.Right)
                return;
            var userId = e.SelectSubItem.ID;
            if (Class1.UserId != groupChat.Owner || userId == Class1.UserId)
                return;
            selectId = userId;
            mnu_strip1.Show(MousePosition.X, MousePosition.Y);
        }

        private void mnu_quit_group_Click(object sender, EventArgs e)
        {
            var ret = MessageBox.Show($"确定将{selectId}移出群聊吗?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (ret != DialogResult.Yes)
                return;
            var req = new http._Group_User.DeleteMemberReq()
            {
                GroupId = groupChat.Id,
                UserId = selectId
            };
            http._Group_User.DeleteMember(req);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnu_invite_group_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var frmInviteGroup = new frm_invite_group(groupChat.Id);
            frmInviteGroup.ShowDialog();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var frmInviteGroup = new frm_invite_group(groupChat.Id);
            frmInviteGroup.ShowDialog();
        }

        private void clb_member_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;
            contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
        }

        private void toolStripDropDownButton4_ButtonClick(object sender, EventArgs e)
        {
            frm_msg_view frmMsgView;
            if (Class1.formMsgViewGroupPool.ContainsKey(groupChat.Id))
            {
                frmMsgView = Class1.formMsgViewGroupPool[groupChat.Id];
                frmMsgView.Activate();
            }
            else
            {
                frmMsgView = new frm_msg_view(groupChat.Id, "group");
                Class1.formMsgViewGroupPool.Add(groupChat.Id, frmMsgView);
                frmMsgView.Show();
            }
        }
    }
}
