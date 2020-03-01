using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Poseidon.infra.sqlite;
using System.Windows.Forms;

namespace Poseidon
{
    public static class Class1
    {
        public static string Ip = "192.168.6.128";

        public static Int64 UserId;

        public static SQLiteDB sql = new SQLiteDB();

        public static Dictionary<long, frm_chat> formChatPool = new Dictionary<long, frm_chat>();

        public enum AddFriendStatus
        {
            Pending = 0,
            Rejected = 1,
            Accepted = 2
        }

        public static void GridAdd(DataGridView dgv, Dictionary<string, object> dict)
        {
            var idx = dgv.Rows.Add(new DataGridViewRow());
            foreach (var item in dict)
                dgv.Rows[idx].Cells[item.Key].Value = item.Value;
        }

        public static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        public static DateTime StampToDateTime(long timestamp)
        {
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            return dateTime.AddSeconds(timestamp).ToLocalTime();
        }
        public static long DateTimeToStamp(DateTime dataTime)
        {
            return (long)(dataTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }
        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
