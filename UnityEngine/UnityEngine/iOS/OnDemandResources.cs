// Decompiled with JetBrains decompiler
// Type: UnityEngine.iOS.OnDemandResources
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine.iOS
{
  public static class OnDemandResources
  {
    public static extern bool enabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern OnDemandResourcesRequest PreloadAsyncImpl(string[] tags);

    public static OnDemandResourcesRequest PreloadAsync(string[] tags)
    {
      return OnDemandResources.PreloadAsyncImpl(tags);
    }
  }
}
