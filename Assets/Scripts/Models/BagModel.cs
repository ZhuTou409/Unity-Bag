using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bag
{
    public class BagModel
    {
        //存放每个格子内的item数据
        public List<BagItem> Items;
        //记录装备系统的槽
        public List<ViewSlot> EquipSlot;
        //当前背包物品数
        private int ItemCount
        {
            get { return ItemCount; }
            set { ItemCount = value; }
        }
        //背包槽个数，只在第一次初始化时赋值，故不添加get set
        private int BagSlotNum;
        public BagModel()
        {
            Items = new List<BagItem>();

        }
        //创建每个格子相对应的数据块
        public void InitItemsData(int ItemNumber)
        {
            this.BagSlotNum = ItemNumber;
            for (int i = 0; i < BagSlotNum; i++)
            {
                Items.Add(new BagItem());
            }
        }
        //寻找某一个id对应的item
        public BagItem FindItemById(int id)
        {
            for(int i = 0;i<Items.Count; i++ )
            {
                if(id == Items[i].Id)
                {
                    return Items[i];
                }
            }
            return null;
        }
    }
    public enum ItemType
    {
        medicine,weapon,equip,dress,bullet,Nul
    }
    public class BagItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public int Count { get; set; }
        public ItemType Type {get;set; }
        public ViewItem viewItem { get; set; }
        //构造函数
        public  BagItem(int Id, string Name,string ImagePath, int Count, ItemType type)
        {
            this.Id = Id;
            this.Name = Name;
            this.ImageName = ImagePath;
            this.Count = Count;
            this.Type = type;
            this.viewItem = null;
        }
        //默认构造函数
        public BagItem()
        {
            this.Id = -1;
            this.Count = 0;
        }
        //拷贝构造函数
        public BagItem(BagItem it)
        {
            this.Count = it.Count;
            this.Id = it.Id;
            this.Type = it.Type;
            this.ImageName = it.ImageName;
            this.viewItem = it.viewItem;
        }
        //改变item的位置
        public void SetItemTransform(ViewItem viewIt)
        {
            viewItem = viewIt;
        }
        //将item数据清零
        public void ClearItem()
        {
            this.Count = 0;
            this.Id = -1;
            this.Type = ItemType.Nul;
            this.ImageName = null;
            this.viewItem = null;
        }
        //改变item的数据(通过传入另一个item)
        public void ChangeItem(BagItem it, int slotId)
        {
            if(it.Id == -1)
            {
                //传入的是空item
                Debug.LogError("传入空item！若想清空item，请使用clear()");
            }
            ChangeItem(it);
            viewItem.itemCtl.id = slotId;

        }
        private void ChangeItem(BagItem it)
        {
            this.Count = it.Count;
            this.Id = it.Id;
            this.Type = it.Type;
            this.ImageName = it.ImageName;
            this.viewItem = it.viewItem;
        }
        //交换两个item的数据，并返回被交换后的it
        public BagItem ExchangeItem(BagItem it)
        {
            BagItem tempIt = new BagItem(it.Id, it.Name, it.ImageName, it.Count, it.Type);
            tempIt.SetItemTransform(it.viewItem);
            it.ChangeItem(this);
            this.ChangeItem(tempIt);
            return it;
        }
    }

    
}

