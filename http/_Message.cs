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
        }
        public struct SyncMessageResp
        {
            public List<Message> Messages;
            public List<UserRelationRequest> UserRelations;
            public List<Object> Objects;
            public long LastOnlineTime;
            public int StatusCode;
            public string StatusMessage;
        }

        public struct UpdateMessageStatusReq
        {
            public Dictionary<long, int> MessageIds;
            public Dictionary<long, int> UserRelationRequestIds;
        }
        public struct UpdateMessageStatusResp
        {
            public int StatusCode;
            public string StatusMessage;
        }
        public struct FetchMessageStatusReq
        {
            public List<long> MessageIds;
            public List<long> UserRelationRequestIds;
        }
        public struct FetchMessageStatusResp
        {
            public Dictionary<long, int> MessageIds;
            public Dictionary<long, int> UserRelationRequestIds;
            public int StatusCode;
            public string StatusMessage;
        }
        public static SendMessageResp SendMessage(SendMessageReq req)
        {
            var ret = Class1.DoHttpRequest("/message", "POST", new Dictionary<string, string> { { "access_token", Class1.AccessToken } }, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<SendMessageResp>(ret);
            return resp;
        }
        public static SyncMessageResp SyncMessage(SyncMessageReq req)
        {
            var ret = Class1.DoHttpRequest($"/message?user_id={req.UserId}&message_id={req.MessageId}&user_relation_id={req.UserRelationId}", "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } },  null);
            var resp = JsonConvert.DeserializeObject<SyncMessageResp>(ret);
            return resp;
        }
        public static UpdateMessageStatusResp UpdateMessageStatus(UpdateMessageStatusReq req)
        {
            var ret = Class1.DoHttpRequest("/message/status", "PUT", new Dictionary<string, string> { { "access_token", Class1.AccessToken } },  JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<UpdateMessageStatusResp>(ret);
            return resp;
        }
        public static FetchMessageStatusResp FetchMessageStatus(FetchMessageStatusReq req)
        {
            var url = "/message/status?";
            foreach (var messageId in req.MessageIds)
                url += "message_ids=" + messageId + "&";
            foreach (var userRelationRequestId in req.UserRelationRequestIds)
                url += "user_relation_request_ids=" + userRelationRequestId + "&";
            var ret = Class1.DoHttpRequest(url, "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } },  null);
            var resp = JsonConvert.DeserializeObject<FetchMessageStatusResp>(ret);
            return resp;
        }
    }
}
