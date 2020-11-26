using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tianbo.Wang
{
    public class WindowsChildUp : WindowsChildBase
    {
        public WindowsChildUp() : base()
        {

        }

        public WindowsChildUp(RectTransform parent) : base(parent)
        {
            selfChildType = DragToChildType.Up;
        }

        public override void SetPosAndSize()
        {
            base.SetPosAndSize();


            parentCenterPoint = parentRect.GetCenter();
            parentSize = parentRect.GetSize();
            selfRect.sizeDelta = new Vector2(0, parentSize.y / times);
            selfRect.anchoredPosition = Vector2.zero;
            childCenter = selfRect.GetCenter();
            childSize = selfRect.GetLocalSize();
            if (titleDragObj != null)
            {
                selfRect.anchoredPosition = Vector2.zero - new Vector2(0, titleDragObj.transform.GetRectTransform().GetSize().y / 2f);
            }


        }

        public override void SetAnchoredAndPivote()
        {
            base.SetAnchoredAndPivote();
            selfRect.anchorMin = new Vector2(0, 1);
            selfRect.anchorMax = new Vector2(1, 1);
            selfRect.pivot = new Vector2(0.5f, 1);
        }

    }
}