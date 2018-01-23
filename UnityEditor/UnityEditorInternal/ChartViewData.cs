// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ChartViewData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal
{
  internal class ChartViewData
  {
    public ChartSeriesViewData[] series { get; private set; }

    public ChartSeriesViewData[] overlays { get; private set; }

    public int[] order { get; private set; }

    public float[] grid { get; private set; }

    public string[] gridLabels { get; private set; }

    public string[] selectedLabels { get; private set; }

    public int firstSelectableFrame { get; private set; }

    public bool hasOverlay { get; set; }

    public float maxValue { get; set; }

    public int numSeries { get; private set; }

    public int chartDomainOffset { get; private set; }

    public void Assign(ChartSeriesViewData[] series, int firstFrame, int firstSelectableFrame)
    {
      this.series = series;
      this.chartDomainOffset = firstFrame;
      this.firstSelectableFrame = firstSelectableFrame;
      this.numSeries = series.Length;
      if (this.order == null || this.order.Length != this.numSeries)
      {
        this.order = new int[this.numSeries];
        int index = 0;
        for (int length = this.order.Length; index < length; ++index)
          this.order[index] = this.order.Length - 1 - index;
      }
      if (this.overlays != null && this.overlays.Length == this.numSeries)
        return;
      this.overlays = new ChartSeriesViewData[this.numSeries];
    }

    public void AssignSelectedLabels(string[] selectedLabels)
    {
      this.selectedLabels = selectedLabels;
    }

    public void SetGrid(float[] grid, string[] labels)
    {
      this.grid = grid;
      this.gridLabels = labels;
    }

    public Vector2 GetDataDomain()
    {
      if (this.series == null || this.numSeries == 0 || this.series[0].numDataPoints == 0)
        return Vector2.zero;
      Vector2 vector2 = Vector2.one * this.series[0].xValues[0];
      for (int index = 0; index < this.numSeries; ++index)
      {
        if (this.series[index].numDataPoints != 0)
        {
          vector2.x = Mathf.Min(vector2.x, this.series[index].xValues[0]);
          vector2.y = Mathf.Max(vector2.y, this.series[index].xValues[this.series[index].numDataPoints - 1]);
        }
      }
      return vector2;
    }
  }
}
