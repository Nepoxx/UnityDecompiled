// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorCache
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class EditorCache : IDisposable
  {
    private Dictionary<UnityEngine.Object, EditorWrapper> m_EditorCache;
    private Dictionary<UnityEngine.Object, bool> m_UsedEditors;
    private EditorFeatures m_Requirements;

    public EditorCache()
      : this(EditorFeatures.None)
    {
    }

    public EditorCache(EditorFeatures requirements)
    {
      this.m_Requirements = requirements;
      this.m_EditorCache = new Dictionary<UnityEngine.Object, EditorWrapper>();
      this.m_UsedEditors = new Dictionary<UnityEngine.Object, bool>();
    }

    public EditorWrapper this[UnityEngine.Object o]
    {
      get
      {
        this.m_UsedEditors[o] = true;
        if (this.m_EditorCache.ContainsKey(o))
          return this.m_EditorCache[o];
        EditorWrapper editorWrapper = EditorWrapper.Make(o, this.m_Requirements);
        this.m_EditorCache[o] = editorWrapper;
        return editorWrapper;
      }
    }

    public void CleanupUntouchedEditors()
    {
      List<UnityEngine.Object> objectList = new List<UnityEngine.Object>();
      foreach (UnityEngine.Object key in this.m_EditorCache.Keys)
      {
        if (!this.m_UsedEditors.ContainsKey(key))
          objectList.Add(key);
      }
      if (this.m_EditorCache != null)
      {
        foreach (UnityEngine.Object key in objectList)
        {
          EditorWrapper editorWrapper = this.m_EditorCache[key];
          this.m_EditorCache.Remove(key);
          if (editorWrapper != null)
            editorWrapper.Dispose();
        }
      }
      this.m_UsedEditors.Clear();
    }

    public void CleanupAllEditors()
    {
      this.m_UsedEditors.Clear();
      this.CleanupUntouchedEditors();
    }

    public void Dispose()
    {
      this.CleanupAllEditors();
      GC.SuppressFinalize((object) this);
    }

    ~EditorCache()
    {
      Debug.LogError((object) "Failed to dispose EditorCache.");
    }
  }
}
