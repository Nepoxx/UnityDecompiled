// Decompiled with JetBrains decompiler
// Type: UnityEngine.AudioHighPassFilter
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The Audio High Pass Filter passes high frequencies of an AudioSource, and cuts off signals with frequencies lower than the Cutoff Frequency.</para>
  /// </summary>
  [RequireComponent(typeof (AudioBehaviour))]
  public sealed class AudioHighPassFilter : Behaviour
  {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("AudioHighPassFilter.highpassResonaceQ is obsolete. Use highpassResonanceQ instead (UnityUpgradable) -> highpassResonanceQ", true)]
    public float highpassResonaceQ
    {
      get
      {
        return this.highpassResonanceQ;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Highpass cutoff frequency in hz. 10.0 to 22000.0. Default = 5000.0.</para>
    /// </summary>
    public extern float cutoffFrequency { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Determines how much the filter's self-resonance isdampened.</para>
    /// </summary>
    public extern float highpassResonanceQ { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
