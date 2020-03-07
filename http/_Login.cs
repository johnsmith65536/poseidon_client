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
            public int StatusCode;
            public string StatusMessage;
        }
        public struct LogoutReq
        {
            public long UserId;
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
            if (resp.StatusCode == 255)
                Console.WriteLine("error: " + resp.StatusMessage);
            return resp;
        }
        public static LogoutResp Logout(LogoutReq req)
        {
            var ret = Class1.DoHttpRequest("/logout", "POST", null, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<LogoutResp>(ret);
            if (resp.StatusCode == 255)
                Console.WriteLine("error: " + resp.StatusMessage);
            return resp;
        }
    }
}
