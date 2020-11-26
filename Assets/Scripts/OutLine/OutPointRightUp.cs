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
            selfRect.anchorMax = new Vector2(0, 0);
            selfRect.anchorMin = new Vector2(0, 0);
        }

        public override void RefreshRect(float lineWidth, Color lineColor)
        {
            lineObjRect.sizeDelta = new Vector2(lineWidth * 2, lineWidth * 2);
            lineObjRect.position = selfRect.GetCenter() + new Vector3(selfRect.GetSize().x / 2f, selfRect.GetSize().y / 2f, 0);
            lineObjRect.GetComponent<Image>().color = lineColor;
        }


        protected override void GetStartDragObjPos()
        {
            startDragObjPosX = selfRect.anchoredPosition.x - selfRect.sizeDelta.x / 2f;

            startDragObjPosY = selfRect.anchoredPosition.y - selfRect.sizeDelta.y / 2f;
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