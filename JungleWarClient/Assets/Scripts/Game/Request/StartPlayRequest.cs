using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayRequest : BaseRequest {
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.StartPlay;

        base.Awake();
    }
    public override void OnResponse(string data)
    {
        SyncManager.Instace.AddListener(GameFacade.Instance.StartPlaying);
    }

}
