using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.rpc
{
    class _Object
    {
        public static CreateObjectResp CreateObject(string eTag,string name)
        {
            var req = new CreateObjectReq()
            {
                ETag = eTag,
                Name = name
            };
            return _Init.Client.CreateObject(req);
        }
    }
}
