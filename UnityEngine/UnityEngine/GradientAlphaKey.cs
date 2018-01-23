// Decompiled with JetBrains decompiler
// Type: UnityEngine.GradientAlphaKey
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Alpha key used by Gradient.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct GradientAlphaKey
  {
    /// <summary>
    ///   <para>Alpha channel of key.</para>
    /// </summary>
    public float alpha;
    /// <summary>
    ///   <para>Time of the key (0 - 1).</para>
    /// </summary>
    public float time;

    /// <summary>
    ///   <para>Gradient alpha key.</para>
    /// </summary>
    /// <param name="alpha">Alpha of key (0 - 1).</param>
    /// <param name="time">Time of the key (0 - 1).</param>
    public GradientAlphaKey(float alpha, float time)
    {
      this.alpha = alpha;
      this.time = time;
    }
  }
}
