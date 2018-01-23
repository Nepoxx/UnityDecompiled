// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.PersistentCallGroup
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
  [Serializable]
  internal class PersistentCallGroup
  {
    [FormerlySerializedAs("m_Listeners")]
    [SerializeField]
    private List<PersistentCall> m_Calls;

    public PersistentCallGroup()
    {
      this.m_Calls = new List<PersistentCall>();
    }

    public int Count
    {
      get
      {
        return this.m_Calls.Count;
      }
    }

    public PersistentCall GetListener(int index)
    {
      return this.m_Calls[index];
    }

    public IEnumerable<PersistentCall> GetListeners()
    {
      return (IEnumerable<PersistentCall>) this.m_Calls;
    }

    public void AddListener()
    {
      this.m_Calls.Add(new PersistentCall());
    }

    public void AddListener(PersistentCall call)
    {
      this.m_Calls.Add(call);
    }

    public void RemoveListener(int index)
    {
      this.m_Calls.RemoveAt(index);
    }

    public void Clear()
    {
      this.m_Calls.Clear();
    }

    public void RegisterEventPersistentListener(int index, UnityEngine.Object targetObj, string methodName)
    {
      PersistentCall listener = this.GetListener(index);
      listener.RegisterPersistentListener(targetObj, methodName);
      listener.mode = PersistentListenerMode.EventDefined;
    }

    public void RegisterVoidPersistentListener(int index, UnityEngine.Object targetObj, string methodName)
    {
      PersistentCall listener = this.GetListener(index);
      listener.RegisterPersistentListener(targetObj, methodName);
      listener.mode = PersistentListenerMode.Void;
    }

    public void RegisterObjectPersistentListener(int index, UnityEngine.Object targetObj, UnityEngine.Object argument, string methodName)
    {
      PersistentCall listener = this.GetListener(index);
      listener.RegisterPersistentListener(targetObj, methodName);
      listener.mode = PersistentListenerMode.Object;
      listener.arguments.unityObjectArgument = argument;
    }

    public void RegisterIntPersistentListener(int index, UnityEngine.Object targetObj, int argument, string methodName)
    {
      PersistentCall listener = this.GetListener(index);
      listener.RegisterPersistentListener(targetObj, methodName);
      listener.mode = PersistentListenerMode.Int;
      listener.arguments.intArgument = argument;
    }

    public void RegisterFloatPersistentListener(int index, UnityEngine.Object targetObj, float argument, string methodName)
    {
      PersistentCall listener = this.GetListener(index);
      listener.RegisterPersistentListener(targetObj, methodName);
      listener.mode = PersistentListenerMode.Float;
      listener.arguments.floatArgument = argument;
    }

    public void RegisterStringPersistentListener(int index, UnityEngine.Object targetObj, string argument, string methodName)
    {
      PersistentCall listener = this.GetListener(index);
      listener.RegisterPersistentListener(targetObj, methodName);
      listener.mode = PersistentListenerMode.String;
      listener.arguments.stringArgument = argument;
    }

    public void RegisterBoolPersistentListener(int index, UnityEngine.Object targetObj, bool argument, string methodName)
    {
      PersistentCall listener = this.GetListener(index);
      listener.RegisterPersistentListener(targetObj, methodName);
      listener.mode = PersistentListenerMode.Bool;
      listener.arguments.boolArgument = argument;
    }

    public void UnregisterPersistentListener(int index)
    {
      this.GetListener(index).UnregisterPersistentListener();
    }

    public void RemoveListeners(UnityEngine.Object target, string methodName)
    {
      List<PersistentCall> persistentCallList = new List<PersistentCall>();
      for (int index = 0; index < this.m_Calls.Count; ++index)
      {
        if (this.m_Calls[index].target == target && this.m_Calls[index].methodName == methodName)
          persistentCallList.Add(this.m_Calls[index]);
      }
      this.m_Calls.RemoveAll(new Predicate<PersistentCall>(persistentCallList.Contains));
    }

    public void Initialize(InvokableCallList invokableList, UnityEventBase unityEventBase)
    {
      foreach (PersistentCall call in this.m_Calls)
      {
        if (call.IsValid())
        {
          BaseInvokableCall runtimeCall = call.GetRuntimeCall(unityEventBase);
          if (runtimeCall != null)
            invokableList.AddPersistentInvokableCall(runtimeCall);
        }
      }
    }
  }
}
