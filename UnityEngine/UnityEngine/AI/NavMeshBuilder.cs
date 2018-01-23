// Decompiled with JetBrains decompiler
// Type: UnityEngine.AI.NavMeshBuilder
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.AI
{
  /// <summary>
  ///   <para>Navigation mesh builder interface.</para>
  /// </summary>
  public static class NavMeshBuilder
  {
    public static void CollectSources(Bounds includedWorldBounds, int includedLayerMask, NavMeshCollectGeometry geometry, int defaultArea, List<NavMeshBuildMarkup> markups, List<NavMeshBuildSource> results)
    {
      if (markups == null)
        throw new ArgumentNullException(nameof (markups));
      if (results == null)
        throw new ArgumentNullException(nameof (results));
      includedWorldBounds.extents = Vector3.Max(includedWorldBounds.extents, 1f / 1000f * Vector3.one);
      NavMeshBuildSource[] navMeshBuildSourceArray = NavMeshBuilder.CollectSourcesInternal(includedLayerMask, includedWorldBounds, (Transform) null, true, geometry, defaultArea, markups.ToArray());
      results.Clear();
      results.AddRange((IEnumerable<NavMeshBuildSource>) navMeshBuildSourceArray);
    }

    public static void CollectSources(Transform root, int includedLayerMask, NavMeshCollectGeometry geometry, int defaultArea, List<NavMeshBuildMarkup> markups, List<NavMeshBuildSource> results)
    {
      if (markups == null)
        throw new ArgumentNullException(nameof (markups));
      if (results == null)
        throw new ArgumentNullException(nameof (results));
      Bounds includedWorldBounds = new Bounds();
      NavMeshBuildSource[] navMeshBuildSourceArray = NavMeshBuilder.CollectSourcesInternal(includedLayerMask, includedWorldBounds, root, false, geometry, defaultArea, markups.ToArray());
      results.Clear();
      results.AddRange((IEnumerable<NavMeshBuildSource>) navMeshBuildSourceArray);
    }

    private static NavMeshBuildSource[] CollectSourcesInternal(int includedLayerMask, Bounds includedWorldBounds, Transform root, bool useBounds, NavMeshCollectGeometry geometry, int defaultArea, NavMeshBuildMarkup[] markups)
    {
      return NavMeshBuilder.INTERNAL_CALL_CollectSourcesInternal(includedLayerMask, ref includedWorldBounds, root, useBounds, geometry, defaultArea, markups);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern NavMeshBuildSource[] INTERNAL_CALL_CollectSourcesInternal(int includedLayerMask, ref Bounds includedWorldBounds, Transform root, bool useBounds, NavMeshCollectGeometry geometry, int defaultArea, NavMeshBuildMarkup[] markups);

    public static NavMeshData BuildNavMeshData(NavMeshBuildSettings buildSettings, List<NavMeshBuildSource> sources, Bounds localBounds, Vector3 position, Quaternion rotation)
    {
      if (sources == null)
        throw new ArgumentNullException(nameof (sources));
      NavMeshData data = new NavMeshData(buildSettings.agentTypeID);
      data.position = position;
      data.rotation = rotation;
      NavMeshBuilder.UpdateNavMeshDataListInternal(data, buildSettings, (object) sources, localBounds);
      return data;
    }

    public static bool UpdateNavMeshData(NavMeshData data, NavMeshBuildSettings buildSettings, List<NavMeshBuildSource> sources, Bounds localBounds)
    {
      if ((UnityEngine.Object) data == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (data));
      if (sources == null)
        throw new ArgumentNullException(nameof (sources));
      return NavMeshBuilder.UpdateNavMeshDataListInternal(data, buildSettings, (object) sources, localBounds);
    }

    private static bool UpdateNavMeshDataListInternal(NavMeshData data, NavMeshBuildSettings buildSettings, object sources, Bounds localBounds)
    {
      return NavMeshBuilder.INTERNAL_CALL_UpdateNavMeshDataListInternal(data, ref buildSettings, sources, ref localBounds);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_UpdateNavMeshDataListInternal(NavMeshData data, ref NavMeshBuildSettings buildSettings, object sources, ref Bounds localBounds);

    public static AsyncOperation UpdateNavMeshDataAsync(NavMeshData data, NavMeshBuildSettings buildSettings, List<NavMeshBuildSource> sources, Bounds localBounds)
    {
      if ((UnityEngine.Object) data == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (data));
      if (sources == null)
        throw new ArgumentNullException(nameof (sources));
      return NavMeshBuilder.UpdateNavMeshDataAsyncListInternal(data, buildSettings, (object) sources, localBounds);
    }

    /// <summary>
    ///   <para>Cancels an asynchronous update of the specified NavMesh data. See Also: UpdateNavMeshDataAsync.</para>
    /// </summary>
    /// <param name="data">The data associated with asynchronous updating.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Cancel(NavMeshData data);

    private static AsyncOperation UpdateNavMeshDataAsyncListInternal(NavMeshData data, NavMeshBuildSettings buildSettings, object sources, Bounds localBounds)
    {
      return NavMeshBuilder.INTERNAL_CALL_UpdateNavMeshDataAsyncListInternal(data, ref buildSettings, sources, ref localBounds);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern AsyncOperation INTERNAL_CALL_UpdateNavMeshDataAsyncListInternal(NavMeshData data, ref NavMeshBuildSettings buildSettings, object sources, ref Bounds localBounds);
  }
}
