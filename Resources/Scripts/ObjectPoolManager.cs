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

    public abstract void Reset();

    public abstract object NewObject();
    
}

/// <summary>
/// 基础类型缓存池实现
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPoolImpl<T> : ObjectPool where T : class, MonoBehaviourEx, new()
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


    public override object NewObject()
    {
        object ret = new T();
        return ret;
    }

    public override void Reset()
    {
        
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

    public T Get<T>() where T : class, MonoBehaviourEx, new()
    {
        string objectName = typeof(T).Name;
        if(!m_pools.ContainsKey(objectName) )
        {
            if(m_pools[objectName].Size() > 0)
            return (T)m_pools[objectName].Dequeue();
        }else
        {
            m_pools[objectName] = new ObjectPoolImpl<T>();
            return (T)m_pools[objectName].NewObject();
        }
        return default(T);
    }

    public void Recycle<T>(T obj) where T : class, MonoBehaviourEx, new()
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