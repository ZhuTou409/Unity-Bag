using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTool : MonoBehaviour{

    /// <summary>
    /// 获取随机数
    /// </summary>
    /// <param name="num1">随机数最小范围</param>
    /// <param name="num2">随机数最大范围</param>
    /// <returns>返回的随机数</returns>
    public static int GetRandomInt(int num1,int num2)
    {
        if (num1<num2)
        {
            return UnityEngine.Random.Range(num1,num2);
        }
        else
        {
            return UnityEngine.Random.Range(num2,num1);
        }
    }

    /// <summary>
    /// 清理内存，频繁调用gc会消耗系统内存，一般在场景切换时调用
    /// </summary>
    public static void CleanMemory()
    {
        GC.Collect();
        Resources.UnloadUnusedAssets();
    }
    /// <summary>
    /// 根据字符c分割字符串
    /// </summary>
    /// <param name="str">被分割的字符串</param>
    /// <param name="char">根据字符char</param>
    /// <returns>返回的字符数组</returns>
    public static string[] SplitString(string str,char c)
    {
        return str.Split(c);
    }
    /// <summary>
    /// 内存中是否还有这个键
    /// </summary>
    /// <param name="keyName">键的名称</param>
    /// <returns>返回bool结果</returns>
    public static bool HasKey(string keyName)
    {
        return PlayerPrefs.HasKey(keyName);
    }
    /// <summary>
    /// PlayerPrefs中获取键的值
    /// </summary>
    /// <param name="keyName">键的名称</param>
    /// <returns>返回的int值</returns>
    public static int GetInt(string keyName)
    {
        return PlayerPrefs.GetInt(keyName);
    }
    /// <summary>
    /// 设置int键的值
    /// </summary>
    /// <param name="keyName">键的名称</param>
    /// <param name="keyInt">int值</param>
    public static void SetInt(string keyName, int keyInt)
    {
        PlayerPrefs.SetInt(keyName,keyInt);
    }

    /// <summary>
    /// 获取内存中float的值
    /// </summary>
    /// <param name="keyName">键的名称</param>
    /// <returns>返回的float值</returns>
    public static float GetFloat(string keyName)
    {
        return PlayerPrefs.GetFloat(keyName);
    }
    /// <summary>
    /// 设置键的float值
    /// </summary>
    /// <param name="keyName">键的名称</param>
    /// <param name="keyFloat">float值</param>
    public static void SetFloat(string keyName,float keyFloat)
    {
        PlayerPrefs.SetFloat(keyName,keyFloat);
    }
    /// <summary>
    /// 获取内存中string的值
    /// </summary>
    /// <param name="keyName">键的名称</param>
    /// <returns>返回的string值</returns>
    public static string GetString(string keyName)
    {
        return PlayerPrefs.GetString(keyName);
    }
    /// <summary>
    /// 设置string键的值
    /// </summary>
    /// <param name="keyName">键的名称</param>
    /// <param name="str">string值</param>
    public static void SetString(string keyName,string str)
    {
        PlayerPrefs.SetString(keyName,str);
    }
    /// <summary>
    /// 删除内存中所有键
    /// </summary>
    public static void DeleteAllKey()
    {
        PlayerPrefs.DeleteAll();
    }
    /// <summary>
    /// 删除内存中制定的键
    /// </summary>
    /// <param name="KeyName">键的名称</param>
    public static void DeleteKey(string KeyName)
    {
        PlayerPrefs.DeleteKey(KeyName);
    }
    /// <summary>
    /// 查找子物体
    /// </summary>
    /// <param name="GameObjectName">父节点</param>
    /// <param name="ChildName">子物体名称</param>
    /// <returns>返回子物体，没找到返回空</returns>
    public static Transform FindTheChild(GameObject GameObjectName,string ChildName)
    {
        Transform Ttransform = GameObjectName.transform.Find(ChildName);
        if (Ttransform == null)
        {
            foreach (Transform item in GameObjectName.transform)
            {
                Ttransform = FindTheChild(item.gameObject,ChildName);
                if (Ttransform != null)
                {
                    return Ttransform;
                }
            }
        }
        return Ttransform;
    }
    /// <summary>
    /// 获取子物体脚本
    /// </summary>
    /// <typeparam name="T">子物体脚本类型</typeparam>
    /// <param name="GameObjectName">父节点</param>
    /// <param name="chileName">子物体名称</param>
    /// <returns>返回子物体的脚本</returns>
    public static T GetTheChildComponent<T>(GameObject GameObjectName, string chileName) where T:Component
    {
        Transform Ttransform = FindTheChild(GameObjectName,chileName);
        if (Ttransform!=null)
        {
            return Ttransform.GetComponent<T>();
        }
        else
        {
            return null; 
        }
    }
    /// <summary>
    /// 给子物体添加脚本
    /// </summary>
    /// <typeparam name="T">添加的脚本</typeparam>
    /// <param name="GameObjectName">父节点</param>
    /// <param name="ChildName">子节点名称</param>
    /// <returns>添加完脚本后返回该脚本</returns>
    public static T ADDTheChildCompont<T>(GameObject GameObjectName, string ChildName) where T : Component
    {
        Transform Ttransform = FindTheChild(GameObjectName,ChildName);
        if (Ttransform!=null)
        {
            T[] TtransformArr = Ttransform.GetComponents<T>();
            for (int i = 0; i < TtransformArr.Length; i++)
            {
                if (TtransformArr[i]!=null)
                {
                    Destroy(TtransformArr[i]);
                }
            }
            return Ttransform.gameObject.AddComponent<T>();
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 添加子节点
    /// </summary>
    /// <param name="ParentTrans">父节点</param>
    /// <param name="TchildTrans">子节点</param>
    public static void AddChildToParent(Transform ParentTrans,Transform TchildTrans)
    {
        TchildTrans.parent = ParentTrans;
        TchildTrans.localPosition = Vector3.zero;
        TchildTrans.localScale = Vector3.one;
        TchildTrans.localEulerAngles = Vector3.zero;
    }
    
    /// <summary>
    /// 加载场景的开关
    /// </summary>
    public static void OpenLoadSceneHelper()
    {
        GameObject uiRoot = GameObject.Find("Canvas");
        GameObject go = uiRoot.transform.Find("LoadSceneHelper").gameObject;
        if (go.activeSelf==false)
        {
            go.SetActive(true);
        }
    }
    public static bool s_bIsFirstLoad = true;
}