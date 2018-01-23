// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.TimerEventScheduler
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
  internal class TimerEventScheduler : IScheduler
  {
    private readonly List<ScheduledItem> m_ScheduledItems = new List<ScheduledItem>();
    private readonly List<ScheduledItem> m_ScheduleTransactions = new List<ScheduledItem>();
    private readonly List<ScheduledItem> m_UnscheduleTransactions = new List<ScheduledItem>();
    private int m_LastUpdatedIndex = -1;
    private bool m_TransactionMode;

    public void Schedule(IScheduledItem item)
    {
      if (item == null)
        return;
      ScheduledItem scheduledItem = item as ScheduledItem;
      if (scheduledItem == null)
        throw new NotSupportedException("Scheduled Item type is not supported by this scheduler");
      if (this.m_TransactionMode)
      {
        this.m_ScheduleTransactions.Add(scheduledItem);
      }
      else
      {
        if (this.m_ScheduledItems.Contains(scheduledItem))
          throw new ArgumentException("Cannot schedule function " + (object) scheduledItem + " more than once");
        this.m_ScheduledItems.Add(scheduledItem);
      }
    }

    public IScheduledItem ScheduleOnce(Action<TimerState> timerUpdateEvent, long delayMs)
    {
      TimerEventScheduler.TimerEventSchedulerItem eventSchedulerItem1 = new TimerEventScheduler.TimerEventSchedulerItem(timerUpdateEvent);
      eventSchedulerItem1.delayMs = delayMs;
      TimerEventScheduler.TimerEventSchedulerItem eventSchedulerItem2 = eventSchedulerItem1;
      this.Schedule((IScheduledItem) eventSchedulerItem2);
      return (IScheduledItem) eventSchedulerItem2;
    }

    public IScheduledItem ScheduleUntil(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, Func<bool> stopCondition)
    {
      TimerEventScheduler.TimerEventSchedulerItem eventSchedulerItem1 = new TimerEventScheduler.TimerEventSchedulerItem(timerUpdateEvent);
      eventSchedulerItem1.delayMs = delayMs;
      eventSchedulerItem1.intervalMs = intervalMs;
      eventSchedulerItem1.timerUpdateStopCondition = stopCondition;
      TimerEventScheduler.TimerEventSchedulerItem eventSchedulerItem2 = eventSchedulerItem1;
      this.Schedule((IScheduledItem) eventSchedulerItem2);
      return (IScheduledItem) eventSchedulerItem2;
    }

    public IScheduledItem ScheduleForDuration(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, long durationMs)
    {
      TimerEventScheduler.TimerEventSchedulerItem eventSchedulerItem1 = new TimerEventScheduler.TimerEventSchedulerItem(timerUpdateEvent);
      eventSchedulerItem1.delayMs = delayMs;
      eventSchedulerItem1.intervalMs = intervalMs;
      eventSchedulerItem1.timerUpdateStopCondition = (Func<bool>) null;
      TimerEventScheduler.TimerEventSchedulerItem eventSchedulerItem2 = eventSchedulerItem1;
      eventSchedulerItem2.SetDuration(durationMs);
      this.Schedule((IScheduledItem) eventSchedulerItem2);
      return (IScheduledItem) eventSchedulerItem2;
    }

    private bool RemovedScheduledItemAt(int index)
    {
      if (index < 0)
        return false;
      ScheduledItem scheduledItem = this.m_ScheduledItems[index];
      this.m_ScheduledItems.RemoveAt(index);
      scheduledItem.OnItemUnscheduled();
      return true;
    }

    public void Unschedule(IScheduledItem item)
    {
      ScheduledItem scheduledItem = item as ScheduledItem;
      if (scheduledItem == null)
        return;
      if (this.m_TransactionMode)
        this.m_UnscheduleTransactions.Add(scheduledItem);
      else if (!this.RemovedScheduledItemAt(this.m_ScheduledItems.IndexOf(scheduledItem)))
        throw new ArgumentException("Cannot unschedule unknown scheduled function " + (object) scheduledItem);
    }

    public void UpdateScheduledEvents()
    {
      try
      {
        this.m_TransactionMode = true;
        long num1 = (long) ((double) Time.realtimeSinceStartup * 1000.0);
        int count = this.m_ScheduledItems.Count;
        long num2 = num1 + 20L;
        int num3 = this.m_LastUpdatedIndex + 1;
        if (num3 >= count)
          num3 = 0;
        for (int index1 = 0; index1 < count; ++index1)
        {
          long num4 = (long) ((double) Time.realtimeSinceStartup * 1000.0);
          if (num4 >= num2)
            break;
          int index2 = num3 + index1;
          if (index2 >= count)
            index2 -= count;
          ScheduledItem scheduledItem = this.m_ScheduledItems[index2];
          if (num4 - scheduledItem.delayMs >= scheduledItem.startMs)
          {
            TimerState state = new TimerState() { start = scheduledItem.startMs, now = num4 };
            scheduledItem.PerformTimerUpdate(state);
            scheduledItem.startMs = num4;
            scheduledItem.delayMs = scheduledItem.intervalMs;
            if (scheduledItem.ShouldUnschedule())
              this.Unschedule((IScheduledItem) scheduledItem);
          }
          this.m_LastUpdatedIndex = index2;
        }
      }
      finally
      {
        this.m_TransactionMode = false;
        for (int index = 0; index < this.m_UnscheduleTransactions.Count; ++index)
          this.Unschedule((IScheduledItem) this.m_UnscheduleTransactions[index]);
        this.m_UnscheduleTransactions.Clear();
        for (int index = 0; index < this.m_ScheduleTransactions.Count; ++index)
          this.Schedule((IScheduledItem) this.m_ScheduleTransactions[index]);
        this.m_ScheduleTransactions.Clear();
      }
    }

    private class TimerEventSchedulerItem : ScheduledItem
    {
      private readonly Action<TimerState> m_TimerUpdateEvent;

      public TimerEventSchedulerItem(Action<TimerState> updateEvent)
      {
        this.m_TimerUpdateEvent = updateEvent;
      }

      public override void PerformTimerUpdate(TimerState state)
      {
        if (this.m_TimerUpdateEvent == null)
          return;
        this.m_TimerUpdateEvent(state);
      }

      public override string ToString()
      {
        return this.m_TimerUpdateEvent.ToString();
      }
    }
  }
}
