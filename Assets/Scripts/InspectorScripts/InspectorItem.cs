using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inspector
{
    public abstract class InspectorItem
    {
        protected ComponentItem componentItem;
        protected InspectorPanel inspectorPanel;
        protected GameObject selectObj;
        protected RectTransform selectObjRectTransform;

        /// <summary>
        /// 物体名字
        /// </summary>
        protected InputField nameInputField;
        /// <summary>
        /// 物体的X位置
        /// </summary>
        protected InputField vectorXInputField;
        /// <summary>
        /// 物体的Y位置
        /// </summary>
        protected InputField vectorYInputField;
        /// <summary>
        /// 物体的宽度
        /// </summary>
        protected InputField sizeXInputField;
        /// <summary>
        /// 物体的高度
        /// </summary>
        protected InputField sizeYInputField;

        /// <summary>
        /// 初始化，只执行一次
        /// </summary>
        /// <param name="contentArea"></param>
        /// <param name="_selectObj"></param>
        public virtual void Init(Transform contentArea, GameObject _selectObj)
        {
            selectObj = _selectObj;
            selectObjRectTransform = selectObj.GetComponent<RectTransform>();
            componentItem = _selectObj.GetComponent<ComponentItem>();
            inspectorPanel = contentArea.GetComponentInParent<InspectorPanel>();

            InstantInspectorItem(contentArea);

            RefreshValue();

            InitCommand();

            InitEvent();

        }

        /// <summary>
        /// 初始化属性面板的物体
        /// </summary>
        /// <param name="contentArea"></param>
        protected virtual void InstantInspectorItem(Transform contentArea)
        {
            GameObject tempBtnName = GameObject.Instantiate(inspectorPanel.oneValue, contentArea);
            tempBtnName.transform.Find("Title").GetComponent<Text>().text = "Name";
            nameInputField = tempBtnName.GetComponentInChildren<InputField>();

            GameObject tempBtnPos = GameObject.Instantiate(inspectorPanel.twoValue, contentArea);
            tempBtnPos.transform.Find("Title").GetComponent<Text>().text = "Position";
            vectorXInputField = tempBtnPos.transform.Find("InputVectorX").GetComponent<InputField>();
            vectorYInputField = tempBtnPos.transform.Find("InputVectorY").GetComponent<InputField>();

            GameObject tempBtnSize = GameObject.Instantiate(inspectorPanel.twoValue, contentArea);
            tempBtnSize.transform.Find("Title").GetComponent<Text>().text = "Size";
            sizeXInputField = tempBtnSize.transform.Find("InputVectorX").GetComponent<InputField>();
            sizeYInputField = tempBtnSize.transform.Find("InputVectorY").GetComponent<InputField>();
        }


        /// <summary>
        /// 刷新赋值，每次刷新的时候调用
        /// </summary>
        public virtual void RefreshValue()
        {
            nameInputField.SetTextWithoutNotify( selectObj.name);
            vectorXInputField.SetTextWithoutNotify(selectObjRectTransform.position.x.ToString());
            vectorYInputField.SetTextWithoutNotify(selectObjRectTransform.position.y.ToString());

            sizeXInputField.SetTextWithoutNotify( selectObjRectTransform.sizeDelta.x.ToString());
            sizeYInputField.SetTextWithoutNotify(selectObjRectTransform.sizeDelta.y.ToString());

        }

        /// <summary>
        /// 初始化命令事件
        /// </summary>
        protected virtual void InitCommand()
        {
            InitInputFieldCommand(nameInputField, NameInputFieldChangeValue);
            InitInputFieldCommand(vectorXInputField, VectorXInputFieldChangeValue);
            InitInputFieldCommand(vectorYInputField, VectorYInputFieldChangeValue);
            InitInputFieldCommand(sizeXInputField, SizeXInputFieldChangeValue);
            InitInputFieldCommand(sizeYInputField, SizeYInputFieldChangeValue);
        }


        /// <summary>
        /// 赋值后初始化事件，为物体赋值，初始的时候为物体赋值，不然物体的值与UI不符
        /// </summary>
        protected virtual void InitEvent()
        {
            NameInputFieldChangeValue(nameInputField.text);
            VectorXInputFieldChangeValue(vectorXInputField.text);
            VectorYInputFieldChangeValue(vectorYInputField.text);
            SizeXInputFieldChangeValue(sizeXInputField.text);
            SizeYInputFieldChangeValue(sizeYInputField.text);
        }

        /// <summary>
        /// 为物体赋值事件
        /// </summary>
        /// <param name="arg0"></param>
        private void NameInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "Name", arg0);
            EventCenter.BroadcastEvent(EventSendType.RefreshInspector);
        }

        private void VectorXInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "PosVectorX", arg0);
            EventCenter.BroadcastEvent(EventSendType.RefreshInspector);
        }

        private void VectorYInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "PosVectorY", arg0);
            EventCenter.BroadcastEvent(EventSendType.RefreshInspector);
        }

        private void SizeXInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "SizeVectorX", arg0);
            EventCenter.BroadcastEvent(EventSendType.RefreshInspector);
        }

        private void SizeYInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "SizeVectorY", arg0);
            EventCenter.BroadcastEvent(EventSendType.RefreshInspector);
        }



        #region 工具

        /// <summary>
        /// 设置drop down的OptionData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="drop"></param>
        /// <param name="ls"></param>
        protected void SetOptionData<T>(Dropdown drop, List<T> ls)
        {
            List<Dropdown.OptionData> ods = new List<Dropdown.OptionData>();
            for (int i = 0; i < ls.Count; i++)
            {
                ods.Add(new Dropdown.OptionData(ls[i].ToString()));
            }
            drop.options = ods;
        }
        protected void SetOptionData(Dropdown drop, Transform parent)
        {
            List<Dropdown.OptionData> ods = new List<Dropdown.OptionData>();
            for (int i = 0; i < parent.childCount; i++)
            {
                if(parent.GetChild(i).gameObject.activeSelf)
                    ods.Add(new Dropdown.OptionData(parent.GetChild(i).name.ToString()));
            }
            drop.options = ods;
        }

        protected char OnValidateInput(string text, int charIndex, char addedChar)
        {
            int result;
            if (int.TryParse(text + addedChar, out result))
            {
                return addedChar;
            }
            else
            {
                return default(char);
            }

        }

        protected void InitInputFieldCommand(InputField inputField, Action<string> tempAction)
        {
            inputField.onValueChanged.AddListener((text) =>
            {
                SendCommand(inputField, text, tempAction);
            });
            inputField.onEndEdit.AddListener((text) =>
            {
                ExcuteCommand();
            });
        }
        protected void InitDropDownCommand(Dropdown dropdown, Action<int> tempAction)
        {
            int lastDropDownValue = 0;
            lastDropDownValue = dropdown.value;
            dropdown.onValueChanged.AddListener((index) =>
            {
                SendCommand(dropdown, index, lastDropDownValue, tempAction);
                ExcuteCommand();
                lastDropDownValue = dropdown.value;
            });
        }

        protected void ComponentItemCommand(ComponentItem item,string value, Action<string> tempAction)
        {
            SendCommand(item, value, tempAction);
            ExcuteCommand();
        }

        protected void ExcuteCommand()
        {
            CommadManager.Instance.ExcuteAllCommand();
        }

        protected void SendCommand(InputField inputText, string value, Action<string> tempAction)
        {
            InputFieldReciver reciver = new InputFieldReciver();
            reciver.DoAction = tempAction;
            reciver.UnDoAction = tempAction;
            reciver.inputText = inputText;
            reciver.startValue = inputText.textComponent.text;
            reciver.value = value;
            Command c = new Command(reciver);
            CommadManager.Instance.AddCommand(c);
        }

        protected void SendCommand(Dropdown dropdown, int value, int lastDropDownValue, Action<int> tempAction)
        {
            DropdownReciver reciver = new DropdownReciver();
            reciver.DoAction = tempAction;
            reciver.UnDoAction = tempAction;
            reciver.dropdown = dropdown;
            reciver.startValue = lastDropDownValue;
            reciver.value = value;
            Command c = new Command(reciver);
            CommadManager.Instance.AddCommand(c);
        }


        protected void SendCommand(ComponentItem item, string value, Action<string> tempAction)
        {
            ImagePathReciver reciver = new ImagePathReciver();
            reciver.DoAction = tempAction;
            reciver.UnDoAction = tempAction;
            reciver.item = item;
            reciver.startValue = item.imageUrl;
            reciver.value = value;
            Command c = new Command(reciver);
            CommadManager.Instance.AddCommand(c);
        }

        #endregion
    }
}

