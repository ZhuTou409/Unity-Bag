using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSys;

public class Main : MonoBehaviour
{

    void Start()
    {
        LiteEventManager.Instance.Register(CollectKey.Trig, Func);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            LiteEventManager.Instance.TriggerEvent(CollectKey.Trig, "hello,我要触发你");
        }
    }

    void Func(object a)
    {
        Debug.Log(a+"func函数被触发");
    }
}
