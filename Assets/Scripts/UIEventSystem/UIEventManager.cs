using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace EventSys
{
    public class UIEventManager : EventTrigger
    {
        public static UIEventManager Get(GameObject obj)
        {
            UIEventManager temp = obj.GetComponent<UIEventManager>();
            if (temp == null)
            {
                temp = obj.AddComponent<UIEventManager>();
            }
            return temp;
        }
        public UnityAction<PointerEventData> OnClickCallBack;
        public UnityAction<PointerEventData> OnPointerEnterCallBack;
        public UnityAction<PointerEventData> OnPointerLeaveCallBack;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (OnClickCallBack != null)
            {
                OnClickCallBack(eventData);
            }
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (OnPointerEnterCallBack != null)
            {
                OnPointerEnterCallBack(eventData);
            }
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (OnPointerLeaveCallBack != null)
            {
                OnPointerLeaveCallBack(eventData);
            }
        }
    }
}

