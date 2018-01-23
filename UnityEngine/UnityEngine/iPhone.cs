// Decompiled with JetBrains decompiler
// Type: UnityEngine.iPhone
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [Obsolete("iPhone class is deprecated. Please use iOS.Device instead (UnityUpgradable) -> UnityEngine.iOS.Device", true)]
  public sealed class iPhone
  {
    public static iPhoneGeneration generation
    {
      get
      {
        return iPhoneGeneration.Unknown;
      }
    }

    public static string vendorIdentifier
    {
      get
      {
        return (string) null;
      }
    }

    public static string advertisingIdentifier
    {
      get
      {
        return (string) null;
      }
    }

    public static bool advertisingTrackingEnabled
    {
      get
      {
        return false;
      }
    }

    public static void SetNoBackupFlag(string path)
    {
    }

    public static void ResetNoBackupFlag(string path)
    {
    }
  }
}
