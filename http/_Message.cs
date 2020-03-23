using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.http
{
    class _Message
    {
        public struct Message
        {
            public long Id;
            public long UserIdSend;
            public long UserIdRecv;
            public long GroupId;
            public string Content;
            public long CreateTime;
            public int ContentType;
            public int MsgType;
            public bool IsRead;
        }
        public struct UserRelationRequest
        {
            public long Id;
            public long UserIdSend;
            public long UserIdRecv;
            public long CreateTime;
            public int Status;
            public long ParentId;
        }
        public struct GroupUserRequest
        {
            public long Id;
            public long UserIdSend;
            public long UserIdRecv;
            public long GroupId;
            public long CreateTime;
            public int Status;
            public long ParentId;
            public int Type;
        }
        public struct Object
        {
            public long Id;
            public string ETag;
            public string Name;
        }
        public struct SendMessageReq
        {
            public long UserIdSend;
            public long IdRecv;
            public string Content;
            public int ContentType;
            public int MessageType;
        }
        public struct SendMessageResp
        {
            public long Id;
            public long CreateTime;
            public int StatusCode;
            public string StatusMessage;
        }
        public struct SyncMessageReq
        {
            public long UserId;
            public long MessageId;
            public long UserRelationId;
            public long GroupUserId;
        }
        public struct SyncMessageResp
        {
            public List<Message> Messages;
            public List<UserRelationRequest> UserRelations;
            public List<Object> Objects;
            public List<GroupUserRequest> GroupUsers;
            public long LastOnlineTime;
            public int StatusCode;
            public string StatusMessage;
        }
        public struct FetchFriendHistoryMessageReq
        {
            public long UserIdAlice;
            public long UserIdBob;
            public long LocalCount;
        }
        public struct FetchFriendHistoryMessageResp
        {
            public List<Message> Messages;
            public List<Object> Objects;
            public int StatusCode;
            public string StatusMessage;
        }
        public struct FetchGroupHistoryMessageReq
        {
            public long GroupId;
            public long LocalCount;
        }
        public struct FetchGroupHistoryMessageResp
        {
            public List<Message> Messages;
            public List<Object> Objects;
            public int StatusCode;
            public string StatusMessage;
        }


        public static SendMessageResp SendMessage(SendMessageReq req)
        {
            req.Content = Convert.ToBase64String(Class1.Gzip(System.Text.Encoding.Default.GetBytes(req.Content)));
            var ret = Class1.DoHttpRequest("/message", "POST", new Dictionary<string, string> { { "access_token", Class1.AccessToken } }, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<SendMessageResp>(ret);
            return resp;
        }
        public static SyncMessageResp SyncMessage(SyncMessageReq req)
        {
            var ret = Class1.DoHttpRequest($"/message?user_id={req.UserId}&message_id={req.MessageId}&user_relation_id={req.UserRelationId}&group_user_id={req.GroupUserId}", "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } },  null);
            var resp = JsonConvert.DeserializeObject<SyncMessageResp>(ret);
            for (int i = 0;i < resp.Messages.Count;i++)
            {
                var rawContent = System.Text.Encoding.Default.GetString(Class1.UnGzip(Convert.FromBase64String(resp.Messages[i].Content)));
                var msg = new Message()
                {
                    Id = resp.Messages[i].Id,
                    UserIdSend = resp.Messages[i].UserIdSend,
                    UserIdRecv = resp.Messages[i].UserIdRecv,
                    GroupId = resp.Messages[i].GroupId,
                    Content = rawContent,
                    CreateTime = resp.Messages[i].CreateTime,
                    ContentType = resp.Messages[i].ContentType,
                    MsgType = resp.Messages[i].MsgType,
                    IsRead = resp.Messages[i].IsRead
                };
                resp.Messages[i] = msg;
            }
            return resp;
        }
        public static FetchFriendHistoryMessageResp FetchFriendHistoryMessage(FetchFriendHistoryMessageReq req)
        {
            var url = $"/message/history/friend?user_id_alice={req.UserIdAlice}&user_id_bob={req.UserIdBob}&local_count={req.LocalCount}";
            var ret = Class1.DoHttpRequest(url, "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } }, null);
            var resp = JsonConvert.DeserializeObject<FetchFriendHistoryMessageResp>(ret);
            for (int i = 0; i < resp.Messages.Count; i++)
            {
                var rawContent = System.Text.Encoding.Default.GetString(Class1.UnGzip(Convert.FromBase64String(resp.Messages[i].Content)));
                var msg = new Message()
                {
                    Id = resp.Messages[i].Id,
                    UserIdSend = resp.Messages[i].UserIdSend,
                    UserIdRecv = resp.Messages[i].UserIdRecv,
                    GroupId = resp.Messages[i].GroupId,
                    Content = rawContent,
                    CreateTime = resp.Messages[i].CreateTime,
                    ContentType = resp.Messages[i].ContentType,
                    MsgType = resp.Messages[i].MsgType,
                    IsRead = resp.Messages[i].IsRead
                };
                resp.Messages[i] = msg;
            }
            return resp;
        }
        public static FetchGroupHistoryMessageResp FetchGroupHistoryMessage(FetchGroupHistoryMessageReq req)
        {
            var url = $"/message/history/group?group_id={req.GroupId}&local_count={req.LocalCount}";
            var ret = Class1.DoHttpRequest(url, "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } }, null);
            var resp = JsonConvert.DeserializeObject<FetchGroupHistoryMessageResp>(ret);
            for (int i = 0; i < resp.Messages.Count; i++)
            {
                var rawContent = System.Text.Encoding.Default.GetString(Class1.UnGzip(Convert.FromBase64String(resp.Messages[i].Content)));
                var msg = new Message()
                {
                    Id = resp.Messages[i].Id,
                    UserIdSend = resp.Messages[i].UserIdSend,
                    UserIdRecv = resp.Messages[i].UserIdRecv,
                    GroupId = resp.Messages[i].GroupId,
                    Content = rawContent,
                    CreateTime = resp.Messages[i].CreateTime,
                    ContentType = resp.Messages[i].ContentType,
                    MsgType = resp.Messages[i].MsgType,
                    IsRead = resp.Messages[i].IsRead
                };
                resp.Messages[i] = msg;
            }
            return resp;
        }
    }
}
