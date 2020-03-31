using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bag.Collect
{
    public class CollectModel
    {
        private Dictionary<int,EquipListItem> equipList;
        public CollectModel()
        {
            equipList = new Dictionary<int, EquipListItem>();
        }
        //插入item到列表
        public void AddItemToList(int key,EquipListItem it)
        {
            equipList.Add(key,it);
        }
        //从列表删除item以及消除item数据
        public void RemoveAt(int id,bool flag)
        {
            
            if(equipList.TryGetValue(id,out EquipListItem it))
            {
                if (flag == true)
                    it.DestroySceneObject();
                it.ClearItem();
                equipList.Remove(id);
            }
        }
        //仅从列表删除item
        public void RemoveFromList(int id)
        {
            if(equipList.TryGetValue(id, out EquipListItem it))
            {
                it.DestroySceneObject();
                equipList.Remove(id);
            }
        }
        //返回某列表中的某个Item
        public EquipListItem SelectItem(int index)
        {
            if (equipList.TryGetValue(index,out EquipListItem it))
            {
                return it;
            }
            else
            {
                Debug.Log("查找失败,index: "+index);
            }
            return null;
        }
        //返回列表长度
        public int ListCount()
        {
            return equipList.Count;
        }
    }
    public class EquipListItem:BaseItem
    {
        public GameObject obj;
        public Sprite sprite;
        //存储场景中的装备物体
        public GameObject SceneObj;
        public EquipListItem(int id, string name, int count, ItemType type,int gain,string discribe,GameObject obj,GameObject sceneObject,string prefabPath)
            :base(id,name,count,type,gain,discribe,prefabPath)
        {
            this.obj = obj;
            this.SceneObj = sceneObject;
        }
        public EquipListItem(BaseItem it,GameObject obj,GameObject sceneObj):base(it)
        {
            this.obj = obj;
            this.SceneObj = sceneObj;
        }
        public override void ClearItem()
        {
            base.ClearItem();
            GameObject.Destroy(obj);
            discribe = null;
        }
        public void DestroySceneObject()
        {
            GameObject.Destroy(SceneObj);
            SceneObj = null;
        }
    }
}
