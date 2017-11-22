using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    /// <summary>
    /// 关键帧数据
    /// </summary>
    public class KeyData
    {
        //命令
        public Cmd cmd;
        public string data;
        //用户ID
        public int clientId;
        public KeyData(Cmd cmd, string data, int clientID)
        {
            this.cmd = cmd;
            this.data = data;
            this.clientId = clientID;
        }

        public KeyData(string dataStr)
        {
            string[] str = dataStr.Split('#');
            this.cmd = (Cmd)int.Parse(str[0]);
            this.data = str[1];
            this.clientId = int.Parse(str[2]);
        }

        public override string ToString()
        {
            int iCmd = (int)cmd;
            string str = iCmd.ToString() + "#" + data +"#" + clientId.ToString();
            return str;
        }
    }


}
