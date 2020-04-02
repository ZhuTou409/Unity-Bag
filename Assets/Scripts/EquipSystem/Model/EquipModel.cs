using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Base;
namespace Bag
{
    public class EquipModel
    {
        //当前主武器的槽
        public List<GunSlot> gunSlots { get; set; }
        //手枪槽
        public GunSlot hanGunSlot { get; set; }
        //手枪槽
        public GunSlot steelSlot { get; set; }
        //当前拥有的主武器，id为挂载的位置
        public Dictionary<int, GunItem> gunItems { get; set; }
        public EquipModel()
        {
            gunSlots = new List<GunSlot>();
            gunItems = new Dictionary<int, GunItem>();
        }
        //移除某个武器的某个装备
        public void RemoveEquip(GunItem gun, int equipKey)
        {
            gun.equipItems.Remove(equipKey);
        }
        //安全版
        public bool RemoveEquip(int gunId,int equipKey)
        {
            if (gunItems.TryGetValue(gunId,out GunItem it))
            {
                if(it.equipItems.TryGetValue(equipKey,out BagItemInfo itInfo))
                {
                    it.equipItems.Remove(equipKey);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 通过id获取某个主武器
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GunItem GetGunItem(int id)
        {
            if(gunItems.TryGetValue(id,out GunItem it))
            {
                return it;
            }
            return null;
        }

    }

    public class EquipSlot
    {
        public Transform slotTrans { get; set; }
        public Text tx { get; set; }
        public EquipType slotType { get; set; }
        public BagGrIdIns info;
        public EquipSlot(Transform slotTrans, int id ,int gunId,EquipType type)
        {
            this.slotTrans = slotTrans;
            this.slotType = type;
            info = slotTrans.GetComponent<BagGrIdIns>();
            info.id = id;
            info.parentId = gunId;
        }
        //拷贝构造函数
        public EquipSlot(EquipSlot slot)
        {
            this.slotTrans = slot.slotTrans;
            this.slotType = slot.slotType;
            this.info.id = slot.info.id;
            this.info.parentId = slot.info.parentId;
        }
    }


    public class GunItem:BaseItem
    {
        public Transform slotTrans { get; set; }
        public Text tx { get; set; }
        //数值
        public int hurtNum;
        public float steadyNum;
        public int capacity;
        //装备槽
        public Dictionary<int,EquipSlot> equipSlots;
        //装备
        public Dictionary<int, BagItemInfo> equipItems;
        //枪支的实例化物体(还未启用）
        public Transform gunTrans { get; set; }
        //构造函数
        public GunItem(Transform slotTrans, Dictionary<int, EquipSlot> equipSlot,string name, 
            int id, EquipType type, int hurtNum,float steadyNum,string prefabPath, int capacity):base(id,name,1,type,id,null,prefabPath)
        {
            this.slotTrans = slotTrans;
            this.hurtNum = hurtNum;
            this.steadyNum = steadyNum;
            this.equipSlots = equipSlot;
            this.capacity = capacity;
            equipItems = new Dictionary<int, BagItemInfo>();
        }
        public GunItem()
        {
            id = -1;
        }
        public GunItem(BaseItem it,Transform slotTrans, Dictionary<int, EquipSlot> equipSlot, 
            int hurtNum, float steadyNum, int capacity) :base(it)
        {
            this.slotTrans = slotTrans;
            this.hurtNum = hurtNum;
            this.steadyNum = steadyNum;
            this.equipSlots = equipSlot;
            this.capacity = capacity;
            equipItems = new Dictionary<int, BagItemInfo>();
        }
        //拷贝构造函数
        public GunItem(GunItem it)
        {
            this.slotTrans = it.slotTrans;
            this.id = it.id;
            this.type = it.type;
            this.hurtNum = it.hurtNum;
            this.steadyNum = it.steadyNum;
            this.name = it.name;
            this.ScenePrefabPath = it.ScenePrefabPath;
            this.capacity = it.capacity;
            //深拷贝
            equipSlots = new Dictionary<int, EquipSlot>();
            List<int> keyList = new List<int>();
            keyList.AddRange(it.equipSlots.Keys);
            for(int i = 0;i<keyList.Count;i++)
            {
                equipSlots.Add(keyList[i], it.equipSlots[keyList[i]]);
            }
            //两种不同的字典深拷贝方式
            equipItems = new Dictionary<int, BagItemInfo>();
            var enumerator = it.equipItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                equipItems.Add(enumerator.Current.Key, enumerator.Current.Value);
            }
        }
    }
    public class GunSlot
    {
        public Transform transform;
        public bool isEmpty;
        public EquipType gunType;
        public GunSlot(Transform trans, EquipType type)
        {
            this.transform = trans;
            isEmpty = true;
            gunType = type;
        }
    }

}
