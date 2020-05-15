using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectEx 
{
    public static T GetOrAddComponent<T>(this GameObject go) where T:Component
    {
        T c;
        c = go.GetComponent<T> ();
        if (c == null)
        {
          c=  go.AddComponent<T>();
        }
        return c;
    }
}
