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

	/// <summary>
	/// 更新
	/// </summary>
	public abstract void Updata();

}

public class TcpChannel : Channel
{
	//public byte bytes;
	public override void SendMsg(byte[] data)
	{
		
	}
	public override void RecMsg(byte[] data)
	{
		
	}
	public override void Updata()
	{
		
	}
}
