using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Util;
using System;

public class GameSceneState : BaseSceneState
{

    private GameContext m_context;
 
    public GameSceneState(SceneController controller)
        : base(controller)
		{
			this.m_sceneName = "gameScene1";
			this.m_eState = ESceneState.E_SCENE_STATE_GAME;
			SceneManager.sceneLoaded += OnSceneLoaded;
            m_context = new GameContext();
            UnityEngine.Random.InitState(1);
		
		}

    public override void SceneStateBegin()
    {
        m_bRunning = true;
        
    }


    public void OnQuitGame()
    {
        GlobalClient.NetWorkManager.Disconnect();
        Application.Quit();
    }

     

    void OnDestroy()
    {
        
    }

    
		

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == m_sceneName)
        {
            Debug.Log("场景加载完成!");
            m_bLoaded = true;
        }
    }

    public override void SceneStateEnd()
    {

    }

    public override void SceneStateUpdate()
    {
       
    }
}
