using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Reflection;

public class UiManager : SingleMono<UiManager>
{
    /// <summary>
    /// 改变游戏模式按钮（编辑模式/运行模式）
    /// </summary>
    public Button changeModeBtn;

    /// <summary>
    /// 储存信息按钮
    /// </summary>
    public Button saveLayoutBtn;
    /// <summary>
    /// 读取信息按钮
    /// </summary>
    public Button loadLayoutBtn;


    Text changeModeBtnText;

    /// <summary>
    /// 属性面板子物体的载体(面板生成的时候才有赋值)
    /// </summary>
    GameObject inspectorPanel;

    /// <summary>
    /// 组件生成的载体
    /// </summary>
    public RectTransform objContainer;

    public List<string> allMethods = new List<string>();

    private void Start()
    {
        changeModeBtn.onClick.AddListener(ChangeMode);
        saveLayoutBtn.onClick.AddListener(SaveLayout);
        loadLayoutBtn.onClick.AddListener(LoadLayout);
        changeModeBtnText = changeModeBtn.GetComponentInChildren<Text>();
        EventCenter.AddListener<GameManager.GameMode>(EventSendType.ChangeGameMode, ChangeGameMode);
        RefreshBtnText();
        GetActions();
    }

    void GetActions()
    {
        Assembly ass = Assembly.GetAssembly(typeof(AllComponentMethods));

        for (int i = 0; i < ass.GetTypes().Length; i++)
        {
            if (ass.GetTypes()[i] == typeof(AllComponentMethods))
            {
                for (int j = 0; j < ass.GetTypes()[i].GetMethods().Length; j++)
                {
                    allMethods.Add(ass.GetTypes()[i].GetMethods()[j].Name);
                }
            }
        }


    }

    public GameObject GetGameObjectById(string Id)
    {
        GameObject go = null;
        ComponentItem[] componentItems = objContainer.GetComponentsInChildren<ComponentItem>();
        for (int i = 0; i < componentItems.Length; i++)
        {
            if (componentItems[i].timeID == Id)
            {
                go = componentItems[i].gameObject;
            }
        }
        return go;
    }

    private void LoadLayout()
    {
        JsonData data = DataManager.Instance.GetDataFromFile(PathStatic.LayoutJsonPath);
        if (string.IsNullOrEmpty(data.ToJson()))
        {
            return;
        }
        for (int i = 0; i < objContainer.childCount; i++)
        {
            DestroyImmediate(objContainer.GetChild(i).gameObject);
        }
        for (int i = 0; i < data.Count; i++)
        {
            GameObject tempInstanceObjResource = ResourceManager.Instance.GetGameobject(PathStatic.PrefabsComponentsPath + data[i]["Type"].ToJson().Trim('"'));
            if (tempInstanceObjResource != null)
            {
                GameObject tempInstanceObj = Instantiate(tempInstanceObjResource, objContainer);
                ComponentItem componentItem = tempInstanceObj.GetOrAddComponent<ComponentItem>();
                componentItem.timeID = data[i]["TimeId"].ToJson().Trim('"');
                componentItem.actionObjId = data[i]["ActionObj"].ToJson().Trim('"');
                componentItem.actionStr = data[i]["Action"].ToJson().Trim('"');
                tempInstanceObj.GetComponent<RectTransform>().position = new Vector2(float.Parse(data[i]["PosX"].ToJson().Trim('"')), float.Parse(data[i]["PosY"].ToJson().Trim('"')));
                tempInstanceObj.name = data[i]["Name"].ToJson().Trim('"');
            }
        }
    }

    private void SaveLayout()
    {
        JsonData keyData = new JsonData();
        ComponentItem[] componentItems = objContainer.GetComponentsInChildren<ComponentItem>();
        for (int i = 0; i < componentItems.Length; i++)
        {
            JsonData data = new JsonData();
            data["Name"] = componentItems[i].name;
            data["PosX"] = componentItems[i].transform.position.x;
            data["PosY"] = componentItems[i].transform.position.y;
            data["TimeId"] = componentItems[i].timeID;
            data["Type"] = componentItems[i].componentType.ToString();
            data["ActionObj"] = componentItems[i].actionObjId;
            data["Action"] = componentItems[i].actionStr;
            keyData.Add(data);
        }
        DataManager.Instance.SaveDataToFile(keyData, PathStatic.LayoutJsonPath);
    }

    private void LateUpdate()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (IsOverSelectGameobj(GameManager.Instance.selectGameobject) && GameManager.Instance.GetGameMode() == GameManager.GameMode.Editor)
            {
                ShowInspectorPanel();
            }
            else if (!IsOverSelectGameobj(inspectorPanel.gameObject))
            {
                CloseInspectorPanel();
            }
        }

    }
    PointerEventData pointerEventData;
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    public bool IsOverSelectGameobj(GameObject tempGo)
    {
        pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject == tempGo)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<GameManager.GameMode>(EventSendType.ChangeGameMode, ChangeGameMode);
    }
    private void ChangeGameMode(GameManager.GameMode arg1)
    {
        RefreshBtnText();

    }

    private void ChangeMode()
    {
        if (GameManager.Instance.GetGameMode() == GameManager.GameMode.Game)
        {
            GameManager.Instance.SetGameMode(GameManager.GameMode.Editor);
        }
        else
        {
            GameManager.Instance.SetGameMode(GameManager.GameMode.Game);
        }
    }

    void RefreshBtnText()
    {
        changeModeBtnText.text = GameManager.Instance.GetGameMode().ToString();
        if (GameManager.Instance.GetGameMode() == GameManager.GameMode.Editor)
        {
            ShowInspectorPanel();
        }
        else
        {
            CloseInspectorPanel();
        }
    }

    /// <summary>
    /// 展示属性面板
    /// </summary>
    void ShowInspectorPanel()
    {
        if (inspectorPanel != null)
        {
            CloseInspectorPanel();
        }
        inspectorPanel = Instantiate(ResourceManager.GetGameObject(PathStatic.InspectorPanelPrefab), transform);

    }
    /// <summary>
    /// 关闭属性面板
    /// </summary>
    void CloseInspectorPanel()
    {
        if (inspectorPanel != null)
        {
            DestroyImmediate(inspectorPanel);
        }
    }
}
