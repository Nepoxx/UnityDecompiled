// Decompiled with JetBrains decompiler
// Type: UnityEditor.Analytics.PerformanceReportingSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Analytics
{
  /// <summary>
  ///   <para>Normally performance reporting is enabled from the Services window, but if writing your own editor extension, this API can be used.</para>
  /// </summary>
  public static class PerformanceReportingSettings
  {
    /// <summary>
    ///   <para>This Boolean field causes the performance reporting feature in Unity to be enabled if true, or disabled if false.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public static extern bool enabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
