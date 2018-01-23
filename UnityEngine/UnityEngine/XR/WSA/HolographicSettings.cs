// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.HolographicSettings
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
  ///   <para>The Holographic Settings contain functions which effect the performance and presentation of Holograms on Windows Holographic platforms.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA")]
  public sealed class HolographicSettings
  {
    /// <summary>
    ///   <para>Option to allow developers to achieve higher framerate at the cost of high latency.  By default this option is off.</para>
    /// </summary>
    /// <param name="activated">True to enable or false to disable Low Latent Frame Presentation.</param>
    [Obsolete("Support for toggling latent frame presentation has been removed", true)]
    public static void ActivateLatentFramePresentation(bool activated)
    {
    }

    /// <summary>
    ///   <para>Returns true if Holographic rendering is currently running with Latent Frame Presentation.  Default value is false.</para>
    /// </summary>
    [Obsolete("Support for toggling latent frame presentation has been removed, and IsLatentFramePresentation will always return true", false)]
    public static bool IsLatentFramePresentation
    {
      get
      {
        return true;
      }
    }

    /// <summary>
    ///   <para>Sets a point in 3d space that is the focal point of the scene for the user for this frame. This helps improve the visual fidelity of content around this point. This must be set every frame.</para>
    /// </summary>
    /// <param name="position">The position of the focal point in the scene, relative to the camera.</param>
    /// <param name="normal">Surface normal of the plane being viewed at the focal point.</param>
    /// <param name="velocity">A vector that describes how the focus point is moving in the scene at this point in time. This allows the HoloLens to compensate for both your head movement and the movement of the object in the scene.</param>
    public static void SetFocusPointForFrame(Vector3 position)
    {
      HolographicSettings.InternalSetFocusPointForFrame(position);
    }

    private static void InternalSetFocusPointForFrame(Vector3 position)
    {
      HolographicSettings.INTERNAL_CALL_InternalSetFocusPointForFrame(ref position);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_InternalSetFocusPointForFrame(ref Vector3 position);

    /// <summary>
    ///   <para>Sets a point in 3d space that is the focal point of the scene for the user for this frame. This helps improve the visual fidelity of content around this point. This must be set every frame.</para>
    /// </summary>
    /// <param name="position">The position of the focal point in the scene, relative to the camera.</param>
    /// <param name="normal">Surface normal of the plane being viewed at the focal point.</param>
    /// <param name="velocity">A vector that describes how the focus point is moving in the scene at this point in time. This allows the HoloLens to compensate for both your head movement and the movement of the object in the scene.</param>
    public static void SetFocusPointForFrame(Vector3 position, Vector3 normal)
    {
      HolographicSettings.InternalSetFocusPointForFrameWithNormal(position, normal);
    }

    private static void InternalSetFocusPointForFrameWithNormal(Vector3 position, Vector3 normal)
    {
      HolographicSettings.INTERNAL_CALL_InternalSetFocusPointForFrameWithNormal(ref position, ref normal);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_InternalSetFocusPointForFrameWithNormal(ref Vector3 position, ref Vector3 normal);

    /// <summary>
    ///   <para>Sets a point in 3d space that is the focal point of the scene for the user for this frame. This helps improve the visual fidelity of content around this point. This must be set every frame.</para>
    /// </summary>
    /// <param name="position">The position of the focal point in the scene, relative to the camera.</param>
    /// <param name="normal">Surface normal of the plane being viewed at the focal point.</param>
    /// <param name="velocity">A vector that describes how the focus point is moving in the scene at this point in time. This allows the HoloLens to compensate for both your head movement and the movement of the object in the scene.</param>
    public static void SetFocusPointForFrame(Vector3 position, Vector3 normal, Vector3 velocity)
    {
      HolographicSettings.InternalSetFocusPointForFrameWithNormalVelocity(position, normal, velocity);
    }

    private static void InternalSetFocusPointForFrameWithNormalVelocity(Vector3 position, Vector3 normal, Vector3 velocity)
    {
      HolographicSettings.INTERNAL_CALL_InternalSetFocusPointForFrameWithNormalVelocity(ref position, ref normal, ref velocity);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_InternalSetFocusPointForFrameWithNormalVelocity(ref Vector3 position, ref Vector3 normal, ref Vector3 velocity);

    /// <summary>
    ///   <para>This method returns whether or not the display associated with the main camera reports as opaque.</para>
    /// </summary>
    public static extern bool IsDisplayOpaque { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
