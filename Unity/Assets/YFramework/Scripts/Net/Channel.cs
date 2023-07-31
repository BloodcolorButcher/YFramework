/// <summary>
/// 抽象类
/// </summary>
public abstract class Channel
{
	/// <summary>
	/// 发送消息
	/// </summary>
	/// <param name="data"></param>
	public abstract void SendMsg(byte[] data);

	/// <summary>
	/// 收到消息
	/// </summary>
	/// <param name="data"></param>
	public abstract void RecMsg(byte[] data);
	
}
