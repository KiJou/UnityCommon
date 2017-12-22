﻿using UnityEngine;

/// <summary>
/// Represents a resource object stored at the specified path. 
/// </summary>
public class Resource
{
    public string Path { get; private set; }
    public object Object { get; set; }
    public bool IsValid { get { return Object != null; } }
    public bool IsUnityObject { get { return Object is Object; } }
    public Object AsUnityObject { get { return Object as Object; } }

    public Resource (string path, object obj = null)
    {
        Path = path;
        Object = obj;
    }
}

/// <summary>
/// A strongly typed version of the <see cref="Resource"/>.
/// </summary>
/// <typeparam name="T">Type of the resource object.</typeparam>
public class Resource<T> : Resource where T : class
{
    public new T Object { get { return CastObject(base.Object); } set { base.Object = value; } }

    public Resource (string path, T obj = null) : base(path, obj) { }

    private T CastObject (object resourceObject)
    {
        var castedResource = resourceObject as T;
        if (castedResource == null)
        {
            Debug.LogError(string.Format("Resource '{0}' is not of type '{1}'.", Path, typeof(T).Name));
            return null;
        }
        return castedResource;
    }
}