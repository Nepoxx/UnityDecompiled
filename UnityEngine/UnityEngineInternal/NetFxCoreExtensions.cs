// Decompiled with JetBrains decompiler
// Type: UnityEngineInternal.NetFxCoreExtensions
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Reflection;

namespace UnityEngineInternal
{
  internal static class NetFxCoreExtensions
  {
    public static Delegate CreateDelegate(this MethodInfo self, Type delegateType, object target)
    {
      return Delegate.CreateDelegate(delegateType, target, self);
    }

    public static MethodInfo GetMethodInfo(this Delegate self)
    {
      return self.Method;
    }
  }
}
