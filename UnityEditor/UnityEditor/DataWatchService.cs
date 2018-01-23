// Decompiled with JetBrains decompiler
// Type: UnityEditor.DataWatchService
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor
{
  internal class DataWatchService : IDataWatchService
  {
    public static DataWatchService sharedInstance = new DataWatchService();
    private static List<DataWatchService.Spy> notificationTmpSpies = new List<DataWatchService.Spy>();
    private TimerEventScheduler m_Scheduler = new TimerEventScheduler();
    private Dictionary<int, DataWatchHandle> m_Handles = new Dictionary<int, DataWatchHandle>();
    private Dictionary<UnityEngine.Object, DataWatchService.Watchers> m_Watched = new Dictionary<UnityEngine.Object, DataWatchService.Watchers>();
    private static int s_WatchID;

    public DataWatchService()
    {
      Undo.postprocessModifications += new Undo.PostprocessModifications(this.PostProcessUndo);
    }

    ~DataWatchService()
    {
      Undo.postprocessModifications -= new Undo.PostprocessModifications(this.PostProcessUndo);
    }

    public UndoPropertyModification[] PostProcessUndo(UndoPropertyModification[] modifications)
    {
      foreach (UndoPropertyModification modification in modifications)
      {
        PropertyModification currentValue = modification.currentValue;
        DataWatchService.Watchers watchers;
        if (currentValue != null && !(currentValue.target == (UnityEngine.Object) null) && this.m_Watched.TryGetValue(currentValue.target, out watchers))
          watchers.isModified = true;
      }
      return modifications;
    }

    public void ForceDirtyNextPoll(UnityEngine.Object obj)
    {
      DataWatchService.Watchers watchers;
      if (!this.m_Watched.TryGetValue(obj, out watchers))
        return;
      watchers.tracker.ForceDirtyNextPoll();
      watchers.isModified = true;
    }

    public void PollNativeData()
    {
      this.m_Scheduler.UpdateScheduledEvents();
    }

    private void NotifyDataChanged(DataWatchService.Watchers w)
    {
      DataWatchService.notificationTmpSpies.Clear();
      DataWatchService.notificationTmpSpies.AddRange((IEnumerable<DataWatchService.Spy>) w.spyList);
      foreach (DataWatchService.Spy notificationTmpSpy in DataWatchService.notificationTmpSpies)
        notificationTmpSpy.onDataChanged(w.watchedObject);
      if (!(w.watchedObject == (UnityEngine.Object) null))
        return;
      this.DoRemoveWatcher(w);
    }

    private void DoRemoveWatcher(DataWatchService.Watchers watchers)
    {
      this.m_Watched.Remove(watchers.watchedObject);
      this.m_Scheduler.Unschedule(watchers.scheduledItem);
      watchers.tracker.ReleaseTracker();
    }

    public IDataWatchHandle AddWatch(UnityEngine.Object watched, Action<UnityEngine.Object> onDataChanged)
    {
      if (watched == (UnityEngine.Object) null)
        throw new ArgumentException("Object watched cannot be null");
      DataWatchHandle dataWatchHandle = new DataWatchHandle(++DataWatchService.s_WatchID, this, watched);
      this.m_Handles[dataWatchHandle.id] = dataWatchHandle;
      DataWatchService.Watchers watchers;
      if (!this.m_Watched.TryGetValue(watched, out watchers))
      {
        watchers = new DataWatchService.Watchers(watched);
        this.m_Watched[watched] = watchers;
        watchers.scheduledItem = this.m_Scheduler.ScheduleUntil(new Action<TimerState>(watchers.OnTimerPoolForChanges), 0L, 0L, (Func<bool>) null);
      }
      watchers.spyList.Add(new DataWatchService.Spy(dataWatchHandle.id, onDataChanged));
      return (IDataWatchHandle) dataWatchHandle;
    }

    public void RemoveWatch(IDataWatchHandle handle)
    {
      DataWatchHandle dataWatchHandle = (DataWatchHandle) handle;
      DataWatchService.Watchers watchers;
      if (this.m_Handles.Remove(dataWatchHandle.id) && this.m_Watched.TryGetValue(dataWatchHandle.watched, out watchers))
      {
        List<DataWatchService.Spy> spyList = watchers.spyList;
        for (int index = 0; index < spyList.Count; ++index)
        {
          if (spyList[index].handleID == dataWatchHandle.id)
          {
            spyList.RemoveAt(index);
            if (!watchers.IsEmpty())
              return;
            this.DoRemoveWatcher(watchers);
            return;
          }
        }
      }
      throw new ArgumentException("Data watch was not registered");
    }

    private struct Spy
    {
      public readonly int handleID;
      public readonly Action<UnityEngine.Object> onDataChanged;

      public Spy(int handleID, Action<UnityEngine.Object> onDataChanged)
      {
        this.handleID = handleID;
        this.onDataChanged = onDataChanged;
      }
    }

    private class Watchers
    {
      public List<DataWatchService.Spy> spyList;
      public ChangeTrackerHandle tracker;
      public IScheduledItem scheduledItem;
      public UnityEngine.Object watchedObject;

      public Watchers(UnityEngine.Object watched)
      {
        this.spyList = new List<DataWatchService.Spy>();
        this.tracker = ChangeTrackerHandle.AcquireTracker(watched);
        this.watchedObject = watched;
      }

      private DataWatchService service
      {
        get
        {
          return DataWatchService.sharedInstance;
        }
      }

      public bool isModified { get; set; }

      public void AddSpy(int handle, Action<UnityEngine.Object> onDataChanged)
      {
        this.spyList.Add(new DataWatchService.Spy(handle, onDataChanged));
      }

      public bool IsEmpty()
      {
        return this.spyList == null;
      }

      public void OnTimerPoolForChanges(TimerState ts)
      {
        if (!this.PollForChanges())
          return;
        this.isModified = false;
        this.service.NotifyDataChanged(this);
      }

      public bool PollForChanges()
      {
        if (this.watchedObject == (UnityEngine.Object) null)
          this.isModified = true;
        else if (this.tracker.PollForChanges())
          this.isModified = true;
        return this.isModified;
      }
    }
  }
}
