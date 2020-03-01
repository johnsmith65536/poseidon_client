using System;

namespace Poseidon.rpc
{
    class _Loginout
    {
        public static bool Login(Int64 userId,string password)
        {
            var req = new LoginReq()
            {
                UserId = userId,
                Password = password
            };
            var resp = _Init.Client.Login(req);
            Console.WriteLine(resp);
            return resp.Success;
        }
    }
}
