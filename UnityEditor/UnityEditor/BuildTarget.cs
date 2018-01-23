// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildTarget
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Target build platform.</para>
  /// </summary>
  [NativeType("Runtime/Serialize/SerializationMetaFlags.h")]
  public enum BuildTarget
  {
    NoTarget = -2,
    [Obsolete("BlackBerry has been removed in 5.4")] BB10 = -1,
    [Obsolete("Use WSAPlayer instead (UnityUpgradable) -> WSAPlayer", true)] MetroPlayer = -1,
    [Obsolete("Use iOS instead (UnityUpgradable) -> iOS", true)] iPhone = -1,
    StandaloneOSX = 2,
    [Obsolete("Use StandaloneOSX instead (UnityUpgradable) -> StandaloneOSX", true)] StandaloneOSXUniversal = 2,
    [Obsolete("StandaloneOSXIntel has been removed in 2017.3")] StandaloneOSXIntel = 4,
    StandaloneWindows = 5,
    [Obsolete("WebPlayer has been removed in 5.4", true)] WebPlayer = 6,
    [Obsolete("WebPlayerStreamed has been removed in 5.4", true)] WebPlayerStreamed = 7,
    iOS = 9,
    [Obsolete("PS3 has been removed in >=5.5")] PS3 = 10, // 0x0000000A
    [Obsolete("XBOX360 has been removed in 5.5")] XBOX360 = 11, // 0x0000000B
    Android = 13, // 0x0000000D
    StandaloneLinux = 17, // 0x00000011
    StandaloneWindows64 = 19, // 0x00000013
    WebGL = 20, // 0x00000014
    WSAPlayer = 21, // 0x00000015
    StandaloneLinux64 = 24, // 0x00000018
    StandaloneLinuxUniversal = 25, // 0x00000019
    [Obsolete("Use WSAPlayer with Windows Phone 8.1 selected")] WP8Player = 26, // 0x0000001A
    [Obsolete("StandaloneOSXIntel64 has been removed in 2017.3")] StandaloneOSXIntel64 = 27, // 0x0000001B
    [Obsolete("BlackBerry has been removed in 5.4")] BlackBerry = 28, // 0x0000001C
    Tizen = 29, // 0x0000001D
    PSP2 = 30, // 0x0000001E
    PS4 = 31, // 0x0000001F
    PSM = 32, // 0x00000020
    XboxOne = 33, // 0x00000021
    [Obsolete("SamsungTV has been removed in 2017.3")] SamsungTV = 34, // 0x00000022
    N3DS = 35, // 0x00000023
    WiiU = 36, // 0x00000024
    tvOS = 37, // 0x00000025
    Switch = 38, // 0x00000026
  }
}
