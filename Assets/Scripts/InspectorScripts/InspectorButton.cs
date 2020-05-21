using UnityEngine;
using UnityEngine.UI;

namespace Inspector
{
    public class InspectorButton : InspectorItem
    {
        InputField userInputField;
        InputField fontSizeInputField;
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

        }
        /// <summary>
        /// 初始化属性面板的物体
        /// </summary>
        /// <param name="contentArea"></param>
        protected override void InstantInspectorItem(Transform contentArea)
        {
            base.InstantInspectorItem(contentArea);

            GameObject userInputObj = GameObject.Instantiate(inspectorPanel.oneValue, contentArea);
            userInputObj.transform.Find("Title").GetComponent<Text>().text = "UserInput";
            userInputField = userInputObj.GetComponentInChildren<InputField>();

            GameObject fontSizeInputObj = GameObject.Instantiate(inspectorPanel.oneValue, contentArea);
            fontSizeInputObj.transform.Find("Title").GetComponent<Text>().text = "FontSize";
            fontSizeInputField = fontSizeInputObj.GetComponentInChildren<InputField>();

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
            base.RefreshValue();

            userInputField.SetTextWithoutNotify(selectObj.GetComponentInChildren<Text>().text);
            fontSizeInputField.SetTextWithoutNotify( selectObj.GetComponentInChildren<Text>().fontSize.ToString());

            SetOptionData<string>(actionDropDown, UiManager.Instance.allMethods);
            actionDropDown.SetValueWithoutNotify(string.IsNullOrEmpty(componentItem.actionStr) ? 0 : UiManager.Instance.allMethods.IndexOf(componentItem.actionStr));

            SetOptionData(actionObjDropdown, selectObj.transform.parent);
            GameObject tempObj = UiManager.Instance.GetGameObjectById(string.IsNullOrEmpty(componentItem.actionObjId) ? componentItem.timeID : componentItem.actionObjId);
            actionObjDropdown.SetValueWithoutNotify(tempObj==null?0: tempObj.transform.GetSiblingIndex());

        }

        /// <summary>
        /// 赋值后初始化事件，为物体赋值
        /// </summary>
        protected override void InitEvent() {

            base.InitEvent();
            UserInputFieldChangeValue(userInputField.text);
            FontSizeInputFieldChangeValue(fontSizeInputField.text);


            ActionDropDownChangeValue(actionDropDown.value);
            ActionObjDropdownChangeValue(actionObjDropdown.value);
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


            InitDropDownCommand(actionDropDown, ActionDropDownChangeValue);
            InitDropDownCommand(actionObjDropdown, ActionObjDropdownChangeValue);

        }

        private void UserInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "UserInput", arg0);
        }

        private void FontSizeInputFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "FontSize", arg0);
        }

        private void ActionDropDownChangeValue(int arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "Action", UiManager.Instance.allMethods[arg0]);
        }

        private void ActionObjDropdownChangeValue(int arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "ActionObject", selectObjRectTransform.parent.GetChild(arg0).GetComponent<ComponentItem>().timeID);
        }


    }
}

