// Decompiled with JetBrains decompiler
// Type: UnityEditor.StaticEditorFlags
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Static Editor Flags.</para>
  /// </summary>
  [System.Flags]
  public enum StaticEditorFlags
  {
    LightmapStatic = 1,
    OccluderStatic = 2,
    OccludeeStatic = 16, // 0x00000010
    BatchingStatic = 4,
    NavigationStatic = 8,
    OffMeshLinkGeneration = 32, // 0x00000020
    ReflectionProbeStatic = 64, // 0x00000040
  }
}
