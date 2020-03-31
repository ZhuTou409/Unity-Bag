using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager Instance;
    //针对BagItem的字典结构，一次性将装备，时装加载入内存中，保证读取速度
    public Dictionary<int, Bag.BaseItem> BagItemDic;
    //存放枪械数据的字典
    public Dictionary<int, GunDBItem> GunItemDic;
    private void InitBagItemDic()
    {
        //待完善
        //ResManager.Instance.LoadJasonFile("xxx");
        //暂时用直接定义代替
        BagItemDic = new Dictionary<int, Bag.BaseItem>();
        Bag.BaseItem it_1 = new Bag.BaseItem(0, "5.56毫米子弹", "Sprit/bullet-5.56mm", 30,Bag.ItemType.bullet,4,"适合5.56mm口径自动步枪","Prefab/Model/bulletModel-5.56mm");
        this.BagItemDic.Add(0, it_1);
        Bag.BaseItem it_2 = new Bag.BaseItem(1, "垂直握把", "Sprit/chuizhiwoba", 1,Bag.ItemType.equip_handle, 5, "增加枪支稳定性", "Prefab/Model/chuizhiwobaModel");
        this.BagItemDic.Add(1, it_2);
        Bag.BaseItem it_3 = new Bag.BaseItem(2, "直角握把", "Sprit/zhijiaowoba", 1, Bag.ItemType.equip_handle, 6, "增加枪支稳定性", "Prefab/Model/zhijiaowobaModel");
        this.BagItemDic.Add(2, it_3);
        Bag.BaseItem it_4 = new Bag.BaseItem(3, "饮料", "Sprit/kele", 1, Bag.ItemType.medicine, 10, "少量回复生命", "Prefab/Model/keleModel");
        this.BagItemDic.Add(3, it_4);
        Bag.BaseItem it_5 = new Bag.BaseItem(4, "2倍 瞄准镜", "Sprit/2-beijing", 1, Bag.ItemType.equip_scope, 2, "放大两倍", "Prefab/Model/2-beijingModel");
        this.BagItemDic.Add(4, it_5);
        Bag.BaseItem it_6 = new Bag.BaseItem(5, "消音器", "Sprit/xiaoyinqi", 1, Bag.ItemType.equip_muzzle, 5, "减小射击噪音", "Prefab/Model/xiaoyinqiModel");
        this.BagItemDic.Add(5, it_6);
        //枪械的gain值作为映射，通过gain值获得GunItemDic中枪械对应的具体值
        Bag.BaseItem it_7 = new Bag.BaseItem(6, "MK14突击步枪", "Sprit/M416", 1, Bag.ItemType.weapon_gun, 0, "使用5.56mm子弹", "Prefab/Model/M416");
        this.BagItemDic.Add(6, it_7);
        Bag.BaseItem it_8 = new Bag.BaseItem(7, "AK47突击步枪", "Sprit/AKM", 1, Bag.ItemType.weapon_gun, 1, "使用7.62mm子弹", "Prefab/Model/AKM");
        this.BagItemDic.Add(7, it_8);
        Bag.BaseItem it_9 = new Bag.BaseItem(8, "SCAR-L突击步枪", "Sprit/SCAR-L", 1, Bag.ItemType.weapon_gun, 2, "使用5.56mm子弹", "Prefab/Model/SCAR-L");
        this.BagItemDic.Add(8, it_9);

        GunItemDic = new Dictionary<int, GunDBItem>();
        //id，名称，子弹类型，子弹容量，伤害数值，稳定系数，配件类型
        GunItemDic.Add(0, new GunDBItem(0, "MK14 突击步枪", "Sprit/Weapon/WeaponHud/ColtM4", "Prefab/Model/M416", BulletType.bullet_556mm, Bag.ItemType.weapon_gun, 30, 30, 0.8f,
            new List<Bag.ItemType>() {  Bag.ItemType.equip_butt, Bag.ItemType.equip_magazine
            ,Bag.ItemType.equip_handle,Bag.ItemType.equip_muzzle,Bag.ItemType.equip_scope}));

        GunItemDic.Add(1, new GunDBItem(1, "AK47 突击步枪", "Sprit/Weapon/WeaponHud/Ak47", "Prefab/Model/AKM", BulletType.bullet_762mm, Bag.ItemType.weapon_gun, 30, 40, 0.6f,
    new List<Bag.ItemType>() {  Bag.ItemType.equip_magazine,Bag.ItemType.equip_muzzle,Bag.ItemType.equip_scope}));

        GunItemDic.Add(2, new GunDBItem(2, "SCAR-L 突击步枪", "Sprit/Weapon/WeaponHud/SciFi", "Prefab/Model/SCAR-L", BulletType.bullet_556mm, Bag.ItemType.weapon_gun, 30, 30, 0.8f,
    new List<Bag.ItemType>() {  Bag.ItemType.equip_magazine
            ,Bag.ItemType.equip_handle,Bag.ItemType.equip_muzzle,Bag.ItemType.equip_scope}));
    }
    private void Awake()
    {
        InitBagItemDic();
        Instance = this;
    }
    public enum BulletType
    {
        bullet_556mm,
        bullet_762mm,
        bullet_9mm,
        bullet_shrapnel
    }
    public class GunDBItem 
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imagePath { get; set; }
        public string prefabPath { get; set; }
        public BulletType bulletType;
        public Bag.ItemType weaponType;
        //基础属性
        public int bulletCap;       //子弹容量
        public int hurtNum;         //伤害数值
        public float steadyNum;       //稳定系数
        public List<Bag.ItemType> equipList;
        public GunDBItem(int id, string name, string path, string prefabPath, DataBaseManager.BulletType type, Bag.ItemType weaponType, int bulletCap, int hurtNum,
            float steadyNum, List<Bag.ItemType> equipList)
        {
            this.id = id;
            this.name = name;
            this.imagePath = path;
            this.bulletType = type;
            this.bulletCap = bulletCap;
            this.hurtNum = hurtNum;
            this.steadyNum = steadyNum;
            this.equipList = equipList;
            this.weaponType = weaponType;
        }
    }

}
