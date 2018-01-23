// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.InvokableCall`4
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events
{
  internal class InvokableCall<T1, T2, T3, T4> : BaseInvokableCall
  {
    public InvokableCall(object target, MethodInfo theFunction)
      : base(target, theFunction)
    {
      // ISSUE: reference to a compiler-generated field
      this.Delegate = (UnityAction<T1, T2, T3, T4>) theFunction.CreateDelegate(typeof (UnityAction<T1, T2, T3, T4>), target);
    }

    public InvokableCall(UnityAction<T1, T2, T3, T4> action)
    {
      this.Delegate += action;
    }

    protected event UnityAction<T1, T2, T3, T4> Delegate;

    public override void Invoke(object[] args)
    {
      if (args.Length != 4)
        throw new ArgumentException("Passed argument 'args' is invalid size. Expected size is 1");
      BaseInvokableCall.ThrowOnInvalidArg<T1>(args[0]);
      BaseInvokableCall.ThrowOnInvalidArg<T2>(args[1]);
      BaseInvokableCall.ThrowOnInvalidArg<T3>(args[2]);
      BaseInvokableCall.ThrowOnInvalidArg<T4>(args[3]);
      // ISSUE: reference to a compiler-generated field
      if (!BaseInvokableCall.AllowInvoke((System.Delegate) this.Delegate))
        return;
      // ISSUE: reference to a compiler-generated field
      this.Delegate((T1) args[0], (T2) args[1], (T3) args[2], (T4) args[3]);
    }

    public void Invoke(T1 args0, T2 args1, T3 args2, T4 args3)
    {
      // ISSUE: reference to a compiler-generated field
      if (!BaseInvokableCall.AllowInvoke((System.Delegate) this.Delegate))
        return;
      // ISSUE: reference to a compiler-generated field
      this.Delegate(args0, args1, args2, args3);
    }

    public override bool Find(object targetObj, MethodInfo method)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.Delegate.Target == targetObj && this.Delegate.GetMethodInfo().Equals((object) method);
    }
  }
}
