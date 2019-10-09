using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum ActionCode
    {
        None,
        Login,
        Register,
        ListRoom,
        CreateRoom,
        JoinRoom,
        QuitRoom,
        UpdateRoom,
        StartGame,
        ShowTimer,
        StartPlay,
        Move,
        Shoot,
        Attack,
        GameOver,
        UpdateResult,
        QuitBattle
    }
}
