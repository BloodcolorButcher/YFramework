
public enum MsgType
{
	/// <summary>
	/// 客户端发送登录请求的类型
	/// </summary>
	S2C_LoginMsg = 0x0100,
	/// <summary>
	/// 服务器回复登录请求
	/// </summary>
	C2S_LoginMsg = 0x1100,
	/// <summary>
	/// 客户端向服务器发送消息
	/// </summary>
	C2S_SendMsg = 0x0200,
	
	/// <summary>
	/// 服务器向客户端发送消息
	/// </summary>
	S2C_SendMsg = 0x1200,
	
}