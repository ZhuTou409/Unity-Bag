using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSys;
using Avatar.Value;

public class ResManager : MonoBehaviour
{
    //单例模式
    public static ResManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    //加载一个prefab
    public GameObject LoadPrefabFromRes(string path, bool Ins)
    {
        GameObject obj = Resources.Load(path) as GameObject;
        if (Ins == true)
            return Instantiate(obj);
        return obj;
    }
    //实例化GameObject
    public GameObject InstantiateObject(GameObject obj)
    {
        return Instantiate(obj);
    }
    //加载jason文件
    public void LoadJasonFile(string path)
    {

    }
    //加载sprite
    public Sprite LoadSpriteFromRes(string path)
    {
        Sprite sp = Resources.Load<Sprite>(path);
        if (sp)
        {
            try
            {
                return Instantiate(sp);
            }
            catch(System.Exception ex)
            {

            }
        }
        return null;
    }
    /// <summary>
    /// 往场景中添加实例
    /// </summary>
    /// <param name="Path"></param>
    /// <param name="location"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public bool CreateGameObject(string Path,Vector3 location, PickInfo info)
    {
        GameObject obj = LoadPrefabFromRes(Path,true);
        //由于模型的问题，需要将rotation的沿z轴旋转90度
        obj.transform.position = location;
        obj.transform.SetParent(AvatarInfoManager.Instance.GetSceneTransform(),true);

        EquipInfo thisInifo = obj.GetComponent<EquipInfo>();
        if(thisInifo == null)
        {
            obj.AddComponent<EquipInfo>();
        }
        EquipInfo Info = obj.GetComponent<EquipInfo>();
        Info.equipId = info.index;
        Info.type = info.type;
        return true;
    }
    public void ClearMemory()
    {
        Resources.UnloadUnusedAssets();
    }
}
