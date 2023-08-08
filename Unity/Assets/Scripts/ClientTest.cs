using System;
using System.Text;
using UnityEngine;

public class ClientTest : MonoBehaviour
{
  
    
    // Start is called before the first frame update
    void Start()
    {
        // ClientManager.Connect(ip, port);
           
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            C2S_LoginMsg c2SLoginMsg = new C2S_LoginMsg() {RpcId = 1, Account = "yangyue", Password = "yangyue" };
            var data = MemoryPackHelper.Serialize(c2SLoginMsg); 
            // ClientManager.Send(data);
        }
       
        if(Input.GetKeyDown(KeyCode.S))
        {
            C2C_SendMsg c2CSendMsg = new C2C_SendMsg() { RpcId = 2, msg = "111222" };
            
           
            var data = MemoryPackHelper.Serialize(c2CSendMsg); 
            // ClientManager.Send(data);
        }
    }
    private void FixedUpdate()
    {
        
    }
    private void OnDestroy()
    {
        // ClientManager.Close();
    }
}
