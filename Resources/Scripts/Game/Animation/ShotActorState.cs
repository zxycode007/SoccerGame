using UnityEngine;
using System.Collections;

namespace Game
{
    public class ShotActorState : BaseActorState
    {

        float m_angle;
        // Use this for initialization
        public ShotActorState(CreatureEntity playerObj, float angle):base(playerObj)
            
        {
            m_stateName = "PassActorState";
            m_state = ActorState.SHOT;
            m_angle = angle;
        }

        // Update is called once per frame
        public override void Update()
        {
            m_timeAxis.Update(GlobalClient.TimeManager.GetDeltaTime());
            ///时间走完了
            if (m_timeAxis.GetTimePercent() > 0.95)
            {
                //回到站立状态    
                m_playerObj.SetState(new StandActorState(m_playerObj));
            }
        }
    }
}

