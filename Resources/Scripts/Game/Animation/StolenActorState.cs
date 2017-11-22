using UnityEngine;
using System.Collections;

namespace Game
{
    public class StolenActorState : BaseActorState
    {

        // Use this for initialization
        public StolenActorState(CreatureEntity playerObj):base(playerObj)
        {
            m_stateName = "StolenActorState";
            m_state = ActorState.STOLEN;
        }

        // Update is called once per frame
        public override void Update()
        {
            m_timeAxis.Update(GlobalClient.TimeManager.GetDeltaTime());
        }
    }

}
