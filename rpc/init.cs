using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Transport;

namespace Poseidon.rpc
{
    class Init
    {
        public static TTransport transport = new TSocket("192.168.6.128", 8080);
        public static TProtocol protocol = new TBinaryProtocol(transport);
        public static Server.Client Client = new Server.Client(protocol);
        public static void InitRpcClient()
        {
            transport = new TSocket("192.168.6.128", 8080);
            protocol = new TBinaryProtocol(transport);
            Client = new Server.Client(protocol);
            transport.Open();
        }
        public static void DestoryRpcClient()
        {
            transport.Close();
        }
    }
}
