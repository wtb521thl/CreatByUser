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
        InputField actionInputField;
        Dropdown actionDropdown;

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

            GameObject tempBtnAction = GameObject.Instantiate(inspectorPanel.oneValue, contentArea);
            tempBtnAction.transform.Find("Title").GetComponent<Text>().text = "Action";
            actionInputField = tempBtnAction.GetComponentInChildren<InputField>();

            GameObject tempActionObj = GameObject.Instantiate(inspectorPanel.selectValue, contentArea);
            tempActionObj.transform.Find("Title").GetComponent<Text>().text = "ActionObject";
            actionDropdown = tempActionObj.GetComponentInChildren<Dropdown>();


            RefreshValue();

            InitCommand();

        }

        private void InitCommand()
        {
            InitInputFieldCommand(nameInputField, NameInputFieldChangeValue);
            InitInputFieldCommand(vectorXInputField, VectorXInputFieldChangeValue);
            InitInputFieldCommand(vectorYInputField, VectorYInputFieldChangeValue);
            InitInputFieldCommand(actionInputField, ActionInputFieldChangeValue);
            InitDropDownCommand(actionDropdown);
        }

        public override void RefreshValue()
        {
            nameInputField.text = selectObj.name;
            vectorXInputField.text = selectObj.transform.position.x.ToString();
            vectorYInputField.text = selectObj.transform.position.y.ToString();
            actionInputField.text = componentItem.actionStr;
            List<Dropdown.OptionData> ts = new List<Dropdown.OptionData>();
            for (int i = 0; i < selectObj.transform.parent.childCount; i++)
            {
                ts.Add(new Dropdown.OptionData(selectObj.transform.parent.GetChild(i).name));
            }
            actionDropdown.options = ts;

            actionDropdown.SetValueWithoutNotify(UiManager.Instance.GetGameObjectById(string.IsNullOrEmpty(componentItem.actionObjId) ? componentItem.timeID : componentItem.actionObjId).transform.GetSiblingIndex());
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
        int lastDropDownValue=0;
        void InitDropDownCommand(Dropdown dropdown)
        {
            lastDropDownValue = dropdown.value;
            dropdown.onValueChanged.AddListener((index) =>
            {
                SendCommand(dropdown, index);
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

        private void ActionInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "Action", arg0);
        }

        private void ActionDropdownChangeValue(int arg0)
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

        public void SendCommand(Dropdown dropdown, int value)
        {
            DropdownReciver reciver = new DropdownReciver();
            reciver.DoAction = ActionDropdownChangeValue;
            reciver.UnDoAction = ActionDropdownChangeValue;
            reciver.dropdown = dropdown;
            reciver.startValue = lastDropDownValue;
            reciver.value = value;
            Command c = new Command(reciver);
            CommadManager.Instance.AddCommand(c);
        }
    }
}

