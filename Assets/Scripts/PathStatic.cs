
using UnityEngine;

public static class PathStatic 
{
    #region Resource路径
    public static string PrefabsPath = "Prefabs/";
    public static string PrefabsComponentsPath = PrefabsPath + "Components/";
    public static string RightMousePanelPrefab = PrefabsPath+"RightMousePanel";
    public static string ButtonPrefab = PrefabsPath + "Button";
    public static string RightMousePanelButtonPrefab = PrefabsPath + "RightMousePanelButton";
    public static string InspectorPanelPrefab = PrefabsPath + "InspectorPanel";
    public static string InspectorItemTypePath = PrefabsPath + "InspectorItemType/";

    #endregion
    #region Streaming路径
    public static string LayoutJsonPath = Application.streamingAssetsPath + "/Layout.json";

    #endregion
}
