using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RightMousePanel : MonoBehaviour
{
    RectTransform rightMousePanel;
    GameObject btnItem;

    void InitObj()
    {
        if (rightMousePanel == null)
        {
            rightMousePanel = Instantiate(ResourceManager.Instance.GetGameobject(PathStatic.RightMousePanelPrefab), transform).GetComponent<RectTransform>();
        }
        if (btnItem == null)
        {
            btnItem = ResourceManager.Instance.GetGameobject(PathStatic.ButtonPrefab);
        }
        for (int i = rightMousePanel.childCount - 1; i >= 0; i--)
        {
            Destroy(rightMousePanel.GetChild(i).gameObject);
        }

        //生成右键菜单
        for (int i = 0; i < GameManager.Instance.rightButtonsName.Length; i++)
        {
            GameObject tempBtnObj = Instantiate(ResourceManager.Instance.GetGameobject(PathStatic.RightMousePanelButtonPrefab), rightMousePanel);
            tempBtnObj.name = GameManager.Instance.rightButtonsName[i];
            Button tempBtn = tempBtnObj.GetComponent<Button>();
            tempBtn.GetComponentInChildren<Text>().text = tempBtnObj.name;
            tempBtn.onClick.AddListener(() =>
            {
                InstanceComponentObj(tempBtnObj);
                DestroyRightMousePanel();
            });
        }
    }
    void DestroyRightMousePanel()
    {
        if (rightMousePanel != null)
        {
            DestroyImmediate(rightMousePanel.gameObject);
        }
    }
    /// <summary>
    /// 生成组件
    /// </summary>
    /// <param name="go"></param>
    void InstanceComponentObj(GameObject go)
    {


        GameObject tempInstanceObjResource = ResourceManager.Instance.GetGameobject(PathStatic.PrefabsComponentsPath + go.name);
        if (tempInstanceObjResource != null)
        {
            InstanceObjReciver reciver = new InstanceObjReciver();
            reciver.obj = tempInstanceObjResource;
            reciver.DoAction += FinishReciverAction;
            reciver.parent = UiManager.Instance.objContainer;
            Command c = new Command(reciver);
            CommadManager.Instance.AddCommand(c);
            CommadManager.Instance.ExcuteAllCommand();
            //GameObject tempInstanceObj = Instantiate(tempInstanceObjResource,UiManager.Instance.objContainer);
            //tempInstanceObj.GetComponent<RectTransform>().position = Input.mousePosition;
            //tempInstanceObj.name = go.name;
        }
    }

    private void FinishReciverAction(GameObject obj)
    {
        obj.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    Vector2 mousePos;
    void Update()
    {

        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            InitObj();
            mousePos = (Vector2)Input.mousePosition;
            rightMousePanel.position = mousePos;
        }

        if (rightMousePanel != null)
        {
            Rect rect = rightMousePanel.rect;
            rect.position += mousePos;
            if (!rect.Contains(Input.mousePosition))
            {
                if (Input.GetMouseButtonDown(0))
                {

                    DestroyRightMousePanel();
                }
            }

            DrawLine(rect);
        }
    }

    private static void DrawLine(Rect rect)
    {
        Vector2 p1 = new Vector2(rect.center.x - rect.size.x / 2f, rect.center.y + rect.size.y / 2f);
        Vector2 p2 = new Vector2(rect.center.x + rect.size.x / 2f, rect.center.y + rect.size.y / 2f);
        Vector2 p3 = new Vector2(rect.center.x + rect.size.x / 2f, rect.center.y - rect.size.y / 2f);
        Vector2 p4 = new Vector2(rect.center.x - rect.size.x / 2f, rect.center.y - rect.size.y / 2f);

        Debug.DrawLine(p1, p2, Color.blue);
        Debug.DrawLine(p2, p3, Color.blue);
        Debug.DrawLine(p3, p4, Color.blue);
        Debug.DrawLine(p4, p1, Color.blue);
    }
}
