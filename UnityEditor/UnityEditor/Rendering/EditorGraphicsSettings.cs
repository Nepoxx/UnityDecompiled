// Decompiled with JetBrains decompiler
// Type: UnityEditor.Rendering.EditorGraphicsSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEditor.Rendering
{
  /// <summary>
  ///   <para>Editor-specific script interface for.</para>
  /// </summary>
  public sealed class EditorGraphicsSettings
  {
    internal static void SetTierSettingsImpl(BuildTargetGroup target, GraphicsTier tier, TierSettings settings)
    {
      EditorGraphicsSettings.INTERNAL_CALL_SetTierSettingsImpl(target, tier, ref settings);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_SetTierSettingsImpl(BuildTargetGroup target, GraphicsTier tier, ref TierSettings settings);

    internal static TierSettings GetTierSettingsImpl(BuildTargetGroup target, GraphicsTier tier)
    {
      TierSettings tierSettings;
      EditorGraphicsSettings.INTERNAL_CALL_GetTierSettingsImpl(target, tier, out tierSettings);
      return tierSettings;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetTierSettingsImpl(BuildTargetGroup target, GraphicsTier tier, out TierSettings value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void OnUpdateTierSettingsImpl(BuildTargetGroup target, bool shouldReloadShaders);

    internal static TierSettings GetCurrentTierSettingsImpl()
    {
      TierSettings tierSettings;
      EditorGraphicsSettings.INTERNAL_CALL_GetCurrentTierSettingsImpl(out tierSettings);
      return tierSettings;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetCurrentTierSettingsImpl(out TierSettings value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool AreTierSettingsAutomatic(BuildTargetGroup target, GraphicsTier tier);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void MakeTierSettingsAutomatic(BuildTargetGroup target, GraphicsTier tier, bool automatic);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void RegisterUndoForGraphicsSettings();

    /// <summary>
    ///   <para>Returns an array of Rendering.AlbedoSwatchInfo.</para>
    /// </summary>
    public static extern AlbedoSwatchInfo[] albedoSwatches { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns TierSettings for the target build platform and shader hardware tier.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tier"></param>
    public static TierSettings GetTierSettings(BuildTargetGroup target, GraphicsTier tier)
    {
      return EditorGraphicsSettings.GetTierSettingsImpl(target, tier);
    }

    /// <summary>
    ///   <para>Allows you to set the PlatformShaderSettings for the specified platform and shader hardware tier.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tier"></param>
    /// <param name="settings"></param>
    public static void SetTierSettings(BuildTargetGroup target, GraphicsTier tier, TierSettings settings)
    {
      if (settings.renderingPath == RenderingPath.UsePlayerSettings)
        throw new ArgumentException("TierSettings.renderingPath must be actual rendering path (not UsePlayerSettings)", nameof (settings));
      EditorGraphicsSettings.SetTierSettingsImpl(target, tier, settings);
      EditorGraphicsSettings.MakeTierSettingsAutomatic(target, tier, false);
      EditorGraphicsSettings.OnUpdateTierSettingsImpl(target, true);
    }

    internal static TierSettings GetCurrentTierSettings()
    {
      return EditorGraphicsSettings.GetCurrentTierSettingsImpl();
    }

    /// <summary>
    ///   <para>Will return PlatformShaderSettings for given platform and shader hardware tier.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tier"></param>
    [Obsolete("Use GetTierSettings() instead (UnityUpgradable) -> GetTierSettings(*)", false)]
    public static PlatformShaderSettings GetShaderSettingsForPlatform(BuildTargetGroup target, ShaderHardwareTier tier)
    {
      TierSettings tierSettings = EditorGraphicsSettings.GetTierSettings(target, (GraphicsTier) tier);
      return new PlatformShaderSettings() { cascadedShadowMaps = tierSettings.cascadedShadowMaps, standardShaderQuality = tierSettings.standardShaderQuality, reflectionProbeBoxProjection = tierSettings.reflectionProbeBoxProjection, reflectionProbeBlending = tierSettings.reflectionProbeBlending };
    }

    /// <summary>
    ///   <para>Allows you to set the PlatformShaderSettings for the specified platform and shader hardware tier.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tier"></param>
    /// <param name="settings"></param>
    [Obsolete("Use SetTierSettings() instead (UnityUpgradable) -> SetTierSettings(*)", false)]
    public static void SetShaderSettingsForPlatform(BuildTargetGroup target, ShaderHardwareTier tier, PlatformShaderSettings settings)
    {
      TierSettings tierSettings = EditorGraphicsSettings.GetTierSettings(target, (GraphicsTier) tier);
      tierSettings.standardShaderQuality = settings.standardShaderQuality;
      tierSettings.cascadedShadowMaps = settings.cascadedShadowMaps;
      tierSettings.reflectionProbeBoxProjection = settings.reflectionProbeBoxProjection;
      tierSettings.reflectionProbeBlending = settings.reflectionProbeBlending;
      EditorGraphicsSettings.SetTierSettings(target, (GraphicsTier) tier, tierSettings);
    }

    /// <summary>
    ///   <para>TODO.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tier"></param>
    [Obsolete("Use GraphicsTier instead of ShaderHardwareTier enum", false)]
    public static TierSettings GetTierSettings(BuildTargetGroup target, ShaderHardwareTier tier)
    {
      return EditorGraphicsSettings.GetTierSettings(target, (GraphicsTier) tier);
    }

    /// <summary>
    ///   <para>TODO.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tier"></param>
    /// <param name="settings"></param>
    [Obsolete("Use GraphicsTier instead of ShaderHardwareTier enum", false)]
    public static void SetTierSettings(BuildTargetGroup target, ShaderHardwareTier tier, TierSettings settings)
    {
      EditorGraphicsSettings.SetTierSettings(target, (GraphicsTier) tier, settings);
    }
  }
}
