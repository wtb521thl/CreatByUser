using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inspector
{
    public class InspectorText : InspectorItem
    {
        InputField userInputField;

        InputField fontSizeInputField;

        public override void Init(Transform contentArea,GameObject _selectObj)
        {
            base.Init(contentArea, _selectObj);

        }

        protected override void InstantInspectorItem(Transform contentArea)
        {
            base.InstantInspectorItem(contentArea);

            GameObject userInputObj = GameObject.Instantiate(inspectorPanel.oneValue, contentArea);
            userInputObj.transform.Find("Title").GetComponent<Text>().text = "UserInput";
            userInputField = userInputObj.GetComponentInChildren<InputField>();

            GameObject fontSizeInputObj = GameObject.Instantiate(inspectorPanel.oneValue, contentArea);
            fontSizeInputObj.transform.Find("Title").GetComponent<Text>().text = "FontSize";
            fontSizeInputField = fontSizeInputObj.GetComponentInChildren<InputField>();

        }

        public override void RefreshValue()
        {
            base.RefreshValue();

            userInputField.SetTextWithoutNotify(selectObj.GetComponentInChildren<Text>().text);
            fontSizeInputField.SetTextWithoutNotify(selectObj.GetComponentInChildren<Text>().fontSize.ToString());
        }

        /// <summary>
        /// 赋值后初始化事件，为物体赋值
        /// </summary>
        protected override void InitEvent()
        {

            base.InitEvent();
            UserInputFieldChangeValue(userInputField.text);
            FontSizeInputFieldChangeValue(fontSizeInputField.text);
        }
        /// <summary>
        /// 初始化命令事件
        /// </summary>
        protected override void InitCommand()
        {
            base.InitCommand();
            InitInputFieldCommand(userInputField, UserInputFieldChangeValue);
            InitInputFieldCommand(fontSizeInputField, FontSizeInputFieldChangeValue);
            fontSizeInputField.onValidateInput += OnValidateInput;
        }



        private void UserInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "UserInput", arg0 );
        }
        private void FontSizeInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "FontSize", arg0);
        }

    }
}

