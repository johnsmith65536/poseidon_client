using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.rpc
{
    class _Oss
    {
        public static GetSTSInfoResp GetSTSInfo(Int64 userId)
        {
            var req = new GetSTSInfoReq()
            {
                UserId = userId,
            };
            return _Init.Client.GetSTSInfo(req);
        }
    }
}
