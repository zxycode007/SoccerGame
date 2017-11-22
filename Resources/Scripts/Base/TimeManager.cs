using UnityEngine;
using System.Collections;
using Net;
using System.Net;
using UnityEngine.UI;
using Game;
using Util;

/// <summary>
/// 时间管理器
/// 游戏更新暂时放这里控制
/// </summary>
public class TimeManager : MonoBehaviour {

    /// <summary>
    /// 当前渲染总帧数
    /// </summary>
    long m_curTicks = 0;
    /// <summary>
    /// 当前逻辑帧时间
    /// </summary>
    float m_curLogicFrameTime;
    /// <summary>
    /// 设定帧率 默认30帧
    /// </summary>
    float m_frameRatePerSecond = 30;
    float m_frameTime;

    float m_accTime = 0;

    void Awake()
    {
        //开始1秒每0.02秒重复执行Tick
        InvokeRepeating("Tick", 1, 0.02f);
        m_frameTime = 1.0f / m_frameRatePerSecond;
        m_curLogicFrameTime = 1.0f / m_frameRatePerSecond;
    }

    void Tick()
    {
        TimerHeap.Tick();
        FrameTimerHeap.Tick();
       
        
    }

    void Update()
    {
        m_accTime += Time.deltaTime;
        //只有大于设定的帧时间时才更新
        while (m_accTime > m_frameTime)
        {
            CurLogicFrameTime = m_frameTime;
            m_curTicks++;
            //做一次游戏更新
            GlobalClient.GameManager.Update();
            m_accTime = m_accTime - m_frameTime;
        }
        //if (m_accTime >= m_frameTime)
        //{
        //    //更新当前帧时间
        //    CurLogicFrameTime = m_accTime;
        //    ///作游戏更新
        //    GlobalClient.GameManager.Update();
        //    m_curTicks++;
        //    m_accTime = 0;
        //}
        
    }

    public long TickCount
    {
        get
        {
            return m_curTicks;
        }
    }
    
    public float CurLogicFrameTime
    {
        get
        {
            return m_curLogicFrameTime;
        }
        set
        {
            m_curLogicFrameTime = value;
        }
    }

    public float FrameTime
    {
        get
        {
            return m_frameTime;
        }
    }

    public float FrameRate
    {
        get
        {
            return m_frameRatePerSecond;
        }
        set
        {
            m_frameRatePerSecond = value;
            if (Mathf.Abs(m_frameRatePerSecond) < 10)
            {
                Debug.LogError("FrameRate太低");
                m_frameRatePerSecond = 10;
            }
            m_frameTime = 1 / m_frameRatePerSecond;
            
        }
    }

    public float GetDeltaTime()
    {
        return CurLogicFrameTime;
    }
}



