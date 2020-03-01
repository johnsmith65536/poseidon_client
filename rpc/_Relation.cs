using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.rpc
{
    class _Relation
    {
        public static Tuple<long,long,int> AddFriend(long userIdSend, long userIdRecv)
        {
            var req = new AddFriendReq()
            {
                UserIdSend = userIdSend,
                UserIdRecv = userIdRecv
            };
            var resp = _Init.Client.AddFriend(req);
            return new Tuple<long, long,int>(resp.Id, resp.CreateTime,resp.StatusCode);
            
        }
        public static Tuple<long,long> ReplyAddFriend(long id, int status)
        {
            var req = new ReplyAddFriendReq()
            {
                Id = id,
                Status = status
            };
            var resp = _Init.Client.ReplyAddFriend(req);
            return new Tuple<long, long>(resp.Id,resp.CreateTime);
        }
        public static Tuple<List<long>,List<long>> FetchFriendList(long userId)
        {
            var req = new FetchFriendsListReq()
            {
                UserId = userId
            };
            var resp = _Init.Client.FetchFriendsList(req);
            return new Tuple<List<long>, List<long>>(resp.OnlineUserIds,resp.OfflineUserIds);
        }
    }
}
