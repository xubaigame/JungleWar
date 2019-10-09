using Common;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel {

    private Text timer;
    private int time = -1;
    private Button successBtn;
    private Button failBtn;
    private Button exitBtn;

    private QuitBattleRequest quitBattleRequest;
    private void Awake()
    {
        timer = transform.Find("Timer").GetComponent<Text>();
        timer.gameObject.SetActive(false);
        successBtn = transform.Find("SuccessButton").GetComponent<Button>();
        successBtn.onClick.AddListener(OnResultClick);
        successBtn.gameObject.SetActive(false);
        failBtn = transform.Find("FailButton").GetComponent<Button>();
        failBtn.onClick.AddListener(OnResultClick);
        failBtn.gameObject.SetActive(false);
        exitBtn = transform.Find("ExitButton").GetComponent<Button>();
        exitBtn.onClick.AddListener(OnExitClick);
        exitBtn.gameObject.SetActive(false);
        quitBattleRequest = GetComponent<QuitBattleRequest>();
    }
    public override void OnEnter()
    {
        GameConfig.GameState = GameStates.Battle;
        gameObject.SetActive(true);
    }
    public override void OnExit()
    {
        successBtn.gameObject.SetActive(false);
        failBtn.gameObject.SetActive(false);
        exitBtn.gameObject.SetActive(false);
        gameObject.SetActive(false);
        GameConfig.GameState = GameStates.Waiting;
    }
    private void OnResultClick()
    {
        uiMng.PopPanel();
        uiMng.PopPanel();
        GameFacade.Instance.GameOver();
    }
    private void OnExitClick()
    {
        quitBattleRequest.SendRequest();
    }
    public void OnExitResponse()
    {
        OnResultClick();
    }
    public void ShowTime(string times)
    {
        int time = int.Parse(times);
        if (time == 3)
        {
            exitBtn.gameObject.SetActive(true);
        }
        timer.gameObject.SetActive(true);
        timer.text = time.ToString();
        timer.transform.localScale = Vector3.one;
        Color tempColor = timer.color;
        tempColor.a = 1;
        timer.color = tempColor;
        timer.transform.DOScale(2, 0.3f).SetDelay(0.3f);
        timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() => timer.gameObject.SetActive(false));
        GameFacade.Instance.PlayNormalSound(AudioManager.Sound_Alert);
    }
    public void OnGameOverResponse(ReturnCode returnCode)
    {
        Button tempBtn = null;
        switch (returnCode)
        {
            case ReturnCode.Success:
                tempBtn = successBtn;
                break;
            case ReturnCode.Fail:
                tempBtn = failBtn;
                break;
        }
        tempBtn.gameObject.SetActive(true);
        tempBtn.transform.localScale = Vector3.zero;
        tempBtn.transform.DOScale(1, 0.5f);
        exitBtn.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        if (GameConfig.GameState == GameStates.Battle)
            quitBattleRequest.SendRequest();
    }
}
