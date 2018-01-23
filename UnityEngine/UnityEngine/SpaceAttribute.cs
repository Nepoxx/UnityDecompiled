// Decompiled with JetBrains decompiler
// Type: UnityEngine.SpaceAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
  public class SpaceAttribute : PropertyAttribute
  {
    public readonly float height;

    public SpaceAttribute()
    {
      this.height = 8f;
    }

    public SpaceAttribute(float height)
    {
      this.height = height;
    }
  }
}
