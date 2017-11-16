using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
     

    /// <summary>
    /// 逻辑层，逻辑更新在这里处理
    /// </summary>
    public class GameMap
    {      
        public int curRoleId = 0;
        //主控玩家，主场
        public bool isHostPlayer = false;
        //游戏对象列表
        public List<GameObj> gameObjList = new List<GameObj>();
        public GameObj curObj;
        private NetManager _netManager;
        private GameContext context;

        
        /// <summary>
        /// 添加关键帧数据
        /// </summary>
        /// <param name="keydatalist">关键帧数据</param>
        public void  AddKeyData(List<KeyData> keydatalist, int frameCount)
        {
                     
              
        }

        public NetManager netManager
        {
            get
            {
                return _netManager;
            }
        }

        public void InitNet(string ip, int tcpPort, int udpPort)
        {
            _netManager = GlobalClient.NetWorkManager;
            netManager.SetIpInfo(ip, tcpPort, udpPort);
            netManager.InitClient();
        }

        /// <summary>
        /// 来自本地客户端的输入命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="param">参数</param>
        public void InputCmd(Cmd cmd, string param)
        {
            //封装关键帧数据
            KeyData keyData = new KeyData(cmd, param, curRoleId);
            //100毫秒向服务器发一次
            _netManager.AddKeyPack(keyData);
        }

        /// <summary>
        /// 获取玩家控制对象
        /// </summary>
        /// <param name="ID">玩家对象ID</param>
        /// <returns></returns>
        public GameObj GetPlayerObj(int id)
        {
            for (int i = 0; i < gameObjList.Count; ++i)
            {
                if (gameObjList[i].ID == id)
                {
                    return gameObjList[i];
                }
            }
            return null;
        }

        public void DoCmd(Cmd cmd, string param, int roleId)
        {
            switch(cmd)
            {
                case Cmd.UseSkill:
                    for (int i = 0; i < gameObjList.Count; ++i)
                    {
                        if(gameObjList[i].mCharData.roleId == roleId)
                        {
                            (gameObjList[i] as Player).DoSkill(int.Parse(param));
                        }
                    }
                    break;
                case Cmd.Move:
                    break;
                case Cmd.Turn:
                    break;
                case Cmd.UnitMoveBegin:
                    {
                        for(int i=0; i<gameObjList.Count; i++)
                        {
                            //单位名字和玩家ID要对应上
                            if(gameObjList[i].mCharData.roleId == roleId && gameObjList[i].mCharData.name == param)
                            {
                                if (GlobalClient.GameManager.viewMap.ViewObjMap.ContainsKey(gameObjList[i].ID))
                                {
                                    Actor ac = GlobalClient.GameManager.viewMap.ViewObjMap[gameObjList[i].ID].actor;
                                    UnitMoveBeginEvtArg arg = new UnitMoveBeginEvtArg();
                                    arg.actor = ac;
                                    context.FireEvent(this, EventType.EVT_UNIT_MOVE_BEGIN, arg);
                                }
                                
                            }
                        }

                    }
                    break;
                case Cmd.UnitMoveEnd:
                    {
                        for (int i = 0; i < gameObjList.Count; i++)
                        {
                            //单位名字和玩家ID要对应上
                            if (gameObjList[i].mCharData.roleId == roleId && gameObjList[i].mCharData.name == param)
                            {

                                if (GlobalClient.GameManager.viewMap.ViewObjMap.ContainsKey(gameObjList[i].ID))
                                {
                                    Actor ac = GlobalClient.GameManager.viewMap.ViewObjMap[gameObjList[i].ID].actor;
                                    UnitMoveEndEvtArg arg = new UnitMoveEndEvtArg();
                                    arg.actor = ac;
                                    context.FireEvent(this, EventType.EVT_UNIT_MOVE_END, arg);
                                }
                            }
                        }
                    }
                    break;
                default:
                    Debug.LogError("无效命令");
                    break;
            }
        }

        /// <summary>
        /// 执行服务器下发的KeyData
        /// </summary>
        /// <param name="keyData"></param>
        public void DoCmd(KeyData keyData)
        {
            //Debug.LogError("执行关键帧 "+keyData.ToString());
            DoCmd(keyData.cmd, keyData.data, keyData.roleId);
        }

        public void Init()
        {
            gameObjList = new List<GameObj>();
             
            context = new GameContext();
        }

        public void Update()
        {
            
            //游戏对象本地更新
            for (int i = 0; i < gameObjList.Count; ++i)
            {
                gameObjList[i].Update();
            }
           
        }
    }
}

