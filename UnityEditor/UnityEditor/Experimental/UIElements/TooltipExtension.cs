// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.TooltipExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.UIElements
{
  internal static class TooltipExtension
  {
    [RequiredByNativeCode]
    internal static void SetTooltip(float mouseX, float mouseY)
    {
      GUIView mouseOverView = GUIView.mouseOverView;
      if (!((Object) mouseOverView != (Object) null) || mouseOverView.visualTree == null || mouseOverView.visualTree.panel == null)
        return;
      IPanel panel = mouseOverView.visualTree.panel;
      VisualElement visualElement = panel.Pick(new Vector2(mouseX, mouseY) - mouseOverView.screenPosition.position);
      if (visualElement != null)
      {
        TooltipEvent pooled = EventBase<TooltipEvent>.GetPooled();
        pooled.target = (IEventHandler) visualElement;
        pooled.tooltip = (string) null;
        pooled.rect = Rect.zero;
        mouseOverView.visualTree.panel.dispatcher.DispatchEvent((EventBase) pooled, panel);
        if (!string.IsNullOrEmpty(pooled.tooltip))
        {
          Rect rect = pooled.rect;
          rect.position += mouseOverView.screenPosition.position;
          GUIStyle.SetMouseTooltip(pooled.tooltip, rect);
        }
      }
    }

    internal static void AddTooltip(this VisualElement e, string tooltip)
    {
      if (string.IsNullOrEmpty(tooltip))
      {
        e.RemoveTooltip();
      }
      else
      {
        TooltipElement tooltipElement = (TooltipElement) e.Query().Children<TooltipElement>((string) null, (string) null) ?? new TooltipElement();
        tooltipElement.style.positionType = (StyleValue<PositionType>) PositionType.Absolute;
        IStyle style = tooltipElement.style;
        StyleValue<float> styleValue1 = (StyleValue<float>) 0.0f;
        tooltipElement.style.positionBottom = styleValue1;
        StyleValue<float> styleValue2 = styleValue1;
        tooltipElement.style.positionTop = styleValue2;
        StyleValue<float> styleValue3 = styleValue2;
        tooltipElement.style.positionRight = styleValue3;
        StyleValue<float> styleValue4 = styleValue3;
        style.positionLeft = styleValue4;
        tooltipElement.tooltip = tooltip;
        e.Add((VisualElement) tooltipElement);
      }
    }

    internal static void RemoveTooltip(this VisualElement e)
    {
      TooltipElement tooltipElement = (TooltipElement) e.Query().Children<TooltipElement>((string) null, (string) null);
      if (tooltipElement == null)
        return;
      e.Remove((VisualElement) tooltipElement);
    }
  }
}
