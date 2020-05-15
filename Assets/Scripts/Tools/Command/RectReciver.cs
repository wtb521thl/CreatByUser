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