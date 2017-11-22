using UnityEngine;
using System.Collections;

namespace Game
{

    public enum ActorState
    {
        IDLE = 0,
        STAND,
        SHOT,
        BIGSHOT,
        PASS,
        RUN1,
        RUN2,
        STOLEN,
        TACKLE,
        OTHER
    }
    /// <summary>
    /// 角色逻辑状态
    /// </summary>
    public class BaseActorState : object
    {
        /// <summary>
        /// 时间轴
        /// </summary>
        protected TimeAxis m_timeAxis;

        /// <summary>
        /// 持续逻辑帧长度
        /// </summary>
        protected int m_duration;

        protected string m_stateName;
        protected ActorState m_state;
        /// <summary>
        /// 玩家操控逻辑对象
        /// </summary>
        protected CreatureEntity m_playerObj;

        public BaseActorState(CreatureEntity playerObj)
        {
            //默认持续一个逻辑帧
            m_duration = 1;
            m_stateName = "BaseActorState";
            m_state = ActorState.OTHER;
            m_playerObj = playerObj;
           // m_timeAxis = new TimeAxis(m_duration * GlobalClient.KeyFrameManager.renderFrameCount * GlobalClient.TimeManager.FrameTime);
        }


        public CreatureEntity PlayerObj
        {
            get
            {
                return m_playerObj;
            }
        }

        public string GetStateName()
        {
            return m_stateName;
        }

        public ActorState GetState()
        {
            return m_state;
        }

        public int Duration
        {
            get
            {
                return m_duration;
            }
            set
            {
                m_duration = value;
                m_timeAxis.TimeAxisLength = m_duration * GlobalClient.TimeManager.CurLogicFrameTime;
            }
        }

        public TimeAxis GetTimeAxis
        {
            get
            {
                return m_timeAxis;
            }
        }

        // Update is called once per frame
        public virtual void Update()
        {

        }

        public virtual void Reset()
        {

        }
    }
}

