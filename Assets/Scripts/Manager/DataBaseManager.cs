using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager Instance;
    //针对BagItem的字典结构，一次性将装备，时装加载入内存中，保证读取速度
    public Dictionary<int, Bag.BagItem> BagItemDic;
    private void InitBagItemDic()
    {
        //待完善
        //ResManager.Instance.LoadJasonFile("xxx");
        //暂时用直接定义代替
        BagItemDic = new Dictionary<int, Bag.BagItem>();
        Bag.BagItem it_1 = new Bag.BagItem(0, "5.56毫米子弹", "Sprit/bullet-5.56mm", 30,Bag.ItemType.bullet);
        this.BagItemDic.Add(0, it_1);
        Bag.BagItem it_2 = new Bag.BagItem(1, "垂直握把", "Sprit/chuizhiwoba", 1,Bag.ItemType.equip);
        this.BagItemDic.Add(1, it_2);
    }
    private void Awake()
    {
        InitBagItemDic();
        Instance = this;
    }

}
