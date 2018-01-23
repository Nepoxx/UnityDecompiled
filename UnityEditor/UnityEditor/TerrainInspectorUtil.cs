// Decompiled with JetBrains decompiler
// Type: UnityEditor.TerrainInspectorUtil
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  internal sealed class TerrainInspectorUtil
  {
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float GetTreePlacementSize(TerrainData terrainData, int prototypeIndex, float spacing, float treeCount);

    public static bool CheckTreeDistance(TerrainData terrainData, Vector3 position, int prototypeIndex, float distanceBias)
    {
      return TerrainInspectorUtil.INTERNAL_CALL_CheckTreeDistance(terrainData, ref position, prototypeIndex, distanceBias);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool INTERNAL_CALL_CheckTreeDistance(TerrainData terrainData, ref Vector3 position, int prototypeIndex, float distanceBias);

    public static Vector3 GetPrototypeExtent(TerrainData terrainData, int prototypeIndex)
    {
      Vector3 vector3;
      TerrainInspectorUtil.INTERNAL_CALL_GetPrototypeExtent(terrainData, prototypeIndex, out vector3);
      return vector3;
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_CALL_GetPrototypeExtent(TerrainData terrainData, int prototypeIndex, out Vector3 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetPrototypeCount(TerrainData terrainData);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern bool PrototypeIsRenderable(TerrainData terrainData, int prototypeIndex);
  }
}
