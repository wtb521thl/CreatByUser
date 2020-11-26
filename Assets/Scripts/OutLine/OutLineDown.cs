using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tianbo.Wang
{
    public class OutLineDown : OutLine
    {
        protected override void SetAnchoredPos()
        {

        }
        public override void RefreshRect(float lineWidth, Color lineColor)
        {
            lineObjRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, selfRect.GetSize().x);
            lineObjRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lineWidth);
            lineObjRect.position = new Vector2(selfRect.GetCenter().x, selfRect.GetCenter().y - selfRect.GetSize().y / 2f);

            lineObjRect.GetComponent<Image>().color = lineColor;
        }

        protected override void GetStartDragObjPos()
        {
            startDragObjPosY = Screen.height - (selfRect.GetCenter().y + selfRect.GetSize().y / 2f);
        }

        protected override void DragLine()
        {
            base.DragLine();
            newObjDisY = startDragObjSizeDeltaY - (Input.mousePosition.y - startDragMousePos.y);
            selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, startDragObjPosY, newObjDisY);
        }

    }
}