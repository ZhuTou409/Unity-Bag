using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSys;

public class AvatarControl : MonoBehaviour
{
    public float rotateSpeed = 3f;
    public float moveSpeed = 0.05f;
    public float PlayerRotateLerp = 0.3f;
    public float RunSpeed = 1.5f;
    private Transform Player;
    private Transform PlayerCamera;
    private GameObject LookAtThis;
    private Animator animator;
    //摄像机与主角之间的距离
    public float CameraDis = 3;
    private Vector3 offset;
    //鼠标旋转范围
    private float minXRotation = -40;
    private float maxXRotation = 40;
    private float minYRotation = -60;
    private float maxYRotation = 60;

    void Start()
    {
        Player = gameObject.GetComponent<Transform>();
        LookAtThis = Player.Find("LookAtThis").gameObject;
        PlayerCamera = GameObject.Find("Camera").transform;
        offset = PlayerCamera.position - Player.position;
        animator = Player.gameObject.GetComponent<Animator>();
        //隐藏鼠标显示
        //Cursor.visible = false;
    }

    private void CameraControler(float inputX,float inputY)
    {
        Ray ray = PlayerCamera.gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        offset = offset.normalized * CameraDis;
        PlayerCamera.position = Vector3.Lerp(PlayerCamera.position, Player.position + offset, 1f);

        //PlayerCamera.LookAt(Player.position);
        PlayerCamera.LookAt(LookAtThis.transform.position);
        if (Input.mousePosition.x>Screen.width || Input.mousePosition.x <= 0)
        {

        }
        PlayerCamera.RotateAround(Player.position, Vector3.up, rotateSpeed*inputX);
        PlayerCamera.RotateAround(Player.position, Player.transform.right, -rotateSpeed*inputY);

        offset = PlayerCamera.position - Player.position;
        offset = offset.normalized * CameraDis;
        PlayerCamera.position = Player.position + offset;
    }

    private void PlayerControler()
    {
        float inputY = Input.GetAxis("Mouse Y");
        float inputX = Input.GetAxis("Mouse X");
        //Debug.Log("inputx:" + Input.mousePosition.x + " inputy:" + Input.mousePosition.y);
        Quaternion TargetBodyCurrentRotation = Player.rotation;
        Player.rotation = Quaternion.Lerp(TargetBodyCurrentRotation, Quaternion.Euler(new Vector3(Player.localEulerAngles.x, PlayerCamera.localEulerAngles.y, Player.localEulerAngles.z)), PlayerRotateLerp);

        CameraControler(inputX, inputY);
    }
    public void TextEvent()
    {
        Debug.Log("Text Event");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(Player.name + " 靠近物体: " + other.name);
        EquipInfo info = other.gameObject.GetComponent<EquipInfo>();
        int index = info.equipId;
        Debug.Log(" equipid: " + index);
        Bag.ItemType type = info.type;
        SendBaseClass send = new PickInfo(index,other.gameObject,type);
        LiteEventManager.Instance.TriggerEvent(CollectKey.Trig, send);
        //UIManager.Instance.m_collectController.AddItemToList(index,DataBaseManager.Instance.BagItemDic[index], 
            //"Prefab/BagItem");
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(Player.name + " 远离物体: " + other.name);
        int index = other.gameObject.GetInstanceID();
        LiteEventManager.Instance.TriggerEvent(CollectKey.Leave, index);
        //UIManager.Instance.m_collectController.RemoveItem(other.gameObject.GetComponent<EquipInfo>().equipId);
    }
    void Update()
    {
        PlayerControler();

        if(Input.GetKey(KeyCode.W))
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                Player.Translate(Vector3.forward * moveSpeed*RunSpeed, Space.Self);
            }
            else
            {
                Player.Translate(Vector3.forward * moveSpeed, Space.Self);
            }
        }
        else if(Input.GetKey(KeyCode.S))
        {
            Player.Translate(Vector3.back * moveSpeed, Space.Self);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Player.Translate(Vector3.left * moveSpeed, Space.Self);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Player.Translate(Vector3.right * moveSpeed, Space.Self);
        }
        else
        {
        }
    }
}
