// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.InvokableCall`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events
{
  internal class InvokableCall<T1> : BaseInvokableCall
  {
    public InvokableCall(object target, MethodInfo theFunction)
      : base(target, theFunction)
    {
      this.Delegate += (UnityAction<T1>) theFunction.CreateDelegate(typeof (UnityAction<T1>), target);
    }

    public InvokableCall(UnityAction<T1> action)
    {
      this.Delegate += action;
    }

    protected event UnityAction<T1> Delegate;

    public override void Invoke(object[] args)
    {
      if (args.Length != 1)
        throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
      BaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
      // ISSUE: reference to a compiler-generated field
      if (!BaseInvokableCall.AllowInvoke((System.Delegate) this.Delegate))
        return;
      // ISSUE: reference to a compiler-generated field
      this.Delegate((T1) args[0]);
    }

    public virtual void Invoke(T1 args0)
    {
      // ISSUE: reference to a compiler-generated field
      if (!BaseInvokableCall.AllowInvoke((System.Delegate) this.Delegate))
        return;
      // ISSUE: reference to a compiler-generated field
      this.Delegate(args0);
    }

    public override bool Find(object targetObj, MethodInfo method)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.Delegate.Target == targetObj && this.Delegate.GetMethodInfo().Equals((object) method);
    }
  }
}
