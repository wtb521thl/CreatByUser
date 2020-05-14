using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InsButton : MonoBehaviour
{
    Button self;

    List<OutLine> outLines = new List<OutLine>();


    private void Awake()
    {
        self = GetComponent<Button>();

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        if (GameManager.Instance.GetGameMode() == GameManager.GameMode.Editor)
        {
            InsOutLine();
        }
        EventCenter.AddListener<GameManager.GameMode>(EventSendType.ChangeGameMode, ChangeGameMode);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<GameManager.GameMode>(EventSendType.ChangeGameMode, ChangeGameMode);
    }
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
            if(outLines[i].lineObj!=null)
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
            outLines[i].RefreshRect(points, 5);
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
        Rect rect = self.image.rectTransform.rect;
        rect.position = (Vector2)self.image.rectTransform.position - rect.size / 2f;
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
}
