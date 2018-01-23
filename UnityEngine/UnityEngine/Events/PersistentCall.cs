// Decompiled with JetBrains decompiler
// Type: UnityEngine.Events.PersistentCall
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Reflection;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
  [Serializable]
  internal class PersistentCall
  {
    [FormerlySerializedAs("mode")]
    [SerializeField]
    private PersistentListenerMode m_Mode = PersistentListenerMode.EventDefined;
    [FormerlySerializedAs("arguments")]
    [SerializeField]
    private ArgumentCache m_Arguments = new ArgumentCache();
    [FormerlySerializedAs("enabled")]
    [FormerlySerializedAs("m_Enabled")]
    [SerializeField]
    private UnityEventCallState m_CallState = UnityEventCallState.RuntimeOnly;
    [FormerlySerializedAs("instance")]
    [SerializeField]
    private UnityEngine.Object m_Target;
    [FormerlySerializedAs("methodName")]
    [SerializeField]
    private string m_MethodName;

    public UnityEngine.Object target
    {
      get
      {
        return this.m_Target;
      }
    }

    public string methodName
    {
      get
      {
        return this.m_MethodName;
      }
    }

    public PersistentListenerMode mode
    {
      get
      {
        return this.m_Mode;
      }
      set
      {
        this.m_Mode = value;
      }
    }

    public ArgumentCache arguments
    {
      get
      {
        return this.m_Arguments;
      }
    }

    public UnityEventCallState callState
    {
      get
      {
        return this.m_CallState;
      }
      set
      {
        this.m_CallState = value;
      }
    }

    public bool IsValid()
    {
      return this.target != (UnityEngine.Object) null && !string.IsNullOrEmpty(this.methodName);
    }

    public BaseInvokableCall GetRuntimeCall(UnityEventBase theEvent)
    {
      if (this.m_CallState == UnityEventCallState.RuntimeOnly && !Application.isPlaying || (this.m_CallState == UnityEventCallState.Off || theEvent == null))
        return (BaseInvokableCall) null;
      MethodInfo method = theEvent.FindMethod(this);
      if (method == null)
        return (BaseInvokableCall) null;
      switch (this.m_Mode)
      {
        case PersistentListenerMode.EventDefined:
          return theEvent.GetDelegate((object) this.target, method);
        case PersistentListenerMode.Void:
          return (BaseInvokableCall) new InvokableCall((object) this.target, method);
        case PersistentListenerMode.Object:
          return PersistentCall.GetObjectCall(this.target, method, this.m_Arguments);
        case PersistentListenerMode.Int:
          return (BaseInvokableCall) new CachedInvokableCall<int>(this.target, method, this.m_Arguments.intArgument);
        case PersistentListenerMode.Float:
          return (BaseInvokableCall) new CachedInvokableCall<float>(this.target, method, this.m_Arguments.floatArgument);
        case PersistentListenerMode.String:
          return (BaseInvokableCall) new CachedInvokableCall<string>(this.target, method, this.m_Arguments.stringArgument);
        case PersistentListenerMode.Bool:
          return (BaseInvokableCall) new CachedInvokableCall<bool>(this.target, method, this.m_Arguments.boolArgument);
        default:
          return (BaseInvokableCall) null;
      }
    }

    private static BaseInvokableCall GetObjectCall(UnityEngine.Object target, MethodInfo method, ArgumentCache arguments)
    {
      System.Type type = typeof (UnityEngine.Object);
      if (!string.IsNullOrEmpty(arguments.unityObjectArgumentAssemblyTypeName))
        type = System.Type.GetType(arguments.unityObjectArgumentAssemblyTypeName, false) ?? typeof (UnityEngine.Object);
      ConstructorInfo constructor = typeof (CachedInvokableCall<>).MakeGenericType(type).GetConstructor(new System.Type[3]{ typeof (UnityEngine.Object), typeof (MethodInfo), type });
      UnityEngine.Object @object = arguments.unityObjectArgument;
      if (@object != (UnityEngine.Object) null && !type.IsAssignableFrom(@object.GetType()))
        @object = (UnityEngine.Object) null;
      return constructor.Invoke(new object[3]{ (object) target, (object) method, (object) @object }) as BaseInvokableCall;
    }

    public void RegisterPersistentListener(UnityEngine.Object ttarget, string mmethodName)
    {
      this.m_Target = ttarget;
      this.m_MethodName = mmethodName;
    }

    public void UnregisterPersistentListener()
    {
      this.m_MethodName = string.Empty;
      this.m_Target = (UnityEngine.Object) null;
    }
  }
}
