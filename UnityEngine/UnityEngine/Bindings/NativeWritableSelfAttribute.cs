// Decompiled with JetBrains decompiler
// Type: UnityEngine.Bindings.NativeWritableSelfAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Bindings
{
  [AttributeUsage(AttributeTargets.Method)]
  internal sealed class NativeWritableSelfAttribute : Attribute, IBindingsWritableSelfProviderAttribute, IBindingsAttribute
  {
    public NativeWritableSelfAttribute()
    {
      this.WritableSelf = true;
    }

    public NativeWritableSelfAttribute(bool writable)
    {
      this.WritableSelf = writable;
    }

    public bool WritableSelf { get; set; }
  }
}
