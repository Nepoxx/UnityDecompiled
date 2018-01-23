// Decompiled with JetBrains decompiler
// Type: UnityEngine.GridLayout
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
  [RequireComponent(typeof (Transform))]
  [NativeType(Header = "Modules/Grid/Public/Grid.h")]
  public class GridLayout : Behaviour
  {
    public Vector3 cellSize
    {
      get
      {
        Vector3 ret;
        this.get_cellSize_Injected(out ret);
        return ret;
      }
    }

    public Vector3 cellGap
    {
      get
      {
        Vector3 ret;
        this.get_cellGap_Injected(out ret);
        return ret;
      }
    }

    public extern GridLayout.CellLayout cellLayout { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public extern GridLayout.CellSwizzle cellSwizzle { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public Bounds GetBoundsLocal(Vector3Int cellPosition)
    {
      Bounds ret;
      this.GetBoundsLocal_Injected(ref cellPosition, out ret);
      return ret;
    }

    public Vector3 CellToLocal(Vector3Int cellPosition)
    {
      Vector3 ret;
      this.CellToLocal_Injected(ref cellPosition, out ret);
      return ret;
    }

    public Vector3Int LocalToCell(Vector3 localPosition)
    {
      Vector3Int ret;
      this.LocalToCell_Injected(ref localPosition, out ret);
      return ret;
    }

    public Vector3 CellToLocalInterpolated(Vector3 cellPosition)
    {
      Vector3 ret;
      this.CellToLocalInterpolated_Injected(ref cellPosition, out ret);
      return ret;
    }

    public Vector3 LocalToCellInterpolated(Vector3 localPosition)
    {
      Vector3 ret;
      this.LocalToCellInterpolated_Injected(ref localPosition, out ret);
      return ret;
    }

    public Vector3 CellToWorld(Vector3Int cellPosition)
    {
      Vector3 ret;
      this.CellToWorld_Injected(ref cellPosition, out ret);
      return ret;
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
      Vector3Int ret;
      this.WorldToCell_Injected(ref worldPosition, out ret);
      return ret;
    }

    public Vector3 LocalToWorld(Vector3 localPosition)
    {
      Vector3 ret;
      this.LocalToWorld_Injected(ref localPosition, out ret);
      return ret;
    }

    public Vector3 WorldToLocal(Vector3 worldPosition)
    {
      Vector3 ret;
      this.WorldToLocal_Injected(ref worldPosition, out ret);
      return ret;
    }

    public Vector3 GetLayoutCellCenter()
    {
      Vector3 ret;
      this.GetLayoutCellCenter_Injected(out ret);
      return ret;
    }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_cellSize_Injected(out Vector3 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_cellGap_Injected(out Vector3 ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetBoundsLocal_Injected(ref Vector3Int cellPosition, out Bounds ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void CellToLocal_Injected(ref Vector3Int cellPosition, out Vector3 ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void LocalToCell_Injected(ref Vector3 localPosition, out Vector3Int ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void CellToLocalInterpolated_Injected(ref Vector3 cellPosition, out Vector3 ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void LocalToCellInterpolated_Injected(ref Vector3 localPosition, out Vector3 ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void CellToWorld_Injected(ref Vector3Int cellPosition, out Vector3 ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void WorldToCell_Injected(ref Vector3 worldPosition, out Vector3Int ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void LocalToWorld_Injected(ref Vector3 localPosition, out Vector3 ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void WorldToLocal_Injected(ref Vector3 worldPosition, out Vector3 ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetLayoutCellCenter_Injected(out Vector3 ret);

    public enum CellLayout
    {
      Rectangle,
    }

    public enum CellSwizzle
    {
      XYZ,
      XZY,
      YXZ,
      YZX,
      ZXY,
      ZYX,
    }
  }
}
