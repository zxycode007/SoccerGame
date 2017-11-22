using UnityEngine;
using System.Collections;
using System;

namespace Game
{
    /// <summary>
    /// 游戏逻辑对象
    /// </summary>
    public class LogicEntity 
    {
        protected int entityId = 10000;
        /// <summary>
        /// -1中立 1主场玩家  0 客场玩家
        /// </summary>
        public int teamNo = -1;
        protected Vector3 pos = Vector3.zero;
        protected Vector3 direction = Vector3.zero;
        protected Quaternion rotation;
        protected LogicEntityManager _gameMap;
        protected CharData _charData;
        private GameContext context;
        /// <summary>
        /// 速度
        /// </summary>
        float m_speed;

        public virtual void Init(CharData charData, LogicEntityManager gameMap)
        {
            context = new GameContext();
            _charData = charData;
            _gameMap = gameMap;
            //Id = charData.roleId;
            try
            {
                //解析单位名字转换ID
                entityId = int.Parse(charData.name);
            }catch(Exception e)
            {
                Debug.LogError(string.Format("Parse charData.Name to Integer Error! {0}",_charData.name));
            }
            
        }

        public int ID
        {
            get
            {
                return entityId;
            }
        }

        public CharData mCharData
        {
            get
            {
                return _charData;
            }
        }

        public Vector3 Position
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
            }
        }
       
        public Vector3 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }

        public float Speed
        {
            get
            {
                return m_speed;
            }
            set
            {
                m_speed = value;
            }
        }
        public virtual void Update()
        {

        }
    }
}
