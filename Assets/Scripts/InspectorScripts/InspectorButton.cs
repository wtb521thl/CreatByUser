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
            GameObject tempBtnName = GameObject.Instantiate(oneValue, contentArea);
            tempBtnName.transform.Find("Title").GetComponent<Text>().text = "Name";
            nameInputField = tempBtnName.GetComponentInChildren<InputField>();
            nameInputField.text = _selectObj.name.Split(',')[0];
            nameInputField.onEndEdit.AddListener(NameInputFieldChangeValue);
            GameObject tempBtnPos = GameObject.Instantiate(twoValue, contentArea);
            tempBtnPos.transform.Find("Title").GetComponent<Text>().text = "Position";
            vectorXInputField = tempBtnPos.transform.Find("InputVectorX").GetComponent<InputField>();
            vectorXInputField.text = selectObj.transform.position.x.ToString();
            vectorXInputField.onEndEdit.AddListener(VectorXInputFieldChangeValue);
            vectorYInputField = tempBtnPos.transform.Find("InputVectorY").GetComponent<InputField>();
            vectorYInputField.text = selectObj.transform.position.y.ToString();
            vectorYInputField.onEndEdit.AddListener(VectorYInputFieldChangeValue);
            GameObject tempBtnAction = GameObject.Instantiate(oneValue, contentArea);
            tempBtnAction.transform.Find("Title").GetComponent<Text>().text = "Action";
            actionInputField = tempBtnAction.GetComponentInChildren<InputField>();
            actionInputField.text = _selectObj.name.Split(',')[1];
            actionInputField.onEndEdit.AddListener(ActionInputFieldChangeValue);

            GameObject tempActionObj = GameObject.Instantiate(selectValue, contentArea);
            tempActionObj.transform.Find("Title").GetComponent<Text>().text = "ActionObject";
            actionDropdown = tempActionObj.GetComponentInChildren<Dropdown>();
            List<Dropdown.OptionData> ts = new List<Dropdown.OptionData>();
            for (int i = 0; i < selectObj.transform.parent.childCount; i++)
            {
                ts.Add(new Dropdown.OptionData(selectObj.transform.parent.GetChild(i).name));
            }
            actionDropdown.options = ts;
            actionDropdown.onValueChanged.AddListener(ActionDropdownChangeValue);
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
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "ActionObject", actionDropdown.options[arg0].text);
        }
    }
}

