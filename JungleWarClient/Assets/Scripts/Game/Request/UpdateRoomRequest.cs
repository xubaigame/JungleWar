using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRoomRequest : BaseRequest {
    private RoomPanel roomPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.UpdateRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }
    public override void OnResponse(string data)
    {
        UserData userData1;
        UserData userData2;
        string[] udStrArray = data.Split('|');
        userData1 = new UserData(udStrArray[0]);
        string ud1 = userData1.Username + "," + userData1.TotalCount + "," + userData1.WinCount;
        SyncManager.Instace.AddListener(ud1, roomPanel.SetLocalPlayerRes);
        if (udStrArray.Length>1)
        {
            userData2 = new UserData(udStrArray[1]);
            string ud2 = userData2.Username + "," + userData2.TotalCount + "," + userData2.WinCount;
            SyncManager.Instace.AddListener(ud2, roomPanel.SetEnemyPlayerRes);
        }
        else
        {
            SyncManager.Instace.AddListener(roomPanel.ClearEnemyPlayerRes);
        }
    }
}
