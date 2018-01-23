// Decompiled with JetBrains decompiler
// Type: UnityEditor.VertexChannelCompressionFlags
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>This enum is used to build a bitmask for controlling per-channel vertex compression.</para>
  /// </summary>
  [System.Flags]
  public enum VertexChannelCompressionFlags
  {
    kPosition = 1,
    kNormal = 2,
    kColor = 4,
    kUV0 = 8,
    kUV1 = 16, // 0x00000010
    kUV2 = 32, // 0x00000020
    kUV3 = 64, // 0x00000040
    kTangent = 128, // 0x00000080
  }
}
