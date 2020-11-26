using System;
using UnityEngine;

namespace Tianbo.Wang
{
    public class OutLine : IOutLine
    {
        /// <summary>
        /// 自身的框transform
        /// </summary>
        public RectTransform selfRect;

        /// <summary>
        /// 生成的物体名字
        /// </summary>
        public string insLineName;
        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool isInit = false;
        /// <summary>
        /// 是否正在拖拽
        /// </summary>
        public bool isDrag = false;

        /// <summary>
        /// 外框预设
        /// </summary>
        GameObject outLinePrefab;
        /// <summary>
        /// 鼠标图片icon
        /// </summary>
        Texture2D enterIcon;
        public GameObject lineObj;
        protected RectTransform lineObjRect;

        protected Vector2 startDragMousePos;
        /// <summary>
        /// 开始拖拽的时候物体sizeDelta的X值
        /// </summary>
        protected float startDragObjSizeDeltaX;
        protected float startDragObjSizeDeltaY;
        /// <summary>
        /// 开始拖拽时候物体距离父物体边界距离
        /// </summary>
        protected float startDragObjPosX;
        protected float startDragObjPosY;

        /// <summary>
        /// 鼠标移动后计算出来的物体size
        /// </summary>
        protected float newObjDisX;
        protected float newObjDisY;

        /// <summary>
        /// 记录物体世界坐标临时值
        /// </summary>
        Vector2 worldPos;

        protected Vector2 tempSelfPos;
        protected Vector2 tempSelfSize;

        public Action DragLineAction;

        public virtual void Init(GameObject go)
        {
            selfRect = go.GetComponent<RectTransform>();
            outLinePrefab = Resources.Load<GameObject>("Prefabs/OutLine");
            enterIcon = Resources.Load<Texture2D>("Texture/MouseEnterIcon");
            lineObj = GameObject.Instantiate(outLinePrefab, selfRect);
            lineObj.name = insLineName;
            lineObjRect = lineObj.GetComponent<RectTransform>();
            EventTriggerListener.Get(lineObj).OnMouseDrag = DragingLine;
            EventTriggerListener.Get(lineObj).OnMouseBeginDrag = BeginDragLine;
            EventTriggerListener.Get(lineObj).OnMouseEndDrag = EndDragLine;
            EventTriggerListener.Get(lineObj).OnMouseEnter = EnterLine;
            EventTriggerListener.Get(lineObj).OnMouseExit = ExitLine;
            isInit = true;

        }


        /// <summary>
        /// updata中刷新调用（后续可添加颜色、材质球等属性）
        /// </summary>
        /// <param name="lineColor">线条颜色</param>
        /// <param name="lineWidth">线条的宽度</param>
        public virtual void RefreshRect(float lineWidth, Color lineColor)
        {

        }
        /// <summary>
        /// 鼠标进入事件 更改鼠标icon
        /// </summary>
        void EnterLine(GameObject go)
        {
            if (!isDrag)
            {
                Cursor.SetCursor(enterIcon, Vector2.zero, CursorMode.Auto);
            }
        }
        /// <summary>
        /// 鼠标退出事件，恢复鼠标icon
        /// </summary>
        void ExitLine(GameObject go)
        {
            if (!isDrag)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }

        Vector2 startPos;
        Vector2 startSize;
        /// <summary>
        /// 开始拖拽事件
        /// </summary>
        void BeginDragLine(GameObject go)
        {

            isDrag = true;
            startDragMousePos = Input.mousePosition;

            worldPos = selfRect.position;//先记录先物体的世界坐标，防止在更改锚点的时候无法恢复原位

            startDragObjSizeDeltaX = selfRect.GetSize().x;
            startDragObjSizeDeltaY = selfRect.GetSize().y;

            SetAnchoredPos(); //更改锚点设置
            selfRect.position = worldPos;
            startPos = selfRect.position;
            startSize = selfRect.sizeDelta;
            GetStartDragObjPos();
        }
        /// <summary>
        /// 更改锚点设置
        /// </summary>
        protected virtual void SetAnchoredPos()
        {
            selfRect.anchorMax = Vector2.zero;
            selfRect.anchorMin = Vector2.one;
        }
        /// <summary>
        /// 获取距离父物体边界值
        /// </summary>
        protected virtual void GetStartDragObjPos()
        {

        }


        /// <summary>
        /// 拖拽事件
        /// </summary>
        protected void DragingLine(GameObject go)
        {
            DragLineAction?.Invoke();
            DragLine();
        }

        protected virtual void DragLine()
        {

        }

        /// <summary>
        /// 拖拽结束
        /// </summary>
        void EndDragLine(GameObject go)
        {
            tempSelfPos = selfRect.GetCenter();
            tempSelfSize = selfRect.GetSize();

            selfRect.anchorMax = Vector2.one;
            selfRect.anchorMin = Vector2.zero;

            selfRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tempSelfSize.x);
            selfRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tempSelfSize.y);
            selfRect.position = tempSelfPos;

            isDrag = false;

        }

        public void DestroySelf()
        {
            GameObject.DestroyImmediate(lineObj);
        }
    }
}