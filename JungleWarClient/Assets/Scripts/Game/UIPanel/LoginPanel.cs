using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;
public class LoginPanel : BasePanel
{
    private Button closeButton;
    private InputField usernameIF;
    private InputField passwordIF;
    private LoginRequest loginRequest;
    private void Awake()
    {
        usernameIF = transform.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        transform.Find("LoginButton").GetComponent<Button>().onClick.AddListener(OnLoginClick);
        loginRequest = GetComponent<LoginRequest>();
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(OnCloseClick);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        EnterAnimation();

        
    }

    public override void OnPause()
    {
        HideAnimation();
    }
    public override void OnResume()
    {
        EnterAnimation();
    }

    public override void OnExit()
    {
        HideAnimation();
    }
    public void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
    }
    private void OnLoginClick()
    {
        PlayClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空 ";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "密码不能为空 ";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg); return;
        }
        loginRequest.SendRequest(usernameIF.text, passwordIF.text);
    }
    
    private void OnRegisterClick()
    {
        PlayClickSound();
        uiMng.PushPanel(UIPanelType.Register);
    }

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            SyncManager.Instace.AddListener("登录成功", uiMng.ShowMessage);
            SyncManager.Instace.AddListener(UIPanelType.RoomList, uiMng.PushPanel);
        }
        else
        {
            SyncManager.Instace.AddListener("用户名或密码错误，无法登录，请重新输入!!", uiMng.ShowMessage);
        }
    }
    private void EnterAnimation()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.2f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.2f);
    }
    private void HideAnimation()
    {
        transform.DOScale(0, 0.3f);
        transform.DOLocalMoveX(1000, 0.3f).OnComplete(() => gameObject.SetActive(false));
    }
}
