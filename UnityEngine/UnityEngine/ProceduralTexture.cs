// Decompiled with JetBrains decompiler
// Type: UnityEngine.ProceduralTexture
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Class for ProceduralTexture handling.</para>
  /// </summary>
  [Obsolete("Built-in support for Substance Designer materials has been deprecated and will be removed in Unity 2018.1. To continue using Substance Designer materials in Unity 2018.1, you will need to install a suitable third-party external importer from the Asset Store.", false)]
  public sealed class ProceduralTexture : Texture
  {
    /// <summary>
    ///   <para>The output type of this ProceduralTexture.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern ProceduralOutputType GetProceduralOutputType();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern ProceduralMaterial GetProceduralMaterial();

    /// <summary>
    ///   <para>Check whether the ProceduralMaterial that generates this ProceduralTexture is set to an output format with an alpha channel.</para>
    /// </summary>
    public extern bool hasAlpha { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool HasBeenGenerated();

    /// <summary>
    ///   <para>The format of the pixel data in the texture (Read Only).</para>
    /// </summary>
    public extern TextureFormat format { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///         <para>Grab pixel values from a ProceduralTexture.
    /// </para>
    ///       </summary>
    /// <param name="x">X-coord of the top-left corner of the rectangle to grab.</param>
    /// <param name="y">Y-coord of the top-left corner of the rectangle to grab.</param>
    /// <param name="blockWidth">Width of rectangle to grab.</param>
    /// <param name="blockHeight">Height of the rectangle to grab.
    /// Get the pixel values from a rectangular area of a ProceduralTexture into an array.
    /// The block is specified by its x,y offset in the texture and by its width and height. The block is "flattened" into the array by scanning the pixel values across rows one by one.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Color32[] GetPixels32(int x, int y, int blockWidth, int blockHeight);
  }
}
