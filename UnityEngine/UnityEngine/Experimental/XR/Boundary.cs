// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.XR.Boundary
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Experimental.XR
{
  [MovedFrom("UnityEngine.Experimental.VR")]
  public static class Boundary
  {
    [ExcludeFromDocs]
    public static bool TryGetDimensions(out Vector3 dimensionsOut)
    {
      Boundary.Type boundaryType = Boundary.Type.PlayArea;
      return Boundary.TryGetDimensions(out dimensionsOut, boundaryType);
    }

    public static bool TryGetDimensions(out Vector3 dimensionsOut, [DefaultValue("Type.PlayArea")] Boundary.Type boundaryType)
    {
      return Boundary.TryGetDimensionsInternal(out dimensionsOut, (int) boundaryType);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool TryGetDimensionsInternal(out Vector3 dimensionsOut, int boundaryType);

    [ExcludeFromDocs]
    public static bool TryGetGeometry(List<Vector3> geometry)
    {
      Boundary.Type boundaryType = Boundary.Type.PlayArea;
      return Boundary.TryGetGeometry(geometry, boundaryType);
    }

    public static bool TryGetGeometry(List<Vector3> geometry, [DefaultValue("Type.PlayArea")] Boundary.Type boundaryType)
    {
      if (geometry == null)
        throw new ArgumentNullException(nameof (geometry));
      geometry.Clear();
      return Boundary.TryGetGeometryInternal((object) geometry, (int) boundaryType);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool TryGetGeometryInternal(object geometryOut, int boundaryType);

    public static extern bool visible { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    public static extern bool configured { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    public enum Type
    {
      PlayArea,
      TrackedArea,
    }
  }
}
