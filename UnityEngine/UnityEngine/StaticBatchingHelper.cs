// Decompiled with JetBrains decompiler
// Type: UnityEngine.StaticBatchingHelper
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  internal struct StaticBatchingHelper
  {
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Mesh InternalCombineVertices(MeshSubsetCombineUtility.MeshInstance[] meshes, string meshName);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void InternalCombineIndices(MeshSubsetCombineUtility.SubMeshInstance[] submeshes, Mesh combinedMesh);
  }
}
