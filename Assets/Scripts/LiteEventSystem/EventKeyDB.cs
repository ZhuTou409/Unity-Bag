using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base;

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
        GunValue,
        Protect,
        BulletsCount,
        Medicine
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
        public EquipType type;
        public PickInfo(int id, GameObject obj,EquipType T):base(id,obj)
        {
            type = T;
        }
        public PickInfo()
        {

        }
    }
    public class ValueInfo<T1, T2, T3, T4> : SendBaseClass
    {
        public T1 param_1;
        public T2 param_2;
        public T3 param_3;
        public T4 param_4;
        public ValueInfo(T1 par, T2 par2, T3 par3,T4 par4)
        {
            param_1 = par;
            param_2 = par2;
            param_3 = par3;
            param_4 = par4;
        }
        public ValueInfo()
        {

        }

    }
    //三参数
    public class ValueInfo<T1,T2,T3>:SendBaseClass
    {
        public T1 param_1;
        public T2 param_2;
        public T3 param_3;
        public ValueInfo(T1 par,T2 par2,T3 par3)
        {
            param_1 = par;
            param_2 = par2;
            param_3 = par3;
        }
        public ValueInfo()
        {

        }

    }
    //两参数
    public class ValueInfo<T1, T2> : SendBaseClass
    {
        public T1 param_1;
        public T2 param_2;
        public ValueInfo(T1 par, T2 par2)
        {
            param_1 = par;
            param_2 = par2;
        }
        public ValueInfo()
        {

        }
    }
}
