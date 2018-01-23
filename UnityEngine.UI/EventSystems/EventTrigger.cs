// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.EventTrigger
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace UnityEngine.EventSystems
{
  /// <summary>
  ///   <para>Receives events from the EventSystem and calls registered functions for each event.</para>
  /// </summary>
  [AddComponentMenu("Event/Event Trigger")]
  public class EventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler, IEventSystemHandler
  {
    [FormerlySerializedAs("delegates")]
    [SerializeField]
    private List<EventTrigger.Entry> m_Delegates;
    /// <summary>
    ///   <para>All the functions registered in this EventTrigger (deprecated).</para>
    /// </summary>
    [Obsolete("Please use triggers instead (UnityUpgradable) -> triggers", true)]
    public List<EventTrigger.Entry> delegates;

    protected EventTrigger()
    {
    }

    /// <summary>
    ///   <para>All the functions registered in this EventTrigger.</para>
    /// </summary>
    public List<EventTrigger.Entry> triggers
    {
      get
      {
        if (this.m_Delegates == null)
          this.m_Delegates = new List<EventTrigger.Entry>();
        return this.m_Delegates;
      }
      set
      {
        this.m_Delegates = value;
      }
    }

    private void Execute(EventTriggerType id, BaseEventData eventData)
    {
      int index = 0;
      for (int count = this.triggers.Count; index < count; ++index)
      {
        EventTrigger.Entry trigger = this.triggers[index];
        if (trigger.eventID == id && trigger.callback != null)
          trigger.callback.Invoke(eventData);
      }
    }

    /// <summary>
    ///   <para>Called by the EventSystem when the pointer enters the object associated with this EventTrigger.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.PointerEnter, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem when the pointer exits the object associated with this EventTrigger.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnPointerExit(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.PointerExit, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem every time the pointer is moved during dragging.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnDrag(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.Drag, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem when an object accepts a drop.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnDrop(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.Drop, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem when a PointerDown event occurs.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.PointerDown, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem when a PointerUp event occurs.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.PointerUp, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem when a Click event occurs.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.PointerClick, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem when a Select event occurs.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnSelect(BaseEventData eventData)
    {
      this.Execute(EventTriggerType.Select, eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem when a new object is being selected.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnDeselect(BaseEventData eventData)
    {
      this.Execute(EventTriggerType.Deselect, eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem when a Scroll event occurs.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnScroll(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.Scroll, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem when a Move event occurs.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnMove(AxisEventData eventData)
    {
      this.Execute(EventTriggerType.Move, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem when the object associated with this EventTrigger is updated.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnUpdateSelected(BaseEventData eventData)
    {
      this.Execute(EventTriggerType.UpdateSelected, eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem when a drag has been found, but before it is valid to begin the drag.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnInitializePotentialDrag(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.InitializePotentialDrag, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Called before a drag is started.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.BeginDrag, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem once dragging ends.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnEndDrag(PointerEventData eventData)
    {
      this.Execute(EventTriggerType.EndDrag, (BaseEventData) eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem when a Submit event occurs.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnSubmit(BaseEventData eventData)
    {
      this.Execute(EventTriggerType.Submit, eventData);
    }

    /// <summary>
    ///   <para>Called by the EventSystem when a Cancel event occurs.</para>
    /// </summary>
    /// <param name="eventData">Current event data.</param>
    public virtual void OnCancel(BaseEventData eventData)
    {
      this.Execute(EventTriggerType.Cancel, eventData);
    }

    /// <summary>
    ///   <para>UnityEvent class for Triggers.</para>
    /// </summary>
    [Serializable]
    public class TriggerEvent : UnityEvent<BaseEventData>
    {
    }

    /// <summary>
    ///   <para>An Entry in the EventSystem delegates list.</para>
    /// </summary>
    [Serializable]
    public class Entry
    {
      /// <summary>
      ///   <para>What type of event is the associated callback listening for.</para>
      /// </summary>
      public EventTriggerType eventID = EventTriggerType.PointerClick;
      /// <summary>
      ///   <para>The desired TriggerEvent to be Invoked.</para>
      /// </summary>
      public EventTrigger.TriggerEvent callback = new EventTrigger.TriggerEvent();
    }
  }
}
