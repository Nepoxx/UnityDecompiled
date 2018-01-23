// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.Build.Player.ScriptCompilationResult
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.ObjectModel;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.Player
{
  [UsedByNativeCode]
  [Serializable]
  public struct ScriptCompilationResult
  {
    [NativeName("assemblies")]
    internal string[] m_Assemblies;
    [Ignore]
    internal TypeDB m_TypeDB;

    public ReadOnlyCollection<string> assemblies
    {
      get
      {
        return Array.AsReadOnly<string>(this.m_Assemblies);
      }
    }

    public TypeDB typeDB
    {
      get
      {
        return this.m_TypeDB;
      }
    }
  }
}
