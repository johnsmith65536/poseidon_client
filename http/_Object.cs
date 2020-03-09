using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.http
{
    class _Object
    {
        public struct CreateObjectReq
        {
            public string ETag;
            public string Name;
        }
        public struct CreateObjectResp
        {
            public long Id;
            public int StatusCode;
            public string StatusMessage;
        }
        public static CreateObjectResp CreateObject(CreateObjectReq req)
        {
            var ret = Class1.DoHttpRequest("/object", "POST", new Dictionary<string, string> { { "access_token", Class1.AccessToken } },  JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<CreateObjectResp>(ret);
            return resp;
        }
    }
}
