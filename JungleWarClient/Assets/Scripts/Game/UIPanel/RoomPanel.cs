using Common;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel {
    private Text localPlayerUsername;
    private Text localPlayerTotalCount;
    private Text localPlayerWinCount;

    private Text enemyPlayerUsername;
    private Text enemyPlayerTotalCount;
    private Text enemyPlayerWinCount;

    private Transform bluePanel;
    private Transform redPanel;
    private Transform startButton;
    private Transform exitButton;

    private QuitRoomRequest quitRoomRequest;
    private StartGameRequest startGameRequest;
    
    // Use this for initialization
    void Awake () {
        localPlayerUsername = transform.Find("BluePanel/Username").GetComponent<Text>();
        localPlayerTotalCount = transform.Find("BluePanel/TotalCount").GetComponent<Text>();
        localPlayerWinCount = transform.Find("BluePanel/WinCount").GetComponent<Text>();
        enemyPlayerUsername = transform.Find("RedPanel/Username").GetComponent<Text>();
        enemyPlayerTotalCount = transform.Find("RedPanel/TotalCount").GetComponent<Text>();
        enemyPlayerWinCount = transform.Find("RedPanel/WinCount").GetComponent<Text>();
        bluePanel = transform.Find("BluePanel");
        redPanel = transform.Find("RedPanel");
        startButton = transform.Find("StartButton");
        exitButton = transform.Find("ExitButton");
        quitRoomRequest = GetComponent<QuitRoomRequest>();
        startGameRequest = GetComponent<StartGameRequest>();
        transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(OnStartClick);
        transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(OnExitClick);
        GameFacade.Instance.AddListener(BeforeDestroy);
    }
    public override void OnEnter()
    {
        GameConfig.GameState = GameStates.Room;
        EnterAnim();
        
    }
    public override void OnExit()
    {
        GameConfig.GameState = GameStates.Waiting;
        ExitAnim();
    }
    public override void OnPause()
    {
        ExitAnim();
    }
    public override void OnResume()
    {
        EnterAnim();
    }
    // Update is called once per frame
    void Update () {
		
	}
    private void OnStartClick()
    {
        startGameRequest.SendRequest();
    }
    public void OnStartResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail)
        {
            SyncManager.Instace.AddListener("您不是房主，无法开始游戏", uiMng.ShowMessage);
        }
        else if(returnCode == ReturnCode.Success)
        {
            SyncManager.Instace.AddListener(UIPanelType.Game, uiMng.PushPanel);
            SyncManager.Instace.AddListener(GameFacade.Instance.EnterPlaying);
        }
    }
    private void OnExitClick()
    {
        quitRoomRequest.SendRequest();
    }
    public void OnExitResponse()
    {
        SyncManager.Instace.AddListener(uiMng.PopPanel);
    }
    private void EnterAnim()
    {
        gameObject.SetActive(true);
        bluePanel.localPosition = new Vector3(-1000, 0, 0);
        bluePanel.DOLocalMoveX(-174, 0.4f);
        redPanel.localPosition = new Vector3(1000, 0, 0);
        redPanel.DOLocalMoveX(174, 0.4f);
        startButton.localScale = Vector3.zero;
        startButton.DOScale(1, 0.4f);
        exitButton.localScale = Vector3.zero;
        exitButton.DOScale(1, 0.4f);
    }
    private void ExitAnim()
    {
        bluePanel.DOLocalMoveX(-1000, 0.4f);
        redPanel.DOLocalMoveX(1000, 0.4f);
        startButton.DOScale(0, 0.4f);
        exitButton.DOScale(0, 0.4f).OnComplete(() => gameObject.SetActive(false));
    }
    public void SetLocalPlayerRes(string username, string totalCount, string winCount)
    {
        localPlayerUsername.text = username;
        localPlayerTotalCount.text = "总场数：" + totalCount;
        localPlayerWinCount.text = "胜利：" + winCount;
    }
    public void SetEnemyPlayerRes(string username, string totalCount, string winCount)
    {
        enemyPlayerUsername.text = username;
        enemyPlayerTotalCount.text = "总场数：" + totalCount;
        enemyPlayerWinCount.text = "胜利：" + winCount;
    }
    public void ClearEnemyPlayerRes()
    {
        enemyPlayerUsername.text = "";
        enemyPlayerTotalCount.text = "等待玩家加入....";
        enemyPlayerWinCount.text = "";
    }
    private void BeforeDestroy()
    {
        if (GameConfig.GameState==GameStates.Room)
            quitRoomRequest.SendRequest();
    }
}
