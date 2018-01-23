// Decompiled with JetBrains decompiler
// Type: UnityEditor.UnwrapParam
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Unwrapping settings.</para>
  /// </summary>
  public struct UnwrapParam
  {
    /// <summary>
    ///   <para>Maximum allowed angle distortion (0..1).</para>
    /// </summary>
    public float angleError;
    /// <summary>
    ///   <para>Maximum allowed area distortion (0..1).</para>
    /// </summary>
    public float areaError;
    /// <summary>
    ///   <para>This angle (in degrees) or greater between triangles will cause seam to be created.</para>
    /// </summary>
    public float hardAngle;
    /// <summary>
    ///   <para>How much uv-islands will be padded.</para>
    /// </summary>
    public float packMargin;
    internal int recollectVertices;

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void SetDefaults(out UnwrapParam param);
  }
}
