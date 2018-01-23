// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.ConfigField
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.VersionControl
{
  /// <summary>
  ///   <para>This class describes the.</para>
  /// </summary>
  public sealed class ConfigField
  {
    private IntPtr m_thisDummy;
    private string m_guid;

    internal ConfigField()
    {
    }

    [GeneratedByOldBindingsGenerator]
    [ThreadAndSerializationSafe]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Dispose();

    ~ConfigField()
    {
      this.Dispose();
    }

    /// <summary>
    ///   <para>Name of the configuration field.</para>
    /// </summary>
    public extern string name { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Label that is displayed next to the configuration field in the editor.</para>
    /// </summary>
    public extern string label { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Descrition of the configuration field.</para>
    /// </summary>
    public extern string description { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>This is true if the configuration field is required for the version control plugin to function correctly.</para>
    /// </summary>
    public extern bool isRequired { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>This is true if the configuration field is a password field.</para>
    /// </summary>
    public extern bool isPassword { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
