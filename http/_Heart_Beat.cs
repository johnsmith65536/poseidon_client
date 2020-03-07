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
        public struct HeartBeatResp
        {
            public int StatusCode;
            public string StatusMessage;
        }
        public static HeartBeatResp HeartBeat(long userId)
        {
            var ret = Class1.DoHttpRequest("http://" + Class1.Ip + Class1.HttpPort + "/heart_beat/" + userId,"GET");
            var resp = JsonConvert.DeserializeObject<HeartBeatResp>(ret);
            if (resp.StatusCode != 0)
                Console.WriteLine("error: " + resp.StatusMessage);
            return resp;
        }
    }
}
