using Common;
using System;
using System.Net.Sockets;
using System.Reflection;
using 丛林战争服务端.DAO;
using 丛林战争服务端.Model;
using 丛林战争服务端.Tool;

namespace 丛林战争服务端.Servers
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        private Message msg=new Message();
        private ResultDAO resultDAO = new ResultDAO();
        protected DBHelper dbHelper;

        internal User User { get; set; }

        internal Result Result { get; set; }

        internal Room Room { get; set; }

        public int HP
        {
            get; set;
        }
        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            Assembly assembly = Assembly.GetExecutingAssembly();
            dbHelper = assembly.CreateInstance(Config.DbNameSpace + Config.DbType) as MysqlHelper;
        }
       
        
        public void Start()
        {
            if(clientSocket!=null&&clientSocket.Connected)
                clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }
                msg.ReadMessage(count, OnProcessMessage);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
        }

        public void OnProcessMessage(RequestCode requestCode,ActionCode actionCode,string data)
        {
            server.HandlerRequest(requestCode,actionCode,data,this);
        }
        private void Close()
        {
            if(clientSocket!=null)
                clientSocket.Close();
            if (Room != null)
                Room.QuitRoom(this);
            server.RemoveClient(this);
            
        }

        public void Send(ActionCode actionCode, string data)
        {
            byte[] sendData= Message.PackData(actionCode, data);
            clientSocket.Send(sendData);
        }
        public void SetUserData(User user,Result result)
        {
            User = user;
            Result = result;
        }
        public bool IsHouseOwner()
        {
            if(Room!=null)
                return Room.IsHouseOwner(this);
            return false;
        }
        public bool TakeDamage(int damage)
        {
            HP -= damage;
            HP = Math.Max(HP, 0);
            if (HP <= 0) return true;
            return false;
        }
        public bool IsDie()
        {
            return HP <= 0;
        }
        public void UpdataResult(bool isVivtory)
        {
            UpdateResultToDB(isVivtory);
            UpdateResultToClient();
        }
        private void UpdateResultToDB(bool isVivtory)
        {
            Result.TotalCount++;
            if (isVivtory)
            {
                Result.WinCount++;
            }
            resultDAO.UpdateOrAddResult(dbHelper, Result);
        }
        private void UpdateResultToClient()
        {
            Send(ActionCode.UpdateResult, string.Format("{0},{1}", Result.TotalCount, Result.WinCount));
        }
    }
}
