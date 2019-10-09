using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateResultRequest : BaseRequest {
    private RoomListPanel roomListPanel;
    private int totalCount;
    private int winCount;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.UpdateResult;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }
    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        totalCount = int.Parse(strs[0]);
        winCount = int.Parse(strs[1]);
        SyncManager.Instace.AddListener(UpdateUserData);
    }
    private void UpdateUserData()
    {
        roomListPanel.OnUpdateResultResponse(totalCount, winCount);
    }
}
