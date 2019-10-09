using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class RequestManager : BaseManager {
    private Dictionary<ActionCode, BaseRequest> requestDict=new Dictionary<ActionCode, BaseRequest>();
    public RequestManager(GameFacade facade) : base(facade)
    {

    }

    public void AddRequest(ActionCode actionCode, BaseRequest baseRequest)
    {
        requestDict.Add(actionCode, baseRequest);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        if (requestDict.ContainsKey(actionCode))
        {
            requestDict.Remove(actionCode);
        }
    }

    public void HandleResponse(ActionCode actionCode, string data)
    {
        BaseRequest request;
        requestDict.TryGetValue(actionCode, out request);
        if (request == null)
        {
            Debug.LogWarning("无法获取ActionCode["+ actionCode + "]对应的Request类");
            return;
        }
        request.OnResponse(data);
    }
}
