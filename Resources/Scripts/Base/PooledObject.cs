using UnityEngine;
using System.Collections;

namespace Game
{
    public class PooledObject : MonoBehaviour
    {
        string m_poolName;

        public string poolName
        {
            get
            {
                return m_poolName;
            }
            set
            {
                m_poolName = value;
            }
        }

        /// <summary>
        /// 重新设初值
        /// </summary>
        public virtual void Reset()
        {

        }

        

        /// <summary>
        /// 回收处理方法
        /// </summary>
         public virtual void Recycle()
         {
             
         }
        
    }
}

