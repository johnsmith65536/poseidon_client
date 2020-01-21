using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.rpc
{
    class Relation
    {
        public static Tuple<long,long> AddFriend(long userIdSend, long userIdRecv)
        {
            var req = new AddFriendReq()
            {
                UserIdSend = userIdSend,
                UserIdRecv = userIdRecv
            };
            var resp = Init.Client.AddFriend(req);
            return new Tuple<long, long>(resp.Id, resp.CreateTime);
            
        }
        public static void ReplyAddFriend(long id, int status)
        {
            var req = new ReplyAddFriendReq()
            {
                Id = id,
                Status = status
            };
            var resp = Init.Client.ReplyAddFriend(req);
        }
        public static Tuple<List<long>,List<long>> FetchFriendList(long userId)
        {
            var req = new FetchFriendsListReq()
            {
                UserId = userId
            };
            var resp = Init.Client.FetchFriendsList(req);
            return new Tuple<List<long>, List<long>>(resp.OnlineUserIds,resp.OfflineUserIds);
        }
    }
}
