using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Common;
using UnityEngine;

public class ClientManager : BaseManager
{

    private Socket clientSocket;
    private Message msg = new Message();

    public ClientManager(GameFacade facade) : base(facade)
    {

    }
    public override void OnInit()
    {
        base.OnInit();

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(Config.ServerIP, Config.ServerPort);
            Start();
        }
        catch (Exception e)
        {
            Debug.LogError("无法连接到服务器端,请检查您的网络!!" + e);
        }
    }

    private void Start()
    {
        clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);

    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            if (clientSocket == null || clientSocket.Connected == false)
                return;
            int count = clientSocket.EndReceive(ar);

            msg.ReadMessage(count, OnProcessDataCallback);
            Start();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnProcessDataCallback(ActionCode actionCode,string data)
    {
        GameFacade.Instance.HandleResponse(actionCode, data);
    }
    public override void OnDestroy()
    {
        try
        {
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Debug.Log("无法关闭与服务器端的连接,请检查您的网络!!" + e);
        }
    }

    public void SendMessage(RequestCode requestCode, ActionCode actionCode, string data)
    {
        byte[] bytes = Message.PackData(requestCode, actionCode, data);
        clientSocket.Send(bytes);
    }
}
