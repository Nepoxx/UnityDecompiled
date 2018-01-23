// Decompiled with JetBrains decompiler
// Type: UnityEngine.VR.VRNode
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.VR
{
  [Obsolete("VRNode has been moved and renamed.  Use UnityEngine.XR.XRNode instead (UnityUpgradable) -> UnityEngine.XR.XRNode", true)]
  public enum VRNode
  {
    LeftEye,
    RightEye,
    CenterEye,
    Head,
    LeftHand,
    RightHand,
    GameController,
    TrackingReference,
    HardwareTracker,
  }
}
