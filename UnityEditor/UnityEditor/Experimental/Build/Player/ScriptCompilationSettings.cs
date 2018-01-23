// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.Build.Player.ScriptCompilationSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.Player
{
  [UsedByNativeCode]
  [Serializable]
  public struct ScriptCompilationSettings
  {
    [NativeName("target")]
    internal BuildTarget m_Target;
    [NativeName("group")]
    internal BuildTargetGroup m_Group;
    [NativeName("options")]
    internal ScriptCompilationOptions m_Options;
    [NativeName("resultTypeDB")]
    internal TypeDB m_ResultTypeDB;

    public BuildTarget target
    {
      get
      {
        return this.m_Target;
      }
      set
      {
        this.m_Target = value;
      }
    }

    public BuildTargetGroup group
    {
      get
      {
        return this.m_Group;
      }
      set
      {
        this.m_Group = value;
      }
    }

    public ScriptCompilationOptions options
    {
      get
      {
        return this.m_Options;
      }
      set
      {
        this.m_Options = value;
      }
    }
  }
}
