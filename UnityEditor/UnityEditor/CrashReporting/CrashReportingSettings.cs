// Decompiled with JetBrains decompiler
// Type: UnityEditor.CrashReporting.CrashReportingSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.CrashReporting
{
  /// <summary>
  ///   <para>Editor API for the Unity Services editor feature. Normally CrashReporting is enabled from the Services window, but if writing your own editor extension, this API can be used.</para>
  /// </summary>
  public static class CrashReportingSettings
  {
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetEventUrl();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetEventUrl(string eventUrl);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string GetNativeEventUrl();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetNativeEventUrl(string eventUrl);

    /// <summary>
    ///   <para>This Boolean field will cause the CrashReporting feature in Unity to be enabled if true, or disabled if false.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public static extern bool enabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>This Boolean field will cause the CrashReporting feature in Unity to capture exceptions that occur in the editor while running in Play mode if true, or ignore those errors if false.</para>
    /// </summary>
    public static extern bool captureEditorExceptions { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
