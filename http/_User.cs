using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.http
{
    class _User
    {
        public struct User
        {
            public long Id;
            public string NickName;
            public long LastOnlineTime;
            public bool IsFriend;
        }

        public struct CreateUserReq
        {
            public string Password;
            public string NickName;
        }

        public struct CreateUserResp
        {
            public long UserId;
            public int StatusCode;
            public string StatusMessage;
        }
        public struct SearchUserReq
        {
            public long UserId;
            public string Data;
        }

        public struct SearchUserResp
        {
            public List<User> Users;
            public int StatusCode;
            public string StatusMessage;
        }
        public static CreateUserResp CreateUser(CreateUserReq req)
        {
            var ret = Class1.DoHttpRequest("/user", "POST", new Dictionary<string, string> { { "access_token", Class1.AccessToken } },  JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<CreateUserResp>(ret);
            return resp;
        }
        public static SearchUserResp SearchUser(SearchUserReq req)
        {
            var ret = Class1.DoHttpRequest($"/user/search?user_id={req.UserId}&data={req.Data}", "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } });
            var resp = JsonConvert.DeserializeObject<SearchUserResp>(ret);
            return resp;
        }
    }
}
