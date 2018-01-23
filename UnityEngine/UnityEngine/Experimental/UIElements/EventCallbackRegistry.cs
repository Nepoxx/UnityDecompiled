// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.EventCallbackRegistry
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  internal class EventCallbackRegistry
  {
    private static readonly EventCallbackListPool s_ListPool = new EventCallbackListPool();
    private EventCallbackList m_Callbacks;
    private EventCallbackList m_TemporaryCallbacks;
    private int m_IsInvoking;

    public EventCallbackRegistry()
    {
      this.m_IsInvoking = 0;
    }

    private static EventCallbackList GetCallbackList(EventCallbackList initializer = null)
    {
      return EventCallbackRegistry.s_ListPool.Get(initializer);
    }

    private static void ReleaseCallbackList(EventCallbackList toRelease)
    {
      EventCallbackRegistry.s_ListPool.Release(toRelease);
    }

    private EventCallbackList GetCallbackListForWriting()
    {
      if (this.m_IsInvoking > 0)
      {
        if (this.m_TemporaryCallbacks == null)
          this.m_TemporaryCallbacks = this.m_Callbacks == null ? EventCallbackRegistry.GetCallbackList((EventCallbackList) null) : EventCallbackRegistry.GetCallbackList(this.m_Callbacks);
        return this.m_TemporaryCallbacks;
      }
      if (this.m_Callbacks == null)
        this.m_Callbacks = EventCallbackRegistry.GetCallbackList((EventCallbackList) null);
      return this.m_Callbacks;
    }

    private EventCallbackList GetCallbackListForReading()
    {
      if (this.m_TemporaryCallbacks != null)
        return this.m_TemporaryCallbacks;
      return this.m_Callbacks;
    }

    private bool ShouldRegisterCallback(long eventTypeId, Delegate callback, CallbackPhase phase)
    {
      if ((object) callback == null)
        return false;
      EventCallbackList callbackListForReading = this.GetCallbackListForReading();
      if (callbackListForReading != null)
        return !callbackListForReading.Contains(eventTypeId, callback, phase);
      return true;
    }

    private bool UnregisterCallback(long eventTypeId, Delegate callback, Capture useCapture)
    {
      if ((object) callback == null)
        return false;
      EventCallbackList callbackListForWriting = this.GetCallbackListForWriting();
      CallbackPhase phase = useCapture != Capture.Capture ? CallbackPhase.TargetAndBubbleUp : CallbackPhase.CaptureAndTarget;
      return callbackListForWriting.Remove(eventTypeId, callback, phase);
    }

    public void RegisterCallback<TEventType>(EventCallback<TEventType> callback, Capture useCapture = Capture.NoCapture) where TEventType : EventBase<TEventType>, new()
    {
      long eventTypeId = EventBase<TEventType>.TypeId();
      CallbackPhase phase = useCapture != Capture.Capture ? CallbackPhase.TargetAndBubbleUp : CallbackPhase.CaptureAndTarget;
      if (!this.ShouldRegisterCallback(eventTypeId, (Delegate) callback, phase))
        return;
      this.GetCallbackListForWriting().Add((EventCallbackFunctorBase) new EventCallbackFunctor<TEventType>(callback, phase));
    }

    public void RegisterCallback<TEventType, TCallbackArgs>(EventCallback<TEventType, TCallbackArgs> callback, TCallbackArgs userArgs, Capture useCapture = Capture.NoCapture) where TEventType : EventBase<TEventType>, new()
    {
      long eventTypeId = EventBase<TEventType>.TypeId();
      CallbackPhase phase = useCapture != Capture.Capture ? CallbackPhase.TargetAndBubbleUp : CallbackPhase.CaptureAndTarget;
      if (!this.ShouldRegisterCallback(eventTypeId, (Delegate) callback, phase))
        return;
      this.GetCallbackListForWriting().Add((EventCallbackFunctorBase) new EventCallbackFunctor<TEventType, TCallbackArgs>(callback, userArgs, phase));
    }

    public bool UnregisterCallback<TEventType>(EventCallback<TEventType> callback, Capture useCapture = Capture.NoCapture) where TEventType : EventBase<TEventType>, new()
    {
      return this.UnregisterCallback(EventBase<TEventType>.TypeId(), (Delegate) callback, useCapture);
    }

    public bool UnregisterCallback<TEventType, TCallbackArgs>(EventCallback<TEventType, TCallbackArgs> callback, Capture useCapture = Capture.NoCapture) where TEventType : EventBase<TEventType>, new()
    {
      return this.UnregisterCallback(EventBase<TEventType>.TypeId(), (Delegate) callback, useCapture);
    }

    public void InvokeCallbacks(EventBase evt)
    {
      if (this.m_Callbacks == null)
        return;
      ++this.m_IsInvoking;
      for (int index = 0; index < this.m_Callbacks.Count && !evt.isImmediatePropagationStopped; ++index)
        this.m_Callbacks[index].Invoke(evt);
      --this.m_IsInvoking;
      if (this.m_IsInvoking != 0 || this.m_TemporaryCallbacks == null)
        return;
      EventCallbackRegistry.ReleaseCallbackList(this.m_Callbacks);
      this.m_Callbacks = EventCallbackRegistry.GetCallbackList(this.m_TemporaryCallbacks);
      EventCallbackRegistry.ReleaseCallbackList(this.m_TemporaryCallbacks);
      this.m_TemporaryCallbacks = (EventCallbackList) null;
    }

    public bool HasCaptureHandlers()
    {
      return this.m_Callbacks != null && this.m_Callbacks.capturingCallbackCount > 0;
    }

    public bool HasBubbleHandlers()
    {
      return this.m_Callbacks != null && this.m_Callbacks.bubblingCallbackCount > 0;
    }
  }
}
