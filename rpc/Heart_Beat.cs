using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.rpc
{
    class Heart_Beat
    {
        public static void HeartBeat(Int64 userId)
        {
            var req = new HeartBeatReq()
            {
                UserId = userId,
            };
            Init.Client.HeartBeat(req);
        }
    }
}
