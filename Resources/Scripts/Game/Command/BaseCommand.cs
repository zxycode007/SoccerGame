using UnityEngine;
using System.Collections;

namespace Game
{

    /// <summary>
    /// 游戏命令
    /// </summary>
    public class BaseCommand : object
    {
        //命令名字
        protected string m_commandName;
        protected bool m_bFinished;
        protected bool m_bRunning;
        protected ECommandType m_commandType;
        /// <summary>
        /// 玩家ID
        /// </summary>
        protected int m_clientID;

        public BaseCommand()
        {
            m_clientID = GlobalClient.NetWorkManager.ClientID;
            m_commandName = "BaseCommand";
            m_bFinished = false;
            m_bRunning = false;
            m_commandType = ECommandType.NoCommand;
        }

        public BaseCommand(int playerID)
        {
            m_clientID = playerID;
            m_commandName = "BaseCommand";
            m_bFinished = false;
            m_bRunning = false;
            m_commandType = ECommandType.NoCommand;
        }

        public bool isRunning()
        {
            return m_bRunning;
        }

        public virtual void run()
        {
            return;
        }

        public virtual void update()
        {

        }

        public virtual bool isFinished()
        {
            return m_bRunning;
        }

        public ECommandType CommandType
        {
            get
            {
                return m_commandType;
            }
        }

        public string CommandName
        {
            get
            {
                return m_commandName;
            }
        }

    }
}
