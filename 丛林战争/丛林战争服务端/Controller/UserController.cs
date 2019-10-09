using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using 丛林战争服务端.DAO;
using 丛林战争服务端.Model;
using 丛林战争服务端.Servers;

namespace 丛林战争服务端.Controller
{
    class UserController : BaseController
    {
        private UserDAO userDAO = new UserDAO();
        private ResultDAO resultDAO = new ResultDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }
        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            User user= userDAO.VerifyUser(dbHelper,strs[0], strs[1]);
            if(user==null)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                Result res = resultDAO.GetResultByUserid(dbHelper, user.Id);
                client.SetUserData(user, res);
                return string.Format("{0},{1},{2},{3}", ((int)ReturnCode.Success).ToString(), user.Username, res.TotalCount, res.WinCount);
            }
        }
        public string Register(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            string username = strs[0];
            string password = strs[1];
            bool res = userDAO.GetUserByUsername(dbHelper, username);
            if(res)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            userDAO.AddUser(dbHelper, username, password);
            return ((int)ReturnCode.Success).ToString();
        }
    }
}
