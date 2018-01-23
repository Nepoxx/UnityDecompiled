// Decompiled with JetBrains decompiler
// Type: UnityEditor.iOSTargetOSVersion
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Supported iOS deployment versions.</para>
  /// </summary>
  [Obsolete("targetOSVersion is obsolete, use targetOSVersionString", false)]
  public enum iOSTargetOSVersion
  {
    iOS_4_0 = 10, // 0x0000000A
    iOS_4_1 = 12, // 0x0000000C
    iOS_4_2 = 14, // 0x0000000E
    iOS_4_3 = 16, // 0x00000010
    iOS_5_0 = 18, // 0x00000012
    iOS_5_1 = 20, // 0x00000014
    iOS_6_0 = 22, // 0x00000016
    iOS_7_0 = 24, // 0x00000018
    iOS_7_1 = 26, // 0x0000001A
    iOS_8_0 = 28, // 0x0000001C
    iOS_8_1 = 30, // 0x0000001E
    Unknown = 999, // 0x000003E7
  }
}
