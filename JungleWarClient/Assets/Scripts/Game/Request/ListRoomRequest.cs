using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListRoomRequest : BaseRequest {

    private RoomListPanel roomListPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.ListRoom;
        roomListPanel=GetComponent<RoomListPanel>();
        base.Awake();
    }

    public void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        List<UserData> userDatas = new List<UserData>();
        if (data != "0")
        {
            
            string[] roomArray = data.Split('|');
            foreach (var room in roomArray)
            {
                string[] strs = room.Split(',');
                UserData userData = new UserData(int.Parse(strs[0]), strs[1], int.Parse(strs[2]), int.Parse(strs[3]));
                userDatas.Add(userData);
            }
        }
        else
        {

        }
        SyncManager.Instace.AddListener(userDatas, roomListPanel.LoadRoomItem);
    }
}
