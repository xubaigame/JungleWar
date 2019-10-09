using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager:BaseManager {
    private Dictionary<UIPanelType, string> panelPathDict;//保存面板路径
    private Dictionary<UIPanelType, BasePanel> panelDict;//保存实例化以后的面板
    private Stack<BasePanel> panelStack;//保存显示界面的栈
    private MessagePanel msgPanel;

    public void InjectMsgPanel(MessagePanel msgPanel)
    {
        this.msgPanel = msgPanel;
    }
    /////单例模式
    //private static UIManager _instance;
    //public static UIManager instance
    //{
    //    get
    //    {
    //        if (_instance==null)
    //        _instance=new UIManager();
    //        return _instance;
    //    } 
    //}

    public override void OnInit()
    {
        base.OnInit();
        PushPanel(UIPanelType.Message);
        PushPanel(UIPanelType.Start);
    }
    public UIManager(GameFacade facade) : base(facade)
    {
        ParseUIPanelTypeJson();
    }
    //场景中，画布的位置
    private Transform canvasTransfrom;
    public Transform CanvasTransfrom
    {
        get
        {
            if (canvasTransfrom == null)
            {
                canvasTransfrom = GameObject.Find("Canvas").transform;
            }
            return canvasTransfrom;
        }
    }
 
    /// <summary>
    /// 场景显示界面压栈
    /// </summary>
    /// <param name="panelType">面板类型</param>
    public void PushPanel(UIPanelType panelType)
    {
        if(panelStack==null)
            panelStack=new Stack<BasePanel>();
        if(panelStack.Count>0)
        {
            BasePanel basePanel = panelStack.Peek();
            basePanel.OnPause();
        }
        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
    }
    /// <summary>
    /// 窗体弹栈
    /// </summary>
    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();
        if (panelStack.Count <= 0) return;
        BasePanel panel = panelStack.Pop();
        panel.OnExit();
        if (panelStack.Count <= 0) return;
        BasePanel panel2 = panelStack.Peek();
        panel2.OnResume();
    }

    /// <summary>
    /// 创建面板并将其添加至面板字典中
    /// </summary>
    /// <param name="panelType">面板类型</param>
    /// <returns></returns>
    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict=new Dictionary<UIPanelType, BasePanel>();
        }
        //BasePanel panel;
        //panelDict.TryGetValue(panelType, out panel);
        BasePanel panel = panelDict.TryGet(panelType);
        if (panel == null)
        {
            //string path;
            //panelPathDict.TryGetValue(panelType, out path);
            string path = panelPathDict.TryGet(panelType);
            GameObject  instPanel=GameObject.Instantiate(Resources.Load(path)) as GameObject;
            instPanel.transform.SetParent(CanvasTransfrom,false);
            instPanel.GetComponent<BasePanel>().UIMng = this;
            panelDict.Add(panelType,instPanel.GetComponent<BasePanel>());
            panelDict.TryGetValue(panelType, out panel);
        }
        return panel;
    }
    class UIPanelTypeJson//临时类，解析Json
    {
        public List<UIPanelInfo> infoList;
    }
    /// <summary>
    /// 解析Json,获取面板路径
    /// </summary>
    private void ParseUIPanelTypeJson()
    {
        panelPathDict=new Dictionary<UIPanelType, string>();
        TextAsset ta= Resources.Load<TextAsset>("UITypePanel");
        UIPanelTypeJson jsonObject= JsonUtility.FromJson<UIPanelTypeJson>(ta.text);
        foreach (UIPanelInfo item in jsonObject.infoList)
        {
            panelPathDict.Add(item.panelType,item.path);
        }
    }

    public void ShowMessage(string msg)
    {
        if (msgPanel == null)
        {
            Debug.Log("无法显示信息");
        }
        msgPanel.ShowMessage(msg);
    }
    public void LoadRoomItem(List<UserData> userDatas)
    {

    }
}
