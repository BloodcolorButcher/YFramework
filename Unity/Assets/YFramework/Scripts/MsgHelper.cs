
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class MsgHelper<T>
    {

        public MsgHelper()
        {
            t = default;
        }
        
        private T t;
        private int step = 0;
       
        public void Bind(Action<T> action)
        {
            T t = default;
            if (t ==null)
            {
                Debug.Log("null");
            }
            GameManager.ClientManager.AddMsgDic(MsgType.S2C_LoginMsg,RecLoginMsg2);
            C2S_LoginMsg c2SLoginMsg = new C2S_LoginMsg() {RpcId = 1, Account = "yangyue", Password = "yangyue" };
            var data = MemoryPackHelper.Serialize(c2SLoginMsg);
            var datas = YYProtolcol.TcpProtocol.MsgToBytes((int)MsgType.C2S_LoginMsg, data);
            
         
            GameManager.ClientManager.Send(datas);
        }
        private CancellationTokenSource _cts = null;
        private async void RequsetEvent()
        {
            _cts = new CancellationTokenSource();
            _cts.CancelAfterSlim(TimeSpan.FromSeconds(5)); // 5sec timeout.
     
            // var request = await UnityWebRequest.Get("http://foo").SendWebRequest().WithCancellation(_cts.Token);
            //
            // if(request.error)
            // {
            //     
            // }
            //var cancelTask = UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cts.Token);
            //var cancelTask = UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cts.Token);
            // int index = await UniTask.WhenAny(Request(),  UniTask.Delay(TimeSpan.FromSeconds(5)));
            // if(index ==0)
            // {
            //     Debug.Log("完成请求");
            // }
            // else
            // {
            //     Debug.Log("请求超时");
            // }

            //等待   
            bool isCanceled = await  Request(_cts.Token).SuppressCancellationThrow();
            // bool isCanceled = await  Request().TimeoutWithoutException(TimeSpan.FromSeconds(5));
            if(isCanceled)
            {
                Debug.Log("取消了");
          
            }
            else
            {
                Debug.Log("完成了监听"); 
            }
        }
        
        async UniTask Request()
        {
            //()=>
       
            // await UniTask.WaitForFixedUpdate(cancellationToken);
       
            await UniTask.WaitUntil(()=>
                {
                  
                    return t != null;
                    
                }
            );
            // WaitUntil拓展，指定某个值改变时触发
            // await UniTask.WaitUntilValueChanged(this, x =>
            // {
            //     return x.stop;
            // });
        }
        async UniTask Request(CancellationToken cancellationToken)
        {
            //()=>
       
            // await UniTask.WaitForFixedUpdate(cancellationToken);
       
            await UniTask.WaitUntil(()=>
                {
                    // Debug.Log("等待中");
                    return t != null;
           
            
                },PlayerLoopTiming.Update,cancellationToken
            );
            // WaitUntil拓展，指定某个值改变时触发
            // await UniTask.WaitUntilValueChanged(this, x =>
            // {
            //     return x.stop;
            // });
        }
        
        private void RecLoginMsg2(byte[] datas)
        {
            Debug.Log("触发了消息");
            try
            {
                t = MemoryPackHelper.DeserializeObject<T>(datas);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
            GameManager.ClientManager.RemMsgDic(MsgType.S2C_LoginMsg,RecLoginMsg2);
        
        }
    }
    
    //unitask 方法
    
    //超时器
    
