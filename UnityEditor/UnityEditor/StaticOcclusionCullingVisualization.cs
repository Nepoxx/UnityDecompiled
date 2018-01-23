// Decompiled with JetBrains decompiler
// Type: UnityEditor.StaticOcclusionCullingVisualization
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Used to visualize static occlusion culling at development time in scene view.</para>
  /// </summary>
  public sealed class StaticOcclusionCullingVisualization
  {
    /// <summary>
    ///   <para>If set to true, visualization of target volumes is enabled.</para>
    /// </summary>
    public static extern bool showOcclusionCulling { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If set to true, the visualization lines of the PVS volumes will show all cells rather than cells after culling.</para>
    /// </summary>
    public static extern bool showPreVisualization { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If set to true, visualization of view volumes is enabled.</para>
    /// </summary>
    public static extern bool showViewVolumes { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool showDynamicObjectBounds { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If set to true, visualization of portals is enabled.</para>
    /// </summary>
    public static extern bool showPortals { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If set to true, visualization of portals is enabled.</para>
    /// </summary>
    public static extern bool showVisibilityLines { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If set to true, culling of geometry is enabled.</para>
    /// </summary>
    public static extern bool showGeometryCulling { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool isPreviewOcclusionCullingCameraInPVS { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern Camera previewOcclusionCamera { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern Camera previewOcclucionCamera { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
