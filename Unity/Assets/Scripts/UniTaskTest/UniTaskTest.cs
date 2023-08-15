using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
        m_questBtn.onClick.AddListener(RequsetEvent);
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
        // m_text.text = "请求事件";
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
        // _cts = new CancellationTokenSource();
        // _cts.CancelAfterSlim(TimeSpan.FromSeconds(5)); // 5sec timeout.
        bool isComplete = await Request();

        if(isComplete)
        {
           Debug.Log("完成了监听"); 
        }
    }
    async UniTask<bool> Request()
    {
        //()=>
       
        // await UniTask.WaitForFixedUpdate(cancellationToken);
        await UniTask.WaitUntil(()=>stop ==-1);
        return true;
    }
    
}
