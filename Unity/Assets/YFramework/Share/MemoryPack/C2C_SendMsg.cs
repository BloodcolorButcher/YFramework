
using MemoryPack;

[MemoryPackable]
public partial class C2C_SendMsg : IRequest
{

	public int RpcId { get; set; }
	
	public string msg { get; set; }
}