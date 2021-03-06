﻿using UnityEngine;
using System;


namespace Game
{
    public class StandCommand : BaseCommand
    {
        
        public StandCommand():base()
        {
             
            m_commandName = "StandCommand";
            m_commandType = ECommandType.Stand;
        }

        public StandCommand(int playerID):base(playerID)
        {
            m_commandName = "StandCommand";
            m_commandType = ECommandType.Stand;
        }


        public override void run()
        {
            base.run();

        }

        public override void update()
        {
            if (m_bFinished)
                return;
            if(!m_bRunning)
            {
                CreatureEntity playerObj = GlobalClient.GameManager.LogicManager.GetEntityByID(m_clientID) as CreatureEntity;
                 if (playerObj != null)
                 {
                     //设置为站立状态
                     playerObj.SetState(new StandActorState(playerObj));
                 }else
                 {
                     m_bFinished = true;
                     return;
                 }
                
            }
            run();


        }
        

    }
}
