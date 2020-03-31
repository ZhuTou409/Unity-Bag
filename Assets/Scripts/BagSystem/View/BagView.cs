using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Bag
{
    public class BagView
    {
        public List<ViewSlot> m_slots;

        public GameObject m_bagList;
        public GameObject DragSprite;
        public GameObject m_bag;

        public int m_slotNum { get; }

        public GameObject closeBtn;
        //构造函数
        public BagView()
        {
            m_slots = new List<ViewSlot>();
            m_slotNum = 0;
            m_bag = GameObject.Find("Canvas/Bag");
            //m_bagList = m_bag.transform.Find("BagList").gameObject;
            m_bagList = m_bag.transform.Find("BagScrollView/Viewport/BagList").gameObject;

            DragSprite = ResManager.Instance.LoadPrefabFromRes("Prefab/DragSprite", true);
            DragSprite.transform.SetParent(m_bag.transform,false);
            DragSprite.transform.localPosition = Vector3.zero;
            DragSprite.SetActive(false);

            //按键
            Transform topRight = m_bag.transform.Find("TopRight");
            closeBtn = topRight.Find("Close").gameObject;
            
        }
        //初始化背包列表
        public void InitBagViewSlots(int SlotNum, string BagGridPath)
        {
            SetBagListArea(SlotNum, 70, 3);
            for (int i = 0;i<SlotNum;i++)
            {
                GameObject obj = ResManager.Instance.LoadPrefabFromRes(BagGridPath, true);
                obj.GetComponent<BagGrIdIns>().id = i;
                //int InstanceID = obj.GetInstanceID();
                ViewSlot st = new ViewSlot(obj,i);
                m_slots.Add(st);
                m_slots[i].slotObj.transform.SetParent(m_bagList.transform,false);
                //Debug.Log("槽的id：" + m_slots[i].slotID);
            }
        }
        //初始化背包滚动列表时要修改滚动区域
        private void SetBagListArea(int slotCount,int cellSizeY,int SpacingY)
        {
            m_bagList.GetComponent<RectTransform>().sizeDelta = new Vector2(0, slotCount * (cellSizeY+SpacingY));
        }
        //设置slot的bool值
        public void SetSlotEmptyOrNot(int key, bool flag)
        {
            m_slots[key].isEmpty = flag;
        }
        //设置背包界面是可见
        public void ShowBagUI()
        {
            m_bag.gameObject.SetActive(true);

        }
        //隐藏背包
        public void HideBagUI()
        {
            m_bag.gameObject.SetActive(false);
        }
        //返回当前背包ui状态
        public bool BagUIActive()
        {
            return m_bag.gameObject.activeSelf;
        }
        
    }
    //public class ViewItem
    //{
    //    public Transform number_trans;
    //    public GameObject m_obj;
    //    public BagItemController itemCtl;
    //    public ViewItem(GameObject m_obj, Transform number_trans,BagItemController itemCtl)
    //    {
    //        this.number_trans = number_trans;
    //        this.m_obj = m_obj;
    //        this.itemCtl = itemCtl;
    //    }
    //    public ViewItem()
    //    {
    //        number_trans = null;
    //        m_obj = null;
    //    }
    //}
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
            isEmpty = true;
        }
        public ViewSlot(GameObject obj)
        {
            slotObj = obj;
            slotID = -1;
            isEmpty = true;
        }
    }
}