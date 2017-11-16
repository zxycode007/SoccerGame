using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Game
{
    /// <summary>
    /// ViewMap管理视图层的对象
    /// </summary>
    public class ViewMap
    {
        private GameMap _logicMap;
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
        public GameMap LogicMap
        {
            get
            {
                return _logicMap;
            }
        }
        /// <summary>
        /// 本队球员
        /// </summary>
        private List<ViewObj> _myTeamBoys = new List<ViewObj>();
        /// <summary>
        /// GameObj id，ViewObj的映射
        /// </summary>
        private Dictionary<int, ViewObj> _viewObjMap;

        public Dictionary<int, ViewObj> ViewObjMap
        {
            get
            {
                return _viewObjMap;
            }
        }
        public List<ViewObj> MyTeam
        {
            get
            {
                return _myTeamBoys;
            }
        }

         

        /// <summary>
        /// 当前控制对象
        /// </summary>
        private ViewObj _curViewObj = null;
        public ViewObj CurViewObj
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
                _logicMap.curObj = value.gameObj;
            }
        }


        private List<ViewObj> viewOjbList = new List<ViewObj>();

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            context = new GameContext();
            _logicMap = new GameMap();
            viewOjbList = new List<ViewObj>();
            _viewObjMap = new Dictionary<int, ViewObj>();
        }

        /// <summary>
        /// 创建一个实体
        /// </summary>
        /// <param name="charData"></param>
        public void CreateViewObj(CharData charData, Vector3 Pos, Quaternion rotation)
        {
            ViewObj obj = new ViewObj();
            obj.Create(charData, this, Pos, rotation);
            viewOjbList.Add(obj);
            ViewObjMap.Add(obj.ObjID, obj);
            _logicMap.gameObjList.Add(obj.gameObj);
        }

        

        /// <summary>
        /// 创建玩家自己
        /// </summary>
        /// <param name="charData"></param>
        public void CreateMe(CharData charData, Vector3 Pos, Quaternion rotation)
        {
            MeViewPlayer obj = new MeViewPlayer();
            obj.Create(charData, this, Pos, rotation);
            //obj.AddMoveController();
            viewOjbList.Add(obj);
            _myTeamBoys.Add(obj);
            ViewObjMap.Add(obj.ObjID, obj);
            _logicMap.gameObjList.Add(obj.gameObj);
            CurViewObj = obj;
        }


        

        /// <summary>
        /// 创建足球
        /// </summary>
        public void CreateSoccer()
        {
            ViewSoccer obj = new ViewSoccer();
            
        }

        /// <summary>
        /// 创建一个玩家
        /// </summary>
        /// <param name="charData"></param>
        public void CreatePlayer(CharData charData, Vector3 Pos, Quaternion rotation)
        {
            ViewPlayer obj = new ViewPlayer();
            obj.Create(charData, this,Pos, rotation);
            viewOjbList.Add(obj);
            ViewObjMap.Add(obj.ObjID, obj);
            _logicMap.gameObjList.Add(obj.gameObj);
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
                if (i == 0 && charData.roleId == LogicMap.curRoleId)
                {
                    //主玩家主场
                    GameManager.instance.viewMap._logicMap.isHostPlayer = true;

                }
                if(charData.roleId == LogicMap.curRoleId)
                {
                    
                    if(GameManager.instance.viewMap._logicMap.isHostPlayer == true)
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
                    Debug.Log(string.Format("创建玩家自己,玩家Name:{0}",charData.name));
                }
                else
                {
                    if (GameManager.instance.viewMap._logicMap.isHostPlayer == true)
                    {
                        CreatePlayer(charData, Team2SpawnPoints[team2PlayerNum].position, Team2SpawnPoints[team2PlayerNum].rotation);
                        team2PlayerNum++;
                    }else
                    {
                        CreatePlayer(charData, Team1SpawnPoints[team1PlayerNum].position, Team1SpawnPoints[team1PlayerNum].rotation);
                        team1PlayerNum++;
                        
                    }
                    //创建别人
                    
                    Debug.Log(string.Format("创建别人,玩家Name:{0}", charData.name));
                }
            }
        }

        public void Update()
        {

            GlobalClient.CommandManager.Update();
            //逻辑层更新后
            _logicMap.Update();
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
                if (viewOjbList[i].gameObj.mCharData.roleId == roleId && viewOjbList[i].gameObj.ID==gameObjID)
                {
                    viewOjbList[i].Pos = cPos;
                    viewOjbList[i].EulerAngles = cAngle;
                    break;
                }
            }
        }

        
    }
}


