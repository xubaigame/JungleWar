using Common;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel {

    private RectTransform battleRes;
    private RectTransform roomList;
    private VerticalLayoutGroup roomLayout;
    private GameObject roomItemPrefab;
    RoomPanel roomPanel;

    private ListRoomRequest listRoomRequest;
    private CreateRoomRequest createRoomRequest;
    private JoinRoomRequest joinRoomRequest;
    private void Awake()
    {
        battleRes = transform.Find("BattleRes").GetComponent<RectTransform>();
        roomList = transform.Find("RoomList").GetComponent<RectTransform>();
        roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;
        transform.Find("RoomList/CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
        transform.Find("RoomList/CreateRoomButton").GetComponent<Button>().onClick.AddListener(OnCreateRoomClick);
        transform.Find("RoomList/RefreshButton").GetComponent<Button>().onClick.AddListener(OnRefreshClick);
        listRoomRequest = GetComponent<ListRoomRequest>();
        createRoomRequest = GetComponent<CreateRoomRequest>();
        joinRoomRequest = GetComponent<JoinRoomRequest>();
    }
    public override void OnEnter()
    {
        SetBattleRes();
        listRoomRequest.SendRequest();
        EnterAnim();
    }

    public override void OnExit()
    {
        HideAnim();
    }
    public override void OnPause()
    {
        HideAnim();
    }
    public override void OnResume()
    {
        listRoomRequest.SendRequest();
        EnterAnim();
    }

    private void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
    }
    private void OnCreateRoomClick()
    {
        uiMng.PushPanel(UIPanelType.Room);
        createRoomRequest.SendRequest();
    }
    private void OnRefreshClick()
    {
        listRoomRequest.SendRequest();
    }

    private void EnterAnim()
    {
        gameObject.SetActive(true);
        battleRes.localPosition = new Vector3(-1000, 0);
        battleRes.DOLocalMoveX(-290, 0.5f);

        roomList.localPosition = new Vector3(1000, 0);
        roomList.DOLocalMoveX(171, 0.5f);
    }
    private void HideAnim()
    {
        battleRes.DOLocalMoveX(-1000, 0.5f);

        roomList.DOLocalMoveX(1000, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
    private void SetBattleRes()
    {
        UserData ud = GameFacade.Instance.GetUserData();
        transform.Find("BattleRes/Username").GetComponent<Text>().text = ud.Username;
        transform.Find("BattleRes/TotalCount").GetComponent<Text>().text = "总场数:" + ud.TotalCount.ToString();
        transform.Find("BattleRes/WinCount").GetComponent<Text>().text = "胜利:" + ud.WinCount.ToString();
    }
    public void LoadRoomItem(List<UserData> udList)
    {
        RoomItem[] riArray = roomLayout.GetComponentsInChildren<RoomItem>();
        foreach (RoomItem ri in riArray)
        {
            ri.DestroySelf();
        }
        int count = udList.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject roomItem = GameObject.Instantiate(roomItemPrefab);
            roomItem.transform.SetParent(roomLayout.transform);
            UserData ud = udList[i];
            roomItem.GetComponent<RoomItem>().SetRoomInfo(ud.Id, ud.Username, ud.TotalCount, ud.WinCount, this);
        } 
        int roomCount = GetComponentsInChildren<RoomItem>().Length;
        Vector2 size = roomLayout.GetComponent<RectTransform>().sizeDelta;
        roomLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            roomCount * (roomItemPrefab.GetComponent<RectTransform>().sizeDelta.y + roomLayout.spacing));
    
    }
    public void OnJoinClick(int id)
    {
        joinRoomRequest.SendRequest(id);
    }
    public void OnJoinResponse(ReturnCode returnCode,UserData userData1,UserData userData2)
    {
        switch (returnCode)
        {
            case ReturnCode.Success:
                SyncManager.Instace.AddListener(UIPanelType.Room, uiMng.PushPanel);
                Thread.Sleep(100);
                SyncManager.Instace.AddListener(SetRoomInfo);
                Thread.Sleep(100);
                string ud1 = userData1.Username + "," + userData1.TotalCount + "," + userData1.WinCount;
                string ud2 = userData2.Username + "," + userData2.TotalCount + "," + userData2.WinCount;
                SyncManager.Instace.AddListener(ud1, roomPanel.SetLocalPlayerRes);
                SyncManager.Instace.AddListener(ud2, roomPanel.SetEnemyPlayerRes);
                break;
            case ReturnCode.Fail:
                SyncManager.Instace.AddListener("房间已满", uiMng.ShowMessage);
                break;
            case ReturnCode.NotFound:
                SyncManager.Instace.AddListener("房间已被销毁", uiMng.ShowMessage);
                break;
        }
    }
    public void SetRoomInfo()
    {
        roomPanel = GameObject.Find("RoomPanel(Clone)").GetComponent<RoomPanel>();
    }
    public void OnUpdateResultResponse(int totalCount, int winCount)
    {
        GameFacade.Instance.UpdateResult(totalCount, winCount);
        SetBattleRes();
    }
}
