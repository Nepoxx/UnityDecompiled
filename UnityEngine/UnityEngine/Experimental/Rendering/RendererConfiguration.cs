// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.Rendering.RendererConfiguration
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.Rendering
{
  [Flags]
  public enum RendererConfiguration
  {
    None = 0,
    PerObjectLightProbe = 1,
    PerObjectReflectionProbes = 2,
    PerObjectLightProbeProxyVolume = 4,
    PerObjectLightmaps = 8,
    ProvideLightIndices = 16, // 0x00000010
    PerObjectMotionVectors = 32, // 0x00000020
    PerObjectLightIndices8 = 64, // 0x00000040
  }
}
