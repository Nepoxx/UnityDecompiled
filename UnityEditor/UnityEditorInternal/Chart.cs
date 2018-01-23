// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Chart
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class Chart
  {
    private static int s_ChartHash = "Charts".GetHashCode();
    private static readonly Color s_OverlayBackgroundDimFactor = new Color(0.9f, 0.9f, 0.9f, 0.4f);
    private int m_DragItemIndex = -1;
    public string m_NotSupportedWarning = (string) null;
    private readonly List<Chart.LabelLayoutData> m_LabelData = new List<Chart.LabelLayoutData>(16);
    private readonly List<int> m_LabelOrder = new List<int>(16);
    private readonly List<int> m_MostOverlappingLabels = new List<int>(16);
    private readonly List<int> m_OverlappingLabels = new List<int>(16);
    private readonly List<float> m_SelectedFrameValues = new List<float>(16);
    public const float kSideWidth = 180f;
    private const int kDistFromTopToFirstLabel = 38;
    private const int kLabelHeight = 11;
    private const int kCloseButtonSize = 13;
    private const float kLabelOffset = 5f;
    private const float kWarningLabelHeightOffset = 43f;
    private const float kChartMinHeight = 130f;
    private const float k_LineWidth = 2f;
    private const int k_LabelLayoutMaxIterations = 5;
    private Vector3[] m_LineDrawingPoints;
    private float[] m_StackedSampleSums;
    private string m_ChartSettingsName;
    private int m_chartControlID;
    private Vector2 m_DragDownPos;
    private int[] m_OldChartOrder;

    public Chart()
    {
      this.labelRange = new Vector2(float.NegativeInfinity, float.PositiveInfinity);
    }

    public void LoadAndBindSettings(string chartSettingsName, ChartViewData cdata)
    {
      this.m_ChartSettingsName = chartSettingsName;
      this.LoadChartsSettings(cdata);
    }

    public event Chart.ChangedEventHandler closed;

    public event Chart.ChangedEventHandler selected;

    public GUIContent legendHeaderLabel { get; set; }

    public Vector2 labelRange { get; set; }

    private int MoveSelectedFrame(int selectedFrame, ChartViewData cdata, int direction)
    {
      Vector2 dataDomain = cdata.GetDataDomain();
      int num1 = (int) ((double) dataDomain.y - (double) dataDomain.x);
      int num2 = selectedFrame + direction;
      if (num2 < cdata.firstSelectableFrame || num2 > cdata.chartDomainOffset + num1)
        return selectedFrame;
      return num2;
    }

    private int DoFrameSelectionDrag(float x, Rect r, ChartViewData cdata, int len)
    {
      int num = Mathf.RoundToInt((float) (((double) x - (double) r.x) / (double) r.width * (double) len - 0.5));
      GUI.changed = true;
      return Mathf.Clamp(num + cdata.chartDomainOffset, cdata.firstSelectableFrame, cdata.chartDomainOffset + len);
    }

    private int HandleFrameSelectionEvents(int selectedFrame, int chartControlID, Rect chartFrame, ChartViewData cdata)
    {
      Event current = Event.current;
      switch (current.type)
      {
        case EventType.MouseDown:
          if (chartFrame.Contains(current.mousePosition))
          {
            GUIUtility.keyboardControl = chartControlID;
            GUIUtility.hotControl = chartControlID;
            Vector2 dataDomain = cdata.GetDataDomain();
            int len = (int) ((double) dataDomain.y - (double) dataDomain.x);
            selectedFrame = this.DoFrameSelectionDrag(current.mousePosition.x, chartFrame, cdata, len);
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == chartControlID)
          {
            GUIUtility.hotControl = 0;
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == chartControlID)
          {
            Vector2 dataDomain = cdata.GetDataDomain();
            int len = (int) ((double) dataDomain.y - (double) dataDomain.x);
            selectedFrame = this.DoFrameSelectionDrag(current.mousePosition.x, chartFrame, cdata, len);
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.keyboardControl == chartControlID && selectedFrame >= 0)
          {
            if (current.keyCode == KeyCode.LeftArrow)
            {
              selectedFrame = this.MoveSelectedFrame(selectedFrame, cdata, -1);
              current.Use();
              break;
            }
            if (current.keyCode == KeyCode.RightArrow)
            {
              selectedFrame = this.MoveSelectedFrame(selectedFrame, cdata, 1);
              current.Use();
              break;
            }
            break;
          }
          break;
      }
      return selectedFrame;
    }

    public void OnLostFocus()
    {
      if (GUIUtility.hotControl == this.m_chartControlID)
        GUIUtility.hotControl = 0;
      this.m_chartControlID = 0;
    }

    protected virtual void DoLegendGUI(Rect position, Chart.ChartType type, ChartViewData cdata, EventType evtType, bool active)
    {
      if (Event.current.type == EventType.Repaint)
        Chart.Styles.legendBackground.Draw(position, GUIContent.none, false, false, active, false);
      Rect position1 = position;
      GUIContent content = this.legendHeaderLabel ?? GUIContent.none;
      position1.height = Chart.Styles.legendHeaderLabel.CalcSize(content).y;
      GUI.Label(position1, content, Chart.Styles.legendHeaderLabel);
      position.yMin += position1.height;
      position.xMin += 5f;
      position.xMax -= 5f;
      this.DoSeriesList(position, this.m_chartControlID, type, cdata);
      Rect position2 = position1;
      position2.xMax -= (float) Chart.Styles.legendHeaderLabel.padding.right;
      position2.xMin = position2.xMax - 13f;
      position2.yMin += (float) Chart.Styles.legendHeaderLabel.padding.top;
      position2.yMax = position2.yMin + 13f;
      // ISSUE: reference to a compiler-generated field
      if (!GUI.Button(position2, GUIContent.none, Chart.Styles.closeButton) || this.closed == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.closed(this);
    }

    public int DoGUI(Chart.ChartType type, int selectedFrame, ChartViewData cdata, bool active)
    {
      if (cdata == null)
        return selectedFrame;
      this.m_chartControlID = GUIUtility.GetControlID(Chart.s_ChartHash, FocusType.Keyboard);
      GUILayoutOption guiLayoutOption = GUILayout.MinHeight(Math.Max((float) (5.0 + (double) ((cdata.numSeries + 1) * 11) + 38.0), 130f));
      Rect rect1 = GUILayoutUtility.GetRect(GUIContent.none, Chart.Styles.background, new GUILayoutOption[1]{ guiLayoutOption });
      Rect rect2 = rect1;
      rect2.x += 180f;
      rect2.width -= 180f;
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(this.m_chartControlID);
      // ISSUE: reference to a compiler-generated field
      if (typeForControl == EventType.MouseDown && rect1.Contains(current.mousePosition) && this.selected != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.selected(this);
      }
      if (this.m_DragItemIndex == -1)
        selectedFrame = this.HandleFrameSelectionEvents(selectedFrame, this.m_chartControlID, rect2, cdata);
      Rect position1 = rect2;
      position1.x -= 180f;
      position1.width = 180f;
      this.DoLegendGUI(position1, type, cdata, typeForControl, active);
      if (current.type == EventType.Repaint)
      {
        Chart.Styles.rightPane.Draw(rect2, false, false, active, false);
        if (this.m_NotSupportedWarning == null)
        {
          --rect2.height;
          if (type == Chart.ChartType.StackedFill)
            this.DrawChartStacked(selectedFrame, cdata, rect2);
          else
            this.DrawChartLine(selectedFrame, cdata, rect2);
        }
        else
        {
          Rect position2 = rect2;
          position2.x += 59.4f;
          position2.y += 43f;
          GUI.Label(position2, this.m_NotSupportedWarning, EditorStyles.boldLabel);
        }
      }
      return selectedFrame;
    }

    private void DrawSelectedFrame(int selectedFrame, ChartViewData cdata, Rect r)
    {
      if (cdata.firstSelectableFrame == -1 || selectedFrame - cdata.firstSelectableFrame < 0)
        return;
      Chart.DrawVerticalLine(selectedFrame, cdata, r, Chart.Styles.selectedFrameColor1, Chart.Styles.selectedFrameColor2, 1f);
    }

    internal static void DrawVerticalLine(int frame, ChartViewData cdata, Rect r, Color color1, Color color2, float widthFactor)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      frame -= cdata.chartDomainOffset;
      if (frame < 0)
        return;
      Vector2 dataDomain = cdata.GetDataDomain();
      float num = dataDomain.y - dataDomain.x;
      HandleUtility.ApplyWireMaterial();
      GL.Begin(7);
      GL.Color(color1);
      GL.Vertex3(r.x + r.width / num * (float) frame, r.y + 1f, 0.0f);
      GL.Vertex3((float) ((double) r.x + (double) r.width / (double) num * (double) frame + (double) r.width / (double) num), r.y + 1f, 0.0f);
      GL.Color(color2);
      GL.Vertex3((float) ((double) r.x + (double) r.width / (double) num * (double) frame + (double) r.width / (double) num), r.yMax, 0.0f);
      GL.Vertex3(r.x + r.width / num * (float) frame, r.yMax, 0.0f);
      GL.End();
    }

    private void DrawMaxValueScale(ChartViewData cdata, Rect r)
    {
      Handles.Label(new Vector3((float) ((double) r.x + (double) r.width / 2.0 - 20.0), r.yMin + 2f, 0.0f), "Scale: " + (object) cdata.maxValue);
    }

    private void DrawChartLine(int selectedFrame, ChartViewData cdata, Rect r)
    {
      for (int index = 0; index < cdata.numSeries; ++index)
        this.DrawChartItemLine(r, cdata, index);
      if ((double) cdata.maxValue > 0.0)
        this.DrawMaxValueScale(cdata, r);
      this.DrawSelectedFrame(selectedFrame, cdata, r);
      this.DrawLabels(r, cdata, selectedFrame, Chart.ChartType.Line);
    }

    private void DrawChartStacked(int selectedFrame, ChartViewData cdata, Rect r)
    {
      HandleUtility.ApplyWireMaterial();
      Vector2 dataDomain = cdata.GetDataDomain();
      int length = (int) ((double) dataDomain.y - (double) dataDomain.x);
      if (length <= 0)
        return;
      if (this.m_StackedSampleSums == null || this.m_StackedSampleSums.Length < length)
        this.m_StackedSampleSums = new float[length];
      for (int index = 0; index < length; ++index)
        this.m_StackedSampleSums[index] = 0.0f;
      for (int index = 0; index < cdata.numSeries; ++index)
      {
        if (cdata.hasOverlay)
          this.DrawChartItemStackedOverlay(r, index, cdata, this.m_StackedSampleSums);
        this.DrawChartItemStacked(r, index, cdata, this.m_StackedSampleSums);
      }
      this.DrawSelectedFrame(selectedFrame, cdata, r);
      this.DrawGridStacked(r, cdata);
      this.DrawLabels(r, cdata, selectedFrame, Chart.ChartType.StackedFill);
      if (!cdata.hasOverlay)
        return;
      string str = ProfilerDriver.selectedPropertyPath;
      if (str.Length <= 0)
        return;
      int num = str.LastIndexOf('/');
      if (num != -1)
        str = str.Substring(num + 1);
      GUIContent content = EditorGUIUtility.TempContent("Selected: " + str);
      Vector2 vector2 = EditorStyles.whiteBoldLabel.CalcSize(content);
      EditorGUI.DropShadowLabel(new Rect((float) ((double) r.x + (double) r.width - (double) vector2.x - 3.0), r.y + 3f, vector2.x, vector2.y), content, Chart.Styles.selectedLabel);
    }

    internal static void DoLabel(float x, float y, string text, float alignment)
    {
      if (string.IsNullOrEmpty(text))
        return;
      GUIContent content = EditorGUIUtility.TempContent(text);
      Vector2 vector2 = Chart.Styles.whiteLabel.CalcSize(content);
      EditorGUI.DoDropShadowLabel(new Rect(x + vector2.x * alignment, y, vector2.x, vector2.y), content, Chart.Styles.whiteLabel, Chart.Styles.labelDropShadowOpacity);
    }

    private void DrawGridStacked(Rect r, ChartViewData cdata)
    {
      if (Event.current.type != EventType.Repaint || cdata.grid == null || cdata.gridLabels == null)
        return;
      GL.Begin(1);
      GL.Color(new Color(1f, 1f, 1f, 0.2f));
      float num1 = (double) cdata.series[0].rangeAxis.sqrMagnitude != 0.0 ? (float) (1.0 / ((double) cdata.series[0].rangeAxis.y - (double) cdata.series[0].rangeAxis.x)) * r.height : 0.0f;
      float num2 = r.y + r.height;
      for (int index = 0; index < cdata.grid.Length; ++index)
      {
        float y = num2 - (cdata.grid[index] - cdata.series[0].rangeAxis.x) * num1;
        if ((double) y > (double) r.y)
        {
          GL.Vertex3(r.x + 80f, y, 0.0f);
          GL.Vertex3(r.x + r.width, y, 0.0f);
        }
      }
      GL.End();
      for (int index = 0; index < cdata.grid.Length; ++index)
      {
        float num3 = num2 - (cdata.grid[index] - cdata.series[0].rangeAxis.x) * num1;
        if ((double) num3 > (double) r.y)
          Chart.DoLabel(r.x + 5f, num3 - 8f, cdata.gridLabels[index], 0.0f);
      }
    }

    private void DrawLabels(Rect chartPosition, ChartViewData data, int selectedFrame, Chart.ChartType chartType)
    {
      if (data.selectedLabels == null || Event.current.type != EventType.Repaint)
        return;
      Vector2 dataDomain = data.GetDataDomain();
      if (selectedFrame < data.firstSelectableFrame || selectedFrame > data.chartDomainOffset + (int) ((double) dataDomain.y - (double) dataDomain.x) || (double) dataDomain.y - (double) dataDomain.x == 0.0)
        return;
      int index1 = selectedFrame - data.chartDomainOffset;
      this.m_LabelOrder.Clear();
      this.m_LabelOrder.AddRange((IEnumerable<int>) data.order);
      this.m_SelectedFrameValues.Clear();
      float num1 = 0.0f;
      bool flag1 = chartType == Chart.ChartType.StackedFill;
      int num2 = 0;
      int index2 = 0;
      for (int numSeries = data.numSeries; index2 < numSeries; ++index2)
      {
        float yValue = data.series[index2].yValues[index1];
        this.m_SelectedFrameValues.Add(yValue);
        if (data.series[index2].enabled)
        {
          num1 += yValue;
          ++num2;
        }
      }
      if (num2 == 0)
        return;
      this.m_LabelData.Clear();
      float num3 = chartPosition.x + chartPosition.width * (float) (((double) index1 + 0.5) / ((double) dataDomain.y - (double) dataDomain.x));
      float a = 0.0f;
      int num4 = 0;
      int index3 = 0;
      for (int numSeries = data.numSeries; index3 < numSeries; ++index3)
      {
        Chart.LabelLayoutData labelLayoutData = new Chart.LabelLayoutData();
        float num5 = this.m_SelectedFrameValues[index3];
        if (data.series[index3].enabled && (double) num5 >= (double) this.labelRange.x && (double) num5 <= (double) this.labelRange.y)
        {
          Vector2 rangeAxis = data.series[index3].rangeAxis;
          float num6 = (double) rangeAxis.sqrMagnitude != 0.0 ? rangeAxis.y - rangeAxis.x : 1f;
          if (flag1)
          {
            float num7 = 0.0f;
            for (int index4 = 0; index4 < numSeries; ++index4)
            {
              int index5 = data.order[index4];
              if (index5 < index3 && data.series[index5].enabled)
                num7 += data.series[index5].yValues[index1];
            }
            num5 = (float) ((double) num1 - (double) num7 - 0.5 * (double) num5);
          }
          Vector2 position = new Vector2(num3 + 0.5f, chartPosition.y + chartPosition.height * (float) (1.0 - ((double) num5 - (double) rangeAxis.x) / (double) num6));
          Vector2 size = Chart.Styles.whiteLabel.CalcSize(EditorGUIUtility.TempContent(data.selectedLabels[index3]));
          position.y -= 0.5f * size.y;
          position.y = Mathf.Clamp(position.y, chartPosition.yMin, chartPosition.yMax - size.y);
          labelLayoutData.position = new Rect(position, size);
          labelLayoutData.desiredYPosition = labelLayoutData.position.center.y;
          ++num4;
        }
        this.m_LabelData.Add(labelLayoutData);
        a = Mathf.Max(a, labelLayoutData.position.width);
      }
      if (num4 == 0)
        return;
      if (!flag1)
        this.m_LabelOrder.Sort(new Comparison<int>(this.SortLineLabelIndices));
      if ((double) num3 > (double) chartPosition.x + (double) chartPosition.width - (double) a)
      {
        int index4 = 0;
        for (int numSeries = data.numSeries; index4 < numSeries; ++index4)
        {
          Chart.LabelLayoutData labelLayoutData = this.m_LabelData[index4];
          labelLayoutData.position.x -= labelLayoutData.position.width;
          this.m_LabelData[index4] = labelLayoutData;
        }
      }
      else if ((double) num3 > (double) chartPosition.x + (double) a)
      {
        int num5 = 0;
        int index4 = 0;
        for (int numSeries = data.numSeries; index4 < numSeries; ++index4)
        {
          int index5 = this.m_LabelOrder[index4];
          if ((double) this.m_LabelData[index5].position.size.sqrMagnitude != 0.0)
          {
            if ((num5 & 1) == 0)
            {
              Chart.LabelLayoutData labelLayoutData = this.m_LabelData[index5];
              labelLayoutData.position.x -= labelLayoutData.position.width + 1f;
              this.m_LabelData[index5] = labelLayoutData;
            }
            ++num5;
          }
        }
      }
      for (int index4 = 0; index4 < 5; ++index4)
      {
        this.m_MostOverlappingLabels.Clear();
        int index5 = 0;
        for (int numSeries = data.numSeries; index5 < numSeries; ++index5)
        {
          this.m_OverlappingLabels.Clear();
          this.m_OverlappingLabels.Add(index5);
          if ((double) this.m_LabelData[index5].position.size.sqrMagnitude != 0.0)
          {
            for (int index6 = 0; index6 < numSeries; ++index6)
            {
              if ((double) this.m_LabelData[index6].position.size.sqrMagnitude != 0.0 && index5 != index6 && this.m_LabelData[index5].position.Overlaps(this.m_LabelData[index6].position))
                this.m_OverlappingLabels.Add(index6);
            }
            if (this.m_OverlappingLabels.Count > this.m_MostOverlappingLabels.Count)
            {
              this.m_MostOverlappingLabels.Clear();
              this.m_MostOverlappingLabels.AddRange((IEnumerable<int>) this.m_OverlappingLabels);
            }
          }
        }
        if (this.m_MostOverlappingLabels.Count != 1)
        {
          float totalHeight;
          float num5 = this.GetGeometricCenter(this.m_MostOverlappingLabels, this.m_LabelData, out totalHeight);
          bool flag2 = true;
          while (flag2)
          {
            flag2 = false;
            float y1 = num5 - 0.5f * totalHeight;
            float y2 = num5 + 0.5f * totalHeight;
            int index6 = 0;
            for (int numSeries = data.numSeries; index6 < numSeries; ++index6)
            {
              if (!this.m_MostOverlappingLabels.Contains(index6))
              {
                Rect position = this.m_LabelData[index6].position;
                if ((double) position.size.sqrMagnitude != 0.0)
                {
                  float x = (double) position.xMax >= (double) num3 ? position.xMin : position.xMax;
                  if (position.Contains(new Vector2(x, y1)) || position.Contains(new Vector2(x, y2)))
                  {
                    this.m_MostOverlappingLabels.Add(index6);
                    flag2 = true;
                  }
                }
              }
            }
            double geometricCenter = (double) this.GetGeometricCenter(this.m_MostOverlappingLabels, this.m_LabelData, out totalHeight);
            if ((double) num5 - 0.5 * (double) totalHeight < (double) chartPosition.yMin)
              num5 = chartPosition.yMin + 0.5f * totalHeight;
            else if ((double) num5 + 0.5 * (double) totalHeight > (double) chartPosition.yMax)
              num5 = chartPosition.yMax - 0.5f * totalHeight;
          }
          this.m_MostOverlappingLabels.Sort(new Comparison<int>(this.SortOverlappingRectIndices));
          float num6 = 0.0f;
          int index7 = 0;
          for (int count = this.m_MostOverlappingLabels.Count; index7 < count; ++index7)
          {
            int overlappingLabel = this.m_MostOverlappingLabels[index7];
            Chart.LabelLayoutData labelLayoutData = this.m_LabelData[overlappingLabel];
            labelLayoutData.position.y = num5 - totalHeight * 0.5f + num6;
            this.m_LabelData[overlappingLabel] = labelLayoutData;
            num6 += labelLayoutData.position.height;
          }
        }
        else
          break;
      }
      Color contentColor = GUI.contentColor;
      for (int index4 = 0; index4 < data.numSeries; ++index4)
      {
        int index5 = this.m_LabelOrder[index4];
        if ((double) this.m_LabelData[index5].position.size.sqrMagnitude != 0.0)
        {
          GUI.contentColor = Color.Lerp(data.series[index5].color, Color.white, Chart.Styles.labelLerpToWhiteAmount);
          EditorGUI.DoDropShadowLabel(this.m_LabelData[index5].position, EditorGUIUtility.TempContent(data.selectedLabels[index5]), Chart.Styles.whiteLabel, Chart.Styles.labelDropShadowOpacity);
        }
      }
      GUI.contentColor = contentColor;
    }

    private int SortLineLabelIndices(int index1, int index2)
    {
      return -this.m_LabelData[index1].desiredYPosition.CompareTo(this.m_LabelData[index2].desiredYPosition);
    }

    private int SortOverlappingRectIndices(int index1, int index2)
    {
      return -this.m_LabelOrder.IndexOf(index1).CompareTo(this.m_LabelOrder.IndexOf(index2));
    }

    private float GetGeometricCenter(List<int> overlappingRects, List<Chart.LabelLayoutData> labelData, out float totalHeight)
    {
      float num = 0.0f;
      totalHeight = 0.0f;
      int index = 0;
      for (int count = overlappingRects.Count; index < count; ++index)
      {
        int overlappingRect = overlappingRects[index];
        num += labelData[overlappingRect].desiredYPosition;
        totalHeight += labelData[overlappingRect].position.height;
      }
      return num / (float) overlappingRects.Count;
    }

    private void DrawChartItemLine(Rect r, ChartViewData cdata, int index)
    {
      ChartSeriesViewData chartSeriesViewData = cdata.series[index];
      if (!chartSeriesViewData.enabled)
        return;
      if (this.m_LineDrawingPoints == null || chartSeriesViewData.numDataPoints > this.m_LineDrawingPoints.Length)
        this.m_LineDrawingPoints = new Vector3[chartSeriesViewData.numDataPoints];
      Vector2 dataDomain = cdata.GetDataDomain();
      float num1 = dataDomain.y - dataDomain.x;
      if ((double) num1 <= 0.0)
        return;
      float num2 = 1f / num1 * r.width;
      float num3 = (double) cdata.series[index].rangeAxis.sqrMagnitude != 0.0 ? (float) (1.0 / ((double) cdata.series[index].rangeAxis.y - (double) cdata.series[index].rangeAxis.x)) * r.height : 0.0f;
      float num4 = r.y + r.height;
      for (int index1 = 0; index1 < chartSeriesViewData.numDataPoints; ++index1)
        this.m_LineDrawingPoints[index1].Set((chartSeriesViewData.xValues[index1] - dataDomain.x) * num2 + r.x, num4 - (chartSeriesViewData.yValues[index1] - chartSeriesViewData.rangeAxis.x) * num3, 0.0f);
      using (new Handles.DrawingScope(cdata.series[index].color))
        Handles.DrawAAPolyLine(2f, chartSeriesViewData.numDataPoints, this.m_LineDrawingPoints);
    }

    private void DrawChartItemStacked(Rect r, int index, ChartViewData cdata, float[] stackedSampleSums)
    {
      Vector2 dataDomain = cdata.GetDataDomain();
      int num1 = (int) ((double) dataDomain.y - (double) dataDomain.x);
      float num2 = r.width / (float) num1;
      index = cdata.order[index];
      if (!cdata.series[index].enabled)
        return;
      Color color = cdata.series[index].color;
      if (cdata.hasOverlay)
        color *= Chart.s_OverlayBackgroundDimFactor;
      GL.Begin(5);
      float x = r.x + num2 * 0.5f;
      float num3 = (double) cdata.series[0].rangeAxis.sqrMagnitude != 0.0 ? (float) (1.0 / ((double) cdata.series[0].rangeAxis.y - (double) cdata.series[0].rangeAxis.x)) * r.height : 0.0f;
      float num4 = r.y + r.height;
      int index1 = 0;
      while (index1 < num1)
      {
        float y = num4 - stackedSampleSums[index1];
        float yValue = cdata.series[index].yValues[index1];
        if ((double) yValue != -1.0)
        {
          float num5 = (yValue - cdata.series[0].rangeAxis.x) * num3;
          if ((double) y - (double) num5 < (double) r.yMin)
            num5 = y - r.yMin;
          GL.Color(color);
          GL.Vertex3(x, y - num5, 0.0f);
          GL.Vertex3(x, y, 0.0f);
          stackedSampleSums[index1] += num5;
        }
        ++index1;
        x += num2;
      }
      GL.End();
    }

    private void DrawChartItemStackedOverlay(Rect r, int index, ChartViewData cdata, float[] stackedSampleSums)
    {
      Vector2 dataDomain = cdata.GetDataDomain();
      int num1 = (int) ((double) dataDomain.y - (double) dataDomain.x);
      float num2 = r.width / (float) num1;
      int index1 = cdata.order[index];
      if (!cdata.series[index1].enabled)
        return;
      Color color = cdata.series[index1].color;
      GL.Begin(5);
      float x = r.x + num2 * 0.5f;
      float num3 = (double) cdata.series[0].rangeAxis.sqrMagnitude != 0.0 ? (float) (1.0 / ((double) cdata.series[0].rangeAxis.y - (double) cdata.series[0].rangeAxis.x)) * r.height : 0.0f;
      float num4 = r.y + r.height;
      int index2 = 0;
      while (index2 < num1)
      {
        float y = num4 - stackedSampleSums[index2];
        float yValue = cdata.overlays[index1].yValues[index2];
        if ((double) yValue != -1.0)
        {
          float num5 = (yValue - cdata.series[0].rangeAxis.x) * num3;
          GL.Color(color);
          GL.Vertex3(x, y - num5, 0.0f);
          GL.Vertex3(x, y, 0.0f);
        }
        ++index2;
        x += num2;
      }
      GL.End();
    }

    protected virtual Rect DoSeriesList(Rect position, int chartControlID, Chart.ChartType chartType, ChartViewData cdata)
    {
      Rect rect = position;
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(chartControlID);
      Vector2 mousePosition = current.mousePosition;
      if (this.m_DragItemIndex != -1)
      {
        if (typeForControl != EventType.MouseUp)
        {
          if (typeForControl == EventType.KeyDown && current.keyCode == KeyCode.Escape)
          {
            GUIUtility.hotControl = 0;
            Array.Copy((Array) this.m_OldChartOrder, (Array) cdata.order, this.m_OldChartOrder.Length);
            this.m_DragItemIndex = -1;
            current.Use();
          }
        }
        else if (GUIUtility.hotControl == chartControlID)
        {
          GUIUtility.hotControl = 0;
          this.m_DragItemIndex = -1;
          current.Use();
        }
      }
      for (int index1 = cdata.numSeries - 1; index1 >= 0; --index1)
      {
        int index2 = cdata.order[index1];
        GUIContent guiContent = EditorGUIUtility.TempContent(cdata.series[index2].name);
        rect.height = Chart.Styles.seriesLabel.CalcHeight(guiContent, rect.width);
        Rect position1 = rect;
        if (index1 == this.m_DragItemIndex)
          position1.y = mousePosition.y - this.m_DragDownPos.y;
        if (chartType == Chart.ChartType.StackedFill)
        {
          Rect position2 = position1;
          position2.xMin = position2.xMax - rect.height;
          switch (typeForControl)
          {
            case EventType.MouseDown:
              if (position2.Contains(mousePosition))
              {
                this.m_DragItemIndex = index1;
                this.m_DragDownPos = mousePosition;
                this.m_DragDownPos.x -= rect.x;
                this.m_DragDownPos.y -= rect.y;
                this.m_OldChartOrder = new int[cdata.numSeries];
                Array.Copy((Array) cdata.order, (Array) this.m_OldChartOrder, this.m_OldChartOrder.Length);
                GUIUtility.hotControl = chartControlID;
                current.Use();
                break;
              }
              break;
            case EventType.MouseUp:
              if (this.m_DragItemIndex == index1)
                current.Use();
              this.m_DragItemIndex = -1;
              break;
            case EventType.MouseDrag:
              if (index1 == this.m_DragItemIndex)
              {
                bool flag1 = (double) mousePosition.y > (double) rect.yMax;
                bool flag2 = (double) mousePosition.y < (double) rect.yMin;
                if (flag1 || flag2)
                {
                  int num = cdata.order[index1];
                  int index3 = !flag2 ? Mathf.Max(0, index1 - 1) : Mathf.Min(cdata.numSeries - 1, index1 + 1);
                  cdata.order[index1] = cdata.order[index3];
                  cdata.order[index3] = num;
                  this.m_DragItemIndex = index3;
                  this.SaveChartsSettingsOrder(cdata);
                }
                current.Use();
                break;
              }
              break;
            case EventType.Repaint:
              Chart.Styles.seriesDragHandle.Draw(position2, false, false, false, false);
              break;
          }
        }
        this.DoSeriesToggle(position1, guiContent, ref cdata.series[index2].enabled, cdata.series[index2].color, cdata);
        rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
      }
      return rect;
    }

    protected void DoSeriesToggle(Rect position, GUIContent label, ref bool enabled, Color color, ChartViewData cdata)
    {
      Color backgroundColor = GUI.backgroundColor;
      GUI.backgroundColor = !enabled ? Color.black : color;
      EditorGUI.BeginChangeCheck();
      enabled = GUI.Toggle(position, enabled, label, Chart.Styles.seriesLabel);
      if (EditorGUI.EndChangeCheck())
        this.SaveChartsSettingsEnabled(cdata);
      GUI.backgroundColor = backgroundColor;
    }

    private void LoadChartsSettings(ChartViewData cdata)
    {
      if (string.IsNullOrEmpty(this.m_ChartSettingsName))
        return;
      string str1 = EditorPrefs.GetString(this.m_ChartSettingsName + "Order");
      if (!string.IsNullOrEmpty(str1))
      {
        try
        {
          string[] strArray = str1.Split(',');
          if (strArray.Length == cdata.numSeries)
          {
            for (int index = 0; index < cdata.numSeries; ++index)
              cdata.order[index] = int.Parse(strArray[index]);
          }
        }
        catch (FormatException ex)
        {
        }
      }
      string str2 = EditorPrefs.GetString(this.m_ChartSettingsName + "Visible");
      for (int index = 0; index < cdata.numSeries; ++index)
      {
        if (index < str2.Length && (int) str2[index] == 48)
          cdata.series[index].enabled = false;
      }
    }

    private void SaveChartsSettingsOrder(ChartViewData cdata)
    {
      if (string.IsNullOrEmpty(this.m_ChartSettingsName))
        return;
      string empty = string.Empty;
      for (int index = 0; index < cdata.numSeries; ++index)
      {
        if (empty.Length != 0)
          empty += ",";
        empty += (string) (object) cdata.order[index];
      }
      EditorPrefs.SetString(this.m_ChartSettingsName + "Order", empty);
    }

    protected void SaveChartsSettingsEnabled(ChartViewData cdata)
    {
      string empty = string.Empty;
      for (int index = 0; index < cdata.numSeries; ++index)
        empty += (string) (object) (char) (!cdata.series[index].enabled ? 48 : 49);
      EditorPrefs.SetString(this.m_ChartSettingsName + "Visible", empty);
    }

    public delegate void ChangedEventHandler(Chart sender);

    internal enum ChartType
    {
      StackedFill,
      Line,
    }

    private static class Styles
    {
      public static readonly GUIStyle background = (GUIStyle) "OL Box";
      public static readonly GUIStyle legendHeaderLabel = EditorStyles.label;
      public static readonly GUIStyle legendBackground = (GUIStyle) "ProfilerLeftPane";
      public static readonly GUIStyle rightPane = (GUIStyle) "ProfilerRightPane";
      public static readonly GUIStyle seriesLabel = (GUIStyle) "ProfilerPaneSubLabel";
      public static readonly GUIStyle seriesDragHandle = (GUIStyle) "RL DragHandle";
      public static readonly GUIStyle closeButton = (GUIStyle) "WinBtnClose";
      public static readonly GUIStyle whiteLabel = (GUIStyle) "ProfilerBadge";
      public static readonly GUIStyle selectedLabel = (GUIStyle) "ProfilerSelectedLabel";
      public static readonly float labelDropShadowOpacity = 0.3f;
      public static readonly float labelLerpToWhiteAmount = 0.5f;
      public static readonly Color selectedFrameColor1 = new Color(1f, 1f, 1f, 0.6f);
      public static readonly Color selectedFrameColor2 = new Color(1f, 1f, 1f, 0.7f);
    }

    private struct LabelLayoutData
    {
      public Rect position;
      public float desiredYPosition;
    }
  }
}
