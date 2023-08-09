using System;
using System.Text;
using UnityEngine;

public class ClientTest : MonoBehaviour
{
  
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            C2S_LoginMsg c2SLoginMsg = new C2S_LoginMsg() {RpcId = 1, Account = "yangyue", Password = "yangyue2" };
            var data = MemoryPackHelper.Serialize(c2SLoginMsg);
            var datas = YYProtolcol.TcpProtocol.MsgToBytes((int)MsgType.C2S_LoginMsg, data);
            Debug.Log(string.Join(" ",datas));
         
            ClientController.Instance.Send(datas);
            // ClientController.Instance.RecMsg(datas);
            
            
        }
       
        if(Input.GetKeyDown(KeyCode.S))
        {
            C2C_SendMsg c2CSendMsg = new C2C_SendMsg() { RpcId = 2, msg = "111222" };
            
           
            var data = MemoryPackHelper.Serialize(c2CSendMsg); 
            // ClientManager.Send(data);
        }
    }

}
