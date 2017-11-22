using UnityEngine;
using System.Collections;
using Game.Util;
using UnityEngine.UI;
using System;

namespace Game
{
    /// <summary>
    /// 游戏更新
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        private EntityViewManager _viewManager;
        private LogicEntityManager _logicManager;
        

        private GameContext context;

        public GameManager()
        {
            context = new GameContext();
        }
        public EntityViewManager ViewManager
        {
            get
            {
                return _viewManager;
            }
        }

        public LogicEntityManager LogicManager
        {
            get
            {
                return _logicManager;
            }
        }


        // Update is called once per frame
        //游戏帧总更新
        public void Update()
        {
            //关键帧更新
            GlobalClient.KeyFrameManager.Update();
            //命令更新
            GlobalClient.CommandManager.Update();
            //逻辑层更新
            LogicManager.Update();
            //推进游戏显示层更新
            ViewManager.Update();
            //网络更新
            GlobalClient.NetWorkManager.Update();
            
        }

        /// <summary>
        /// 获取用户输入，上传数
        /// </summary>
        public void SendKeyFrameData()
        {
            InputData inputInfo = GlobalClient.GetPlayerController().GetPlayerInputData();
            //本地操作的关键帧数据
            KeyFrame kf = new KeyFrame(GlobalClient.NetWorkManager.ClientID,LogicManager.curObj.ID, Convert.ToByte(inputInfo.bMoved), Convert.ToByte(inputInfo.shootPower), Convert.ToByte(inputInfo.passPower), Convert.ToByte(inputInfo.stolenPower), inputInfo.angle.ToString());
            //把当前操作的关键帧数据发给服务器
            GlobalClient.NetWorkManager.SendKeyFrameData(kf);
        }



        /// <summary>
        /// 初始化游戏
        /// </summary>
        public void Init()
        {
            _logicManager = new LogicEntityManager();
            _viewManager = new EntityViewManager();
            ViewManager.Init();
            LogicManager.Init();
            
        }



        public bool IsHostPlayer()
        {
            return LogicManager.isHostPlayer;
        }

        
    }
}
