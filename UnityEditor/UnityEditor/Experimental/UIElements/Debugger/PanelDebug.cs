// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.Debugger.PanelDebug
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.Debugger
{
  internal class PanelDebug : BasePanelDebug
  {
    private List<PanelDebug.RepaintData> m_RepaintDatas = new List<PanelDebug.RepaintData>();
    internal uint highlightedElement;

    internal override bool RecordRepaint(VisualElement visualElement)
    {
      if (!this.enabled)
        return false;
      this.m_RepaintDatas.Add(new PanelDebug.RepaintData(visualElement.controlid, visualElement.worldBound, Color.HSVToRGB((float) (visualElement.controlid * 11U % 32U) / 32f, 0.6f, 1f)));
      return true;
    }

    internal override bool EndRepaint()
    {
      if (!this.enabled)
        return false;
      PanelDebug.RepaintData repaintData1 = (PanelDebug.RepaintData) null;
      foreach (PanelDebug.RepaintData repaintData2 in this.m_RepaintDatas)
      {
        Color c = repaintData2.color;
        if ((int) this.highlightedElement != 0)
        {
          if ((int) this.highlightedElement != (int) repaintData2.controlId)
          {
            c = Color.gray;
          }
          else
          {
            repaintData1 = repaintData2;
            continue;
          }
        }
        PickingData.DrawRect(repaintData2.contentRect, c);
      }
      this.m_RepaintDatas.Clear();
      if (repaintData1 != null)
        PickingData.DrawRect(repaintData1.contentRect, repaintData1.color);
      return true;
    }

    public class RepaintData
    {
      public readonly Color color;
      public readonly Rect contentRect;
      public readonly uint controlId;

      public RepaintData(uint controlId, Rect contentRect, Color color)
      {
        this.contentRect = contentRect;
        this.color = color;
        this.controlId = controlId;
      }
    }
  }
}
