// Decompiled with JetBrains decompiler
// Type: UnityEngine.UnitySynchronizationContext
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Collections.Generic;
using System.Threading;
using UnityEngine.Scripting;

namespace UnityEngine
{
  internal sealed class UnitySynchronizationContext : SynchronizationContext
  {
    private readonly Queue<UnitySynchronizationContext.WorkRequest> m_AsyncWorkQueue = new Queue<UnitySynchronizationContext.WorkRequest>(20);
    private readonly int m_MainThreadID = Thread.CurrentThread.ManagedThreadId;
    private const int kAwqInitialCapacity = 20;

    public override void Send(SendOrPostCallback callback, object state)
    {
      if (this.m_MainThreadID == Thread.CurrentThread.ManagedThreadId)
      {
        callback(state);
      }
      else
      {
        using (ManualResetEvent waitHandle = new ManualResetEvent(false))
        {
          lock ((object) this.m_AsyncWorkQueue)
            this.m_AsyncWorkQueue.Enqueue(new UnitySynchronizationContext.WorkRequest(callback, state, waitHandle));
          waitHandle.WaitOne();
        }
      }
    }

    public override void Post(SendOrPostCallback callback, object state)
    {
      lock ((object) this.m_AsyncWorkQueue)
        this.m_AsyncWorkQueue.Enqueue(new UnitySynchronizationContext.WorkRequest(callback, state, (ManualResetEvent) null));
    }

    private void Exec()
    {
      lock ((object) this.m_AsyncWorkQueue)
      {
        while (this.m_AsyncWorkQueue.Count > 0)
          this.m_AsyncWorkQueue.Dequeue().Invoke();
      }
    }

    [RequiredByNativeCode]
    private static void InitializeSynchronizationContext()
    {
      if (SynchronizationContext.Current != null)
        return;
      SynchronizationContext.SetSynchronizationContext((SynchronizationContext) new UnitySynchronizationContext());
    }

    [RequiredByNativeCode]
    private static void ExecuteTasks()
    {
      UnitySynchronizationContext current = SynchronizationContext.Current as UnitySynchronizationContext;
      if (current == null)
        return;
      current.Exec();
    }

    private struct WorkRequest
    {
      private readonly SendOrPostCallback m_DelagateCallback;
      private readonly object m_DelagateState;
      private readonly ManualResetEvent m_WaitHandle;

      public WorkRequest(SendOrPostCallback callback, object state, ManualResetEvent waitHandle = null)
      {
        this.m_DelagateCallback = callback;
        this.m_DelagateState = state;
        this.m_WaitHandle = waitHandle;
      }

      public void Invoke()
      {
        this.m_DelagateCallback(this.m_DelagateState);
        if (this.m_WaitHandle == null)
          return;
        this.m_WaitHandle.Set();
      }
    }
  }
}
