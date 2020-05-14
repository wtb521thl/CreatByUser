using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleMono<GameManager>
{
    public enum GameMode
    {
        Editor,
        Game
    }
    private GameMode gameMode;

    public static Dictionary<EventSendType, List<string>> allEventTypeEvent = new Dictionary<EventSendType, List<string>>();

    /// <summary>
    /// 当前选中的物体
    /// </summary>
    public GameObject selectGameobject;



    public void SetGameMode(GameMode tempGameMode)
    {
        gameMode = tempGameMode;
        EventCenter.BroadcastEvent<GameMode>(EventSendType.ChangeGameMode, gameMode);
    }
    public GameMode GetGameMode()
    {
       return gameMode;
    }

}
