using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MsgHelper
{
	private CancellationTokenSource _cts = null;
	byte[] bytes = null;
	public MsgHelper()
	{
		_cts = new CancellationTokenSource();
	}
	/// <summary>
	/// 发送请求 超时时间为请求 间隔时间 请求次数
	/// </summary>
	/// <param name="sendEvent"></param>
	/// <param name="interval">两次发送信息的间隔时间(毫秒)</param>
	/// <param name="repeat">发送次数</param>
	/// <param name="recType">取消事件</param>
	/// <returns></returns>
	public async UniTask<byte[]> SendRequest(Action sendEvent, int interval, int repeat, MsgType recType)
	{

		_cts.CancelAfterSlim(interval*repeat);
		GameManager.ClientManager.AddMsgDic(recType, RecMsg);
		SendEvent(sendEvent, interval, repeat, _cts.Token).Forget();
		//等待   
		bool isCanceled = await WaitCallBack(_cts.Token).SuppressCancellationThrow();
		// bool isCanceled = await  Request().TimeoutWithoutException(TimeSpan.FromSeconds(5));
		if(isCanceled)
		{
			Debug.Log("超时取消了");
		}
		else
		{
			_cts.Cancel();
			Debug.Log("收到了消息");
		}
		_cts.Dispose();
		GameManager.ClientManager.RemMsgDic(recType, RecMsg);

		return bytes;
	}

	/// <summary>
	/// 重复执行的请求
	/// </summary>
	/// <param name="sendEvent">请求的具体内容</param>
	/// <param name="interval">请求的时间间隔</param>
	/// <param name="repeat">重复次数</param>
	/// <param name="cancellationToken">取消的协议</param>
	async UniTask SendEvent(Action sendEvent, int interval, int repeat, CancellationToken cancellationToken)
	{
		for( int i = 0; i < repeat; i++ )
		{
			sendEvent?.Invoke();
			await UniTask.Delay(interval, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);
		}

	}


	/// <summary>
	/// 等待回复
	/// </summary>
	/// <param name="cancellationToken">超时令牌</param>
	async UniTask WaitCallBack(CancellationToken cancellationToken)
	{
		await UniTask.WaitUntil(() =>
			{
				// Debug.Log("等待中");
				return bytes != null;
			},
			PlayerLoopTiming.Update,
			cancellationToken
			);

	}

	private void RecMsg(byte[] datas)
	{
		bytes = datas;

	}
}