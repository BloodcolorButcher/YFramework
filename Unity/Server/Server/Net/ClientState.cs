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
		_channel.MsgEvent += MsgHandler;
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
			case MsgType.C2S_LoginMsg:
			{
				C2S_LoginMsg c2SLoginMsg = MemoryPackHelper.DeserializeObject<C2S_LoginMsg>(datas);
				S2C_LoginMsg s2CLoginMsg = new S2C_LoginMsg();
				s2CLoginMsg.RpcId = c2SLoginMsg.RpcId;
				if(c2SLoginMsg.Account == "yangyue" && c2SLoginMsg.Password== "yangyue")
				{
					s2CLoginMsg.Message = "登录成功！！！";
					s2CLoginMsg.Error = 2000;

				}
				else
				{
					s2CLoginMsg.Message = "登录失败！！！";
					s2CLoginMsg.Error = 2001;
				}
				var msg = MemoryPackHelper.Serialize(s2CLoginMsg);
				var bytes = YYProtolcol.TcpProtocol.MsgToBytes((int)MsgType.S2C_LoginMsg,msg);
				
				// SendMsg(bytes);

			}
				break;
			case MsgType.C2S_SendMsg:
			{
				
			}
				break;
			
			
			
		}
	}

	/// <summary>
	/// 关闭连接
	/// </summary>
	public void Close()
	{
		_channel.MsgEvent -= MsgHandler;
		workSocket.Close();
		workSocket = null;
	}
	
}