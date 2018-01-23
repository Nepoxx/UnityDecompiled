// Decompiled with JetBrains decompiler
// Type: UnityEditor.DrawCameraMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Drawing modes for Handles.DrawCamera.</para>
  /// </summary>
  public enum DrawCameraMode
  {
    [Obsolete("Renamed to better distinguish this mode from new Progressive baked modes. Please use BakedLightmap instead. (UnityUpgradable) -> BakedLightmap", true)] Baked = -18, // -0x00000012
    [Obsolete("Renamed to better distinguish this mode from new Progressive baked modes. Please use RealtimeDirectionality instead. (UnityUpgradable) -> RealtimeDirectionality", true)] Directionality = -17, // -0x00000011
    [Obsolete("Renamed to better distinguish this mode from new Progressive baked modes. Please use RealtimeIndirect instead. (UnityUpgradable) -> RealtimeIndirect", true)] Irradiance = -16, // -0x00000010
    [Obsolete("Renamed to better distinguish this mode from new Progressive baked modes. Please use RealtimeEmissive instead. (UnityUpgradable) -> RealtimeEmissive", true)] Emissive = -15, // -0x0000000F
    [Obsolete("Renamed to better distinguish this mode from new Progressive baked modes. Please use RealtimeAlbedo instead. (UnityUpgradable) -> RealtimeAlbedo", true)] Albedo = -14, // -0x0000000E
    [Obsolete("Renamed to better distinguish this mode from new Progressive baked modes. Please use RealtimeCharting instead. (UnityUpgradable) -> RealtimeCharting", true)] Charting = -12, // -0x0000000C
    Normal = -1,
    Textured = 0,
    Wireframe = 1,
    TexturedWire = 2,
    ShadowCascades = 3,
    RenderPaths = 4,
    AlphaChannel = 5,
    Overdraw = 6,
    Mipmaps = 7,
    DeferredDiffuse = 8,
    DeferredSpecular = 9,
    DeferredSmoothness = 10, // 0x0000000A
    DeferredNormal = 11, // 0x0000000B
    RealtimeCharting = 12, // 0x0000000C
    Systems = 13, // 0x0000000D
    RealtimeAlbedo = 14, // 0x0000000E
    RealtimeEmissive = 15, // 0x0000000F
    RealtimeIndirect = 16, // 0x00000010
    RealtimeDirectionality = 17, // 0x00000011
    BakedLightmap = 18, // 0x00000012
    Clustering = 19, // 0x00000013
    LitClustering = 20, // 0x00000014
    ValidateAlbedo = 21, // 0x00000015
    ValidateMetalSpecular = 22, // 0x00000016
    ShadowMasks = 23, // 0x00000017
    LightOverlap = 24, // 0x00000018
    BakedAlbedo = 25, // 0x00000019
    BakedEmissive = 26, // 0x0000001A
    BakedDirectionality = 27, // 0x0000001B
    BakedTexelValidity = 28, // 0x0000001C
    BakedIndices = 29, // 0x0000001D
    BakedCharting = 30, // 0x0000001E
    SpriteMask = 31, // 0x0000001F
  }
}
