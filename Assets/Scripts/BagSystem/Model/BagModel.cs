using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag.Collect;
namespace Bag
{
    public class BagModel
    {
        //存放每个格子内的item数据
        public List<BagItem> Items;
        public Dictionary<int, BagItemInfo> BagItemsDic;
        //记录装备系统的槽
        public List<ViewSlot> EquipSlot;
        //当前背包物品数
        private int ItemCount
        {
            get { return ItemCount; }
            set { ItemCount = value; }
        }
        public BagModel()
        {
            Items = new List<BagItem>();

        }
        //创建每个格子相对应的数据块
        public void InitItemsData()
        {
            BagItemsDic = new Dictionary<int, BagItemInfo>();
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
        //从bagItemDic中彻底删除指定物品
        public bool RemoveItemFromDic(int key)
        {
            BagItemsDic.Remove(key);
            return true;
        }
    }
    public enum ItemType
    {
        //五个装备的顺序不要弄错，与界面的顺序是对应的
        equip_butt,
        equip_magazine,
        equip_handle,
        equip_muzzle,
        equip_scope,
        dress,bullet,
        medicine,
        weapon_gun,
        weapon_hangun,
        weapon_steel,
        Nul
    }
    public enum ItemSize { equipSize, bagSize }
    //item基础数据类
    public class BaseItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public ItemType type { get; set; }
        public int count { get; set; }
        public string imageName { get; set; }
        public string ScenePrefabPath { get; set; }
        public int gain;
        public string discribe;
        public BaseItem(int id, string name, int count,ItemType type,int gain,string discribe,string prefabPath)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.count = count;
            this.gain = gain;
            this.discribe = discribe;
            imageName = null;
            ScenePrefabPath = prefabPath;
        }
        public BaseItem(int id, string name, string imageName, int count,ItemType type, int gain,string discribe,string prefabPath)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.count = count;
            this.gain = gain;
            this.imageName = imageName;
            this.discribe = discribe;
            ScenePrefabPath = prefabPath;
        }
        //默认构造函数
        public BaseItem()
        {
            this.id = -1;
            this.count = 0;
        }
        public BaseItem(BaseItem it)
        {
            this.id = it.id;
            this.name = it.name;
            this.type = it.type;
            this.count = it.count;
            this.gain = it.gain;
            this.discribe = it.discribe;
            this.imageName = it.imageName;
            this.ScenePrefabPath = it.ScenePrefabPath;
        }
        public BaseItem(EquipListItem it)
        {
            this.id = it.id;
            this.name = it.name;
            this.type = it.type;
            this.count = it.count;
            this.gain = it.gain;
            this.discribe = it.discribe;
            this.imageName = it.imageName;
            this.ScenePrefabPath = it.ScenePrefabPath;
        }
        public virtual void ClearItem()
        {
            this.count = 0;
            this.id = -1;
            this.type = ItemType.Nul;
            this.gain = 0;
            this.discribe = null;
            this.ScenePrefabPath = null;
        }
    }
    public class BagItemInfo:BaseItem
    {
        public GameObject Obj { get; set; }
        public Sprite Image { get; set; }
        public BagItemController ItemCtl { get; set; }
        public BagItemInfo(int id, string name, Sprite sprite, int count, ItemType type, GameObject obj,string prefabPath):base(id,name,count,type,1,"", prefabPath)
        {
            this.Obj = obj;
            this.Image = sprite;
            ItemCtl = Obj.GetComponent<BagItemController>();
            if (ItemCtl == null)
                Debug.LogError("创建BagItemInfo失败，传入GameObject没有BagItemController组件");
        }
        //拷贝构造函数
        public BagItemInfo(BagItemInfo it):base(it)
        {
            this.Image = it.Image;
            this.Obj = it.Obj;
            this.ItemCtl = it.ItemCtl;
            
        }
        public BagItemInfo(EquipListItem it,BagController ctl)
        {
            GameObject.Destroy(it.obj);
            Obj = ResManager.Instance.LoadPrefabFromRes("Prefab/Bagitem", true);
            Image = it.sprite;
            ItemCtl = Obj.GetComponent<BagItemController>();
            ItemCtl.SetController(ctl);
            this.id = it.id;
            this.name = it.name;
            this.type = it.type;
            this.count = it.count;
            this.gain = it.gain;
            this.discribe = it.discribe;
        }
        public override void ClearItem()
        {
            base.ClearItem();
            this.Image = null;
            this.Obj = null;
            this.ItemCtl = null;
        }
    }
    //dataBase中存放装备的数据结构
    public class BagItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public int Count { get; set; }
        public ItemType Type {get;set; }
        //public ViewItem viewItem { get; set; }
        //构造函数
        public  BagItem(int Id, string Name,string ImagePath, int Count, ItemType type)
        {
            this.Id = Id;
            this.Name = Name;
            this.ImageName = ImagePath;
            this.Count = Count;
            this.Type = type;
            //this.viewItem = null;
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
            //this.viewItem = it.viewItem;
        }

        ////改变item的位置
        //public void SetItemTransform(ViewItem viewIt)
        //{
        //    viewItem = viewIt;
        //}
        ////将item数据清零
        //public void ClearItem()
        //{
        //    this.Count = 0;
        //    this.Id = -1;
        //    this.Type = ItemType.Nul;
        //    this.ImageName = null;
        //    this.viewItem = null;
        //}
        ////改变item的数据(通过传入另一个item)
        //public void ChangeItem(BagItem it, int slotId)
        //{
        //    if(it.Id == -1)
        //    {
        //        //传入的是空item
        //        Debug.LogError("传入空item！若想清空item，请使用clear()");
        //    }
        //    ChangeItem(it);
        //    viewItem.itemCtl.id = slotId;

        //}
        //private void ChangeItem(BagItem it)
        //{
        //    this.Count = it.Count;
        //    this.Id = it.Id;
        //    this.Type = it.Type;
        //    this.ImageName = it.ImageName;
        //    this.viewItem = it.viewItem;
        //}
        ////交换两个item的数据，并返回被交换后的it
        //public BagItem ExchangeItem(BagItem it)
        //{
        //    BagItem tempIt = new BagItem(it.Id, it.Name, it.ImageName, it.Count, it.Type);
        //    tempIt.SetItemTransform(it.viewItem);
        //    it.ChangeItem(this);
        //    this.ChangeItem(tempIt);
        //    return it;
        //}
    }

    
}

