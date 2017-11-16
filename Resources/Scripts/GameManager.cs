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
        private ViewMap _viewMap;
        public ViewMap viewMap
        {
            get
            {
                return _viewMap;
            }
        }

        // Update is called once per frame
        //游戏帧总更新
        public void Update()
        {

            //curFrameCount++;
            //if(curFrameCount >= renderFrameCount)
            //{
            //    curFrameCount = 0;
            //    //只有当前本地关键帧<服务器关键帧才更新
            //    if (curKeyFrameNo >= serverKeyFrameNo)
            //    {
            //        //同步一下帧号
            //        curKeyFrameNo = serverKeyFrameNo;
            //        InputData inputInfo = GlobalClient.GetPlayerController().GetPlayerInputData();
            //        //当前控制角色ID
            //        string param = GlobalClient.GameManager._viewMap.LogicMap.curObj.ID.ToString();
            //        //本地操作的关键帧数据
            //        KeyFrame kf = new KeyFrame(_viewMap.LogicMap.curRoleId, Convert.ToByte(inputInfo.bMoved), Convert.ToByte(inputInfo.shootPower), Convert.ToByte(inputInfo.passPower), Convert.ToByte(inputInfo.stolenPower), param);
            //        //把当前操作的关键帧数据发给服务器
            //        GlobalClient.NetWorkManager.SendKeyFrameData(kf);
            //    }
            //    else
            //    {
            //        //没到达服务器同步的关键帧，本地关键帧继续+
            //        curKeyFrameNo++;

            //    }

            //}else
            //{
            //    //逻辑更新   
            //    if (_viewMap == null)
            //    {
            //        return;
            //    }
            //    _viewMap.Update();
            //}
            //命令管理器更新
            GlobalClient.KeyFrameManager.Update();

            GlobalClient.NetWorkManager.Update();
            ////逻辑更新   
            //if (_viewMap == null)
            //{
            //    Debug.LogError("游戏实体管理器不存在!");
            //    return;
            //}
            //_viewMap.Update();
            
        }

        /// <summary>
        /// 获取用户输入，上传数
        /// </summary>
        public void SendKeyFrameData()
        {
            InputData inputInfo = GlobalClient.GetPlayerController().GetPlayerInputData();
            //本地操作的关键帧数据
            KeyFrame kf = new KeyFrame(_viewMap.LogicMap.curRoleId,GlobalClient.GameManager._viewMap.LogicMap.curObj.ID, Convert.ToByte(inputInfo.bMoved), Convert.ToByte(inputInfo.shootPower), Convert.ToByte(inputInfo.passPower), Convert.ToByte(inputInfo.stolenPower), inputInfo.angle.ToString());
            //把当前操作的关键帧数据发给服务器
            GlobalClient.NetWorkManager.SendKeyFrameData(kf);
        }



        /// <summary>
        /// 初始化游戏
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="tcpPort">tcp端口</param>
        /// <param name="udpPort">udp端口</param>
        public void InitGame(string ip, int tcpPort, int udpPort)
        {
            _viewMap = new ViewMap();
            _viewMap.Init();
            _viewMap.LogicMap.Init();
            _viewMap.LogicMap.InitNet(ip, tcpPort, udpPort);
        }



        public bool IsHostPlayer()
        {
            return _viewMap.LogicMap.isHostPlayer;
        }

        public NetManager NetManager
        {
            get
            {
                return _viewMap.LogicMap.netManager;
            }
        }
    }
}
