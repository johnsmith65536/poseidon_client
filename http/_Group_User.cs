using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.http
{
    class _Group_User
    {
        public struct FetchGroupListReq
        {
            public long UserId;
        }
        public struct FetchGroupListResp
        {
            public long[] GroupIds;
            public int StatusCode;
            public string StatusMessage;
        }
        public struct FetchMemberListReq
        {
            public long GroupId;
        }
        public struct FetchMemberListResp
        {
            public long[] OnlineUserIds;
            public long[] OfflineUserIds;
            public int StatusCode;
            public string StatusMessage;
        }

        public struct GetLastReadMsgIdReq
        {
            public long UserId;
        }
        public struct GetLastReadMsgIdResp
        {
            public Dictionary<long, long> LastReadMsgId;
            public int StatusCode;
            public string StatusMessage;
        }

        public struct UpdateLastReadMsgIdReq
        {
            public long UserId;
            public Dictionary<long, long> LastReadMsgId;
        }

        public struct UpdateLastReadMsgIdResp
        {
            public int StatusCode;
            public string StatusMessage;
        }
        public struct AddGroupReq
        {
            public long UserId;
            public long GroupId;
        }

        public struct AddGroupResp
        {
            public long Id;
            public long CreateTime;
            public int StatusCode;
            public string StatusMessage;
        }
        public struct ReplyAddGroupReq
        {
            public long Id;
            public int Status;
        }

        public struct ReplyAddGroupResp
        {
            public long Id;
            public long CreateTime;
            public int StatusCode;
            public string StatusMessage;
        }
        public static FetchGroupListResp FetchGroupList(FetchGroupListReq req)
        {
            var ret = Class1.DoHttpRequest("/group/list/" + req.UserId, "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } });
            var resp = JsonConvert.DeserializeObject<FetchGroupListResp>(ret);
            return resp;
        }
        public static FetchMemberListResp FetchMemberList(FetchMemberListReq req)
        {
            var ret = Class1.DoHttpRequest("/group/member/" + req.GroupId, "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } });
            var resp = JsonConvert.DeserializeObject<FetchMemberListResp>(ret);
            return resp;
        }
        public static GetLastReadMsgIdResp GetLastReadMsgId(GetLastReadMsgIdReq req)
        {
            var ret = Class1.DoHttpRequest("/group/last_read_msg_id/" + req.UserId, "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } });
            var resp = JsonConvert.DeserializeObject<GetLastReadMsgIdResp>(ret);
            return resp;
        }
        public static UpdateLastReadMsgIdResp UpdateLastReadMsgId(UpdateLastReadMsgIdReq req)
        {
            var ret = Class1.DoHttpRequest("/group/last_read_msg_id/" + req.UserId, "PUT", new Dictionary<string, string> { { "access_token", Class1.AccessToken } }, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<UpdateLastReadMsgIdResp>(ret);
            return resp;
        }
        public static AddGroupResp AddGroup(AddGroupReq req)
        {
            var ret = Class1.DoHttpRequest("/group/member/add", "POST", new Dictionary<string, string> { { "access_token", Class1.AccessToken } }, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<AddGroupResp>(ret);
            return resp;
        }
        public static ReplyAddGroupResp ReplyAddGroup(ReplyAddGroupReq req)
        {
            var ret = Class1.DoHttpRequest("/group/member/add/reply", "POST", new Dictionary<string, string> { { "access_token", Class1.AccessToken } }, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<ReplyAddGroupResp>(ret);
            return resp;
        }
    }
}
