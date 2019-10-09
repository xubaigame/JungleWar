using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class CreateRoomRequest : BaseRequest {
    RoomPanel roomPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        
        base.Awake();
    }

    public void SendRequest()
    {
        if(roomPanel==null)
            roomPanel = GameObject.Find("RoomPanel(Clone)").GetComponent<RoomPanel>();
        base.SendRequest("r");
    }
    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        RoleType roleType = (RoleType)int.Parse(strs[1]);
        GameFacade.Instance.SetCurrentRoleType(roleType);
        if(returnCode==ReturnCode.Success)
        {
            UserData userData = GameFacade.Instance.GetUserData();
            string s = userData.Username + "," + userData.TotalCount.ToString() + "," + userData.WinCount.ToString();
            SyncManager.Instace.AddListener(s, roomPanel.SetLocalPlayerRes);
            SyncManager.Instace.AddListener(roomPanel.ClearEnemyPlayerRes);
        }
    }
}
