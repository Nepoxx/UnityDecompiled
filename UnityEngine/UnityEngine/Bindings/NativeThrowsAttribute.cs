// Decompiled with JetBrains decompiler
// Type: UnityEngine.Bindings.NativeThrowsAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Bindings
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
  internal class NativeThrowsAttribute : Attribute, IBindingsThrowsProviderAttribute, IBindingsAttribute
  {
    public NativeThrowsAttribute()
    {
      this.ThrowsException = true;
    }

    public NativeThrowsAttribute(bool throwsException)
    {
      this.ThrowsException = throwsException;
    }

    public bool ThrowsException { get; set; }
  }
}
