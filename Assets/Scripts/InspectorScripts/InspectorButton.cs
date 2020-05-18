using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inspector
{
    public class InspectorButton : InspectorItem
    {

        public override void Init(Transform contentArea,GameObject _selectObj)
        {
            base.Init(contentArea, _selectObj);
            GameObject tempBtnName = GameObject.Instantiate(oneValue, contentArea);
            tempBtnName.transform.Find("Title").GetComponent<Text>().text = "Name";
            tempBtnName.GetComponentInChildren<InputField>().text = selectObj.name;
            GameObject tempBtnPos = GameObject.Instantiate(twoValue, contentArea);
            tempBtnPos.transform.Find("Title").GetComponent<Text>().text = "Position";
            tempBtnPos.transform.Find("InputVectorX").GetComponent<InputField>().text = selectObj.transform.position.x.ToString();
            tempBtnPos.transform.Find("InputVectorY").GetComponent<InputField>().text = selectObj.transform.position.y.ToString();
            GameObject tempBtnAction = GameObject.Instantiate(oneValue, contentArea);
            tempBtnAction.transform.Find("Title").GetComponent<Text>().text = "Action";
        }
    }
}

