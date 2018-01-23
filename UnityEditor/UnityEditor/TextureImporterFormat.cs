// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureImporterFormat
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Imported texture format for TextureImporter.</para>
  /// </summary>
  public enum TextureImporterFormat
  {
    [Obsolete("HDR is handled automatically now")] AutomaticCompressedHDR = -7,
    [Obsolete("HDR is handled automatically now")] AutomaticHDR = -6,
    [Obsolete("Use crunchedCompression property instead")] AutomaticCrunched = -5,
    [Obsolete("Use textureCompression property instead")] AutomaticTruecolor = -3,
    [Obsolete("Use textureCompression property instead")] Automatic16bit = -2,
    Automatic = -1,
    [Obsolete("Use textureCompression property instead")] AutomaticCompressed = -1,
    Alpha8 = 1,
    ARGB16 = 2,
    RGB24 = 3,
    RGBA32 = 4,
    ARGB32 = 5,
    RGB16 = 7,
    DXT1 = 10, // 0x0000000A
    DXT5 = 12, // 0x0000000C
    RGBA16 = 13, // 0x0000000D
    RGBAHalf = 17, // 0x00000011
    BC6H = 24, // 0x00000018
    BC7 = 25, // 0x00000019
    BC4 = 26, // 0x0000001A
    BC5 = 27, // 0x0000001B
    DXT1Crunched = 28, // 0x0000001C
    DXT5Crunched = 29, // 0x0000001D
    PVRTC_RGB2 = 30, // 0x0000001E
    PVRTC_RGBA2 = 31, // 0x0000001F
    PVRTC_RGB4 = 32, // 0x00000020
    PVRTC_RGBA4 = 33, // 0x00000021
    ETC_RGB4 = 34, // 0x00000022
    ATC_RGB4 = 35, // 0x00000023
    ATC_RGBA8 = 36, // 0x00000024
    EAC_R = 41, // 0x00000029
    EAC_R_SIGNED = 42, // 0x0000002A
    EAC_RG = 43, // 0x0000002B
    EAC_RG_SIGNED = 44, // 0x0000002C
    ETC2_RGB4 = 45, // 0x0000002D
    ETC2_RGB4_PUNCHTHROUGH_ALPHA = 46, // 0x0000002E
    ETC2_RGBA8 = 47, // 0x0000002F
    ASTC_RGB_4x4 = 48, // 0x00000030
    ASTC_RGB_5x5 = 49, // 0x00000031
    ASTC_RGB_6x6 = 50, // 0x00000032
    ASTC_RGB_8x8 = 51, // 0x00000033
    ASTC_RGB_10x10 = 52, // 0x00000034
    ASTC_RGB_12x12 = 53, // 0x00000035
    ASTC_RGBA_4x4 = 54, // 0x00000036
    ASTC_RGBA_5x5 = 55, // 0x00000037
    ASTC_RGBA_6x6 = 56, // 0x00000038
    ASTC_RGBA_8x8 = 57, // 0x00000039
    ASTC_RGBA_10x10 = 58, // 0x0000003A
    ASTC_RGBA_12x12 = 59, // 0x0000003B
    ETC_RGB4Crunched = 64, // 0x00000040
    ETC2_RGBA8Crunched = 65, // 0x00000041
  }
}
