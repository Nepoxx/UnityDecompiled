// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.ClickSelector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class ClickSelector : MouseManipulator
  {
    public ClickSelector()
    {
      this.activators.Add(new ManipulatorActivationFilter()
      {
        button = MouseButton.LeftMouse
      });
      this.activators.Add(new ManipulatorActivationFilter()
      {
        button = MouseButton.RightMouse
      });
    }

    protected override void RegisterCallbacksOnTarget()
    {
      this.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.Capture);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
      this.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.Capture);
    }

    protected void OnMouseDown(MouseDownEvent e)
    {
      if (!(e.currentTarget is ISelectable) || !this.CanStartManipulation((IMouseEvent) e))
        return;
      GraphElement currentTarget = e.currentTarget as GraphElement;
      if (currentTarget != null)
      {
        VisualElement parent = currentTarget.shadow.parent;
        while (parent != null && !(parent is UnityEditor.Experimental.UIElements.GraphView.GraphView))
          parent = parent.shadow.parent;
        UnityEditor.Experimental.UIElements.GraphView.GraphView selectionContainer = parent as UnityEditor.Experimental.UIElements.GraphView.GraphView;
        if (!currentTarget.IsSelected(selectionContainer))
          currentTarget.Select(selectionContainer, e.shiftKey || e.ctrlKey);
      }
    }
  }
}
