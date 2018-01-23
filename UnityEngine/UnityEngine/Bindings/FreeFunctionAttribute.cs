// Decompiled with JetBrains decompiler
// Type: UnityEngine.Bindings.FreeFunctionAttribute
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Bindings
{
  [AttributeUsage(AttributeTargets.Method)]
  internal class FreeFunctionAttribute : NativeMethodAttribute
  {
    public FreeFunctionAttribute()
    {
      this.IsFreeFunction = true;
    }

    public FreeFunctionAttribute(string name)
      : base(name, true)
    {
    }

    public FreeFunctionAttribute(string name, bool isThreadSafe)
      : base(name, true, isThreadSafe)
    {
    }
  }
}
