﻿using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class LocalResourceLoader<TResource> : LoadResourceRunner<TResource> where TResource : class
{
    public string RootPath { get; private set; }

    private Action<string> logAction;
    private IRawConverter<TResource> converter;
    private RawDataRepresentation usedRepresentation;
    private byte[] rawData;

    public LocalResourceLoader (string rootPath, Resource<TResource> resource, 
        IRawConverter<TResource> converter, Action<string> logAction = null)
    {
        RootPath = rootPath;
        Resource = resource;
        this.logAction = logAction;
        this.converter = converter;
        usedRepresentation = new RawDataRepresentation();
    }

    public override async Task Run ()
    {
        await base.Run();

        var startTime = Time.time;

        // Corner case when loading folders.
        if (typeof(TResource) == typeof(Folder))
        {
            (Resource as Resource<Folder>).Object = new Folder(Resource.Path);
            HandleOnCompleted();
            return;
        }

        var filePath = string.IsNullOrEmpty(RootPath) ? Resource.Path : string.Concat(RootPath, '/', Resource.Path);
        filePath = string.Concat(Application.dataPath, "/", filePath);

        foreach (var representation in converter.Representations)
        {
            usedRepresentation = representation;
            var fullPath = string.Concat(filePath, ".", representation.Extension);
            if (!File.Exists(fullPath)) continue;

            rawData = await IOUtils.ReadFileAsync(fullPath);
            break;
        }

        if (rawData == null) Debug.LogError(string.Format("Failed to load {0}.{1} resource using local file system: File not found.", Resource.Path, usedRepresentation.Extension));
        else Resource.Object = await converter.ConvertAsync(rawData);

        logAction?.Invoke($"Resource '{Resource.Path}' loaded {(rawData.Length / 1024f) / 1024f:0.###}MB over {Time.time - startTime:0.###} seconds.");

        HandleOnCompleted();
    }
}
