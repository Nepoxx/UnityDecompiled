// Decompiled with JetBrains decompiler
// Type: UnityEngine.Bindings.NativeNameAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Bindings
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
  internal class NativeNameAttribute : Attribute, IBindingsNameProviderAttribute, IBindingsAttribute
  {
    public NativeNameAttribute()
    {
    }

    public NativeNameAttribute(string name)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (name == "")
        throw new ArgumentException("name cannot be empty", nameof (name));
      this.Name = name;
    }

    public string Name { get; set; }
  }
}
