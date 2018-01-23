// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.IScheduler
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  public interface IScheduler
  {
    IScheduledItem ScheduleOnce(Action<TimerState> timerUpdateEvent, long delayMs);

    IScheduledItem ScheduleUntil(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, Func<bool> stopCondition = null);

    IScheduledItem ScheduleForDuration(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, long durationMs);

    /// <summary>
    ///   <para>Manually unschedules a previously scheduled action.</para>
    /// </summary>
    /// <param name="item">The item to be removed from this scheduler.</param>
    void Unschedule(IScheduledItem item);

    /// <summary>
    ///   <para>Add this item to the list of scheduled tasks.</para>
    /// </summary>
    /// <param name="item">The item to register.</param>
    void Schedule(IScheduledItem item);
  }
}
