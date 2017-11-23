using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MoveCommand : BaseCommand
    {
        Quaternion m_direction;
        Vector3 m_target;

         public MoveCommand():base()
        {
             
            m_commandName = "MoveCommand";
            m_commandType = ECommandType.Move;
        }

         public MoveCommand(int playerID)
             : base(playerID)
        {
            m_commandName = "MoveCommand";
            m_commandType = ECommandType.Move;
            m_direction = Quaternion.identity;
        }

         public MoveCommand(int playerID, Quaternion dir)
             : base(playerID)
         {
             m_commandName = "MoveCommand";
             m_commandType = ECommandType.Move;
             m_direction = dir;
         }

         public MoveCommand(int playerID, float angle)
             : base(playerID)
         {
             m_commandName = "MoveCommand";
             m_commandType = ECommandType.Move;
             CreatureEntity playerObj = GlobalClient.GameManager.LogicManager.GetEntityByID(m_clientID) as CreatureEntity;
             if(playerObj != null)
             {
                 Quaternion rot = Quaternion.Euler(Vector3.up * angle);
                 m_direction = rot;
                
             }
         }

        // Use this for initialization
        public override void run()
        {
            base.run();
            CreatureEntity playerObj = GlobalClient.GameManager.LogicManager.GetEntityByID(m_clientID) as CreatureEntity;
            if(playerObj != null)
            {
                playerObj.Rotation = m_direction;
                //到达终点命令结束，自动切换站立状态
                //if(Vector3.Distance(playerObj.Position, m_target) < 0.01f)
                //{
                //    m_bFinished = true;
                //    //站立状态在RunActorState更新
                //    //playerObj.SetState(new StandActorState(playerObj));
                //}

            }
            m_bFinished = true;

        }

        public override void update()
        {
            if (m_bFinished)
                return;
            if (!m_bRunning)
            {
                CreatureEntity playerObj = GlobalClient.GameManager.LogicManager.GetEntityByID(m_clientID) as CreatureEntity;
                if (playerObj != null)
                {
                    //设置为移动状态
                    playerObj.SetState(new RunActorState(playerObj));

                }
                else
                {
                    m_bFinished = true;
                    return;
                }
            }
            run();

        }
    }
}

