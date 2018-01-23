// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.IDataWatchHandle
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  public interface IDataWatchHandle : IDisposable
  {
    UnityEngine.Object watched { get; }

    bool disposed { get; }
  }
}
