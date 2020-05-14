using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorPanel : MonoBehaviour
{
    GameObject selectObj;
    private void Awake()
    {
        selectObj = GameManager.Instance.selectGameobject;
        if (selectObj == null)
        {

        }
    }
}
