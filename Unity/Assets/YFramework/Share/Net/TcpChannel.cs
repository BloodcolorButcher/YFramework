using System;
using System.Collections.Generic;
using System.IO;

public class TcpChannel : Channel
{
	
	//接收队列
	private readonly Queue<byte[]> _recQueue = new Queue<byte[]>();
	//生产队列
	private readonly Queue<byte[]> _prodQueue = new Queue<byte[]>();

	private readonly Queue<byte[]> _bufferQueue = new Queue<byte[]>();

	private object _obg = new object();

	/// <summary>
	/// 外部需要订阅的消息事件
	/// </summary>
	public Action<MsgType, byte[]> MsgEvent;
	//public byte bytes;
	public override void SendMsg(byte[] datas)
	{

	}
	public override void RecMsg(byte[] datas)
	{
		//把收到的消息放到缓存队列
		_recQueue.Enqueue(datas);
		ReceiveHandler();
	}
	public override void Updata()
	{
		if(_recQueue.Count < 1)
		{
			return;
		}
		SwitchQueue();
		ReceiveHandler();
	}

	void SwitchQueue()
	{
		lock( _obg )
		{
			Swap(_recQueue, _prodQueue);
		}
	}

	private void ReceiveHandler()
	{

		if(_prodQueue.Count < 1)
		{
			return;
		}

		//把所有的缓冲都塞到缓存中
		for( int i = 0; i < _prodQueue.Count; i++ )
		{
			_bufferQueue.Enqueue(_prodQueue.Dequeue());
		}

		//把所有的缓存都拿出来
		using (MemoryStream stream = new MemoryStream())
		{


			int tempCount = _bufferQueue.Count;
			for( int i = 0; i < tempCount; i++ )
			{
				stream.Write(_bufferQueue.Dequeue());
				//缓存数据拼接后的log
				// if(tempCount>1)
				// {
				// 	Debug.Log("stream读取到的数据"+BitConverter.ToString(stream.ToArray()));
				// }

			}

			//设置读取指针
			stream.Seek(0, SeekOrigin.Begin);
			stream.Position = 0;

			int left = 0;
			int right = (int)stream.Length;
			//如果收到的消息小于3直接把byte[]塞入
			while(left < right)
			{
				if(right - left < 2)
				{
					byte[] tempData = new byte[right - left];
					stream.Read(tempData, 0, right - left);
					_bufferQueue.Enqueue(tempData);
					break;
				}

				int len_h = stream.ReadByte();
				int len_l = stream.ReadByte();
				int length = YYProtolcol.Bytes2Int(len_h, len_l);
				
				left += 2;

                
				if(left + length > right)
				{
					left -= 2;
					stream.Position -= 2;
					// Debug.Log("left"+left +"length"+ length +"right"+ right);
					byte[] tempData = new byte[right - left];
					stream.Read(tempData, 0, right - left);
					_bufferQueue.Enqueue(tempData);
					// Debug.Log("接受的消息在下一次接收中" + BitConverter.ToString(tempData));
					break;
				}
				else
				{

					//main_cmd sub_cmd
					int main_cmd = stream.ReadByte();
					int sub_cmd = stream.ReadByte();

					//msg data
					byte[] tempdata = new byte[length - 2];
					stream.Read(tempdata, 0, length - 2);
					
					int cmd = YYProtolcol.Bytes2Int(main_cmd, sub_cmd);
					FireMsg(cmd, tempdata);
                    
				}
                
			}

		}
	}

	private void FireMsg(int type, byte[] data)
	{
		// Debug.Log(type);

		// MsgDic[type]?.Invoke(data);
		MsgEvent?.Invoke((MsgType)type, data);
	}

	private void Swap<T>(T t1, T t2)
	{
		T temp = t2;
		t2 = t1;
		t1 = temp;
	}
}