// Decompiled with JetBrains decompiler
// Type: UnityEditor.Rendering.PlatformShaderSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.InteropServices;

namespace UnityEditor.Rendering
{
  /// <summary>
  ///   <para>Used to set up shader settings, per-platform and per-shader-hardware-tier.</para>
  /// </summary>
  [Obsolete("Use TierSettings instead (UnityUpgradable) -> UnityEditor.Rendering.TierSettings", false)]
  public struct PlatformShaderSettings
  {
    /// <summary>
    ///   <para>Allows you to specify whether cascaded shadow maps should be used.</para>
    /// </summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool cascadedShadowMaps;
    /// <summary>
    ///   <para>Allows you to specify whether Reflection Probes Box Projection should be used.</para>
    /// </summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool reflectionProbeBoxProjection;
    /// <summary>
    ///   <para>Allows you to specify whether Reflection Probes Blending should be enabled.</para>
    /// </summary>
    [MarshalAs(UnmanagedType.I1)]
    public bool reflectionProbeBlending;
    /// <summary>
    ///   <para>Allows you to select Standard Shader Quality.</para>
    /// </summary>
    public ShaderQuality standardShaderQuality;
  }
}
