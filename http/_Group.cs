using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.http
{
    class _Group
    {
        public struct CreateGroupReq
        {
            public long Owner;
            public string Name;
            public long[] UserIds;
        }
        public struct CreateGroupResp
        {
            public long Id;
            public int StatusCode;
            public string StatusMessage;
        }
        public struct SearchGroupReq
        {
            public long UserId;
            public string Data;
        }
        public struct Group
        {
            public long Id;
            public string Name;
            public long CreateTime;
            public bool IsMember;
        }
        public struct SearchGroupResp
        {
            public Group[] Groups;
            public int StatusCode;
            public string StatusMessage;
        }
        public static CreateGroupResp CreateGroup(CreateGroupReq req)
        {
            var ret = Class1.DoHttpRequest("/group", "POST", new Dictionary<string, string> { { "access_token", Class1.AccessToken } }, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<CreateGroupResp>(ret);
            return resp;
        }
        public static SearchGroupResp SearchGroup(SearchGroupReq req)
        {
            var ret = Class1.DoHttpRequest("/group/search?user_id=" + req.UserId + "&data=" + req.Data, "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } });
            var resp = JsonConvert.DeserializeObject<SearchGroupResp>(ret);
            return resp;
        }
    }
}
