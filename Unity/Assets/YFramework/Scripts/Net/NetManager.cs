using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public static class NetManager
{

	//用来监听Socket  fd文件描述符；文件说明符
	public static Socket listenfd;

	//客户端Socket及状态信息
	public static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();

	//Select的检查列表
	static List<Socket> checkRead = new List<Socket>();

	public static void StartLoop(int listenPort)
	{
		//给监听socket listenfd初始化
		listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		//bind 为socket绑定ip和端口号
		IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
		IPEndPoint ipEp = new IPEndPoint(ipAdr, listenPort);
		listenfd.Bind(ipEp);
		//Listen 设置监听个数 , 指定队列中最多可容纳等待接收的连接数，0表示不限制
		listenfd.Listen(0);
		Debug.Log("[服务器]启动成功");
		//循环
		while(true)
		{
			ResetCheckRead(); //重置checkRead
			Socket.Select(checkRead, null, null, 1000);
			//检查可读对象
			for( int i = checkRead.Count - 1; i >= 0; i-- )
			{
				Socket s = checkRead[i];
				//如果是服务器监听socket收到消息代表，有新的成员加入
				if(s == listenfd)
				{
					ReadListenfd(s);
				}
				//代表着收到客户端的消息
				else
				{
					ReadClientfd(s);
				}
			}
		}
	}


	/// <summary>
	/// 填充用来检查select的类表
	/// 填充checkRead列表
	/// </summary>
	private static void ResetCheckRead()
	{
		//清空检查列表
		checkRead.Clear();
		//添加服务器监听socket
		checkRead.Add(listenfd);
		//添加服务器绑定的监听客户端的socket
		foreach(ClientState s in clients.Values)
		{
			checkRead.Add(s.workSocket);
		}
	}


	/// <summary>
	///  读取Listenfd socket收到的消息
	/// </summary>
	/// <param name="listenfd">服务器的监听Socket</param>
	public static void ReadListenfd(Socket listenfd)
	{
		try
		{
			Socket clientfd = listenfd.Accept();
			Debug.Log("新Socket成员:" + clientfd.RemoteEndPoint.ToString() + "加入");
			ClientState state = new ClientState();
			state.workSocket = clientfd;
			// state.lastPingTime = GetTimeStamp();
			clients.Add(clientfd, state);
		}
		catch (SocketException ex)
		{
			Debug.Log("新成员接收失败 Accept fail" + ex.ToString());
		}
	}
	//读取Clientfd
	public static void ReadClientfd(Socket clientfd)
	{

		ClientState state = clients[clientfd];

		int bytesRead;

		try
		{

			bytesRead = clientfd.Receive(state.buffer, 0, 1024, 0);
		}
		catch (SocketException ex)
		{
			Debug.Log("Receive SocketException " + ex.ToString());
			//Close(state);
			return;
		}
		//客户端关闭
		if(bytesRead <= 0)
		{
			Debug.Log("Socket Close " + clientfd.RemoteEndPoint.ToString() + "客户端退出了该连接！");
			Close(state);
			return;
		}

		else
		{

			//state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
			Debug.Log("收到消息");
			Console.WriteLine(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
			//BaseMessage x = new BaseMessage();
			//x.RpcId = 1234;
			//x.Account = "yangyue";
			//x.Password = "123";
			//byte[] bytes = ProtobufTool.Serialize<BaseMessage>(x);
			state.workSocket.BeginSend(state.buffer, 0, bytesRead, 0, null, null);
		}
		//消息处理
		//readBuff.writeIdx += count;
		////处理二进制消息
		//OnReceiveData(state);
		////移动缓冲区
		//readBuff.CheckAndMoveBytes();
	}
	/// <summary>
	/// 关闭一个客户端，关闭该客户端维护类的socket
	/// 从客户端监听列表移除
	/// </summary>
	/// <param name="state">客户端对应的消息维护类</param>
	public static void Close(ClientState state)
	{
		////消息分发
		//MethodInfo mei = typeof(EventHandler).GetMethod("OnDisconnect");
		//object[] ob = { state };
		//mei.Invoke(null, ob);
		////关闭
		state.workSocket.Close();
		clients.Remove(state.workSocket);

	}

	public static void Close()
	{
		listenfd.Close();
	}
}