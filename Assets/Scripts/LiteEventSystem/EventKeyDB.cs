using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventSys
{
    public enum CollectKey
    {
        Trig,
        Leave,
        Collect,
        RemoveToBag
    }
    //主角数值变化的key
    public enum AvatarValueKey
    {
        Blood,
        Speed,
        Attack,
        Protect,
        Steady,
        Goods
    }
    public class SendBaseClass
    {
        public int index;
        public GameObject Instance;
        public SendBaseClass(int id, GameObject obj)
        {
            index = id;
            Instance = obj;
        }
        public SendBaseClass()
        {

        }
    }
    public class PickInfo:SendBaseClass
    {
        public Bag.ItemType type;
        public PickInfo(int id, GameObject obj,Bag.ItemType T):base(id,obj)
        {
            type = T;
        }
        public PickInfo()
        {

        }
    }
}
