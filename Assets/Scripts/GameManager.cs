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

    public string[] rightButtonsName;

    public static Dictionary<EventSendType, List<string>> allEventTypeEvent = new Dictionary<EventSendType, List<string>>();

    /// <summary>
    /// 当前选中的物体
    /// </summary>
    public GameObject selectGameobject;
    public GameObject lastSelectGameObject;

    public void SetGameMode(GameMode tempGameMode)
    {
        gameMode = tempGameMode;
        EventCenter.BroadcastEvent<GameMode>(EventSendType.ChangeGameMode, gameMode);
    }
    public GameMode GetGameMode()
    {
       return gameMode;
    }


    public static string ChangeJsonDataToChinese(string content)
    {
        return System.Text.RegularExpressions.Regex.Unescape(content).Trim('"');
    }




    /// <summary>
    /// 画出边界
    /// </summary>
    /// <param name="bounds"></param>
    public void DrawBounds(Bounds bounds, Color lineColor)
    {
        Debug.DrawLine(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z),
            new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z), lineColor);

        Debug.DrawLine(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z),
            new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z), lineColor);

        Debug.DrawLine(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z),
            new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z), lineColor);

        Debug.DrawLine(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z),
            new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z), lineColor);


        Debug.DrawLine(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z),
            new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z), lineColor);

        Debug.DrawLine(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z),
            new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z), lineColor);

        Debug.DrawLine(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z),
            new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z), lineColor);

        Debug.DrawLine(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z),
            new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z), lineColor);


        Debug.DrawLine(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z),
            new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z), lineColor);

        Debug.DrawLine(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z),
            new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z), lineColor);

        Debug.DrawLine(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z),
            new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z), lineColor);

        Debug.DrawLine(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z),
            new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z), lineColor);

    }


}
