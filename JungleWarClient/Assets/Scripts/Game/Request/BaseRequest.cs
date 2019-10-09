using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class BaseRequest : MonoBehaviour
{
    protected RequestCode requestCode = RequestCode.None;
    protected ActionCode actionCode = ActionCode.None;
    public virtual void Awake()
    {
        GameFacade.Instance.AddRequest(actionCode, this);
    }

    public void SendRequest(string data)
    {
        GameFacade.Instance.SendMessage(requestCode, actionCode, data);
    }
    public virtual void OnResponse(string data) { }

    public void OnDestroy()
    {
        if(GameFacade.Instance!=null)
            GameFacade.Instance.RemoveRequest(actionCode);
    }
}
