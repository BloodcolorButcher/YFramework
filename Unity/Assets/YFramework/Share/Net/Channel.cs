using System;

/// <summary>
/// 抽象类
/// </summary>
public abstract class Channel
{

	public abstract Action<MsgType, byte[]> MsgEvent
	{
		get;
		set;
	}
	/// <summary>
	/// 发送消息
	/// </summary>
	/// <param name="data"></param>
	public abstract void SendMsg(byte[] datas);

	/// <summary>
	/// 收到消息
	/// </summary>
	/// <param name="data"></param>
	public abstract void RecMsg(byte[] datas);

	/// <summary>
	/// 更新
	/// </summary>
	public abstract void Updata();

    
}

