using UnityEngine;
using System.Collections;

namespace Game
{
    public class GamePlayerManager
    {
        GamePlayer m_self;

        public GamePlayerManager()
        {

        }

        public GamePlayer Self
        {
            get
            {
                return m_self;
            }
            set
            {
                m_self = value;
            }
        }

         
    }
}

