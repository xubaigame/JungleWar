using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 丛林战争服务端.Model;
using 丛林战争服务端.Tool;

namespace 丛林战争服务端.DAO
{
    class ResultDAO
    {
        public Result GetResultByUserid(DBHelper dBHelper, int userId)
        {
            try
            {
                string sql = "select * from result where userid = @userid";
                MySqlParameter[] ps =
               {
                new MySqlParameter("@userid",userId)
            };
                DataTable dt = dBHelper.ExecuteTable(sql, System.Data.CommandType.Text, ps);
                if (dt.Rows.Count > 0)
                {
                    int id = Convert.ToInt32(dt.Rows[0]["id"]);
                    int totalCount = Convert.ToInt32(dt.Rows[0]["totalcount"]);
                    int winCount = Convert.ToInt32(dt.Rows[0]["wincount"]);

                    Result res = new Result(id, userId, totalCount, winCount);
                    return res;
                }
                else
                {
                    Result res = new Result(-1, userId, 0, 0);
                    return res;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetResultByUserid的时候出现异常：" + e);
                return null;
            }

        }
        public void UpdateOrAddResult(DBHelper dBHelper, Result res)
        {
            try
            {
                string sql;
                if (res.Id <= -1)
                {
                    sql="insert into result set totalcount=@totalcount,wincount=@wincount,userid=@userid";
                }
                else
                {
                    sql = "update result set totalcount=@totalcount,wincount=@wincount where userid=@userid ";
                }
                MySqlParameter[] ps =
               {
                new MySqlParameter("@totalcount",res.TotalCount),
                new MySqlParameter("@wincount",res.WinCount),
                new MySqlParameter("@userid",res.UserId)
            };
                dBHelper.ExecutNonQuery(sql, CommandType.Text, ps);
                if (res.Id <= -1)
                {
                    Result tempRes = GetResultByUserid(dBHelper, res.UserId);
                    res.Id = tempRes.Id;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在UpdateOrAddResult的时候出现异常：" + e);
            }
        }
    }

}