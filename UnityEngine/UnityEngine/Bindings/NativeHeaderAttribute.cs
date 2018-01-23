// Decompiled with JetBrains decompiler
// Type: UnityEngine.Bindings.NativeHeaderAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Bindings
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
  internal class NativeHeaderAttribute : Attribute, IBindingsHeaderProviderAttribute, IBindingsAttribute
  {
    public NativeHeaderAttribute()
    {
    }

    public NativeHeaderAttribute(string header)
    {
      if (header == null)
        throw new ArgumentNullException(nameof (header));
      if (header == "")
        throw new ArgumentException("header cannot be empty", nameof (header));
      this.Header = header;
    }

    public string Header { get; set; }
  }
}
