// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.InvokableCall
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Reflection;
using UnityEngineInternal;

namespace UnityEngine.Events
{
  internal class InvokableCall : BaseInvokableCall
  {
    public InvokableCall(object target, MethodInfo theFunction)
      : base(target, theFunction)
    {
      this.Delegate += (UnityAction) theFunction.CreateDelegate(typeof (UnityAction), target);
    }

    public InvokableCall(UnityAction action)
    {
      this.Delegate += action;
    }

    private event UnityAction Delegate;

    public override void Invoke(object[] args)
    {
      // ISSUE: reference to a compiler-generated field
      if (!BaseInvokableCall.AllowInvoke((System.Delegate) this.Delegate))
        return;
      // ISSUE: reference to a compiler-generated field
      this.Delegate();
    }

    public void Invoke()
    {
      // ISSUE: reference to a compiler-generated field
      if (!BaseInvokableCall.AllowInvoke((System.Delegate) this.Delegate))
        return;
      // ISSUE: reference to a compiler-generated field
      this.Delegate();
    }

    public override bool Find(object targetObj, MethodInfo method)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.Delegate.Target == targetObj && this.Delegate.GetMethodInfo().Equals((object) method);
    }
  }
}
