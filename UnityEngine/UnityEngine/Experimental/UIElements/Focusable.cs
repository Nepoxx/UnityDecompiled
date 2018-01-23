// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.Focusable
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>Base class for objects that can get the focus.</para>
  /// </summary>
  public abstract class Focusable : CallbackEventHandler
  {
    private int m_FocusIndex;

    protected Focusable()
    {
      this.m_FocusIndex = 0;
    }

    /// <summary>
    ///   <para>Return the focus controller for this element.</para>
    /// </summary>
    public abstract FocusController focusController { get; }

    /// <summary>
    ///   <para>An integer used to sort focusables in the focus ring. A negative value means that the element can not be focused.</para>
    /// </summary>
    public int focusIndex
    {
      get
      {
        return this.m_FocusIndex;
      }
      set
      {
        this.m_FocusIndex = value;
      }
    }

    /// <summary>
    ///   <para>Return true if the element can be focused.</para>
    /// </summary>
    public virtual bool canGrabFocus
    {
      get
      {
        return this.m_FocusIndex >= 0;
      }
    }

    /// <summary>
    ///   <para>Attempt to give the focus to this element.</para>
    /// </summary>
    public virtual void Focus()
    {
      if (this.focusController == null)
        return;
      this.focusController.SwitchFocus(!this.canGrabFocus ? (Focusable) null : this);
    }

    /// <summary>
    ///   <para>Tell the element to release the focus.</para>
    /// </summary>
    public void Blur()
    {
      if (this.focusController == null || this.focusController.focusedElement != this)
        return;
      this.focusController.SwitchFocus((Focusable) null);
    }

    protected internal override void ExecuteDefaultAction(EventBase evt)
    {
      base.ExecuteDefaultAction(evt);
      if (evt.GetEventTypeId() == EventBase<MouseDownEvent>.TypeId() && (evt as MouseDownEvent).button == 0)
        this.Focus();
      if (this.focusController == null)
        return;
      this.focusController.SwitchFocusOnEvent(evt);
    }
  }
}
