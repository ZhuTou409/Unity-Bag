using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EventSys;
using Avatar.Value;
namespace Bag
{
    public class BagController 
    {
        private BagView m_bagView;
        public BagModel m_bagModel;
        public EquipModel m_equipModel;
        //private EquipView m_equipView;

        private int m_initBagItemNum;
        private int m_maxBagItemNum;

        public BagController()
        {
            //初始化五个格子
            m_initBagItemNum = 10;
            //最多10个格子
            m_maxBagItemNum = 10;
            //实例化view和model
            m_bagView = new BagView();
            m_bagModel = new BagModel();
            m_equipModel = new EquipModel();
            //m_equipView = new EquipView(m_bagView.m_bagList.transform.parent.GetChild(1));
            //初始化背包格子和数据
            InitBag(m_initBagItemNum, "Prefab/BagGrid");
            //初始化武器格子
            InitGunSlots(m_bagView.m_bag.transform);
            //添加事件监听
            LiteEventManager.Instance.Register(CollectKey.RemoveToBag, AddItemFromColect);

            UIEventManager.Get(m_bagView.closeBtn).OnClickCallBack += CloseView;
        }

        public void Update()
        {
            //打开背包
            if(Input.GetKeyDown(KeyCode.B))
            {
                if(m_bagView.BagUIActive() == false)
                {
                    OpenView(null);
                }else
                {
                    CloseView(null);
                }
            }
        }
        //初始化背包格子和数据
        private void InitBag(int BagItemNum,string BagGridPath)
        {
            m_bagView.InitBagViewSlots(BagItemNum, BagGridPath);
            m_bagModel.InitItemsData();
        }
        //初始化装备系统的槽slot
        private void InitGunSlots(Transform bag)
        {
            //获得weaponList
            Transform weaponList = bag.GetChild(1);
            int i = 0;
            for (i = 0; i < weaponList.childCount - 2; i++)
            {
                GunSlot slot = new GunSlot(weaponList.GetChild(i), ItemType.weapon_gun);
                m_equipModel.gunSlots.Add(slot);
            }
            //初始化手枪槽和冷兵器槽
            m_equipModel.hanGunSlot =  new GunSlot(weaponList.GetChild(i), ItemType.weapon_hangun);
            m_equipModel.steelSlot = new GunSlot(weaponList.GetChild(i + 1), ItemType.weapon_steel);
        }
        //添加装备，主武器
        public bool AddGun(DataBaseManager.GunDBItem it)
        {
            //查看主武器槽的挂载情况
            int firstEmptySlot = -1;
            int slotCount = m_equipModel.gunSlots.Count;
            for (int i = 0;i< slotCount; i++)
            {
                if(m_equipModel.gunSlots[i].isEmpty == true)
                {
                    firstEmptySlot = i;
                    break;
                }
            }
           
            //有空位
            if(firstEmptySlot != -1)
            {
                //初始化item并挂载
                InitGunItem(it, firstEmptySlot);
            }
            //没有空位,此时可以确定每个槽都挂载了item
            else
            {
                //最后一个武器出队
                Debug.Log("没有空位了");
                GunItem lastItem = m_equipModel.gunItems[slotCount - 1];
                //将物体放回场景中
                Debug.Log(lastItem.id);
                Vector3 pos = AvatarInfoManager.Instance.GetAvatarTransform().position;
                ResManager.Instance.CreateGameObject(lastItem.ScenePrefabPath, 
                    new Vector3(pos.x, -0.8f, pos.z), new PickInfo(lastItem.id, null, lastItem.type));
                //被丢弃的枪支内的装备放回背包中
                var equipList = lastItem.equipItems.GetEnumerator();
                while(equipList.MoveNext())
                {
                    Debug.Log("equipItems Keys:" + equipList.Current.Key);
                    EquipItemToBagItem(lastItem, equipList.Current.Key,false);
                }
                //foreach(var key in lastItem.equipItems.Keys)
                //{
                //    Debug.Log("equipItems Keys:" + key);
                //    EquipItemToBagItem(lastItem, key);
                //}
                //删除物体
                GameObject.Destroy(lastItem.slotTrans.gameObject);
                //从字典中移除
                m_equipModel.gunItems.Remove(slotCount);

                for (int i = slotCount - 2; i >= 0; i--)
                {
                    GunItem item = new GunItem(m_equipModel.gunItems[i]);
                    //Debug.Log(i+"  "+item.name);
                    m_equipModel.gunItems[i+1] = item;
                    //更新枪支装备槽的parentId,parentId记录字典key值
                    var enumerator = item.equipSlots.GetEnumerator();
                    while(enumerator.MoveNext())
                    {
                        enumerator.Current.Value.info.parentId = i + 1;
                    }
                    //重新挂载
                    item.slotTrans.SetParent(m_equipModel.gunSlots[i + 1].transform,false);
                    item.slotTrans.localPosition = Vector3.zero;
                }
                //将新加入的item挂载在第一个槽上
                InitGunItem(it, 0);
            }
            //测试
            //var enumerator = m_equipModel.gunItems.GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    Debug.Log("第" + enumerator.Current.Key.ToString() + "个item：" + m_equipModel.gunItems[enumerator.Current.Key].name);
            //}
            return true;
        }

        //根据传入it的信息，初始化一个gunItem，slotNum为挂载在第几个槽
        private void InitGunItem(DataBaseManager.GunDBItem it,int slotNum)
        {
            //实例化gunItem
            GameObject obj = ResManager.Instance.LoadPrefabFromRes("Prefab/GunItem", true);
            Text tx = obj.transform.GetChild(0).GetComponent<Text>();
            tx.text = it.name;
            //加载枪械图片
            Sprite sprite = ResManager.Instance.LoadSpriteFromRes(it.imagePath);
            Image img = obj.transform.GetChild(1).GetComponent<Image>();
            img.sprite = sprite;

            Transform partList = obj.transform.GetChild(2);
            //初始化item对应的装备槽数据
            List<ItemType> type = it.equipList;
            int equipListCount = type.Count;
            //装备槽位对照表
            List<ItemType> allEquipType = new List<ItemType>() {ItemType.equip_butt,ItemType.equip_magazine,
                    ItemType.equip_handle,ItemType.equip_muzzle,ItemType.equip_scope};

            Dictionary<int, EquipSlot> equipSlotDic = new Dictionary<int, EquipSlot>();
            //不同的枪所能挂载的配件不同,能显示的装备槽数量也不同
            for (int i = 0; i < 5; i++)
            {
                Transform part = partList.GetChild(i);
                //给每个槽的parentId赋值
                part.GetComponent<BagGrIdIns>().parentId = slotNum;
                for (int j = 0; j < equipListCount; j++)
                {
                    if (type[j] == allEquipType[i])
                    {
                        //将这个槽显示出来
                        part.gameObject.SetActive(true);
                        //加入装备槽字典
                        equipSlotDic.Add(i, new EquipSlot(part, i, slotNum, type[j]));
                    }
                }
            }
            //正式挂载
            //物理挂载
            obj.transform.SetParent(m_equipModel.gunSlots[slotNum].transform,false);
            obj.transform.localPosition = Vector3.zero;
            //数据挂载
            GunItem item = new GunItem(obj.transform, equipSlotDic, it.name,it.id, it.weaponType, it.hurtNum, it.steadyNum, it.prefabPath);
            Debug.Log("prefabPath:" + item.ScenePrefabPath);
            if (m_equipModel.gunItems.TryGetValue(slotNum, out GunItem element))
                m_equipModel.gunItems[slotNum] = item;
            else
                m_equipModel.gunItems.Add(slotNum, item);
            //挂载完成，将该槽设置为已挂载
            m_equipModel.gunSlots[slotNum].isEmpty = false;
        }

        //添加item
        public bool AddItem(BaseItem it)
        {
            int id = it.id;
            int itemNum = m_bagModel.BagItemsDic.Count;
            int firstEmptySlot = -1;
            //背包中已经有这个id的物品
            var enumerator = m_bagModel.BagItemsDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (m_bagModel.BagItemsDic[enumerator.Current.Key].id == id &&
                    (it.type == ItemType.bullet || it.type ==ItemType.medicine))
                {
                    //只有药品和子弹的数量可以叠加
                    if (it.type == ItemType.bullet || it.type == ItemType.medicine)
                    {
                        enumerator.Current.Value.count += it.count;
                        ChangeItemNum(enumerator.Current.Value, enumerator.Current.Value.count);
                        return true;
                    }
                }
            }
            //背包中没有这个物品，遍历背包槽，找到第一个空槽
            for (int i = 0; i < m_bagView.m_slots.Count; i++)
            {
                //Debug.Log(i + " : " + m_bagView.m_slots[i].isEmpty);
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
                Sprite sprite = ResManager.Instance.LoadSpriteFromRes(it.imageName);
                //初始化item,加载名字，数量，图片
                Transform numberObj = obj.transform.GetChild(0);
                Text number = numberObj.GetComponent<Text>();
                number.text = it.count.ToString();

                Image img = obj.transform.GetChild(1).GetComponent<Image>();
                img.sprite = sprite;

                Text name = obj.transform.GetChild(2).GetComponent<Text>();
                name.text = it.name;
                //赋值id，传递bagController
                BagItemController itemView = obj.GetComponent<BagItemController>();
                itemView.SetController(this);
                itemView.SetDicKey(firstEmptySlot);
                //实例化一个BagItemInfo
                BagItemInfo itInfo = new BagItemInfo(id, it.name, sprite, it.count, it.type, obj,it.ScenePrefabPath);
                //挂载item到空槽中
                MountPrefabToSlot(itInfo, firstEmptySlot);
            }
            return true;
        }
        //从装备栏拖入背包统一使用此函数
        private bool AddItem(BagItemInfo itInfo,int slotNum)
        {
            int id = itInfo.id;
            int itemNum = m_bagModel.BagItemsDic.Count;
            int firstEmptySlot = -1;
            //背包中已经有这个id的物品
            var enumerator = m_bagModel.BagItemsDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (m_bagModel.BagItemsDic[enumerator.Current.Key].id == id &&
                    (itInfo.type == ItemType.bullet || itInfo.type == ItemType.medicine))
                {
                    //只有药品和子弹的数量可以叠加
                    if (itInfo.type == ItemType.bullet || itInfo.type == ItemType.medicine)
                    {
                        enumerator.Current.Value.count += itInfo.count;
                        ChangeItemNum(enumerator.Current.Value, enumerator.Current.Value.count);
                        return true;
                    }
                }
            }
            //背包中没有这个物品，遍历背包槽，找到第一个空槽
            for (int i = 0; i < m_bagView.m_slots.Count; i++)
            {
                if (m_bagView.m_slots[i].isEmpty == true)
                {
                    firstEmptySlot = i;
                    break;
                }
            }
            //背包中没有这个物品，将该物体挂载在槽上
            MountPrefabToSlot(itInfo, firstEmptySlot);
            Debug.Log("挂载完成");
            //改变item形态
            ChangeItemSize(itInfo, ItemSize.bagSize);
            return true;
        }

        //从待拾取装备列表添加装备到背包中
        void AddItemFromColect(object equipItem)
        {
            //将equiipItem类型转变为bagItemInfo

            Debug.Log("开始挂载");
            BaseItem it = (BaseItem)equipItem;
            if(it.type == ItemType.weapon_gun || it.type == ItemType.weapon_steel)
            {
                DataBaseManager.GunDBItem gunItem = DataBaseManager.Instance.GunItemDic[it.id];
                Debug.Log(gunItem.prefabPath);
                AddGun(gunItem);
            }
            else
            {
                AddItem((BaseItem)equipItem);
            }
        }
        //处理物品拖拽
        public void HanderDropCtl(Transform parentSlot, Transform item,GameObject obj,int dicKey)
        {
            string tag = obj.tag;
            //拖入背包槽
            if(tag == "Grid")
            {
                Debug.Log("拖入背包槽");
                DropToEmptySlot(parentSlot, item, obj, dicKey);
            }
            //槽内有item
            else if(tag == "BagItem")
            {
                ChangeItem(parentSlot, item, obj, dicKey);
                Debug.Log("交换");

            }
            //丢弃物品
            else if (tag == "Package")
            {
                Debug.Log("丢弃");
                AbandonEquip(parentSlot, item, dicKey);

            }
            //拖入装备槽
            else if (tag == "EquipSlot")
            {
                DropToEquipSlot(parentSlot, item, obj, dicKey);
                Debug.Log("拖入装备槽");

            }
        }
        //丢弃装备
        public bool AbandonEquip(Transform parentSlot, Transform item,int dicKey)
        {
            string tag = parentSlot.tag;
            if (tag == "Grid")
            {
                if (m_bagModel.BagItemsDic.TryGetValue(dicKey, out BagItemInfo it))
                {
                    GameObject.Destroy(it.Obj);
                    //加载prefab到地图中
                    //GameObject obj = ResManager.Instance.LoadPrefabFromRes(it.ScenePrefabPath, true);
                    //obj.transform.position = new Vector3(1, 0, 1);
                    ////设置prefab的基本属性
                    //EquipInfo info = obj.GetComponent<EquipInfo>();
                    //info.equipId = it.id;
                    ////正式从背包中清除装备
                    //it.ClearItem();
                    //m_bagModel.RemoveItemFromDic(dicKey);
                    Vector3 pos = AvatarInfoManager.Instance.GetAvatarTransform().position;
                    ResManager.Instance.CreateGameObject(it.ScenePrefabPath,
                        new Vector3(pos.x, -0.8f, pos.z), new PickInfo(it.id, null, it.type));
                    //更新槽的flag
                    m_bagView.m_slots[dicKey].isEmpty = true;
                    //整理内存
                    ResManager.Instance.ClearMemory();
                }
            }
            else if (tag == "EquipSlot")
            {
                int gunId = parentSlot.GetComponent<BagGrIdIns>().parentId;
                if (m_equipModel.gunItems.TryGetValue(gunId, out GunItem gunIt))
                {
                    if (gunIt.equipItems.TryGetValue(dicKey, out BagItemInfo it))
                    {
                        GameObject.Destroy(it.Obj);
                        //加载prefab到地图中
                        //GameObject obj = ResManager.Instance.LoadPrefabFromRes(it.ScenePrefabPath, true);
                        //obj.transform.position = new Vector3(1, 1, 1.4f);
                        ////设置prefab的基本属性
                        //EquipInfo info = obj.GetComponent<EquipInfo>();
                        //info.equipId = it.id;
                        Vector3 pos = AvatarInfoManager.Instance.GetAvatarTransform().position;
                        Debug.Log(pos);
                        ResManager.Instance.CreateGameObject(it.ScenePrefabPath,
                            new Vector3(-0.8f, pos.y, pos.z), new PickInfo(it.id,null,it.type));
                        //正式从背包中清除装备
                        it.ClearItem();
                        m_equipModel.RemoveEquip(gunIt, dicKey);
                        ResManager.Instance.ClearMemory();
                    }
                }
            }
            else
                return false;

            return true;
        }

        //从背包、另一把枪的装备栏 拖去装备栏
        public bool DropToEquipSlot(Transform parentSlot, Transform item, GameObject dropSlot,int dicKey)
        {
            string tag = parentSlot.tag;
            int parentId = parentSlot.GetComponent<BagGrIdIns>().id;
            int dropSlotId = dropSlot.GetComponent<BagGrIdIns>().id;
            //得到该装备槽对应的枪
            int gunId = dropSlot.GetComponent<BagGrIdIns>().parentId;
            Debug.Log("parentId: " + parentId);
            //如果是从背包栏中托过来的
            if (tag == "Grid")
            {
                //取出上一个槽的item数值引用
                BagItemInfo last_It = m_bagModel.BagItemsDic[dicKey];
                //拷贝上一个槽的item数值
                BagItemInfo temp_It = new BagItemInfo(last_It);
                //挂载item到新的槽(其实是将上一个槽的数值赋给新的槽)
                if (m_equipModel.gunItems.TryGetValue(gunId, out GunItem gun))
                {
                    //确认拖入的装备类型和槽是否对应
                    if(gun.equipSlots[dropSlotId].slotType!= temp_It.type)
                    {
                        Debug.Log("类型不对应: " + gun.equipSlots[dropSlotId].slotType + temp_It.type);
                        return false;
                    }
                    //将item加入该枪的装备字典中
                    gun.equipItems.Add(dropSlotId, temp_It);
                    temp_It.ItemCtl.DicKey = dropSlotId;
                    //改变位置,物理挂载
                    temp_It.Obj.transform.SetParent(gun.equipSlots[dropSlotId].slotTrans,false);
                    temp_It.Obj.transform.localPosition = Vector3.zero;
                    ChangeItemSize(temp_It, ItemSize.equipSize);
                    //清空上一个槽的数值
                    m_bagModel.BagItemsDic.Remove(dicKey);
                    m_bagView.m_slots[dicKey].isEmpty = true;
                    Debug.Log("完成");
                }
                else
                {
                    Debug.Log("没找到枪");
                }
            }
            else if (tag == "EquipSlot")
            {
                int slotGunId = parentSlot.GetComponent<BagGrIdIns>().parentId;
                //获取
                if (m_equipModel.gunItems.TryGetValue(slotGunId, out GunItem gun))
                {
                    //拷贝上一个槽的item数值
                    BagItemInfo lastItem = new BagItemInfo(gun.equipItems[dicKey]);
                    //确认拖入的装备类型和槽是否对应
                    if (gun.equipSlots[dropSlotId].slotType != lastItem.type)
                    {
                        Debug.Log("类型不对应: " + gun.equipSlots[dropSlotId].slotType + lastItem.type);
                        return false;
                    }
                    //获取dropSlot有关信息
                    int dropGunId = dropSlot.GetComponent<BagGrIdIns>().parentId;
                    GunItem dropGun = m_equipModel.gunItems[dropGunId];
                    //挂载
                    lastItem.Obj.transform.SetParent(dropGun.equipSlots[dropSlotId].slotTrans,false);
                    lastItem.Obj.transform.localPosition = Vector3.zero;
                    dropGun.equipItems.Add(dropSlotId, lastItem);
                    lastItem.ItemCtl.DicKey = dropSlotId;
                    //清空上一个槽的数值
                    gun.equipItems.Remove(parentId);
                }
            }
            return true;
        }

        //从背包拖去背包，从装备栏拖去背包
        public void DropToEmptySlot(Transform parentSlot, Transform item, GameObject dropSlot,int itemId)
        {
            string tag = parentSlot.tag;
            int parentId = parentSlot.GetComponent<BagGrIdIns>().id;
            int dropSlotId = dropSlot.GetComponent<BagGrIdIns>().id;
            //如果是从背包栏中托过来的
            if (tag == "Grid")
            {
                //取出上一个槽的item数值引用
                BagItemInfo last_It = m_bagModel.BagItemsDic[itemId];
                //拷贝上一个槽的item数值
                //BagItemInfo temp_It = new BagItemInfo(last_It);
                //Debug.Log("temp_It的id："+temp_It.id);
                //挂载item到新的槽(其实是将上一个槽的数值赋给新的槽)
                MountPrefabToSlot(last_It, dropSlotId);
                //清空上一个槽的数值
                m_bagModel.BagItemsDic.Remove(parentId);
                m_bagView.m_slots[parentId].isEmpty = true;
            }
            //如果是从装备栏拖过去的
            else if(tag == "EquipSlot")
            {
                //得到该装备槽对应的枪
                int gunId = parentSlot.GetComponent<BagGrIdIns>().parentId;
                //获取
                if(m_equipModel.gunItems.TryGetValue(gunId,out GunItem gun))
                {
                    EquipItemToBagItem(gun, itemId);
                    //拷贝上一个槽的item数值
                    //BagItemInfo tempItem = new BagItemInfo(gun.equipItems[itemId]);
                    //挂载item到新的槽(其实是将上一个槽的数值赋给新的槽)
                    //AddItem(tempItem, dropSlotId);
                    //清空上一个槽的数值
                    //m_equipModel.RemoveEquip(gun, itemId);
                }
            }
        }

        /// <summary>
        /// 从装备栏挂载到背包栏
        /// </summary>
        /// <param name="gun"></param>
        /// <param name="itemId">装备槽的id</param>
        private void EquipItemToBagItem(GunItem gun,int itemId,bool flag = true)
        {
            //拷贝上一个槽的item数值
            BagItemInfo tempItem = new BagItemInfo(gun.equipItems[itemId]);
            //挂载item到新的槽(其实是将上一个槽的数值赋给新的槽)
            AddItem(tempItem, 0);
            //清空上一个槽的数值
            if(flag == true)
                m_equipModel.RemoveEquip(gun, itemId);
        }

        //交换item
        public void ChangeItem(Transform parentSlot, Transform item, GameObject drop,int itemId)
        {
            string tag = parentSlot.tag;
            int parentSlotId = parentSlot.GetComponent<BagGrIdIns>().id;
            int dropSlotId = drop.transform.parent.GetComponent<BagGrIdIns>().id;
            //获取交换对象的id
            int dropId = drop.transform.GetComponent<BagItemController>().DicKey;
            //拷贝这一个槽的item数值
            BagItemInfo aim_It = new BagItemInfo(m_bagModel.BagItemsDic[dropId]);
            //如果是从背包栏中拖拽过来的
            if (tag == "Grid")
            {
                //拷贝上一个槽的item数值
                BagItemInfo drop_It = new BagItemInfo(m_bagModel.BagItemsDic[itemId]);
                //直接交换两个槽的item
                MountPrefabToSlot(drop_It, dropSlotId);
                MountPrefabToSlot(aim_It, parentSlotId);
            }
            //从装备栏中拖过来的,不进行交换，直接添加入背包
            else if (tag == "EquipSlot")
            {
                int gunId = parentSlot.GetComponent<BagGrIdIns>().parentId;
                //拷贝上一个槽的item数值
                BagItemInfo drop_It = new BagItemInfo(m_equipModel.gunItems[gunId].equipItems[itemId]);
                //背包添加此item
                AddItem(drop_It, -1);
                //删除装备槽的item
                bool res = m_equipModel.RemoveEquip(gunId, itemId);
                Debug.Log(res);
            }
        }

        //获取某一个item的数据，返回baginteminfo类
        public BagItemInfo GetItemInfo(Transform parent,int dicKey)
        {

            if (parent.tag == "EquipSlot")
            {
                int gunId = parent.GetComponent<BagGrIdIns>().parentId;
                return m_equipModel.gunItems[gunId].equipItems[dicKey];
            }
            else if (parent.tag == "Grid")
            {
                if (m_bagModel.BagItemsDic.TryGetValue(dicKey, out BagItemInfo info))
                {
                    return m_bagModel.BagItemsDic[dicKey];
                }
                else
                    Debug.Log("查找失败");

            }
            return null;
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
            it.ItemCtl.DicKey = slotNum;
            //添加入itemDic中
            if (m_bagModel.BagItemsDic.TryGetValue(slotNum, out BagItemInfo gun))
                m_bagModel.BagItemsDic[slotNum] = it;
            else
                m_bagModel.BagItemsDic.Add(slotNum, it);
            //挂载item到空槽中
            it.Obj.transform.SetParent(m_bagView.m_slots[slotNum].slotObj.transform,false);

            it.Obj.transform.localPosition = Vector3.zero;
            //更新该槽的数据
            m_bagView.SetSlotEmptyOrNot(slotNum, false);
        }

        //更改界面item的表现
        public void ChangeItemNum(BagItemInfo it, int num)
        {
            Text tx = it.Obj.transform.GetChild(0).GetComponent<Text>();
            tx.text = num.ToString();
        }

        //更改item的形态
        private void ChangeItemSize(BagItemInfo itemInfo, ItemSize size)
        {
            if(size == ItemSize.bagSize)
            {
                //转为背包形态
                //itemInfo.Obj.transform.localScale = new Vector3(1, 1, 1);
                itemInfo.Obj.GetComponent<Image>().sprite = null;
                //将子物体设置为不可见
                for (int i = 0; i < itemInfo.Obj.transform.childCount; i++)
                {
                    itemInfo.Obj.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            else if(size == ItemSize.equipSize)
            {
                //转为装备形态
                //itemInfo.Obj.transform.localScale = new Vector3(0.28f, 0.64f, 1);
                itemInfo.Obj.GetComponent<Image>().sprite = itemInfo.Image;
                //将子物体设置为不可见
                for (int i = 0; i < itemInfo.Obj.transform.childCount; i++)
                {
                    itemInfo.Obj.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        public void CloseView(PointerEventData data)
        {
            m_bagView.HideBagUI();
        }
        public void OpenView(PointerEventData data)
        {
            m_bagView.ShowBagUI();
        }
    }
}

