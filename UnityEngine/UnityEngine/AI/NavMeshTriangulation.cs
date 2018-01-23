// Decompiled with JetBrains decompiler
// Type: UnityEngine.AI.NavMeshTriangulation
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
  /// <summary>
  ///   <para>Contains data describing a triangulation of a navmesh.</para>
  /// </summary>
  [UsedByNativeCode]
  [MovedFrom("UnityEngine")]
  public struct NavMeshTriangulation
  {
    /// <summary>
    ///   <para>Vertices for the navmesh triangulation.</para>
    /// </summary>
    public Vector3[] vertices;
    /// <summary>
    ///   <para>Triangle indices for the navmesh triangulation.</para>
    /// </summary>
    public int[] indices;
    /// <summary>
    ///   <para>NavMesh area indices for the navmesh triangulation.</para>
    /// </summary>
    public int[] areas;

    /// <summary>
    ///   <para>NavMeshLayer values for the navmesh triangulation.</para>
    /// </summary>
    [Obsolete("Use areas instead.")]
    public int[] layers
    {
      get
      {
        return this.areas;
      }
    }
  }
}
