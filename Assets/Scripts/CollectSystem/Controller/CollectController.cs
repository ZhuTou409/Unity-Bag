using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventSys;

namespace Bag.Collect
{
    public class CollectController
    {
        public CollectView m_collectView;
        public CollectModel m_collectModel;
        public CollectController(GameObject canvas)
        {
            //注册监听
            LiteEventManager.Instance.Register(CollectKey.Trig, AddItem);
            LiteEventManager.Instance.Register(CollectKey.Leave, RemoveItem);
            LiteEventManager.Instance.Register(CollectKey.Collect, SelectItem);

            m_collectView = new CollectView(canvas);
            m_collectModel = new CollectModel();
        }
        //添加item
        void AddItem(object obj)
        {
            //根据key读取数据
            PickInfo recive = new PickInfo();
            recive = obj as PickInfo;
            BaseItem it = DataBaseManager.Instance.BagItemDic[recive.index];
            int count = m_collectModel.ListCount();
            if (count == 0)
            {
                m_collectView.ShowCollectUI();
            }
            if(AddItemToList(recive, it, "Prefab/BagItem"))
                m_collectView.ChangeListHeight(count+1);
        }
        //添加item到列表
        public bool AddItemToList(PickInfo send,BaseItem it,string prefabPath)
        {
            GameObject obj = ResManager.Instance.LoadPrefabFromRes(prefabPath,true);
            Sprite sprite = ResManager.Instance.LoadSpriteFromRes(it.imageName);

            //用gameobject的唯一标识符作为字典的索引

            int index = send.Instance.GetInstanceID();
            EquipListItem listItem = new EquipListItem(it, obj,send.Instance);

            m_collectView.ChangeListHeight(m_collectModel.ListCount());
            //初始化prefab的各个子物体
            Transform numberObj = obj.transform.GetChild(0);
            Text number = numberObj.GetComponent<Text>();
            number.text = it.count.ToString();

            Image img = obj.transform.GetChild(1).GetComponent<Image>();
            img.sprite = sprite;
            listItem.sprite = sprite;

            Text name = obj.transform.GetChild(2).GetComponent<Text>();
            name.text = it.name;

            Text discribe = obj.transform.GetChild(3).GetComponent<Text>();
            discribe.text = it.discribe;

            obj.transform.GetComponent<BagItemController>().SetDicKey(index);

            //初始化完成

            m_collectModel.AddItemToList(index,listItem);
            m_collectView.MountItem(listItem.obj.transform);
            
            return true;
        }
        //移除item
        public void RemoveItem(object index)
        {
            m_collectModel.RemoveAt((int)index,false);
            //判断隐藏界面
            int count = m_collectModel.ListCount();
            if (count == 0)
            {
                m_collectView.HideCollectUI();
            }
            m_collectView.ChangeListHeight(count);
        }
        //选择item
        public void SelectItem(object index)
        {
            int id = (int)index;
            //判断隐藏界面
            int count = m_collectModel.ListCount();

            m_collectView.ChangeListHeight(count);
            //Debug.Log("将要添加的item id:"+ index);
            //触发添加item到背包事件
            BaseItem it = new BaseItem(m_collectModel.SelectItem(id));
            if(it!=null)
            {
                LiteEventManager.Instance.TriggerEvent(CollectKey.RemoveToBag,it);
            }
            
            m_collectModel.RemoveAt(id,true);
            if (count-1 == 0)
            {
                m_collectView.HideCollectUI();
            }
            //删除场景内该物体

        }
        //返回某个item
        public EquipListItem SelectItem(int index)
        {
            return m_collectModel.SelectItem(index);
        }
    }
}