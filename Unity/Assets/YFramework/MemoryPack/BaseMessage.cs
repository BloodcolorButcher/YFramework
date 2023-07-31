using MemoryPack;

[MemoryPackable]
public class BaseMessage: IRequest
{
	public int RpcId { get; set; }

	
	public string Account { get; set; }
	
	public string Password { get; set; }
}