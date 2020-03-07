using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.http
{
    class _Message
    {
        public struct SendMessageReq
        {
            public long UserIdSend;
            public long IdRecv;
            public string Content;
            public int ContentType;
            public int MessageType;
        }
        public struct SendMessageResp
        {
            public long Id;
            public long CreateTime;
            public int StatusCode;
            public string StatusMessage;
        }
        public static SendMessageResp SendMessage(SendMessageReq req)
        {
            var ret = Class1.DoHttpRequest("http://" + Class1.Ip + Class1.HttpPort + "/message", "POST", null, JsonConvert.SerializeObject(req));
            var resp = JsonConvert.DeserializeObject<SendMessageResp>(ret);
            if (resp.StatusCode != 0)
                Console.WriteLine("error: " + resp.StatusMessage);
            return resp;
        }
    }
}
