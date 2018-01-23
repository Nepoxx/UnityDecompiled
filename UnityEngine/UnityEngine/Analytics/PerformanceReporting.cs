// Decompiled with JetBrains decompiler
// Type: UnityEngine.Analytics.PerformanceReporting
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Analytics
{
  /// <summary>
  ///   <para>Unity Performace provides insight into your game performace.</para>
  /// </summary>
  public static class PerformanceReporting
  {
    /// <summary>
    ///   <para>Controls whether the Performance Reporting service is enabled at runtime.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public static extern bool enabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
