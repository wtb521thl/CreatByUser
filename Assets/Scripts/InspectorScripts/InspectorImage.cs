using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inspector
{
    public class InspectorImage : InspectorItem
    {

        public override void Init(Transform contentArea,GameObject _selectObj)
        {
            base.Init(contentArea, _selectObj);

            GameObject tempImageName = GameObject.Instantiate(oneValue, contentArea);
            tempImageName.transform.Find("Title").GetComponent<Text>().text = "Name";
            tempImageName.GetComponentInChildren<InputField>().text = selectObj.name;
            GameObject tempImagePos = GameObject.Instantiate(twoValue, contentArea);
            tempImagePos.transform.Find("Title").GetComponent<Text>().text = "Position";
            tempImagePos.transform.Find("InputVectorX").GetComponent<InputField>().text = selectObj.transform.position.x.ToString();
            tempImagePos.transform.Find("InputVectorY").GetComponent<InputField>().text = selectObj.transform.position.y.ToString();
        }
    }
}

