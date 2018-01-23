// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.IDataWatchService
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  internal interface IDataWatchService
  {
    IDataWatchHandle AddWatch(UnityEngine.Object watched, Action<UnityEngine.Object> onDataChanged);

    void RemoveWatch(IDataWatchHandle handle);

    void ForceDirtyNextPoll(UnityEngine.Object obj);
  }
}
