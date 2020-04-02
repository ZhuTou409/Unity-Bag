using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag;
using Bag.Collect;
using Avatar.Value;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public BagController m_bagController;
    public CollectController m_collectController;
    public GameObject canvas;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        m_bagController = new BagController();
        m_collectController = new CollectController(canvas);

        //for(int i = 0;i<DataBaseManager.Instance.BagItemDic.Count-3;i++)
        //{
        //    m_bagController.AddItem(DataBaseManager.Instance.BagItemDic[i]);
        //}

        //m_bagController.AddGun(DataBaseManager.Instance.GunItemDic[0]);
        //m_bagController.AddGun(DataBaseManager.Instance.GunItemDic[1]);
    }

    // Update is called once per frame
    void Update()
    {
        
        m_bagController.Update();
        if (Input.GetKeyDown(KeyCode.R))
        {
            var enumerator = m_bagController.m_equipModel.gunItems[1].equipItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Debug.Log("key: " + enumerator.Current.Key + " name: " + enumerator.Current.Value.name);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            var enumerator = m_bagController.m_equipModel.gunItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Debug.Log("key: " + enumerator.Current.Key + " name: " + enumerator.Current.Value.name);
            }

        }
        //实验
        else if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("当前556子弹数量：" + AvatarInfoManager.Instance.GetBulletCount(556));
            Debug.Log("当前762子弹数量：" + AvatarInfoManager.Instance.GetBulletCount(762));
            Debug.Log("枪支1 攻击力：" + AvatarInfoManager.Instance.GetGunAttackVaalue(0));
            Debug.Log("枪支1 稳定性：" + AvatarInfoManager.Instance.GetGunSteadyValue(0));
            Debug.Log("枪支2 攻击力：" + AvatarInfoManager.Instance.GetGunAttackVaalue(1));
            Debug.Log("枪支2 稳定性：" + AvatarInfoManager.Instance.GetGunSteadyValue(1));
        }
    }
}
