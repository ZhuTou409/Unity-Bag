using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
