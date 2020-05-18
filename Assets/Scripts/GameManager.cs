using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

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

    string inspectorDataPath;
    
    public void SetInspectorData(string _key,string _value)
    {
        inspectorDataPath = Application.streamingAssetsPath + "/InspectorData.json";

        DataManager.Instance.SaveDataToFile(_key, _value, inspectorDataPath);

    }
    public Dictionary<string,string> GetInspectorData()
    {
        inspectorDataPath = Application.streamingAssetsPath + "/InspectorData.json";
        
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("Name", DataManager.Instance.GetDataFromFile(inspectorDataPath, "Name", "Button").Trim('"'));
        dic.Add("PosVectorX", DataManager.Instance.GetDataFromFile(inspectorDataPath, "PosVectorX", "").Trim('"'));
        dic.Add("PosVectorY", DataManager.Instance.GetDataFromFile(inspectorDataPath, "PosVectorY", "").Trim('"'));
        dic.Add("Action", DataManager.Instance.GetDataFromFile(inspectorDataPath, "Action", "").Trim('"'));
        dic.Add("ActionObject", DataManager.Instance.GetDataFromFile(inspectorDataPath, "ActionObject", "").Trim('"'));
        return dic;
    }

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
