using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using 丛林战争服务端.Servers;

namespace 丛林战争服务端.Tool
{
    class MysqlHelper : DBHelper
    {
        private string str = "";

        public MysqlHelper()
        {
            str = Config.Constr;
        }
        /// <summary>
        /// 对数据库的增加,删除,修改
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">连接类型</param>
        /// <param name="ps">参数列表</param>
        /// <returns></returns>
        public int ExecutNonQuery(string sql, CommandType type, params DbParameter[] ps)
        {
            using (MySqlConnection con = new MySqlConnection(str))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    if (ps != null)
                    {
                        
                        cmd.Parameters.AddRange(ChangParameter(ps));
                    }
                    cmd.CommandType = type;
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 查询首行首列
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">连接类型</param>
        /// <param name="ps">参数列表</param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, CommandType type, params DbParameter[] ps)
        {
            using (MySqlConnection con = new MySqlConnection(str))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    if (ps != null)
                    {
                        cmd.Parameters.AddRange(ChangParameter(ps));
                    }
                    cmd.CommandType = type;
                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 返回查询器
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">连接类型</param>
        /// <param name="ps">参数列表</param>
        /// <returns></returns>
        public  DbDataReader ExecuteReader(string sql, CommandType type, params DbParameter[] ps)
        {
            MySqlConnection con = new MySqlConnection(str);
            using (MySqlCommand cmd = new MySqlCommand(sql, con))
            {
                if (ps != null)
                {
                    cmd.Parameters.AddRange(ChangParameter(ps));
                }
                try
                {
                    cmd.CommandType = type;
                    con.Open();
                    return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                }
                catch (Exception ex)
                {
                    con.Close();
                    con.Dispose();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 查询返回满足条件的集合(表)
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">连接类型</param>
        /// <param name="ps">参数列表</param>
        /// <returns></returns>
        public DataTable ExecuteTable(string sql, CommandType type, params DbParameter[] ps)
        {
            DataTable dt = new DataTable();
            using (MySqlDataAdapter sda = new MySqlDataAdapter(sql, str))
            {
                if (ps != null)
                {
                    sda.SelectCommand.Parameters.AddRange(ChangParameter(ps));

                }
                sda.Fill(dt);
            }
            return dt;
        }

        private MySqlParameter[] ChangParameter(DbParameter[] ps)
        {
            MySqlParameter[] mps=new MySqlParameter[ps.Length];
            for (int i = 0; i < ps.Length; i++)
            {
                mps[i]=ps[i] as MySqlParameter;
            }
            return mps;
        }
    }
}
