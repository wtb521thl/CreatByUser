using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inspector
{
    public class InspectorText : InspectorItem
    {

        public override void Init(Transform contentArea,GameObject _selectObj)
        {
            base.Init(contentArea, _selectObj);

            GameObject tempTextName = GameObject.Instantiate(oneValue, contentArea);
            tempTextName.transform.Find("Title").GetComponent<Text>().text = "Name";
            tempTextName.GetComponentInChildren<InputField>().text = selectObj.name;
            GameObject tempTextPos = GameObject.Instantiate(twoValue, contentArea);
            tempTextPos.transform.Find("Title").GetComponent<Text>().text = "Position";
            tempTextPos.transform.Find("InputVectorX").GetComponent<InputField>().text = selectObj.transform.position.x.ToString();
            tempTextPos.transform.Find("InputVectorY").GetComponent<InputField>().text = selectObj.transform.position.y.ToString();
        }
    }
}

