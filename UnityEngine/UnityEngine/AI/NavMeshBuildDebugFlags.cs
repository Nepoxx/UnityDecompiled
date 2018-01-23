// Decompiled with JetBrains decompiler
// Type: UnityEngine.AI.NavMeshBuildDebugFlags
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.AI
{
  /// <summary>
  ///   <para>Bitmask used for operating with debug data from the NavMesh build process.</para>
  /// </summary>
  [Flags]
  public enum NavMeshBuildDebugFlags
  {
    None = 0,
    InputGeometry = 1,
    Voxels = 2,
    Regions = 4,
    RawContours = 8,
    SimplifiedContours = 16, // 0x00000010
    PolygonMeshes = 32, // 0x00000020
    PolygonMeshesDetail = 64, // 0x00000040
    All = PolygonMeshesDetail | PolygonMeshes | SimplifiedContours | RawContours | Regions | Voxels | InputGeometry, // 0x0000007F
  }
}
