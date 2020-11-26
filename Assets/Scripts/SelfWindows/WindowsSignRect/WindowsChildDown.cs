using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tianbo.Wang
{
    public class WindowsChildDown : WindowsChildBase
    {
        public WindowsChildDown() : base()
        {

        }

        public WindowsChildDown(RectTransform parent) : base(parent)
        {
            selfChildType = DragToChildType.Down;

        }

        public override void SetPosAndSize()
        {
            base.SetPosAndSize();

            selfRect.sizeDelta = new Vector2(0, parentSize.y / times);
            selfRect.anchoredPosition = Vector2.zero;
            childCenter = selfRect.GetCenter();
            childSize = selfRect.GetLocalSize();
        }

        public override void SetAnchoredAndPivote()
        {
            base.SetAnchoredAndPivote();
            selfRect.anchorMin = new Vector2(0, 0);
            selfRect.anchorMax = new Vector2(1, 0);
            selfRect.pivot = new Vector2(0.5f, 0);
        }

    }
}