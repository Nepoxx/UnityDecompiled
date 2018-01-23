// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.ManipulatorActivationFilter
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  public struct ManipulatorActivationFilter
  {
    public MouseButton button;
    public EventModifiers modifiers;

    public bool Matches(IMouseEvent e)
    {
      return this.button == (MouseButton) e.button && this.HasModifiers(e);
    }

    private bool HasModifiers(IMouseEvent e)
    {
      return ((this.modifiers & EventModifiers.Alt) == EventModifiers.None || e.altKey) && ((this.modifiers & EventModifiers.Alt) != EventModifiers.None || !e.altKey) && (((this.modifiers & EventModifiers.Control) == EventModifiers.None || e.ctrlKey) && ((this.modifiers & EventModifiers.Control) != EventModifiers.None || !e.ctrlKey)) && (((this.modifiers & EventModifiers.Shift) == EventModifiers.None || e.shiftKey) && ((this.modifiers & EventModifiers.Shift) != EventModifiers.None || !e.shiftKey) && (((this.modifiers & EventModifiers.Command) == EventModifiers.None || e.commandKey) && ((this.modifiers & EventModifiers.Command) != EventModifiers.None || !e.commandKey)));
    }
  }
}
