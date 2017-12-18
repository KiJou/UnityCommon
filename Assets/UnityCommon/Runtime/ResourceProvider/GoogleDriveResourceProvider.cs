﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides resources stored in Google Drive.
/// Will only work for the resources covered by the available converters; 
/// use <see cref="AddConverter{T}(IRawConverter{T})"/> to extend covered resource types.
/// </summary>
public class GoogleDriveResourceProvider : MonoRunnerResourceProvider
{
    /// <summary>
    /// Path to the drive folder where resources are located.
    /// </summary>
    public string DriveRootPath { get; set; }

    private Dictionary<Type, object> converters = new Dictionary<Type, object>();

    /// <summary>
    /// Adds a resource type converter.
    /// </summary>
    public void AddConverter<T> (IRawConverter<T> converter) where T : UnityEngine.Object
    {
        converters.Add(typeof(T), converter);
    }

    protected override AsyncRunner<UnityResource<T>> CreateLoadRunner<T> (UnityResource<T> resource) 
    {
        return new GoogleDriveResourceLoader<T>(DriveRootPath, resource, ResolveConverter<T>(), this);
    }

    protected override AsyncAction<List<UnityResource<T>>> LocateResourcesAtPath<T> (string path)
    {
        return new GoogleDriveResourceLocator<T>(DriveRootPath, path, ResolveConverter<T>(), this).Run();
    }

    protected override void UnloadResource (UnityResource resource)
    {
        if (resource.IsValid) Destroy(resource.Object);
    }

    private IRawConverter<T> ResolveConverter<T> ()
    {
        var resourceType = typeof(T);
        if (!converters.ContainsKey(resourceType))
        {
            Debug.LogError(string.Format("Converter for resource of type '{0}' is not available.", resourceType.Name));
            return null;
        }
        return converters[resourceType] as IRawConverter<T>;
    }
}
