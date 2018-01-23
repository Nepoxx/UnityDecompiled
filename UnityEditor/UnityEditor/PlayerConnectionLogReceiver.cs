// Decompiled with JetBrains decompiler
// Type: UnityEditor.PlayerConnectionLogReceiver
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Networking.PlayerConnection;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking.PlayerConnection;

namespace UnityEditor
{
  internal class PlayerConnectionLogReceiver : ScriptableSingleton<PlayerConnectionLogReceiver>
  {
    [SerializeField]
    private PlayerConnectionLogReceiver.ConnectionState state = PlayerConnectionLogReceiver.ConnectionState.Disconnected;
    private const string prefsKey = "PlayerConnectionLoggingState";

    private static Guid logMessageId
    {
      get
      {
        return new Guid("394ada03-8ba0-4f26-b001-1a6cdeb05a62");
      }
    }

    private static Guid cleanLogMessageId
    {
      get
      {
        return new Guid("3ded2dda-cdf2-46d8-a3f6-01741741e7a9");
      }
    }

    private void OnEnable()
    {
      this.State = (PlayerConnectionLogReceiver.ConnectionState) EditorPrefs.GetInt("PlayerConnectionLoggingState", 1);
    }

    internal PlayerConnectionLogReceiver.ConnectionState State
    {
      get
      {
        return this.state;
      }
      set
      {
        if (this.state == value)
          return;
        switch (this.state)
        {
          case PlayerConnectionLogReceiver.ConnectionState.CleanLog:
            ScriptableSingleton<EditorConnection>.instance.Unregister(PlayerConnectionLogReceiver.cleanLogMessageId, new UnityAction<MessageEventArgs>(this.LogMessage));
            break;
          case PlayerConnectionLogReceiver.ConnectionState.FullLog:
            ScriptableSingleton<EditorConnection>.instance.Unregister(PlayerConnectionLogReceiver.logMessageId, new UnityAction<MessageEventArgs>(this.LogMessage));
            break;
        }
        this.state = value;
        switch (this.state)
        {
          case PlayerConnectionLogReceiver.ConnectionState.CleanLog:
            ScriptableSingleton<EditorConnection>.instance.Register(PlayerConnectionLogReceiver.cleanLogMessageId, new UnityAction<MessageEventArgs>(this.LogMessage));
            break;
          case PlayerConnectionLogReceiver.ConnectionState.FullLog:
            ScriptableSingleton<EditorConnection>.instance.Register(PlayerConnectionLogReceiver.logMessageId, new UnityAction<MessageEventArgs>(this.LogMessage));
            break;
        }
        EditorPrefs.SetInt("PlayerConnectionLoggingState", (int) this.state);
      }
    }

    private void LogMessage(MessageEventArgs messageEventArgs)
    {
      string str1 = Encoding.UTF8.GetString(((IEnumerable<byte>) messageEventArgs.data).Skip<byte>(4).ToArray<byte>());
      LogType logType = (LogType) messageEventArgs.data[0];
      if (!Enum.IsDefined(typeof (LogType), (object) logType))
        logType = LogType.Log;
      StackTraceLogType stackTraceLogType = Application.GetStackTraceLogType(logType);
      Application.SetStackTraceLogType(logType, StackTraceLogType.None);
      string str2 = "<i>" + ProfilerDriver.GetConnectionIdentifier(messageEventArgs.playerId) + "</i> " + str1;
      Debug.unityLogger.Log(logType, (object) str2);
      Application.SetStackTraceLogType(logType, stackTraceLogType);
    }

    internal enum ConnectionState
    {
      Disconnected,
      CleanLog,
      FullLog,
    }
  }
}
