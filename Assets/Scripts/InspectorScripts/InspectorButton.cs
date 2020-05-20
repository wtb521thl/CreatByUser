using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inspector
{
    public class InspectorButton : InspectorItem
    {
        InputField nameInputField;
        InputField vectorXInputField;
        InputField vectorYInputField;
        InputField sizeXInputField;
        InputField sizeYInputField;
        Dropdown actionDropDown;
        Dropdown actionObjDropdown;

        /// <summary>
        /// 初始化赋值
        /// </summary>
        /// <param name="contentArea"></param>
        /// <param name="_selectObj"></param>
        public override void Init(Transform contentArea, GameObject _selectObj)
        {
            base.Init(contentArea, _selectObj);

            InstantInspectorItem(contentArea);

            RefreshValue();

            InitCommand();

            InitEvent();

        }
        /// <summary>
        /// 初始化属性面板的物体
        /// </summary>
        /// <param name="contentArea"></param>
        private void InstantInspectorItem(Transform contentArea)
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

            GameObject tempBtnAction = GameObject.Instantiate(inspectorPanel.selectValue, contentArea);
            tempBtnAction.transform.Find("Title").GetComponent<Text>().text = "Action";
            actionDropDown = tempBtnAction.GetComponentInChildren<Dropdown>();

            GameObject tempActionObj = GameObject.Instantiate(inspectorPanel.selectValue, contentArea);
            tempActionObj.transform.Find("Title").GetComponent<Text>().text = "ActionObject";
            actionObjDropdown = tempActionObj.GetComponentInChildren<Dropdown>();
        }



        /// <summary>
        /// 刷新赋值
        /// </summary>
        public override void RefreshValue()
        {
            nameInputField.text = selectObj.name;
            vectorXInputField.text = selectObjRectTransform.position.x.ToString();
            vectorYInputField.text = selectObjRectTransform.position.y.ToString();

            sizeXInputField.text = selectObjRectTransform.sizeDelta.x.ToString();
            sizeYInputField.text = selectObjRectTransform.sizeDelta.y.ToString();


            SetOptionData<string>(actionDropDown, UiManager.Instance.allMethods);
            actionDropDown.SetValueWithoutNotify(string.IsNullOrEmpty(componentItem.actionStr) ? 0 : UiManager.Instance.allMethods.IndexOf(componentItem.actionStr));

            SetOptionData(actionObjDropdown, selectObj.transform.parent);
            actionObjDropdown.SetValueWithoutNotify(UiManager.Instance.GetGameObjectById(string.IsNullOrEmpty(componentItem.actionObjId) ? componentItem.timeID : componentItem.actionObjId).transform.GetSiblingIndex());

        }
        void SetOptionData<T>( Dropdown drop ,List<T> ls)
        {
            List<Dropdown.OptionData> ods = new List<Dropdown.OptionData>();
            for (int i = 0; i < ls.Count; i++)
            {
                ods.Add(new Dropdown.OptionData(ls[i].ToString()));
            }
            drop.options = ods;
        }
        void SetOptionData(Dropdown drop, Transform parent)
        {
            List<Dropdown.OptionData> ods = new List<Dropdown.OptionData>();
            for (int i = 0; i < parent.childCount; i++)
            {
                ods.Add(new Dropdown.OptionData(parent.GetChild(i).name.ToString()));
            }
            drop.options = ods;
        }

        /// <summary>
        /// 赋值后初始化事件，为物体赋值
        /// </summary>
        void InitEvent() {

            NameInputFieldChangeValue(nameInputField.text);
            VectorXInputFieldChangeValue(vectorXInputField.text);
            VectorYInputFieldChangeValue(vectorYInputField.text);
            SizeXInputFieldChangeValue(sizeXInputField.text);
            SizeYInputFieldChangeValue(sizeYInputField.text);
            ActionDropDownChangeValue(actionDropDown.value);
            ActionObjDropdownChangeValue(actionObjDropdown.value);
        }
        /// <summary>
        /// 初始化命令事件
        /// </summary>
        private void InitCommand()
        {
            InitInputFieldCommand(nameInputField, NameInputFieldChangeValue);
            InitInputFieldCommand(vectorXInputField, VectorXInputFieldChangeValue);
            InitInputFieldCommand(vectorYInputField, VectorYInputFieldChangeValue);
            InitInputFieldCommand(sizeXInputField, SizeXInputFieldChangeValue);
            InitInputFieldCommand(sizeYInputField, SizeYInputFieldChangeValue);
            InitDropDownCommand(actionDropDown, ActionDropDownChangeValue);
            InitDropDownCommand(actionObjDropdown, ActionObjDropdownChangeValue);

        }


        void InitInputFieldCommand(InputField inputField, Action<string> tempAction)
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
  
        void InitDropDownCommand(Dropdown dropdown,Action<int> tempAction)
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


        private void NameInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "Name", arg0);
        }

        private void VectorXInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "PosVectorX", arg0);
        }

        private void VectorYInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "PosVectorY", arg0);
        }

        private void SizeXInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "SizeVectorX", arg0);
        }

        private void SizeYInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "SizeVectorY", arg0);
        }

        private void ActionDropDownChangeValue(int arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "Action", UiManager.Instance.allMethods[arg0]);
        }

        private void ActionObjDropdownChangeValue(int arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "ActionObject", selectObjRectTransform.parent.GetChild(arg0).GetComponent<ComponentItem>().timeID);
        }

        void ExcuteCommand()
        {
            CommadManager.Instance.ExcuteAllCommand();
        }

        public void SendCommand(InputField inputText, string value, Action<string> tempAction)
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

        public void SendCommand(Dropdown dropdown, int value,int lastDropDownValue, Action<int> tempAction)
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
    }
}

