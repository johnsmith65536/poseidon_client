using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.http
{
    class _Login
    {
        public struct LoginReq
        {
            public long UserId;
            public string Password;
        }
        public struct LoginResp
        {
            public bool Success;
            public string AccessToken;
            public int StatusCode;
            public string StatusMessage;
        }
        public struct LogoutReq
        {
            public long UserId;
            public string AccessToken;
        }
        public struct LogoutResp
        {
            public int StatusCode;
            public string StatusMessage;
        }
        public static LoginResp Login(LoginReq req)
        {
            var ret = Class1.DoHttpRequest("/login", "POST", null, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<LoginResp>(ret);
            return resp;
        }
        public static LogoutResp Logout(LogoutReq req)
        {
            var ret = Class1.DoHttpRequest("/logout", "POST", new Dictionary<string, string>{{ "access_token", Class1.AccessToken}}, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<LogoutResp>(ret);
            return resp;
        }
    }
}
