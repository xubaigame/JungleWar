using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitBattleRequest : BaseRequest {
    private GamePanel gamePanel;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.QuitBattle;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }
    public void SendRequest()
    {
        base.SendRequest("r");
    }
    public override void OnResponse(string data)
    {
        SyncManager.Instace.AddListener(gamePanel.OnExitResponse);
    }
}
