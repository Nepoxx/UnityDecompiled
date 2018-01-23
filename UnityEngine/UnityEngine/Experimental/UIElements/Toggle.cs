// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.Toggle
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  public class Toggle : VisualElement
  {
    private Action clickEvent;

    public Toggle(Action clickEvent)
    {
      this.clickEvent = clickEvent;
      this.AddManipulator((IManipulator) new Clickable(new Action(this.OnClick)));
    }

    public bool on
    {
      get
      {
        return (this.pseudoStates & PseudoStates.Checked) == PseudoStates.Checked;
      }
      set
      {
        if (value)
          this.pseudoStates |= PseudoStates.Checked;
        else
          this.pseudoStates &= ~PseudoStates.Checked;
      }
    }

    /// <summary>
    ///   <para>Sets the event callback for this toggle button.</para>
    /// </summary>
    /// <param name="clickEvent">The action to be called when this Toggle is clicked.</param>
    public void OnToggle(Action clickEvent)
    {
      this.clickEvent = clickEvent;
    }

    private void OnClick()
    {
      this.on = !this.on;
      if (this.clickEvent == null)
        return;
      this.clickEvent();
    }
  }
}
