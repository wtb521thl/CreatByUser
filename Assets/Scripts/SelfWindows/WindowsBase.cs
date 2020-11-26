using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tianbo.Wang
{
    public class WindowsBase : MonoBehaviour
    {
        /// <summary>
        /// 拖拽的物体
        /// </summary>
        public GameObject titleDragObj;
        /// <summary>
        /// 展示给用户的名字
        /// </summary>
        public string windowsName;
        [HideInInspector]
        public RectTransform selfTrans;

        Vector3 dragTitleStartMousePos;
        Vector3 dragTitleStartPos;

        /// <summary>
        /// 拖拽的临时窗口
        /// </summary>
        RectTransform dragObjRect;

        /// <summary>
        /// 拖拽的物体预设
        /// </summary>
        GameObject dragObjPrefab;

        WindowsBase dragEndWindow;

        DragToChildType dragToChildType;

        string path = "Prefabs/DragWindowsPrefab";

        public Vector2 minSize = new Vector2(400, 600);

        public List<GameObject> allTitleObjects = new List<GameObject>();

        /// <summary>
        /// 当前窗口是否与其他窗口合并中（拉到标题栏）
        /// </summary>
        public bool curWinInOtherWin = false;

        protected virtual void Awake()
        {
            selfTrans = GetComponent<RectTransform>();
            WindowsManager.Instance.RegisterWindows(this);
            InitChildRect();
            if (titleDragObj != null)
            {
                titleDragObj.name = transform.name + "_Title";
                EventTriggerListener.Get(titleDragObj).OnMouseBeginDrag += BeginDragTitle;
                EventTriggerListener.Get(titleDragObj).OnMouseDrag += DragTitle;
                EventTriggerListener.Get(titleDragObj).OnMouseEndDrag += EndDragTitle;
                EventTriggerListener.Get(titleDragObj).OnMouseClick += ClickTitle;
                titleDragObj.GetComponentInChildren<Text>().text = windowsName;
                allTitleObjects.Clear();
                allTitleObjects.Add(titleDragObj);
            }

            dragObjPrefab = Resources.Load<GameObject>(path);

        }


        private void ClickTitle(GameObject go)
        {
            for (int i = 0; i < WindowsManager.Instance.allWindowsBase.Count; i++)
            {
                if (WindowsManager.Instance.allWindowsBase[i].titleDragObj != null && WindowsManager.Instance.allWindowsBase[i].titleDragObj.name == go.name)
                {
                    WindowsManager.Instance.allWindowsBase[i].transform.SetAsLastSibling();
                }
            }
        }

        /// <summary>
        /// 开始拖拽
        /// </summary>
        /// <param name="go"></param>
        private void BeginDragTitle(GameObject go)
        {
            curWinInOtherWin = false;
            for (int i = 0; i < allTitleObjects.Count; i++)
            {
                WindowsBase tempWindows = WindowsManager.Instance.allWindowsBase.Find((p) =>
                {
                    if (p.titleDragObj == null)
                    {
                        return false;
                    }
                    return p.titleDragObj.name == allTitleObjects[i].name;
                });
                if (tempWindows != null && tempWindows != this)
                {
                    curWinInOtherWin = true;
                    break;
                }
            }

            dragTitleStartMousePos = Input.mousePosition;

            dragObjRect = Instantiate(dragObjPrefab, transform.parent).GetComponent<RectTransform>();
            dragObjRect.anchorMin = Vector2.zero;
            dragObjRect.anchorMax = Vector2.one;
            dragObjRect.pivot = Vector2.one / 2f;
            dragObjRect.transform.GetComponentInChildren<Text>(true).text = windowsName;
            dragObjRect.sizeDelta = selfTrans.sizeDelta;
            dragObjRect.position = dragTitleStartMousePos;
            dragTitleStartPos = dragObjRect.position;
        }
        /// <summary>
        /// 正在拖拽标题部分执行的事件
        /// </summary>
        /// <param name="go"></param>
        private void DragTitle(GameObject go)
        {

            for (int i = 0; i < WindowsManager.Instance.allWindowsBase.Count; i++)
            {
                if (WindowsManager.Instance.allWindowsBase[i] == this)
                {
                    ChangeDragObjSizeAndPos(i);
                    dragEndWindow = WindowsManager.Instance.allWindowsBase[i];
                    continue;
                }
                if (MouseEnterAndExit.IsMouseEnter(WindowsManager.Instance.allWindowsBase[i].selfTrans))
                {
                    bool isSet = false;
                    for (int j = 0; j < WindowsManager.Instance.allWindowsBase[i].windowsChildBases.Count; j++)
                    {
                        if (WindowsManager.Instance.allWindowsBase[i].windowsChildBases[j].IsMouseEnter())
                        {
                            isSet = true;
                            SetRectPosAndSizeEqual(dragObjRect, WindowsManager.Instance.allWindowsBase[i].windowsChildBases[j]);
                            dragToChildType = WindowsManager.Instance.allWindowsBase[i].windowsChildBases[j].selfChildType;
                            dragEndWindow = WindowsManager.Instance.allWindowsBase[i];
                            break;
                        }
                    }
                    if (!isSet)
                    {
                        ChangeDragObjSizeAndPos(i);
                        dragEndWindow = WindowsManager.Instance.allWindowsBase[i];
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// 最终自身要变成的形状  localSize
        /// </summary>
        Vector2 endDragSizeDelta;
        /// <summary>
        /// 最终自身要去到的位置
        /// </summary>
        Vector2 endDragPos;
        /// <summary>
        /// 当鼠标放到标题上，或者鼠标在空白部分的时候
        /// </summary>
        /// <param name="i"></param>
        private void ChangeDragObjSizeAndPos(int i)
        {
            if (WindowsManager.Instance.allWindowsBase[i].titleDragObj != null && MouseEnterAndExit.IsMouseEnter(WindowsManager.Instance.allWindowsBase[i].titleDragObj.transform as RectTransform))
            {
                RectTransform titleRect = WindowsManager.Instance.allWindowsBase[i].titleDragObj.GetComponent<RectTransform>();
                Vector2 titleSize = titleRect.GetSize();
                dragObjRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, titleSize.x);
                dragObjRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, titleSize.y);
                dragObjRect.position = new Vector3((dragTitleStartPos + Input.mousePosition - dragTitleStartMousePos).x, titleRect.GetCenter().y, 0);
                endDragSizeDelta = WindowsManager.Instance.allWindowsBase[i].selfTrans.GetLocalSize();
                endDragPos = WindowsManager.Instance.allWindowsBase[i].selfTrans.position;
                dragToChildType = DragToChildType.Title;
            }
            else
            {
                dragObjRect.position = dragTitleStartPos + Input.mousePosition - dragTitleStartMousePos;
                dragObjRect.sizeDelta = selfTrans.sizeDelta;
                endDragSizeDelta = selfTrans.GetLocalSize();
                endDragPos = dragObjRect.position;
                dragToChildType = DragToChildType.None;

            }
        }
        /// <summary>
        /// 鼠标挪到标志位上的时候，将自身的形状与标志位重叠
        /// </summary>
        /// <param name="changeRect"></param>
        /// <param name="childBase"></param>
        void SetRectPosAndSizeEqual(RectTransform changeRect, WindowsChildBase childBase)
        {
            changeRect.position = childBase.childCenter;
            changeRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, childBase.childSize.x);
            changeRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, childBase.childSize.y);

            endDragSizeDelta = changeRect.GetLocalSize();
            endDragPos = changeRect.position;
        }
        /// <summary>
        /// 拖拽松手后
        /// </summary>
        /// <param name="go"></param>
        private void EndDragTitle(GameObject go)
        {

            if (!curWinInOtherWin)
            {
                WindowsManager.Instance.FillGap(this);
            }


            if (dragToChildType != DragToChildType.None && dragToChildType != DragToChildType.Title)
            {
                for (int j = 0; j < dragEndWindow.windowsChildBases.Count; j++)
                {
                    if (dragEndWindow.windowsChildBases[j].selfChildType == dragToChildType)
                    {
                        SetRectPosAndSizeEqual(dragObjRect, dragEndWindow.windowsChildBases[j]);  //填补漏洞后刷新子物体标志位置
                        break;
                    }
                }
            }
            else if (dragToChildType == DragToChildType.Title)
            {
                if (dragEndWindow != this)
                {
                    endDragSizeDelta = dragEndWindow.selfTrans.GetLocalSize();
                    endDragPos = dragEndWindow.selfTrans.position;
                }
            }
            //为当前物体赋值
            selfTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, endDragSizeDelta.x);
            selfTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, endDragSizeDelta.y);

            selfTrans.position = endDragPos;

            ResetChildSigns();

            WindowsManager.Instance.ReduceRoomByInsertWindow(this, dragEndWindow, dragToChildType, dragObjRect);
            WindowsManager.Instance.RefreshWindowsLayoutLine();

            DestroyImmediate(dragObjRect.gameObject);

            if (dragToChildType == DragToChildType.None)
            {
                InstantiateOutlines();
            }
            else
            {
                DestroyOutLines();
            }

            if (curWinInOtherWin)
            {
                CurWinDestroyOtherWinTitle();
            }

            if (dragToChildType == DragToChildType.Title)
            {
                AddTitle();
            }

        }


        /// <summary>
        /// 解除合并标题
        /// </summary>
        private void CurWinDestroyOtherWinTitle()
        {
            for (int i = 0; i < allTitleObjects.Count; i++)
            {
                WindowsBase tempWindows = WindowsManager.Instance.allWindowsBase.Find((p) =>
                {
                    if (p.titleDragObj == null)
                    {
                        return false;
                    }
                    return p.titleDragObj.name == allTitleObjects[i].name;
                });
                if (tempWindows != null && tempWindows != this)
                {
                    for (int j = tempWindows.allTitleObjects.Count - 1; j >= 0; j--)
                    {
                        if (tempWindows.allTitleObjects[j].name == titleDragObj.name && tempWindows.allTitleObjects[j].name != tempWindows.titleDragObj.name)
                        {
                            DestroyImmediate(tempWindows.allTitleObjects[j]);
                            tempWindows.allTitleObjects.RemoveAt(j);
                        }
                    }
                }
                if (allTitleObjects[i].name != titleDragObj.name)
                {
                    DestroyImmediate(allTitleObjects[i]);
                }
            }
            allTitleObjects.Clear();
            allTitleObjects.Add(titleDragObj);
        }
        /// <summary>
        /// 拖入标题栏，双方都增加新标题（可拖拽）
        /// </summary>
        private void AddTitle()
        {
            if (dragEndWindow != this)
            {
                for (int i = 0; i < dragEndWindow.allTitleObjects.Count; i++)
                {
                    WindowsBase tempOtherWin = WindowsManager.Instance.allWindowsBase.Find((p) => { return p.name == dragEndWindow.allTitleObjects[i].name.Split('_')[0]; });
                    if (tempOtherWin != null)
                    {
                        if (allTitleObjects.Find((p) => { return p.name == tempOtherWin.titleDragObj.name; }) == null)
                        {
                            GameObject beginWinAddEndTitle = Instantiate(tempOtherWin.titleDragObj, titleDragObj.transform.parent);
                            beginWinAddEndTitle.name = tempOtherWin.transform.name + "_Title";
                            beginWinAddEndTitle.transform.SetAsFirstSibling();
                            EventTriggerListener.Get(beginWinAddEndTitle).OnMouseBeginDrag += tempOtherWin.BeginDragTitle;
                            EventTriggerListener.Get(beginWinAddEndTitle).OnMouseDrag += tempOtherWin.DragTitle;
                            EventTriggerListener.Get(beginWinAddEndTitle).OnMouseEndDrag += tempOtherWin.EndDragTitle;
                            EventTriggerListener.Get(beginWinAddEndTitle).OnMouseClick += tempOtherWin.ClickTitle;
                            allTitleObjects.Add(beginWinAddEndTitle);
                        }

                        if (tempOtherWin.allTitleObjects.Find((p) => { return p.name == titleDragObj.name; }) == null)
                        {
                            GameObject endWinAddSelfTitle = Instantiate(titleDragObj, tempOtherWin.titleDragObj.transform.parent);
                            endWinAddSelfTitle.name = transform.name + "_Title";
                            EventTriggerListener.Get(endWinAddSelfTitle).OnMouseBeginDrag += BeginDragTitle;
                            EventTriggerListener.Get(endWinAddSelfTitle).OnMouseDrag += DragTitle;
                            EventTriggerListener.Get(endWinAddSelfTitle).OnMouseEndDrag += EndDragTitle;
                            EventTriggerListener.Get(endWinAddSelfTitle).OnMouseClick += ClickTitle;
                            tempOtherWin.allTitleObjects.Add(endWinAddSelfTitle);
                        }

                    }
                }
            }
        }

       

        /// <summary>
        /// 刷新子物体 自身的标志物体
        /// </summary>
        public void ResetChildSigns()
        {
            for (int i = 0; i < windowsChildBases.Count; i++)
            {
                windowsChildBases[i].Refresh();
            }
        }

        protected virtual void OnDestroy()
        {
            if (titleDragObj != null)
            {
                EventTriggerListener.Get(titleDragObj).OnMouseBeginDrag -= BeginDragTitle;
                EventTriggerListener.Get(titleDragObj).OnMouseDrag -= DragTitle;
                EventTriggerListener.Get(titleDragObj).OnMouseEndDrag -= EndDragTitle;
                EventTriggerListener.Get(titleDragObj).OnMouseClick -= ClickTitle;
            }
            WindowsManager.Instance.UnRegisterWindows(this);
        }

        #region 标志物体
        public List<WindowsChildBase> windowsChildBases = new List<WindowsChildBase>();
        WindowsChildBase windowsChildUp;
        WindowsChildBase windowsChildDown;
        WindowsChildBase windowsChildLeft;
        WindowsChildBase windowsChildRight;
        /// <summary>
        /// 生成子物体（用于标志其他窗口停靠的位置）
        /// </summary>
        void InitChildRect()
        {
            windowsChildBases.Clear();
            windowsChildUp = new WindowsChildUp(selfTrans);
            windowsChildBases.Add(windowsChildUp);
            windowsChildDown = new WindowsChildDown(selfTrans);
            windowsChildBases.Add(windowsChildDown);
            windowsChildLeft = new WindowsChildLeft(selfTrans);
            windowsChildBases.Add(windowsChildLeft);
            windowsChildRight = new WindowsChildRight(selfTrans);
            windowsChildBases.Add(windowsChildRight);
        }
        #endregion

        #region outline

        List<OutLine> outLines = new List<OutLine>();
        public void InstantiateOutlines()
        {
            DestroyOutLines();
            outLines.Clear();
            outLines.Add(OutLineManager.Instance.GetOutLine(this, "OutLineLeft"));
            outLines.Add(OutLineManager.Instance.GetOutLine(this, "OutLineRight"));
            outLines.Add(OutLineManager.Instance.GetOutLine(this, "OutLineUp"));
            outLines.Add(OutLineManager.Instance.GetOutLine(this, "OutLineDown"));

            for (int i = 0; i < outLines.Count; i++)
            {
                outLines[i].Init(gameObject);
                outLines[i].RefreshRect(5, Color.green);
                outLines[i].DragLineAction = RefreshLinePosition;
            }
        }

        private void DestroyOutLines()
        {
            for (int i = 0; i < outLines.Count; i++)
            {
                outLines[i].DestroySelf();
            }
        }

        void RefreshLinePosition()
        {
            for (int i = 0; i < outLines.Count; i++)
            {
                outLines[i].RefreshRect(5, Color.green);
            }
            ResetChildSigns();
        }
        #endregion

    }

}