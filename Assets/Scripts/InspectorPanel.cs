using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using Inspector;

public class InspectorPanel : MonoBehaviour
{
    public Transform contentArea;
    private void Awake()
    {
        Refresh();
    }


    void Refresh()
    {
        if (GameManager.Instance.selectGameobject != null)
        {
            for (int i = contentArea.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(contentArea.GetChild(i).gameObject);
            }



            ////反射获取 代替switch
            //string tempStr = "Inspector" + GameManager.Instance.selectGameobject.GetComponent<ComponentItem>().componentType.ToString();
            //Assembly assembly = Assembly.Load("Inspector");
            //object obj = assembly.CreateInstance("Inspector." + tempStr);
            //InspectorItem inspectorItem = (InspectorItem)obj;
            //inspectorItem.Init(contentArea, GameManager.Instance.selectGameobject);


            switch (GameManager.Instance.selectGameobject.GetComponent<ComponentItem>().componentType)
            {
                case ComponentType.Button:
                    InspectorItem inspectorItemBtn = new InspectorButton();
                    inspectorItemBtn.Init(contentArea, GameManager.Instance.selectGameobject);
                    break;
                case ComponentType.Text:

                    InspectorItem inspectorItemText = new InspectorText();
                    inspectorItemText.Init(contentArea, GameManager.Instance.selectGameobject);
                    break;
                case ComponentType.Image:
                    InspectorItem inspectorItemImage = new InspectorImage();
                    inspectorItemImage.Init(contentArea, GameManager.Instance.selectGameobject);
                    break;
            }
        }
    }
}
