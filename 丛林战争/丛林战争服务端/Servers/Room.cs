using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 丛林战争服务端.Servers
{
    enum RoomState
    {
        WaitingJoin,
        WaitingBattle,
        Battle,
        End
    }
    class Room
    {
        private List<Client> clientRoom = new List<Client>();
        private RoomState state = RoomState.WaitingJoin;
        private Server server;
        private const int MAX_HP = 200;
        public Room(Server server)
        {
            this.server = server;
        }
        public void AddClient(Client client)
        {
            client.HP = MAX_HP;
            clientRoom.Add(client);
            client.Room = this;
            if(clientRoom.Count>=2)
            {
                state = RoomState.WaitingBattle;
            }
        }
        public void RemoveClient(Client client)
        {
            client.Room = null;
            clientRoom.Remove(client);
            if (clientRoom.Count >= 2)
            {
                state = RoomState.WaitingBattle;
            }
            else
            {
                state = RoomState.WaitingJoin;
            }
        }
        public string GetHouseOwnerData()
        {
            return clientRoom[0].User.Id + "," + clientRoom[0].User.Username + "," + clientRoom[0].Result.TotalCount + "," + clientRoom[0].Result.WinCount;
        }
        public bool IsWaitingJoin()
        {
            return state == RoomState.WaitingJoin;
        }
        public int GetId()
        {
            if(clientRoom.Count>0)
            {
                return clientRoom[0].User.Id;
            }
            return -1;
        }
        
        public String GetRoomData()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var client in clientRoom)
            {
                sb.Append(client.User.Id + "," + client.User.Username + "," + client.Result.TotalCount + "," + client.Result.WinCount + "|");
            }
            if(sb.Length>0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public void BroadcastMessage(Client excludeClient,ActionCode actionCode,string data)
        {
            foreach (var client in clientRoom)
            {
                if(client!= excludeClient)
                {
                    server.SendResponse(client, actionCode, data);
                }
            }
        }
        public bool IsHouseOwner(Client client)
        {
            return client == clientRoom[0];
        }
        public void Close()
        {
            foreach (var client in clientRoom)
            {
                client.Room = null;
                
            }
            clientRoom.Clear();
            server.RemoveRoom(this);
        }
        public void QuitRoom(Client client)
        {
            if (client == clientRoom[0])
            {
                Close();
            }
            else
            {
                clientRoom.Remove(client);
            }

        }
        public void StartTimer()
        {
            new Thread(RunTimer).Start();
        }
        private void RunTimer()
        {
            Thread.Sleep(1000);
            for (int i = 3;  i>0; i--)
            {
                BroadcastMessage(null,ActionCode.ShowTimer,i.ToString());
                Thread.Sleep(1000);
            }
            BroadcastMessage(null, ActionCode.StartPlay, "r");
        }
        public void TakeDamage(int damage, Client excludeClient)
        {
            bool isDie = false;
            foreach (Client client in clientRoom)
            {
                if (client != excludeClient)
                {
                    if (client.TakeDamage(damage))
                    {
                        isDie = true;
                    }
                }
            }
            if (isDie == false) return;
            //如果其中一个角色死亡，要结束游戏
            foreach (Client client in clientRoom)
            {
                if (client.IsDie())
                {
                    client.UpdataResult(false);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Fail).ToString());
                }
                else
                {
                    client.UpdataResult(true);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Success).ToString());
                }
            }
            Close();
        }
    }                                                                                                                              
}
