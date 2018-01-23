// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.ShadowSplitData
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
  [UsedByNativeCode]
  public struct ShadowSplitData
  {
    public int cullingPlaneCount;
    private ShadowSplitData.\u003C_cullingPlanes\u003E__FixedBuffer6 _cullingPlanes;
    public Vector4 cullingSphere;

    public unsafe Plane GetCullingPlane(int index)
    {
      if (index < 0 || index >= this.cullingPlaneCount || index >= 10)
        throw new IndexOutOfRangeException("Invalid plane index");
      // ISSUE: reference to a compiler-generated field
      fixed (float* numPtr = &this._cullingPlanes.FixedElementField)
      {
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        return new Plane(new Vector3(^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4) * 4), ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 1) * 4), ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 2) * 4)), ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 3) * 4));
      }
    }

    public unsafe void SetCullingPlane(int index, Plane plane)
    {
      if (index < 0 || index >= this.cullingPlaneCount || index >= 10)
        throw new IndexOutOfRangeException("Invalid plane index");
      // ISSUE: reference to a compiler-generated field
      fixed (float* numPtr = &this._cullingPlanes.FixedElementField)
      {
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4) * 4) = plane.normal.x;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 1) * 4) = plane.normal.y;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 2) * 4) = plane.normal.z;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) ((IntPtr) numPtr + (IntPtr) (index * 4 + 3) * 4) = plane.distance;
      }
    }
  }
}
