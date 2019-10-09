using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class ShootRequest : BaseRequest
{
    private PlayerManager playerMng;
    private RoleType rt;
    private Vector3 pos;
    private Vector3 rotation;

    public PlayerManager PlayerMng
    {
      
        set
        {
            playerMng = value;
        }
    }

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Shoot;
        base.Awake();
    }
    public void SendRequest(RoleType roleType, Vector3 pos, Vector3 rotation)
    {
        string data = string.Format("{0}|{1},{2},{3}|{4},{5},{6}", (int)roleType, pos.x, pos.y, pos.z, rotation.x, rotation.y, rotation.z);
        base.SendRequest(data);
    }
    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        RoleType rt = (RoleType)int.Parse(strs[0]);
        Vector3 pos = UnityTools.ParseVector3(strs[1]);
        Vector3 rotation = UnityTools.ParseVector3(strs[2]);
        this.rt = rt;
        this.pos = pos;
        this.rotation = rotation;
        SyncManager.Instace.AddListener(Shoot);
    }
    private void Shoot()
    {
        playerMng.RemoteShoot(rt, pos, rotation);
    }
}
