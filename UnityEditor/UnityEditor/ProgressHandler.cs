// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProgressHandler
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ProgressHandler
  {
    private ProgressHandler.ProgressCallback m_ProgressCallback;
    private string m_Title;
    private float m_ProgressRangeMin;
    private float m_ProgressRangeMax;

    public ProgressHandler(string title, ProgressHandler.ProgressCallback callback, float progressRangeMin = 0.0f, float progressRangeMax = 1f)
    {
      this.m_Title = title;
      this.m_ProgressCallback += callback;
      this.m_ProgressRangeMin = progressRangeMin;
      this.m_ProgressRangeMax = progressRangeMax;
    }

    private float CalcGlobalProcess(float localProcess)
    {
      return Mathf.Clamp((float) ((double) this.m_ProgressRangeMin * (1.0 - (double) localProcess) + (double) this.m_ProgressRangeMax * (double) localProcess), 0.0f, 1f);
    }

    public void OnProgress(string message, float progress)
    {
      this.m_ProgressCallback(this.m_Title, message, this.CalcGlobalProcess(progress));
    }

    public ProgressHandler SpawnFromLocalSubRange(float localRangeMin, float localRangeMax)
    {
      return new ProgressHandler(this.m_Title, this.m_ProgressCallback, this.CalcGlobalProcess(localRangeMin), this.CalcGlobalProcess(localRangeMax));
    }

    public delegate void ProgressCallback(string title, string message, float globalProgress);
  }
}
