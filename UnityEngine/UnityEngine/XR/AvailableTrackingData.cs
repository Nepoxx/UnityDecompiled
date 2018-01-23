// Decompiled with JetBrains decompiler
// Type: UnityEngine.XR.AvailableTrackingData
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.XR
{
  [Flags]
  internal enum AvailableTrackingData
  {
    None = 0,
    PositionAvailable = 1,
    RotationAvailable = 2,
    VelocityAvailable = 4,
    AngularVelocityAvailable = 8,
    AccelerationAvailable = 16, // 0x00000010
    AngularAccelerationAvailable = 32, // 0x00000020
  }
}
