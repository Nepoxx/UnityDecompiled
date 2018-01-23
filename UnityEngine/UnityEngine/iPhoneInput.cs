// Decompiled with JetBrains decompiler
// Type: UnityEngine.iPhoneInput
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Obsolete("iPhoneInput class is deprecated. Please use Input instead (UnityUpgradable) -> Input", true)]
  public class iPhoneInput
  {
    [Obsolete("orientation property is deprecated. Please use Input.deviceOrientation instead (UnityUpgradable) -> Input.deviceOrientation", true)]
    public static iPhoneOrientation orientation
    {
      get
      {
        return iPhoneOrientation.Unknown;
      }
    }

    [Obsolete("lastLocation property is deprecated. Please use Input.location.lastData instead.", true)]
    public static LocationInfo lastLocation
    {
      get
      {
        return new LocationInfo();
      }
    }

    public static iPhoneAccelerationEvent[] accelerationEvents
    {
      get
      {
        return (iPhoneAccelerationEvent[]) null;
      }
    }

    public static iPhoneTouch[] touches
    {
      get
      {
        return (iPhoneTouch[]) null;
      }
    }

    public static int touchCount
    {
      get
      {
        return 0;
      }
    }

    public static bool multiTouchEnabled
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public static int accelerationEventCount
    {
      get
      {
        return 0;
      }
    }

    public static Vector3 acceleration
    {
      get
      {
        return new Vector3();
      }
    }

    public static iPhoneTouch GetTouch(int index)
    {
      return new iPhoneTouch();
    }

    public static iPhoneAccelerationEvent GetAccelerationEvent(int index)
    {
      return new iPhoneAccelerationEvent();
    }
  }
}
