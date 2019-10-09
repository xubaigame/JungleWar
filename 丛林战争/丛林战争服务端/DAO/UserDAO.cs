using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 丛林战争服务端.Tool;
using MySql.Data.MySqlClient;
using System.Data;
using 丛林战争服务端.Model;

namespace 丛林战争服务端.DAO
{
    class UserDAO
    {
        public User VerifyUser(DBHelper dBHelper, string username,string password)
        {
            try
            {
                string sql = "select * from user where username =@username and password =@password";
                MySqlParameter[] ps =
                {
                new MySqlParameter("@username",username),
                new MySqlParameter("@password",password)
            };
                DataTable dt = dBHelper.ExecuteTable(sql, System.Data.CommandType.Text, ps);
                if (dt.Rows.Count > 0)
                {
                    User user = new User();
                    user.Id = Convert.ToInt32(dt.Rows[0]["id"]);
                    user.Username = dt.Rows[0]["username"].ToString();
                    user.Password = dt.Rows[0]["password"].ToString();
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在VerifyUser中发生异常"+e);
                return null;
            }
            
        }
        public bool GetUserByUsername(DBHelper dBHelper, string username)
        {
            try
            {
                string sql = "select * from user where username =@username";
                MySqlParameter[] ps =
                {
                new MySqlParameter("@username",username),
            };
                DataTable dt = dBHelper.ExecuteTable(sql, System.Data.CommandType.Text, ps);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在GetUserByUsername中发生异常" + e);
                return false;
            }
        }

        public void AddUser(DBHelper dBHelper, string username, string password)
        {
            try
            {
                string sql = "insert into user(username,password) values(@username,@password)";
                MySqlParameter[] ps =
                {
                new MySqlParameter("@username",username),
                new MySqlParameter("@password",password)
                };
                int result = dBHelper.ExecutNonQuery(sql, System.Data.CommandType.Text, ps);
            }
            catch (Exception e)
            {
                Console.WriteLine("在AddUser中发生异常" + e);
            }
        }
    }
}
