using UnityEngine;
using System.Collections;

namespace Game
{
    public class RunActorState : BaseActorState
    {
        Vector3 m_oldPos;
        Vector3 m_target;
        public RunActorState(Player playerObj):base(playerObj)
        {
            m_stateName = "RunActorState";
            m_state = ActorState.RUN1;
            m_oldPos = playerObj.Position;
            m_duration = 4;
            m_timeAxis = new TimeAxis(m_duration * GlobalClient.KeyFrameManager.renderFrameCount * GlobalClient.TimeManager.FrameTime);
            Vector3 dir = m_playerObj.Rotation * m_playerObj.Direction;
            m_target = dir.normalized * m_duration * m_playerObj.Speed;
        }

        // Update is called once per frame
        public override void Update()
        {
            
            
            m_timeAxis.Update(GlobalClient.TimeManager.GetDeltaTime());

            ///时间走完了
            if(m_timeAxis.GetTimePercent() > 0.95)
            {
                //回到站立状态    
                m_playerObj.SetState(new StandActorState(m_playerObj));
                m_timeAxis.CurTime = 1.0f;
            }
            //通过时间轴进度更新位置
            Vector3 newPos = m_oldPos + m_target * m_timeAxis.GetTimePercent();
            //Debug.Log(string.Format("TimeAxis Percent{0}, TimeAxis CurTimePos{1}, CurTick{2} PlayerObj Pos{3},  OldPos{4},   NewPos{5}, DeltaTime{6}", m_timeAxis.GetTimePercent(),m_timeAxis.CurTime, GlobalClient.TimeManager.TickCount, m_playerObj.Position, oldPos, newPos, GlobalClient.TimeManager.GetDeltaTime()));
            m_playerObj.Position = newPos;
            
            
            
        }

        public override void Reset()    
        {
            m_timeAxis.Reset();

        }
    }
}

