using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Bag
{
    public class BagView
    {
        public List<ViewSlot> m_slots ;
        //存放当前界面中的item,key是该item所在的槽的位置
        public Dictionary<int, ViewItem> m_items;
        private GameObject m_bagList;

        public GameObject DragSprite;
        public int m_slotNum { get; }
        //构造函数
        public BagView()
        {
            m_slots = new List<ViewSlot>();
            m_items = new Dictionary<int, ViewItem>();
            m_slotNum = 0;
            m_bagList = GameObject.Find("Canvas/Bag/BagList");
            DragSprite = ResManager.Instance.LoadPrefabFromRes("Prefab/DragSprite", true);
            DragSprite.transform.SetParent(m_bagList.transform.parent);
            DragSprite.transform.localPosition = Vector3.zero;
            DragSprite.SetActive(false);
        }
        //初始化背包列表
        public void InitBagViewSlots(int SlotNum, string BagGridPath)
        {
            for(int i = 0;i<SlotNum;i++)
            {
                GameObject obj = ResManager.Instance.LoadPrefabFromRes(BagGridPath, true);
                obj.GetComponent<BagGrIdIns>().id = i;
                int InstanceID = obj.GetInstanceID();
                ViewSlot st = new ViewSlot(obj,i);
                m_slots.Add(st);
                m_slots[i].slotObj.transform.SetParent(m_bagList.transform);
                //Debug.Log("槽的id：" + m_slots[i].slotID);
            }
        }
        
    }
    public class ViewItem
    {
        public Transform number_trans;
        public GameObject m_obj;
        public BagItemController itemCtl;
        public ViewItem(GameObject m_obj, Transform number_trans,BagItemController itemCtl)
        {
            this.number_trans = number_trans;
            this.m_obj = m_obj;
            this.itemCtl = itemCtl;
        }
        public ViewItem()
        {
            number_trans = null;
            m_obj = null;
        }
    }
    public class ViewSlot
    {
        public GameObject slotObj;
        //用getInstanceID获取每个槽对应的id
        public int slotID;
        public bool isEmpty;
        public ViewSlot(GameObject obj,int id)
        {
            slotObj = obj;
            slotID = id;
            isEmpty = false;
        }
        public ViewSlot(GameObject obj)
        {
            slotObj = obj;
            slotID = -1;
            isEmpty = false;
        }
    }
}