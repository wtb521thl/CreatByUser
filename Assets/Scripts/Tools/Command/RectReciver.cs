using UnityEngine;
public class RectReciver : IReciver
{
    public RectTransform selfRect;

    public System.Action<Vector2, Vector2> DoAction;
    public System.Action<Vector2, Vector2> UnDoAction;

    public Vector2 startPos;
    public Vector2 startSizeDelte;

    public Vector2 endPos;
    public Vector2 endSizeDelte;


    public void Action()
    {
        selfRect.position = endPos;
        selfRect.sizeDelta = endSizeDelte;
        DoAction?.Invoke(endPos, endSizeDelte);
    }

    public void UndoAction()
    {
        selfRect.position = startPos;
        selfRect.sizeDelta = startSizeDelte;
        UnDoAction?.Invoke(startPos, startSizeDelte);
    }
}


public class InputFieldReciver : IReciver
{
    public UnityEngine.UI.InputField inputText;

    public System.Action<string> DoAction;
    public System.Action<string> UnDoAction;


    public string value;

    public string startValue;

    public void Action()
    {
        inputText.SetTextWithoutNotify(value);
        DoAction?.Invoke(value);
    }

    public void UndoAction()
    {
        inputText.SetTextWithoutNotify(startValue);
        UnDoAction?.Invoke(startValue);
    }
}



public class DropdownReciver : IReciver
{
    public UnityEngine.UI.Dropdown dropdown;

    public System.Action<int> DoAction;
    public System.Action<int> UnDoAction;


    public int value;

    public int startValue;

    public void Action()
    {
        dropdown.SetValueWithoutNotify(value);
        DoAction?.Invoke(value);
    }

    public void UndoAction()
    {
        dropdown.SetValueWithoutNotify(startValue);
        UnDoAction?.Invoke(startValue);
    }
}

public class ImagePathReciver : IReciver
{
    public ComponentItem item;

    public System.Action<string> DoAction;
    public System.Action<string> UnDoAction;


    public string value;

    public string startValue;

    public void Action()
    {
        item.imageUrl = value;
        DoAction?.Invoke(value);
    }

    public void UndoAction()
    {
        item.imageUrl = startValue;
        UnDoAction?.Invoke(startValue);
    }
}


public class DestroyObjReciver : IReciver
{
    public GameObject obj;

    public System.Action DoAction;
    public System.Action UnDoAction;


    GameObject copyGameObj;


    public void Action()
    {
        copyGameObj = GameObject.Instantiate(obj, obj.transform.parent);
        copyGameObj.name = obj.name;
        copyGameObj.SetActive(false);
        GameObject.DestroyImmediate(obj);
    }

    public void UndoAction()
    {
        copyGameObj.SetActive(true);
        obj = copyGameObj;
    }
}


public class InstanceObjReciver : IReciver
{
    public GameObject obj;
    public Transform parent;

    public System.Action<GameObject> DoAction;
    public System.Action UnDoAction;


    GameObject instanceGameObj;


    public void Action()
    {
        instanceGameObj = GameObject.Instantiate(obj, parent);
        instanceGameObj.name = obj.name;
        DoAction?.Invoke(instanceGameObj);
    }

    public void UndoAction()
    {
        GameObject.DestroyImmediate(instanceGameObj);
    }
}


public class SetSelectGameObjectReciver : IReciver
{

    public GameObject willBeObj;

    public GameObject lastObj;

    public System.Action<GameObject> DoAction;
    public System.Action<GameObject> UnDoAction;
    public void Action()
    {
        DoAction?.Invoke(willBeObj);
    }

    public void UndoAction()
    {
        DoAction?.Invoke(lastObj);
    }
}