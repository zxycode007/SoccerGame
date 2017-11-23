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
        /// 阵营 红，蓝，中立
        /// </summary>
        public ECampType campType = ECampType.None;
        protected Vector3 pos = Vector3.zero;
        protected Vector3 direction = Vector3.zero;
        protected Quaternion rotation;
        protected CharData _charData;
        //实体名字
        protected string name;
        private GameContext context;
        /// <summary>
        /// 速度
        /// </summary>
        float m_speed;

        public virtual void Init(CharData charData, LogicEntityManager gameMap)
        {
            context = new GameContext();
            _charData = charData;

            //从服务器发来创建实体名字
            name = _charData.entityName;
            try
            {
                //解析单位名字转换ID
                entityId = int.Parse(charData.entityName);
            }catch(Exception e)
            {
                Debug.LogError(string.Format("Parse charData.Name to Integer Error! {0}",_charData.entityName));
            }
            
        }

        public int ID
        {
            get
            {
                return entityId;
            }
        }

        /// <summary>
        /// 逻辑实体名
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
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
