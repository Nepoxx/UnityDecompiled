// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.WorldAnchor
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
  ///   <para>The WorldAnchor component allows a GameObject's position to be locked in physical space.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA")]
  [RequireComponent(typeof (Transform))]
  public sealed class WorldAnchor : Component
  {
    private WorldAnchor()
    {
    }

    public event WorldAnchor.OnTrackingChangedDelegate OnTrackingChanged;

    /// <summary>
    ///   <para>Returns true if this WorldAnchor is located (read only).  A return of false typically indicates a loss of tracking.</para>
    /// </summary>
    public bool isLocated
    {
      get
      {
        return this.IsLocated_Internal();
      }
    }

    /// <summary>
    ///   <para>Assigns the &lt;a href="https:msdn.microsoft.comen-uslibrarywindowsappswindows.perception.spatial.spatialanchor.aspx"&gt;Windows.Perception.Spatial.SpatialAnchor&lt;a&gt; COM pointer maintained by this WorldAnchor.</para>
    /// </summary>
    /// <param name="spatialAnchorPtr">A live &lt;a href="https:msdn.microsoft.comen-uslibrarywindowsappswindows.perception.spatial.spatialanchor.aspx"&gt;Windows.Perception.Spatial.SpatialAnchor&lt;a&gt; COM pointer.</param>
    public void SetNativeSpatialAnchorPtr(IntPtr spatialAnchorPtr)
    {
      this.SetSpatialAnchor_Internal_FromScript(spatialAnchorPtr);
    }

    private void SetSpatialAnchor_Internal_FromScript(IntPtr spatialAnchorPtr)
    {
      WorldAnchor.INTERNAL_CALL_SetSpatialAnchor_Internal_FromScript(this, spatialAnchorPtr);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetSpatialAnchor_Internal_FromScript(WorldAnchor self, IntPtr spatialAnchorPtr);

    /// <summary>
    ///         <para>Retrieve a native pointer to the &lt;a href="https:msdn.microsoft.comen-uslibrarywindowsappswindows.perception.spatial.spatialanchor.aspx"&gt;Windows.Perception.Spatial.SpatialAnchor&lt;a&gt; COM object.
    /// This function calls &lt;a href=" https:msdn.microsoft.comen-uslibrarywindowsdesktopms691379.aspx"&gt;IUnknown::AddRef&lt;a&gt; on the pointer before returning it. The pointer must be released by calling &lt;a href=" https:msdn.microsoft.comen-uslibrarywindowsdesktopms682317.aspx"&gt;IUnknown::Release&lt;a&gt;.</para>
    ///       </summary>
    /// <returns>
    ///   <para>The native pointer to the &lt;a href=" https:msdn.microsoft.comen-uslibrarywindowsappswindows.perception.spatial.spatialanchor.aspx"&gt;Windows.Perception.Spatial.SpatialAnchor&lt;a&gt; COM object.</para>
    /// </returns>
    public IntPtr GetNativeSpatialAnchorPtr()
    {
      return this.GetSpatialAnchor_Internal();
    }

    private IntPtr GetSpatialAnchor_Internal()
    {
      IntPtr num;
      WorldAnchor.INTERNAL_CALL_GetSpatialAnchor_Internal(this, out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetSpatialAnchor_Internal(WorldAnchor self, out IntPtr value);

    private bool IsLocated_Internal()
    {
      return WorldAnchor.INTERNAL_CALL_IsLocated_Internal(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_IsLocated_Internal(WorldAnchor self);

    [RequiredByNativeCode]
    private static void Internal_TriggerEventOnTrackingLost(WorldAnchor self, bool located)
    {
      // ISSUE: reference to a compiler-generated field
      if (!((UnityEngine.Object) self != (UnityEngine.Object) null) || self.OnTrackingChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      self.OnTrackingChanged(self, located);
    }

    /// <summary>
    ///   <para>Event that is fired when this object's tracking state changes.</para>
    /// </summary>
    /// <param name="located">Set to true if the object is locatable.</param>
    /// <param name="self">The WorldAnchor reporting the tracking state change.</param>
    public delegate void OnTrackingChangedDelegate(WorldAnchor self, bool located);
  }
}
