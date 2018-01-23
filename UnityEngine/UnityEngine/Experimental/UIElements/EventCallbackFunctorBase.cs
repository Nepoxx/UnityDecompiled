// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.EventCallbackFunctorBase
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  internal abstract class EventCallbackFunctorBase
  {
    protected EventCallbackFunctorBase(CallbackPhase phase)
    {
      this.phase = phase;
    }

    public CallbackPhase phase { get; private set; }

    public abstract void Invoke(EventBase evt);

    public abstract bool IsEquivalentTo(long eventTypeId, Delegate callback, CallbackPhase phase);

    protected bool PhaseMatches(EventBase evt)
    {
      switch (this.phase)
      {
        case CallbackPhase.TargetAndBubbleUp:
          if (evt.propagationPhase != PropagationPhase.AtTarget && evt.propagationPhase != PropagationPhase.BubbleUp)
            return false;
          break;
        case CallbackPhase.CaptureAndTarget:
          if (evt.propagationPhase != PropagationPhase.Capture && evt.propagationPhase != PropagationPhase.AtTarget)
            return false;
          break;
      }
      return true;
    }
  }
}
