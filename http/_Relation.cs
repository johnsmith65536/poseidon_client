using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.http
{
    class _Relation
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
        public static AddFriendResp AddFriend(AddFriendReq req)
        {
            var ret = Class1.DoHttpRequest("/friend", "POST", null, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<AddFriendResp>(ret);
            if (resp.StatusCode == 255)
                Console.WriteLine("error: " + resp.StatusMessage);
            return resp;
        }
        public static ReplyAddFriendResp ReplyAddFriend(ReplyAddFriendReq req)
        {
            var ret = Class1.DoHttpRequest("/friend/reply", "POST", null, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<ReplyAddFriendResp>(ret);
            if (resp.StatusCode == 255)
                Console.WriteLine("error: " + resp.StatusMessage);
            return resp;
        }
        public static FetchFriendListResp FetchFriendList(FetchFriendListReq req)
        {
            var ret = Class1.DoHttpRequest("/friend/" + req.UserId, "GET");
            var resp = JsonConvert.DeserializeObject<FetchFriendListResp>(ret);
            if (resp.StatusCode == 255)
                Console.WriteLine("error: " + resp.StatusMessage);
            return resp;
        }
        public static DeleteFriendResp DeleteFriend(DeleteFriendReq req)
        {
            var ret = Class1.DoHttpRequest($"/friend?user_id_send={req.UserIdSend}&user_id_recv={req.UserIdRecv}", "DELETE");
            var resp = JsonConvert.DeserializeObject<DeleteFriendResp>(ret);
            if (resp.StatusCode == 255)
                Console.WriteLine("error: " + resp.StatusMessage);
            return resp;
        }
    }
}
