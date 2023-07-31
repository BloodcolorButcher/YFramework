using MemoryPack;

[MemoryPackable()]
public class C2S_LoginMsg : IRequest
{
	
	public int RpcId { get; set; }

	
	public string Account { get; set; }

	
	public string Password { get; set; }


}