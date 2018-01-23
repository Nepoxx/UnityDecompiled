// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ChartSeriesViewData
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditorInternal
{
  internal class ChartSeriesViewData
  {
    public bool enabled;

    public ChartSeriesViewData(string name, int numDataPoints, Color color)
    {
      this.name = name;
      this.color = color;
      this.numDataPoints = numDataPoints;
      this.xValues = new float[numDataPoints];
      this.yValues = new float[numDataPoints];
      this.enabled = true;
    }

    public string name { get; private set; }

    public Color color { get; private set; }

    public float[] xValues { get; private set; }

    public float[] yValues { get; private set; }

    public Vector2 rangeAxis { get; set; }

    public int numDataPoints { get; private set; }
  }
}
