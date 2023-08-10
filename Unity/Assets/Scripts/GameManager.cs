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
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        ClientManager.Connect(ip,port);
    }

    // Update is called once per frame
    void Update()
    {
        
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
