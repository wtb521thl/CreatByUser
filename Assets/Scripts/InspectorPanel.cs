using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using Inspector;
using System;

public class InspectorPanel : MonoBehaviour
{
    public Transform contentArea;

    public GameObject oneValue;
    public GameObject twoValue;
    public GameObject selectValue;
    public GameObject imageValue;

    InspectorItem inspectorItem;

    private void Awake()
    {
        Init();
        InitItem();
        EventCenter.AddListener<GameObject, string, string>(EventSendType.InspectorChange, InspectorChange);
    }
    /// <summary>
    /// 初始化预设
    /// </summary>
    void Init()
    {
        oneValue = ResourceManager.Instance.GetGameobject(PathStatic.InspectorItemTypePath + "OneValue");
        twoValue = ResourceManager.Instance.GetGameobject(PathStatic.InspectorItemTypePath + "TwoValue");
        selectValue = ResourceManager.Instance.GetGameobject(PathStatic.InspectorItemTypePath + "SelectValue");
        imageValue = ResourceManager.Instance.GetGameobject(PathStatic.InspectorItemTypePath + "ImageValue");
    }

    private void InspectorChange(GameObject arg1, string arg2, string arg3)
    {
        Refresh();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<GameObject, string, string>(EventSendType.InspectorChange, InspectorChange);
    }
    /// <summary>
    /// 根据选中物体生成对应的结构
    /// </summary>
    public void InitItem()
    {
        if (GameManager.Instance.selectGameobject != null)
        {
            for (int i = contentArea.childCount - 1; i >= 0; i--)
            {
                Destroy(contentArea.GetChild(i).gameObject);
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
                    inspectorItem = new InspectorButton();
                    break;
                case ComponentType.Text:
                    inspectorItem = new InspectorText();
                    break;
                case ComponentType.Image:
                    inspectorItem = new InspectorImage();
                    break;
            }
            inspectorItem.Init(contentArea, GameManager.Instance.selectGameobject);
        }
    }


    public void Refresh()
    {
        if (GameManager.Instance.selectGameobject != null)
        {
            inspectorItem.RefreshValue();
        }
    }
}
