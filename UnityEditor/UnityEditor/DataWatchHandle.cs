// Decompiled with JetBrains decompiler
// Type: UnityEditor.DataWatchHandle
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor
{
  internal class DataWatchHandle : IDataWatchHandle, IDisposable
  {
    public readonly int id;
    public WeakReference service;

    public DataWatchHandle(int id, DataWatchService service, UnityEngine.Object watched)
    {
      this.id = id;
      this.service = new WeakReference((object) service);
      this.watched = watched;
    }

    public UnityEngine.Object watched { get; private set; }

    public bool disposed
    {
      get
      {
        return object.ReferenceEquals((object) this.watched, (object) null);
      }
    }

    public void Dispose()
    {
      if (this.disposed)
        throw new InvalidOperationException("DataWatchHandle was already disposed of");
      if (this.service != null && this.service.IsAlive)
        (this.service.Target as DataWatchService).RemoveWatch((IDataWatchHandle) this);
      this.service = (WeakReference) null;
      this.watched = (UnityEngine.Object) null;
    }
  }
}
