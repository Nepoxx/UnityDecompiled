// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.UnityEvent`1
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine.Events
{
  [Serializable]
  public abstract class UnityEvent<T0> : UnityEventBase
  {
    private object[] m_InvokeArray = (object[]) null;

    [RequiredByNativeCode]
    public UnityEvent()
    {
    }

    public void AddListener(UnityAction<T0> call)
    {
      this.AddCall(UnityEvent<T0>.GetDelegate(call));
    }

    public void RemoveListener(UnityAction<T0> call)
    {
      this.RemoveListener(call.Target, call.GetMethodInfo());
    }

    protected override MethodInfo FindMethod_Impl(string name, object targetObj)
    {
      return UnityEventBase.GetValidMethodInfo(targetObj, name, new System.Type[1]{ typeof (T0) });
    }

    internal override BaseInvokableCall GetDelegate(object target, MethodInfo theFunction)
    {
      return (BaseInvokableCall) new InvokableCall<T0>(target, theFunction);
    }

    private static BaseInvokableCall GetDelegate(UnityAction<T0> action)
    {
      return (BaseInvokableCall) new InvokableCall<T0>(action);
    }

    public void Invoke(T0 arg0)
    {
      List<BaseInvokableCall> baseInvokableCallList = this.PrepareInvoke();
      for (int index = 0; index < baseInvokableCallList.Count; ++index)
      {
        InvokableCall<T0> invokableCall = baseInvokableCallList[index] as InvokableCall<T0>;
        if (invokableCall != null)
        {
          invokableCall.Invoke(arg0);
        }
        else
        {
          BaseInvokableCall baseInvokableCall = baseInvokableCallList[index];
          if (this.m_InvokeArray == null)
            this.m_InvokeArray = new object[1];
          this.m_InvokeArray[0] = (object) arg0;
          baseInvokableCall.Invoke(this.m_InvokeArray);
        }
      }
    }

    internal void AddPersistentListener(UnityAction<T0> call)
    {
      this.AddPersistentListener(call, UnityEventCallState.RuntimeOnly);
    }

    internal void AddPersistentListener(UnityAction<T0> call, UnityEventCallState callState)
    {
      int persistentEventCount = this.GetPersistentEventCount();
      this.AddPersistentListener();
      this.RegisterPersistentListener(persistentEventCount, call);
      this.SetPersistentListenerState(persistentEventCount, callState);
    }

    internal void RegisterPersistentListener(int index, UnityAction<T0> call)
    {
      if (call == null)
        Debug.LogWarning((object) "Registering a Listener requires an action");
      else
        this.RegisterPersistentListener(index, (object) (call.Target as UnityEngine.Object), call.Method);
    }
  }
}
