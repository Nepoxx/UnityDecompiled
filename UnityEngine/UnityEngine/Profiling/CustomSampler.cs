// Decompiled with JetBrains decompiler
// Type: UnityEngine.Profiling.CustomSampler
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Profiling
{
  /// <summary>
  ///   <para>Custom CPU Profiler label used for profiling arbitrary code blocks.</para>
  /// </summary>
  [UsedByNativeCode]
  public sealed class CustomSampler : Sampler
  {
    internal static CustomSampler s_InvalidCustomSampler = new CustomSampler();

    internal CustomSampler()
    {
    }

    /// <summary>
    ///   <para>Creates a new CustomSampler for profiling parts of your code.</para>
    /// </summary>
    /// <param name="name">Name of the Sampler.</param>
    /// <returns>
    ///   <para>CustomSampler object or null if a built-in Sampler with the same name exists.</para>
    /// </returns>
    public static CustomSampler Create(string name)
    {
      return CustomSampler.CreateInternal(name) ?? CustomSampler.s_InvalidCustomSampler;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern CustomSampler CreateInternal(string name);

    /// <summary>
    ///   <para>Begin profiling a piece of code with a custom label defined by this instance of CustomSampler.</para>
    /// </summary>
    /// <param name="targetObject"></param>
    [GeneratedByOldBindingsGenerator]
    [Conditional("ENABLE_PROFILER")]
    [ThreadAndSerializationSafe]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Begin();

    /// <summary>
    ///   <para>Begin profiling a piece of code with a custom label defined by this instance of CustomSampler.</para>
    /// </summary>
    /// <param name="targetObject"></param>
    [Conditional("ENABLE_PROFILER")]
    public void Begin(Object targetObject)
    {
      this.BeginWithObject(targetObject);
    }

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void BeginWithObject(Object targetObject);

    /// <summary>
    ///   <para>End profiling a piece of code with a custom label.</para>
    /// </summary>
    [Conditional("ENABLE_PROFILER")]
    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void End();
  }
}
