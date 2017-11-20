using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



namespace Game
{
    public class TimeAxis : object
    {
        float m_beginTimePos;
        float m_endTimePos;
        float m_curTimePos;
        float m_timeLen;
        public int i = 0;
        float m_timeScale;

        GameContext context;

        public TimeAxis(float length)
        {
            context = new GameContext();
            m_beginTimePos = Time.deltaTime;
            m_endTimePos = m_beginTimePos + length;
            m_curTimePos = m_beginTimePos;
            m_timeLen = length;
        }

         public void Update(float deltaTime)
        {
            m_curTimePos += deltaTime;
            m_curTimePos = Mathf.Clamp(m_curTimePos, m_beginTimePos, m_endTimePos);
            i++;
            
        }

        public void Reset()
        {
             m_beginTimePos = Time.time;
             m_curTimePos = 0;
             m_endTimePos = m_beginTimePos + m_timeLen;
        }

        public float CurTime
        {
            get
            {
                return m_curTimePos - m_beginTimePos;
            }
            set
            {
                m_curTimePos = m_beginTimePos + value;
                m_curTimePos = Mathf.Clamp(m_curTimePos, m_beginTimePos, m_endTimePos);
            }
        }

        public float GetTimePercent()
        {
            if(m_timeLen == 0)
            {
              return 0;
            }
            return CurTime / m_timeLen;
        }

        public float TimeAxisLength
        {
            get
            {
                return m_timeLen;
            }
            set
            {
                m_timeLen = value;
                Reset();
            }

        }
    }

}
    

