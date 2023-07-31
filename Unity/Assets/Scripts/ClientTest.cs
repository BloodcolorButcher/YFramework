using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ClientTest : MonoBehaviour
{
  
    //ip和地址
    private string ip = "127.0.0.1";
    private int port = 8888;
    // Start is called before the first frame update
    void Start()
    {
        ClientManager.Connect(ip, port);
           
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            ClientManager.Send(Encoding.UTF8.GetBytes("你好服务器！！！"));
        }
    }
    private void OnDestroy()
    {
        ClientManager.Close();
    }
}
