// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.SurfaceObserver
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA
{
  /// <summary>
  ///   <para>SurfaceObserver is the main API portal for spatial mapping functionality in Unity.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA")]
  public sealed class SurfaceObserver : IDisposable
  {
    private IntPtr m_Observer;

    /// <summary>
    ///   <para>Basic constructor for SurfaceObserver.</para>
    /// </summary>
    public SurfaceObserver()
    {
      this.m_Observer = this.Internal_Create();
    }

    [RequiredByNativeCode]
    private static void InvokeSurfaceChangedEvent(SurfaceObserver.SurfaceChangedDelegate onSurfaceChanged, int surfaceId, SurfaceChange changeType, Bounds bounds, long updateTime)
    {
      if (onSurfaceChanged == null)
        return;
      SurfaceId surfaceId1;
      surfaceId1.handle = surfaceId;
      onSurfaceChanged(surfaceId1, changeType, bounds, DateTime.FromFileTime(updateTime));
    }

    [RequiredByNativeCode]
    private static void InvokeSurfaceDataReadyEvent(SurfaceObserver.SurfaceDataReadyDelegate onDataReady, int surfaceId, MeshFilter outputMesh, WorldAnchor outputAnchor, MeshCollider outputCollider, float trisPerCubicMeter, bool bakeCollider, bool outputWritten, float elapsedBakeTimeSeconds)
    {
      if (onDataReady == null)
        return;
      SurfaceData bakedData;
      bakedData.id.handle = surfaceId;
      bakedData.outputMesh = outputMesh;
      bakedData.outputAnchor = outputAnchor;
      bakedData.outputCollider = outputCollider;
      bakedData.trianglesPerCubicMeter = trisPerCubicMeter;
      bakedData.bakeCollider = bakeCollider;
      onDataReady(bakedData, outputWritten, elapsedBakeTimeSeconds);
    }

    ~SurfaceObserver()
    {
      if (!(this.m_Observer != IntPtr.Zero))
        return;
      SurfaceObserver.DestroyThreaded(this.m_Observer);
      this.m_Observer = IntPtr.Zero;
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    ///   <para>This method sets the observation volume as an axis aligned box at the requested location.  Successive calls can be used to reshape the observation volume and/or to move it in the scene as needed.  Extents are the distance from the center of the box to its edges along each axis.</para>
    /// </summary>
    /// <param name="origin">The origin of the requested observation volume.</param>
    /// <param name="extents">The extents in meters of the requested observation volume.</param>
    public void SetVolumeAsAxisAlignedBox(Vector3 origin, Vector3 extents)
    {
      this.Internal_SetVolumeAsAxisAlignedBox(this.m_Observer, origin, extents);
    }

    /// <summary>
    ///   <para>This method sets the observation volume as a sphere at the requested location.  Successive calls can be used to reshape the observation volume and/or to move it in the scene as needed.</para>
    /// </summary>
    /// <param name="origin">The origin of the requested observation volume.</param>
    /// <param name="radiusMeters">The radius in meters of the requested observation volume.</param>
    public void SetVolumeAsSphere(Vector3 origin, float radiusMeters)
    {
      this.Internal_SetVolumeAsSphere(this.m_Observer, origin, radiusMeters);
    }

    /// <summary>
    ///   <para>This method sets the observation volume as an oriented box at the requested location.  Successive calls can be used to reshape the observation volume and/or to move it in the scene as needed.  Extents are the distance from the center of the box to its edges along each axis.</para>
    /// </summary>
    /// <param name="origin">The origin of the requested observation volume.</param>
    /// <param name="extents">The extents in meters of the requested observation volume.</param>
    /// <param name="orientation">The orientation of the requested observation volume.</param>
    public void SetVolumeAsOrientedBox(Vector3 origin, Vector3 extents, Quaternion orientation)
    {
      this.Internal_SetVolumeAsOrientedBox(this.m_Observer, origin, extents, orientation);
    }

    /// <summary>
    ///   <para>This method sets the observation volume as a frustum at the requested location.  Successive calls can be used to reshape the observation volume and/or to move it in the scene as needed.</para>
    /// </summary>
    /// <param name="planes">Planes defining the frustum as returned from GeometryUtility.CalculateFrustumPlanes.</param>
    public void SetVolumeAsFrustum(Plane[] planes)
    {
      if (planes == null)
        throw new ArgumentNullException(nameof (planes));
      if (planes.Length != 6)
        throw new ArgumentException("Planes array must be 6 items long", nameof (planes));
      this.Internal_SetVolumeAsFrustum(this.m_Observer, planes);
    }

    public void Update(SurfaceObserver.SurfaceChangedDelegate onSurfaceChanged)
    {
      if (onSurfaceChanged == null)
        throw new ArgumentNullException(nameof (onSurfaceChanged));
      SurfaceObserver.Internal_Update(this.m_Observer, onSurfaceChanged);
    }

    public bool RequestMeshAsync(SurfaceData dataRequest, SurfaceObserver.SurfaceDataReadyDelegate onDataReady)
    {
      if (onDataReady == null)
        throw new ArgumentNullException(nameof (onDataReady));
      if ((UnityEngine.Object) dataRequest.outputMesh == (UnityEngine.Object) null)
        throw new ArgumentNullException("dataRequest.outputMesh");
      if ((UnityEngine.Object) dataRequest.outputAnchor == (UnityEngine.Object) null)
        throw new ArgumentNullException("dataRequest.outputAnchor");
      if ((UnityEngine.Object) dataRequest.outputCollider == (UnityEngine.Object) null && dataRequest.bakeCollider)
        throw new ArgumentException("dataRequest.outputCollider must be non-NULL if dataRequest.bakeCollider is true", "dataRequest.outputCollider");
      if ((double) dataRequest.trianglesPerCubicMeter < 0.0)
        throw new ArgumentException("dataRequest.trianglesPerCubicMeter must be greater than zero", "dataRequest.trianglesPerCubicMeter");
      bool workQueue = SurfaceObserver.Internal_AddToWorkQueue(this.m_Observer, onDataReady, dataRequest.id.handle, dataRequest.outputMesh, dataRequest.outputAnchor, dataRequest.outputCollider, dataRequest.trianglesPerCubicMeter, dataRequest.bakeCollider);
      if (!workQueue)
        Debug.LogError((object) "RequestMeshAsync has failed.  Is your surface ID valid?");
      return workQueue;
    }

    /// <summary>
    ///   <para>Call Dispose when the SurfaceObserver is no longer needed.  This will ensure that the object is cleaned up appropriately but will not affect any Meshes, components, or objects returned by RequestMeshAsync.</para>
    /// </summary>
    public void Dispose()
    {
      if (this.m_Observer != IntPtr.Zero)
      {
        SurfaceObserver.Destroy(this.m_Observer);
        this.m_Observer = IntPtr.Zero;
      }
      GC.SuppressFinalize((object) this);
    }

    private IntPtr Internal_Create()
    {
      IntPtr num;
      SurfaceObserver.INTERNAL_CALL_Internal_Create(this, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_Create(SurfaceObserver self, out IntPtr value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Destroy(IntPtr observer);

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void DestroyThreaded(IntPtr observer);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Update(IntPtr observer, SurfaceObserver.SurfaceChangedDelegate onSurfaceChanged);

    private void Internal_SetVolumeAsAxisAlignedBox(IntPtr observer, Vector3 origin, Vector3 extents)
    {
      SurfaceObserver.INTERNAL_CALL_Internal_SetVolumeAsAxisAlignedBox(this, observer, ref origin, ref extents);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_SetVolumeAsAxisAlignedBox(SurfaceObserver self, IntPtr observer, ref Vector3 origin, ref Vector3 extents);

    private void Internal_SetVolumeAsOrientedBox(IntPtr observer, Vector3 origin, Vector3 extents, Quaternion orientation)
    {
      SurfaceObserver.INTERNAL_CALL_Internal_SetVolumeAsOrientedBox(this, observer, ref origin, ref extents, ref orientation);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_SetVolumeAsOrientedBox(SurfaceObserver self, IntPtr observer, ref Vector3 origin, ref Vector3 extents, ref Quaternion orientation);

    private void Internal_SetVolumeAsSphere(IntPtr observer, Vector3 origin, float radiusMeters)
    {
      SurfaceObserver.INTERNAL_CALL_Internal_SetVolumeAsSphere(this, observer, ref origin, radiusMeters);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Internal_SetVolumeAsSphere(SurfaceObserver self, IntPtr observer, ref Vector3 origin, float radiusMeters);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Internal_SetVolumeAsFrustum(IntPtr observer, Plane[] planes);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool Internal_AddToWorkQueue(IntPtr observer, SurfaceObserver.SurfaceDataReadyDelegate onDataReady, int surfaceId, MeshFilter filter, WorldAnchor wa, MeshCollider mc, float trisPerCubicMeter, bool createColliderData);

    /// <summary>
    ///   <para>The SurfaceChanged delegate handles SurfaceChanged events as generated by calling Update on a SurfaceObserver.  Applications can use the bounds, changeType, and updateTime to selectively generate mesh data for the set of known surfaces.</para>
    /// </summary>
    /// <param name="surfaceId">The ID of the surface that has changed.</param>
    /// <param name="changeType">The type of change this event represents (Added, Updated, Removed).</param>
    /// <param name="bounds">The bounds of the surface as reported by the device.</param>
    /// <param name="updateTime">The update time of the surface as reported by the device.</param>
    public delegate void SurfaceChangedDelegate(SurfaceId surfaceId, SurfaceChange changeType, Bounds bounds, DateTime updateTime);

    /// <summary>
    ///   <para>The SurfaceDataReadyDelegate handles events generated when the engine has completed generating a mesh.  Mesh generation is requested through GetMeshAsync and may take many frames to complete.</para>
    /// </summary>
    /// <param name="bakedData">Struct containing output data.</param>
    /// <param name="outputWritten">Set to true if output has been written and false otherwise.</param>
    /// <param name="elapsedBakeTimeSeconds">Elapsed seconds between mesh bake request and propagation of this event.</param>
    public delegate void SurfaceDataReadyDelegate(SurfaceData bakedData, bool outputWritten, float elapsedBakeTimeSeconds);
  }
}
