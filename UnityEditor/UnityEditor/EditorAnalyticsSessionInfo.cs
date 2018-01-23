// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorAnalyticsSessionInfo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Accesses for EditorAnalytics session information.</para>
  /// </summary>
  [RequiredByNativeCode]
  public static class EditorAnalyticsSessionInfo
  {
    /// <summary>
    ///   <para>Returns session id used for tracking the Editor session.</para>
    /// </summary>
    public static extern long id { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns session time since startup of the Editor.</para>
    /// </summary>
    public static extern long elapsedTime { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns total time the Editor was in focus since the beginning of a session.</para>
    /// </summary>
    public static extern long focusedElapsedTime { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns total time the Editor was in playmode since the beginning of a session.</para>
    /// </summary>
    public static extern long playbackElapsedTime { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Returns total time there was user iteraction in the Editor since the beginning of a session.</para>
    /// </summary>
    public static extern long activeElapsedTime { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The userId is random GUID and is persisted across Editor session.</para>
    /// </summary>
    public static extern string userId { [MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
