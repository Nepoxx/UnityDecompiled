// Decompiled with JetBrains decompiler
// Type: UnityEngine.ColorUsageAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public sealed class ColorUsageAttribute : PropertyAttribute
  {
    public readonly bool showAlpha = true;
    public readonly bool hdr = false;
    public readonly float minBrightness = 0.0f;
    public readonly float maxBrightness = 8f;
    public readonly float minExposureValue = 0.125f;
    public readonly float maxExposureValue = 3f;

    public ColorUsageAttribute(bool showAlpha)
    {
      this.showAlpha = showAlpha;
    }

    public ColorUsageAttribute(bool showAlpha, bool hdr, float minBrightness, float maxBrightness, float minExposureValue, float maxExposureValue)
    {
      this.showAlpha = showAlpha;
      this.hdr = hdr;
      this.minBrightness = minBrightness;
      this.maxBrightness = maxBrightness;
      this.minExposureValue = minExposureValue;
      this.maxExposureValue = maxExposureValue;
    }
  }
}
