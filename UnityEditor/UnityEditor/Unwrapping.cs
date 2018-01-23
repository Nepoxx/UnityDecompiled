// Decompiled with JetBrains decompiler
// Type: UnityEditor.Unwrapping
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>This class holds everything you may need in regard to uv-unwrapping.</para>
  /// </summary>
  public sealed class Unwrapping
  {
    /// <summary>
    ///   <para>Will generate per-triangle uv (3 UVs for each triangle) with default settings.</para>
    /// </summary>
    /// <param name="src">The source mesh to generate UVs for.</param>
    /// <returns>
    ///   <para>The list of UVs generated.</para>
    /// </returns>
    public static Vector2[] GeneratePerTriangleUV(Mesh src)
    {
      UnwrapParam settings = new UnwrapParam();
      UnwrapParam.SetDefaults(out settings);
      return Unwrapping.GeneratePerTriangleUV(src, settings);
    }

    /// <summary>
    ///   <para>Will generate per-triangle uv (3 UVs for each triangle) with provided settings.</para>
    /// </summary>
    /// <param name="src">The source mesh to generate UVs for.</param>
    /// <param name="settings">Allows you to specify custom parameters to control the unwrapping.</param>
    /// <returns>
    ///   <para>The list of UVs generated.</para>
    /// </returns>
    public static Vector2[] GeneratePerTriangleUV(Mesh src, UnwrapParam settings)
    {
      return Unwrapping.GeneratePerTriangleUVImpl(src, settings);
    }

    internal static Vector2[] GeneratePerTriangleUVImpl(Mesh src, UnwrapParam settings)
    {
      return Unwrapping.INTERNAL_CALL_GeneratePerTriangleUVImpl(src, ref settings);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern Vector2[] INTERNAL_CALL_GeneratePerTriangleUVImpl(Mesh src, ref UnwrapParam settings);

    /// <summary>
    ///   <para>Will auto generate uv2 with default settings for provided mesh, and fill them in.</para>
    /// </summary>
    /// <param name="src"></param>
    public static void GenerateSecondaryUVSet(Mesh src)
    {
      MeshUtility.SetPerTriangleUV2(src, Unwrapping.GeneratePerTriangleUV(src));
    }

    /// <summary>
    ///   <para>Will auto generate uv2 with provided settings for provided mesh, and fill them in.</para>
    /// </summary>
    /// <param name="src"></param>
    /// <param name="settings"></param>
    public static void GenerateSecondaryUVSet(Mesh src, UnwrapParam settings)
    {
      MeshUtility.SetPerTriangleUV2(src, Unwrapping.GeneratePerTriangleUV(src, settings));
    }
  }
}
