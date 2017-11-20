using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Game
{
    
    public class ObjectPool
    {
        string m_name;
        GameObject m_prefab;
        Stack<GameObject> m_pool;
        int m_size;

        public string PoolName
        {
            get
            {
                return m_name;
            }
        }

        public ObjectPool(string name, GameObject prefab)
        {
            m_pool = new Stack<GameObject>();
            m_size = 200;
            m_name = name;
            m_prefab = prefab;
        }

        public int Size
        {
            get
            {
                return m_size;
            }
            set
            {
                m_size = value;
                if (m_size <= 0)
                {
                    m_size = 1;
                }
                else
                {
                    while (m_pool.Count > m_size)
                    {
                        GameObject obj = m_pool.Pop();
                        GameObject.DestroyObject(obj);
                    }
                }
            }
        }

        public void Clear()
        {
            while(m_pool.Count > 0)
            {
                GameObject obj = m_pool.Pop();
                GameObject.DestroyObject(obj);
            }
        }

        public void RecycleObj(GameObject obj)
        {
            if(obj != null)
            {
                PooledObject pobj = obj.GetComponent<PooledObject>();
                if (pobj == null)
                {
                    Debug.LogWarning("is not pooled object!");
                    return;
                }
                pobj.Recycle();
                m_pool.Push(obj);
            }
        }

        public GameObject Get()
        {
             GameObject obj;
            if(m_pool.Count > 0)
            {
                obj = m_pool.Pop();
                PooledObject pobj = obj.GetComponent<PooledObject>();
                pobj.Reset();
                
            }else
            {
                if(m_prefab == null)
                {
                    return null;
                }
                obj = GameObject.Instantiate(m_prefab);
                PooledObject pobj = obj.GetComponent<PooledObject>();
                pobj.poolName = m_name;
                pobj.Reset();
            }
            return obj;
        }
    }

    public class ObjecPoolManager
    {
        Dictionary<string, ObjectPool> m_pools;

        public ObjecPoolManager()
        {
            m_pools = new Dictionary<string, ObjectPool>();
           
        }

        public void Init()
        {
            RegisterNewPool<ConsoleItemWidget>(GlobalClient.prefabData["ConsoleItem"]);
        }

        public void RegisterNewPool<T>(GameObject prefab)
        {
            Type type = typeof(T);
            string name = type.Name;
            if(m_pools.ContainsKey(name))
            {
                m_pools[name].Clear();
            }
            m_pools[name] = new ObjectPool(name, prefab);
        }


        public GameObject Get<T>()
        {
            GameObject obj = null;
            Type type = typeof(T);
            if(m_pools.ContainsKey(type.Name))
            {
                return m_pools[type.Name].Get();
            }else
            {
                Debug.LogError("not registered pool");
            }

            return obj;

        }

        public void Recycle(GameObject obj)
        {
            if(obj != null)
            {
                PooledObject pobj = obj.GetComponent<PooledObject>();
                if(pobj != null && m_pools.ContainsKey(pobj.poolName))
                {
                    m_pools[pobj.poolName].RecycleObj(pobj.gameObject);
                    return;
                }
                Debug.LogError("is not pooled object!");
            }
        } 

    }
}

