using UnityEngine;
using System.Collections;

namespace Game
{
    //视图对象
    public class EntityView
    {
        //游戏逻辑对象
        public LogicEntity gameObj;
        public Actor actor;
        
        protected EntityViewManager _viewMap;

        //U3d本地对象
        public GameObject gameGo;
        //U3d本地Transform
        protected Transform gameTrans;
        protected GameContext context;

        

        public virtual void Create(CharData charData, EntityViewManager viewMap, Vector3 Pos, Quaternion rotation)
        {
            _viewMap = viewMap;
            gameObj = new LogicEntity();
            gameObj.Init(charData, GlobalClient.GameManager.LogicManager);
            //gameGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject obj = GlobalClient.prefabData["PlayerActor1"];
            gameGo = GameObject.Instantiate(obj, Pos, rotation) as GameObject;
            gameGo.name = charData.name;
            gameTrans = gameGo.transform;
            context = new GameContext();
        }

        /// <summary>
        /// 玩家ID
        /// </summary>
        public int PlayerID
        {
            get
            {
                if(gameObj == null)
                {
                    return -1;
                }
                return gameObj.mCharData.roleId;
            }
        }

        public int ObjID
        {
            get
            {
                if(gameObj == null)
                {
                    return -1;
                }
                return gameObj.ID;
            }
        }

        public Vector3 Pos
        {
            get
            {
                if (gameTrans == null)
                {
                    return Vector3.zero;
                }
                return gameTrans.position;
            }
            set
            {
                if (gameTrans != null)
                {
                    gameTrans.position = value;
                }
            }
        }

        public Vector3 EulerAngles
        {
            get
            {
                if (gameTrans == null)
                {
                    return Vector3.zero;
                }
                return gameTrans.localEulerAngles;
            }
            set
            {
                gameTrans.localRotation = Quaternion.Euler(value);
            }
        }

        //更新逻辑对象
        public virtual void Update()
        {
            if(gameObj == null)
            {
                return;
            }
            //gameObj.Update();
        }
    }
}

