// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.EventCallbackFunctor`2
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  internal class EventCallbackFunctor<TEventType, TCallbackArgs> : EventCallbackFunctorBase where TEventType : EventBase<TEventType>, new()
  {
    private EventCallback<TEventType, TCallbackArgs> m_Callback;
    private TCallbackArgs m_UserArgs;
    private long m_EventTypeId;

    public EventCallbackFunctor(EventCallback<TEventType, TCallbackArgs> callback, TCallbackArgs userArgs, CallbackPhase phase)
      : base(phase)
    {
      this.m_Callback = callback;
      this.m_UserArgs = userArgs;
      this.m_EventTypeId = EventBase<TEventType>.TypeId();
    }

    public override void Invoke(EventBase evt)
    {
      if (evt == null)
        throw new ArgumentNullException();
      if (evt.GetEventTypeId() != this.m_EventTypeId || !this.PhaseMatches(evt))
        return;
      this.m_Callback(evt as TEventType, this.m_UserArgs);
    }

    public override bool IsEquivalentTo(long eventTypeId, Delegate callback, CallbackPhase phase)
    {
      return this.m_EventTypeId == eventTypeId && (Delegate) this.m_Callback == callback && this.phase == phase;
    }
  }
}
