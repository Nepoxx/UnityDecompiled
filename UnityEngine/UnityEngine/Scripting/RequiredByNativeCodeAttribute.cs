// Decompiled with JetBrains decompiler
// Type: UnityEngine.Scripting.RequiredByNativeCodeAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Scripting
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface, Inherited = false)]
  internal class RequiredByNativeCodeAttribute : Attribute
  {
    public RequiredByNativeCodeAttribute()
    {
    }

    public RequiredByNativeCodeAttribute(string name)
    {
      this.Name = name;
    }

    public RequiredByNativeCodeAttribute(bool optional)
    {
      this.Optional = optional;
    }

    public RequiredByNativeCodeAttribute(string name, bool optional)
    {
      this.Name = name;
      this.Optional = optional;
    }

    public string Name { get; set; }

    public bool Optional { get; set; }
  }
}
