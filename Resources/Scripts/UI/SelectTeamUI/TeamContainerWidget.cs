using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public enum E_TEAM_NO
    {
        TEAM1,
        TEAM2,
        NOTEAM
    }

    public class TeamContainerWidget : MonoBehaviour
    {
        //玩家列表
        public List<GamePlayer> m_playerList;
        //队伍号
        public E_TEAM_NO m_teamNo;
        ///预置体
        public GameObject m_playerItemPrefab;
        //主窗口
        public SelectTeamMainWindow m_mainWnd;

        /// <summary>
        /// 某个玩家加入
        /// </summary>
        /// <param name="player">玩家信息</param>
        public void Join(GamePlayer player)
        {
            m_playerList.Add(player);
            RefreshData();
        }

        /// <summary>
        /// 玩家退出 会回到等待区
        /// </summary>
        /// <param name="player">玩家信息</param>
        /// <returns>退出玩家的信息</returns>
        public GamePlayer Quit(GamePlayer player)
        {
             GamePlayer ret = null;
             if(m_playerList.Where(s=>s.PlayerID == player.PlayerID).Count<GamePlayer>() > 0)
             {
                 ret = m_playerList.Where(s => s.PlayerID == player.PlayerID).First<GamePlayer>();
                 //返回查到的第一个
                 m_playerList.Remove(ret);
             }
             RefreshData();
             return ret;
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        public void RefreshData()
        {
            m_mainWnd.m_team1.Clear();
            m_mainWnd.m_team2.Clear();
            m_mainWnd.m_noteam.Clear();
            for(int i=0; i<transform.GetChildCount(); i++)
            {
               Transform child = transform.GetChild(i);
               if(child != null)
               {
                   DestroyObject(child.gameObject);
               }
            }
            for(int i=0; i<m_playerList.Count; i++)
            {
                GamePlayer player = m_playerList[i];
                switch(m_teamNo)
                {
                    case E_TEAM_NO.TEAM1:
                        m_mainWnd.m_team1.Add(player);
                        break;
                    case E_TEAM_NO.TEAM2:
                        m_mainWnd.m_team2.Add(player);
                        break;
                    case E_TEAM_NO.NOTEAM:
                        m_mainWnd.m_noteam.Add(player);
                        break;
                }
                GameObject go = GameObject.Instantiate(m_playerItemPrefab) as GameObject;
                if(go != null)
                {
                    PlayerItem item = go.GetComponent<PlayerItem>();
                    item.PlayerInfo = player;
                    go.transform.SetParent(transform);
                }
            }
        }

        void Awake()
        {
            m_playerList = new List<GamePlayer>();
            RefreshData();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

