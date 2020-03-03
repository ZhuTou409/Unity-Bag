using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UGUIEventListener : EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
    public VoidDelegate onLongPress;                    //长按按键

    public float interval = 1f;                         //长按按键判断间隔
    public float invokeInterval = 0.2f;                 //长按状态方法调用间隔
    private bool isPointDown = false;
    private float lastInvokeTime;                       //鼠标点击下的时间
    private float timer;
    static public UGUIEventListener Get(GameObject go)
    {
        UGUIEventListener listener = go.GetComponent<UGUIEventListener>();
        if (listener == null) listener = go.AddComponent<UGUIEventListener>();
        return listener;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(gameObject);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        isPointDown = true;
        lastInvokeTime = Time.time;
        if (onDown != null) onDown(gameObject);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(gameObject);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        isPointDown = false;                            //鼠标移出按钮时推出长按状态
        if (onExit != null) onExit(gameObject);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        isPointDown = false;
        if (onUp != null) onUp(gameObject);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(gameObject);
    }
    private void Update()
    {
        if (isPointDown)
        {
            if (Time.time-lastInvokeTime>interval)
            {
                timer += Time.deltaTime;
                if (timer>invokeInterval)
                {
                    onLongPress.Invoke(gameObject);
                    timer = 0;
                }
            }
        }
    }
}