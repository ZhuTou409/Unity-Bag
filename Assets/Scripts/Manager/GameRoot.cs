using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag;
using Avatar.Value;
public class GameRoot : MonoBehaviour
{
    //public AvatarInfoManager avaInfoMgr;
    //public Transform avatarTrans;
    //public Transform sceneTrans;
    private void Awake()
    {
        //avatarTrans = GameObject.Find("Avatar").transform;
        //sceneTrans = GameObject.Find("Scene").transform;
        //avaInfoMgr = new AvatarInfoManager(avatarTrans,sceneTrans);
        Debug.Log(AvatarInfoManager.Instance.GetAvatarTransform().localPosition);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
