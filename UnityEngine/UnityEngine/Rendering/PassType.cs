// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.PassType
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Rendering
{
  public enum PassType
  {
    Normal = 0,
    Vertex = 1,
    VertexLM = 2,
    [Obsolete("VertexLMRGBM PassType is obsolete. Please use VertexLM PassType together with DecodeLightmap shader function.")] VertexLMRGBM = 3,
    ForwardBase = 4,
    ForwardAdd = 5,
    LightPrePassBase = 6,
    LightPrePassFinal = 7,
    ShadowCaster = 8,
    Deferred = 10, // 0x0000000A
    Meta = 11, // 0x0000000B
    MotionVectors = 12, // 0x0000000C
  }
}
