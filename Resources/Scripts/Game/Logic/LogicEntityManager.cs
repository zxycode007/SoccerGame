using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
     

    /// <summary>
    /// 逻辑层管理器
    /// </summary>
    public class LogicEntityManager
    {      
        //主控玩家，主场
        public bool isHostPlayer = false;
        //游戏对象列表
        public List<LogicEntity> entities = new List<LogicEntity>();
        public LogicEntity curObj;
        private GameContext context;

        
        /// <summary>
        /// 添加关键帧数据
        /// </summary>
        /// <param name="keydatalist">关键帧数据</param>
        public void  AddKeyData(List<KeyData> keydatalist, int frameCount)
        {
                     
              
        }

        /// <summary>
        /// 来自本地客户端的输入命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="param">参数</param>
        public void InputCmd(Cmd cmd, string param)
        {
            //封装关键帧数据
            KeyData keyData = new KeyData(cmd, param, GlobalClient.NetWorkManager.ClientID);
            //100毫秒向服务器发一次
            GlobalClient.NetWorkManager.AddKeyPack(keyData);
        }

        /// <summary>
        /// 获取玩家控制对象
        /// </summary>
        /// <param name="ID">玩家对象ID</param>
        /// <returns></returns>
        public LogicEntity GetPlayerObj(int id)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                if (entities[i].ID == id)
                {
                    return entities[i];
                }
            }
            return null;
        }

        public void DoCmd(Cmd cmd, string param, int roleId)
        {
            switch(cmd)
            {
                case Cmd.UseSkill:
                    for (int i = 0; i < entities.Count; ++i)
                    {
                        if(entities[i].mCharData.roleId == roleId)
                        {
                            (entities[i] as CreatureEntity).DoSkill(int.Parse(param));
                        }
                    }
                    break;
                case Cmd.Move:
                    break;
                case Cmd.Turn:
                    break;
                case Cmd.UnitMoveBegin:
                    {
                        for(int i=0; i<entities.Count; i++)
                        {
                            //单位名字和玩家ID要对应上
                            if(entities[i].mCharData.roleId == roleId && entities[i].mCharData.name == param)
                            {
                                if (GlobalClient.GameManager.ViewManager.ViewObjMap.ContainsKey(entities[i].ID))
                                {
                                    Actor ac = GlobalClient.GameManager.ViewManager.ViewObjMap[entities[i].ID].actor;
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
                        for (int i = 0; i < entities.Count; i++)
                        {
                            //单位名字和玩家ID要对应上
                            if (entities[i].mCharData.roleId == roleId && entities[i].mCharData.name == param)
                            {

                                if (GlobalClient.GameManager.ViewManager.ViewObjMap.ContainsKey(entities[i].ID))
                                {
                                    Actor ac = GlobalClient.GameManager.ViewManager.ViewObjMap[entities[i].ID].actor;
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
            DoCmd(keyData.cmd, keyData.data, keyData.clientId);
        }

        public void Init()
        {
            entities = new List<LogicEntity>();
             
            context = new GameContext();
        }

        public void Update()
        {
            
            //游戏对象本地更新
            for (int i = 0; i < entities.Count; ++i)
            {
                entities[i].Update();
            }
           
        }
    }
}

