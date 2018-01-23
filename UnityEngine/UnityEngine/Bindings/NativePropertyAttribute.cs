// Decompiled with JetBrains decompiler
// Type: UnityEngine.Bindings.NativePropertyAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Bindings
{
  [AttributeUsage(AttributeTargets.Property)]
  internal class NativePropertyAttribute : NativeMethodAttribute
  {
    public NativePropertyAttribute()
    {
    }

    public NativePropertyAttribute(string name)
      : base(name)
    {
    }

    public NativePropertyAttribute(string name, TargetType targetType)
      : base(name)
    {
      this.TargetType = targetType;
    }

    public NativePropertyAttribute(string name, bool isFree, TargetType targetType)
      : base(name, isFree)
    {
      this.TargetType = targetType;
    }

    public NativePropertyAttribute(string name, bool isFree, TargetType targetType, bool isThreadSafe)
      : base(name, isFree, isThreadSafe)
    {
      this.TargetType = targetType;
    }

    public TargetType TargetType { get; set; }
  }
}
