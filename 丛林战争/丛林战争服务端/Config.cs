using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace 丛林战争服务端
{
     public class Config
    {
        private static string conStr;
        private static string dbType;
        private static string dbNameSpace;
        private static string iPAddress;
        private static int iPPort;
        public static string Constr
        {
            get { return conStr; }
        }
        public static string DbType
        {
            get { return dbType; }
        }
        public static string DbNameSpace
        {
            get { return dbNameSpace; }
        }
        public static string IPAddress
        {
            get { return iPAddress; }
        }
        public static int IPPort
        {
            get { return iPPort; }
        }
        public Config()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            //创建XML文件读取流
            XmlDataDocument xml = new XmlDataDocument();
            //以相对路径将文件读入文件流
            xml.Load(Directory.GetCurrentDirectory() + "\\Config\\ServerConfig.xml");
            //获取XML文件中第一个子节点
            XmlNode node = xml.DocumentElement.FirstChild;
            dbType = node["Type"].InnerXml;
            conStr = node["Sources"].InnerXml;
            dbNameSpace = node["NameSpace"].InnerXml;
            node = xml.SelectSingleNode("Config/Server");
            iPAddress = node["IPAddress"].InnerXml;
            iPPort = int.Parse(node["IPPort"].InnerXml);
        }
    }
}
