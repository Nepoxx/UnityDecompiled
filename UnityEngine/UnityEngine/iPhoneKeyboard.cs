// Decompiled with JetBrains decompiler
// Type: UnityEngine.iPhoneKeyboard
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Obsolete("iPhoneKeyboard class is deprecated. Please use TouchScreenKeyboard instead (UnityUpgradable) -> TouchScreenKeyboard", true)]
  public class iPhoneKeyboard
  {
    public string text
    {
      get
      {
        return string.Empty;
      }
      set
      {
      }
    }

    public static bool hideInput
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public bool active
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public bool done
    {
      get
      {
        return false;
      }
    }

    public static Rect area
    {
      get
      {
        return new Rect();
      }
    }

    public static bool visible
    {
      get
      {
        return false;
      }
    }
  }
}
