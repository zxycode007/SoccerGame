using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

namespace Game
{
    public class LoginSceneState : BaseSceneState
    {

        GameContext m_context;
        GamePlayer m_gamePlayer;


        public LoginSceneState(SceneController controller)
            : base(controller)
        {
            this.m_sceneName = "LoginScene";
            this.m_eState = ESceneState.E_SCENE_STATE_LOGIN;
            SceneManager.sceneLoaded += OnSceneLoaded;
            m_context = new GameContext();
        }

        public override void SceneStateBegin()
        {
            m_bRunning = true;
            GlobalClient.AddEventReceiver(EventType.EVT_USER_LOGIN, m_context);
            GlobalClient.AddEventReceiver(EventType.EVT_SERVER_ON_CONNECT, m_context);
            GlobalClient.AddEventReceiver(EventType.EVT_TO_TEAM_SELECT, m_context);
            m_context.UserLoginHandler += OnLoginHandler;
            m_context.ServerOnConnectHandler += OnLoginToServerHandler;
            m_context.ToSelectTeamHandler += OnToSelectTeam;
            m_bLoaded = true;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == m_sceneName)
            {
                m_bLoaded = true;
            }

        }

        void OnLoginToServerHandler(object sender, EventArgs arg)
        {
            ServerOnConnectArg sarg = arg as ServerOnConnectArg;
            if (sarg != null)
            {
                m_gamePlayer.PlayerID = sarg.ClientID;
                GlobalClient.NetWorkManager.SendPlayerInfo(m_gamePlayer);

            }
        }

        void OnToSelectTeam(object sender, EventArgs arg)
        {
            ToSelectTeamArg targ = arg as ToSelectTeamArg;
            if (targ != null)
            {
                GamePlayer playerInfo = targ.playerInfo;
                GlobalClient.PlayerManager.Self = playerInfo;
                m_controller.SetSceneState(new SelectTeamSceneState(m_controller));
            }

        }


        void OnLoginHandler(object sender, EventArgs arg)
        {
            Debug.Log("User Login");
            UserLoginArg uarg = arg as UserLoginArg;
            if (m_gamePlayer == null)
            {
                m_gamePlayer = new GamePlayer();
            }
            if (uarg != null)
            {
                GlobalClient.hostIP = uarg.info.hostIp;
                GlobalClient.TCP_Port = uarg.info.tcpPort;
                GlobalClient.UDP_Port = uarg.info.udpPort;
                m_gamePlayer.PlayerName = uarg.info.userName;
                if (uarg.info.userIconPath == null)
                {
                    uarg.info.userIconPath = "iconH001";
                }
                m_gamePlayer.PlayerIcon = uarg.info.userIconPath;
                if (GlobalClient.NetWorkManager != null)
                {
                    GameManager.instance.Init();
                    GlobalClient.NetWorkManager.SetIpInfo(GlobalClient.HostIP, GlobalClient.TCP_Port, GlobalClient.UDP_Port);
                    GlobalClient.NetWorkManager.InitClient();
                    GlobalClient.NetWorkManager.Connect();
                }
                //m_controller.SetSceneState(new GameSceneState(m_controller));

            }
        }


        public override void SceneStateEnd()
        {
            // GlobalClient.RemoveEventReceiver(EventType.EVT_USER_LOGIN, m_context);
        }


        public override void SceneStateUpdate()
        {
            if (GlobalClient.NetWorkManager != null && GlobalClient.NetWorkManager.isInitialized == true)
            {
                GlobalClient.NetWorkManager.Update();
            }
        }
    }
}


