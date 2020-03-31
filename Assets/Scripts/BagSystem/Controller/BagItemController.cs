using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BagItemController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler,
    IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
{
    Bag.BagController m_bagCtl;
    private Transform tf;
    private Transform parentTf;
    private GameObject spriteTf;
    public int DicKey;
    //c#传递class数据类型都是传引用
    public void SetController(Bag.BagController bagCtl)
    {
        m_bagCtl = bagCtl;
    }
    //
    public void SetDicKey(int id)
    {
        this.DicKey = id;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //获得item的transform
        tf = eventData.pointerCurrentRaycast.gameObject.transform;
        parentTf = transform.parent;
        //在待拾取列表中不支持拖拽
        if (parentTf.tag != "CollectList")
        {
            
            Sprite sprite = m_bagCtl.GetItemInfo(parentTf, DicKey).Image;
            Debug.Log(m_bagCtl.GetItemInfo(parentTf, DicKey).name);
            spriteTf = m_bagCtl.GetDragSprite();
            spriteTf.transform.position = eventData.position;
            spriteTf.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
            spriteTf.SetActive(true);
            Debug.Log("Drag Begin");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        spriteTf.transform.position = eventData.position;
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.tag);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        spriteTf.transform.localPosition = Vector3.zero;
        spriteTf.SetActive(false);
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        m_bagCtl.HanderDropCtl(parentTf, this.transform, obj, DicKey);
    }
    /// <summary>
    /// 点击事件的三个回调函数
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //触发对应的事件
        //只有在待拾取列表中才支持点击
        parentTf = transform.parent;
        if (parentTf.tag == "CollectList")
        {
            Debug.Log("item被点击,id: " + DicKey);
            EventSys.LiteEventManager.Instance.TriggerEvent(EventSys.CollectKey.Collect, DicKey);
        }
    }
}
