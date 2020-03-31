using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace EventSys
{

    public class LiteEventManager 
    {
        private class EventMode:UnityEvent<object>
        {

        }
        private Dictionary<Enum, EventMode> m_eventDictionary = new Dictionary<Enum, EventMode>();
        /// <summary>
        /// 单例模式
        /// </summary>
        #region
        private static LiteEventManager instance;

        public static LiteEventManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new LiteEventManager();
                }
                return instance;
            }
        }
        #endregion
        /// <summary>
        /// 添加监听者
        /// </summary>
        /// <param name="eventKey"></param>
        /// <param name="listener"></param>
        public void Register(Enum eventKey,UnityAction<object> listener)
        {
            EventMode tempEvent = null;
            if(m_eventDictionary.TryGetValue(eventKey,out tempEvent))
            {
                tempEvent.AddListener(listener);
            }
            else
            {
                tempEvent = new EventMode();
                tempEvent.AddListener(listener);
                m_eventDictionary.Add(eventKey, tempEvent);
            }
        }

        public void RemoveListering(Enum eventKey,UnityAction<object> listener)
        {
            if (instance == null) return;
            EventMode tempEvent = null;
            if(m_eventDictionary.TryGetValue(eventKey,out tempEvent))
            {
                tempEvent.RemoveListener(listener);
            }
        }

        public void TriggerEvent(Enum eventKey,object trigger)
        {
            EventMode tempEvent;
            if(m_eventDictionary.TryGetValue(eventKey,out tempEvent))
            {
                tempEvent.Invoke(trigger);
            }
        }
    }
}

