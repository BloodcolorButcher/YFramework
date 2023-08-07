using MemoryPack;

[MemoryPackable]
public partial class S2C_LoginMsg : IResponse
{
	public MsgType MsgType { get; set;}
	public int Error { get; set; }

	
	public string Message { get; set; }

	
	public int RpcId { get; set; }
    
 
}