// Decompiled with JetBrains decompiler
// Type: UnityEditor.VersionControl.ChangeSet
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
  ///   <para>Wrapper around a changeset description and ID.</para>
  /// </summary>
  public sealed class ChangeSet
  {
    /// <summary>
    ///   <para>The ID of  the default changeset.</para>
    /// </summary>
    public static string defaultID = "-1";
    private IntPtr m_thisDummy;

    public ChangeSet()
    {
      this.InternalCreate();
    }

    public ChangeSet(string description)
    {
      this.InternalCreateFromString(description);
    }

    public ChangeSet(string description, string revision)
    {
      this.InternalCreateFromStringString(description, revision);
    }

    public ChangeSet(ChangeSet other)
    {
      this.InternalCopyConstruct(other);
    }

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void InternalCreate();

    [GeneratedByOldBindingsGenerator]
    [ThreadAndSerializationSafe]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void InternalCopyConstruct(ChangeSet other);

    [GeneratedByOldBindingsGenerator]
    [ThreadAndSerializationSafe]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void InternalCreateFromString(string description);

    [GeneratedByOldBindingsGenerator]
    [ThreadAndSerializationSafe]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void InternalCreateFromStringString(string description, string changeSetID);

    [GeneratedByOldBindingsGenerator]
    [ThreadAndSerializationSafe]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Dispose();

    /// <summary>
    ///   <para>Description of a changeset.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern string description { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Version control specific ID of a changeset.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern string id { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    ~ChangeSet()
    {
      this.Dispose();
    }
  }
}
