// Decompiled with JetBrains decompiler
// Type: UnityEngine.LightmapSettings
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Stores lightmaps of the scene.</para>
  /// </summary>
  public sealed class LightmapSettings : Object
  {
    /// <summary>
    ///   <para>Lightmap array.</para>
    /// </summary>
    public static extern LightmapData[] lightmaps { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Non-directional, Directional or Directional Specular lightmaps rendering mode.</para>
    /// </summary>
    public static extern LightmapsMode lightmapsMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Holds all data needed by the light probes.</para>
    /// </summary>
    public static extern LightProbes lightProbes { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void Reset();

    [Obsolete("Use lightmapsMode instead.", false)]
    public static LightmapsModeLegacy lightmapsModeLegacy
    {
      get
      {
        return LightmapsModeLegacy.Single;
      }
      set
      {
      }
    }

    [Obsolete("Use QualitySettings.desiredColorSpace instead.", false)]
    public static ColorSpace bakedColorSpace
    {
      get
      {
        return QualitySettings.desiredColorSpace;
      }
      set
      {
      }
    }
  }
}
