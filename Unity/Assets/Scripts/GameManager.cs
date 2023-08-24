using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    
    //ip和地址
    private string ip = "127.0.0.1";
    private int port = 8888;

    public static ClientManager ClientManager => ClientManager.Instance;

    private int i = 5;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        ClientManager.Connect(ip,port);
        // Debug.Log(TimerSystem.Instance.Timestamp());
        // Debug.Log(long.MaxValue);
        // TimerSystem.Instance.Schedule(obj => { Debug.Log("执行回调方法"+i); },null,false,3,2,5);
    }

    // Update is called once per frame
    void Update()
    {
        // TimerSystem.Instance.Updata();
    }
    void FixedUpdate()
    {
        ClientManager.Updata();
    }

    private void OnDestroy()
    {
        ClientManager.Close();
    }

}
