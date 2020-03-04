﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Bag
{
    public class BagController 
    {
        private BagView m_bagView;
        public BagModel m_bagModel;
        private int m_initBagItemNum;
        private int m_maxBagItemNum;

        public BagController()
        {
            //初始化五个格子
            m_initBagItemNum = 5;
            //最多10个格子
            m_maxBagItemNum = 10;
            //实例化view和model
            m_bagView = new BagView();
            m_bagModel = new BagModel();
            //初始化背包格子和数据
            InitBag(m_initBagItemNum, "Prefab/BagGrid");
        }

        public void Update()
        {
        }
        //初始化背包格子和数据
        private void InitBag(int BagItemNum,string BagGridPath)
        {
            m_bagView.InitBagViewSlots(BagItemNum, BagGridPath);
            m_bagModel.InitItemsData();
        }

        //初始化装备系统的槽slot(未开发)
        private void InitEquipSysSlot()
        {
            
        }

        //添加item
        public void AddItem(BagItem it)
        {
            int id = it.Id;
            int itemNum = m_bagModel.BagItemsDic.Count;
            bool flag = false;
            int firstEmptySlot = -1;
            //获取字典的keys并存入一个列表中
            //List<int> keyList = new List<int>();
            //keyList.AddRange(m_bagModel.BagItemsDic.Keys);
            //背包中已经有这个id的物品
            if (m_bagModel.BagItemsDic.TryGetValue(id,out BagItemInfo value))
            {
                //只有药品和子弹的数量可以叠加
                if (it.Type == ItemType.bullet || it.Type == ItemType.medicine)
                {
                    value.Count += it.Count;
                    ChangeItemNum(value, value.Count);
                    flag = true;
                }
            }
            else
            {
                //背包中没有这个物品，遍历背包槽，找到第一个空槽
                for (int i = 0; i < m_bagView.m_slots.Count; i++)
                {
                    Debug.Log(i + " : " + m_bagView.m_slots[i].isEmpty);
                    if (m_bagView.m_slots[i].isEmpty == true)
                    {
                        firstEmptySlot = i;
                        break;
                    }
                }
                //背包还有空位
                if (firstEmptySlot != -1)
                {
                    //更新背包存放item的字典
                    //实例化一个item
                    GameObject obj = ResManager.Instance.LoadPrefabFromRes("Prefab/BagItem", true);
                    Sprite sprite = ResManager.Instance.LoadSpriteFromRes(it.ImageName);
                    //初始化item,加载名字，数量，图片
                    Transform numberObj = obj.transform.GetChild(0);
                    Text number = numberObj.GetComponent<Text>();
                    number.text = it.Count.ToString();

                    Image img = obj.transform.GetChild(1).GetComponent<Image>();
                    img.sprite = sprite;

                    Text name = obj.transform.GetChild(2).GetComponent<Text>();
                    name.text = it.Name;
                    //赋值id，传递bagController
                    BagItemController itemView = obj.GetComponent<BagItemController>();
                    itemView.SetController(this);
                    itemView.SetID(firstEmptySlot);
                    //实例化一个BagItemInfo
                    BagItemInfo itInfo = new BagItemInfo(firstEmptySlot, it.Name, sprite, it.Count, it.Type, obj);
                    Debug.Log("id: " + id + "name: " + itInfo.Image.name);
                    //挂载item到空槽中
                    MountPrefabToSlot(itInfo, firstEmptySlot);
                    Debug.Log(itInfo.Name);
                }
            }
        }
        //public void AddItem(BagItem it)
        //{
        //    int id = it.Id;
        //    int itemNum = m_bagModel.Items.Count;
        //    bool flag = false;
        //    int firstEmptySlot = -1;
        //    for(int i = 0;i<itemNum;i++)
        //    {
        //        //背包中已经有这个id的物品,只有药品和子弹的数量可以叠加
        //        if (id == m_bagModel.Items[i].Id && (it.Type == ItemType.bullet || it.Type == ItemType.medicine))
        //        {
        //            m_bagModel.Items[i].Count += it.Count;
        //            ChangeItemNum(id, m_bagModel.Items[i].Count);
        //            flag = true;
        //        }
        //        if (m_bagModel.Items[i].Id == -1)
        //        {
        //            firstEmptySlot = i;
        //            break;
        //        }
        //    }
        //    //没有这个物品,且背包还有空位
        //    if(flag == false && firstEmptySlot != -1)
        //    {
        //        //更新槽的数值
        //        m_bagModel.Items[firstEmptySlot] = it;
        //        //找个空槽把物品放进去
        //        GameObject obj = ResManager.Instance.LoadPrefabFromRes("Prefab/BagItem", true);

        //        //item挂载的三个gameObject顺序已知,故使用getChild提高效率
        //        //初始化item,加载名字，数量，图片
        //        Transform numberObj = obj.transform.GetChild(0);
        //        Text number = numberObj.GetComponent<Text>();
        //        number.text = it.Count.ToString();

        //        Image img = obj.transform.GetChild(1).GetComponent<Image>();
        //        img.sprite = ResManager.Instance.LoadSpriteFromRes(it.ImageName);

        //        Text name = obj.transform.GetChild(2).GetComponent<Text>();
        //        name.text = it.Name;
        //        //赋值id，传递bagController
        //        BagItemController itemView = obj.GetComponent<BagItemController>();
        //        itemView.SetController(this);
        //        itemView.SetID(firstEmptySlot);
        //        //更新item的位置信息
        //        ViewItem viewIt = new ViewItem(obj, numberObj,itemView);
        //        it.SetItemTransform(viewIt);
        //        //正式挂载
        //        MountPrefabToSlot(it, firstEmptySlot);
        //        Debug.Log(it.Name);
        //    }

        //}

        //处理物品拖拽
        public void HanderDropCtl(Transform parentSlot, Transform item,GameObject obj,int itemId)
        {
            string tag = obj.tag;
            //空槽
            if(tag == "Grid")
                DropToEmptySlot(parentSlot, item, obj);
            //槽内有item
            else if(tag == "BagItem")
            {
                ChangeItem(parentSlot, item, obj);
            }
            else if (tag == "Pakage")
            {

            }
            else
            {

            }
        }
        public void DropToEmptySlot(Transform parentSlot, Transform item, GameObject dropSlot)
        {
            string tag = parentSlot.tag;
            int parentId = parentSlot.GetComponent<BagGrIdIns>().id;
            int dropSlotId = dropSlot.GetComponent<BagGrIdIns>().id;
            //如果是从背包栏中托过来的
            if (tag == "Grid")
            {
                //取出上一个槽的item数值引用
                BagItemInfo last_It = m_bagModel.BagItemsDic[parentId];
                //拷贝上一个槽的item数值
                BagItemInfo temp_It = new BagItemInfo(last_It);
                //挂载item到新的槽(其实是将上一个槽的数值赋给新的槽)
                MountPrefabToSlot(temp_It, dropSlotId);
                //清空上一个槽的数值
                m_bagModel.BagItemsDic.Remove(parentId);
            }
        }
        //public void DropToEmptySlot(Transform parentSlot,Transform item, GameObject dropSlot)
        //{
        //    string tag = parentSlot.tag;
        //    int parentId = parentSlot.GetComponent<BagGrIdIns>().id;
        //    int dropSlotId = dropSlot.GetComponent<BagGrIdIns>().id;
        //    //如果是从背包栏中托过来的
        //    if (tag == "Grid")
        //    {
        //        //取出上一个槽的item数值引用
        //        BagItem last_It = m_bagModel.Items[parentId];
        //        //拷贝上一个槽的item数值
        //        BagItem temp_It = new BagItem(last_It);
        //        //挂载item到新的槽(其实是将上一个槽的数值赋给新的槽)
        //        MountPrefabToSlot(temp_It, dropSlotId);
        //        //清空上一个槽的数值
        //        last_It.ClearItem();
        //    }
        //}
        public void ChangeItem(Transform parentSlot, Transform item, GameObject drop)
        {
            string tag = parentSlot.tag;
            int parentId = parentSlot.GetComponent<BagGrIdIns>().id;
            int dropSlotId = drop.transform.parent.GetComponent<BagGrIdIns>().id;
            //如果是从背包栏中托过来的
            if (tag == "Grid")
            {
                //拷贝上一个槽的item数值
                BagItemInfo last_It = new BagItemInfo(m_bagModel.BagItemsDic[parentId]);
                //拷贝这一个槽的item数值
                BagItemInfo this_It = new BagItemInfo(m_bagModel.BagItemsDic[dropSlotId]);
                //交换两个槽的item
                MountPrefabToSlot(last_It, dropSlotId);
                MountPrefabToSlot(this_It, parentId);
            }
        }
        //public void ChangeItem(Transform parentSlot, Transform item, GameObject drop)
        //{
        //    string tag = parentSlot.tag;
        //    int parentId = parentSlot.GetComponent<BagGrIdIns>().id;
        //    int dropSlotId = drop.transform.parent.GetComponent<BagGrIdIns>().id;
        //    //如果是从背包栏中托过来的
        //    if (tag == "Grid")
        //    {
        //        //拷贝上一个槽的item数值
        //        BagItem last_It = new BagItem(m_bagModel.Items[parentId]);
        //        //拷贝这一个槽的item数值
        //        BagItem this_It = new BagItem(m_bagModel.Items[dropSlotId]);
        //        //交换两个槽的item
        //        MountPrefabToSlot(last_It, dropSlotId);
        //        MountPrefabToSlot(this_It, parentId);
        //    }
        //}
        //获取某一个item的数据
        public BagItemInfo GetItemInfo(int id)
        {
            return m_bagModel.BagItemsDic[id];
        }
        //获取用于拖拽的sprite
        public GameObject GetDragSprite()
        {
            return m_bagView.DragSprite;
        }
        //将一个实例化后的物体挂载在指定的槽中
        public void MountPrefabToSlot(BagItemInfo it, int slotNum)
        {
            //更新bagItemCtl的id值
            it.ItemCtl.id = slotNum;
            //添加入itemDic中
            m_bagModel.BagItemsDic[slotNum] = it;
            //挂载item到空槽中
            it.Obj.transform.SetParent(m_bagView.m_slots[slotNum].slotObj.transform);
            it.Obj.transform.localPosition = Vector3.zero;
            //更新该槽的数据
            m_bagView.m_slots[slotNum].isEmpty = false;
            //m_bagModel.Items[slotNum].ChangeItem(it,slotNum);
            //it.viewItem.m_obj.transform.SetParent(m_bagView.m_slots[slotNum].slotObj.transform);
            //it.viewItem.m_obj.transform.localPosition = Vector3.zero;
        }
        ////其实是把数值清零
        //public void RemoveBagItem(int id)
        //{
        //    m_bagModel.Items[id].Count = 0;
        //    m_bagModel.Items[id].Id = -1;
        //    m_bagModel.Items[id].Type = ItemType.Nul;
        //    m_bagModel.Items[id].ImageName = "";
        //    m_bagModel.Items[id].viewItem = null;
        //}
        //更改界面item的表现
        public void ChangeItemNum(BagItemInfo it, int num)
        {
            Text tx = it.Obj.transform.GetChild(0).GetComponent<Text>();
            tx.text = num.ToString();
        }
        //public void ChangeItemNum(int key, int num)
        //{
        //    Text tx = m_bagModel.Items[key].viewItem.number_trans.GetComponent<Text>();
        //    tx.text = num.ToString();
        //}
    }
}

