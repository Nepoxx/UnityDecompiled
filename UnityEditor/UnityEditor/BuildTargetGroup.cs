// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildTargetGroup
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Build target group.</para>
  /// </summary>
  [NativeType(Header = "Editor/Src/BuildPipeline/BuildTargetPlatformSpecific.h")]
  public enum BuildTargetGroup
  {
    Unknown = 0,
    Standalone = 1,
    [Obsolete("WebPlayer was removed in 5.4, consider using WebGL", true)] WebPlayer = 2,
    iOS = 4,
    [Obsolete("Use iOS instead (UnityUpgradable) -> iOS", true)] iPhone = 4,
    [Obsolete("PS3 has been removed in >=5.5")] PS3 = 5,
    [Obsolete("XBOX360 has been removed in 5.5")] XBOX360 = 6,
    Android = 7,
    WebGL = 13, // 0x0000000D
    [Obsolete("Use WSA instead")] Metro = 14, // 0x0000000E
    WSA = 14, // 0x0000000E
    [Obsolete("Use WSA instead")] WP8 = 15, // 0x0000000F
    [Obsolete("BlackBerry has been removed as of 5.4")] BlackBerry = 16, // 0x00000010
    Tizen = 17, // 0x00000011
    PSP2 = 18, // 0x00000012
    PS4 = 19, // 0x00000013
    PSM = 20, // 0x00000014
    XboxOne = 21, // 0x00000015
    [Obsolete("SamsungTV has been removed as of 2017.3")] SamsungTV = 22, // 0x00000016
    N3DS = 23, // 0x00000017
    WiiU = 24, // 0x00000018
    tvOS = 25, // 0x00000019
    Facebook = 26, // 0x0000001A
    Switch = 27, // 0x0000001B
  }
}
