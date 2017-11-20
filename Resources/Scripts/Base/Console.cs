using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;  


namespace Game
{

    public enum EConsoleInfoType
    {
        EConsoleInfoType_Message,
        EConsoleInfoType_Warning,
        EConsoleInfoType_Error
    }

    public class ConsoleInfo
    {
        EConsoleInfoType m_type;
        string m_message;
        System.DateTime m_time;


        public ConsoleInfo()
        {
            m_time = DateTime.Now;
        }

        public ConsoleInfo(string msg, EConsoleInfoType type)
        {
            m_type = type;
            m_message = msg;
            m_time = DateTime.Now;
        }

    }

    public class Console
    {
        Stack<ConsoleInfo> infoStack;

        int maxcount;

        public int MaxCount
        {
            get
            {
                return maxcount;
            }
            set
            {
                maxcount = value;
            }
        }

        public Console()
        {
            infoStack = new Stack<ConsoleInfo>();
        }

        public void Log(string msg)
        {
            ConsoleInfo info = new ConsoleInfo(msg, EConsoleInfoType.EConsoleInfoType_Message);
            while (infoStack.Count >= maxcount)
            {
                Pop();
            }
            infoStack.Push(info);
        }

        public void LogWarning(string msg)
        {
            ConsoleInfo info = new ConsoleInfo(msg, EConsoleInfoType.EConsoleInfoType_Warning);
            while (infoStack.Count >= maxcount)
            {
                Pop();
            }
            infoStack.Push(info);
        }

        public void LogError(string str)
        {
#if DEBUG
            StackTrace insStackTrace = new StackTrace(true);
            StackFrame insStackFrame = insStackTrace.GetFrame(1);
            string fileName = insStackFrame.GetFileName();
            int fileLines = insStackFrame.GetFileLineNumber();
            int fileColumns = insStackFrame.GetFileColumnNumber();
            string msg = string.Format("FileName::{0} LineNum::{1} :: ColumnsNum{2}   {3}", fileName, fileLines, fileColumns, str);
#else
        string msg = str;
#endif
            ConsoleInfo info = new ConsoleInfo(msg, EConsoleInfoType.EConsoleInfoType_Error);
            while (infoStack.Count >= maxcount)
            {
                Pop();
            }
            infoStack.Push(info);
        }

        void Push(string msg, EConsoleInfoType type)
        {
            while (infoStack.Count >= maxcount)
            {
                Pop();
            }
            ConsoleInfo info = new ConsoleInfo(msg, type);
            infoStack.Push(info);
        }

        ConsoleInfo Pop()
        {
            return infoStack.Pop();
        }

        public ConsoleInfo[] ToArray()
        {
            return infoStack.ToArray();
        }

    }

}

