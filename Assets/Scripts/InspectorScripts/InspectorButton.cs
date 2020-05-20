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
        Dropdown actionDropDown;
        Dropdown actionObjDropdown;

        public override void Init(Transform contentArea, GameObject _selectObj)
        {
            base.Init(contentArea, _selectObj);

            GameObject tempBtnName = GameObject.Instantiate(inspectorPanel.oneValue, contentArea);
            tempBtnName.transform.Find("Title").GetComponent<Text>().text = "Name";
            nameInputField = tempBtnName.GetComponentInChildren<InputField>();

            GameObject tempBtnPos = GameObject.Instantiate(inspectorPanel.twoValue, contentArea);
            tempBtnPos.transform.Find("Title").GetComponent<Text>().text = "Position";
            vectorXInputField = tempBtnPos.transform.Find("InputVectorX").GetComponent<InputField>();

            vectorYInputField = tempBtnPos.transform.Find("InputVectorY").GetComponent<InputField>();

            GameObject tempBtnAction = GameObject.Instantiate(inspectorPanel.selectValue, contentArea);
            tempBtnAction.transform.Find("Title").GetComponent<Text>().text = "Action";
            actionDropDown = tempBtnAction.GetComponentInChildren<Dropdown>();

            GameObject tempActionObj = GameObject.Instantiate(inspectorPanel.selectValue, contentArea);
            tempActionObj.transform.Find("Title").GetComponent<Text>().text = "ActionObject";
            actionObjDropdown = tempActionObj.GetComponentInChildren<Dropdown>();


            RefreshValue();

            InitCommand();

            InitEvent();

        }
        void InitEvent() {
            //赋值后刷新事件
            NameInputFieldChangeValue(nameInputField.text);
            VectorXInputFieldChangeValue(vectorXInputField.text);
            VectorYInputFieldChangeValue(vectorYInputField.text);
            ActionDropDownChangeValue(actionDropDown.value);
            ActionObjDropdownChangeValue(actionObjDropdown.value);
        }

        private void InitCommand()
        {
            InitInputFieldCommand(nameInputField, NameInputFieldChangeValue);
            InitInputFieldCommand(vectorXInputField, VectorXInputFieldChangeValue);
            InitInputFieldCommand(vectorYInputField, VectorYInputFieldChangeValue);
            InitDropDownCommand(actionDropDown, ActionDropDownChangeValue);
            InitDropDownCommand(actionObjDropdown, ActionObjDropdownChangeValue);

        }

        public override void RefreshValue()
        {
            nameInputField.text = selectObj.name;
            vectorXInputField.text = selectObj.transform.position.x.ToString();
            vectorYInputField.text = selectObj.transform.position.y.ToString();

            List<Dropdown.OptionData> ods = new List<Dropdown.OptionData>();
            for (int i = 0; i < UiManager.Instance.allMethods.Count; i++)
            {
                ods.Add(new Dropdown.OptionData(UiManager.Instance.allMethods[i]));
            }
            actionDropDown.options = ods;
            actionDropDown.SetValueWithoutNotify(string.IsNullOrEmpty(componentItem.actionStr) ?0: UiManager.Instance.allMethods.IndexOf(componentItem.actionStr));

            List<Dropdown.OptionData> ts = new List<Dropdown.OptionData>();
            for (int i = 0; i < selectObj.transform.parent.childCount; i++)
            {
                ts.Add(new Dropdown.OptionData(selectObj.transform.parent.GetChild(i).name));
            }
            actionObjDropdown.options = ts;
            actionObjDropdown.SetValueWithoutNotify(UiManager.Instance.GetGameObjectById(string.IsNullOrEmpty(componentItem.actionObjId) ? componentItem.timeID : componentItem.actionObjId).transform.GetSiblingIndex());

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

        private void ActionDropDownChangeValue(int arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "Action", UiManager.Instance.allMethods[arg0]);
        }

        private void ActionObjDropdownChangeValue(int arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "ActionObject", selectObj.transform.parent.GetChild(arg0).GetComponent<ComponentItem>().timeID);

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

