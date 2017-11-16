using UnityEngine;
using System.Collections;

namespace Game
{
    public class PassActorState : BaseActorState
    {

        Vector3 m_passDir;
        // Use this for initialization
        public PassActorState(Player playerObj, Vector3 dir):base(playerObj)
             
        {
            m_stateName = "PassActorState";
            m_state = ActorState.PASS;
            m_passDir = dir;
        }

        // Update is called once per frame
        public override void Update()
        {
            m_timeAxis.Update(GlobalClient.TimeManager.GetDeltaTime());
            if (m_timeAxis.GetTimePercent() > 0.95)
            {
                //回到站立状态    
                m_playerObj.SetState(new StandActorState(m_playerObj));
            }

        }
    }
}

