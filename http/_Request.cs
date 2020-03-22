using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.http
{
    class _Request
    {
        public struct UpdateRequestStatusReq
        {
            public Dictionary<long, int> UserRelationRequestIds;
            public Dictionary<long, int> GroupUserRequestIds;

        }
        public struct UpdateRequestStatusResp
        {
            public int StatusCode;
            public string StatusMessage;
        }
        public struct FetchRequestStatusReq
        {
            public List<long> UserRelationRequestIds;
            public List<long> GroupUserRequestIds;
        }
        public struct FetchRequestStatusResp
        {
            public Dictionary<long, int> UserRelationRequestIds;
            public Dictionary<long, int> GroupUserRequestIds;
            public int StatusCode;
            public string StatusMessage;
        }
        public static UpdateRequestStatusResp UpdateRequestStatus(UpdateRequestStatusReq req)
        {
            var ret = Class1.DoHttpRequest("/request/status", "PUT", new Dictionary<string, string> { { "access_token", Class1.AccessToken } }, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<UpdateRequestStatusResp>(ret);
            return resp;
        }
        public static FetchRequestStatusResp FetchRequestStatus(FetchRequestStatusReq req)
        {
            var url = "/request/status?";
            foreach (var userRelationRequestId in req.UserRelationRequestIds)
                url += "user_relation_request_ids=" + userRelationRequestId + "&";
            foreach (var groupUserRequestIds in req.GroupUserRequestIds)
                url += "group_user_request_ids=" + groupUserRequestIds + "&";
            var ret = Class1.DoHttpRequest(url, "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } }, null);
            var resp = JsonConvert.DeserializeObject<FetchRequestStatusResp>(ret);
            return resp;
        }
    }
}
