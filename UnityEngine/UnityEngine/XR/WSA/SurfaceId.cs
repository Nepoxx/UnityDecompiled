// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.SurfaceId
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA
{
  /// <summary>
  ///   <para>SurfaceId is a structure wrapping the unique ID used to denote Surfaces.  SurfaceIds are provided through the onSurfaceChanged callback in Update and returned after a RequestMeshAsync call has completed.  SurfaceIds are guaranteed to be unique though Surfaces are sometimes replaced with a new Surface in the same location with a different ID.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA")]
  public struct SurfaceId
  {
    /// <summary>
    ///   <para>The actual integer ID referring to a single surface.</para>
    /// </summary>
    public int handle;
  }
}
