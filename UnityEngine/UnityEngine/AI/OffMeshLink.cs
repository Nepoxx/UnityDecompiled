// Decompiled with JetBrains decompiler
// Type: UnityEngine.AI.OffMeshLink
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
  /// <summary>
  ///   <para>Link allowing movement outside the planar navigation mesh.</para>
  /// </summary>
  [MovedFrom("UnityEngine")]
  public sealed class OffMeshLink : Behaviour
  {
    /// <summary>
    ///   <para>Is link active.</para>
    /// </summary>
    public extern bool activated { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is link occupied. (Read Only)</para>
    /// </summary>
    public extern bool occupied { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Modify pathfinding cost for the link.</para>
    /// </summary>
    public extern float costOverride { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Can link be traversed in both directions.</para>
    /// </summary>
    public extern bool biDirectional { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Explicitly update the link endpoints.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void UpdatePositions();

    /// <summary>
    ///   <para>NavMeshLayer for this OffMeshLink component.</para>
    /// </summary>
    [Obsolete("Use area instead.")]
    public extern int navMeshLayer { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>NavMesh area index for this OffMeshLink component.</para>
    /// </summary>
    public extern int area { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Automatically update endpoints.</para>
    /// </summary>
    public extern bool autoUpdatePositions { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The transform representing link start position.</para>
    /// </summary>
    public extern Transform startTransform { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The transform representing link end position.</para>
    /// </summary>
    public extern Transform endTransform { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
