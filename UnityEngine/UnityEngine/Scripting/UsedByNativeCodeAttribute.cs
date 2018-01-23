// Decompiled with JetBrains decompiler
// Type: UnityEngine.Scripting.UsedByNativeCodeAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Scripting
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface, Inherited = false)]
  internal class UsedByNativeCodeAttribute : Attribute
  {
    public UsedByNativeCodeAttribute()
    {
    }

    public UsedByNativeCodeAttribute(string name)
    {
      this.Name = name;
    }

    public string Name { get; set; }
  }
}
