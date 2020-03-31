using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag;
using Bag.Collect;

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var enumerator = m_bagController.m_equipModel.gunItems[0].equipItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Debug.Log("key: " + enumerator.Current.Key + " name: " + enumerator.Current.Value.name);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (Bag.GunItem it in m_bagController.m_equipModel.gunItems.Values)
            {
                Debug.Log("dicKey: " + it.name + "PrefabPath:" + it.ScenePrefabPath);
            }

        }
    }
}
