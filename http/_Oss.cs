using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.http
{
    class _Oss
    {
        public struct GetSTSInfoReq
        {
            public long UserId;
        }
        public struct GetSTSInfoResp
        {
            public string SecurityToken;
            public string AccessKeyId;
            public string AccessKeySecret;
            public int StatusCode;
            public string StatusMessage;
        }
        public static GetSTSInfoResp GetSTSInfo(GetSTSInfoReq req)
        {
            var ret = Class1.DoHttpRequest("/sts_info/" + req.UserId, "GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } });
            var resp = JsonConvert.DeserializeObject<GetSTSInfoResp>(ret);
            return resp;
        }
    }
}
