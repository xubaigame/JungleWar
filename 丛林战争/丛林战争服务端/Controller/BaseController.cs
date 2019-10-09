using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;
using 丛林战争服务端.Servers;
using 丛林战争服务端.Tool;

namespace 丛林战争服务端.Controller
{
    abstract class BaseController
    {
        protected RequestCode requestCode;
        protected DBHelper dbHelper;
        public RequestCode RequestCode
        {
            get { return requestCode; }
        }
        public BaseController()
        {
            requestCode = RequestCode.None;
            Assembly assembly = Assembly.GetExecutingAssembly();
            dbHelper = assembly.CreateInstance(Config.DbNameSpace+Config.DbType) as MysqlHelper;
        }
        public virtual string DefaultHandle(string data,Client client,Server server)
        {
            return null;
        }
    }
}
