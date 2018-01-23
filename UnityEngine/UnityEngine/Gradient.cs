// Decompiled with JetBrains decompiler
// Type: UnityEngine.Gradient
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Gradient used for animating colors.</para>
  /// </summary>
  [RequiredByNativeCode]
  [StructLayout(LayoutKind.Sequential)]
  public sealed class Gradient
  {
    internal IntPtr m_Ptr;

    /// <summary>
    ///   <para>Create a new Gradient object.</para>
    /// </summary>
    [RequiredByNativeCode]
    public Gradient()
    {
      this.Init();
    }

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Init();

    [ThreadAndSerializationSafe]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void Cleanup();

    ~Gradient()
    {
      this.Cleanup();
    }

    /// <summary>
    ///   <para>Calculate color at a given time.</para>
    /// </summary>
    /// <param name="time">Time of the key (0 - 1).</param>
    public Color Evaluate(float time)
    {
      Color color;
      Gradient.INTERNAL_CALL_Evaluate(this, time, out color);
      return color;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_Evaluate(Gradient self, float time, out Color value);

    /// <summary>
    ///   <para>All color keys defined in the gradient.</para>
    /// </summary>
    public extern GradientColorKey[] colorKeys { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>All alpha keys defined in the gradient.</para>
    /// </summary>
    public extern GradientAlphaKey[] alphaKeys { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Control how the gradient is evaluated.</para>
    /// </summary>
    public extern GradientMode mode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal Color constantColor
    {
      get
      {
        Color color;
        this.INTERNAL_get_constantColor(out color);
        return color;
      }
      set
      {
        this.INTERNAL_set_constantColor(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_constantColor(out Color value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_constantColor(ref Color value);

    /// <summary>
    ///   <para>Setup Gradient with an array of color keys and alpha keys.</para>
    /// </summary>
    /// <param name="colorKeys">Color keys of the gradient (maximum 8 color keys).</param>
    /// <param name="alphaKeys">Alpha keys of the gradient (maximum 8 alpha keys).</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetKeys(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys);
  }
}
