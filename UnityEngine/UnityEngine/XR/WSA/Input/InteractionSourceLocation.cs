// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.WSA.Input.InteractionSourceLocation
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
  /// <summary>
  ///   <para>Represents the position and velocity of a hand or controller - this has been deprecated. Use InteractionSourcePose instead.</para>
  /// </summary>
  [MovedFrom("UnityEngine.VR.WSA.Input")]
  [Obsolete("InteractionSourceLocation is deprecated, and will be removed in a future release. Use InteractionSourcePose instead. (UnityUpgradable) -> InteractionSourcePose", true)]
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct InteractionSourceLocation
  {
    public bool TryGetVelocity(out Vector3 velocity)
    {
      velocity = Vector3.zero;
      return false;
    }

    public bool TryGetPosition(out Vector3 position)
    {
      position = Vector3.zero;
      return false;
    }
  }
}
