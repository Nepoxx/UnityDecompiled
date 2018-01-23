// Decompiled with JetBrains decompiler
// Type: UnityEngine.Bindings.StaticAccessorAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Bindings
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property)]
  internal class StaticAccessorAttribute : Attribute, IBindingsAttribute
  {
    public StaticAccessorAttribute()
    {
    }

    internal StaticAccessorAttribute(string name)
    {
      this.Name = name;
    }

    public StaticAccessorAttribute(StaticAccessorType type)
    {
      this.Type = type;
    }

    public StaticAccessorAttribute(string name, StaticAccessorType type)
    {
      this.Name = name;
      this.Type = type;
    }

    public string Name { get; set; }

    public StaticAccessorType Type { get; set; }
  }
}
