// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.Device
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine.iOS
{
  public sealed class Device
  {
    public static extern string systemVersion { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern DeviceGeneration generation { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    public static extern string vendorIdentifier { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string GetAdvertisingIdentifier();

    public static string advertisingIdentifier
    {
      get
      {
        string advertisingIdentifier = Device.GetAdvertisingIdentifier();
        Application.InvokeOnAdvertisingIdentifierCallback(advertisingIdentifier, Device.advertisingTrackingEnabled);
        return advertisingIdentifier;
      }
    }

    public static extern bool advertisingTrackingEnabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetNoBackupFlag(string path);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void ResetNoBackupFlag(string path);

    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool RequestStoreReview();
  }
}
