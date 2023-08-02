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
        if(Input.GetKeyDown(KeyCode.W))
        {
            C2S_LoginMsg c2SLoginMsg = new C2S_LoginMsg() { RpcId = 1, Account = "yangyue", Password = "yangyue" };
            var data = MemoryPackHelper.Serialize(c2SLoginMsg); 
            ClientManager.Send(data);
        }
    }
    private void OnDestroy()
    {
        ClientManager.Close();
    }
}
