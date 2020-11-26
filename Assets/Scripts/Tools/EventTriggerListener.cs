using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{
    public Action<GameObject> OnMouseDown;
    public Action<GameObject> OnMouseUp;
    public Action<GameObject> OnMouseClick;
    public Action<GameObject> OnMouseDrag;
    public Action<GameObject> OnMouseBeginDrag;
    public Action<GameObject> OnMouseEndDrag;
    public Action<GameObject> OnMouseEnter;
    public Action<GameObject> OnMouseExit;

    public static EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener eventTriggerListener = go.GetComponent<EventTriggerListener>();
        if (eventTriggerListener == null)
        {
            eventTriggerListener = go.AddComponent<EventTriggerListener>();
        }
        return eventTriggerListener;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown?.Invoke(gameObject);
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        OnMouseClick?.Invoke(gameObject);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        OnMouseUp?.Invoke(gameObject);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        OnMouseDrag?.Invoke(gameObject);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter?.Invoke(gameObject);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit?.Invoke(gameObject);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        OnMouseBeginDrag?.Invoke(gameObject);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        OnMouseEndDrag?.Invoke(gameObject);
    }
}
