using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectorPanel : MonoBehaviour
{
    GameObject selectObj;

    GameObject oneValue;
    GameObject twoValue;
    GameObject selectValue;

    public Transform contentArea;
    private void Awake()
    {
        oneValue = Resources.Load<GameObject>("Prefabs/InspectorItemType/OneValue");
        twoValue = Resources.Load<GameObject>("Prefabs/InspectorItemType/TwoValue");
        selectValue = Resources.Load<GameObject>("Prefabs/InspectorItemType/SelectValue");

    }
    private void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        selectObj = GameManager.Instance.selectGameobject;
        if (selectObj != null)
        {
            for (int i = contentArea.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(contentArea.GetChild(i).gameObject);
            }
            switch (selectObj.GetComponent<ComponentItem>().componentType)
            {
                case ComponentType.Button:
                    GameObject tempBtnName = Instantiate(oneValue, contentArea);
                    tempBtnName.transform.Find("Title").GetComponent<Text>().text = "Name";
                    GameObject tempBtnPos = Instantiate(twoValue, contentArea);
                    tempBtnPos.transform.Find("Title").GetComponent<Text>().text = "Position";
                    GameObject tempBtnAction = Instantiate(oneValue, contentArea);
                    tempBtnName.transform.Find("Title").GetComponent<Text>().text = "Action";
                    break;
                case ComponentType.Text:
                    GameObject tempTextName = Instantiate(oneValue, contentArea);
                    tempTextName.transform.Find("Title").GetComponent<Text>().text = "Name";
                    GameObject tempTextPos = Instantiate(twoValue, contentArea);
                    tempTextPos.transform.Find("Title").GetComponent<Text>().text = "Position";
                    break;
                case ComponentType.Image:
                    GameObject tempImageName = Instantiate(oneValue, contentArea);
                    tempImageName.transform.Find("Title").GetComponent<Text>().text = "Name";
                    GameObject tempImagePos = Instantiate(twoValue, contentArea);
                    tempImagePos.transform.Find("Title").GetComponent<Text>().text = "Position";
                    break;
            }
        }
    }
}
