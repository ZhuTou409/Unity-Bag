using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base;

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
        Bag.BaseItem it_1 = new Bag.BaseItem(0, "5.56毫米子弹", "Sprit/bullet-5.56mm", 30,EquipType.bullet_556,4,"适合5.56mm口径自动步枪","Prefab/Weapons/Modular/Bullet_556mm_Box");
        this.BagItemDic.Add(0, it_1);
        Bag.BaseItem it_2 = new Bag.BaseItem(1, "垂直握把", "Sprit/chuizhiwoba", 1,EquipType.equip_handle, 0.5f, "增加枪支稳定性", "Prefab/Weapons/Modular/Handle_CZ");
        this.BagItemDic.Add(1, it_2);
        Bag.BaseItem it_3 = new Bag.BaseItem(2, "直角握把", "Sprit/zhijiaowoba", 1, EquipType.equip_handle, 0.6f, "增加枪支稳定性", "Prefab/Weapons/Modular/Handle_ZJ");
        this.BagItemDic.Add(2, it_3);
        Bag.BaseItem it_4 = new Bag.BaseItem(3, "饮料", "Sprit/kele", 1, EquipType.medicine_s, 10, "少量回复生命", "Prefab/Props/SM_Prop_Pills_01");
        this.BagItemDic.Add(3, it_4);
        Bag.BaseItem it_5 = new Bag.BaseItem(4, "2倍 瞄准镜", "Sprit/2-beijing", 1, EquipType.equip_scope, 2, "放大两倍", "Prefab/Weapons/Modular/Scope_2");
        this.BagItemDic.Add(4, it_5);
        Bag.BaseItem it_6 = new Bag.BaseItem(5, "消音器", "Sprit/xiaoyinqi", 1, EquipType.equip_muzzle, 0.5f, "减小射击噪音", "Prefab/Weapons/Modular/Silencer_01");
        this.BagItemDic.Add(5, it_6);
        //枪械的gain值作为映射，通过gain值获得GunItemDic中枪械对应的具体值
        Bag.BaseItem it_7 = new Bag.BaseItem(6, "MK14突击步枪", "Sprit/M416", 1, EquipType.weapon_gun, 6, "使用5.56mm子弹", "Prefab/Weapons/Guns/M416");
        this.BagItemDic.Add(6, it_7);
        Bag.BaseItem it_8 = new Bag.BaseItem(7, "AK47突击步枪", "Sprit/AKM", 1, EquipType.weapon_gun, 7, "使用7.62mm子弹", "Prefab/Weapons/Guns/AKM");
        this.BagItemDic.Add(7, it_8);
        Bag.BaseItem it_9 = new Bag.BaseItem(8, "SCAR-L突击步枪", "Sprit/SCAR-L", 1, EquipType.weapon_gun, 8, "使用5.56mm子弹", "Prefab/Weapons/Guns/SCAR-L");
        this.BagItemDic.Add(8, it_9);

        GunItemDic = new Dictionary<int, GunDBItem>();
        //id，名称，子弹类型，子弹容量，伤害数值，稳定系数，配件类型
        GunItemDic.Add(6, new GunDBItem(6, "MK14 突击步枪", "Sprit/Weapon/WeaponHud/ColtM4", "Prefab/Weapons/Guns/M416", BulletType.bullet_556mm, EquipType.weapon_gun, 30, 30, 0.8f,
            new List<EquipType>() {  EquipType.equip_butt, EquipType.equip_magazine
            ,EquipType.equip_handle,EquipType.equip_muzzle,EquipType.equip_scope}));

        GunItemDic.Add(7, new GunDBItem(7, "AK47 突击步枪", "Sprit/Weapon/WeaponHud/Ak47", "Prefab/Weapons/Guns/AKM", BulletType.bullet_762mm, EquipType.weapon_gun, 30, 40, 0.6f,
    new List<EquipType>() {  EquipType.equip_magazine,EquipType.equip_muzzle,EquipType.equip_scope}));

        GunItemDic.Add(8, new GunDBItem(8, "SCAR-L 突击步枪", "Sprit/Weapon/WeaponHud/SciFi", "Prefab/Weapons/Guns/SCAR-L", BulletType.bullet_556mm, EquipType.weapon_gun, 30, 30, 0.8f,
    new List<EquipType>() {  EquipType.equip_magazine
            ,EquipType.equip_handle,EquipType.equip_muzzle,EquipType.equip_scope}));
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
        public EquipType weaponType;
        //基础属性
        public int bulletCap;       //子弹容量
        public int hurtNum;         //伤害数值
        public float steadyNum;       //稳定系数
        public List<EquipType> equipList;
        public GunDBItem(int id, string name, string path, string prefabPath, BulletType type, EquipType weaponType, int bulletCap, int hurtNum,
            float steadyNum, List<EquipType> equipList)
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
            this.prefabPath = prefabPath;
        }
    }

}
