// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.UnityEvent
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
  public class UnityEvent : UnityEventBase
  {
    private object[] m_InvokeArray = (object[]) null;

    [RequiredByNativeCode]
    public UnityEvent()
    {
    }

    public void AddListener(UnityAction call)
    {
      this.AddCall(UnityEvent.GetDelegate(call));
    }

    public void RemoveListener(UnityAction call)
    {
      this.RemoveListener(call.Target, call.GetMethodInfo());
    }

    protected override MethodInfo FindMethod_Impl(string name, object targetObj)
    {
      return UnityEventBase.GetValidMethodInfo(targetObj, name, new System.Type[0]);
    }

    internal override BaseInvokableCall GetDelegate(object target, MethodInfo theFunction)
    {
      return (BaseInvokableCall) new InvokableCall(target, theFunction);
    }

    private static BaseInvokableCall GetDelegate(UnityAction action)
    {
      return (BaseInvokableCall) new InvokableCall(action);
    }

    public void Invoke()
    {
      List<BaseInvokableCall> baseInvokableCallList = this.PrepareInvoke();
      for (int index = 0; index < baseInvokableCallList.Count; ++index)
      {
        InvokableCall invokableCall = baseInvokableCallList[index] as InvokableCall;
        if (invokableCall != null)
        {
          invokableCall.Invoke();
        }
        else
        {
          BaseInvokableCall baseInvokableCall = baseInvokableCallList[index];
          if (this.m_InvokeArray == null)
            this.m_InvokeArray = new object[0];
          baseInvokableCall.Invoke(this.m_InvokeArray);
        }
      }
    }

    internal void AddPersistentListener(UnityAction call)
    {
      this.AddPersistentListener(call, UnityEventCallState.RuntimeOnly);
    }

    internal void AddPersistentListener(UnityAction call, UnityEventCallState callState)
    {
      int persistentEventCount = this.GetPersistentEventCount();
      this.AddPersistentListener();
      this.RegisterPersistentListener(persistentEventCount, call);
      this.SetPersistentListenerState(persistentEventCount, callState);
    }

    internal void RegisterPersistentListener(int index, UnityAction call)
    {
      if (call == null)
        Debug.LogWarning((object) "Registering a Listener requires an action");
      else
        this.RegisterPersistentListener(index, (object) (call.Target as UnityEngine.Object), call.Method);
    }
  }
}
