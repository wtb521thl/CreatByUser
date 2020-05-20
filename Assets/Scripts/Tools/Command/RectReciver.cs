using UnityEngine;
public class RectReciver: IReciver
{
    public RectTransform selfRect;

    Vector2 startPos;
    Vector2 startSizeDelte;

    public Vector2 endPos;
    public Vector2 endSizeDelte;


    public void Action() {
        startPos = selfRect.position;
        startSizeDelte = selfRect.sizeDelta;

        selfRect.position = endPos;
        selfRect.sizeDelta = endSizeDelte;
    }

    public void UndoAction() {
        selfRect.position = startPos;
        selfRect.sizeDelta = startSizeDelte;
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