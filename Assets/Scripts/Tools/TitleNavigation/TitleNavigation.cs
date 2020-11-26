using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Tianbo.Wang
{
    public class TitleNavigation : MonoBehaviour
    {

        /// <summary>
        /// 所有的节点 
        /// </summary>
        public List<NodeItem> allNodes = new List<NodeItem>();

        List<GameObject> allNodeObjs = new List<GameObject>();

        public Transform parent;
        /// <summary>
        /// 每个分段的间距
        /// </summary>
        public int offset = 50;

        /// <summary>
        /// 第一层的行间距
        /// </summary>
        public int firstLevelOffset = 0;

        public string prefabPath = "Prefabs/TitleNodeItem";

        public string nameStr = "NameStr";

        public List<NodeItem> selectedNodes = new List<NodeItem>();

        /// <summary>
        /// 当前点击的节点事件
        /// </summary>
        public Action<NodeItem[]> ClickNodeAction;

        /// <summary>
        /// 是否打开以及菜单
        /// </summary>
        public bool isOpenFirst = false;
        /// <summary>
        /// 是否是静态的，所有的都打开，不可关闭
        /// </summary>
        public bool isStaticAndOpenAlways = false;
        /// <summary>
        /// 是否可以点击两次以上
        /// </summary>
        public bool canClickTwice = true;

        public bool sliderAutoValue = false;

        ScrollRect selfScrollRect;

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        private void Update()
        {
            if (selfScrollRect == null)
            {
                selfScrollRect = GetComponentInChildren<ScrollRect>();
            }
            if (selfScrollRect == null)
            {
                return;
            }
            pointerEventData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            for (int i = 0; i < raycastResults.Count; i++)
            {
                if (raycastResults[i].gameObject == gameObject)
                {
                    if (Input.GetAxis("Mouse ScrollWheel") != 0 && selfScrollRect.verticalScrollbar != null)
                    {
                        selfScrollRect.verticalScrollbar.value += Input.GetAxis("Mouse ScrollWheel") * selfScrollRect.verticalScrollbar.size;
                    }
                }
            }
        }

        /// <summary>
        /// 新增节点
        /// </summary>
        /// <param name="addItem"></param>
        public void AddOne(NodeItemSerializable addItem)
        {
            if (parent == null)
            {
                parent = transform.Find("Content");
            }

            GameObject tempItem = Instantiate(ResourceManager.GetGameObject(prefabPath), parent);
            tempItem.name = addItem.nodeName;
            if (!isStaticAndOpenAlways)
            {
                tempItem.GetComponent<HorizontalLayoutGroup>().padding.left = firstLevelOffset + addItem.nodeLevel * offset;
            }
            tempItem.transform.Find(nameStr).GetComponent<Text>().text = addItem.nodeName;
            allNodeObjs.Add(tempItem);
            EventTriggerListener.Get(tempItem).OnMouseClick += ClickNode;
            NodeItem tempNodeItem = tempItem.GetComponent<NodeItem>();
            tempNodeItem.nodeName = addItem.nodeName;
            tempNodeItem.parentNodeName = addItem.parentNodeName;
            tempNodeItem.nodeLevel = addItem.nodeLevel;
            tempNodeItem.param = addItem.nodeParam;

            SetChildAndParent(tempNodeItem);

            SetParentIndex(tempNodeItem);

            allNodes.Add(tempNodeItem);

        }

        public void ChangeOne(string oldParamName, string changeItemParam)
        {
            GameObject tempItem = allNodeObjs.Find((p) => { return p.GetComponent<NodeItem>().param == oldParamName; });
            tempItem.transform.Find(nameStr).GetComponent<Text>().text = changeItemParam;
            NodeItem tempNodeItem = tempItem.GetComponent<NodeItem>();
            tempNodeItem.nodeName = changeItemParam;

            GameObject tempParentItem = allNodeObjs.Find((p) => { return p.GetComponent<NodeItem>().parentNodeName == oldParamName.Split(new string[] { "||" }, StringSplitOptions.None)[0]; });
            if (tempParentItem != null)
            {
                tempParentItem.GetComponent<NodeItem>().parentNodeName = changeItemParam;
            }
        }

        public void RemoveAllNode()
        {
            for (int i = 0; i < allNodeObjs.Count; i++)
            {
                DestroyImmediate(allNodeObjs[i]);
            }
            allNodeObjs.Clear();
            allNodes.Clear();
        }

        /// <summary>
        /// 每次增加或者减少节点后调用此方法，刷新节点信息
        /// </summary>
        public void RefreshNodeItemChildInfo()
        {
            RefreshParentLayout();
            for (int i = 0; i < allNodes.Count; i++)
            {
                if (!isStaticAndOpenAlways)
                {
                    if (i == 0 && isOpenFirst)
                    {
                        allNodes[i].Init(true);
                    }
                    else
                    {
                        allNodes[i].Init();
                    }
                }
                else
                {
                    if (i == 0 && isOpenFirst)
                    {
                        allNodes[i].Init(true, true);
                    }
                    else
                    {
                        allNodes[i].Init(false, true);
                    }
                }
            }
        }
        /// <summary>
        /// 强制刷新canvas
        /// </summary>
        public void RefreshParentLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent as RectTransform);
        }
        public void RefreshScrollViewPosition()
        {
            if (selectedNodes != null)
            {
                SliderAutoChangeValue(selectedNodes[0].gameObject);
            }
        }

        /// <summary>
        /// 外部调用
        /// </summary>
        /// <param name="allSelectObjs"></param>
        public void ClickNodeByObjects(GameObject[] allSelectObjs)
        {
            selectedNodes.Clear();
            if (allSelectObjs != null)
            {
                for (int i = 0; i < allSelectObjs.Length; i++)
                {
                    selectedNodes.Add(allNodes.Find((p) => { return p.param == allSelectObjs[i].name; }));
                }
            }
            RefreshSelectNodeState();
        }

        /// <summary>
        /// 内部调用
        /// </summary>
        /// <param name="go"></param>
        private void ClickNode(GameObject go)
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                selectedNodes.Clear();
            }
            NodeItem tempAddItem = allNodes.Find((p) => { return p == go.GetComponent<NodeItem>(); });
            if (tempAddItem != null)
            {
                selectedNodes.Add(tempAddItem);
            }
            ClickNodeAction?.Invoke(selectedNodes.ToArray());
            RefreshSelectNodeState();
        }


        void RefreshSelectNodeState()
        {
            for (int i = 0; i < allNodes.Count; i++)
            {
                allNodes[i].IsSelected = false;
            }
            if (selectedNodes.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < selectedNodes.Count; i++)
            {
                selectedNodes[i].IsSelected = true;
                if (selectedNodes[i].parentNode != null)
                {
                    selectedNodes[i].parentNode.OpenChilds();
                }
            }

            SliderAutoChangeValue(selectedNodes[0]);
        }


        private void SliderAutoChangeValue(GameObject go)
        {
            int allActiveNodeCount = 0;
            int selectIndex = 0;
            for (int i = 0; i < allNodes.Count; i++)
            {
                if (allNodes[i].gameObject.activeSelf)
                {
                    allActiveNodeCount += 1;
                }
                if (allNodes[i].gameObject == go)
                {
                    selectIndex = allActiveNodeCount;
                }
            }
            StartCoroutine("WaitOne", 1f - (float)selectIndex / (float)allActiveNodeCount);
        }
        private void SliderAutoChangeValue(NodeItem item)
        {
            int allActiveNodeCount = 0;
            int selectIndex = 0;
            for (int i = 0; i < allNodes.Count; i++)
            {
                if (allNodes[i].gameObject.activeSelf)
                {
                    allActiveNodeCount += 1;
                }
                if (allNodes[i] == item)
                {
                    selectIndex = allActiveNodeCount;
                }
            }
            StartCoroutine("WaitOne", 1f - (float)selectIndex / (float)allActiveNodeCount);
        }
        IEnumerator WaitOne(float value)
        {
            yield return null;
            selfScrollRect.verticalScrollbar.value = value;
        }

        //NodeItem[] GetNodeItems(NodeItem curNode)
        //{
        //    List<NodeItem> nodeItems = new List<NodeItem>();
        //    nodeItems.Add(curNode);
        //    GetParentNode(nodeItems, curNode);
        //    nodeItems.Reverse();
        //    return nodeItems.ToArray();
        //}

        //void GetParentNode(List<NodeItem> nodeItems,NodeItem curNode)
        //{
        //    if(curNode.parentNode != null)
        //    {
        //        nodeItems.Add(curNode.parentNode);
        //        GetParentNode(nodeItems, curNode.parentNode);
        //    }
        //}


        /// <summary>
        /// 设置trans索引
        /// </summary>
        /// <param name="tempNodeItem"></param>
        private void SetParentIndex(NodeItem tempNodeItem)
        {
            if (tempNodeItem.parentNode != null)
            {
                tempNodeItem.transform.SetSiblingIndex(tempNodeItem.parentNode.transform.GetSiblingIndex() + GetIndex(tempNodeItem.parentNode) - 1);
            }
        }

        /// <summary>
        /// 设置父子关系
        /// </summary>
        /// <param name="tempNodeItem"></param>
        private void SetChildAndParent(NodeItem tempNodeItem)
        {
            for (int i = 0; i < allNodes.Count; i++)
            {
                if (allNodes[i].parentNodeName == tempNodeItem.nodeName && allNodes[i].nodeLevel != tempNodeItem.nodeLevel)
                {
                    allNodes[i].parentNode = tempNodeItem;
                    tempNodeItem.childNodes.Add(allNodes[i]);
                }
                if (allNodes[i].nodeName == tempNodeItem.parentNodeName && allNodes[i].nodeLevel != tempNodeItem.nodeLevel)
                {
                    if (!allNodes[i].childNodes.Contains(tempNodeItem))
                    {
                        allNodes[i].childNodes.Add(tempNodeItem);
                        tempNodeItem.parentNode = allNodes[i];
                    }
                }
            }
        }
        /// <summary>
        /// 获得父子物体相加后索引
        /// </summary>
        /// <param name="nodeItem"></param>
        /// <returns></returns>
        private int GetIndex(NodeItem nodeItem)
        {
            int index = 0;
            if (nodeItem.childNodes.Count != 0)
            {
                for (int i = 0; i < nodeItem.childNodes.Count; i++)
                {
                    index += GetIndex(nodeItem.childNodes[i]);
                }
            }
            index += 1;

            return index;
        }

    }
    [Serializable]
    public class SmallItem
    {
        public List<string> itemStrs = new List<string>();
        public SmallItem(List<string> _itemStrs)
        {
            itemStrs = _itemStrs;
        }
    }
}