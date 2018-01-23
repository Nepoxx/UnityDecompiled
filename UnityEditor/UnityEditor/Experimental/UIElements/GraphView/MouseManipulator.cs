// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.MouseManipulator
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal abstract class MouseManipulator : Manipulator
  {
    private ManipulatorActivationFilter m_CurrentActivator;

    protected MouseManipulator()
    {
      this.activators = new List<ManipulatorActivationFilter>();
    }

    public List<ManipulatorActivationFilter> activators { get; private set; }

    protected bool CanStartManipulation(IMouseEvent e)
    {
      foreach (ManipulatorActivationFilter activator in this.activators)
      {
        if (activator.Matches(e))
        {
          this.m_CurrentActivator = activator;
          return true;
        }
      }
      return false;
    }

    protected bool CanStopManipulation(IMouseEvent e)
    {
      return (MouseButton) e.button == this.m_CurrentActivator.button;
    }
  }
}
