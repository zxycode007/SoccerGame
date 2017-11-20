using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Game
{

    public enum EventType
    {

        //操作事件
        EVT_MOUSE_FIRE1,
        EVT_MOUSE_FIRE2,
        EVT_JOYSTICK_BEGIN_DRAG,
        EVT_JOYSTICK_END_DRAG,
        EVT_SHOT_BUTTON_DOWN,
        EVT_SHOT_BUTTON_UP,
        EVT_PASS_BUTTON_DOWN,
        EVT_PASS_BUTTON_UP,
        EVT_STOLEN_BUTTON_DOWN,
        EVT_STOLEN_BUTTON_UP,


        //单位事件
        EVT_UNIT_MOVE_BEGIN,
        EVT_UNIT_MOVE_END,

        //系统事件
        EVT_KEYFRAME_UPDATE,   //关键帧更新
        EVT_SELECTTEAM_PLAYER_READY,   //选择队伍玩家准备好了
        EVT_SELECTTEAM_TEAM_UPDATE,    //队伍更新
        EVT_USER_LOGIN,
        EVT_TO_TEAM_SELECT,            //去选择队伍
        //网络事件
        EVT_SERVER_ON_CONNECT             //服务器连接上了


    }


    public class MouseFire1EvtArg : EventArgs
    {


    }

    public class MouseFire2EvtArg : EventArgs
    {
        //public RaycastHit hit;
    }

    public class JoyStickBeginDrag : EventArgs
    {

    }

    public class UnitMoveBeginEvtArg : EventArgs
    {
        public Game.Actor actor;
    }

    public class UnitMoveEndEvtArg : EventArgs
    {
        public Game.Actor actor;
    }

    public class KeyFrameUpdateArg : EventArgs
    {

    }

    public class SelectTeamPlayerReadyArg : EventArgs
    {
        public Game.GamePlayer player;

    }

    public class SelectTeamTeamUpdateArg : EventArgs
    {
        public List<Game.GamePlayer> team1;
        public List<Game.GamePlayer> team2;

    }

    public class UserLoginArg : EventArgs
    {
        public LoginInfo info;
    }

    public class ServerOnConnectArg : EventArgs
    {
        public int PlayerID = -1;
    }

    public class ToSelectTeamArg : EventArgs
    {
        public Game.GamePlayer playerInfo;
    }

    public class GameContext : object
    {



        //事件


        public event EventHandler MouseFire1Handler;

        public event EventHandler MouseFire2Handler;

        public event EventHandler UnitMoveBeginHandler;

        public event EventHandler UnitMoveEndHandler;

        public event EventHandler KeyFrameUpdateHandler;

        public event EventHandler SelectTeamPlayerReadyHandler;

        public event EventHandler SelectTeamTeamUpdateHandler;

        public event EventHandler UserLoginHandler;

        public event EventHandler ServerOnConnectHandler;

        public event EventHandler ToSelectTeamHandler;


        public void FireEvent(object sender, EventType type, EventArgs arg, GameContext receiver)
        {
            if (GlobalClient.EventReceivers == null)
                return;
            if (GlobalClient.EventReceivers.ContainsKey(type))
            {
                HashSet<GameContext> set = GlobalClient.EventReceivers[type];
                foreach (GameContext context in set)
                {
                    if (context == receiver)
                    {
                        //Debug.LogWarning ("找到注册对象receiver" + receiver.ToString ());
                        if (context != null)
                        {
                            context.OnEvent(sender, type, arg);
                        }
                    }
                }
            }
        }


        public void FireEvent(object sender, EventType type, EventArgs arg)
        {
            if (GlobalClient.EventReceivers == null)
                return;
            if (GlobalClient.EventReceivers.ContainsKey(type))
            {
                HashSet<GameContext> set = GlobalClient.EventReceivers[type];
                foreach (GameContext context in set)
                {
                    if (context != null)
                    {
                        context.OnEvent(sender, type, arg);
                    }
                }
            }

        }


        public void OnEvent(object sender, EventType type, EventArgs arg)
        {
            switch (type)
            {

                case (EventType.EVT_MOUSE_FIRE1):
                    {
                        if (MouseFire1Handler != null)
                        {
                            MouseFire1Handler(sender, arg);
                        }
                        break;
                    }
                case (EventType.EVT_MOUSE_FIRE2):
                    {
                        if (MouseFire2Handler != null)
                        {
                            MouseFire2Handler(sender, arg);
                        }
                        break;
                    }
                case (EventType.EVT_UNIT_MOVE_BEGIN):
                    {
                        if (UnitMoveBeginHandler != null)
                        {
                            UnitMoveBeginHandler(sender, arg);
                        }
                        break;
                    }
                case (EventType.EVT_UNIT_MOVE_END):
                    {
                        if (UnitMoveEndHandler != null)
                        {
                            UnitMoveEndHandler(sender, arg);
                        }
                        break;
                    }
                case (EventType.EVT_KEYFRAME_UPDATE):
                    {
                        if (KeyFrameUpdateHandler != null)
                        {
                            KeyFrameUpdateHandler(sender, arg);
                        }
                        break;
                    }
                case (EventType.EVT_SELECTTEAM_PLAYER_READY):
                    {
                        if (SelectTeamPlayerReadyHandler != null)
                        {
                            SelectTeamPlayerReadyHandler(sender, arg);
                        }
                        break;
                    }
                case (EventType.EVT_SELECTTEAM_TEAM_UPDATE):
                    {
                        if (SelectTeamTeamUpdateHandler != null)
                        {
                            SelectTeamTeamUpdateHandler(sender, arg);
                        }
                        break;
                    }
                case (EventType.EVT_USER_LOGIN):
                    {
                        if (UserLoginHandler != null)
                        {
                            UserLoginHandler(sender, arg);
                        }
                        break;
                    }
                case (EventType.EVT_SERVER_ON_CONNECT):
                    {
                        if (ServerOnConnectHandler != null)
                        {
                            ServerOnConnectHandler(sender, arg);
                        }
                        break;
                    }
                case (EventType.EVT_TO_TEAM_SELECT):
                    {
                        if (ToSelectTeamHandler != null)
                        {
                            ToSelectTeamHandler(sender, arg);
                        }
                        break;
                    }


            }

        }



    }


}