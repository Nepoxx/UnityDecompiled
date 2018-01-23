// Decompiled with JetBrains decompiler
// Type: UnityEngine.RangeAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public sealed class RangeAttribute : PropertyAttribute
  {
    public readonly float min;
    public readonly float max;

    public RangeAttribute(float min, float max)
    {
      this.min = min;
      this.max = max;
    }
  }
}
