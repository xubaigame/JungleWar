using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
         
public class PlayerManager : BaseManager {

  

    private Dictionary<RoleType, RoleData> roleDataDict = new Dictionary<RoleType, RoleData>();
    private Transform rolePositions;
    private UserData userData;
    private RoleType currentRoleType;
    private GameObject remoteRoleGameObject;
    private GameObject currentRoleGameObject;
    private GameObject playerSyncRequest;
    private ShootRequest shootRequest;
    private AttackRequest attackRequest;

    public void SetCurrentRoleType(RoleType rt)
    {
        currentRoleType = rt;
    }
    public UserData UserData
    {
        set { userData = value; }
        get { return userData; }
    }
    public PlayerManager(GameFacade facade) : base(facade)
    {
    }
    public override void OnInit()
    {
        rolePositions = GameObject.Find("RolePositions").transform;
        InitRoleDataDict();

    }
    private void InitRoleDataDict()
    {
        roleDataDict.Add(RoleType.Blue, new RoleData(RoleType.Blue, "Hunter_BLUE", "Arrow_BLUE", "Explosion_BLUE", rolePositions.Find("Position1")));
        roleDataDict.Add(RoleType.Red, new RoleData(RoleType.Red, "Hunter_RED", "Arrow_RED", "Explosion_RED", rolePositions.Find("Position2")));
    }
    public void SpawnRoles()
    {
        foreach (var item in roleDataDict.Values)
        {
            GameObject go= GameObject.Instantiate(item.RolePrefab, item.SpawnPosition, Quaternion.identity);
            go.tag = "Player";
            if (item.RoleType==currentRoleType)
            {
                currentRoleGameObject = go;
                currentRoleGameObject.GetComponent<PlayerInfo>().isLocal = true;
            }
            else
            {
                remoteRoleGameObject = go;
            }
        }
    }
    public GameObject GetCurrentRoleGameObject()
    {
        return currentRoleGameObject;
    }
    private RoleData GetRoleData(RoleType rt)
    {
        RoleData roleData = null;
        roleDataDict.TryGetValue(rt, out roleData);
        return roleData;
    }
    public void AddControlScript()
    {
        currentRoleGameObject.AddComponent<PlayerMove>();
        PlayerAttack playerAttack= currentRoleGameObject.AddComponent<PlayerAttack>();
        playerAttack.PlayerManager = this;
        RoleType rt = currentRoleGameObject.GetComponent<PlayerInfo>().roleType;
        RoleData rd = GetRoleData(rt);
        playerAttack.arrowPrefab = rd.ArrowPrefab;
    }
    public void CreateSyncRequest()
    {
        playerSyncRequest=new GameObject("PlayerSyncRequest");
        playerSyncRequest.AddComponent<MoveRequest>().SetLocalPlayer(currentRoleGameObject.transform,currentRoleGameObject.GetComponent<PlayerMove>()).SetRemotePlayer(remoteRoleGameObject.transform);
        shootRequest=playerSyncRequest.AddComponent<ShootRequest>();
        shootRequest.PlayerMng = this;
        attackRequest = playerSyncRequest.AddComponent<AttackRequest>();
    }
    public void Shoot(GameObject arrowPrefab,Vector3 pos,Quaternion rotation)
    {
        GameFacade.Instance.PlayNormalSound(AudioManager.Sound_Timer);
        Arrow arrow=GameObject.Instantiate(arrowPrefab, pos, rotation).GetComponent<Arrow>();
        arrow.isLocal = true;
        shootRequest.SendRequest(arrowPrefab.GetComponent<Arrow>().roleType, pos, rotation.eulerAngles);
    }
    public void RemoteShoot(RoleType rt, Vector3 pos, Vector3 rotation)
    {
        GameObject arrowPrefab = GetRoleData(rt).ArrowPrefab;
        Transform transform= GameObject.Instantiate(arrowPrefab).GetComponent<Transform>();
        transform.position = pos;
        transform.eulerAngles = rotation;
    }
    public void SendAttack(int damage)
    {
        attackRequest.SendRequest(damage);
    }
    public void GameOver()
    {
        GameObject.Destroy(currentRoleGameObject);
        GameObject.Destroy(playerSyncRequest);
        GameObject.Destroy(remoteRoleGameObject);
        shootRequest = null;
        attackRequest = null;
    }
    public void UpdateResult(int totalCount,int winCount)
    {
        UserData.TotalCount = totalCount;
        UserData.WinCount = winCount;
    }
}
