using System.Net.Sockets;
using System.Text;

public class ClientState
{
	public Socket workSocket;
	public const int BufferSize = 1024;
	public byte[] buffer = new byte[BufferSize];
	public StringBuilder sb = new StringBuilder();
	//Ping
	public long lastPingTime = 0;
	//玩家
		
}