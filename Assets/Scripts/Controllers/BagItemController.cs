using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BagItemController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Bag.BagController m_bagCtl;
    private Transform tf;
    private Transform parentTf;
    private GameObject spriteTf;
    public int id;
    //c#传递class数据类型都是传引用
    public void SetController(Bag.BagController bagCtl)
    {
        m_bagCtl = bagCtl;
    }
    //
    public void SetID(int id)
    {
        this.id = id;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //获得item的transform
        tf = eventData.pointerCurrentRaycast.gameObject.transform.parent;
        parentTf = transform.parent; 

        Sprite sprite = m_bagCtl.GetItemInfo(id).Image;

        spriteTf = m_bagCtl.GetDragSprite();
        spriteTf.transform.position = eventData.position;
        spriteTf.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
        spriteTf.SetActive(true);
        Debug.Log("Drag Begin");
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
        Debug.Log(obj.name);
        m_bagCtl.HanderDropCtl(parentTf, this.transform, obj, id);
    }
}
