using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Common;

public class SyncManager : MonoBehaviour {

    public static SyncManager Instace;
    private List<Action> List0;
    private Dictionary<string ,Action<string>>List1;
    private Dictionary<UIPanelType, Action<UIPanelType>> List2;
    private Dictionary<string, Action<string, string, string>> List3;
    private Dictionary<List<UserData>, Action<List<UserData>>> List4;
    private Dictionary<ReturnCode, Action<ReturnCode>> List5;
    private void Awake()
    {
        Instace = this;
        List0 = new List<Action>();
        List1 = new Dictionary<string, Action<string>>();
        List2 = new Dictionary<UIPanelType, Action<UIPanelType>>();
        List3 = new Dictionary<string, Action<string, string, string>>();
        List4 = new Dictionary<List<UserData>, Action<List<UserData>>>();
        List5 = new Dictionary<ReturnCode, Action<ReturnCode>>();
    }
    
    private void Update()
    {
        if(List0.Count>0)
        {
            List0[0]();
            List0.RemoveAt(0);
        }
        if (List1.Count > 0)
        {
            List1[List1.Keys.First()](List1.Keys.First());
            List1.Remove(List1.Keys.First());
        }
        if (List2.Count > 0)
        {
            List2[List2.Keys.First()](List2.Keys.First());
            List2.Remove(List2.Keys.First());
        }
        if (List3.Count > 0)
        {
            string[] strs = List3.Keys.First().Split(',');
            List3[List3.Keys.First()](strs[0], strs[1], strs[2]);
            List3.Remove(List3.Keys.First());
        }
        if (List4.Count > 0)
        {
            List4[List4.Keys.First()](List4.Keys.First());
            List4.Remove(List4.Keys.First());
        }
        if(List5.Count>0)
        {
            List5[List5.Keys.First()](List5.Keys.First());
            List5.Remove(List5.Keys.First());
        }
    }
    public void AddListener(Action action)
    {
        List0.Add(action);
    }
    public void AddListener(string s, Action<string> action)
    {
        List1.Add(s,action);
    }
    public void AddListener(UIPanelType s, Action<UIPanelType> action)
    {
        List2.Add(s,action);
    }
    public void AddListener(string s, Action<string,string,string> action)
    {
        List3.Add(s, action);
    }
    public void AddListener(List<UserData> s, Action<List<UserData>> action)
    {
        List4.Add(s, action);
    }
    public void AddListener(ReturnCode s, Action<ReturnCode> action)
    {
        List5.Add(s, action);
    }
}
