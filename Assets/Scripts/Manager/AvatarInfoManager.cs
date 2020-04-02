using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSys;
using Base;

namespace Avatar.Value
{
    public class AvatarInfoManager
    {

        //可通过单例模式获得人物数值

        private static AvatarInfoManager instance;
        public static AvatarInfoManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AvatarInfoManager();
                }
                return instance;
            }
        }

        private float speed { get; set; }
        private int blood { get; set; }
        //主武器列表
        private List<BattleValue> battleValue { get; set; }
        int MaxGun = 2;
        //各类型子弹数量
        private Dictionary<int, int> bulletCount { get; set; }
        //各类型急救包
        private Dictionary<int, int> medicineCount { get; set; }

        private GoodsValue goodsValue { get; set; }
        private Transform Avatar { get; set; }
        private Transform Scene { get; set; }
        public AvatarInfoManager()
        {
            Debug.Log("初始化avatarInfoManager");
            battleValue = new List<BattleValue>();
            goodsValue = new GoodsValue();
            bulletCount = new Dictionary<int, int>();
            //总共两种子弹
            bulletCount.Add(556, 0);
            bulletCount.Add(762, 0);
            //两种药品
            medicineCount = new Dictionary<int, int>();
            medicineCount.Add(0, 0);
            medicineCount.Add(1, 0);
            //初始化数值
            speed = 1f;
            blood = 100;
            Avatar = GameObject.Find("Avatar").transform;
            Scene = GameObject.Find("Scene").transform;
            //完全依赖事件系统，不采取其他模式更改人物数值
            LiteEventManager.Instance.Register(AvatarValueKey.GunValue, AttackValueChange);
            LiteEventManager.Instance.Register(AvatarValueKey.Blood, BloodValueChange);
           
            LiteEventManager.Instance.Register(AvatarValueKey.Protect, ProtectValueChange);
            LiteEventManager.Instance.Register(AvatarValueKey.Speed, SpeedValueChange);

            LiteEventManager.Instance.Register(EquipType.bullet_556, BulletValueChange);
            LiteEventManager.Instance.Register(EquipType.bullet_762, BulletValueChange);

            LiteEventManager.Instance.Register(EquipType.medicine_s, MedicineValueChange);
            LiteEventManager.Instance.Register(EquipType.medicine_k, MedicineValueChange);

            LiteEventManager.Instance.Register(EquipType.weapon_gun,AddGun);

        }

        //public AvatarInfoManager()
        //{
        //    battleValue = new List<BattleValue>();
        //    goodsValue = new GoodsValue();
        //    bulletCount = new Dictionary<int, int>();
        //    //总共三种子弹
        //    bulletCount.Add(556, 0);
        //    bulletCount.Add(762, 0);
        //    speed = 1f;
        //    blood = 100;
        //    Scene = GameObject.Find("Scene").transform;
        //    Avatar = GameObject.Find("Avatar").transform;
        //    //完全依赖事件系统，不采取其他模式更改人物数值
        //    LiteEventManager.Instance.Register(AvatarValueKey.GunValue, AttackValueChange);
        //    LiteEventManager.Instance.Register(AvatarValueKey.Blood, BloodValueChange);
        //    LiteEventManager.Instance.Register(AvatarValueKey.Medicine, MedicineValueChange);
        //    LiteEventManager.Instance.Register(AvatarValueKey.Protect, ProtectValueChange);
        //    LiteEventManager.Instance.Register(AvatarValueKey.Speed, SpeedValueChange);
        //    LiteEventManager.Instance.Register(AvatarValueKey.BulletsCount, BulletValueChange);
        //}
        /// <summary>
        /// 获取主角当前位置
        /// </summary>
        /// <returns></returns>
        public Transform GetAvatarTransform()
        {
            return Avatar;
        }
        /// <summary>
        /// 返回场景根节点
        /// </summary>
        /// <returns></returns>
        public Transform GetSceneTransform()
        {
            return Scene;
        }
        /// <summary>
        /// 返回某种类型的子弹，目前有三种类型：556,762
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetBulletCount(int index)
        {
            if(bulletCount.TryGetValue(index,out int it))
            {
                return it;
            }
            return -1;
        }
        /// <summary>
        /// 获取某种药品数量，目前两种类型，0，1
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetMedicingCount(int index)
        {
            if(medicineCount.TryGetValue(index,out int it))
            {
                return it;
            }
            return -1;
        }
        /// <summary>
        /// 获取某个枪支的攻击力
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetGunAttackVaalue(int index)
        {
            if(battleValue.Count>index)
            {
                return battleValue[index].attackValue;
            }
            return -1;
        }
        /// <summary>
        /// 获取某枪支的稳定性
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetGunSteadyValue(int index)
        {
            if (battleValue.Count > index)
            {
                return battleValue[index].steadyValue;
            }
            return -1;
        }
        /// <summary>
        /// 枪械数值改变,以下几个函数类似
        /// </summary>
        /// <param name="obj"></param>
        public void AttackValueChange(object obj)
        {
            ValueInfo<int, int,float,int> temp = new ValueInfo<int, int, float, int>();
            //规定传入的参数依次为为枪的id，伤害，稳定，弹匣容量
            temp = (ValueInfo<int, int, float, int>)obj;
            int index = temp.param_1;
            //伤害
            battleValue[index].attackValue += temp.param_2;
            //稳定
            battleValue[index].steadyValue += temp.param_3;
            //弹匣容量
            battleValue[index].capacity += temp.param_4;
            Debug.Log("攻击力：" + battleValue[index].attackValue + "稳定性：" + battleValue[index].steadyValue
                + "容量：" + battleValue[index].capacity);
        }

        public void BloodValueChange(object obj)
        {
            blood += (int)obj;
            if(blood <= 0)
            {
                //触发死亡
            }
        }

        public void SpeedValueChange(object obj)
        {
            speed += (float)obj;
        }

        public void ProtectValueChange(object obj)
        {

        }

        public void BulletValueChange(object obj)
        {
            object[] temp = (object[])obj;
            //对于子弹数量来说，不存在交换，只是单纯的增加与减少
            //子弹类型，子弹数量
            bulletCount[(int)temp[0]] += (int)temp[1];
        }

        public void MedicineValueChange(object obj)
        {
            object[] temp = (object[])obj;
            //对于子弹数量来说，不存在交换，只是单纯的增加与减少
            //子弹类型，子弹数量
            bulletCount[(int)temp[0]] += (int)temp[1];
        }
        /// <summary>
        /// 添加枪支
        /// </summary>
        /// <param name="obj"></param>
        public void AddGun(object obj)
        {
            Debug.Log("添加枪支");
            ValueInfo<int, int,float,int> temp = new ValueInfo<int, int,float,int>();
            temp = (ValueInfo<int, int, float, int>)obj;
            int index = temp.param_1;
            
            if (index > battleValue.Count && index <= MaxGun)
            {
                BattleValue bt = new BattleValue(temp.param_2, temp.param_3, temp.param_4);
                battleValue.Add(bt);
            }else if(index< battleValue.Count)
            {
                BattleValue bt = new BattleValue(temp.param_2, temp.param_3, temp.param_4);
                battleValue[index] = bt;
            }
            else if(index == battleValue.Count)
            {
                BattleValue bt = new BattleValue(temp.param_2, temp.param_3, temp.param_4);
                battleValue.Add(bt);
            }
            Debug.Log("攻击力：" + battleValue[index].attackValue
                + "稳定性：" + battleValue[index].steadyValue + "容量：" + battleValue[index].capacity);
        }
        public void Test(object obj)
        {
            ValueInfo<int, int, int, int> temp = new ValueInfo<int, int, int, int>();
            //规定传入的参数依次为为枪的id，伤害，稳定，弹匣容量
            temp = (ValueInfo<int, int, int, int>)obj;
            Debug.Log(temp.param_1);
        }
    }
    public class BattleValue
    {
        
        public int attackValue { get; set; }
        //弹匣容量
        public int capacity{ get; set; }
        //枪口稳定系数
        public float steadyValue { get; set; }

        public BattleValue(int attack,float steady,int capacity)
        {
            attackValue = attack;
            this.capacity = capacity;
            steadyValue = steady;
        }
        //初始值
        public BattleValue()
        {
            attackValue = 5;
            capacity = 0;
            steadyValue = 1;
        }
    }
    public class GoodsValue
    {
        public int bullet_556 { get; set; }
        public int bullet_762 { get; set; }
        public int bullet_9 { get; set; }
        public int firstAid { get; set; }
        public int drink { get; set; }

        public GoodsValue(int b_556,int b_762,int b_9,int aid,int drink)
        {
            bullet_556 = b_556;
            bullet_762 = b_762;
            bullet_9 = b_9;
            firstAid = aid;
            this.drink = drink;
        }
        public GoodsValue()
        {
            bullet_556 = 0;
            bullet_762 = 0;
            bullet_9 = 0;
            firstAid = 0;
            this.drink = 0;
        }
        //拷贝构造函数
        public GoodsValue(GoodsValue gv)
        {
            bullet_556 = gv.bullet_556;
            bullet_762 = gv.bullet_762;
            bullet_9 = gv.bullet_9;
            firstAid = gv.firstAid;
            this.drink = gv.drink;
        }
    }
}

