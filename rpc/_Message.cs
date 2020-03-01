using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.rpc
{
    class _Message
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
            var resp = _Init.Client.SendMessage(req);
            return new Tuple<long,long>(resp.Id,resp.CreateTime);
        }
        public static void UpdateMessageStatus(Dictionary<long, int> message, Dictionary<long, int> userRelationRequest)
        {
            var req = new UpdateMessageStatusReq()
            {
                MessageIds = message,
                UserRelationRequestIds = userRelationRequest
            };
            _Init.Client.UpdateMessageStatus(req);
        }
        public static Tuple<List<Message>,List<UserRelation>,long> SyncMessage(long userId,long messageId,long userRelationId)
        {
            var req = new SyncMessageReq()
            {
                UserId = userId,
                MessageId = messageId,
                UserRelationId = userRelationId,
            };
            var resp = _Init.Client.SyncMessage(req);
            return new Tuple<List<Message>, List<UserRelation>,long>(resp.Messages, resp.UserRelations,resp.LastOnlineTime);
        }
    }
}
