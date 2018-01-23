// Decompiled with JetBrains decompiler
// Type: UnityEngine.TrailRenderer
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>The trail renderer is used to make trails behind objects in the scene as they move about.</para>
  /// </summary>
  public sealed class TrailRenderer : Renderer
  {
    /// <summary>
    ///   <para>Set the curve describing the width of the trail at various points along its length.</para>
    /// </summary>
    public extern AnimationCurve widthCurve { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set the color gradient describing the color of the trail at various points along its length.</para>
    /// </summary>
    public extern Gradient colorGradient { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Get the positions of all vertices in the trail.</para>
    /// </summary>
    /// <param name="positions">The array of positions to retrieve.</param>
    /// <returns>
    ///   <para>How many positions were actually stored in the output array.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int GetPositions(Vector3[] positions);

    /// <summary>
    ///   <para>Get the number of line segments in the trail.</para>
    /// </summary>
    [Obsolete("Use positionCount instead (UnityUpgradable) -> positionCount", false)]
    public int numPositions
    {
      get
      {
        return this.positionCount;
      }
    }

    /// <summary>
    ///   <para>How long does the trail take to fade out.</para>
    /// </summary>
    public extern float time { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The width of the trail at the spawning point.</para>
    /// </summary>
    public extern float startWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The width of the trail at the end of the trail.</para>
    /// </summary>
    public extern float endWidth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set an overall multiplier that is applied to the TrailRenderer.widthCurve to get the final width of the trail.</para>
    /// </summary>
    public extern float widthMultiplier { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Does the GameObject of this trail renderer auto destructs?</para>
    /// </summary>
    public extern bool autodestruct { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set this to a value greater than 0, to get rounded corners between each segment of the trail.</para>
    /// </summary>
    public extern int numCornerVertices { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set this to a value greater than 0, to get rounded corners on each end of the trail.</para>
    /// </summary>
    public extern int numCapVertices { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set the minimum distance the trail can travel before a new vertex is added to it.</para>
    /// </summary>
    public extern float minVertexDistance { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set the color at the start of the trail.</para>
    /// </summary>
    public Color startColor
    {
      get
      {
        Color ret;
        this.get_startColor_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_startColor_Injected(ref value);
      }
    }

    /// <summary>
    ///   <para>Set the color at the end of the trail.</para>
    /// </summary>
    public Color endColor
    {
      get
      {
        Color ret;
        this.get_endColor_Injected(out ret);
        return ret;
      }
      set
      {
        this.set_endColor_Injected(ref value);
      }
    }

    /// <summary>
    ///   <para>Get the number of line segments in the trail.</para>
    /// </summary>
    public extern int positionCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get the position of a vertex in the trail.</para>
    /// </summary>
    /// <param name="index">The index of the position to retrieve.</param>
    /// <returns>
    ///   <para>The position at the specified index in the array.</para>
    /// </returns>
    public Vector3 GetPosition(int index)
    {
      Vector3 ret;
      this.GetPosition_Injected(index, out ret);
      return ret;
    }

    /// <summary>
    ///   <para>Configures a trail to generate Normals and Tangents. With this data, Scene lighting can affect the trail via Normal Maps and the Unity Standard Shader, or your own custom-built Shaders.</para>
    /// </summary>
    public extern bool generateLightingData { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Choose whether the U coordinate of the trail texture is tiled or stretched.</para>
    /// </summary>
    public extern LineTextureMode textureMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Select whether the trail will face the camera, or the orientation of the Transform Component.</para>
    /// </summary>
    public extern LineAlignment alignment { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///         <para>Removes all points from the TrailRenderer.
    /// Useful for restarting a trail from a new position.</para>
    ///       </summary>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void Clear();

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_startColor_Injected(out Color ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_startColor_Injected(ref Color value);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void get_endColor_Injected(out Color ret);

    [SpecialName]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void set_endColor_Injected(ref Color value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void GetPosition_Injected(int index, out Vector3 ret);
  }
}
