// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.TooltipElement
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
  internal class TooltipElement : VisualElement
  {
    public TooltipElement()
    {
      this.RegisterCallback<TooltipEvent>(new EventCallback<TooltipEvent>(this.OnTooltip), Capture.NoCapture);
    }

    public string tooltip { get; set; }

    private void OnTooltip(TooltipEvent e)
    {
      e.tooltip = this.tooltip;
      e.rect = this.worldBound;
      e.StopPropagation();
    }
  }
}
