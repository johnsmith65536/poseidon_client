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
            public Class1.Group[] Groups;
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

        public struct GetGroupLastReadMsgIdReq
        {
            public long UserId;
        }
        public struct GetGroupLastReadMsgIdResp
        {
            public Dictionary<long, long> LastReadMsgId;
            public int StatusCode;
            public string StatusMessage;
        }

        public struct UpdateGroupLastReadMsgIdReq
        {
            public long UserId;
            public Dictionary<long, long> LastReadMsgId;
        }

        public struct UpdateGroupLastReadMsgIdResp
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
        public struct DeleteMemberReq
        {
            public long Operator;
            public long GroupId;
            public long UserId;
        }

        public struct DeleteMemberResp
        {
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
        public static GetGroupLastReadMsgIdResp GetGroupLastReadMsgId(GetGroupLastReadMsgIdReq req)
        {
            var ret = Class1.DoHttpRequest("/last_read_msg_id/group/" + req.UserId, "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } });
            var resp = JsonConvert.DeserializeObject<GetGroupLastReadMsgIdResp>(ret);
            return resp;
        }
        public static UpdateGroupLastReadMsgIdResp UpdateGroupLastReadMsgId(UpdateGroupLastReadMsgIdReq req)
        {
            var ret = Class1.DoHttpRequest("/last_read_msg_id/group/" + req.UserId, "PUT", new Dictionary<string, string> { { "access_token", Class1.AccessToken } }, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<UpdateGroupLastReadMsgIdResp>(ret);
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
        public static DeleteMemberResp DeleteMember(DeleteMemberReq req)
        {
            var ret = Class1.DoHttpRequest("/group/member", "DELETE", new Dictionary<string, string> { { "access_token", Class1.AccessToken } }, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<DeleteMemberResp>(ret);
            return resp;
        }
    }
}
