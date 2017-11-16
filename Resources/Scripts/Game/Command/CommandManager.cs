using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public enum ECommandType
    {
        Stand,
        //移动命令
        Move,
        //射门命令
        Shot,
        //传球命令
        Pass,
        //抢球命令
        Stolen,

        NoCommand
    }

    public class CommandManager : object
    {
        List<BaseCommand> m_commandList;
        // Use this for initialization
        public CommandManager()
        {
            m_commandList = new List<BaseCommand>();
        }

        public void AddCommand(BaseCommand cmd)
        {
            m_commandList.Add(cmd);
        }

        

        public void Clear()
        {
            m_commandList.Clear();
        }

        
        public void Update()
        {

            if (m_commandList.Count > 0)
            {
                for(int i=m_commandList.Count-1; i>=0; i--)
                {
                    BaseCommand cmd = m_commandList[i] as BaseCommand;
                    if (cmd == null || cmd.isFinished())
                    {
                        m_commandList.RemoveAt(i);
                        continue;
                    }
                    cmd.update();
                }
               
            }


        }
    }
}
