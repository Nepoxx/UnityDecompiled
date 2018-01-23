// Decompiled with JetBrains decompiler
// Type: UnityEngine.Grid
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
  [RequireComponent(typeof (Transform))]
  [NativeType(Header = "Modules/Grid/Public/Grid.h")]
  public sealed class Grid : GridLayout
  {
    public Vector3 GetCellCenterLocal(Vector3Int position)
    {
      return this.CellToLocalInterpolated((Vector3) position + this.GetLayoutCellCenter());
    }

    public Vector3 GetCellCenterWorld(Vector3Int position)
    {
      return this.LocalToWorld(this.CellToLocalInterpolated((Vector3) position + this.GetLayoutCellCenter()));
    }

    public new Vector3 cellSize
    {
      get
      {
        Vector3 ret;
        this.get_cellSize_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_cellSize_Injected(ref value);
      }
    }

    public new Vector3 cellGap
    {
      get
      {
        Vector3 ret;
        this.get_cellGap_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_cellGap_Injected(ref value);
      }
    }

    public new extern GridLayout.CellLayout cellLayout { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public new extern GridLayout.CellSwizzle cellSwizzle { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static Vector3 Swizzle(GridLayout.CellSwizzle swizzle, Vector3 position)
    {
      Vector3 ret;
      Grid.Swizzle_Injected(swizzle, ref position, out ret);
      return ret;
    }

    public static Vector3 InverseSwizzle(GridLayout.CellSwizzle swizzle, Vector3 position)
    {
      Vector3 ret;
      Grid.InverseSwizzle_Injected(swizzle, ref position, out ret);
      return ret;
    }

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_cellSize_Injected(out Vector3 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_cellSize_Injected(ref Vector3 value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_cellGap_Injected(out Vector3 ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_cellGap_Injected(ref Vector3 value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Swizzle_Injected(GridLayout.CellSwizzle swizzle, ref Vector3 position, out Vector3 ret);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void InverseSwizzle_Injected(GridLayout.CellSwizzle swizzle, ref Vector3 position, out Vector3 ret);
  }
}
