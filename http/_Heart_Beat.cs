using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.http
{
    class _Heart_Beat
    {
        public struct HeartBeatReq
        {
            public long UserId;
        }
        public struct HeartBeatResp
        {
            public int StatusCode;
            public string StatusMessage;
        }
        public static HeartBeatResp HeartBeat(HeartBeatReq req)
        {
            var ret = Class1.DoHttpRequest("/heart_beat/" + req.UserId,"GET", new Dictionary<string, string> { { "access_token", Class1.AccessToken } });
            var resp = JsonConvert.DeserializeObject<HeartBeatResp>(ret);
            return resp;
        }
    }
}
