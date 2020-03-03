using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserView : MonoBehaviour {
    private Text levelText;
    private Text coneText;
    private Text currentExp;
    private Text fullExp;
    [HideInInspector]
    public Button addExp;
    [HideInInspector]
    public Button addCone;

    private void Awake()
    {
        levelText = GameTool.FindTheChild(gameObject, "levelText").gameObject.GetComponent<Text>();
        coneText = GameTool.FindTheChild(gameObject, "coneText").gameObject.GetComponent<Text>();
        currentExp = GameTool.FindTheChild(gameObject, "currentExp").gameObject.GetComponent<Text>();
        fullExp = GameTool.FindTheChild(gameObject, "fullExp").gameObject.GetComponent<Text>();
        addExp = GameTool.FindTheChild(gameObject, "addExp").gameObject.GetComponent<Button>();
        addCone = GameTool.FindTheChild(gameObject,"addCone").gameObject.GetComponent<Button>();
        
    }
    public void SetConeText(int cone)
    {
        coneText.text = cone.ToString();
    }
    public void SetFullExp(int fullexp)
    {
        this.fullExp.text = fullexp.ToString();
    }
    public void SetCurrentExp(int currentexp)
    {
        this.currentExp.text = currentexp.ToString(); 
    }
    public void SetLevelText(int level)
    {
        this.levelText.text = level.ToString();
    }
    private void Start()
    {
        
    }


}
