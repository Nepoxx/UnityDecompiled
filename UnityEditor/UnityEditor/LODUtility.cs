// Decompiled with JetBrains decompiler
// Type: UnityEditor.LODUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>LOD Utility Helpers.</para>
  /// </summary>
  public sealed class LODUtility
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern LODVisualizationInformation CalculateVisualizationData(Camera camera, LODGroup group, int lodLevel);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern float CalculateDistance(Camera camera, float relativeScreenHeight, LODGroup group);

    internal static Vector3 CalculateWorldReferencePoint(LODGroup group)
    {
      Vector3 vector3;
      LODUtility.INTERNAL_CALL_CalculateWorldReferencePoint(group, out vector3);
      return vector3;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_CalculateWorldReferencePoint(LODGroup group, out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool NeedUpdateLODGroupBoundingBox(LODGroup group);

    /// <summary>
    ///   <para>Recalculate the bounding region for the given LODGroup.</para>
    /// </summary>
    /// <param name="group"></param>
    public static void CalculateLODGroupBoundingBox(LODGroup group)
    {
      if ((UnityEngine.Object) group == (UnityEngine.Object) null)
        throw new ArgumentNullException(nameof (group));
      group.RecalculateBounds();
    }
  }
}
