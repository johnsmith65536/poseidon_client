using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.rpc
{
    class Messages
    {
        public static Tuple<long,long> SendMessage(Int64 userId, Int64 idRecv, string content, Int32 contentType, Int32 msgType)
        {
            var req = new SendMessageReq()
            {
                UserIdSend = userId,
                IdRecv = idRecv,
                Content = content,
                ContentType = contentType,
                MessageType = msgType
            };
            var resp = Init.Client.SendMessage(req);
            return new Tuple<long,long>(resp.Id,resp.CreateTime);
        }
        public static void MessageDelivered(long msgId)
        {
            var req = new MessageDeliveredReq()
            {
                MsgId = msgId
            };
            var resp = Init.Client.MessageDelivered(req);
        }
        public static Tuple<List<Message>,List<UserRelation>> FetchOfflineMessage(long userId,long messageId,long userRelationId)
        {
            var req = new FetchOfflineMessageReq()
            {
                UserId = userId,
                MessageId = messageId,
                UserRelationId = userRelationId,
            };
            var resp = Init.Client.FetchOfflineMessage(req);
            return new Tuple<List<Message>, List<UserRelation>>(resp.Messages, resp.UserRelations);
        }
    }
}
