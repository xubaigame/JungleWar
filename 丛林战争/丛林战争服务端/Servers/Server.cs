using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using 丛林战争服务端.Controller;

namespace 丛林战争服务端.Servers
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;
        private List<Client> clientList;
        private List<Room> roomList;
        private ControllerManager controllerManager;

        public Server()
        {
            controllerManager = new ControllerManager(this);
        }

        public Server(string IP, int Port)
        {
            controllerManager=new ControllerManager(this);
            SetIpAndPort(IP, Port);
        }

        public void SetIpAndPort(string IP, int Port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(IP), Port);
        }

        public void Start()
        {
            serverSocket=new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(0);
            clientList=new List<Client>();
            roomList = new List<Room>();
            serverSocket.BeginAccept(WaitClientConnect,null);
            Console.ReadKey();
        }

        private void WaitClientConnect(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            client.Start();
            clientList.Add(client);
            serverSocket.BeginAccept(WaitClientConnect, null);
        }

        public void RemoveClient(Client client)
        {
            lock (clientList)
            {
                clientList.Remove(client);
            }
        }

        public void SendResponse(Client client, ActionCode actionCode, string data)
        {
            //相应客户端
            client.Send(actionCode, data);
        }

        public void HandlerRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode,actionCode,data,client);
        }

        public void CreateRoom(Client client)
        {
            Room room = new Room(this);
            room.AddClient(client);
            roomList.Add(room);
        }
        public void RemoveRoom(Room room)
        {
            if(roomList!=null&&room!=null)
            {
                roomList.Remove(room);
            }
        }
        public List<Room>GetRoomList()
        {
            return roomList;
        }

        public Room GetRoomById(int id)
        {
            foreach (var room in roomList)
            {
                if (room.GetId() == id)
                    return room;
            }
            return null;
        }
    }
}
