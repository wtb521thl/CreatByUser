using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tianbo.Wang
{
    public class WindowsChildBase
    {
        public RectTransform selfRect;
        public RectTransform parentRect;

        protected Vector2 parentCenterPoint;
        protected Vector2 parentSize;
        protected GameObject titleDragObj;

        public float times = 3;

        public Vector2 childCenter;
        public Vector2 childSize;

        public DragToChildType selfChildType;

        public WindowsChildBase()
        {

        }
        public WindowsChildBase(RectTransform parent)
        {
            Init(parent);
        }

        public virtual void Init(RectTransform parent)
        {
            parentRect = parent;
            titleDragObj = parentRect.GetComponent<WindowsBase>().titleDragObj;
            selfRect = new GameObject(this.GetType().ToString()).AddComponent<RectTransform>();
            selfRect.SetParent(parentRect);
            WindowsManager.Instance.WaitoEnd(() =>
            {
                Refresh();
            });
        }

        public virtual void Refresh()
        {

            parentCenterPoint = parentRect.GetCenter();
            parentSize = parentRect.GetSize();
            SetAnchoredAndPivote();
            SetPosAndSize();

        }

        /// <summary>
        /// 设置标志物体的尺寸和位置
        /// </summary>
        public virtual void SetPosAndSize()
        {

        }
        /// <summary>
        /// 设置标志物体的锚点
        /// </summary>
        public virtual void SetAnchoredAndPivote()
        {

        }

        /// <summary>
        /// 鼠标是否在此物体上
        /// </summary>
        /// <returns></returns>
        public virtual bool IsMouseEnter()
        {
            return MouseEnterAndExit.IsMouseEnter(selfRect);
        }


    }
    public enum DragToChildType
    {
        None,
        Title,
        Left,
        Right,
        Up,
        Down
    }
}