// Decompiled with JetBrains decompiler
// Type: UnityEditor.Lightmapping
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Allows to control the lightmapping job.</para>
  /// </summary>
  public sealed class Lightmapping
  {
    /// <summary>
    ///   <para>Delegate which is called when bake job is completed.</para>
    /// </summary>
    public static Lightmapping.OnCompletedFunction completed;

    /// <summary>
    ///   <para>The lightmap baking workflow mode used. Iterative mode is default, but you can switch to on demand mode which bakes only when the user presses the bake button.</para>
    /// </summary>
    public static extern Lightmapping.GIWorkflowMode giWorkflowMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is realtime GI enabled?</para>
    /// </summary>
    public static extern bool realtimeGI { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is baked GI enabled?</para>
    /// </summary>
    public static extern bool bakedGI { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Scale for indirect lighting.</para>
    /// </summary>
    public static extern float indirectOutputScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Boost the albedo.</para>
    /// </summary>
    public static extern float bounceBoost { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern Lightmapping.ConcurrentJobsType concurrentJobsType { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Clears the cache used by lightmaps, reflection probes and default reflection.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearDiskCache();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateCachePath();

    internal static extern long diskCacheSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern string diskCachePath { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    internal static extern bool enlightenForceWhiteAlbedo { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern bool enlightenForceUpdates { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern UnityEngine.FilterMode filterMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern ulong occupiedTexelCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern ulong GetVisibleTexelCount(int lightmapIndex);

    internal static LightmapConvergence GetLightmapConvergence(int lightmapIndex)
    {
      LightmapConvergence lightmapConvergence;
      Lightmapping.INTERNAL_CALL_GetLightmapConvergence(lightmapIndex, out lightmapConvergence);
      return lightmapConvergence;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetLightmapConvergence(int lightmapIndex, out LightmapConvergence value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern float GetLightmapBakeTimeRaw();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern float GetLightmapBakeTimeTotal();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern float GetLightmapBakePerformanceTotal();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern float GetLightmapBakePerformance(int lightmapIndex);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void PrintStateToConsole();

    /// <summary>
    ///   <para>Starts an asynchronous bake job.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool BakeAsync();

    /// <summary>
    ///   <para>Stars a synchronous bake job.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool Bake();

    /// <summary>
    ///   <para>Cancels the currently running asynchronous bake job.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Cancel();

    /// <summary>
    ///   <para>Force the Progressive Path Tracer to stop baking and use the computed results as they are.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ForceStop();

    /// <summary>
    ///   <para>Returns true when the bake job is running, false otherwise (Read Only).</para>
    /// </summary>
    public static extern bool isRunning { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    private static void Internal_CallCompletedFunctions()
    {
      if (Lightmapping.completed == null)
        return;
      Lightmapping.completed();
    }

    /// <summary>
    ///   <para>Returns the current lightmapping build progress or 0 if Lightmapping.isRunning is false.</para>
    /// </summary>
    public static extern float buildProgress { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Deletes all lightmap assets and makes all lights behave as if they weren't baked yet.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Clear();

    /// <summary>
    ///   <para>Remove the lighting data asset used by the current scene.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ClearLightingDataAsset();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Tetrahedralize(Vector3[] positions, out int[] outIndices, out Vector3[] outPositions);

    /// <summary>
    ///   <para>Starts a synchronous bake job for the probe.</para>
    /// </summary>
    /// <param name="probe">Target probe.</param>
    /// <param name="path">The location where cubemap will be saved.</param>
    /// <returns>
    ///   <para>Returns true if baking was succesful.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool BakeReflectionProbe(UnityEngine.ReflectionProbe probe, string path);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool BakeReflectionProbeSnapshot(UnityEngine.ReflectionProbe probe);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool BakeAllReflectionProbesSnapshots();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void OnUpdateLightmapEncoding(BuildTargetGroup target);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void GetTerrainGIChunks(Terrain terrain, ref int numChunksX, ref int numChunksY);

    /// <summary>
    ///   <para>The lighting data asset used by the active scene.</para>
    /// </summary>
    public static extern LightingDataAsset lightingDataAsset { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Bakes an array of scenes.</para>
    /// </summary>
    /// <param name="paths">The path of the scenes that should be baked.</param>
    public static void BakeMultipleScenes(string[] paths)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Lightmapping.\u003CBakeMultipleScenes\u003Ec__AnonStorey0 scenesCAnonStorey0 = new Lightmapping.\u003CBakeMultipleScenes\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      scenesCAnonStorey0.paths = paths;
      // ISSUE: reference to a compiler-generated field
      if (scenesCAnonStorey0.paths.Length == 0)
        return;
      // ISSUE: reference to a compiler-generated field
      for (int index1 = 0; index1 < scenesCAnonStorey0.paths.Length; ++index1)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index2 = index1 + 1; index2 < scenesCAnonStorey0.paths.Length; ++index2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (scenesCAnonStorey0.paths[index1] == scenesCAnonStorey0.paths[index2])
            throw new Exception("no duplication of scenes is allowed");
        }
      }
      if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        return;
      // ISSUE: reference to a compiler-generated field
      scenesCAnonStorey0.sceneSetup = EditorSceneManager.GetSceneManagerSetup();
      // ISSUE: reference to a compiler-generated field
      scenesCAnonStorey0.OnBakeFinish = (Lightmapping.OnCompletedFunction) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      scenesCAnonStorey0.OnBakeFinish = new Lightmapping.OnCompletedFunction(scenesCAnonStorey0.\u003C\u003Em__0);
      // ISSUE: reference to a compiler-generated field
      scenesCAnonStorey0.BakeOnAllOpen = (EditorSceneManager.SceneOpenedCallback) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      scenesCAnonStorey0.BakeOnAllOpen = new EditorSceneManager.SceneOpenedCallback(scenesCAnonStorey0.\u003C\u003Em__1);
      // ISSUE: reference to a compiler-generated field
      EditorSceneManager.sceneOpened += scenesCAnonStorey0.BakeOnAllOpen;
      // ISSUE: reference to a compiler-generated field
      EditorSceneManager.OpenScene(scenesCAnonStorey0.paths[0]);
      // ISSUE: reference to a compiler-generated field
      for (int index = 1; index < scenesCAnonStorey0.paths.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        EditorSceneManager.OpenScene(scenesCAnonStorey0.paths[index], OpenSceneMode.Additive);
      }
    }

    [Obsolete("lightmapSnapshot has been deprecated. Use lightingDataAsset instead (UnityUpgradable) -> lightingDataAsset", true)]
    public static LightmapSnapshot lightmapSnapshot
    {
      get
      {
        return (LightmapSnapshot) null;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Starts an asynchronous bake job for the selected objects.</para>
    /// </summary>
    [Obsolete("BakeSelectedAsync has been deprecated. Use BakeAsync instead (UnityUpgradable) -> BakeAsync()", true)]
    public static bool BakeSelectedAsync()
    {
      return false;
    }

    /// <summary>
    ///   <para>Starts a synchronous bake job for the selected objects.</para>
    /// </summary>
    [Obsolete("BakeSelected has been deprecated. Use Bake instead (UnityUpgradable) -> Bake()", true)]
    public static bool BakeSelected()
    {
      return false;
    }

    /// <summary>
    ///   <para>Starts an asynchronous bake job, but only bakes light probes.</para>
    /// </summary>
    [Obsolete("BakeLightProbesOnlyAsync has been deprecated. Use BakeAsync instead (UnityUpgradable) -> BakeAsync()", true)]
    public static bool BakeLightProbesOnlyAsync()
    {
      return false;
    }

    /// <summary>
    ///   <para>Starts a synchronous bake job, but only bakes light probes.</para>
    /// </summary>
    [Obsolete("BakeLightProbesOnly has been deprecated. Use Bake instead (UnityUpgradable) -> Bake()", true)]
    public static bool BakeLightProbesOnly()
    {
      return false;
    }

    internal enum ConcurrentJobsType
    {
      Min,
      Low,
      High,
    }

    /// <summary>
    ///   <para>Workflow mode for lightmap baking. Default is Iterative.</para>
    /// </summary>
    public enum GIWorkflowMode
    {
      Iterative,
      OnDemand,
      Legacy,
    }

    /// <summary>
    ///   <para>Delegate used by Lightmapping.completed callback.</para>
    /// </summary>
    public delegate void OnCompletedFunction();
  }
}
