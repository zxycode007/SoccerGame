﻿using UnityEngine;
using System.Collections;
using Util;
using Game;

/// <summary>
/// 游戏主循环
/// </summary>
public class MainLoop : MonoBehaviour
{
    /// <summary>
    /// 场景控制器
    /// </summary>
    private SceneController m_sceneController;
    /// <summary>
    /// 游戏环境上下文
    /// </summary>
    private GameContext m_context;
    void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        //开始1秒每0.02秒重复执行Tick
        
    }
    
    
    // Use this for initialization
    void Start()
    {
        m_context = new GameContext();
        m_sceneController = new SceneController();
    }

    // Update is called once per frame
    void Update()
    {
        m_sceneController.UpdateSceneState();
        
    }

    void Destory()
    {
        GlobalClient.NetWorkManager.Disconnect();
       
        Debug.Log("结束游戏");
    }

    void OnApplicationQuit()
    {
        Debug.Log("程序退出");
        GlobalClient.NetWorkManager.Disconnect();
    }
}
