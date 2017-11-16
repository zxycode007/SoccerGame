using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ObjectPool
{
    public string objectName { get; protected set; }
    public Type objectType { get; protected set; }

    public virtual object GetObject();

    public virtual void   Recycle(object obj);
    
}
public class ObjectPoolImpl<T>  : ObjectPool where T : new()
{

}
 
public class ObjecPoolManager
{

     

}