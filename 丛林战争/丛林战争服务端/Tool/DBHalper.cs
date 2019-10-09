using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace 丛林战争服务端.Tool
{
    interface DBHelper
    {
        int ExecutNonQuery(string sql, CommandType type, params DbParameter[] ps);
        object ExecuteScalar(string sql, CommandType type, params DbParameter[] ps);
        DbDataReader ExecuteReader(string sql, CommandType type, params DbParameter[] ps);
        DataTable ExecuteTable(string sql, CommandType type, params DbParameter[] ps);
    }
        
}
