// Decompiled with JetBrains decompiler
// Type: UnityEditor.Advertisements.AdvertisementSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Advertisements
{
  /// <summary>
  ///   <para>Editor API for the Unity Services editor feature. Normally UnityAds is enabled from the Services window, but if writing your own editor extension, this API can be used.</para>
  /// </summary>
  public static class AdvertisementSettings
  {
    /// <summary>
    ///   <para>Global boolean for enabling or disabling the advertisement feature.</para>
    /// </summary>
    public static extern bool enabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Controls if testing advertisements are used instead of production advertisements.</para>
    /// </summary>
    public static extern bool testMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Controls if the advertisement system should be initialized immediately on startup.</para>
    /// </summary>
    public static extern bool initializeOnStartup { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns if a specific platform is enabled.</para>
    /// </summary>
    /// <param name="platform"></param>
    /// <returns>
    ///   <para>Boolean for the platform.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [Obsolete("No longer supported and will always return true")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool IsPlatformEnabled(RuntimePlatform platform);

    /// <summary>
    ///   <para>Enable the specific platform.</para>
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="value"></param>
    [GeneratedByOldBindingsGenerator]
    [Obsolete("No longer supported and will do nothing")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetPlatformEnabled(RuntimePlatform platform, bool value);

    /// <summary>
    ///   <para>Gets the game identifier specified for a runtime platform.</para>
    /// </summary>
    /// <param name="platform"></param>
    /// <returns>
    ///   <para>The platform specific game identifier.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetGameId(RuntimePlatform platform);

    /// <summary>
    ///   <para>Sets the game identifier for the specified platform.</para>
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="gameId"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetGameId(RuntimePlatform platform, string gameId);

    /// <summary>
    ///   <para>Gets the game identifier specified for a runtime platform.</para>
    /// </summary>
    /// <param name="platformName"></param>
    /// <returns>
    ///   <para>The platform specific game identifier.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern string GetPlatformGameId(string platformName);

    /// <summary>
    ///   <para>Sets the game identifier for the specified platform.</para>
    /// </summary>
    /// <param name="platformName"></param>
    /// <param name="gameId"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetPlatformGameId(string platformName, string gameId);

    internal static extern bool enabledForPlatform { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ApplyEnableSettings(BuildTarget target);
  }
}
