using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Tianbo.Wang
{
    public partial class NodeItem : MonoBehaviour
    {
        GameObject selectImage;
        GameObject hoverImage;
        public Vector3 closeRotAngle = new Vector3(0, 0, 90);
        public Vector3 openRotAngle = Vector3.zero;

        Image iconImage;

        public List<NodeItem> childNodes = new List<NodeItem>();

        public NodeItem parentNode;

        public bool open = false;

        bool isSelected = false;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                ChangeSelectColor(isSelected);
            }
        }
        /// <summary>
        /// 是否是静态的，可以点击，但是没有打开关闭的动作和动画
        /// </summary>
        public bool isStatic = false;

        public Action<NodeItem> MouseEnterAction;

        public Action<NodeItem> MouseExitAction;

        private void Awake()
        {
            iconImage = transform.Find("Icon").GetComponent<Image>();
            selectImage = transform.Find("SelectImage").gameObject;
            hoverImage = transform.Find("HoverImage").gameObject;
            EventTriggerListener.Get(gameObject).OnMouseClick += ItemClick;
            EventTriggerListener.Get(gameObject).OnMouseEnter += ItemEnter;
            EventTriggerListener.Get(gameObject).OnMouseExit += ItemExit;
            ItemExit(gameObject);
        }

        private void ItemEnter(GameObject go)
        {
            hoverImage.SetActive(true);
            MouseEnterAction?.Invoke(this);
        }
        private void ItemExit(GameObject go)
        {
            hoverImage.SetActive(false);
            MouseExitAction?.Invoke(this);
        }
        /// <summary>
        /// 初始化时候必须调用
        /// </summary>
        /// <param name="isOpen">是否打开状态</param>
        /// <param name="isStatic">是否是静态的，不可以点击</param>
        public void Init(bool _isOpen = false, bool _isStatic = false)
        {
            isStatic = _isStatic;
            if (!isStatic)
            {
                iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, childNodes.Count == 0 ? 0 : 1);
                iconImage.rectTransform.localEulerAngles = closeRotAngle;
            }
            else
            {
                iconImage.gameObject.SetActive(false);
            }

            if (_isOpen)
            {
                OpenChilds();
            }
            else
            {
                CloseChilds();
            }

            IsSelected = false;
        }


        void ChangeSelectColor(bool _isSelecter)
        {
            selectImage.SetActive(_isSelecter);
        }


        private void ItemClick(GameObject go)
        {
            if (!open)
            {
                OpenChilds();
            }
            else
            {
                CloseChilds();
            }
        }

        public void OpenParent()
        {
            OpenParentFunc(parentNode);
        }
        void OpenParentFunc(NodeItem parentNode)
        {
            parentNode.OpenChilds();
            if (parentNode.parentNode != null)
            {
                OpenParentFunc(parentNode.parentNode);
            }
        }

        public void OpenChilds()
        {
            ChangeState(true);
            open = true;
        }

        public void CloseChilds()
        {
            ChangeState(false);
            open = false;
        }

        void ChangeState(bool open)
        {
            if (!isStatic)
            {
                for (int i = 0; i < childNodes.Count; i++)
                {
                    if (!open)
                    {
                        CloseAllNode(childNodes[i]);
                    }
                    else
                    {
                        childNodes[i].gameObject.SetActive(open);
                    }
                }
                if (open)
                {
                    iconImage.rectTransform.DOLocalRotate(openRotAngle, 0.3f);
                }
                else
                {
                    iconImage.rectTransform.DOLocalRotate(closeRotAngle, 0.3f);
                }
            }
        }

        void CloseAllNode(NodeItem nodeItem)
        {
            if (nodeItem.childNodes.Count == 0)
            {
                nodeItem.Init();
                nodeItem.gameObject.SetActive(false);
            }
            else
            {
                for (int i = 0; i < nodeItem.childNodes.Count; i++)
                {
                    CloseAllNode(nodeItem.childNodes[i]);
                }
                if (nodeItem.parentNode != null)
                {
                    nodeItem.Init();
                    nodeItem.gameObject.SetActive(false);
                }
            }
        }
    }

    public partial class NodeItem
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        public string nodeName;


        /// <summary>
        /// 节点级别
        /// </summary>
        public int nodeLevel;

        /// <summary>
        /// 父节点名字
        /// </summary>
        public string parentNodeName;

        /// <summary>
        /// 参数 (这里参数为了区别同名不同层级的item)
        /// </summary>
        public string param;

    }

    [Serializable]
    public class NodeItemSerializable
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        public string nodeName;

        /// <summary>
        /// 节点级别
        /// </summary>
        public int nodeLevel;

        /// <summary>
        /// 父节点名字
        /// </summary>
        public string parentNodeName;

        /// <summary>
        /// 节点地址名称
        /// </summary>
        public string nodeParam;


        public NodeItemSerializable(string _nodeName, string _itemParentName, int _nodeLevel, string _nodeParam = "")
        {
            nodeName = _nodeName;
            nodeParam = _nodeParam;
            parentNodeName = _itemParentName;
            nodeLevel = _nodeLevel;
        }
    }

    public class NodeItemSerializableInfo
    {
        public List<NodeItemSerializable> nodeItemSerializables = new List<NodeItemSerializable>();

        public void Add(string _nodeName, string _itemParentName, int _nodeLevel, string _nodeParam = "")
        {
            nodeItemSerializables.Add(new NodeItemSerializable(_nodeName, _itemParentName, _nodeLevel, _nodeParam));
        }
        public void Clear()
        {
            nodeItemSerializables.Clear();
        }
    }

}