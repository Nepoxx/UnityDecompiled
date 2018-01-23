// Decompiled with JetBrains decompiler
// Type: UnityEditor.UISystemProfilerRenderService
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [InitializeOnLoad]
  internal class UISystemProfilerRenderService : IDisposable
  {
    private UISystemProfilerRenderService.LRUCache m_Cache;
    private bool m_Disposed;

    public UISystemProfilerRenderService()
    {
      this.m_Cache = new UISystemProfilerRenderService.LRUCache(10);
    }

    public void Dispose()
    {
      this.m_Disposed = true;
      this.m_Cache.Clear();
    }

    private Texture2D Generate(int frameIndex, int renderDataIndex, int renderDataCount, bool overdraw)
    {
      return !this.m_Disposed ? ProfilerProperty.UISystemProfilerRender(frameIndex, renderDataIndex, renderDataCount, overdraw) : (Texture2D) null;
    }

    public Texture2D GetThumbnail(int frameIndex, int renderDataIndex, int infoRenderDataCount, bool overdraw)
    {
      if (this.m_Disposed)
        return (Texture2D) null;
      Texture2D texture2D = (Texture2D) null;
      if ((UnityEngine.Object) texture2D == (UnityEngine.Object) null)
        texture2D = this.Generate(frameIndex, renderDataIndex, infoRenderDataCount, overdraw);
      return texture2D;
    }

    private class LRUCache
    {
      private int m_Capacity;
      private Dictionary<long, Texture2D> m_Cache;
      private List<long> m_CacheQueue;
      private int m_CacheQueueFront;

      public LRUCache(int capacity)
      {
        if (capacity <= 0)
          capacity = 16;
        this.m_Capacity = capacity;
        this.m_Cache = new Dictionary<long, Texture2D>(this.m_Capacity);
        this.m_CacheQueue = new List<long>(Enumerable.Repeat<long>(-1L, this.m_Capacity).Select<long, long>((Func<long, long>) (value => value)));
        this.m_CacheQueueFront = 0;
      }

      public void Clear()
      {
        foreach (long cache in this.m_CacheQueue)
        {
          Texture2D t;
          if (this.m_Cache.TryGetValue(cache, out t))
            ProfilerProperty.ReleaseUISystemProfilerRender(t);
        }
        this.m_Cache.Clear();
        this.m_CacheQueue.Clear();
        this.m_CacheQueue.AddRange(Enumerable.Repeat<long>(-1L, this.m_Capacity).Select<long, long>((Func<long, long>) (value => value)));
        this.m_CacheQueueFront = 0;
      }

      public void Add(long key, Texture2D data)
      {
        if (!((UnityEngine.Object) this.Get(key) == (UnityEngine.Object) null))
          return;
        if (this.m_CacheQueue[this.m_CacheQueueFront] != -1L)
        {
          long cache = this.m_CacheQueue[this.m_CacheQueueFront];
          Texture2D t;
          if (this.m_Cache.TryGetValue(cache, out t))
          {
            this.m_Cache.Remove(cache);
            ProfilerProperty.ReleaseUISystemProfilerRender(t);
          }
        }
        this.m_CacheQueue[this.m_CacheQueueFront] = key;
        this.m_Cache[key] = data;
        ++this.m_CacheQueueFront;
        if (this.m_CacheQueueFront == this.m_Capacity)
          this.m_CacheQueueFront = 0;
      }

      public Texture2D Get(long key)
      {
        Texture2D texture2D;
        if (!this.m_Cache.TryGetValue(key, out texture2D))
          return (Texture2D) null;
        this.m_CacheQueue[this.m_CacheQueue.IndexOf(key)] = this.m_CacheQueue[this.m_CacheQueueFront];
        this.m_CacheQueue[this.m_CacheQueueFront] = key;
        ++this.m_CacheQueueFront;
        if (this.m_CacheQueueFront == this.m_Capacity)
          this.m_CacheQueueFront = 0;
        return texture2D;
      }
    }
  }
}
