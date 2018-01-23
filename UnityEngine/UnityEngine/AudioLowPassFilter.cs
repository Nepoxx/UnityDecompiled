// Decompiled with JetBrains decompiler
// Type: UnityEngine.AudioLowPassFilter
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
  ///   <para>The Audio Low Pass Filter passes low frequencies of an AudioSource or all sounds reaching an AudioListener, while removing frequencies higher than the Cutoff Frequency.</para>
  /// </summary>
  [RequireComponent(typeof (AudioBehaviour))]
  public sealed class AudioLowPassFilter : Behaviour
  {
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("AudioLowPassFilter.lowpassResonaceQ is obsolete. Use lowpassResonanceQ instead (UnityUpgradable) -> lowpassResonanceQ", true)]
    public float lowpassResonaceQ
    {
      get
      {
        return this.lowpassResonanceQ;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Lowpass cutoff frequency in hz. 10.0 to 22000.0. Default = 5000.0.</para>
    /// </summary>
    public extern float cutoffFrequency { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns or sets the current custom frequency cutoff curve.</para>
    /// </summary>
    public extern AnimationCurve customCutoffCurve { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Determines how much the filter's self-resonance is dampened.</para>
    /// </summary>
    public extern float lowpassResonanceQ { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }
  }
}
