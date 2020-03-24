using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.http
{
    class _User_Relation
    {
        public struct AddFriendReq
        {
            public long UserIdSend;
            public long UserIdRecv;
        }
        public struct AddFriendResp
        {
            public long Id;
            public long CreateTime;
            public int StatusCode;
            public string StatusMessage;
        }
        public struct ReplyAddFriendReq
        {
            public long Id;
            public int Status;
        }
        public struct ReplyAddFriendResp
        {
            public long Id;
            public long CreateTime;
            public int StatusCode;
            public string StatusMessage;
        }
        public struct FetchFriendListReq
        {
            public long UserId;
        }
        public struct FetchFriendListResp
        {
            public List<long> OnlineUserIds;
            public List<long> OfflineUserIds;
            public int StatusCode;
            public string StatusMessage;
        }
        public struct DeleteFriendReq
        {
            public long UserIdSend;
            public long UserIdRecv;
        }
        public struct DeleteFriendResp
        {
            public int StatusCode;
            public string StatusMessage;
        }
        public struct GetFriendLastReadMsgIdReq
        {
            public long UserId;
        }
        public struct GetFriendLastReadMsgIdResp
        {
            public Dictionary<long, long> LastReadMsgId;
            public int StatusCode;
            public string StatusMessage;
        }

        public struct UpdateFriendLastReadMsgIdReq
        {
            public long UserId;
            public Dictionary<long, long> LastReadMsgId;
        }

        public struct UpdateFriendLastReadMsgIdResp
        {
            public int StatusCode;
            public string StatusMessage;
        }
        public static AddFriendResp AddFriend(AddFriendReq req)
        {
            var ret = Class1.DoHttpRequest("/friend", "POST", new Dictionary<string, string> { { "access_token", Class1.AccessToken } },  JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<AddFriendResp>(ret);
            return resp;
        }
        public static ReplyAddFriendResp ReplyAddFriend(ReplyAddFriendReq req)
        {
            var ret = Class1.DoHttpRequest("/friend/reply", "POST", new Dictionary<string, string> { { "access_token", Class1.AccessToken } },  JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<ReplyAddFriendResp>(ret);
            return resp;
        }
        public static FetchFriendListResp FetchFriendList(FetchFriendListReq req)
        {
            var ret = Class1.DoHttpRequest("/friend/" + req.UserId, "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } });
            var resp = JsonConvert.DeserializeObject<FetchFriendListResp>(ret);
            return resp;
        }
        public static DeleteFriendResp DeleteFriend(DeleteFriendReq req)
        {
            var ret = Class1.DoHttpRequest($"/friend?user_id_send={req.UserIdSend}&user_id_recv={req.UserIdRecv}", "DELETE", new Dictionary<string, string> { { "access_token", Class1.AccessToken } });
            var resp = JsonConvert.DeserializeObject<DeleteFriendResp>(ret);
            return resp;
        }
        public static GetFriendLastReadMsgIdResp GetFriendLastReadMsgId(GetFriendLastReadMsgIdReq req)
        {
            var ret = Class1.DoHttpRequest("/last_read_msg_id/friend/" + req.UserId, "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } });
            var resp = JsonConvert.DeserializeObject<GetFriendLastReadMsgIdResp>(ret);
            return resp;
        }
        public static UpdateFriendLastReadMsgIdResp UpdateFriendLastReadMsgId(UpdateFriendLastReadMsgIdReq req)
        {
            if (!Class1.IsOnline)
                return new UpdateFriendLastReadMsgIdResp();
            var ret = Class1.DoHttpRequest("/last_read_msg_id/friend/" + req.UserId, "PUT", new Dictionary<string, string> { { "access_token", Class1.AccessToken } }, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<UpdateFriendLastReadMsgIdResp>(ret);
            return resp;
        }
    }
}
