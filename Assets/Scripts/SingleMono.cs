using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMono<T>:MonoBehaviour where T:MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
            }
            if (instance == null)
            {
                GameObject go= new GameObject("Single_" + typeof(T).ToString());
                instance=go.AddComponent<T>();
            }
            return instance;
        }
    }

}
