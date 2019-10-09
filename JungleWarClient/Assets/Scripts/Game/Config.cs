using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{
    private static string serverIP;
    private static int serverPort;

    public static string ServerIP
    {
        get
        {
            return serverIP;
        }
    }
    public static int ServerPort
    {
        get
        {
            return serverPort;
        }
    }

    static Config()
    {
        serverIP = "xxx.xxx.xxx.xxx";
        serverPort = xxxx;
    }
}
