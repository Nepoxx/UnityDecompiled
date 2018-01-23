// Decompiled with JetBrains decompiler
// Type: UnityEngine.Windows.CrashReporting
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Windows
{
  /// <summary>
  ///   <para>Exposes useful information related to crash reporting on Windows platforms.</para>
  /// </summary>
  public static class CrashReporting
  {
    /// <summary>
    ///   <para>Returns the path to the crash report folder on Windows.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public static extern string crashReportFolder { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
