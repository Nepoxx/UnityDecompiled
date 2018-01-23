// Decompiled with JetBrains decompiler
// Type: UnityEngine.Profiling.Recorder
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Profiling
{
  /// <summary>
  ///   <para>Records profiling data produced by a specific Sampler.</para>
  /// </summary>
  [UsedByNativeCode]
  public sealed class Recorder
  {
    internal static Recorder s_InvalidRecorder = new Recorder();
    internal IntPtr m_Ptr;

    internal Recorder()
    {
    }

    ~Recorder()
    {
      if (!(this.m_Ptr != IntPtr.Zero))
        return;
      this.DisposeNative();
    }

    /// <summary>
    ///   <para>Use this function to get a Recorder for the specific Profiler label.</para>
    /// </summary>
    /// <param name="samplerName">Sampler name.</param>
    /// <returns>
    ///   <para>Recorder object for the specified Sampler.</para>
    /// </returns>
    public static Recorder Get(string samplerName)
    {
      return Sampler.Get(samplerName).GetRecorder();
    }

    /// <summary>
    ///   <para>Returns true if Recorder is valid and can collect data. (Read Only)</para>
    /// </summary>
    public bool isValid
    {
      get
      {
        return this.m_Ptr != IntPtr.Zero;
      }
    }

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void DisposeNative();

    /// <summary>
    ///   <para>Enables recording.</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern bool enabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Accumulated time of Begin/End pairs for the previous frame in nanoseconds. (Read Only)</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern long elapsedNanoseconds { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Number of time Begin/End pairs was called during the previous frame. (Read Only)</para>
    /// </summary>
    [ThreadAndSerializationSafe]
    public extern int sampleBlockCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }
  }
}
