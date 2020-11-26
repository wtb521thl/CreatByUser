using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tianbo.Wang
{
    public class OutMoveMiddle : OutLine
    {

        protected override void SetAnchoredPos()
        {
            selfRect.anchorMax = new Vector2(0.5f, 0.5f);
            selfRect.anchorMin = new Vector2(0.5f, 0.5f);
        }

        public override void RefreshRect(float lineWidth, Color lineColor)
        {
            lineObjRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, selfRect.GetSize().x);
            lineObjRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, selfRect.GetSize().y);
            lineObjRect.position = selfRect.GetCenter();
            Image tempImage = lineObjRect.GetComponent<Image>();//中间部分没有颜色不适用LineColor
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
            base.DragLine();
            offset = new Vector2(Input.mousePosition.x - startDragMousePos.x, Input.mousePosition.y - startDragMousePos.y);
            selfRect.position = startDragPos + offset;
        }
    }

}