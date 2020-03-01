using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.rpc
{
    class _User
    {
        public static long CreateUser(string nickName, string password)
        {
            var req = new CreateUserReq()
            {
                NickName = nickName,
                Password = password
            };
            var resp = _Init.Client.CreateUser(req);
            Console.WriteLine(resp);
            return resp.UserId;
        }

        public static List<User> SearchUser(long userId, string data)
        {
            var req = new SearchUserReq()
            {
                UserId = userId,
                Data = data
            };
            var resp = _Init.Client.SearchUser(req);
            return resp.Users;
        }
    }
}
