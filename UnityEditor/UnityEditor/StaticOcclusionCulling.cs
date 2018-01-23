// Decompiled with JetBrains decompiler
// Type: UnityEditor.StaticOcclusionCulling
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>StaticOcclusionCulling lets you perform static occlusion culling operations.</para>
  /// </summary>
  public sealed class StaticOcclusionCulling
  {
    /// <summary>
    ///   <para>Used to generate static occlusion culling data.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Compute();

    /// <summary>
    ///   <para>Used to compute static occlusion culling data asynchronously.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool GenerateInBackground();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void InvalidatePrevisualisationData();

    /// <summary>
    ///   <para>Used to cancel asynchronous generation of static occlusion culling data.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Cancel();

    /// <summary>
    ///   <para>Used to check if asynchronous generation of static occlusion culling data is still running.</para>
    /// </summary>
    public static extern bool isRunning { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Clears the PVS of the opened scene.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Clear();

    public static extern float smallestOccluder { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern float smallestHole { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern float backfaceThreshold { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Does the scene contain any occlusion portals that were added manually rather than automatically?</para>
    /// </summary>
    public static extern bool doesSceneHaveManualPortals { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns the size in bytes that the PVS data is currently taking up in this scene on disk.</para>
    /// </summary>
    public static extern int umbraDataSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetDefaultOcclusionBakeSettings();
  }
}
