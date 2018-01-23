// Decompiled with JetBrains decompiler
// Type: UnityEngine.WaitForSeconds
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Suspends the coroutine execution for the given amount of seconds using scaled time.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class WaitForSeconds : YieldInstruction
  {
    internal float m_Seconds;

    /// <summary>
    ///   <para>Creates a yield instruction to wait for a given number of seconds using scaled time.</para>
    /// </summary>
    /// <param name="seconds"></param>
    public WaitForSeconds(float seconds)
    {
      this.m_Seconds = seconds;
    }
  }
}
