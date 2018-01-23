// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.Build.AssetBundle.ResourceFile
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.AssetBundle
{
  [UsedByNativeCode]
  [Serializable]
  public struct ResourceFile
  {
    [NativeName("fileName")]
    internal string m_FileName;
    [NativeName("fileAlias")]
    internal string m_FileAlias;
    [NativeName("serializedFile")]
    internal bool m_SerializedFile;

    public string fileName
    {
      get
      {
        return this.m_FileName;
      }
    }

    public string fileAlias
    {
      get
      {
        return this.m_FileAlias;
      }
    }

    public bool serializedFile
    {
      get
      {
        return this.m_SerializedFile;
      }
    }
  }
}
