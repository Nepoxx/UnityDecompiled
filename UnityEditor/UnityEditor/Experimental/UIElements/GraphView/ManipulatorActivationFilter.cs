// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.ManipulatorActivationFilter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal struct ManipulatorActivationFilter
  {
    public MouseButton button;
    public EventModifiers modifiers;

    public bool Matches(IMouseEvent evt)
    {
      return this.button == (MouseButton) evt.button && this.HasModifiers(evt);
    }

    private bool HasModifiers(IMouseEvent evt)
    {
      return ((this.modifiers & EventModifiers.Alt) == EventModifiers.None || evt.altKey) && ((this.modifiers & EventModifiers.Alt) != EventModifiers.None || !evt.altKey) && (((this.modifiers & EventModifiers.Control) == EventModifiers.None || evt.ctrlKey) && ((this.modifiers & EventModifiers.Control) != EventModifiers.None || !evt.ctrlKey)) && (((this.modifiers & EventModifiers.Shift) == EventModifiers.None || evt.shiftKey) && ((this.modifiers & EventModifiers.Shift) != EventModifiers.None || !evt.shiftKey) && (((this.modifiers & EventModifiers.Command) == EventModifiers.None || evt.commandKey) && ((this.modifiers & EventModifiers.Command) != EventModifiers.None || !evt.commandKey)));
    }
  }
}
