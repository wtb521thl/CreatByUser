using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ComponentType
{
    Button,
    Text,
    Image
}

public class ComponentItem : MonoBehaviour,AllComponentMethods
{
    RectTransform selfRect;

    List<OutLine> outLines = new List<OutLine>();

    public ComponentType componentType;

    /// <summary>
    /// 时间ID区分物体
    /// </summary>
    public string timeID;

    /// <summary>
    /// 时间执行对象Id
    /// </summary>
    public string actionObjId;
    /// <summary>
    /// 事件名称
    /// </summary>
    public string actionStr;
    /// <summary>
    /// 用户输入的值
    /// </summary>
    public string userInputValue;
    /// <summary>
    /// 用户输入的值字体大小
    /// </summary>
    public int fontSize;
    /// <summary>
    /// 图片地址
    /// </summary>
    public string imageUrl;

    private void Awake()
    {
        timeID = DateTime.Now.Ticks.ToString();
        selfRect = GetComponent<RectTransform>();

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        if (GameManager.Instance.GetGameMode() == GameManager.GameMode.Editor)
        {
            InsOutLine();
        }
        EventCenter.AddListener<GameManager.GameMode>(EventSendType.ChangeGameMode, ChangeGameMode);
        EventCenter.AddListener<GameObject, string, string>(EventSendType.InspectorChange, ChangeInspectorAction);

        switch (componentType)
        {
            case ComponentType.Button:
                GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (!string.IsNullOrEmpty(actionObjId) && !string.IsNullOrEmpty(actionStr))
                    {
                        UiManager.Instance.GetGameObjectById(actionObjId).SendMessage(actionStr);
                    }
                    else
                    {
                        Debug.Log("发送事件失败");
                    }
                });
                break;
            case ComponentType.Text:
                break;
            case ComponentType.Image:
                break;
            default:
                break;
        }
    }


    private void OnDestroy()
    {
        EventCenter.RemoveListener<GameManager.GameMode>(EventSendType.ChangeGameMode, ChangeGameMode);
        EventCenter.RemoveListener<GameObject, string, string>(EventSendType.InspectorChange, ChangeInspectorAction);
    }
    /// <summary>
    /// UI改变后修改物体的数值
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    /// <param name="arg3"></param>
    private void ChangeInspectorAction(GameObject arg1, string arg2, string arg3)
    {
        if (arg1 == gameObject)
        {
            switch (arg2)
            {
                case "Name":
                    selfRect.name = arg3;
                    break;
                case "PosVectorX":
                    selfRect.position = new Vector2(float.Parse(arg3), selfRect.position.y);
                    break;
                case "PosVectorY":
                    selfRect.position = new Vector2(selfRect.position.x, float.Parse(arg3));
                    break;
                case "SizeVectorX":
                    selfRect.sizeDelta = new Vector2(float.Parse(arg3), selfRect.sizeDelta.y);
                    break;
                case "SizeVectorY":
                    selfRect.sizeDelta = new Vector2(selfRect.sizeDelta.x, float.Parse(arg3));
                    break;
                case "Action":
                    actionStr = arg3;
                    break;
                case "ActionObject":
                    actionObjId = arg3;
                    break;
                case "UserInput":
                    userInputValue = arg3;
                    GameManager.Instance.selectGameobject.GetComponentInChildren<Text>().text = userInputValue;
                    break;
                case "FontSize":
                    fontSize =int.Parse( arg3);
                    GameManager.Instance.selectGameobject.GetComponentInChildren<Text>().fontSize = fontSize;
                    break;
                case "ImagePath":
                    imageUrl= arg3;
                    break;
            }
        }
    }

    public void Action()
    {
        Debug.Log(transform.name+"Action");
    }

    public void Action1()
    {
        Debug.Log(transform.name + "Action1");
    }

    #region 物体外部轮廓拖动UI
    private void ChangeGameMode(GameManager.GameMode gameMode)
    {
        if (gameMode == GameManager.GameMode.Editor)
        {
            InsOutLine();
        }
        if (gameMode == GameManager.GameMode.Game)
        {
            DeleteAllOutLines();
        }

    }

    private void DeleteAllOutLines()
    {
        for (int i = 0; i < outLines.Count; i++)
        {
            if (outLines[i].lineObj != null)
                DestroyImmediate(outLines[i].lineObj);
        }
        outLines.Clear();
    }

    /// <summary>
    /// 初始化外边框
    /// </summary>
    void InsOutLine()
    {
        outLines.Add(OutLineManager.Instance.GetOutLine("Up"));
        outLines.Add(OutLineManager.Instance.GetOutLine("Right"));
        outLines.Add(OutLineManager.Instance.GetOutLine("Down"));
        outLines.Add(OutLineManager.Instance.GetOutLine("Left"));
        outLines.Add(OutLineManager.Instance.GetOutLine("LeftUp"));
        outLines.Add(OutLineManager.Instance.GetOutLine("RightUp"));
        outLines.Add(OutLineManager.Instance.GetOutLine("LeftDown"));
        outLines.Add(OutLineManager.Instance.GetOutLine("RightDown"));
        outLines.Add(OutLineManager.Instance.GetOutLine("Middle"));
        for (int i = 0; i < outLines.Count; i++)
        {
            outLines[i].Init(gameObject);
        }
    }

    private void Update()
    {
        Refresh();
        for (int i = 0; i < outLines.Count; i++)
        {
            outLines[i].RefreshRect(points, 5,Color.green);
        }
    }
    /// <summary>
    /// 物体的四个边界顶点
    /// </summary>
    Vector2[] points;
    /// <summary>
    /// 刷新获取四周的点（当前使用物体的rectTransform，后续可改为bounds）
    /// </summary>
    void Refresh()
    {
        Rect rect = selfRect.rect;
        rect.position = (Vector2)selfRect.position - rect.size / 2f;
        points = new Vector2[5];
        GetCornerPoint(rect, out points[0], out points[1], out points[2], out points[3]);
    }
    /// <summary>
    /// 在编辑器中画出线
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <param name="p4"></param>
    private void GetCornerPoint(Rect rect, out Vector2 p1, out Vector2 p2, out Vector2 p3, out Vector2 p4)
    {
        p1 = new Vector2(rect.center.x - rect.size.x / 2f, rect.center.y + rect.size.y / 2f);
        p2 = new Vector2(rect.center.x + rect.size.x / 2f, rect.center.y + rect.size.y / 2f);
        p3 = new Vector2(rect.center.x + rect.size.x / 2f, rect.center.y - rect.size.y / 2f);
        p4 = new Vector2(rect.center.x - rect.size.x / 2f, rect.center.y - rect.size.y / 2f);

        Debug.DrawLine(p1, p2, Color.blue);
        Debug.DrawLine(p2, p3, Color.blue);
        Debug.DrawLine(p3, p4, Color.blue);
        Debug.DrawLine(p4, p1, Color.blue);
    }
    #endregion
}
