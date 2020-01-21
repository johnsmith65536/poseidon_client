using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Transport;
using static Server;

namespace Poseidon.rpc
{
    class Loginout
    {
        public static bool Login(Int64 userId,string password)
        {
            var req = new LoginReq()
            {
                UserId = userId,
                Password = password
            };
            var resp = Init.Client.Login(req);
            Console.WriteLine(resp);
            return resp.Success;
        }
    }
}
