using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverRequest : BaseRequest {
    private GamePanel gamePanel;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.GameOver;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }
    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        SyncManager.Instace.AddListener(returnCode, gamePanel.OnGameOverResponse);
    }
}
