// Decompiled with JetBrains decompiler
// Type: UnityEditor.UISystemProfilerChart
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class UISystemProfilerChart : ProfilerChart
  {
    public bool showMarkers = true;
    private EventMarker[] m_Markers;
    private string[] m_MarkerNames;

    public UISystemProfilerChart(Chart.ChartType type, float dataScale, int seriesCount)
      : base(ProfilerArea.UIDetails, type, dataScale, seriesCount)
    {
    }

    public void Update(int firstFrame, int historyLength)
    {
      int eventMarkersCount = ProfilerDriver.GetUISystemEventMarkersCount(firstFrame, historyLength);
      if (eventMarkersCount == 0)
        return;
      this.m_Markers = new EventMarker[eventMarkersCount];
      this.m_MarkerNames = new string[eventMarkersCount];
      ProfilerDriver.GetUISystemEventMarkersBatch(firstFrame, historyLength, this.m_Markers, this.m_MarkerNames);
    }

    public override int DoChartGUI(int currentFrame, ProfilerArea currentArea)
    {
      int num = base.DoChartGUI(currentFrame, currentArea);
      if (this.m_Markers != null && this.showMarkers)
      {
        Rect lastRect = GUILayoutUtility.GetLastRect();
        lastRect.xMin += 180f;
        for (int index = 0; index < this.m_Markers.Length; ++index)
        {
          EventMarker marker = this.m_Markers[index];
          Color currentColor = ProfilerColors.currentColors[(long) (uint) this.m_Series.Length % (long) ProfilerColors.currentColors.Length];
          Chart.DrawVerticalLine(marker.frame, this.m_Data, lastRect, currentColor.AlphaMultiplied(0.3f), currentColor.AlphaMultiplied(0.4f), 1f);
        }
        this.DrawMarkerLabels(this.m_Data, lastRect, this.m_Markers, this.m_MarkerNames);
      }
      return num;
    }

    private void DrawMarkerLabels(ChartViewData cdata, Rect r, EventMarker[] markers, string[] markerNames)
    {
      Color contentColor = GUI.contentColor;
      Vector2 dataDomain = cdata.GetDataDomain();
      int num1 = (int) ((double) dataDomain.y - (double) dataDomain.x);
      float num2 = r.width / (float) num1;
      int num3 = (int) ((double) r.height / 12.0);
      if (num3 != 0)
      {
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        for (int index = 0; index < markers.Length; ++index)
        {
          int frame = markers[index].frame;
          int num4;
          if (!dictionary.TryGetValue(markers[index].nameOffset, out num4) || num4 != frame - 1 || num4 < cdata.chartDomainOffset)
          {
            int num5 = frame - cdata.chartDomainOffset;
            if (num5 >= 0)
            {
              float num6 = r.x + num2 * (float) num5;
              GUI.contentColor = (ProfilerColors.currentColors[(long) (uint) this.m_Series.Length % (long) ProfilerColors.currentColors.Length] + Color.white) * 0.5f;
              Chart.DoLabel(num6 - 1f, r.y + r.height - (float) ((index % num3 + 1) * 12), markerNames[index], 0.0f);
            }
          }
          dictionary[markers[index].nameOffset] = markers[index].frame;
        }
      }
      GUI.contentColor = contentColor;
    }

    protected override Rect DoSeriesList(Rect position, int chartControlID, Chart.ChartType chartType, ChartViewData cdata)
    {
      Rect position1 = base.DoSeriesList(position, chartControlID, chartType, cdata);
      GUIContent label = EditorGUIUtility.TempContent("Markers");
      Color currentColor = ProfilerColors.currentColors[cdata.numSeries % ProfilerColors.currentColors.Length];
      this.DoSeriesToggle(position1, label, ref this.showMarkers, currentColor, cdata);
      position1.y += position1.height;
      return position1;
    }
  }
}
