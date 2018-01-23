// Decompiled with JetBrains decompiler
// Type: UnityEngine.LightProbes
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Stores light probes for the scene.</para>
  /// </summary>
  public sealed class LightProbes : Object
  {
    public static void GetInterpolatedProbe(Vector3 position, Renderer renderer, out SphericalHarmonicsL2 probe)
    {
      LightProbes.INTERNAL_CALL_GetInterpolatedProbe(ref position, renderer, out probe);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetInterpolatedProbe(ref Vector3 position, Renderer renderer, out SphericalHarmonicsL2 probe);

    /// <summary>
    ///   <para>Positions of the baked light probes (Read Only).</para>
    /// </summary>
    public extern Vector3[] positions { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Coefficients of baked light probes.</para>
    /// </summary>
    public extern SphericalHarmonicsL2[] bakedProbes { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number of light probes (Read Only).</para>
    /// </summary>
    public extern int count { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>The number of cells space is divided into (Read Only).</para>
    /// </summary>
    public extern int cellCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool AreLightProbesAllowed(Renderer renderer);

    [Obsolete("Use GetInterpolatedProbe instead.", true)]
    public void GetInterpolatedLightProbe(Vector3 position, Renderer renderer, float[] coefficients)
    {
    }

    [Obsolete("Use bakedProbes instead.", true)]
    public float[] coefficients
    {
      get
      {
        return new float[0];
      }
      set
      {
      }
    }
  }
}
