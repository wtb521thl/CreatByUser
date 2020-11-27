using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tianbo.Wang
{
    public class OutPointRightUp : OutLine
    {

        protected override void SetAnchoredPos()
        {

        }

        public override void RefreshRect(float lineWidth, Color lineColor)
        {
            lineObjRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth * 2);
            lineObjRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lineWidth * 2);
            lineObjRect.position = new Vector2(selfRect.GetCenter().x + selfRect.GetSize().x / 2f, selfRect.GetCenter().y + selfRect.GetSize().y / 2f);
            lineObjRect.GetComponent<Image>().color = lineColor;
        }


        protected override void GetStartDragObjPos()
        {
            startDragObjPosX = selfRect.GetCenter().x - selfRect.GetSize().x / 2f - (selfRect.parent.GetRectTransform().GetCenter().x - selfRect.parent.GetRectTransform().GetSize().x / 2f);

            startDragObjPosY = (selfRect.GetCenter().y - selfRect.GetSize().y / 2f) - (selfRect.parent.GetRectTransform().GetCenter().y - selfRect.parent.GetRectTransform().GetSize().y / 2f);
        }
        protected override void DragLine()
        {
            base.DragLine();
            newObjDisX = startDragObjSizeDeltaX + (Input.mousePosition.x - startDragMousePos.x);
            selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, startDragObjPosX, newObjDisX);

            newObjDisY = startDragObjSizeDeltaY + (Input.mousePosition.y - startDragMousePos.y);
            selfRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, startDragObjPosY, newObjDisY);
        }
    }

}