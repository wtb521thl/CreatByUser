using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


#region 四周的线
public class OutLineUp: OutLine
{

    protected override void SetAnchoredPos()
    {
        selfRect.anchorMax = new Vector2(0.5f, 0);
        selfRect.anchorMin = new Vector2(0.5f, 0);

    }
    public override void RefreshRect(Vector2[] points, float lineWidth)
    {
        lineObjRect.sizeDelta = new Vector2(Mathf.Abs(points[1].x - points[0].x), lineWidth);
        lineObjRect.anchoredPosition = new Vector2(0, selfRect.sizeDelta.y / 2f + lineWidth / 2f);
    }
    protected override void GetStartDragObjPos()
    {
        startDragObjPosY = selfRect.anchoredPosition.y - selfRect.sizeDelta.y / 2f;
    }
    protected override void DragLine()
    {
        newObjDisY = startDragObjSizeDeltaY + (Input.mousePosition.y - startDragMousePos.y);
        selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, startDragObjPosY, newObjDisY);
    }
}

public class OutLineRight : OutLine
{

    protected override void SetAnchoredPos()
    {
        selfRect.anchorMax = new Vector2(0f, 0.5f);
        selfRect.anchorMin = new Vector2(0f, 0.5f);
    }
    public override void RefreshRect(Vector2[] points, float lineWidth)
    {
        lineObjRect.sizeDelta = new Vector2(lineWidth, Mathf.Abs(points[1].y - points[2].y));
        lineObjRect.anchoredPosition = new Vector2(selfRect.sizeDelta.x / 2f + lineWidth / 2f, 0);
    }
    protected override void GetStartDragObjPos()
    {
        startDragObjPosX = selfRect.anchoredPosition.x - selfRect.sizeDelta.x / 2f;
    }
    protected override void DragLine()
    {
        newObjDisX = startDragObjSizeDeltaX + (Input.mousePosition.x - startDragMousePos.x);
        selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, startDragObjPosX, newObjDisX);
    }
}

public class OutLineDown : OutLine
{

    protected override void SetAnchoredPos()
    {
        selfRect.anchorMax = new Vector2(0.5f, 1);
        selfRect.anchorMin = new Vector2(0.5f, 1);
    }
    public override void RefreshRect(Vector2[] points, float lineWidth)
    {
        lineObjRect.sizeDelta = new Vector2(Mathf.Abs(points[3].x - points[2].x), lineWidth);
        lineObjRect.anchoredPosition = new Vector2(0, -selfRect.sizeDelta.y / 2f - lineWidth / 2f);
    }

    protected override void GetStartDragObjPos()
    {
        startDragObjPosY = -selfRect.anchoredPosition.y - selfRect.sizeDelta.y / 2f;
    }

    protected override void DragLine()
    {
        newObjDisY = startDragObjSizeDeltaY - (Input.mousePosition.y - startDragMousePos.y);
        selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, startDragObjPosY, newObjDisY);
    }

}

public class OutLineLeft : OutLine
{

    protected override void SetAnchoredPos()
    {
        selfRect.anchorMax = new Vector2(1, 0.5f);
        selfRect.anchorMin = new Vector2(1, 0.5f);
    }

    public override void RefreshRect(Vector2[] points, float lineWidth)
    {
        lineObjRect.sizeDelta = new Vector2(lineWidth, Mathf.Abs(points[1].y - points[3].y));
        lineObjRect.anchoredPosition = new Vector2(-(selfRect.sizeDelta.x / 2f + lineWidth / 2f), 0);
    }

    protected override void GetStartDragObjPos()
    {
        startDragObjPosX = -selfRect.anchoredPosition.x - selfRect.sizeDelta.x/2f;
    }
    protected override void DragLine()
    {
        newObjDisX = startDragObjSizeDeltaX - (Input.mousePosition.x - startDragMousePos.x);
        selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, startDragObjPosX, newObjDisX);
    }
}
#endregion


#region 四个顶点
public class OutPointLeftUp : OutLine
{
    protected override void SetAnchoredPos()
    {
        selfRect.anchorMax = new Vector2(1, 0);
        selfRect.anchorMin = new Vector2(1, 0);
    }

    public override void RefreshRect(Vector2[] points, float lineWidth)
    {
        lineObjRect.sizeDelta = new Vector2(lineWidth*2, lineWidth*2);
        lineObjRect.position = points[0];
    }

    protected override void GetStartDragObjPos()
    {
        startDragObjPosX = -selfRect.anchoredPosition.x - selfRect.sizeDelta.x/2f ;

        startDragObjPosY = selfRect.anchoredPosition.y- selfRect.sizeDelta.y / 2f;
    }
    protected override void DragLine()
    {
        newObjDisX = startDragObjSizeDeltaX - (Input.mousePosition.x - startDragMousePos.x);
        selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, startDragObjPosX, newObjDisX);

        newObjDisY = startDragObjSizeDeltaY + (Input.mousePosition.y - startDragMousePos.y);
        selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, startDragObjPosY, newObjDisY);
    }
}


public class OutPointRightUp : OutLine
{

    protected override void SetAnchoredPos()
    {
        selfRect.anchorMax = new Vector2(0, 0);
        selfRect.anchorMin = new Vector2(0, 0);
    }

    public override void RefreshRect(Vector2[] points, float lineWidth)
    {
        lineObjRect.sizeDelta = new Vector2(lineWidth * 2, lineWidth * 2);
        lineObjRect.position = points[1];
    }

    protected override void GetStartDragObjPos()
    {
        startDragObjPosX = selfRect.anchoredPosition.x - selfRect.sizeDelta.x / 2f;

        startDragObjPosY = selfRect.anchoredPosition.y - selfRect.sizeDelta.y / 2f;
    }
    protected override void DragLine()
    {
        newObjDisX = startDragObjSizeDeltaX + (Input.mousePosition.x - startDragMousePos.x);
        selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, startDragObjPosX, newObjDisX);

        newObjDisY = startDragObjSizeDeltaY + (Input.mousePosition.y - startDragMousePos.y);
        selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, startDragObjPosY, newObjDisY);
    }
}


public class OutPointRightDown : OutLine
{

    protected override void SetAnchoredPos()
    {

        selfRect.anchorMax = new Vector2(0, 1);
        selfRect.anchorMin = new Vector2(0, 1);
    }

    public override void RefreshRect(Vector2[] points, float lineWidth)
    {
        lineObjRect.sizeDelta = new Vector2(lineWidth * 2, lineWidth * 2);
        lineObjRect.position = points[2];
    }

    protected override void GetStartDragObjPos()
    {
        startDragObjPosX = selfRect.anchoredPosition.x - selfRect.sizeDelta.x / 2f;

        startDragObjPosY = -selfRect.anchoredPosition.y - selfRect.sizeDelta.y/2f ;
    }
    protected override void DragLine()
    {
        newObjDisX = startDragObjSizeDeltaX + (Input.mousePosition.x - startDragMousePos.x);
        selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, startDragObjPosX, newObjDisX);

        newObjDisY = startDragObjSizeDeltaY - (Input.mousePosition.y - startDragMousePos.y);
        selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, startDragObjPosY, newObjDisY);
    }
}


public class OutPointLeftDown : OutLine
{

    protected override void SetAnchoredPos()
    {
        selfRect.anchorMax = new Vector2(1, 1);
        selfRect.anchorMin = new Vector2(1, 1);
    }

    public override void RefreshRect(Vector2[] points, float lineWidth)
    {
        lineObjRect.sizeDelta = new Vector2(lineWidth * 2, lineWidth * 2);
        lineObjRect.position = points[3];
    }

    protected override void GetStartDragObjPos()
    {
        startDragObjPosX = -selfRect.anchoredPosition.x - selfRect.sizeDelta.x / 2f;

        startDragObjPosY = -selfRect.anchoredPosition.y - selfRect.sizeDelta.y / 2f;
    }
    protected override void DragLine()
    {
        newObjDisX = startDragObjSizeDeltaX - (Input.mousePosition.x - startDragMousePos.x);
        selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, startDragObjPosX, newObjDisX);

        newObjDisY = startDragObjSizeDeltaY - (Input.mousePosition.y - startDragMousePos.y);
        selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, startDragObjPosY, newObjDisY);
    }
}

#endregion


#region 中间移动部分


public class OutMoveMiddle : OutLine
{

    protected override void SetAnchoredPos()
    {
        selfRect.anchorMax = new Vector2(0.5f, 0.5f);
        selfRect.anchorMin = new Vector2(0.5f, 0.5f);
    }

    public override void RefreshRect(Vector2[] points, float lineWidth)
    {
        lineObjRect.sizeDelta = new Vector2(Mathf.Abs(points[1].x - points[0].x), Mathf.Abs(points[1].y - points[2].y));
        lineObjRect.position = (points[0] + points[2])/2f;
        Image tempImage = lineObjRect.GetComponent<Image>();
        tempImage.color = new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, 0);
    }
    Vector2 startDragPos;
    protected override void GetStartDragObjPos()
    {
        startDragPos = selfRect.position;
    }

    Vector2 offset;
    protected override void DragLine()
    {
        offset = new Vector2(Input.mousePosition.x - startDragMousePos.x, Input.mousePosition.y - startDragMousePos.y);
        selfRect.position = startDragPos+offset;
    }
}


#endregion