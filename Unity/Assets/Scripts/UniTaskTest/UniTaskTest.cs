using System;

using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public struct MyStruct
{
    public int Num;
    public string Name;
}

public class UniTaskTest : MonoBehaviour
{
    [SerializeField]
    private Button m_questBtn, m_responseBtn,m_cancelBtn;
    [SerializeField]
    private Text m_text;

    private CancellationTokenSource _cts = null;
    private int stop = 0;
    // Start is called before the first frame update
    void Start()
    {
        MyStruct myStruct = new MyStruct() { Name = "yy", Num = 43 };
        MyStruct myStruct2 = myStruct;
        Debug.Log(myStruct2.Name);
        Debug.Log(myStruct2.Num);
        m_questBtn.onClick.AddListener(QuestBtnClicked);
        m_responseBtn.onClick.AddListener(ResponseBtnClicked);
        m_cancelBtn.onClick.AddListener(CancelBtnClicked);
    }
    private void CancelBtnClicked()
    {
        if(_cts != null)
        {
            _cts.Cancel();
            m_text.text += "取消成功";
        }
        else
        {
            m_text.text += "取消失败";
        }
    }

    private void QuestBtnClicked()
    {
        m_text.text = "请求事件";
         RequsetEvent();
        // DoSomeThing();
        // UniTask.CompletedTask( RequsetEvent());
    }
    private void ResponseBtnClicked()
    {
        m_text.text = "响应事件";
        stop = -1;

    }
   

    // Update is called once per frame
    void Update()
    {
        
    }

    private async void DoSomeThing()
    {
        // Debug.Log(1);
        // await UniTask.Delay(1000);
        // Debug.Log(2);
        // await UniTask.Delay(1000);
        // Debug.Log(3);
        await DoSomeThing2();
        // Debug.Log("第二次调用");
        // await DoSomeThing2();
    }
    
    private async UniTask DoSomeThing2()
    {
        var task = UniTask.DelayFrame(10);
        await  task.ToAsyncLazy();
        await task.ToAsyncLazy(); // 寄了, 抛出异常
    }
    
    
    private async void RequsetEvent()
    {
        stop = 0;
        _cts = new CancellationTokenSource();
        _cts.CancelAfterSlim(TimeSpan.FromSeconds(5)); // 5sec timeout.
        var _cts2 = new CancellationTokenSource();
        var linCancelToken = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, _cts2.Token);
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

        Request2(_cts.Token).Forget();
        //等待   
        bool isCanceled = await  Request(_cts.Token).SuppressCancellationThrow();
         // bool isCanceled = await  Request().TimeoutWithoutException(TimeSpan.FromSeconds(5));
        if(isCanceled)
        {
            Debug.Log("取消了");
          
        }
        else
        {
            _cts.Cancel();_cts.Dispose();
            Debug.Log("完成了监听"); 
        }
    }
    async UniTask Request()
    {
        //()=>
       
        // await UniTask.WaitForFixedUpdate(cancellationToken);
       
        await UniTask.WaitUntil(()=>
        {
            Debug.Log("等待中");
            return stop == -1;
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
                Debug.Log("等待中");
                return stop == -1;
           
            
            },PlayerLoopTiming.Update,cancellationToken
        );
        // WaitUntil拓展，指定某个值改变时触发
        // await UniTask.WaitUntilValueChanged(this, x =>
        // {
        //     return x.stop;
        // });
    }
    
    async UniTask Request2(CancellationToken cancellationToken)
    {
        //()=>
       
        // await UniTask.WaitForFixedUpdate(cancellationToken);
       
        Debug.Log("第一次发送");
        await UniTask.Delay(1000, DelayType.DeltaTime,PlayerLoopTiming.Update,cancellationToken);
        Debug.Log("第二次发送");
        await UniTask.Delay(1000, DelayType.DeltaTime,PlayerLoopTiming.Update,cancellationToken);
        Debug.Log("第三次发送");
        // WaitUntil拓展，指定某个值改变时触发
        // await UniTask.WaitUntilValueChanged(this, x =>
        // {
        //     return x.stop;
        // });
    }
    
}
