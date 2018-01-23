// Decompiled with JetBrains decompiler
// Type: UnityEngine.Bindings.NativeConditionalAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Bindings
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property)]
  internal class NativeConditionalAttribute : Attribute, IBindingsAttribute
  {
    public NativeConditionalAttribute()
    {
    }

    public NativeConditionalAttribute(string condition)
    {
      this.Condition = condition;
      this.Enabled = true;
    }

    public NativeConditionalAttribute(bool enabled)
    {
      this.Enabled = enabled;
    }

    public NativeConditionalAttribute(string condition, bool enabled)
      : this(condition)
    {
      this.Enabled = enabled;
    }

    public string Condition { get; set; }

    public bool Enabled { get; set; }
  }
}
