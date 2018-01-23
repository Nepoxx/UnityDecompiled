// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.ScheduledItem
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  internal abstract class ScheduledItem : IScheduledItem
  {
    public static readonly Func<bool> OnceCondition = (Func<bool>) (() => true);
    public static readonly Func<bool> ForeverCondition = (Func<bool>) (() => false);
    public Func<bool> timerUpdateStopCondition;

    public ScheduledItem()
    {
      this.ResetStartTime();
      this.timerUpdateStopCondition = ScheduledItem.OnceCondition;
    }

    public long startMs { get; set; }

    public long delayMs { get; set; }

    public long intervalMs { get; set; }

    public long endTimeMs { get; private set; }

    protected void ResetStartTime()
    {
      this.startMs = (long) ((double) Time.realtimeSinceStartup * 1000.0);
    }

    public void SetDuration(long durationMs)
    {
      this.endTimeMs = this.startMs + durationMs;
    }

    public abstract void PerformTimerUpdate(TimerState state);

    internal virtual void OnItemUnscheduled()
    {
    }

    public virtual bool ShouldUnschedule()
    {
      if (this.endTimeMs > 0L && (double) Time.realtimeSinceStartup * 1000.0 > (double) this.endTimeMs)
        return true;
      if (this.timerUpdateStopCondition != null)
        return this.timerUpdateStopCondition();
      return false;
    }
  }
}
