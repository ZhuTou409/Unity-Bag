using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour {

    private UserView view;
    private UserModel userModel;
    private void Awake()
    {
        view = gameObject.GetComponent<UserView>();
        userModel = new UserModel();
    }
    public void AddExp(GameObject go)
    {
        userModel.CurrentExp += 99;
        if (userModel.CurrentExp>=userModel.FullExp)
        {
            userModel.CurrentExp = userModel.CurrentExp - userModel.FullExp;
            LevelUP();
        }
        view.SetCurrentExp(userModel.CurrentExp);
        view.SetFullExp(userModel.FullExp);
        view.SetLevelText(userModel.Level);
    }
    public void AddCone(GameObject go)
    {
        userModel.Cone += 1000;
        view.SetConeText(userModel.Cone);
    }
    public void LevelUP()
    {
        userModel.Level++;
        userModel.FullExp = userModel.Level * 100;
    }
    // Use this for initialization
    void Start () {
        UGUIEventListener.Get(view.addCone.gameObject).onClick += AddCone;
        UGUIEventListener.Get(view.addExp.gameObject).onClick += AddExp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
