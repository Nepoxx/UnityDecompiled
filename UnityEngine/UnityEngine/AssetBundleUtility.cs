// Decompiled with JetBrains decompiler
// Type: UnityEngine.AssetBundleUtility
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;

namespace UnityEngine
{
  internal static class AssetBundleUtility
  {
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void PatchAssetBundles(AssetBundle[] bundles, string[] filenames);
  }
}
