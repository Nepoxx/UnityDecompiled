// Decompiled with JetBrains decompiler
// Type: UnityEngine.Rendering.CameraEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Rendering
{
  public enum CameraEvent
  {
    BeforeDepthTexture,
    AfterDepthTexture,
    BeforeDepthNormalsTexture,
    AfterDepthNormalsTexture,
    BeforeGBuffer,
    AfterGBuffer,
    BeforeLighting,
    AfterLighting,
    BeforeFinalPass,
    AfterFinalPass,
    BeforeForwardOpaque,
    AfterForwardOpaque,
    BeforeImageEffectsOpaque,
    AfterImageEffectsOpaque,
    BeforeSkybox,
    AfterSkybox,
    BeforeForwardAlpha,
    AfterForwardAlpha,
    BeforeImageEffects,
    AfterImageEffects,
    AfterEverything,
    BeforeReflections,
    AfterReflections,
    BeforeHaloAndLensFlares,
    AfterHaloAndLensFlares,
  }
}
