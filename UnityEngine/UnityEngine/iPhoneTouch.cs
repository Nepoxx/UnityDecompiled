// Decompiled with JetBrains decompiler
// Type: UnityEngine.iPhoneTouch
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
  [Obsolete("iPhoneTouch struct is deprecated. Please use Touch instead (UnityUpgradable) -> Touch", true)]
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct iPhoneTouch
  {
    [Obsolete("positionDelta property is deprecated. Please use Touch.deltaPosition instead (UnityUpgradable) -> Touch.deltaPosition", true)]
    public Vector2 positionDelta
    {
      get
      {
        return new Vector2();
      }
    }

    [Obsolete("timeDelta property is deprecated. Please use Touch.deltaTime instead (UnityUpgradable) -> Touch.deltaTime", true)]
    public float timeDelta
    {
      get
      {
        return 0.0f;
      }
    }

    public int fingerId
    {
      get
      {
        return 0;
      }
    }

    public Vector2 position
    {
      get
      {
        return new Vector2();
      }
    }

    public Vector2 deltaPosition
    {
      get
      {
        return new Vector2();
      }
    }

    public float deltaTime
    {
      get
      {
        return 0.0f;
      }
    }

    public int tapCount
    {
      get
      {
        return 0;
      }
    }

    public iPhoneTouchPhase phase
    {
      get
      {
        return iPhoneTouchPhase.Began;
      }
    }
  }
}
