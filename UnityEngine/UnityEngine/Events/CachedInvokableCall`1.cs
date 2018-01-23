// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.CachedInvokableCall`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Reflection;

namespace UnityEngine.Events
{
  internal class CachedInvokableCall<T> : InvokableCall<T>
  {
    private readonly T m_Arg1;

    public CachedInvokableCall(Object target, MethodInfo theFunction, T argument)
      : base((object) target, theFunction)
    {
      this.m_Arg1 = argument;
    }

    public override void Invoke(object[] args)
    {
      base.Invoke(this.m_Arg1);
    }

    public override void Invoke(T arg0)
    {
      base.Invoke(this.m_Arg1);
    }
  }
}
