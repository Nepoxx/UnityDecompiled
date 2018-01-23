// Decompiled with JetBrains decompiler
// Type: UnityEngine.Logger
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine
{
  public class Logger : ILogger, ILogHandler
  {
    private const string kNoTagFormat = "{0}";
    private const string kTagFormat = "{0}: {1}";

    private Logger()
    {
    }

    public Logger(ILogHandler logHandler)
    {
      this.logHandler = logHandler;
      this.logEnabled = true;
      this.filterLogType = LogType.Log;
    }

    public ILogHandler logHandler { get; set; }

    public bool logEnabled { get; set; }

    public LogType filterLogType { get; set; }

    public bool IsLogTypeAllowed(LogType logType)
    {
      if (this.logEnabled)
      {
        if (logType == LogType.Exception)
          return true;
        if (this.filterLogType != LogType.Exception)
          return logType <= this.filterLogType;
      }
      return false;
    }

    private static string GetString(object message)
    {
      return message == null ? "Null" : message.ToString();
    }

    public void Log(LogType logType, object message)
    {
      if (!this.IsLogTypeAllowed(logType))
        return;
      this.logHandler.LogFormat(logType, (Object) null, "{0}", (object) Logger.GetString(message));
    }

    public void Log(LogType logType, object message, Object context)
    {
      if (!this.IsLogTypeAllowed(logType))
        return;
      this.logHandler.LogFormat(logType, context, "{0}", (object) Logger.GetString(message));
    }

    public void Log(LogType logType, string tag, object message)
    {
      if (!this.IsLogTypeAllowed(logType))
        return;
      this.logHandler.LogFormat(logType, (Object) null, "{0}: {1}", (object) tag, (object) Logger.GetString(message));
    }

    public void Log(LogType logType, string tag, object message, Object context)
    {
      if (!this.IsLogTypeAllowed(logType))
        return;
      this.logHandler.LogFormat(logType, context, "{0}: {1}", (object) tag, (object) Logger.GetString(message));
    }

    public void Log(object message)
    {
      if (!this.IsLogTypeAllowed(LogType.Log))
        return;
      this.logHandler.LogFormat(LogType.Log, (Object) null, "{0}", (object) Logger.GetString(message));
    }

    public void Log(string tag, object message)
    {
      if (!this.IsLogTypeAllowed(LogType.Log))
        return;
      this.logHandler.LogFormat(LogType.Log, (Object) null, "{0}: {1}", (object) tag, (object) Logger.GetString(message));
    }

    public void Log(string tag, object message, Object context)
    {
      if (!this.IsLogTypeAllowed(LogType.Log))
        return;
      this.logHandler.LogFormat(LogType.Log, context, "{0}: {1}", (object) tag, (object) Logger.GetString(message));
    }

    public void LogWarning(string tag, object message)
    {
      if (!this.IsLogTypeAllowed(LogType.Warning))
        return;
      this.logHandler.LogFormat(LogType.Warning, (Object) null, "{0}: {1}", (object) tag, (object) Logger.GetString(message));
    }

    public void LogWarning(string tag, object message, Object context)
    {
      if (!this.IsLogTypeAllowed(LogType.Warning))
        return;
      this.logHandler.LogFormat(LogType.Warning, context, "{0}: {1}", (object) tag, (object) Logger.GetString(message));
    }

    public void LogError(string tag, object message)
    {
      if (!this.IsLogTypeAllowed(LogType.Error))
        return;
      this.logHandler.LogFormat(LogType.Error, (Object) null, "{0}: {1}", (object) tag, (object) Logger.GetString(message));
    }

    public void LogError(string tag, object message, Object context)
    {
      if (!this.IsLogTypeAllowed(LogType.Error))
        return;
      this.logHandler.LogFormat(LogType.Error, context, "{0}: {1}", (object) tag, (object) Logger.GetString(message));
    }

    public void LogFormat(LogType logType, string format, params object[] args)
    {
      if (!this.IsLogTypeAllowed(logType))
        return;
      this.logHandler.LogFormat(logType, (Object) null, format, args);
    }

    public void LogException(Exception exception)
    {
      if (!this.logEnabled)
        return;
      this.logHandler.LogException(exception, (Object) null);
    }

    public void LogFormat(LogType logType, Object context, string format, params object[] args)
    {
      if (!this.IsLogTypeAllowed(logType))
        return;
      this.logHandler.LogFormat(logType, context, format, args);
    }

    public void LogException(Exception exception, Object context)
    {
      if (!this.logEnabled)
        return;
      this.logHandler.LogException(exception, context);
    }
  }
}
