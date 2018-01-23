// Decompiled with JetBrains decompiler
// Type: UnityEditor.SnapGuideCollection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class SnapGuideCollection
  {
    private List<SnapGuide> currentGuides = (List<SnapGuide>) null;
    private Dictionary<float, List<SnapGuide>> guides = new Dictionary<float, List<SnapGuide>>();

    public void Clear()
    {
      this.guides.Clear();
    }

    public void AddGuide(SnapGuide guide)
    {
      List<SnapGuide> snapGuideList;
      if (!this.guides.TryGetValue(guide.value, out snapGuideList))
      {
        snapGuideList = new List<SnapGuide>();
        this.guides.Add(guide.value, snapGuideList);
      }
      snapGuideList.Add(guide);
    }

    public float SnapToGuides(float value, float snapDistance)
    {
      if (this.guides.Count == 0)
        return value;
      KeyValuePair<float, List<SnapGuide>> keyValuePair = new KeyValuePair<float, List<SnapGuide>>();
      float num1 = float.PositiveInfinity;
      foreach (KeyValuePair<float, List<SnapGuide>> guide in this.guides)
      {
        float num2 = Mathf.Abs(value - guide.Key);
        if ((double) num2 < (double) num1)
        {
          keyValuePair = guide;
          num1 = num2;
        }
      }
      if ((double) num1 <= (double) snapDistance)
      {
        value = keyValuePair.Key;
        this.currentGuides = keyValuePair.Value;
      }
      else
        this.currentGuides = (List<SnapGuide>) null;
      return value;
    }

    public void OnGUI()
    {
      if (Event.current.type != EventType.MouseUp)
        return;
      this.currentGuides = (List<SnapGuide>) null;
    }

    public void DrawGuides()
    {
      if (this.currentGuides == null)
        return;
      foreach (SnapGuide currentGuide in this.currentGuides)
        currentGuide.Draw();
    }
  }
}
