using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSys;

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
                if(instance == null)
                {
                    instance = new AvatarInfoManager();
                }
                return instance;
            }
        }

        private float speed { get; set; }
        private int blood  { get; set; }
        private BattleValue battleValue { get; set; }
        private GoodsValue goodsValue { get; set; }
        private Transform Avatar { get; set; }
        private Transform Scene { get; set; }
        public AvatarInfoManager(Transform avatar,Transform scene)
        {
            battleValue = new BattleValue();
            goodsValue = new GoodsValue();
            speed = 1f;
            blood = 100;
            Avatar = avatar;
            Scene = scene;
            //完全依赖事件系统，不采取其他模式更改人物数值
            LiteEventManager.Instance.Register(AvatarValueKey.Attack, AttackValueChange);
            LiteEventManager.Instance.Register(AvatarValueKey.Blood, BloodValueChange);
            LiteEventManager.Instance.Register(AvatarValueKey.Goods, GoodsValueChange);
            LiteEventManager.Instance.Register(AvatarValueKey.Protect, ProtectValueChange);
            LiteEventManager.Instance.Register(AvatarValueKey.Speed, SpeedValueChange);
            LiteEventManager.Instance.Register(AvatarValueKey.Steady, SteadyValueChange);
        }

        public AvatarInfoManager()
        {
            battleValue = new BattleValue();
            goodsValue = new GoodsValue();
            speed = 1f;
            blood = 100;
            Scene = GameObject.Find("Scene").transform;
            Avatar = GameObject.Find("Avatar").transform;
            //完全依赖事件系统，不采取其他模式更改人物数值
            LiteEventManager.Instance.Register(AvatarValueKey.Attack, AttackValueChange);
            LiteEventManager.Instance.Register(AvatarValueKey.Blood, BloodValueChange);
            LiteEventManager.Instance.Register(AvatarValueKey.Goods, GoodsValueChange);
            LiteEventManager.Instance.Register(AvatarValueKey.Protect, ProtectValueChange);
            LiteEventManager.Instance.Register(AvatarValueKey.Speed, SpeedValueChange);
            LiteEventManager.Instance.Register(AvatarValueKey.Steady, SteadyValueChange);
        }
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
        /// 攻击力改变,以下几个函数类似
        /// </summary>
        /// <param name="obj"></param>
        public void AttackValueChange(object obj)
        {
            battleValue.attackValue += (int)obj;
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
            battleValue.protectValue += (int)obj;
        }

        public void SteadyValueChange(object obj)
        {
            battleValue.steadyValue += (int)obj;
        }

        public void GoodsValueChange(object obj)
        {
            goodsValue = (GoodsValue)obj;
        }
    }
    public class BattleValue
    {
        public int attackValue { get; set; }
        public int protectValue { get; set; }
        //枪口稳定系数
        public float steadyValue { get; set; }

        public BattleValue(int attack,int protect,float steady)
        {
            attackValue = attack;
            protectValue = protect;
            steadyValue = steady;
        }
        //初始值
        public BattleValue()
        {
            attackValue = 5;
            protectValue = 0;
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

