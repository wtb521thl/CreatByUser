// *************************************************************************************************************
// 创建者: RAYDATA-WTB
// 创建时间: 2020/04/13 11:55:23
// 功能: 
// 版 本：v 1.2.0
// *************************************************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class ResourceManager :SingleMono<ResourceManager>
{
    public static Dictionary<string, GameObject> resourceObjs = new Dictionary<string, GameObject>();
    public static GameObject GetGameObject(string path)
    {

        if (resourceObjs.ContainsKey(path))
        {
            return resourceObjs[path];
        }
        else
        {
            GameObject tempGo= Resources.Load<GameObject>(path);
            resourceObjs.Add(path, tempGo);
            return tempGo;
        }
    }
    public GameObject GetGameobject(string path)
    {
        if (resourceObjs.ContainsKey(path))
        {
            return resourceObjs[path];
        }
        else
        {
            GameObject tempGo = Resources.Load<GameObject>(path);
            resourceObjs.Add(path, tempGo);
            return tempGo;
        }
    }
    public void GetGameobjectAsyc(string path , Action<GameObject> FinishAction)
    {
        if (resourceObjs.ContainsKey(path))
        {
            FinishAction(resourceObjs[path]);
        }
        else
        {
            StartCoroutine(GetObjAsyc(path, FinishAction));
        }
    }
    IEnumerator GetObjAsyc(string path, Action<GameObject> FinishAction)
    {
        ResourceRequest resourceRequest= Resources.LoadAsync<GameObject>(path);
        yield return resourceRequest.isDone;
        GameObject tempGo =(GameObject) resourceRequest.asset;
        resourceObjs.Add(path, tempGo);
        FinishAction(tempGo);
    }
}
