using Game.Util;
using Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using Util;

namespace Game
{
    public class NetManager
    {
        private string _ip = "127.0.0.1";
        private int _tcpPort = 1255;
        private int _udpPort = 1337;
        AsycUdpClient client;
        private List<string> keyPack = new List<string>();
        GameContext m_context;

        public bool isInitialized = false;

        public NetManager()
        {
            m_context = new GameContext();
        }

        public void AddKeyPack(KeyData data)
        {
            keyPack.Add(data.ToString());
        }

        public void ClearKeyPack()
        {
            keyPack.Clear();
        }

        public void SetIpInfo(string ip = "127.0.0.1", int tcpPort = 1255, int udpPort = 1337)
        {
            _ip = ip;
            _tcpPort = tcpPort;
            _udpPort = udpPort;
        }

        public void Update()
        {
            //网络客户端更新
            client.Update();
        }

        /// <summary>
        /// 初始化网络客户端
        /// </summary>
        public void InitClient()
        {
            client = new AsycUdpClient();
            client.OnConnect += OnConnect;
            client.OnDisconnect += OnDisconnect;
            client.OnMessage += OnMessage;
            isInitialized = true;
        }

        /// <summary>
        /// 发送自己的位置和方向
        /// </summary>
        public void SycMePos()
        {
            for (int i = 0; i < GameManager.instance.viewMap.MyTeam.Count; i++ )
            {
                //Vector3 pos = GameManager.instance.viewMap.CurViewObj.Pos;
                //Vector3 angle = GameManager.instance.viewMap.CurViewObj.EulerAngles;
                //我方每一个对象的位置角度发过去
                Vector3 pos = GameManager.instance.viewMap.MyTeam[i].Pos;
                Vector3 angle = GameManager.instance.viewMap.MyTeam[i].EulerAngles;
                int name = GameManager.instance.viewMap.MyTeam[i].gameObj.ID;
                MessageBuffer msgBuf = new MessageBuffer();
                msgBuf.WriteInt(cProto.SYNC_POS);
                msgBuf.WriteInt(GameManager.instance.viewMap.CurViewObj.PlayerID);
                msgBuf.WriteInt(name);
                string cPos = string.Format("{0}#{1}#{2}#{3}#{4}#{5}", pos.x, pos.y, pos.z, angle.x, angle.y, angle.z);
                msgBuf.WriteString(cPos);
                Send(msgBuf);
            }
        }

        /// <summary>
        /// 发送关键帧
        /// </summary>
        public void SycKey()
        {
            string keyStr = string.Join(";", keyPack.ToArray());
            MessageBuffer msgBuf = new MessageBuffer();
            msgBuf.WriteInt(cProto.SYNC_KEY);
            //当前帧数
            msgBuf.WriteInt(GlobalClient.KeyFrameManager.curKeyFrameNo);
            msgBuf.WriteString(keyStr);
            Send(msgBuf);
            //发送结束清空关键帧数据
            ClearKeyPack();
        }

        public void SendPlayerInfo(GamePlayer player)
        {
            if (player != null && isInitialized)
            {
                MessageBuffer msg = new MessageBuffer();
                msg.WriteInt(cProto.LOGIN);
                msg.WriteInt(player.PlayerID);
                msg.WriteString(player.PlayerName);
                msg.WriteString(player.PlayerIcon);
                Send(msg);
            }
        }


        public void SendKeyFrameData(KeyFrame frameData)
        {
            MessageBuffer msg = new MessageBuffer();
            msg.WriteInt(cProto.SYNC_KEY);
            //当前帧数
            msg.WriteInt(GlobalClient.KeyFrameManager.curKeyFrameNo);
            //将数据转为字符串方式发送
            msg.WriteString(frameData.ToString());
            Send(msg);
            
        }

        /// <summary>
        /// 客户端准备
        /// </summary>
        public void Ready()
        {
            MessageBuffer msgBuf = new MessageBuffer();
            msgBuf.WriteInt(cProto.READY);
            msgBuf.WriteInt(GameManager.instance.viewMap.LogicMap.curRoleId);
            Send(msgBuf);
        }

        /// <summary>
        /// 连接
        /// </summary>
        public void Connect()
        {
            Debug.Log("Connecting....");
            client.Connect(_ip, _tcpPort, _udpPort);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        private void Send(string msg)
        {
            MessageBuffer msgBuf = new MessageBuffer();
            msgBuf.WriteString(msg);
            if (client.Connected)
            {
                client.Send(msgBuf);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="cproto"></param>
        private void Send(int cproto)
        {
            MessageBuffer msgBuf = new MessageBuffer();
            msgBuf.WriteInt(cproto);
            if (client.Connected)
            {
                client.Send(msgBuf);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgBuf"></param>
        private void Send(MessageBuffer msgBuf)
        {
            if (client.Connected)
            {
                client.Send(msgBuf);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="cproto"></param>
        /// <param name="msg"></param>
        private void Send(int cproto, string msg)
        {
            MessageBuffer msgBuf = new MessageBuffer();
            msgBuf.WriteInt(cproto);
            msgBuf.WriteString(msg);
            if (client.Connected)
            {
                client.Send(msgBuf);
            }
        }

        public bool IsConnected()
        {   
            if(client.Connected)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (client.Connected)
            {
                client.Disconnect();
                Debug.Log("Disconnect.....");
            }
        }

        private void Ping()
        {
            long ping = AsycUdpClient.Ping(new IPEndPoint(IPAddress.Parse(_ip), 1337));
            Debug.Log("Ping....." + ping);
        }

        public void OnConnect()
        {
            Debug.Log("Connected to server!");
        }

        public void OnMessage(MessageBuffer msg)
        {
            int cproto = msg.ReadInt();
            //Debug.Log(cproto);
            switch (cproto)
            {
                case cProto.CONNECT:
                    {
                        int roleId = msg.ReadInt();
                        GameManager.instance.viewMap.LogicMap.curRoleId = roleId;
                        Debug.Log("玩家,id = " + roleId);
                        ServerOnConnectArg arg = new ServerOnConnectArg();
                        arg.PlayerID = roleId;
                        m_context.FireEvent(this, EventType.EVT_SERVER_ON_CONNECT, arg);

                    }
                    break;
                case cProto.READY:
                    break;
                    //位置同步
                case cProto.SYNC_POS:
                    int cRoleId = msg.ReadInt();
                    int gameObjID = msg.ReadInt();
                    string pos = msg.ReadString();
                    GameManager.instance.viewMap.SyncPos(cRoleId, pos, gameObjID);
                    //Debug.Log(string.Format("玩家 {0} ,pos = {1}", cRoleId ,pos));
                    break;
                    //同步关键帧k
                case cProto.TO_TEAM_SELECT:
                    {
                        int playerID = msg.ReadInt();
                        string playerName = msg.ReadString();
                        string playerIconPath = msg.ReadString();
                        GamePlayer playerInfo = new GamePlayer(playerID, playerName, playerIconPath);
                        ToSelectTeamArg arg = new ToSelectTeamArg();
                        arg.playerInfo = playerInfo;
                        m_context.FireEvent(this, EventType.EVT_TO_TEAM_SELECT, arg);
                        break;
                    }
                case cProto.SYNC_KEY:
                    //读取服务器帧数
                    int servFrameCount = msg.ReadInt();
                    //Debug.LogWarning(string.Format("服务器关键帧={0}", servFrameCount));
                    GlobalClient.KeyFrameManager.serverKeyFrameNo = servFrameCount;
                    //当服务器帧数>=当前客户端帧数的时候，客户端本地帧才更新
                    if (servFrameCount >= GlobalClient.KeyFrameManager.curKeyFrameNo)
                    {
                        //解析出所有用户的KeyDataList
                        string keyStr = msg.ReadString();
                        //分号分离各个用户的KeyDataList;
                        string[] keyData = keyStr.Split(';');
                        for (int i = 0; i < keyData.Length; ++i)
                        {
                            if (keyData[i] == "")
                            {
                                continue;
                            }
                            KeyFrame kf = new KeyFrame(keyData[i]);
                            //插入关键帧管理器中
                            GlobalClient.KeyFrameManager.AddKeyData(servFrameCount, kf);
                             
                        }
                        
                    }
                    break;
                case cProto.START:
                    //读取玩家列表
                    string players = msg.ReadString();
                    //创建所有玩家
                    GameManager.instance.viewMap.CreateAllPlayer(players);
                    //初始化服务器关键帧为1
                    GlobalClient.KeyFrameManager.serverKeyFrameNo = 0;
                    //每50毫秒向服务器发送位置信息
                    //TimerHeap.AddTimer(0, 50, SycMePos);
                    //每100毫秒向服务器关键帧数据
                   // TimerHeap.AddTimer(0, 100, SycKey);
                    break;
            }
        }

        public void OnDisconnect()
        {
            Debug.Log("Disconnected from server!");
        }
    }
}
