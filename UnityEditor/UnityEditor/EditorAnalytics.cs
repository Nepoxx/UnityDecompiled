// Decompiled with JetBrains decompiler
// Type: UnityEditor.EditorAnalytics
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Editor API for the EditorAnalytics feature.</para>
  /// </summary>
  [RequiredByNativeCode]
  public static class EditorAnalytics
  {
    internal static bool SendEventServiceInfo(object parameters)
    {
      return EditorAnalytics.SendEvent("serviceInfo", parameters);
    }

    internal static bool SendEventShowService(object parameters)
    {
      return EditorAnalytics.SendEvent("showService", parameters);
    }

    internal static bool SendEventTimelineInfo(object parameters)
    {
      return EditorAnalytics.SendEvent("timelineInfo", parameters);
    }

    internal static bool SendEventBuildTargetDevice(object parameters)
    {
      return EditorAnalytics.SendEvent("buildTargetDevice", parameters);
    }

    internal static bool SendEventSceneViewInfo(object parameters)
    {
      return EditorAnalytics.SendEvent("sceneViewInfo", parameters);
    }

    /// <summary>
    ///   <para>Returns true when EditorAnalytics is enabled.</para>
    /// </summary>
    public static extern bool enabled { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool SendEvent(string eventName, object parameters);
  }
}
