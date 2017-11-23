using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Game
{
    /// <summary>
    /// 视图层管理器
    /// </summary>
    public class EntityViewManager
    {
        private LogicEntityManager _logicManager;
        private GameContext context;

        public List<Transform> Team1SpawnPoints;
        public List<Transform> Team2SpawnPoints;

        void InitSpawnPoints()
        {
            Team1SpawnPoints = new List<Transform>();
            Team2SpawnPoints = new List<Transform>();

            GameObject spp1 = GameObject.Find("Team1SpawnPoints");
            GameObject spp2 = GameObject.Find("Team2SpawnPoints");
            int count1 = spp1.transform.GetChildCount();
            int count2 = spp2.transform.GetChildCount();
            for (int i = 0; i < count1; i++)
            {
                Team1SpawnPoints.Add(spp1.transform.GetChild(i));
            }
            for (int i = 0; i < count1; i++)
            {
                Team2SpawnPoints.Add(spp2.transform.GetChild(i));
            }

        }
        
        /// <summary>
        /// 本队球员
        /// </summary>
        private List<EntityView> _myTeamBoys = new List<EntityView>();
        /// <summary>
        /// GameObj id，ViewObj的映射
        /// </summary>
        private Dictionary<int, EntityView> _viewObjMap;

        public Dictionary<int, EntityView> ViewObjMap
        {
            get
            {
                return _viewObjMap;
            }
        }
        public List<EntityView> MyTeam
        {
            get
            {
                return _myTeamBoys;
            }
        }

         

        /// <summary>
        /// 当前控制对象
        /// </summary>
        private EntityView _curViewObj = null;
        public EntityView CurViewObj
        {
            get
            {
                return _curViewObj;
            }
            set
            {
                _curViewObj = value;
                //逻辑对象也更新
                _curViewObj.gameObj = value.gameObj;
                _logicManager.playerEntity = value.gameObj;
            }
        }


        private List<EntityView> viewOjbList = new List<EntityView>();

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            context = new GameContext();
            _logicManager = GlobalClient.GameManager.LogicManager;
            viewOjbList = new List<EntityView>();
            _viewObjMap = new Dictionary<int, EntityView>();
        }

        /// <summary>
        /// 创建一个实体
        /// </summary>
        /// <param name="charData"></param>
        public void CreateViewObj(CharData charData, Vector3 Pos, Quaternion rotation)
        {
            EntityView obj = new EntityView();
            obj.Create(charData, this, Pos, rotation);
            viewOjbList.Add(obj);
            ViewObjMap.Add(obj.ObjID, obj);
            _logicManager.entities.Add(obj.gameObj);
        }

        

        /// <summary>
        /// 创建玩家自己
        /// </summary>
        /// <param name="charData"></param>
        public void CreateMe(CharData charData, Vector3 Pos, Quaternion rotation)
        {
            PlayerView obj = new PlayerView();
            obj.Create(charData, this, Pos, rotation);
            //obj.AddMoveController();
            viewOjbList.Add(obj);
            _myTeamBoys.Add(obj);
            ViewObjMap.Add(obj.ObjID, obj);
            _logicManager.entities.Add(obj.gameObj);
            CurViewObj = obj;
        }


        

        /// <summary>
        /// 创建足球
        /// </summary>
        public void CreateSoccer()
        {
            SoccerView obj = new SoccerView();
            
        }

        /// <summary>
        /// 创建一个玩家
        /// </summary>
        /// <param name="charData"></param>
        public void CreatePlayer(CharData charData, Vector3 Pos, Quaternion rotation)
        {
            CreatureView obj = new CreatureView();
            obj.Create(charData, this,Pos, rotation);
            viewOjbList.Add(obj);
            ViewObjMap.Add(obj.ObjID, obj);
            _logicManager.entities.Add(obj.gameObj);
        }

        /// <summary>
        /// 创建玩家
        /// </summary>
        /// <param name="players"></param>
        public void CreateAllPlayer(string players)
        {
            InitSpawnPoints();
            //通过字符串拆分取得各个玩家ID信息
            string[] playStr = players.Split(';');
            int team1PlayerNum = 0;
            int team2PlayerNum = 0;
            for(int i = 0; i < playStr.Length; ++i)
            {
                
                CharData charData = new CharData(playStr[i]);
                if (i == 0 && charData.clientID == GlobalClient.NetWorkManager.ClientID)
                {
                    //主玩家主场
                    _logicManager.isHostPlayer = true;

                }
                if (charData.clientID == GlobalClient.NetWorkManager.ClientID)
                {

                    if (_logicManager.isHostPlayer == true)
                    {
                        //创建自己
                        CreateMe(charData, Team1SpawnPoints[team1PlayerNum].position, Team1SpawnPoints[team1PlayerNum].rotation);
                        team1PlayerNum++;
                    }else
                    {
                        CreateMe(charData, Team2SpawnPoints[team2PlayerNum].position, Team2SpawnPoints[team2PlayerNum].rotation);
                        team2PlayerNum++;

                    }
                    
                   
                    //如果第一个玩家是自己，则为主控玩家，发送足球信息
                    Debug.Log(string.Format("创建玩家自己,玩家Name:{0}",charData.entityName));
                }
                else
                {
                    if (_logicManager.isHostPlayer == true)
                    {
                        CreatePlayer(charData, Team2SpawnPoints[team2PlayerNum].position, Team2SpawnPoints[team2PlayerNum].rotation);
                        team2PlayerNum++;
                    }else
                    {
                        CreatePlayer(charData, Team1SpawnPoints[team1PlayerNum].position, Team1SpawnPoints[team1PlayerNum].rotation);
                        team1PlayerNum++;
                        
                    }
                    //创建别人
                    
                    Debug.Log(string.Format("创建别人,玩家Name:{0}", charData.entityName));
                }
            }
        }

        public void Update()
        {

            //再更新视图对象
            for(int i = 0; i < viewOjbList.Count; ++i)
            {
                viewOjbList[i].Update();
            }
        }

        /// <summary>
        /// 同步网络位置
        /// </summary>
        public void SyncPos(int roleId, string pos, int gameObjID)
        {
            string[] str = pos.Split('#');
            float x = float.Parse(str[0]);
            float y = float.Parse(str[1]);
            float z = float.Parse(str[2]);
            float angleX = float.Parse(str[3]);
            float angleY = float.Parse(str[4]);
            float angleZ = float.Parse(str[5]);
            Vector3 cPos = new Vector3(x,y,z);
            Vector3 cAngle = new Vector3(angleX, angleY, angleZ);
            for(int i =0; i < viewOjbList.Count; i++)
            {
                if (viewOjbList[i].gameObj.mCharData.clientID == roleId && viewOjbList[i].gameObj.ID==gameObjID)
                {
                    viewOjbList[i].Pos = cPos;
                    viewOjbList[i].EulerAngles = cAngle;
                    break;
                }
            }
        }

        
    }
}


