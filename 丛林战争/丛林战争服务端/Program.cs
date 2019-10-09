using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using 丛林战争服务端.Model;
using 丛林战争服务端.Servers;
using 丛林战争服务端.Tool;

namespace 丛林战争服务端
{
    class Program
    {
        static void Main(string[] args)
        {
            Config config = new Config();
            Server server=new Server();
            server.SetIpAndPort(Config.IPAddress, Config.IPPort);
            server.Start();
        }
    }
}
