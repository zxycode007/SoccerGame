using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Game;

/// <summary>
/// 选择队伍场景
/// </summary>
public class SelectTeamSceneState : BaseSceneState {

    /// <summary>
    /// 队伍1的玩家列表
    /// </summary>
    List<Game.GamePlayer> Team1Players;
    /// <summary>
    /// 队伍2的玩家列表
    /// </summary>
    List<Game.GamePlayer> Team2Players;

    GameContext m_context;

    public SelectTeamSceneState(SceneController controller)
        : base(controller)
    {
        this.m_sceneName = "selectTeamScene";
        this.m_eState = ESceneState.E_SCENE_STATE_SELECT_TEAM;
        SceneManager.sceneLoaded += OnSceneLoaded;
        m_context = new GameContext();
    }

    public override void SceneStateBegin()
    {
        m_bRunning = true;
        Team1Players = new List<Game.GamePlayer>();
        Team2Players = new List<Game.GamePlayer>();
        GlobalClient.AddEventReceiver(Game.EventType.EVT_SELECTTEAM_PLAYER_READY, m_context);
        GlobalClient.AddEventReceiver(Game.EventType.EVT_SELECTTEAM_TEAM_UPDATE, m_context);
        m_context.SelectTeamTeamUpdateHandler += OnTeamUpdateHandler;
        m_context.SelectTeamPlayerReadyHandler += OnReadyHandler;
        
         
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == m_sceneName)
        {
            m_bLoaded = true;
        }

    }

    void OnReadyHandler(object sender, EventArgs arg)
    {
        SelectTeamPlayerReadyArg sarg = arg as SelectTeamPlayerReadyArg;
        if(sarg != null)
        {
            String msg = string.Format("{0} Ready to GameScene", sarg.player.PlayerName);
            Debug.Log(msg);
        }
       
    }

    void OnTeamUpdateHandler(object sender, EventArgs arg)
    {
        SelectTeamTeamUpdateArg sarg = arg as SelectTeamTeamUpdateArg;
        if(sarg != null)
        {
            Team1Players = sarg.team1;
            Team2Players = sarg.team2;
            Debug.Log("更新队伍");
        }
    }

     

    public override void SceneStateEnd()
    {
        //GlobalClient.RemoveEventReceiver(EventType.EVT_SELECTTEAM_PLAYER_READY, m_context);
        //GlobalClient.RemoveEventReceiver(EventType.EVT_SELECTTEAM_TEAM_UPDATE, m_context);
    }

    /// <summary>
    /// 处理玩家输入
    /// </summary>
    void HandleInput()
    {

    }

    public override void SceneStateUpdate()
    {
        HandleInput();
        if (GlobalClient.NetWorkManager != null && GlobalClient.NetWorkManager.isInitialized == true)
        {
            GlobalClient.NetWorkManager.Update();
        }
    }
}
