using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ServerTest : MonoBehaviour
{
    private Thread _thread;
    // Start is called before the first frame update
    void Start()
    {
        _thread = new Thread(() =>
        {
            NetManager.StartLoop(8888);
        });
        _thread.Start();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        NetManager.Close();
        _thread.Abort();
    }
}
