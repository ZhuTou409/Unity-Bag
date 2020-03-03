using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public BagController m_bagController;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        m_bagController = new BagController();
        m_bagController.AddItem(DataBaseManager.Instance.BagItemDic[0]);
        m_bagController.AddItem(DataBaseManager.Instance.BagItemDic[1]);
        m_bagController.AddItem(DataBaseManager.Instance.BagItemDic[0]);
    }

    // Update is called once per frame
    void Update()
    {
        m_bagController.Update();
    }
}
