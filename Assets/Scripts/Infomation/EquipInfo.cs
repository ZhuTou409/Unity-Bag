using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInfo : MonoBehaviour
{
    //装备类型
    public Bag.ItemType type;
    //装备id
    public int equipId;
    public GameObject obj;
    public EquipInfo(int id, Bag.ItemType T)
    {
        type = T;
        equipId = id;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(trans.name+" 靠近物体: "+other.name);
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log(trans.name + " 远离物体: " + other.name);
    //}
}
