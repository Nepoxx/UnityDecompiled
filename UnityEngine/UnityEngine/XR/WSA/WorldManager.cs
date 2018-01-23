// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.WorldManager
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
  ///   <para>This class represents the state of the real world tracking system.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA")]
  public sealed class WorldManager
  {
    [Obsolete("The option for toggling latent frame presentation has been removed, and is on for performance reasons. This property will be removed in a future release.", false)]
    public static bool IsLatentFramePresentation
    {
      get
      {
        return true;
      }
    }

    [Obsolete("The option for toggling latent frame presentation has been removed, and is on for performance reasons. This method will be removed in a future release.", false)]
    public static void ActivateLatentFramePresentation(bool activated)
    {
    }

    public static event WorldManager.OnPositionalLocatorStateChangedDelegate OnPositionalLocatorStateChanged;

    [RequiredByNativeCode]
    private static void Internal_TriggerPositionalLocatorStateChanged(PositionalLocatorState oldState, PositionalLocatorState newState)
    {
      // ISSUE: reference to a compiler-generated field
      if (WorldManager.OnPositionalLocatorStateChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      WorldManager.OnPositionalLocatorStateChanged(oldState, newState);
    }

    /// <summary>
    ///   <para>The current state of the world tracking systems.</para>
    /// </summary>
    public static extern PositionalLocatorState state { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Return the native pointer to Windows::Perception::Spatial::ISpatialCoordinateSystem which was retrieved from an Windows::Perception::Spatial::ISpatialStationaryFrameOfReference object underlying the Unity World Origin.</para>
    /// </summary>
    /// <returns>
    ///   <para>Pointer to Windows::Perception::Spatial::ISpatialCoordinateSystem.</para>
    /// </returns>
    public static IntPtr GetNativeISpatialCoordinateSystemPtr()
    {
      IntPtr num;
      WorldManager.INTERNAL_CALL_GetNativeISpatialCoordinateSystemPtr(out num);
      return num;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetNativeISpatialCoordinateSystemPtr(out IntPtr value);

    /// <summary>
    ///   <para>Callback on when the world tracking systems state has changed.</para>
    /// </summary>
    /// <param name="oldState">The previous state of the world tracking systems.</param>
    /// <param name="newState">The new state of the world tracking systems.</param>
    public delegate void OnPositionalLocatorStateChangedDelegate(PositionalLocatorState oldState, PositionalLocatorState newState);
  }
}
