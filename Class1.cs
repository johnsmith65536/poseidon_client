using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Poseidon.infra.sqlite;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Collections;
using System.Net;

namespace Poseidon
{
    public static class Class1
    {
        public const string Ip = "192.168.6.128";

        public const string HttpPort = ":8081";

        public const string EndPoint = "oss-cn-shenzhen.aliyuncs.com";
        public const string BucketName = "poseidon-data";

        public static Int64 UserId;

        public static SQLiteDB sql = new SQLiteDB();

        public static Dictionary<long, frm_chat> formChatPool = new Dictionary<long, frm_chat>();

        public enum AddFriendStatus
        {
            Pending = 0,
            Rejected = 1,
            Accepted = 2
        }

        public enum ContentType
        {
            Text = 0,
            Object = 1
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

        public static string GenerateMD5WithFilePath(string filePath)
        {
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash_byte = md5.ComputeHash(file);
            string str = System.BitConverter.ToString(hash_byte);
            str = str.Replace("-", "");
            file.Close();
            return str;
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

        public static void InsertObject(long id,string eTag,string name)
        {
            DataTable dt = Class1.sql.SqlTable($"SELECT * FROM `object` WHERE id = {id}");
            if (dt.Rows.Count != 0)
                return;
            bool ret = Class1.sql.ExecuteNonQuery($"INSERT INTO `object`(id, e_tag, name) VALUES({id},'{eTag}', '{name}')");
            if (!ret)
            {
                MessageBox.Show("DB错误，INSERT INTO object失败", "信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        public static string DoHttpRequest(string url, string method, Hashtable header = null, string data = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + Class1.Ip + Class1.HttpPort + url);
            request.Method = method;
            request.ContentType = "application/json;charset=utf-8";
            if (header != null)
                foreach (var i in header.Keys)
                    request.Headers.Add(i.ToString(), header[i].ToString());
            if (!string.IsNullOrEmpty(data))
            {
                Stream RequestStream = request.GetRequestStream();
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                RequestStream.Write(bytes, 0, bytes.Length);
                RequestStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream ResponseStream = response.GetResponseStream();
            StreamReader StreamReader = new StreamReader(ResponseStream, Encoding.GetEncoding("utf-8"));
            string re = StreamReader.ReadToEnd();
            StreamReader.Close();
            ResponseStream.Close();
            return re;
        }

    }
}
