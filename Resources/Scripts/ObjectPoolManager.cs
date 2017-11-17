using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class ObjectPool
{
    public string objectName { get; protected set; }
    public Type objectType { get; protected set; }

    public  abstract void Enqueue(object obj);

    public  abstract object Dequeue();

    public  abstract int Size();
    
}

public class ObjectPoolImpl<T> : ObjectPool  where T : new()
{
    
    Queue<T> recycleQueue;

    public ObjectPoolImpl()
    {
        objectType = typeof(T);
        objectName = objectType.Name;
        recycleQueue = new Queue<T>();
    }

    public override void Enqueue(object obj)
    {
        if(obj.GetType() == objectType)
        {
            recycleQueue.Enqueue((T)obj);
        }
        
    }

    public override object Dequeue()
    {
        return recycleQueue.Dequeue();
    }

    public override int Size()
    {
        return recycleQueue.Count;
    }

    
   
}
 
public class ObjecPoolManager
{

    Dictionary<string, ObjectPool> m_pools;
    public int maxCacheCount = 100;

    public ObjecPoolManager()
    {
        m_pools = new Dictionary<string, ObjectPool>();
    }

    public T Get<T>()
    {
        string objectName = typeof(T).Name;
        if(m_pools.ContainsKey(objectName) && m_pools[objectName].Size() > 0)
        {
            return (T)m_pools[objectName].Dequeue();
        }
        return default(T);
    }

    public void Recycle<T>(T obj) where T : new()
    {
        
        Type objType = typeof(T);
        string objectName = objType.Name;
        if (m_pools.ContainsKey(objectName) && m_pools[objectName].Size() < maxCacheCount)
        {
            m_pools[objectName].Enqueue(obj);
        }else
        {

            m_pools[objectName] = new ObjectPoolImpl<T>();
            m_pools[objectName].Enqueue(obj);
        }
    }

}