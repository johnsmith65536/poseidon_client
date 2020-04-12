using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.infra.sqlite
{
    public class SQLiteDB
    {

        /// <summary>
        /// 创建数据库文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void CreateDBFile(string fileName)
        {
            var dirPath = new FileInfo(fileName).DirectoryName;
            Directory.CreateDirectory(dirPath);
            if (!File.Exists(fileName))
            {
                SQLiteConnection.CreateFile(fileName);
            }
        }
        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void DeleteDBFile(string fileName)
        {
            string path = fileName;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// 生成连接字符串
        /// </summary>
        /// <returns></returns>
        private string CreateConnectionString(string fileName)
        {
            SQLiteConnectionStringBuilder connectionString = new SQLiteConnectionStringBuilder();
            connectionString.DataSource = fileName;

            string conStr = connectionString.ToString();
            return conStr;
        }

        SQLiteConnection m_dbConnection;
        /// <summary>
        /// 连接到数据库
        /// </summary>
        /// <returns></returns>
        public SQLiteConnection Connection(string fileName)
        {
            m_dbConnection = new SQLiteConnection(CreateConnectionString(fileName));

            m_dbConnection.Open();

            return m_dbConnection;
        }

        /*        /// <summary>
                /// 在指定数据库中创建一个table
                /// </summary>
                /// <param name="sql">sql语言，如：create table highscores (name varchar(20), score int)</param>
                /// <returns></returns>
                public bool CreateTable(string sql)
                {
                    try
                    {
                        SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ExecuteNonQuery(" + sql + ")Err:" + ex);
                        return false;
                    }

                }

                /// <summary>
                /// 在指定数据库中删除一个table
                /// </summary>
                /// <param name="tablename">表名称</param>
                /// <returns></returns>
                public bool DeleteTable(string tablename)
                {
                    try
                    {
                        SQLiteCommand cmd = new SQLiteCommand("DROP TABLE IF EXISTS " + tablename, m_dbConnection);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ExecuteNonQuery(DROP TABLE IF EXISTS " + tablename + ")Err:" + ex);
                        return false;
                    }
                }

                /// <summary>
                /// 在指定表中添加列
                /// </summary>
                /// <param name="tablename">表名</param>
                /// <param name="columnname">列名</param>
                /// <param name="ctype">列的数值类型</param>
                /// <returns></returns>
                public bool AddColumn(string tablename, string columnname, string ctype)
                {
                    try
                    {
                        SQLiteCommand cmd = new SQLiteCommand("ALTER TABLE " + tablename + " ADD COLUMN " + columnname + " " + ctype, m_dbConnection);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ExecuteNonQuery(ALTER TABLE " + tablename + " ADD COLUMN " + columnname + " " + ctype + ")Err:" + ex);
                        return false;
                    }
                }*/

        /// <summary>
        /// 执行增删改查操作
        /// </summary>
        /// <param name="sql">查询语言</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string sql)
        {
            try
            {
                SQLiteCommand cmd;
                cmd = new SQLiteCommand(sql, m_dbConnection);
                cmd.ExecuteNonQuery().ToString();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExecuteNonQuery(" + sql + ")Err:" + ex);
                return false;
            }
        }
        public bool ExecuteNonQueryWithBinary(string sql, byte[] param)
        {
            try
            {
                SQLiteCommand cmd;
                cmd = new SQLiteCommand(sql, m_dbConnection);
                cmd.Parameters.Add("@param", DbType.Binary).Value = param;
                cmd.ExecuteNonQuery().ToString();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExecuteNonQuery(" + sql + ")Err:" + ex);
                return false;
            }
        }



        /// <summary>
        /// 返回记录集查询
        /// </summary>
        /// <param name="sql">sql查询语言</param>
        /// <returns>返回查询结果集</returns>
        public DataTable SqlTable(string sql)
        {
            try
            {
                SQLiteCommand sqlcmd = new SQLiteCommand(sql, m_dbConnection);//sql语句
                sqlcmd.CommandTimeout = 120;
                SQLiteDataReader reader = sqlcmd.ExecuteReader();
                DataTable dt = new DataTable();
                if (reader != null)
                {
                    dt.Load(reader, LoadOption.PreserveChanges, null);
                }
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlReader(" + sql + ")Err:" + ex);
                return null;
            }
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void CloseConn()
        {
            try
            {
                if (m_dbConnection.State == ConnectionState.Open)
                    m_dbConnection.Close();
                else if (m_dbConnection.State == ConnectionState.Broken)
                {
                    m_dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("closeConnErr:" + ex);
            }
        }
    }
}
