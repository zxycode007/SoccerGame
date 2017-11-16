using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StandActorState : BaseActorState
    {
        
        
        public StandActorState(Player playerObj):base(playerObj)
        {
            m_stateName = "StandActorState";
            m_state = ActorState.STAND;
            m_duration = 2;
            m_timeAxis = new TimeAxis(m_duration * GlobalClient.KeyFrameManager.renderFrameCount * GlobalClient.TimeManager.FrameTime); 
            
        }


        // Update is called once per frame
        public override void Update()
        {

            m_timeAxis.Update(GlobalClient.TimeManager.GetDeltaTime());
            if (m_timeAxis.GetTimePercent() > 0.95)
            {
                //站立状态是个循环
                Reset();
            }
        }

        public override void Reset()
        {
            m_timeAxis.Reset();
        }

    }
}

