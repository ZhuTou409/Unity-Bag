using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag;

namespace Bag.Collect
{
    public class CollectView
    {
        private Transform equipListView;
        private Transform collectView;
        public CollectView(GameObject canvas)
        {
            collectView = canvas.transform.Find("SceneEquipList");
            equipListView = collectView.Find("Viewport/List");
            HideCollectUI();
        }
        public void MountItem(Transform it)
        {
            //GameObject parentObj = ResManager.Instance.LoadPrefabFromRes("Prefab/BagGrid", true);
            //parentObj.transform.SetParent(equipListView, false);
            it.SetParent(equipListView, false);
            it.localPosition = Vector3.zero;
        }
        //直接删除，适用于主角原理该物品的时候
        public void UnfixItem(Transform it)
        {

        }
        //显示待拾取装备列表
        public void ShowCollectUI()
        {
            collectView.gameObject.SetActive(true);
        }
        //隐藏待拾取装备列表界面
        public void HideCollectUI()
        {
            collectView.gameObject.SetActive(false);
        }
        //改变列表大小
        public void ChangeListHeight(int itemCount)
        {
            int cellSizeY = 70;
            int spacingY = 3;
            equipListView.GetComponent<RectTransform>().sizeDelta = 
                new Vector2(0, itemCount * (cellSizeY + spacingY));
        }
    }
}
