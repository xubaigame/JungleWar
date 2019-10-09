using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates
{
    Waiting,
    Room,
    Battle
}
public static class GameConfig 
{
    private static GameStates gameState;

    public static GameStates GameState
    {
        get
        {
            return gameState;
        }

        set
        {
            gameState = value;
        }
    }
    static GameConfig()
    {
        gameState = GameStates.Waiting;
    }

}
