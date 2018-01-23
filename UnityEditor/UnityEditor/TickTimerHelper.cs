// Decompiled with JetBrains decompiler
// Type: UnityEditor.TickTimerHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  internal class TickTimerHelper
  {
    private double m_NextTick;
    private double m_Interval;

    public TickTimerHelper(double intervalBetweenTicksInSeconds)
    {
      this.m_Interval = intervalBetweenTicksInSeconds;
    }

    public bool DoTick()
    {
      if (EditorApplication.timeSinceStartup <= this.m_NextTick)
        return false;
      this.m_NextTick = EditorApplication.timeSinceStartup + this.m_Interval;
      return true;
    }

    public void Reset()
    {
      this.m_NextTick = 0.0;
    }
  }
}
