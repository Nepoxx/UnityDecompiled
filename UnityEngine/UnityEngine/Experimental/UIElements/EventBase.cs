// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.EventBase
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>The base class for all UIElements events.</para>
  /// </summary>
  public abstract class EventBase
  {
    private static long s_LastTypeId = 0;
    protected EventBase.EventFlags flags;
    protected IEventHandler m_CurrentTarget;

    protected EventBase()
    {
      this.Init();
    }

    /// <summary>
    ///   <para>Register an event class to the event type system.</para>
    /// </summary>
    /// <returns>
    ///   <para>The type ID.</para>
    /// </returns>
    protected static long RegisterEventType()
    {
      return ++EventBase.s_LastTypeId;
    }

    /// <summary>
    ///   <para>Get the type id for this event instance.</para>
    /// </summary>
    /// <returns>
    ///   <para>The type ID.</para>
    /// </returns>
    public abstract long GetEventTypeId();

    /// <summary>
    ///   <para>The time at which the event was created.</para>
    /// </summary>
    public long timestamp { get; private set; }

    /// <summary>
    ///   <para>Returns whether this event type bubbles up in the event propagation path.</para>
    /// </summary>
    public bool bubbles
    {
      get
      {
        return (this.flags & EventBase.EventFlags.Bubbles) != EventBase.EventFlags.None;
      }
    }

    /// <summary>
    ///   <para>Return whether this event is sent down the event propagation path during the capture phase.</para>
    /// </summary>
    public bool capturable
    {
      get
      {
        return (this.flags & EventBase.EventFlags.Capturable) != EventBase.EventFlags.None;
      }
    }

    /// <summary>
    ///   <para>The target for this event. The is the visual element that received the event. Unlike currentTarget, target does not change when the event is sent to elements along the propagation path.</para>
    /// </summary>
    public IEventHandler target { get; internal set; }

    /// <summary>
    ///   <para>Return true if StopPropagation() has been called for this event.</para>
    /// </summary>
    public bool isPropagationStopped { get; private set; }

    /// <summary>
    ///   <para>Stop the propagation of this event. The event will not be sent to any further element in the propagation path. Further event handlers on the current target will be executed.</para>
    /// </summary>
    public void StopPropagation()
    {
      this.isPropagationStopped = true;
    }

    /// <summary>
    ///   <para>Return true if StopImmediatePropagation() has been called for this event.</para>
    /// </summary>
    public bool isImmediatePropagationStopped { get; private set; }

    /// <summary>
    ///   <para>Immediately stop the propagation of this event. The event will not be sent to any further event handlers on the current target or on any other element in the propagation path.</para>
    /// </summary>
    public void StopImmediatePropagation()
    {
      this.isPropagationStopped = true;
      this.isImmediatePropagationStopped = true;
    }

    /// <summary>
    ///   <para>Return true if the default actions should not be executed for this event.</para>
    /// </summary>
    public bool isDefaultPrevented { get; private set; }

    /// <summary>
    ///   <para>Call this function to prevent the execution of the default actions for this event.</para>
    /// </summary>
    public void PreventDefault()
    {
      if ((this.flags & EventBase.EventFlags.Cancellable) != EventBase.EventFlags.Cancellable)
        return;
      this.isDefaultPrevented = true;
    }

    /// <summary>
    ///   <para>The current propagation phase.</para>
    /// </summary>
    public PropagationPhase propagationPhase { get; internal set; }

    /// <summary>
    ///   <para>The current target of the event. The current target is the element in the propagation path for which event handlers are currently being executed.</para>
    /// </summary>
    public virtual IEventHandler currentTarget
    {
      get
      {
        return this.m_CurrentTarget;
      }
      internal set
      {
        this.m_CurrentTarget = value;
        if (this.imguiEvent == null)
          return;
        VisualElement currentTarget = this.currentTarget as VisualElement;
        if (currentTarget != null)
          this.imguiEvent.mousePosition = currentTarget.WorldToLocal(this.imguiEvent.mousePosition);
      }
    }

    /// <summary>
    ///   <para>Return whether the event is currently being dispatched to visual element. An event can not be redispatched while being dispatched. If you need to recursively redispatch an event, you should use a copy.</para>
    /// </summary>
    public bool dispatch { get; internal set; }

    /// <summary>
    ///   <para>The IMGUIEvent at the source of this event. This can be null as not all events are generated by IMGUI.</para>
    /// </summary>
    public Event imguiEvent { get; protected set; }

    /// <summary>
    ///   <para>Reset the event members to their initial value.</para>
    /// </summary>
    protected virtual void Init()
    {
      this.timestamp = DateTime.Now.Ticks;
      this.flags = EventBase.EventFlags.None;
      this.target = (IEventHandler) null;
      this.isPropagationStopped = false;
      this.isImmediatePropagationStopped = false;
      this.isDefaultPrevented = false;
      this.propagationPhase = PropagationPhase.None;
      this.m_CurrentTarget = (IEventHandler) null;
      this.dispatch = false;
      this.imguiEvent = (Event) null;
    }

    /// <summary>
    ///   <para>Flags to describe the characteristics of an event.</para>
    /// </summary>
    [Flags]
    public enum EventFlags
    {
      None = 0,
      Bubbles = 1,
      Capturable = 2,
      Cancellable = 4,
    }
  }
}
