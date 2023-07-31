using MemoryPack;

[MemoryPackable]
public partial class BaseMessage: IRequest
{
	public int RpcId { get; set; }

	
	public string Account { get; set; }
	
	public string Password { get; set; }
}