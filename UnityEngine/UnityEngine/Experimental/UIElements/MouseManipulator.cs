// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.MouseManipulator
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
  public abstract class MouseManipulator : Manipulator
  {
    private ManipulatorActivationFilter m_currentActivator;

    public MouseManipulator()
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
          this.m_currentActivator = activator;
          return true;
        }
      }
      return false;
    }

    protected bool CanStopManipulation(IMouseEvent e)
    {
      return (MouseButton) e.button == this.m_currentActivator.button && this.target.HasCapture();
    }
  }
}
