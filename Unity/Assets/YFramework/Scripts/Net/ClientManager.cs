using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClientManager
{
	
 //定义套接字
    static Socket socket;

    
    //写入队列
    //static Queue<ByteArray> writeQueue;
    //是否正在连接
    static bool isConnecting = false;

    //是否正在关闭
    //static bool isClosing = false;
    //收到消息后组成一个合成信息

    private static byte[] buffer = new byte[1024];


    ////消息列表长度
    //static int msgCount = 0;
    ////每一次Update处理的消息量
    readonly static int MAX_MESSAGE_FIRE = 10;
    //是否启用心跳
    public static bool isUsePing = true;

    //心跳间隔时间
    public static int pingInterval = 30;
    //上一次发送PING的时间
    //static float lastPingTime = 0;
    ////上一次收到PONG的时间
    //static float lastPongTime = 0;

   

   


    //连接
    public static void Connect(string ip, int port)
    {
     
        //状态判断
        if (socket != null && socket.Connected)
        {
            Debug.Log("服务器连接失败，已经连接上服务器！");
            return;
        }

        if (isConnecting)
        {
            Debug.Log("服务器连接失败, 正在连接中！");
            return;
        }

        //初始化成员
        InitState();
        //参数设置
        socket.NoDelay = true;
        //Connect
        isConnecting = true;
        socket.BeginConnect(ip, port, ConnectCallback, socket);
    }

    //初始化状态
    private static void InitState()
    {
        //Socket
        socket = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
        //接收缓冲区
        //  readBuff = new ByteArray();
        //写入队列
        // writeQueue = new Queue<ByteArray>();
        //是否正在连接
        isConnecting = false;
        //是否正在关闭
        //isClosing = false;
        //消息列表
        //  msgList = new List<MsgBase>();
        //消息列表长度
        // msgCount = 0;
        //上一次发送PING的时间
        // lastPingTime = Time.time;
        //上一次收到PONG的时间
        // lastPongTime = Time.time;
        //监听PONG协议
        //if (!msgListeners.ContainsKey("MsgPong"))
        //{
        //    AddMsgListener("MsgPong", OnMsgPong);
        //}
    }

    //Connect 连接回调
    private static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket) ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("成功连接到服务器 ");
            //FireEvent(NetEvent.ConnectSucc, "");
            isConnecting = false;
            //开始接收
            socket.BeginReceive(buffer, 0, 1024, 0, ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Connect fail " + ex.ToString());
            //FireEvent(NetEvent.ConnectFail, ex.ToString());
            isConnecting = false;
        }
    }


    //关闭连接
    public static void Close()
    {
        //状态判断
        if (socket == null || !socket.Connected)
        {
            return;
        }

        if (isConnecting)
        {
            return;
        }

        //还有数据在发送
        //if (writeQueue.Count > 0)
        //{
        //    isClosing = true;
        //}
        //没有数据在发送
        //else
        //{
       
        socket.Close();
        
        //    FireEvent(NetEvent.Close, "");
        //}
    }

    //发送数据
    //public static void Send(MsgBase msg)
    //{
    //	//状态判断
    //	if (socket == null || !socket.Connected)
    //	{
    //		return;
    //	}
    //	if (isConnecting)
    //	{
    //		return;
    //	}
    //	if (isClosing)
    //	{
    //		return;
    //	}
    //	//数据编码
    //	byte[] nameBytes = MsgBase.EncodeName(msg);
    //	byte[] bodyBytes = MsgBase.Encode(msg);
    //	int len = nameBytes.Length + bodyBytes.Length;
    //	byte[] sendBytes = new byte[2 + len];
    //	//组装长度
    //	sendBytes[0] = (byte)(len % 256);
    //	sendBytes[1] = (byte)(len / 256);
    //	//组装名字
    //	Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
    //	//组装消息体
    //	Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);
    //	//写入队列
    //	ByteArray ba = new ByteArray(sendBytes);
    //	int count = 0;  //writeQueue的长度
    //	lock (writeQueue)
    //	{
    //		writeQueue.Enqueue(ba);
    //		count = writeQueue.Count;
    //	}
    //	//send
    //	if (count == 1)
    //	{
    //		socket.BeginSend(sendBytes, 0, sendBytes.Length,
    //			0, SendCallback, socket);
    //	}
    //}

    //Send回调
    //public static void SendCallback(IAsyncResult ar)
    //{

    //	//获取state、EndSend的处理
    //	Socket socket = (Socket)ar.AsyncState;
    //	//状态判断
    //	if (socket == null || !socket.Connected)
    //	{
    //		return;
    //	}
    //	//EndSend
    //	int count = socket.EndSend(ar);
    //	//获取写入队列第一条数据            
    //	ByteArray ba;
    //	lock (writeQueue)
    //	{
    //		ba = writeQueue.First();
    //	}
    //	//完整发送
    //	ba.readIdx += count;
    //	if (ba.length == 0)
    //	{
    //		lock (writeQueue)
    //		{
    //			writeQueue.Dequeue();
    //			ba = writeQueue.First();
    //		}
    //	}
    //	//继续发送
    //	if (ba != null)
    //	{
    //		socket.BeginSend(ba.bytes, ba.readIdx, ba.length,
    //			0, SendCallback, socket);
    //	}
    //	//正在关闭
    //	else if (isClosing)
    //	{
    //		socket.Close();
    //	}
    //}


    //Receive回调
    public static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket) ar.AsyncState;
            //获取接收数据长度
            int bytesRead = socket.EndReceive(ar);

            if (bytesRead > 0)
            {
                //解析收到的消息
                Debug.Log("收到消息");
                
                var data = new byte[bytesRead];
                Array.Copy(buffer,data,bytesRead);
                // IRequest request = MemoryPackHelper.DeserializeObject<IRequest>(data);
                // Debug.Log(request.RpcId);
                // if(request.RpcId ==1)
                // {
                //     S2C_LoginMsg s2CLoginMsg = MemoryPackHelper.DeserializeObject<S2C_LoginMsg>(data);
                //     Debug.Log("id"+ s2CLoginMsg.RpcId +"error:"+s2CLoginMsg.Error +"msg:"+s2CLoginMsg.Message);
                // }
                // else
                {
                    C2C_SendMsg c2CSendMsg =  MemoryPackHelper.DeserializeObject<C2C_SendMsg>(data);
                    Debug.Log("id"+c2CSendMsg.RpcId + "msg:"+c2CSendMsg.msg);
                }
              
                // Debug.Log(Encoding.UTF8.GetString(buffer,0,bytesRead));
                /*GetString(state.buffer, 0, bytesRead)
                 //ProtobufTool.Serialize<BaseMessage>(x);
                 //Debug.Log(bytes.Length);
                 //IMessage bm = ProtobufTool.Deserialize("BaseMessage" ,bytes);
                 //C2S_LoginMsg t2 = (C2S_LoginMsg) bm;
                 //Debug.LogFormat("id: {0}用户名：{1}密码：{2}", t2.RpcId, t2.Account, t2.Password);

                 sb.Append(Encoding.UTF8.GetString(receiveBuf, 0, bytesRead));

                 Console.WriteLine();
                 response = sb.ToString();
                 Debug.Log(response);*/

            }
            else
            {
            }

           
            socket.BeginReceive(buffer, 0, 1024, 0, ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Receive fail" + ex.ToString());
        }
    }

    //发送数据
    public static void Send(string str)
    {
        byte[] sendbuf = Encoding.UTF8.GetBytes(str);
        Debug.Log(Encoding.UTF8.GetString(sendbuf));
        socket.BeginSend(sendbuf, 0, sendbuf.Length, 0, null, null);
        //socket.Send(sendbuf)
    }
    //发送数据
    public static void Send(byte[] sendbuf)
    {
        socket.BeginSend(sendbuf, 0, sendbuf.Length, 0, null, null);  
    }

    public static void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket) ar.AsyncState;

            if (client == null || !client.Connected)
            {
                return;
            }

            // Complete sending the data to the remote device.  
            int bytesSent = client.EndSend(ar);
            Debug.LogFormat("成功发送 {0} 个字节到服务器.", bytesSent);

            // Signal that all bytes have been sent.  
        }
        catch (Exception e)
        {
            //socket.Close();

            Debug.Log(e.ToString());
        }
    }
 



  


 

    
}