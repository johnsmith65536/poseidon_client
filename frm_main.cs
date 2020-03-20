﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using StackExchange.Redis;
using Poseidon.infra.redis;
using Newtonsoft.Json;
using CCWin.SkinControl;
using Aliyun.OSS;
using Aliyun.OSS.Common;
using System.IO;

namespace Poseidon
{
    public partial class frm_main : Form
    {
        private long delFriendUserId;
        private int tick;
        private enum BroadcastMsgType
        {
            Chat = 0,
            AddFriend = 1,
            ReplyAddFriend = 2,
            AddGroup = 3,
            ReplyAddGroup = 4,
            InviteAddGroup = 5,
            ReplyInviteAddGroup = 6,
        }
        private struct RedisAddFriend
        {
            public long Id;
            public long UserIdSend;
            public long UserIdRecv;
            public long CreateTime;
        };
        private struct RedisReplyAddFriend
        {
            public long Id;
            public long ParentId;
            public long UserIdSend;
            public long UserIdRecv;
            public long CreateTime;
            public int Status;
        };

        private struct RedisMessage
        {
            public long Id;
            public long UserIdSend;
            public long UserIdRecv;
            public long GroupId;
            public string Content;
            public long CreateTime;
            public int ContentType;
            public int MsgType;
            public int IsRead;
            public string ObjectETag;
            public string ObjectName;
        };
        private struct RedisAddGroup
        {
            public long Id;
            public long UserIdSend;
            public long GroupId;
            public long CreateTime;
        }
        private struct RedisReplyAddGroup
        {
            public long Id;
            public long ParentId;
            public long UserIdSend;
            public long UserIdRecv;
            public long GroupId;
            public long CreateTime;
            public int Status;
        }

        public frm_main()
        {
            InitializeComponent();
        }
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
        private void InvokeGridClear(DataGridView dgv)
        {
            if (dgv.InvokeRequired)
            {
                GridClearCallBack stcb = new GridClearCallBack(InvokeGridClear);
                this.Invoke(stcb, new object[] { dgv });
            }
            else
                dgv.Rows.Clear();
        }

        public void LoadMain()
        {
            ChatListBox.Items.Clear();
            ChatListBox.Items.Add(new ChatListItem("在线", true));
            ChatListBox.Items.Add(new ChatListItem("离线", true));

            Class1.chatListSubItemPool.Clear();
            Class1.onlineUserId.Clear();
            Class1.offlineUserId.Clear();


            Class1.frmMsgBox.clb_unread_msg.Items.Clear();
            Class1.frmMsgBox.clb_unread_msg.Items.Add(new ChatListItem("个人消息", true));
            Class1.frmMsgBox.clb_unread_msg.Items.Add(new ChatListItem("系统消息", true));
            Class1.frmMsgBox.clb_unread_msg.Items.Add(new ChatListItem("群消息", true));


            Class1.groupItemPool.Clear();
            ChatListBox.Items.Add(new ChatListItem("群组", true));


            Thread t1 = new Thread(new ThreadStart(() =>
            {
                while (Class1.IsOnline)
                {
                    /*
                     * 拉取好友列表
                     */
                    var req = new http._Relation.FetchFriendListReq()
                    {
                        UserId = Class1.UserId
                    };
                    var resp = http._Relation.FetchFriendList(req);
                    if (resp.StatusCode == 254)
                        return;

                    var oldOnlineUserId = Class1.onlineUserId;
                    var oldOfflineUserId = Class1.offlineUserId;
                    var oldTotalUserId = new HashSet<long>();
                    oldTotalUserId.UnionWith(oldOnlineUserId);
                    oldTotalUserId.UnionWith(oldOfflineUserId);

                    var newOnlineUserId = new HashSet<long>();
                    var newOfflineUserId = new HashSet<long>();
                    var newTotalUserId = new HashSet<long>();

                    foreach (var userId in resp.OnlineUserIds)
                        newOnlineUserId.Add(userId);
                    foreach (var userId in resp.OfflineUserIds)
                        newOfflineUserId.Add(userId);

                    newTotalUserId.UnionWith(newOnlineUserId);
                    newTotalUserId.UnionWith(newOfflineUserId);

                    //分配新增的subItem
                    foreach (var userId in newTotalUserId)
                        if (!oldTotalUserId.Contains(userId))
                        {
                            var subItem = new ChatListSubItem(userId.ToString());
                            subItem.ID = (uint)userId;
                            Class1.chatListSubItemPool.Add(userId, subItem);
                        }

                    //状态转移
                    foreach (var userId in oldOnlineUserId)
                        if (!newOnlineUserId.Contains(userId))
                            Invoke(new Action(() =>
                            {
                                ChatListBox.Items[0].SubItems.Remove(Class1.chatListSubItemPool[userId]);
                            }));
                    foreach (var userId in newOnlineUserId)
                        if (!oldOnlineUserId.Contains(userId))
                            Invoke(new Action(() =>
                            {
                                ChatListBox.Items[0].SubItems.Add(Class1.chatListSubItemPool[userId]);
                            }));


                    foreach (var userId in oldOfflineUserId)
                        if (!newOfflineUserId.Contains(userId))
                            Invoke(new Action(() =>
                            {
                                ChatListBox.Items[1].SubItems.Remove(Class1.chatListSubItemPool[userId]);
                            }));

                    foreach (var userId in newOfflineUserId)
                        if (!oldOfflineUserId.Contains(userId))
                            Invoke(new Action(() =>
                            {
                                ChatListBox.Items[1].SubItems.Add(Class1.chatListSubItemPool[userId]);
                            }));
                    Class1.onlineUserId = newOnlineUserId;
                    Class1.offlineUserId = newOfflineUserId;



                    //销毁删除的subItem
                    foreach (var userId in oldTotalUserId)
                        if (!newTotalUserId.Contains(userId))
                            Class1.chatListSubItemPool.Remove(userId);
                    icon.ChangeIconState();

                    /*
                     * 拉取群组列表
                     */
                    var fetchGroupListReq = new http._Group_User.FetchGroupListReq()
                    {
                        UserId = Class1.UserId
                    };
                    var fetchGroupListResp = http._Group_User.FetchGroupList(fetchGroupListReq);
                    if (resp.StatusCode == 254)
                        return;

                    var oldGroupIds = new HashSet<long>();
                    var newGroupIds = new HashSet<long>();

                    foreach (var item in Class1.groupItemPool)
                        oldGroupIds.Add(item.Key);

                    foreach (var groupId in fetchGroupListResp.GroupIds)
                        newGroupIds.Add(groupId);

                    foreach (var groupId in oldGroupIds)
                    {
                        if (!newGroupIds.Contains(groupId))
                        {
                            var subItem = Class1.groupItemPool[groupId];
                            Invoke(new Action(() =>
                            {
                                ChatListBox.Items[2].SubItems.Remove(subItem);
                            }));
                            Class1.groupItemPool.Remove(groupId);
                        }
                    }

                    foreach (var groupId in newGroupIds)
                    {
                        if (!oldGroupIds.Contains(groupId))
                        {
                            var subItem = new ChatListSubItem(groupId.ToString());
                            subItem.ID = (uint)groupId;
                            Invoke(new Action(() =>
                            {
                                ChatListBox.Items[2].SubItems.Add(subItem);
                            }));
                            Class1.groupItemPool.Add(groupId, subItem);
                        }
                    }



                    /*
                     * 拉取远程request.status
                     */
                    var dt = Class1.sql.SqlTable($"SELECT id, user_id_send, create_time, parent_id FROM `user_relation_request` WHERE `user_id_recv` = {Class1.UserId} AND `status` = 0 AND `parent_id` != -1");

                    // 同步最新UserRelationRequest & GroupUserRequest的status，message暂不同步
                    List<long> userRelationQueryIds = new List<long>();
                    List<long> groupUserQueryIds = new List<long>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var parentId = long.Parse(row["parent_id"].ToString());
                        userRelationQueryIds.Add(parentId);
                    }

                    dt = Class1.sql.SqlTable($"SELECT id, user_id_send, create_time, parent_id FROM `group_user_request` WHERE `user_id_recv` = {Class1.UserId} AND `status` = 0 AND `parent_id` != -1");
                    foreach (DataRow row in dt.Rows)
                    {
                        var parentId = long.Parse(row["parent_id"].ToString());
                        groupUserQueryIds.Add(parentId);
                    }

                    var fetchMessageStatusReq = new http._Message.FetchMessageStatusReq()
                    {
                        MessageIds = new List<long>(),
                        UserRelationRequestIds = userRelationQueryIds,
                        GroupUserRequestIds = groupUserQueryIds
                    };
                    var fetchMessageStatusResp = http._Message.FetchMessageStatus(fetchMessageStatusReq);
                    if (fetchMessageStatusResp.StatusCode == 254)
                        return;
                    foreach (var item in fetchMessageStatusResp.UserRelationRequestIds)
                    {
                        var id = item.Key;
                        var status = item.Value;
                        var ret = Class1.sql.ExecuteNonQuery($"UPDATE `user_relation_request` SET status = {status} WHERE id = {id}");
                        if (!ret)
                        {
                            MessageBox.Show("DB错误，UPDATE user_relation_request失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    foreach (var item in fetchMessageStatusResp.GroupUserRequestIds)
                    {
                        var id = item.Key;
                        var status = item.Value;
                        var ret = Class1.sql.ExecuteNonQuery($"UPDATE `group_user_request` SET status = {status} WHERE id = {id}");
                        if (!ret)
                        {
                            MessageBox.Show("DB错误，UPDATE group_user_request失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    Thread.Sleep(2 * 1000);
                };
            }));
            t1.Start();

            Thread t2 = new Thread(new ThreadStart(() =>    //异步拉取消息
            {
                var fetchGroupListReq = new http._Group_User.FetchGroupListReq()
                {
                    UserId = Class1.UserId
                };
                var fetchGroupListResp = http._Group_User.FetchGroupList(fetchGroupListReq);
                long messageId = -1, userRelationId = -1, groupUserId = -1;
                DataTable dt = Class1.sql.SqlTable($"SELECT count(*) as count, MAX(id) as id FROM `message` WHERE (`user_id_send` = {Class1.UserId} OR `user_id_recv` = {Class1.UserId} OR `group_id` IN ({String.Join(",", fetchGroupListResp.GroupIds)}))");
                if (dt != null && dt.Rows.Count == 1 && (long)dt.Rows[0]["count"] > 0)
                    messageId = (long)dt.Rows[0]["id"];
                dt = Class1.sql.SqlTable($"SELECT count(*) as count, MAX(id) as id FROM `user_relation_request` WHERE (`user_id_send` = {Class1.UserId} OR `user_id_recv` = {Class1.UserId})");
                if (dt != null && dt.Rows.Count == 1 && (long)dt.Rows[0]["count"] > 0)
                    userRelationId = (long)dt.Rows[0]["id"];
                dt = Class1.sql.SqlTable($"SELECT count(*) as count, MAX(id) as id FROM `group_user_request`");
                if (dt != null && dt.Rows.Count == 1 && (long)dt.Rows[0]["count"] > 0)
                    groupUserId = (long)dt.Rows[0]["id"];
                var req = new http._Message.SyncMessageReq()
                {
                    UserId = Class1.UserId,
                    MessageId = messageId,
                    UserRelationId = userRelationId,
                    GroupUserId = groupUserId
                };
                var resp = http._Message.SyncMessage(req);
                var messages = resp.Messages;
                var userRelations = resp.UserRelations;
                var objects = resp.Objects;
                var groupUsers = resp.GroupUsers;
                //消息落库

                foreach (var obj in objects)
                    Class1.InsertObject(obj.Id, obj.ETag, obj.Name);

                foreach (var msg in messages)
                {
                    if (msg.ContentType == (int)Class1.ContentType.Image)
                        Class1.FetchImage(msg.Content);
                    var param = Class1.Gzip(System.Text.Encoding.Default.GetBytes(msg.Content));
                    bool ret1 = Class1.sql.ExecuteNonQueryWithBinary($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type, is_read) VALUES({msg.Id}, " +
                        $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.GroupId}, @param, {msg.CreateTime}, {msg.ContentType}, {msg.MsgType}, {msg.IsRead})", param);
                    if (!ret1)
                    {
                        MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                foreach (var msg in userRelations)
                {
                    bool ret1 = Class1.sql.ExecuteNonQuery($"INSERT INTO `user_relation_request`(id, user_id_send, user_id_recv, create_time, status, parent_id) VALUES({msg.Id}, " +
                        $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.CreateTime}, {msg.Status}, {msg.ParentId})");
                    if (!ret1)
                    {
                        MessageBox.Show("DB错误，INSERT INTO user_relation_request失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                foreach (var msg in groupUsers)
                {
                    bool ret1 = Class1.sql.ExecuteNonQuery($"INSERT INTO `group_user_request`(id, user_id_send, user_id_recv, group_id, create_time, status, parent_id, type) VALUES({msg.Id}, " +
                        $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.GroupId}, {msg.CreateTime}, {msg.Status}, {msg.ParentId}, {msg.Type})");
                    if (!ret1)
                    {
                        MessageBox.Show("DB错误，INSERT INTO group_user_request失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }


                Class1.LoadUnReadMessage();

                Console.WriteLine("fetch offline message done");
            }));
            t2.Start();

            redis.Subscribe("poseidon_heart_beat_channel", HeartBeatChannel);
            redis.Subscribe("poseidon_message_channel_" + Class1.UserId, MessageChannel);
            Class1.UpdateStatusCheckBox(Class1.IsOnline);
            Class1.InvokeToolStripStatusLabel(toolStripStatusLabel1, "在线");
            toolStripStatusLabel2.Text = Class1.FormatDateTime(DateTime.Now);
            notifyIcon1.Text = Class1.UserId.ToString();

            foreach (var item in Class1.formGroupPool)
            {
                var frmGroup = item.Value;
                frmGroup.login();
            }
        }
        private void frm_main_Load(object sender, EventArgs e)
        {
            Class1.frmMsgBox = new frm_msg_box();
            Class1.frmMsgBox.Show();
            Class1.frmMsgBox.Hide();
            LoadMain();
        }
        public void MessageChannel(RedisChannel cnl, RedisValue val)
        {
            ConnectionMultiplexer redisCli = redis.GetRedisConn();
            Console.WriteLine("频道：" + cnl + "\t收到消息:" + val);

            var broadcastMsgType = GetBroadcastMsgType(val.ToString());
            switch (broadcastMsgType)
            {
                case BroadcastMsgType.Chat:
                    {
                        var msg = JsonConvert.DeserializeObject<RedisMessage>(val.ToString());
                        var param = Class1.Gzip(System.Text.Encoding.Default.GetBytes(msg.Content));
                        bool ret = Class1.sql.ExecuteNonQueryWithBinary($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type, is_read) VALUES({msg.Id}, " +
                            $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.GroupId}, @param, {msg.CreateTime}, {msg.ContentType}, {msg.MsgType}, {msg.IsRead})", param);
                        if (!ret)
                        {
                            MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        byte[] imageData = null;
                        if (msg.ContentType == (int)Class1.ContentType.Image)
                            imageData = Class1.FetchImage(msg.Content);
                        switch (msg.MsgType)
                        {
                            case (int)Class1.MsgType.PrivateChat:
                                {
                                    if (Class1.formChatPool.ContainsKey(msg.UserIdSend))
                                    {
                                        var frm_chat = Class1.formChatPool[msg.UserIdSend];
                                        switch (msg.ContentType)
                                        {
                                            case (int)Class1.ContentType.Text:
                                                {
                                                    Class1.appendRtfToMsgBox(frm_chat, msg.UserIdSend.ToString(), Class1.StampToDateTime(msg.CreateTime), msg.Content);
                                                    break;
                                                }
                                            case (int)Class1.ContentType.Object:
                                                {
                                                    Class1.appendFileToMsgBox(frm_chat, msg.UserIdSend.ToString(), Class1.StampToDateTime(msg.CreateTime), "[文件]" + msg.ObjectName, long.Parse(msg.Content));
                                                    break;
                                                }
                                            case (int)Class1.ContentType.Vibration:
                                                {
                                                    Class1.appendSysMsgToMsgBox(frm_chat, "你" + (msg.UserIdSend == Class1.UserId ? "发送" : "收到") + "了一个窗口抖动。\r\n", Class1.StampToDateTime(msg.CreateTime));
                                                    Class1.Vibration(frm_chat);
                                                    break;
                                                }
                                            case (int)Class1.ContentType.Image:
                                                {
                                                    Class1.appendImageToMsgBox(frm_chat, msg.UserIdSend.ToString(), Class1.StampToDateTime(msg.CreateTime), imageData);
                                                    break;
                                                }
                                            default:
                                                {
                                                    Console.WriteLine("unknown content_type content_type = ", msg.ContentType);
                                                    break;
                                                }
                                        }
                                        //消息已读
                                        Class1.UpdateMessageStatus(new Dictionary<long, int> { { msg.Id, 1 } }, new Dictionary<long, int>(), new Dictionary<long, int>());
                                    }
                                    else
                                    {
                                        switch (msg.ContentType)
                                        {
                                            case (int)Class1.ContentType.Text:
                                                {
                                                    Class1.appendPersonalMsgToUnReadBox(msg.Id, msg.UserIdSend, Class1.rtfToText(msg.Content));
                                                    break;
                                                }
                                            case (int)Class1.ContentType.Object:
                                                {
                                                    var objId = long.Parse(msg.Content);
                                                    Class1.InsertObject(objId, msg.ObjectETag, msg.ObjectName);
                                                    Class1.appendPersonalMsgToUnReadBox(msg.Id, msg.UserIdSend, "[文件]" + msg.ObjectName);
                                                    break;
                                                }
                                            case (int)Class1.ContentType.Vibration:
                                                {
                                                    Class1.appendPersonalMsgToUnReadBox(msg.Id, msg.UserIdSend, "你收到了一个窗口抖动");
                                                    break;
                                                }
                                            case (int)Class1.ContentType.Image:
                                                {
                                                    Class1.appendPersonalMsgToUnReadBox(msg.Id, msg.UserIdSend, "[图片]");
                                                    break;
                                                }
                                            default:
                                                {
                                                    Console.WriteLine("unknown content_type content_type = ", msg.ContentType);
                                                    break;
                                                }
                                        }
                                    }
                                    break;
                                }
                            case (int)Class1.MsgType.GroupChat:
                                {
                                    if (Class1.formGroupPool.ContainsKey(msg.GroupId))
                                    {
                                        var frmGroup = Class1.formGroupPool[msg.GroupId];
                                        switch (msg.ContentType)
                                        {
                                            case (int)Class1.ContentType.Text:
                                                {
                                                    cls_group.appendRtfToMsgBox(frmGroup, msg.UserIdSend.ToString(), Class1.StampToDateTime(msg.CreateTime), msg.Content);
                                                    break;
                                                }
                                            case (int)Class1.ContentType.Object:
                                                {
                                                    cls_group.appendFileToMsgBox(frmGroup, msg.UserIdSend.ToString(), Class1.StampToDateTime(msg.CreateTime), "[文件]" + msg.ObjectName, long.Parse(msg.Content));
                                                    break;
                                                }
                                            case (int)Class1.ContentType.Image:
                                                {
                                                    cls_group.appendImageToMsgBox(frmGroup, msg.UserIdSend.ToString(), Class1.StampToDateTime(msg.CreateTime), imageData);
                                                    break;
                                                }
                                            default:
                                                throw new Exception("unknown content_type content_type = " + msg.ContentType);
                                        }
                                        //消息已读
                                        //Class1.UpdateMessageStatus(new Dictionary<long, int> { { msg.Id, 1 } }, new Dictionary<long, int>());
                                        var req = new http._Group_User.UpdateLastReadMsgIdReq()
                                        {
                                            UserId = Class1.UserId,
                                            LastReadMsgId = new Dictionary<long, long>() { { msg.GroupId, msg.Id } }
                                        };
                                        http._Group_User.UpdateLastReadMsgId(req);
                                    }
                                    else
                                    {
                                        switch (msg.ContentType)
                                        {
                                            case (int)Class1.ContentType.Text:
                                                {
                                                    Class1.appendGroupMsgToUnReadBox(msg.Id, msg.UserIdSend, msg.GroupId, Class1.rtfToText(msg.Content));
                                                    break;
                                                }
                                            case (int)Class1.ContentType.Object:
                                                {
                                                    var objId = long.Parse(msg.Content);
                                                    Class1.InsertObject(objId, msg.ObjectETag, msg.ObjectName);
                                                    Class1.appendGroupMsgToUnReadBox(msg.Id, msg.UserIdSend, msg.GroupId, "[文件]" + msg.ObjectName);
                                                    break;
                                                }
                                            case (int)Class1.ContentType.Image:
                                                {
                                                    Class1.appendGroupMsgToUnReadBox(msg.Id, msg.UserIdSend, msg.GroupId, "[图片]");
                                                    break;
                                                }
                                            default:
                                                throw new Exception("unknown content_type content_type = " + msg.ContentType);
                                        }
                                    }
                                    break;
                                }
                            default:
                                throw new Exception("unknown msg_type");
                        }
                        break;
                    }
                case BroadcastMsgType.AddFriend:
                    {
                        var msg = JsonConvert.DeserializeObject<RedisAddFriend>(val.ToString());
                        bool ret = Class1.sql.ExecuteNonQuery($"INSERT INTO `user_relation_request`(id, user_id_send, user_id_recv, create_time, status, parent_id) VALUES({msg.Id}, " +
                            $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.CreateTime}, 0, -1)");
                        if (!ret)
                        {
                            MessageBox.Show("DB错误，INSERT INTO user_relation_request失败888", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        Class1.appendSystemMsgToUnReadBox(msg.Id, msg.UserIdSend, "来自" + msg.UserIdSend + "的好友请求", Class1.UnReadMsgType.AddFriend, new Dictionary<string, object>());
                        break;
                    }
                case BroadcastMsgType.ReplyAddFriend:
                    {
                        var msg = JsonConvert.DeserializeObject<RedisReplyAddFriend>(val.ToString());
                        bool ret = Class1.sql.ExecuteNonQuery($"INSERT INTO `user_relation_request`(id, user_id_send, user_id_recv, create_time, status, parent_id) VALUES({msg.Id}, " +
                            $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.CreateTime}, 0, {msg.ParentId})");
                        if (!ret)
                        {
                            MessageBox.Show("DB错误，INSERT INTO user_relation_request失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        ret = Class1.sql.ExecuteNonQuery($"UPDATE `user_relation_request` SET status = {msg.Status} WHERE id = {msg.ParentId}");
                        if (!ret)
                        {
                            MessageBox.Show("DB错误，UPDATE user_relation_request失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        Class1.appendSystemMsgToUnReadBox(msg.Id, msg.UserIdSend, msg.UserIdSend + (msg.Status == (long)Class1.AddFriendStatus.Accepted ? "通过" : "拒绝") + "了好友请求", Class1.UnReadMsgType.ReplyAddFriend, new Dictionary<string, object>());
                        break;
                    }
                case BroadcastMsgType.AddGroup:
                    {
                        var msg = JsonConvert.DeserializeObject<RedisAddGroup>(val.ToString());
                        bool ret = Class1.sql.ExecuteNonQuery($"INSERT INTO `group_user_request`(id, user_id_send, user_id_recv, group_id, create_time, status, parent_id, type) VALUES({msg.Id}, " +
                            $"{msg.UserIdSend}, {0}, {msg.GroupId}, {msg.CreateTime}, 0, -1, {(int)Class1.GroupUserRequestType.AddGroup})");
                        if (!ret)
                        {
                            MessageBox.Show("DB错误，INSERT INTO group_user_request失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        Class1.appendSystemMsgToUnReadBox(msg.Id, msg.UserIdSend, msg.UserIdSend + "请求加入群聊" + msg.GroupId, Class1.UnReadMsgType.AddGroup, new Dictionary<string, object>() { { "group_id", msg.GroupId } });
                        break;
                    }
                case BroadcastMsgType.ReplyAddGroup:
                    {
                        var msg = JsonConvert.DeserializeObject<RedisReplyAddGroup>(val.ToString());
                        bool ret = Class1.sql.ExecuteNonQuery($"INSERT INTO `group_user_request`(id, user_id_send, user_id_recv, group_id, create_time, status, parent_id, type) VALUES({msg.Id}, " +
                            $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.GroupId}, {msg.CreateTime}, 0, {msg.ParentId}, {(int)Class1.GroupUserRequestType.AddGroup})");
                        if (!ret)
                        {
                            MessageBox.Show("DB错误，INSERT INTO group_user_request失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        ret = Class1.sql.ExecuteNonQuery($"UPDATE `group_user_request` SET status = {msg.Status} WHERE id = {msg.ParentId}");
                        if (!ret)
                        {
                            MessageBox.Show("DB错误，UPDATE group_user_request失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        Class1.appendSystemMsgToUnReadBox(msg.Id, msg.UserIdSend, msg.UserIdSend + (msg.Status == (long)Class1.AddFriendStatus.Accepted ? "通过" : "拒绝") + "了加群请求", Class1.UnReadMsgType.ReplyAddGroup, new Dictionary<string, object>());
                        break;
                    }
                case BroadcastMsgType.InviteAddGroup:
                    {
                        break;
                    }
                case BroadcastMsgType.ReplyInviteAddGroup:
                    {
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Unknown BroadcastMsgType, type: " + broadcastMsgType);

                        break;
                    }
            }

            icon.ChangeIconState();

            Console.WriteLine("线程：" + Thread.CurrentThread.ManagedThreadId + ",是否线程池：" + Thread.CurrentThread.IsThreadPoolThread);
        }
        public void HeartBeatChannel(RedisChannel cnl, RedisValue val)
        {
            var req = new http._Heart_Beat.HeartBeatReq()
            {
                UserId = Class1.UserId
            };
            http._Heart_Beat.HeartBeat(req);
            Console.WriteLine("Send HeartBeat");
            // Console.WriteLine("线程：" + Thread.CurrentThread.ManagedThreadId + ",是否线程池：" + Thread.CurrentThread.IsThreadPoolThread);
        }
        private void frm_main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Class1.IsOnline)
            {
                var req = new http._Login.LogoutReq()
                {
                    UserId = Class1.UserId,
                    AccessToken = Class1.AccessToken
                };
                http._Login.Logout(req);
            }
            notifyIcon1.Dispose();
            Environment.Exit(0);
        }
        private static BroadcastMsgType GetBroadcastMsgType(string jsonString)
        {
            var obj = JsonConvert.DeserializeObject<dynamic>(jsonString);
            BroadcastMsgType type = obj.BroadcastMsgType;
            return type;
        }

        private void mnu_add_friend_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            frm_search_user frm_search_user = new frm_search_user();
            frm_search_user.ShowDialog();
        }
        private void mnu_del_friend_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var req = new http._Relation.DeleteFriendReq()
            {
                UserIdSend = Class1.UserId,
                UserIdRecv = delFriendUserId
            };
            http._Relation.DeleteFriend(req);
            MessageBox.Show("好友删除成功", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void mnu_online_Click(object sender, EventArgs e)
        {
            if (Class1.IsOnline)
                return;
            var ok = Class1.Login(Class1.UserId, Class1.Password);
            if (!ok)
            {
                MessageBox.Show("登陆失败，账号或密码有误", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadMain();
            Class1.UpdateStatusCheckBox(true);
        }

        private void mnu_offline_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
                return;

            Class1.Logout(Class1.UserId, Class1.AccessToken);

            Class1.UpdateStatusCheckBox(false);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = Class1.FormatDateTime(DateTime.Now);
        }

        private void ChatListBox_DoubleClickSubItem(object sender, ChatListEventArgs e, MouseEventArgs es)
        {
            if (es.Button != MouseButtons.Left)
                return;
            var itemId = (long)e.SelectSubItem.ID;

            if (Class1.chatListSubItemPool.ContainsKey(itemId) && Class1.chatListSubItemPool[itemId] == e.SelectSubItem)
            {
                var userId = itemId;
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
                    Dictionary<long, int> readMessage = new Dictionary<long, int>();
                    var subItem = Class1.unReadPrivateMsgItemPool[userId];
                    var ids = ((List<long>)(((Dictionary<string, object>)subItem.Tag)["ids"]));
                    foreach (var id in ids)
                        readMessage.Add(id, 1);
                    Class1.UpdateMessageStatus(readMessage, new Dictionary<long, int>(), new Dictionary<long, int>());

                    Class1.frmMsgBox.clb_unread_msg.Items[0].SubItems.Remove(subItem);
                    Class1.unReadPrivateMsgItemPool.Remove(userId);
                    icon.ChangeIconState();
                }
            }
            else if (Class1.groupItemPool.ContainsKey(itemId) && Class1.groupItemPool[itemId] == e.SelectSubItem)
            {
                var groupId = itemId;
                frm_group frmGroup;
                if (Class1.formGroupPool.ContainsKey(groupId))
                {
                    frmGroup = Class1.formGroupPool[groupId];
                    frmGroup.Activate();
                }
                else
                {
                    frmGroup = new frm_group(groupId);
                    Class1.formGroupPool.Add(groupId, frmGroup);
                    frmGroup.Show();
                }
                if (Class1.unReadGroupMsgItemPool.ContainsKey(groupId))
                {
                    Dictionary<long, int> readMessage = new Dictionary<long, int>();
                    var subItem = Class1.unReadGroupMsgItemPool[groupId];
                    var maxId = (long)((Dictionary<string, object>)subItem.Tag)["max_id"];
                    //Class1.UpdateMessageStatus(readMessage, new Dictionary<long, int>());
                    var req = new http._Group_User.UpdateLastReadMsgIdReq()
                    {
                        UserId = Class1.UserId,
                        LastReadMsgId = new Dictionary<long, long>() { { groupId, maxId } }
                    };
                    http._Group_User.UpdateLastReadMsgId(req);

                    Class1.frmMsgBox.clb_unread_msg.Items[2].SubItems.Remove(subItem);
                    Class1.unReadGroupMsgItemPool.Remove(groupId);
                    icon.ChangeIconState();
                }
            }

        }
        private void ChatListBox_UpSubItem(object sender, ChatListClickEventArgs e, MouseEventArgs es)
        {
            if (es.Button != MouseButtons.Right)
                return;
            var itemId = e.SelectSubItem.ID;
            if (Class1.chatListSubItemPool.ContainsKey(itemId) && Class1.chatListSubItemPool[itemId] == e.SelectSubItem)
            {
                delFriendUserId = itemId;
                mnu_strip.Show(MousePosition.X, MousePosition.Y);
            }
            else if (Class1.groupItemPool.ContainsKey(itemId) && Class1.groupItemPool[itemId] == e.SelectSubItem)
            {

            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            tick = (tick + 1) % 2;
            if (tick % 2 == 0)
                notifyIcon1.Icon = Properties.Resources.blank;
            else
                notifyIcon1.Icon = Properties.Resources.poseidon;
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            Class1.frmMsgBox.Location = icon.GetFormLocation(Class1.frmMsgBox, notifyIcon1);
            var mousePos = MousePosition;
            var iconPos = icon.GetIconRect(notifyIcon1);
            var isInIcon = mousePos.X >= iconPos.Left && mousePos.X <= iconPos.Right && mousePos.Y >= iconPos.Top && mousePos.Y <= iconPos.Bottom;
            var isInForm = Class1.frmMsgBox.Visible && mousePos.X >= Class1.frmMsgBox.Left && mousePos.X <= Class1.frmMsgBox.Right && mousePos.Y >= Class1.frmMsgBox.Top && mousePos.Y <= iconPos.Top;
            var hasUnReadMsg = Class1.frmMsgBox.clb_unread_msg.Items[0].SubItems.Count > 0 || Class1.frmMsgBox.clb_unread_msg.Items[1].SubItems.Count > 0 || Class1.frmMsgBox.clb_unread_msg.Items[2].SubItems.Count > 0;
            if ((isInIcon || isInForm) && hasUnReadMsg)
                Class1.frmMsgBox.Visible = true;
            else
                Class1.frmMsgBox.Visible = false;
        }

        private void mnu_create_group_Click(object sender, EventArgs e)
        {
            var frmCreateGroup = new frm_create_group();
            frmCreateGroup.ShowDialog();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void mnu_join_group_Click(object sender, EventArgs e)
        {
            if (!Class1.IsOnline)
            {
                MessageBox.Show("你目前处于离线状态，暂时无法使用此功能", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var frmSearchGroup = new frm_search_group();
            frmSearchGroup.ShowDialog();
        }
    }
}
