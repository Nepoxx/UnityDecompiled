// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.FocusController
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>Class in charge of managing the focus inside a Panel.</para>
  /// </summary>
  public class FocusController
  {
    /// <summary>
    ///   <para>Constructor.</para>
    /// </summary>
    /// <param name="focusRing"></param>
    public FocusController(IFocusRing focusRing)
    {
      this.focusRing = focusRing;
      this.focusedElement = (Focusable) null;
      this.imguiKeyboardControl = 0;
    }

    private IFocusRing focusRing { get; set; }

    /// <summary>
    ///   <para>The currently focused element.</para>
    /// </summary>
    public Focusable focusedElement { get; private set; }

    private static void AboutToReleaseFocus(Focusable focusable, Focusable willGiveFocusTo, FocusChangeDirection direction)
    {
      FocusOutEvent pooled = FocusEventBase<FocusOutEvent>.GetPooled((IEventHandler) focusable, willGiveFocusTo, direction);
      UIElementsUtility.eventDispatcher.DispatchEvent((EventBase) pooled, (IPanel) null);
      EventBase<FocusOutEvent>.ReleasePooled(pooled);
    }

    private static void ReleaseFocus(Focusable focusable, Focusable willGiveFocusTo, FocusChangeDirection direction)
    {
      BlurEvent pooled = FocusEventBase<BlurEvent>.GetPooled((IEventHandler) focusable, willGiveFocusTo, direction);
      UIElementsUtility.eventDispatcher.DispatchEvent((EventBase) pooled, (IPanel) null);
      EventBase<BlurEvent>.ReleasePooled(pooled);
    }

    private static void AboutToGrabFocus(Focusable focusable, Focusable willTakeFocusFrom, FocusChangeDirection direction)
    {
      FocusInEvent pooled = FocusEventBase<FocusInEvent>.GetPooled((IEventHandler) focusable, willTakeFocusFrom, direction);
      UIElementsUtility.eventDispatcher.DispatchEvent((EventBase) pooled, (IPanel) null);
      EventBase<FocusInEvent>.ReleasePooled(pooled);
    }

    private static void GrabFocus(Focusable focusable, Focusable willTakeFocusFrom, FocusChangeDirection direction)
    {
      FocusEvent pooled = FocusEventBase<FocusEvent>.GetPooled((IEventHandler) focusable, willTakeFocusFrom, direction);
      UIElementsUtility.eventDispatcher.DispatchEvent((EventBase) pooled, (IPanel) null);
      EventBase<FocusEvent>.ReleasePooled(pooled);
    }

    internal void SwitchFocus(Focusable newFocusedElement)
    {
      this.SwitchFocus(newFocusedElement, FocusChangeDirection.unspecified);
    }

    private void SwitchFocus(Focusable newFocusedElement, FocusChangeDirection direction)
    {
      if (newFocusedElement == this.focusedElement)
        return;
      Focusable focusedElement = this.focusedElement;
      if (newFocusedElement == null || !newFocusedElement.canGrabFocus)
      {
        if (focusedElement == null)
          return;
        FocusController.AboutToReleaseFocus(focusedElement, newFocusedElement, direction);
        this.focusedElement = (Focusable) null;
        FocusController.ReleaseFocus(focusedElement, newFocusedElement, direction);
      }
      else
      {
        if (newFocusedElement == focusedElement)
          return;
        if (focusedElement != null)
          FocusController.AboutToReleaseFocus(focusedElement, newFocusedElement, direction);
        FocusController.AboutToGrabFocus(newFocusedElement, focusedElement, direction);
        this.focusedElement = newFocusedElement;
        if (focusedElement != null)
          FocusController.ReleaseFocus(focusedElement, newFocusedElement, direction);
        FocusController.GrabFocus(newFocusedElement, focusedElement, direction);
      }
    }

    /// <summary>
    ///   <para>Ask the controller to change the focus according to the event. The focus controller will use its focus ring to choose the next element to be focused.</para>
    /// </summary>
    /// <param name="e"></param>
    public void SwitchFocusOnEvent(EventBase e)
    {
      FocusChangeDirection focusChangeDirection = this.focusRing.GetFocusChangeDirection(this.focusedElement, e);
      if (focusChangeDirection == FocusChangeDirection.none)
        return;
      this.SwitchFocus(this.focusRing.GetNextFocusable(this.focusedElement, focusChangeDirection), focusChangeDirection);
    }

    internal int imguiKeyboardControl { get; set; }

    internal void SyncIMGUIFocus(IMGUIContainer imguiContainerHavingKeyboardControl)
    {
      if (GUIUtility.keyboardControl == this.imguiKeyboardControl)
        return;
      this.imguiKeyboardControl = GUIUtility.keyboardControl;
      if (GUIUtility.keyboardControl != 0)
        this.SwitchFocus((Focusable) imguiContainerHavingKeyboardControl, FocusChangeDirection.unspecified);
      else
        this.SwitchFocus((Focusable) null, FocusChangeDirection.unspecified);
    }
  }
}
