using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RightMousePanel : MonoBehaviour
{
    public string[] btnsName;
    RectTransform rightMousePanel;
    GameObject btnItem;
    public RectTransform objContainer;
    void InitObj()
    {
        if (rightMousePanel == null)
        {
            rightMousePanel = Instantiate(Resources.Load<GameObject>("Prefabs/RightMousePanel"), transform).GetComponent<RectTransform>();
        }
        if (btnItem == null)
        {
            btnItem = Resources.Load<GameObject>("Prefabs/Button");
        }
        for (int i = rightMousePanel.childCount - 1; i >= 0; i--)
        {
            Destroy(rightMousePanel.GetChild(i).gameObject);
        }
        for (int i = 0; i < btnsName.Length; i++)
        {
            GameObject tempBtnObj = Instantiate(Resources.Load<GameObject>("Prefabs/RightMousePanelButton"), rightMousePanel);
            tempBtnObj.name = btnsName[i];
            Button tempBtn = tempBtnObj.GetComponent<Button>();
            tempBtn.GetComponentInChildren<Text>().text = tempBtnObj.name;
            tempBtn.onClick.AddListener(() =>
            {
                InstanceObj(tempBtnObj);
                DestroyRightMousePanel();
            });
        }
    }
    void DestroyRightMousePanel()
    {
        if (rightMousePanel != null)
        {
            Destroy(rightMousePanel.gameObject);
        }
    }
    void InstanceObj(GameObject go)
    {
        GameObject tempInstanceObjResource = Resources.Load<GameObject>("Prefabs/" + go.name);
        if (tempInstanceObjResource != null)
        {
            GameObject tempInstanceObj = Instantiate(tempInstanceObjResource, objContainer);
            tempInstanceObj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
    Vector2 mousePos;
    void Update()
    {

        if (Input.GetMouseButtonDown(1)&&!EventSystem.current.IsPointerOverGameObject())
        {
            InitObj();
            mousePos = (Vector2)Input.mousePosition;
            rightMousePanel.anchoredPosition = mousePos - new Vector2(0, Screen.height);
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
