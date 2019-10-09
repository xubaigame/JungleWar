using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class GameFacade : MonoBehaviour
{
	private static GameFacade _instance;
	private UIManager uiMng;
	private AudioManager audioMng;
	private PlayerManager playerMng;
	private CameraManager cameraMng;
	private RequestManager requestMng;
	private ClientManager clientMng;

    private List<Action> beforeDestroyActionList=new List<Action>();
    public static GameFacade Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = GameObject.Find("GameFacade");
                if (obj == null)
                    return null;
                _instance =obj.GetComponent<GameFacade>();
            }
            return _instance;
        }
    }
    void Start ()
	{
		InitManager();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void InitManager()
	{
		uiMng=new UIManager(this);
		audioMng = new AudioManager(this);
		playerMng = new PlayerManager(this);
		cameraMng = new CameraManager(this);
		requestMng = new RequestManager(this);
		clientMng = new ClientManager(this);

		uiMng.OnInit();
		audioMng.OnInit();
		playerMng.OnInit();
		cameraMng.OnInit();
		requestMng.OnInit();
		clientMng.OnInit();
	}

	private void OnDestroy()
	{
        foreach (var item in beforeDestroyActionList)
        {
            item();
        }
		uiMng.OnDestroy();
		audioMng.OnDestroy();
		playerMng.OnDestroy();
		cameraMng.OnDestroy();
		requestMng.OnDestroy();
		clientMng.OnDestroy();
	}
    public void PushPanel(UIPanelType uIPanelType)
    {
        uiMng.PushPanel(uIPanelType);
    }
	public void AddRequest(ActionCode actionCode, BaseRequest baseRequest)
	{
		requestMng.AddRequest(actionCode, baseRequest);
	}
	public void RemoveRequest(ActionCode actionCode)
	{
		requestMng.RemoveRequest(actionCode);
	}
	public void HandleResponse(ActionCode actionCode, string data)
	{
		requestMng.HandleResponse(actionCode, data);
	}

	public void ShowMessage(string msg)
	{
		uiMng.ShowMessage(msg);
	}
	public void SendMessage(RequestCode requestCode, ActionCode actionCode, string data)
	{
		clientMng.SendMessage(requestCode, actionCode, data); 
	}
	public void PlayBgSound(string soundName)
	{
		audioMng.PlayBgSound(soundName);
	}
	public void PlayNormalSound(string soundName)
	{
		audioMng.PlayNormalSound(soundName);
	}
    public void SetUserData(UserData ud)
    {
        playerMng.UserData = ud;
    }
    public UserData GetUserData()
    {
        return playerMng.UserData;
    }
    public void SetCurrentRoleType(RoleType rt)
    {
        playerMng.SetCurrentRoleType(rt);
    }
    public GameObject GetCurrentRoleGameObject()
    {
        return playerMng.GetCurrentRoleGameObject();
    }
    public void EnterPlaying()
    {
        playerMng.SpawnRoles();
        cameraMng.FollowRole();
    }
    public void StartPlaying()
    {
        playerMng.AddControlScript();
        playerMng.CreateSyncRequest();
    }
    public void AddListener( Action action)
    {
        beforeDestroyActionList.Add(action);
    }
    public void SendAttack(int damage)
    {
        playerMng.SendAttack(damage);
    }
    public void GameOver()
    {
        cameraMng.WalkthroughScene();
        playerMng.GameOver();
    }
    public void UpdateResult(int totalCount, int winCount)
    {
        playerMng.UpdateResult(totalCount, winCount);
    }
}
