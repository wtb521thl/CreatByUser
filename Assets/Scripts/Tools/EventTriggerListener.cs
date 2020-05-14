using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{
    public Action OnMouseDown;
    public Action OnMouseUp;
    public Action OnMouseClick;
    public Action OnMouseDrag;
    public Action OnMouseBeginDrag;
    public Action OnMouseEndDrag;
    public Action OnMouseEnter;
    public Action OnMouseExit;

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
        OnMouseDown?.Invoke();
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        OnMouseClick?.Invoke();
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        OnMouseUp?.Invoke();
    }
    public override void OnDrag(PointerEventData eventData)
    {
        OnMouseDrag?.Invoke();
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter?.Invoke();
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit?.Invoke();
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        OnMouseBeginDrag?.Invoke();
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        OnMouseEndDrag?.Invoke();
    }
}
