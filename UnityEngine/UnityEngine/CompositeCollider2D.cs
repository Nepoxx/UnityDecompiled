// Decompiled with JetBrains decompiler
// Type: UnityEngine.CompositeCollider2D
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  [RequireComponent(typeof (Rigidbody2D))]
  public sealed class CompositeCollider2D : Collider2D
  {
    public extern CompositeCollider2D.GeometryType geometryType { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern CompositeCollider2D.GenerationType generationType { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern float vertexDistance { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public extern float edgeRadius { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void GenerateGeometry();

    public int GetPathPointCount(int index)
    {
      int num = this.pathCount - 1;
      if (index < 0 || index > num)
        throw new ArgumentOutOfRangeException(nameof (index), string.Format("Path index {0} must be in the range of 0 to {1}.", (object) index, (object) num));
      return this.GetPathPointCount_Internal(index);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int GetPathPointCount_Internal(int index);

    public extern int pathCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern int pointCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public int GetPath(int index, Vector2[] points)
    {
      if (index < 0 || index >= this.pathCount)
        throw new ArgumentOutOfRangeException(nameof (index), string.Format("Path index {0} must be in the range of 0 to {1}.", (object) index, (object) (this.pathCount - 1)));
      if (points == null)
        throw new ArgumentNullException(nameof (points));
      return this.Internal_GetPath(index, points);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern int Internal_GetPath(int index, Vector2[] points);

    public enum GeometryType
    {
      Outlines,
      Polygons,
    }

    public enum GenerationType
    {
      Synchronous,
      Manual,
    }
  }
}
