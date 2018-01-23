// Decompiled with JetBrains decompiler
// Type: UnityEngine.Analytics.AnalyticsSessionInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Analytics
{
  /// <summary>
  ///   <para>Accesses for Analytics session information (common for all game instances).</para>
  /// </summary>
  [RequiredByNativeCode]
  public static class AnalyticsSessionInfo
  {
    public static event AnalyticsSessionInfo.SessionStateChanged sessionStateChanged;

    [RequiredByNativeCode]
    internal static void CallSessionStateChanged(AnalyticsSessionState sessionState, long sessionId, long sessionElapsedTime, bool sessionChanged)
    {
      // ISSUE: reference to a compiler-generated field
      AnalyticsSessionInfo.SessionStateChanged sessionStateChanged = AnalyticsSessionInfo.sessionStateChanged;
      if (sessionStateChanged == null)
        return;
      sessionStateChanged(sessionState, sessionId, sessionElapsedTime, sessionChanged);
    }

    /// <summary>
    ///   <para>Session state.</para>
    /// </summary>
    public static extern AnalyticsSessionState sessionState { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Session id is used for tracking player game session.</para>
    /// </summary>
    public static extern long sessionId { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Session time since the begining of player game session.</para>
    /// </summary>
    public static extern long sessionElapsedTime { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>UserId is random GUID to track a player and is persisted across game session.</para>
    /// </summary>
    public static extern string userId { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>This event occurs when a Analytics session state changes.</para>
    /// </summary>
    /// <param name="sessionState">Current session state.</param>
    /// <param name="sessionId">Current session id.</param>
    /// <param name="sessionElapsedTime">Game player current session time.</param>
    /// <param name="sessionChanged">Set to true when sessionId has changed.</param>
    public delegate void SessionStateChanged(AnalyticsSessionState sessionState, long sessionId, long sessionElapsedTime, bool sessionChanged);
  }
}
