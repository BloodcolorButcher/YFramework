using System.Net.Sockets;

public class ClientState
{
	public Socket workSocket;

	//Ping
	public long lastPingTime = 0;
	private Channel _channel;
	public ClientState()
	{
		_channel = new TcpChannel();
		_channel
	}

	/// <summary>
	/// 接收消息
	/// </summary>
	/// <param name="datas"></param>
	public void RecMsg(byte[] datas)
	{
		_channel.RecMsg(datas);
	}
	public void SendMsg(byte[] datas)
	{
		workSocket.BeginSend(datas, 0, datas.Length, 0, null, null);
	}
	
	public void SendMsg2Others(byte[] datas)
	{
		NetManager.SendOthers(workSocket,datas);
		// workSocket.BeginSend(datas, 0, datas.Length, 0, null, null);
	}
	public void Updata()
	{
		_channel.Updata();
	}
	
	/// <summary>
	/// 各种消息的处理
	/// </summary>
	/// <param name="msgType"></param>
	/// <param name="data"></param>
	private void MsgHandler(MsgType msgType,byte[] datas)
	{
		switch(msgType)
		{
			case MsgType.S2C_LoginMsg:
			{
				C2S_LoginMsg c2SLoginMsg = MemoryPackHelper.DeserializeObject<C2S_LoginMsg>(datas);
				S2C_LoginMsg s2CLoginMsg = new S2C_LoginMsg();
				if(c2SLoginMsg.Account == "yangyue" && c2SLoginMsg.Password== "yangyue")
				{
					s2CLoginMsg.RpcId = c2SLoginMsg.RpcId;
				}
				
				

			}
				break;
			case MsgType.C2S_SendMsg:
			{
				
			}
				break;
			
			
			
		}
	}
	
}