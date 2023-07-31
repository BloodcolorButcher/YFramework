using MemoryPack;

[MemoryPackable()]
public class S2C_LoginMsg : IResponse
{
	
	public int Error { get; set; }

	
	public string Message { get; set; }

	
	public int RpcId { get; set; }
    
 
}