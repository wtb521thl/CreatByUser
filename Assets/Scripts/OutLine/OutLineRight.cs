using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tianbo.Wang
{
    public class OutLineRight : OutLine
    {

        protected override void SetAnchoredPos()
        {

        }
        public override void RefreshRect(float lineWidth, Color lineColor)
        {
            lineObjRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
            lineObjRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, selfRect.GetSize().y);
            lineObjRect.position = new Vector2(selfRect.GetCenter().x + selfRect.GetSize().x / 2f, selfRect.GetCenter().y);
            lineObjRect.GetComponent<Image>().color = lineColor;
        }
        protected override void GetStartDragObjPos()
        {
            startDragObjPosX = selfRect.GetCenter().x - selfRect.GetSize().x / 2f;
        }
        protected override void DragLine()
        {
            base.DragLine();
            newObjDisX = startDragObjSizeDeltaX + (Input.mousePosition.x - startDragMousePos.x);
            selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, startDragObjPosX, newObjDisX);
        }
    }
}