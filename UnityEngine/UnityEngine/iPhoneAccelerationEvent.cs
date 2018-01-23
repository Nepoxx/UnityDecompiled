// Decompiled with JetBrains decompiler
// Type: UnityEngine.iPhoneAccelerationEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
  [Obsolete("iPhoneAccelerationEvent struct is deprecated. Please use AccelerationEvent instead (UnityUpgradable) -> AccelerationEvent", true)]
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct iPhoneAccelerationEvent
  {
    [Obsolete("timeDelta property is deprecated. Please use AccelerationEvent.deltaTime instead (UnityUpgradable) -> AccelerationEvent.deltaTime", true)]
    public float timeDelta
    {
      get
      {
        return 0.0f;
      }
    }

    public Vector3 acceleration
    {
      get
      {
        return new Vector3();
      }
    }

    public float deltaTime
    {
      get
      {
        return -1f;
      }
    }
  }
}
