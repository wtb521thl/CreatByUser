using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InsButton : MonoBehaviour
{
    Button self;
    OutLine outLineUp;
    OutLine outLineRight;
    OutLine outLineDown;
    OutLine outLineLeft ;

    OutLine outPointLeftUp;
    OutLine outPointRightUp;
    OutLine outPointLeftDown;
    OutLine outPointRightDown;
    private void Awake()
    {
        self = GetComponent<Button>();

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        InsOutLine();
    }

    /// <summary>
    /// 初始化外边框
    /// </summary>
    void InsOutLine()
    {
        outLineUp = OutLineManager.Instance.GetOutLine("Up");
        outLineRight = OutLineManager.Instance.GetOutLine("Right");
        outLineDown = OutLineManager.Instance.GetOutLine("Down");
        outLineLeft = OutLineManager.Instance.GetOutLine("Left");

        outPointLeftUp = OutLineManager.Instance.GetOutLine("LeftUp");
        outPointRightUp = OutLineManager.Instance.GetOutLine("RightUp");
        outPointLeftDown = OutLineManager.Instance.GetOutLine("LeftDown");
        outPointRightDown = OutLineManager.Instance.GetOutLine("RightDown");

        outLineUp.Init(gameObject);
        outLineRight.Init(gameObject);
        outLineDown.Init(gameObject);
        outLineLeft.Init(gameObject);

        outPointLeftUp.Init(gameObject);
        outPointRightUp.Init(gameObject);
        outPointLeftDown.Init(gameObject);
        outPointRightDown.Init(gameObject);
    }

    private void Update()
    {
        Refresh();

        outLineUp.RefreshRect(points, 5);
        outLineRight.RefreshRect(points, 5);
        outLineDown.RefreshRect(points, 5);
        outLineLeft.RefreshRect(points, 5);

        outPointLeftUp.RefreshRect(points, 5);
        outPointRightUp.RefreshRect(points, 5);
        outPointLeftDown.RefreshRect(points, 5);
        outPointRightDown.RefreshRect(points, 5);

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
