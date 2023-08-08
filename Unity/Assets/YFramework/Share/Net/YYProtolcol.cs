using System.IO;

/// <summary>
/// 协议
/// </summary>
public static class YYProtolcol
{
	/// <summary>
	/// Tcp通讯协议 长度（两个字节最大传输64kb【消息长度最大为64*1024-2】），消息类型(两个b)，
	/// len_H len_L maincmd subcmd  +datas
	/// 消息体（长度最大为64kb-2）
	/// </summary>
	public static class TcpProtocol
	{
		
		/// <summary>
		/// 消息转换成byte[]
		/// </summary>
		/// <param name="msgType"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static byte[] MsgToBytes(int msgType,byte[] data)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				int len = data.Length+2;
				//填入数据长度
				stream.Write(Num2Bytes(len));
				stream.Write(Num2Bytes(msgType));
				stream.Write(data);
				return stream.ToArray();
			}	
		}
	}
	
	
	/// <summary>
	/// 只取 int的后16位，分成两个byte存储
	/// </summary>
	/// <param name="num"></param>
	/// <returns>Bytes</returns>
	public static byte[] Num2Bytes(int num)
	{
		byte[] bytes = new byte[2];
		bytes[0] = (byte)((num >> 8) & 0xff);
		bytes[1] = (byte)(num & 0xff);
		return bytes;
	}
	/// <summary>
	/// ushort分成两个byte存储
	/// </summary>
	/// <param name="num"></param>
	/// <returns>Bytes</returns>
	private static byte[] Num2Bytes(ushort num)
	{
		byte[] bytes = new byte[2];
		bytes[0] = (byte)((num >> 8) & 0xff);
		bytes[1] = (byte)(num & 0xff);
		return bytes;
	}
	/// <summary>
	/// 两个byte合成一个int
	/// </summary>
	/// <param name="high"></param>
	/// <param name="low"></param>
	/// <returns></returns>
	public static int Bytes2Int(int high,int low)
	{
		return(high << 8) + low;
	}
    
	/// <summary>
	/// 两个byte合成一个unshort
	/// </summary>
	/// <param name="high"></param>
	/// <param name="low"></param>
	/// <returns></returns>
	public static ushort Bytes2UnShort(int high,int low)
	{
		return (ushort)((high << 8) + low);
	}
}