// Decompiled with JetBrains decompiler
// Type: UnityEngine.VR.VRDeviceType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.VR
{
  [Obsolete("VRDeviceType is deprecated. Use XRSettings.supportedDevices instead.", true)]
  public enum VRDeviceType
  {
    [Obsolete("Enum member VRDeviceType.Morpheus has been deprecated. Use VRDeviceType.PlayStationVR instead (UnityUpgradable) -> PlayStationVR", true)] Morpheus = -1,
    None = 0,
    Stereo = 1,
    Split = 2,
    Oculus = 3,
    PlayStationVR = 4,
    Unknown = 5,
  }
}
