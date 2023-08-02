using MemoryPack;

/// <summary>
/// 需要序列化与反序列化
/// </summary>
public static class MemoryPackHelper
{
	/// <summary>
	/// 序列化转化为byte[]
	/// </summary>
	/// <param name="t"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static byte[] Serialize<T>(T t)
	{
		return MemoryPackSerializer.Serialize(t);
	}

	/// <summary>
	/// 从byte[]转化为类
	/// </summary>
	/// <param name="data"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static T DeserializeObject<T>(byte[] data)
	{
		T t = MemoryPackSerializer.Deserialize<T>(data);
		return t;
	}
}