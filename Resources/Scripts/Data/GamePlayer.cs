using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class GamePlayer
    {
        int m_playerID;
        string m_playerName;
        string m_playerIcon;

        public GamePlayer()
        {

        }
        public GamePlayer(int id , string name, string icon)
        {
            m_playerID = id;
            m_playerName = name;
            m_playerIcon = icon;
        }

        public int PlayerID
        {
            get
            {
                return m_playerID;
            }
            set
            {
                m_playerID = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return m_playerName;
            }
            set
            {
                m_playerName = value;
            }
        }

        public string PlayerIcon
        {
            get
            {
                return m_playerIcon;
            }
            set
            {
                m_playerIcon = value;
            }
        }
    }
}
