using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bag;
using Avatar.Value;
public class GameRoot : MonoBehaviour
{
    public AvatarInfoManager avaInfoMgr;
    private void Awake()
    {
        avaInfoMgr = new AvatarInfoManager();
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
