using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Game
{
    public class SelectTeamMainWindow : MonoBehaviour
    {

        public List<GamePlayer> m_team1;
        public List<GamePlayer> m_team2;
        public List<GamePlayer> m_noteam;
        //确定按钮
        public Button m_readyBtn;
          

        void Awake()
        {
            m_team1 = new List<GamePlayer>();
            m_team2 = new List<GamePlayer>();
            m_noteam = new List<GamePlayer>();
            
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 向服务器发送准备好消息
        /// </summary>
        public void OnClickReady()
        {
            Debug.LogWarning("toReady！");
        }
    }
}

