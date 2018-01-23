// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProfilerChart
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class ProfilerChart : Chart
  {
    private static readonly GUIContent performanceWarning = new GUIContent("", (Texture) EditorGUIUtility.LoadIcon("console.warnicon.sml"), "Collecting GPU Profiler data might have overhead. Close graph if you don't need its data");
    private static string[] s_LocalizedChartNames = (string[]) null;
    private const string kPrefCharts = "ProfilerChart";
    private bool m_Active;
    public ProfilerArea m_Area;
    public Chart.ChartType m_Type;
    public float m_DataScale;
    public ChartViewData m_Data;
    public ChartSeriesViewData[] m_Series;

    public ProfilerChart(ProfilerArea area, Chart.ChartType type, float dataScale, int seriesCount)
    {
      this.labelRange = new Vector2(Mathf.Epsilon, float.PositiveInfinity);
      this.m_Area = area;
      this.m_Type = type;
      this.m_DataScale = dataScale;
      this.m_Data = new ChartViewData();
      this.m_Series = new ChartSeriesViewData[seriesCount];
      this.m_Active = this.ReadActiveState();
      this.ApplyActiveState();
    }

    public bool active
    {
      get
      {
        return this.m_Active;
      }
      set
      {
        if (this.m_Active == value)
          return;
        this.m_Active = value;
        this.ApplyActiveState();
        this.SaveActiveState();
      }
    }

    private string GetLocalizedChartName()
    {
      if (ProfilerChart.s_LocalizedChartNames == null)
        ProfilerChart.s_LocalizedChartNames = new string[13]
        {
          LocalizationDatabase.GetLocalizedString("CPU Usage|Graph out the various CPU areas"),
          LocalizationDatabase.GetLocalizedString("GPU Usage|Graph out the various GPU areas"),
          LocalizationDatabase.GetLocalizedString("Rendering"),
          LocalizationDatabase.GetLocalizedString("Memory|Graph out the various memory usage areas"),
          LocalizationDatabase.GetLocalizedString("Audio"),
          LocalizationDatabase.GetLocalizedString("Video"),
          LocalizationDatabase.GetLocalizedString("Physics"),
          LocalizationDatabase.GetLocalizedString("Physics (2D)"),
          LocalizationDatabase.GetLocalizedString("Network Messages"),
          LocalizationDatabase.GetLocalizedString("Network Operations"),
          LocalizationDatabase.GetLocalizedString("UI"),
          LocalizationDatabase.GetLocalizedString("UI Details"),
          LocalizationDatabase.GetLocalizedString("Global Illumination|Graph of the Precomputed Realtime Global Illumination system resource usage.")
        };
      return ProfilerChart.s_LocalizedChartNames[(int) this.m_Area];
    }

    protected override void DoLegendGUI(Rect position, Chart.ChartType type, ChartViewData cdata, EventType evtType, bool active)
    {
      Rect position1 = position;
      position1.xMin = position1.xMax - (float) ProfilerChart.performanceWarning.image.width;
      position1.yMin = position1.yMax - (float) ProfilerChart.performanceWarning.image.height;
      base.DoLegendGUI(position, type, cdata, evtType, active);
      if (this.m_Area != ProfilerArea.GPU)
        return;
      GUI.Label(position1, ProfilerChart.performanceWarning);
    }

    public virtual int DoChartGUI(int currentFrame, ProfilerArea currentArea)
    {
      if (Event.current.type == EventType.Repaint)
      {
        string[] selectedLabels = new string[this.m_Series.Length];
        for (int index = 0; index < this.m_Series.Length; ++index)
        {
          int statisticsIdentifier = ProfilerDriver.GetStatisticsIdentifier(!this.m_Data.hasOverlay ? this.m_Series[index].name : "Selected" + this.m_Series[index].name);
          selectedLabels[index] = ProfilerDriver.GetFormattedStatisticsValue(currentFrame, statisticsIdentifier);
        }
        this.m_Data.AssignSelectedLabels(selectedLabels);
      }
      if (this.legendHeaderLabel == null)
        this.legendHeaderLabel = EditorGUIUtility.TextContentWithIcon(this.GetLocalizedChartName(), string.Format("Profiler.{0}", (object) Enum.GetName(typeof (ProfilerArea), (object) this.m_Area)));
      return this.DoGUI(this.m_Type, currentFrame, this.m_Data, currentArea == this.m_Area);
    }

    public void LoadAndBindSettings()
    {
      this.LoadAndBindSettings(nameof (ProfilerChart) + (object) this.m_Area, this.m_Data);
    }

    private void ApplyActiveState()
    {
      if (this.m_Area != ProfilerArea.GPU)
        return;
      ProfilerDriver.profileGPU = this.active;
    }

    private bool ReadActiveState()
    {
      if (this.m_Area == ProfilerArea.GPU)
        return SessionState.GetBool(nameof (ProfilerChart) + (object) this.m_Area, false);
      return EditorPrefs.GetBool(nameof (ProfilerChart) + (object) this.m_Area, true);
    }

    private void SaveActiveState()
    {
      if (this.m_Area == ProfilerArea.GPU)
        SessionState.SetBool(nameof (ProfilerChart) + (object) this.m_Area, this.m_Active);
      else
        EditorPrefs.SetBool(nameof (ProfilerChart) + (object) this.m_Area, this.m_Active);
    }
  }
}
