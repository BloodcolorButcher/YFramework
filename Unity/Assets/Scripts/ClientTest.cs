using System;
using System.Text;
using UnityEngine;

public class ClientTest : MonoBehaviour
{

    private void Start()
    {
      
        // GameManager.ClientManager.AddMsgDic(MsgType.S2C_LoginMsg,RecLoginMsg2);
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            GameManager.ClientManager.AddMsgDic(MsgType.S2C_LoginMsg,RecLoginMsg2);
            C2S_LoginMsg c2SLoginMsg = new C2S_LoginMsg() {RpcId = 1, Account = "yangyue", Password = "yangyue" };
            var data = MemoryPackHelper.Serialize(c2SLoginMsg);
            var datas = YYProtolcol.TcpProtocol.MsgToBytes((int)MsgType.C2S_LoginMsg, data);
            Debug.Log(string.Join(" ",datas));
         
            GameManager.ClientManager.Send(datas);
            // ClientController.Instance.RecMsg(datas);
            
           
        }
       
        if(Input.GetKeyDown(KeyCode.S))
        {
            
            C2S_LoginMsg c2SLoginMsg = new C2S_LoginMsg() {RpcId = 1, Account = "yangyue", Password = "yangyue2" };
            var data = MemoryPackHelper.Serialize(c2SLoginMsg);
            var datas = YYProtolcol.TcpProtocol.MsgToBytes((int)MsgType.C2S_LoginMsg, data);
            Debug.Log(string.Join(" ",datas));
         
            GameManager.ClientManager.Send(datas);
            // ClientManager.Send(data);
        }
    }

    private void RecLoginMsg(byte[] datas)
    {
        Debug.Log("触发了消息");
        S2C_LoginMsg s2CLoginMsg = MemoryPackHelper.DeserializeObject<S2C_LoginMsg>(datas);
        if(s2CLoginMsg!=null)
        {
            Debug.Log(s2CLoginMsg.RpcId);
            Debug.Log(s2CLoginMsg.Message);
            Debug.Log(s2CLoginMsg.Error);
        }  
        GameManager.ClientManager.RemMsgDic(MsgType.S2C_LoginMsg,RecLoginMsg);
        
    }
    
    private void RecLoginMsg2(byte[] datas)
    {
        Debug.Log("触发了消息");
        datas = new byte[] { 1, 2, 3 };
        S2C_LoginMsg s2CLoginMsg = MemoryPackHelper.DeserializeObject<S2C_LoginMsg>(datas);
        if(s2CLoginMsg!=null)
        {
            Debug.Log(s2CLoginMsg.RpcId);
            Debug.Log(s2CLoginMsg.Message);
            Debug.Log(s2CLoginMsg.Error);
        }
        else
        {
            Debug.Log("解析错误");
        }
        GameManager.ClientManager.RemMsgDic(MsgType.S2C_LoginMsg,RecLoginMsg2);
        
    }

    private void SendCall(MsgType sendMsgTyp,byte[]data ,MsgType recMsgType,Action<byte[]> bytesEvent)
    {
        //注册接收消息
        GameManager.ClientManager.AddMsgDic(recMsgType,bytesEvent);
        //发送消息
        YYProtolcol.TcpProtocol.MsgToBytes((int)sendMsgTyp, data);
    }
    
    
    
    

}
