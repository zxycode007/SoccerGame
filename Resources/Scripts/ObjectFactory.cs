using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ObjectFactoryImpl<T> : ObjectFactory where T : new()
{

    public ObjectFactoryImpl()
    {
        objectType = typeof(T);
        objectName = objectType.Name;
    }

    public override object createObject()
    {
        return new T();
    }

}


public class ObjectFactory
{
    public string objectName { get; protected set; }
    public Type objectType { get; protected set; }
    public virtual object createObject() { return null; }
}
