// Decompiled with JetBrains decompiler
// Type: UnityEngine.SpringJoint
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The spring joint ties together 2 rigid bodies, spring forces will be automatically applied to keep the object at the given distance.</para>
  /// </summary>
  public sealed class SpringJoint : Joint
  {
    /// <summary>
    ///   <para>The spring force used to keep the two objects together.</para>
    /// </summary>
    public extern float spring { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The damper force used to dampen the spring force.</para>
    /// </summary>
    public extern float damper { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The minimum distance between the bodies relative to their initial distance.</para>
    /// </summary>
    public extern float minDistance { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum distance between the bodies relative to their initial distance.</para>
    /// </summary>
    public extern float maxDistance { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The maximum allowed error between the current spring length and the length defined by minDistance and maxDistance.</para>
    /// </summary>
    public extern float tolerance { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
