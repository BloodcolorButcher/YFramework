using System;
using System.Collections;
using System.Collections.Generic;
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

    
    private async void RequsetEvent()
    {
        stop = 0;
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
        int index = await UniTask.WhenAny(Request(),  UniTask.Delay(TimeSpan.FromSeconds(5)));
        if(index ==0)
        {
            Debug.Log("完成请求");
        }
        else
        {
            Debug.Log("请求超时");
        }

        //等待   
        // bool isCanceled = await  Request(_cts.Token).SuppressCancellationThrow();
        // bool isCanceled = await  Request().TimeoutWithoutException(TimeSpan.FromSeconds(5));
        // if(isCanceled)
        // {
        //     Debug.Log("取消了");
        //   
        // }
        // else
        // {
        //     Debug.Log("完成了监听"); 
        // }
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
    
}
