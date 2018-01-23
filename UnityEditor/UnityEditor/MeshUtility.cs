// Decompiled with JetBrains decompiler
// Type: UnityEditor.MeshUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Various utilities for mesh manipulation.</para>
  /// </summary>
  public sealed class MeshUtility
  {
    /// <summary>
    ///   <para>Will insert per-triangle uv2 in mesh and handle vertex splitting etc.</para>
    /// </summary>
    /// <param name="src"></param>
    /// <param name="triUV"></param>
    public static void SetPerTriangleUV2(Mesh src, Vector2[] triUV)
    {
      int num = InternalMeshUtil.CalcTriangleCount(src);
      int length = triUV.Length;
      if (length != 3 * num)
        Debug.LogError((object) ("mesh contains " + (object) num + " triangles but " + (object) length + " uvs are provided"));
      else
        MeshUtility.SetPerTriangleUV2NoCheck(src, triUV);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void SetPerTriangleUV2NoCheck(Mesh src, Vector2[] triUV);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Vector2[] ComputeTextureBoundingHull(Texture texture, int vertexCount);

    /// <summary>
    ///   <para>Change the mesh compression setting for a mesh.</para>
    /// </summary>
    /// <param name="mesh">The mesh to set the compression mode for.</param>
    /// <param name="compression">The compression mode to set.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetMeshCompression(Mesh mesh, ModelImporterMeshCompression compression);

    /// <summary>
    ///   <para>Returns the mesh compression setting for a Mesh.</para>
    /// </summary>
    /// <param name="mesh">The mesh to get information on.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern ModelImporterMeshCompression GetMeshCompression(Mesh mesh);

    /// <summary>
    ///   <para>Optimizes the mesh for GPU access.</para>
    /// </summary>
    /// <param name="mesh"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Optimize(Mesh mesh);
  }
}
