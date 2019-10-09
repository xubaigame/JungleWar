using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class JoinRoomRequest : BaseRequest {
    private RoomListPanel roomListPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    public void SendRequest(int id)
    {
        base.SendRequest(id.ToString());
    }

    public override void OnResponse(string data)
    {
        
        string[] strs = data.Split('-');
        string[] strs2 = strs[0].Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs2[0]);
        
        UserData userData1;
        UserData userData2;
        if (returnCode==ReturnCode.Success)
        {
            string[] udStrArray = strs[1].Split('|');
            userData1 = new UserData(udStrArray[0]);
            userData2 = new UserData(udStrArray[1]);
            roomListPanel.OnJoinResponse(returnCode, userData1, userData2);
            RoleType roleType = (RoleType)int.Parse(strs2[1]);
            GameFacade.Instance.SetCurrentRoleType(roleType);
        }
    }
}
