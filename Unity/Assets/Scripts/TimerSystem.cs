
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimerSystem
{
    #region 单例模式

    private static TimerSystem _instance = null;

    public static TimerSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TimerSystem();
            }

            return _instance;
        }
    }

    #endregion
    /// <summary>
    /// 构造函数
    /// </summary>
    private TimerSystem()
    {
        _currentFrameTimestamp = Timestamp();
        _timerDict = new Dictionary<int, TimerNode>();
        _autoIncreTimerID = 1;
        _cachedList = new List<TimerNode>();
        _removeList = new List<TimerNode>();
    }


    #region 时间戳

    private readonly DateTime _dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private DateTime _dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    
    /// <summary>
    /// 获取时间戳(毫秒)
    /// </summary>
    /// <returns></returns>
    public long Timestamp()
    {
        return (DateTime.Now.Ticks - this._dt1970.Ticks) / 10000;
    }

    /// <summary> 
    /// 根据时间戳获取时间 
    /// </summary>  
    public DateTime ToDateTime(long timeStamp)
    {
        return _dt.AddTicks(timeStamp * 10000);
    }
    #endregion

    //回调
    public delegate void TimerCallBack(object param);
    
    //TimerID 自增
    private int _autoIncreTimerID = 1;
    //当前的时间
    private long _currentFrameTimestamp = long.MinValue;

    //所有的计时器
    private Dictionary<int, TimerNode> _timerDict = null;
    //缓存中的计时器队列
    private List<TimerNode> _cachedList = null;
    //需要移除的计时器队列
    private List<TimerNode> _removeList = null;
    
    
    /// <summary>
    /// 更新当前时间
    /// </summary>
    public void Updata()
    {
        //更新当前时间戳
        _currentFrameTimestamp = Timestamp();
        if (_currentFrameTimestamp>_targetTime)
        {
            Debug.Log("时间提醒，到了定时的时间");
            _targetTime = long.MaxValue;
        }
        
        //把缓存中的计时器添加到总字典
        for( int i = 0; i < _cachedList.Count; i++ )
        {
            TimerNode timer = _cachedList[i];
            _timerDict.Add(timer.timerID, timer);
        }
        _cachedList.Clear();
        
        var iter = _timerDict.GetEnumerator();
        while(iter.MoveNext())
        {
            var timer = iter.Current.Value;
            //判断计时器是否需要进行移除，添加到队列去移除
            if(timer.isRemoved)
            {
                _removeList.Add(timer);
                continue;
            }
            //更新计时的时间
            // timer.elapseTime += deltaTime;
            //计时器到点了，执行回调，放到移除队列去
            if(_currentFrameTimestamp >=timer.targetTime )
            {
                //执行回调
                timer.callback(timer.param);
                //执行次数减1次
                timer.repeat--;
                //更新运行的时间
                timer.targetTime += timer.interval;
                //如果执行次数完成了就添加到移除队列去
                if(timer.repeat <= 0 && timer.isLoop!=true)
                {
                    timer.isRemoved = true;
                    _removeList.Add(timer);
                }
            }
        }
        //清空需要移除的计时器
        for( int i = 0; i < _removeList.Count; i++ )
        {
            TimerNode timer = _removeList[i];
            _timerDict.Remove(timer.timerID);
        }
        _removeList.Clear();
        
    }

    private long _targetTime = long.MaxValue;
    
    public void Schedule(int delay)
    {
        _targetTime = _currentFrameTimestamp + delay * 1000;
    }
    
    /// <summary>
    /// 执行定时器
    /// </summary>
    /// <param name="callback">设置回调函数</param>
    /// <param name="param">，在到点的时候回传到回调函数</param>
    /// <param name="isLoop">是否循环</param>
    /// <param name="repeat">执行的次数</param>
    /// <param name="duration">需要定时的时间（秒）</param>
    /// <param name="delay">延迟的时间（秒）</param>
    /// <returns></returns>
    public int Schedule(TimerCallBack callback, object param,bool isLoop ,int repeat, int interval, int delay = 0)
    {
        TimerNode timerNode = new TimerNode();
        timerNode.callback = callback;
        timerNode.param = param;
        timerNode.isLoop = isLoop;
        timerNode.repeat = repeat;
        timerNode.interval = interval*1000;
        timerNode.targetTime = delay*1000 +_currentFrameTimestamp;
        timerNode.isRemoved = false;
        timerNode.timerID = _autoIncreTimerID;
        _autoIncreTimerID++;
        _cachedList.Add(timerNode);
        return timerNode.timerID;
    }

    

    
    /// <summary>
    /// 计时器节点
    /// </summary>
    private class TimerNode
    {
		
        /// <summary>
        /// 回调
        /// </summary>
        public TimerCallBack callback;
        /// <summary>
        /// 间隔时间
        /// </summary>
        public long interval;

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool isLoop;
   
        /// <summary>
        /// 重复次数
        /// </summary>
        public int repeat;
		
        /// <summary>
        /// 目标时间
        /// </summary>
        public long targetTime;
		
        /// <summary>
        /// 用户传递的参数
        /// </summary>
        public object param;
		
        /// <summary>
        /// 是否已经移除掉了
        /// </summary>
        public bool isRemoved;
		
        /// <summary>
        /// Timer的ID
        /// </summary>
        public int timerID;
    }
}


