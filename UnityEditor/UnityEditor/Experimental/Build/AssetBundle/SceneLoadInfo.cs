// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.Build.AssetBundle.SceneLoadInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.ObjectModel;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.AssetBundle
{
  [UsedByNativeCode]
  [Serializable]
  public struct SceneLoadInfo
  {
    [NativeName("scene")]
    internal string m_Scene;
    [NativeName("processedScene")]
    internal string m_ProcessedScene;
    [NativeName("referencedObjects")]
    internal ObjectIdentifier[] m_ReferencedObjects;
    [NativeName("globalUsage")]
    internal BuildUsageTagGlobal m_GlobalUsage;
    [NativeName("resourceFiles")]
    internal ResourceFile[] m_ResourceFiles;

    public string scene
    {
      get
      {
        return this.m_Scene;
      }
    }

    public string processedScene
    {
      get
      {
        return this.m_ProcessedScene;
      }
    }

    public ReadOnlyCollection<ObjectIdentifier> referencedObjects
    {
      get
      {
        return Array.AsReadOnly<ObjectIdentifier>(this.m_ReferencedObjects);
      }
    }

    public BuildUsageTagGlobal globalUsage
    {
      get
      {
        return this.m_GlobalUsage;
      }
    }

    public ReadOnlyCollection<ResourceFile> resourceFiles
    {
      get
      {
        return Array.AsReadOnly<ResourceFile>(this.m_ResourceFiles);
      }
    }
  }
}
