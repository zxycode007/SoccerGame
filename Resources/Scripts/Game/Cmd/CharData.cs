using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    /// <summary>
    /// 玩家角色数据
    /// </summary>
    public class CharData
    {
        public int roleId;
        public string name;

        /// <summary>
        /// 用#拆分，第一个用户ID，第二个用户名
        /// </summary>
        /// <param name="str"></param>
        public CharData(string str)
        {
            string[] playStr = str.Split('#');
            roleId = int.Parse(playStr[0]);
            name = playStr[1];
        }

        /// <summary>
        /// 用户ID#用户名
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = roleId + "#" + name;
            return str;
        }
    }
}
