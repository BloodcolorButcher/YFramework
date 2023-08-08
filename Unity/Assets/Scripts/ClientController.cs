using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientController : MonoBehaviour
{
    public static ClientController Instance = null;
    //ip和地址
    private string ip = "127.0.0.1";
    private int port = 8888;

    private ClientManager _clientManager = new ClientManager();
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _clientManager.Connect(ip,port);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _clientManager.Updata();
    }
    
    
    
}
