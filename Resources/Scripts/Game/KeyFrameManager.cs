using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Game
{
    /// <summary>
    /// 用于同步操作
    /// </summary>
    public class KeyFrame
    {
        //玩家ID
        int  _playerId;
        int _playerObjId;
        //按键参数
        byte _bMoved;
        byte _shotPower;
        byte _passPower;
        byte _stolenPower;
        //其它参数
        string _param;

        public int PlayerOBjcectID
        {
            get
            {
                return _playerObjId;
            }
            set
            {
                _playerObjId = value;
            }
        }

        public int PlayerID
        {
            get
            {
                return _playerId;
            }
            set
            {
                _playerId = value;
            }
        }

        public byte isMoved
        {
            get
            {
                return _bMoved;
            }
            set
            {
                _bMoved = value;
            }
        }

        public byte PassPower
        {
            get
            {
                return _passPower;
            }
            set
            {
                _passPower = value;
            }
        }

        public byte StolenPower
        {
            get
            {
                return _stolenPower;
            }
            set
            {
                _stolenPower = value;
            }
        }

        public byte ShotPower
        {
            get
            {
                return _shotPower;
            }
            set
            {
                _shotPower = value;
            }
        }

        public string OtherParam
        {
            get
            {
                return _param;
            }
            set
            {
                _param = value;
            }
        }

        public void Parse(string data)
        {
            string[] tmp = data.Split('#');
            _playerId = int.Parse(tmp[0]);
            _playerObjId = int.Parse(tmp[1]);
            _bMoved = byte.Parse(tmp[2]);
            _shotPower = byte.Parse(tmp[3]);
            _passPower = byte.Parse(tmp[4]);
            _stolenPower = byte.Parse(tmp[5]);
            _param = tmp[6];
        }

        public KeyFrame(int playerId,int playerObjId, byte bMoved, byte shotPower, byte passPower, byte stolenPower, string param)
        {
            _playerObjId = playerObjId;
            _playerId = playerId;
            _bMoved = bMoved;
            _shotPower = shotPower;
            _passPower = passPower;
            _stolenPower = stolenPower;
            _param = param;
        }

        /// <summary>
        /// 解析服务器回发的包含关键帧数据的字符串
        /// </summary>
        /// <param name="dataStr"></param>
        public KeyFrame(string dataStr)
        {
            string[] tmpStr = dataStr.Split('#');
            try
            {
                _playerId = int.Parse(tmpStr[0]);
                _playerObjId = int.Parse(tmpStr[1]);
                _bMoved = byte.Parse(tmpStr[2]);
                _shotPower = byte.Parse(tmpStr[3]);
                _passPower = byte.Parse(tmpStr[4]);
                _stolenPower = byte.Parse(tmpStr[5]);
                _param = tmpStr[6];

            }catch(Exception e)
            {
                Debug.LogError("解析关键帧数据出错!" + e.Message);

            }

        }

        /// <summary>
        /// 将参数信息生成字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _playerId.ToString() + "#" + _playerObjId.ToString() + "#" + _bMoved.ToString() + "#" + _shotPower.ToString() + "#" + _passPower.ToString() + "#" + _stolenPower.ToString() + "#" + _param;
        }
    }

    public class KeyFrameManager : object
    {
        //服务器关键帧号
        public int serverKeyFrameNo = -1;
        //当前关键帧号
        public int curKeyFrameNo = 1;
        public int curFrameCount = 0;
        //每N帧一关键帧, 一个关键帧回合由N个游戏回合构成
        public int renderFrameCount = 3;
        /// <summary>
        /// 关键帧号， 关键帧列表（对应关键帧号下的所有玩家的关键帧数据）
        /// </summary>
        Dictionary<int, List<KeyFrame>> keyFrames;

        public KeyFrameManager()
        {
            keyFrames = new Dictionary<int, List<KeyFrame>>();
        }

        public Dictionary<int, List<KeyFrame>> KeyFrames
        {
            get
            {
                return keyFrames;
            }
        }

        public void AddKeyData(int keyNo, KeyFrame data)
        {
            if(keyFrames.ContainsKey(keyNo))
            {
                keyFrames[keyNo].Add(data);
            }else
            {
                keyFrames[keyNo] = new List<KeyFrame>();
                keyFrames[keyNo].Add(data);
            }
        }

        

        public List<KeyFrame> GetKeyData(int keyNo)
        {
            if (keyFrames.ContainsKey(keyNo))
            {
                return keyFrames[keyNo];
            }
            return null;
        }

        public void Update()
        {
            ///还未初始化
            if(serverKeyFrameNo == -1)
            {
                return;
            }
            //当前渲染帧+1
            curFrameCount++;
            //是否到达该推进关键帧的时候
            if(curFrameCount >= renderFrameCount)
            {
               // Debug.LogWarning("推进关键帧");
                curFrameCount = 0;
                //只有当前本地关键帧<服务器关键帧才更新
                if (curKeyFrameNo >= serverKeyFrameNo)
                {
                   // Debug.LogWarning(string.Format("当前{0}，服务器{1}", curKeyFrameNo,serverKeyFrameNo));
                    //同步一下帧号
                    curKeyFrameNo = serverKeyFrameNo;
                    GlobalClient.GameManager.SendKeyFrameData();
                }
                else
                {
                    //没到达服务器同步的关键帧，本地关键帧继续+
                    curKeyFrameNo++;
                    if(keyFrames.ContainsKey(curKeyFrameNo))
                    {
                        ProcessKeyFrameData(GetKeyData(curKeyFrameNo));
                        //处理完移除
                        keyFrames.Remove(curKeyFrameNo);
                    }
                    

                }
            }  //推进游戏逻辑更新
            GlobalClient.GameManager.viewMap.Update();
           
        }

        /// <summary>
        /// 解析关键帧数据，执行命令
        /// </summary>
        /// <param name="data"></param>
        public void ProcessKeyFrameData(List<KeyFrame> data)
        {
            for(int i=0; i<data.Count; i++)
            {
                KeyFrame kf = data[i];
                //shot优先级最高
                if(kf.ShotPower > 0)
                {
                    //执行Shot
                    continue;
                }
                if (kf.PassPower > 0)
                {
                    //执行Shot
                    continue;
                }
                if (kf.StolenPower > 0)
                {
                    continue;
                }
                if(kf.isMoved > 0)
                {
                    //移动
                    for (int j = 0; j < GlobalClient.GameManager.viewMap.LogicMap.gameObjList.Count; ++j)
                    {
                        if (GlobalClient.GameManager.viewMap.LogicMap.gameObjList[j].mCharData.roleId == kf.PlayerID)
                        {
                            //(GlobalClient.GameManager.viewMap.LogicMap.gameObjList[i] as Player).DoSkill(int.Parse(param));
                            string msg = string.Format("isMoved{0}, shotPower{1}, passPower{2}, stolenPower{3}, angle{4}", kf.isMoved, kf.ShotPower, kf.PassPower, kf.StolenPower, kf.OtherParam);
                            //Debug.LogWarning(msg);
                            MoveCommand cmd = new MoveCommand(kf.PlayerOBjcectID, float.Parse(kf.OtherParam));
                            GlobalClient.CommandManager.AddCommand(cmd);
                        }
                    }
                    continue;
                }
                

                

            }
        }

        public void Clear()
        {
            keyFrames.Clear();

        }
    }
}

