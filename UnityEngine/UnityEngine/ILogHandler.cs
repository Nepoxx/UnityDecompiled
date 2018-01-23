// Decompiled with JetBrains decompiler
// Type: UnityEngine.ILogHandler
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  public interface ILogHandler
  {
    void LogFormat(LogType logType, Object context, string format, params object[] args);

    void LogException(Exception exception, Object context);
  }
}
