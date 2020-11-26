using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tianbo.Wang
{
    [Serializable]
    public class LayoutLine
    {
        public LineMoveAxis lineMoveAxis;
        public float value;
        public Image lineImage;
        /// <summary>
        /// 当前线段控制的窗口
        /// </summary>
        public Dictionary<WindowsBase, LineMoveDir> controlWindows = new Dictionary<WindowsBase, LineMoveDir>();
        public LayoutLine()
        {

        }
        public LayoutLine(LineMoveAxis _lineMoveAxis, string lineName, Transform parent)
        {
            lineMoveAxis = _lineMoveAxis;
            lineImage = new GameObject(lineName).AddComponent<Image>();
            lineImage.transform.SetParent(parent);
            lineImage.color = WindowsManager.Instance.layoutLinesColor;
            EventTriggerListener.Get(lineImage.gameObject).OnMouseBeginDrag = BeginDrag;
            EventTriggerListener.Get(lineImage.gameObject).OnMouseEndDrag = EndDrag;
            EventTriggerListener.Get(lineImage.gameObject).OnMouseDrag = OnDrag;
            EventTriggerListener.Get(lineImage.gameObject).OnMouseEnter = OnMouseEnter;
            EventTriggerListener.Get(lineImage.gameObject).OnMouseExit = OnMouseExit;
        }

        private void OnMouseEnter(GameObject go)
        {
            Cursor.SetCursor(lineMoveAxis == LineMoveAxis.Horizential ? WindowsManager.Instance.mouseIconX : WindowsManager.Instance.mouseIconY, new Vector2(10, 10), CursorMode.ForceSoftware);
        }

        private void OnMouseExit(GameObject go)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        }

        Vector2 startMousePos;
        private void BeginDrag(GameObject go)
        {
            startMousePos = Input.mousePosition;
        }
        float offsetX;
        float offsetY;
        RectTransform.Edge edge;
        float insertValue;
        private void OnDrag(GameObject go)
        {
            offsetX = startMousePos.x - Input.mousePosition.x;
            offsetY = startMousePos.y - Input.mousePosition.y;
            startMousePos = Input.mousePosition;

            if (lineMoveAxis == LineMoveAxis.Horizential)
            {
                lineImage.rectTransform.localPosition -= new Vector3(offsetX, 0, 0);
            }
            else
            {
                lineImage.rectTransform.localPosition -= new Vector3(0, offsetY, 0);
            }
            foreach (var itemKey in controlWindows.Keys)
            {
                Vector2 tempItemPos = itemKey.selfTrans.GetCenter();
                Vector2 tempItemSize = itemKey.selfTrans.GetSize();

                switch (controlWindows[itemKey])
                {
                    case LineMoveDir.Up:
                        edge = RectTransform.Edge.Bottom;
                        itemKey.selfTrans.localPosition -= new Vector3(0, offsetY / 2f, 0);
                        itemKey.selfTrans.sizeDelta -= new Vector2(0, offsetY);
                        break;
                    case LineMoveDir.Left:
                        edge = RectTransform.Edge.Right;
                        itemKey.selfTrans.localPosition -= new Vector3(offsetX / 2f, 0, 0);
                        itemKey.selfTrans.sizeDelta += new Vector2(offsetX, 0);
                        break;
                    case LineMoveDir.Down:
                        edge = RectTransform.Edge.Top;
                        itemKey.selfTrans.localPosition -= new Vector3(0, offsetY / 2f, 0);
                        itemKey.selfTrans.sizeDelta += new Vector2(0, offsetY);
                        break;
                    case LineMoveDir.Right:
                        edge = RectTransform.Edge.Left;
                        itemKey.selfTrans.localPosition -= new Vector3(offsetX / 2f, 0, 0);
                        itemKey.selfTrans.sizeDelta -= new Vector2(offsetX, 0);
                        break;
                }
            }

        }
        private void EndDrag(GameObject go)
        {
            foreach (var itemKey in controlWindows.Keys)
            {
                itemKey.ResetChildSigns();
                WindowsManager.Instance.RefreshWindowsLayoutLine();
            }
        }

        public void SetImageLinePosAndSize(Vector2 pos, Vector2 size)
        {
            lineImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            lineImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            lineImage.rectTransform.position = pos;
        }
        public void DestroyLine()
        {
            GameObject.DestroyImmediate(lineImage.gameObject);
        }
    }

}
