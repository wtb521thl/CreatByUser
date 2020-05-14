using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Button changeModeBtn;
    Text changeModeBtnText;

    GameObject inspectorPanel;
    private void Start()
    {
        changeModeBtn.onClick.AddListener(ChangeMode);
        changeModeBtnText= changeModeBtn.GetComponentInChildren<Text>();
        EventCenter.AddListener<GameManager.GameMode>(EventSendType.ChangeGameMode, ChangeGameMode);
        RefreshBtnText();
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
        }else
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
        if (inspectorPanel == null)
        {
            inspectorPanel = Instantiate(Resources.Load<GameObject>("Prefabs/InspectorPanel"), transform);
        }
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
