using System;
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
using Thrift.Collections;

namespace Poseidon
{
    public partial class frm_main : Form
    {
        private long delFriendUserId;
        private enum BroadcastMsgType
        {
            Chat = 0,
            AddFriend = 1,
            ReplyAddFriend = 2
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
        public void LoadUnReadMessage()
        {

            InvokeGridClear(dgv_msg);
            DataTable dt = Class1.sql.SqlTable($"SELECT id, user_id_send, content, create_time, content_type FROM `message` WHERE `user_id_recv` = {Class1.UserId} AND `is_read` = 0");
            foreach (DataRow row in dt.Rows)
            {
                var id = long.Parse(row["id"].ToString());
                var userIdSend = long.Parse(row["user_id_send"].ToString());
                var content = row["content"].ToString();
                var createTime = Class1.FormatDateTime(Class1.StampToDateTime(long.Parse(row["create_time"].ToString())));
                var contentType = long.Parse(row["content_type"].ToString());
                Console.WriteLine("content_type = ",contentType);
                switch (contentType)
                {
                    case (int)Class1.ContentType.Text:
                        {
                            InvokeGridAdd(dgv_msg, new Dictionary<string, object> {
            {"id",id},
            {"type","消息"},
            {"user_id_send",userIdSend},
            {"content",content},
            {"create_time",createTime}
        });
                            break;
                        }
                    case (int)Class1.ContentType.Object:
                        {
                            var objId = long.Parse(content);
                            DataTable dt1 = Class1.sql.SqlTable($"SELECT name FROM `object` WHERE `id` = {objId}");
                            if (dt1.Rows.Count != 1)
                            {
                                Console.WriteLine("rowCount != 1");
                                return;
                            }
                            InvokeGridAdd(dgv_msg, new Dictionary<string, object> {
            {"id",id},
            {"type","消息"},
            {"user_id_send",userIdSend},
            {"content","[文件]" + dt1.Rows[0]["name"].ToString()},
            {"create_time",createTime}
        });
                            break;
                        }
                }
            }

            dt = Class1.sql.SqlTable($"SELECT id, user_id_send, create_time, parent_id FROM `user_relation_request` WHERE `user_id_recv` = {Class1.UserId} AND `status` = 0");


            // 同步最新的status，message暂不同步
            List<long> queryIds = new List<long>();
            foreach (DataRow row in dt.Rows)
            {
                var parentId = long.Parse(row["parent_id"].ToString());
                if (parentId != -1)
                    queryIds.Add(parentId);
            }
            //var resp = rpc._Message.FetchMessageStatus(new List<long>(), queryIds);
            var req = new http._Message.FetchMessageStatusReq()
            {
                MessageIds = new List<long>(),
                UserRelationRequestIds = queryIds
            };
            var resp = http._Message.FetchMessageStatus(req);
            foreach (var item in resp.UserRelationRequestIds)
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


            foreach (DataRow row in dt.Rows)
            {
                var id = long.Parse(row["id"].ToString());
                var userIdSend = long.Parse(row["user_id_send"].ToString());
                var createTime = Class1.FormatDateTime(Class1.StampToDateTime(long.Parse(row["create_time"].ToString())));
                var parentId = long.Parse(row["parent_id"].ToString());
                if (parentId == -1)
                {
                    InvokeGridAdd(dgv_msg, new Dictionary<string, object> {
            {"id",id},
            {"type","好友请求"},
            {"user_id_send",userIdSend},
            {"create_time",createTime}
        });
                }
                else
                {
                    var dt1 = Class1.sql.SqlTable($"SELECT status FROM `user_relation_request` WHERE `id` = {parentId}");
                    if (dt1.Rows.Count == 0)
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
                        Console.WriteLine("无效的status, status = ", status);
                        return;
                    }
                    InvokeGridAdd(dgv_msg, new Dictionary<string, object> {
            {"id",id},
            {"type","好友请求回复"},
            {"user_id_send",userIdSend},
            {"create_time",createTime},
            {"content",statusString}
        });
                }
            }
        }
        private void frm_main_Load(object sender, EventArgs e)
        {
            Thread t1 = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    //ListClear(lbx_friend);
                    InvokeGridClear(dgv_friend);
                    //var resp = rpc._Relation.FetchFriendList(Class1.UserId);
                    var req = new http._Relation.FetchFriendListReq()
                    {
                        UserId = Class1.UserId
                    };
                    var resp = http._Relation.FetchFriendList(req);
                    foreach (long userId in resp.OnlineUserIds)
                        //ListAdd(lbx_friend, userId + "(online)");
                        InvokeGridAdd(dgv_friend, new Dictionary<string, object> {
            {"user_id",userId},
            {"status","online"}
        });
                    foreach (long userId in resp.OfflineUserIds)
                        //ListAdd(lbx_friend, userId + "(offline)");
                        InvokeGridAdd(dgv_friend, new Dictionary<string, object> {
            {"user_id",userId},
            {"status","offline"}
        });
                    Thread.Sleep(2 * 1000);
                };
            }));
            t1.Start();

            Thread t2 = new Thread(new ThreadStart(() =>    //异步拉取消息
            {
                long messageId = -1, userRelationId = -1;
                DataTable dt = Class1.sql.SqlTable($"SELECT count(*) as count, MAX(id) as id FROM `message` WHERE (`user_id_send` = {Class1.UserId} OR `user_id_recv` = {Class1.UserId})");
                if (dt != null && dt.Rows.Count == 1 && (long)dt.Rows[0]["count"] > 0)
                    messageId = (long)dt.Rows[0]["id"];
                dt = Class1.sql.SqlTable($"SELECT count(*) as count, MAX(id) as id FROM `user_relation_request` WHERE (`user_id_send` = {Class1.UserId} OR `user_id_recv` = {Class1.UserId})");
                if (dt != null && dt.Rows.Count == 1 && (long)dt.Rows[0]["count"] > 0)
                    userRelationId = (long)dt.Rows[0]["id"];
                //var resp = rpc._Message.SyncMessage(Class1.UserId, messageId, userRelationId);
                var req = new http._Message.SyncMessageReq()
                {
                    UserId = Class1.UserId,
                    MessageId = messageId,
                    UserRelationId = userRelationId
                };
                var resp = http._Message.SyncMessage(req);
                var messages = resp.Messages;
                var userRelations = resp.UserRelations;
                var objects = resp.Objects;
                var lastOnlineTime = resp.LastOnlineTime;
                //消息落库

                foreach (var obj in objects)
                {
                    /*bool ret1 = Class1.sql.ExecuteNonQuery($"INSERT INTO `object`(id, e_tag, name) VALUES({obj.Id}, '{obj.ETag}', '{obj.Name}')");
                    if (!ret1)
                    {
                        //MessageBox.Show("DB错误，INSERT INTO object失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //return;
                    }*/

                    Class1.InsertObject(obj.Id, obj.ETag, obj.Name);
                }

                foreach (var msg in messages)
                {
                    bool ret1 = Class1.sql.ExecuteNonQuery($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type, is_read) VALUES({msg.Id}, " +
                        $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.GroupId}, '{msg.Content}', {msg.CreateTime}, {msg.ContentType}, {msg.MsgType}, {msg.IsRead})");
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
                LoadUnReadMessage();


            }));
            t2.Start();


            Thread t3 = new Thread(new ThreadStart(() =>
            {
                ConnectionMultiplexer redisCli = redis.GetRedisConn();
                var sub = redisCli.GetSubscriber();
                var chn = "poseidon_heart_beat_channel";
                sub.Subscribe(chn, HeartBeatChannel);
            }));
            t3.Start();

            Thread t4 = new Thread(new ThreadStart(() =>
            {
                ConnectionMultiplexer redisCli = redis.GetRedisConn();
                var sub = redisCli.GetSubscriber();
                var chn = "poseidon_message_channel_" + Class1.UserId;
                sub.Subscribe(chn, MessageChannel);
            }));
            t4.Start();


        }
        public void MessageChannel(RedisChannel cnl, RedisValue val)
        {
            ConnectionMultiplexer redisCli = redis.GetRedisConn();
            Console.WriteLine("频道：" + cnl + "\t收到消息:" + val);

            var msgType = GetBroadcastMsgType(val.ToString());
            switch (msgType)
            {
                case BroadcastMsgType.Chat:
                    {
                        var msg = JsonConvert.DeserializeObject<RedisMessage>(val.ToString());
                        //var newText = txt_status.Text + "消息接受成功, msgId = " + msg.Id + ", content = " + msg.Content + Environment.NewLine;
                        //SetText(txt_status, newText);
                        //rpc._Message.MessageDelivered(msg.Id);
                        bool ret = Class1.sql.ExecuteNonQuery($"INSERT INTO `message`(id, user_id_send, user_id_recv, group_id, content, create_time, content_type, msg_type, is_read) VALUES({msg.Id}, " +
                            $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.GroupId}, '{msg.Content}', {msg.CreateTime}, {msg.ContentType}, {msg.MsgType}, {msg.IsRead})");
                        if (!ret)
                        {
                            MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //ListAdd(lbx_msg, $"[新消息]来自{msg.UserIdSend}，内容:{msg.Content}");
                        switch (msg.ContentType)
                        {
                            case (int)Class1.ContentType.Text:
                                {
                                    InvokeGridAdd(dgv_msg, new Dictionary<string, object> {
            {"id",msg.Id},
            {"type","消息"},
            {"user_id_send",msg.UserIdSend},
            {"content",msg.Content},
            {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(msg.CreateTime))}
        });
                                    break;
                                }
                            case (int)Class1.ContentType.Object:
                                {
                                    var objId = long.Parse(msg.Content);

                                    /*bool ret1 = Class1.sql.ExecuteNonQuery($"INSERT INTO `object`(id, e_tag, name) VALUES({objId}, '{msg.ObjectETag}', '{msg.ObjectName}')");
                                    if (!ret1)
                                    {
                                        //MessageBox.Show("DB错误，INSERT INTO message失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        //return;
                                    }*/

                                    Class1.InsertObject(objId, msg.ObjectETag, msg.ObjectName);

                                    InvokeGridAdd(dgv_msg, new Dictionary<string, object> {
            {"id",msg.Id},
            {"type","消息"},
            {"user_id_send",msg.UserIdSend},
            {"content","[文件]" + msg.ObjectName},
            {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(msg.CreateTime))}
        });
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("unknown content_type content_type = ", msg.ContentType);
                                    break;
                                }
                        }
                        if (Class1.formChatPool.ContainsKey(msg.UserIdSend))
                        {
                            var frm_chat = Class1.formChatPool[msg.UserIdSend];
                            //ListAdd(frm_chat.listBox1, "对方:" + msg.Content + " " + Class1.FormatDateTime(Class1.StampToDateTime(msg.CreateTime)));
                            switch (msg.ContentType)
                            {
                                case (int)Class1.ContentType.Text:
                                    {
                                        InvokeGridAdd(frm_chat.dgv_msg, new Dictionary<string, object> {
            {"user_id",msg.UserIdSend},
            {"content",msg.Content},
            {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(msg.CreateTime))}
        });
                                        break;
                                    }
                                case (int)Class1.ContentType.Object:
                                    {
                                        InvokeGridAdd(frm_chat.dgv_msg, new Dictionary<string, object> {
            {"user_id",msg.UserIdSend},
            {"content","[文件]" + msg.ObjectName},
            {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(msg.CreateTime))},
            {"e_tag",msg.ObjectETag}
        });
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
                case BroadcastMsgType.AddFriend:
                    {
                        var msg = JsonConvert.DeserializeObject<RedisAddFriend>(val.ToString());
                        //var newText = txt_status.Text + "用户" + msg.UserIdSend + "请求添加你为好友, msgId = " + msg.Id + Environment.NewLine;
                        //SetText(txt_status, newText);
                        //SetText(txt_add_friend_id, msg.Id.ToString());
                        bool ret = Class1.sql.ExecuteNonQuery($"INSERT INTO `user_relation_request`(id, user_id_send, user_id_recv, create_time, status, parent_id) VALUES({msg.Id}, " +
                            $"{msg.UserIdSend}, {msg.UserIdRecv}, {msg.CreateTime}, 0, -1)");
                        if (!ret)
                        {
                            MessageBox.Show("DB错误，INSERT INTO user_relation_request失败888", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //ListAdd(lbx_msg, $"[好友请求]来自{msg.UserIdSend}的好友请求");
                        InvokeGridAdd(dgv_msg, new Dictionary<string, object> {
            {"id",msg.Id},
            {"type","好友请求"},
            {"user_id_send",msg.UserIdSend},
            {"content",""},
            {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(msg.CreateTime))}
        });
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


                        string statusString;
                        if (msg.Status == (long)Class1.AddFriendStatus.Accepted)
                            statusString = "请求通过";
                        else if (msg.Status == (long)Class1.AddFriendStatus.Rejected)
                            statusString = "请求被拒绝";
                        else
                        {
                            Console.WriteLine("无效的status, status = ", status);
                            return;
                        }

                        InvokeGridAdd(dgv_msg, new Dictionary<string, object> {
            {"id",msg.Id},
            {"type","好友请求回复"},
            {"user_id_send",msg.UserIdSend},
            {"content",statusString},
            {"create_time",Class1.FormatDateTime(Class1.StampToDateTime(msg.CreateTime))}
        });
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Unknown BroadcastMsgType, type: " + msgType);

                        break;
                    }
            }


            // Console.WriteLine("线程：" + Thread.CurrentThread.ManagedThreadId + ",是否线程池：" + Thread.CurrentThread.IsThreadPoolThread);
        }
        public void HeartBeatChannel(RedisChannel cnl, RedisValue val)
        {
            //rpc._Heart_Beat.HeartBeat(Class1.UserId);
            var req = new http._Heart_Beat.HeartBeatReq()
            {
                UserId = Class1.UserId
            };
            http._Heart_Beat.HeartBeat(req);
            Console.WriteLine("Send HeartBeat");
            //Console.WriteLine("频道：" + cnl + "\t收到消息:" + val); ;
            // Console.WriteLine("线程：" + Thread.CurrentThread.ManagedThreadId + ",是否线程池：" + Thread.CurrentThread.IsThreadPoolThread);
        }
        private void frm_main_FormClosed(object sender, FormClosedEventArgs e)
        {
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
            frm_search_user frm_search_user = new frm_search_user();
            frm_search_user.ShowDialog();
        }
        private void UpdateMessageStatus(Dictionary<long, int> message, Dictionary<long, int> userRelationRequest)
        {
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
            //rpc._Message.UpdateMessageStatus(message, userRelationRequest);
            var req = new http._Message.UpdateMessageStatusReq()
            {
                MessageIds = message,
                UserRelationRequestIds = userRelationRequest
            };
            http._Message.UpdateMessageStatus(req);
        }

        private void dgv_friend_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                Console.WriteLine(dgv_friend.Rows[e.RowIndex].Cells["user_id"].Value);
                var userId = long.Parse(dgv_friend.Rows[e.RowIndex].Cells["user_id"].Value.ToString());
                dgv_friend.ClearSelection();
                dgv_friend.Rows[e.RowIndex].Selected = true;
                dgv_friend.CurrentCell = dgv_friend.Rows[e.RowIndex].Cells[e.ColumnIndex];

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
                Dictionary<long, int> readMessage = new Dictionary<long, int>();
                foreach (DataGridViewRow row in dgv_msg.Rows)
                {
                    var type = row.Cells["type"].Value.ToString();
                    var userIdSend = long.Parse(row.Cells["user_id_send"].Value.ToString());
                    if (type == "消息" && userIdSend == userId)
                    {
                        var id = long.Parse(row.Cells["id"].Value.ToString());
                        readMessage.Add(id, 1);
                    }
                }
                UpdateMessageStatus(readMessage, new Dictionary<long, int>());
                LoadUnReadMessage();
            }
        }

        private void dgv_msg_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                //Console.WriteLine(dgv_friend.Rows[e.RowIndex].Cells["user_id"].Value);
                var type = dgv_msg.Rows[e.RowIndex].Cells["type"].Value.ToString();
                dgv_msg.ClearSelection();
                dgv_msg.Rows[e.RowIndex].Selected = true;
                dgv_msg.CurrentCell = dgv_msg.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var userIdSend = long.Parse(dgv_msg.Rows[e.RowIndex].Cells["user_id_send"].Value.ToString());
                var id = long.Parse(dgv_msg.Rows[e.RowIndex].Cells["id"].Value.ToString());
                if (type == "消息")
                {
                    frm_chat frm_chat;
                    if (Class1.formChatPool.ContainsKey(userIdSend))
                    {
                        frm_chat = Class1.formChatPool[userIdSend];
                        frm_chat.Activate();
                    }
                    else
                    {
                        frm_chat = new frm_chat(userIdSend);
                        Class1.formChatPool.Add(userIdSend, frm_chat);
                        frm_chat.Show();
                    }
                    Dictionary<long, int> readMessage = new Dictionary<long, int>();
                    foreach (DataGridViewRow row in dgv_msg.Rows)
                    {
                        var _type = row.Cells["type"].Value.ToString();
                        var _userIdSend = long.Parse(row.Cells["user_id_send"].Value.ToString());
                        if (_type == "消息" && userIdSend == _userIdSend)
                        {
                            var _id = long.Parse(row.Cells["id"].Value.ToString());
                            readMessage.Add(_id, 1);
                        }
                    }
                    UpdateMessageStatus(readMessage, new Dictionary<long, int>());
                    LoadUnReadMessage();
                }
                else if (type == "好友请求")
                {
                    var ret = MessageBox.Show(userIdSend + "请求添加为好友，是否接受？", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    int status;
                    if (ret == DialogResult.Yes)
                        status = (int)Class1.AddFriendStatus.Accepted;
                    else if (ret == DialogResult.No)
                        status = (int)Class1.AddFriendStatus.Rejected;
                    else
                        return;

                    //var resp = rpc._Relation.ReplyAddFriend(id, status);
                    var req = new http._Relation.ReplyAddFriendReq()
                    {
                        Id = id,
                        Status = status
                    };
                    var resp = http._Relation.ReplyAddFriend(req);
                    var replyId = resp.Id;
                    var createTime = resp.CreateTime;
                    bool ok = Class1.sql.ExecuteNonQuery($"UPDATE `user_relation_request` SET `status` = {status} WHERE `id` = {id}");
                    if (!ok)
                    {
                        MessageBox.Show("DB错误，UPDATE user_relation_request", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ok = Class1.sql.ExecuteNonQuery($"INSERT INTO `user_relation_request`(id, user_id_send, user_id_recv, create_time, status, parent_id) VALUES({replyId}, " +
                        $"{Class1.UserId}, {userIdSend}, {createTime}, 0, {id})");
                    if (!ok)
                    {
                        MessageBox.Show("DB错误，INSERT INTO user_relation_request", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    LoadUnReadMessage();
                }
                else if (type == "好友请求回复")
                {
                    UpdateMessageStatus(new Dictionary<long, int>(), new Dictionary<long, int> { { id, 1 } });
                    LoadUnReadMessage();
                }
                else
                {
                    Console.WriteLine("unknown type");
                }

            }
        }
        private void dgv_friend_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    delFriendUserId = long.Parse(dgv_friend.Rows[e.RowIndex].Cells["user_id"].Value.ToString());
                    dgv_friend.ClearSelection();
                    dgv_friend.Rows[e.RowIndex].Selected = true;
                    dgv_friend.CurrentCell = dgv_friend.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    mnu_strip.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void mnu_del_friend_Click(object sender, EventArgs e)
        {
            //rpc._Relation.DeleteFriend(Class1.UserId, delFriendUserId);
            var req = new http._Relation.DeleteFriendReq()
            {
                UserIdSend = Class1.UserId,
                UserIdRecv = delFriendUserId
            };
            http._Relation.DeleteFriend(req);
            MessageBox.Show("好友删除成功", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
