// Decompiled with JetBrains decompiler
// Type: UnityEngine.LineUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>A collection of common line functions.</para>
  /// </summary>
  public sealed class LineUtility
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void GeneratePointsToKeep3D(object pointsList, float tolerance, object pointsToKeepList);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void GeneratePointsToKeep2D(object pointsList, float tolerance, object pointsToKeepList);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void GenerateSimplifiedPoints3D(object pointsList, float tolerance, object simplifiedPoints);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void GenerateSimplifiedPoints2D(object pointsList, float tolerance, object simplifiedPoints);

    public static void Simplify(List<Vector3> points, float tolerance, List<int> pointsToKeep)
    {
      if (points == null)
        throw new ArgumentNullException(nameof (points));
      if (pointsToKeep == null)
        throw new ArgumentNullException(nameof (pointsToKeep));
      LineUtility.GeneratePointsToKeep3D((object) points, tolerance, (object) pointsToKeep);
    }

    public static void Simplify(List<Vector3> points, float tolerance, List<Vector3> simplifiedPoints)
    {
      if (points == null)
        throw new ArgumentNullException(nameof (points));
      if (simplifiedPoints == null)
        throw new ArgumentNullException(nameof (simplifiedPoints));
      LineUtility.GenerateSimplifiedPoints3D((object) points, tolerance, (object) simplifiedPoints);
    }

    public static void Simplify(List<Vector2> points, float tolerance, List<int> pointsToKeep)
    {
      if (points == null)
        throw new ArgumentNullException(nameof (points));
      if (pointsToKeep == null)
        throw new ArgumentNullException(nameof (pointsToKeep));
      LineUtility.GeneratePointsToKeep2D((object) points, tolerance, (object) pointsToKeep);
    }

    public static void Simplify(List<Vector2> points, float tolerance, List<Vector2> simplifiedPoints)
    {
      if (points == null)
        throw new ArgumentNullException(nameof (points));
      if (simplifiedPoints == null)
        throw new ArgumentNullException(nameof (simplifiedPoints));
      LineUtility.GenerateSimplifiedPoints2D((object) points, tolerance, (object) simplifiedPoints);
    }
  }
}
