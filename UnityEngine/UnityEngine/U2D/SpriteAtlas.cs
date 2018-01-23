// Decompiled with JetBrains decompiler
// Type: UnityEngine.U2D.SpriteAtlas
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.U2D
{
  /// <summary>
  ///   <para>Sprite Atlas is an asset created within Unity. It is part of the built-in sprite packing solution.</para>
  /// </summary>
  public sealed class SpriteAtlas : Object
  {
    internal SpriteAtlas()
    {
      SpriteAtlas.Internal_Create(this);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void Internal_Create([Writable] SpriteAtlas mono);

    /// <summary>
    ///   <para>Return true if this SpriteAtlas is a variant.</para>
    /// </summary>
    public extern bool isVariant { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get the tag of this SpriteAtlas.</para>
    /// </summary>
    public extern string tag { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Get the total number of Sprite packed into this atlas.</para>
    /// </summary>
    public extern int spriteCount { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Clone all the Sprite in this atlas and fill them into the supplied array.</para>
    /// </summary>
    /// <param name="sprites">Array of Sprite that will be filled.</param>
    /// <returns>
    ///   <para>The size of the returned array.</para>
    /// </returns>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int GetSprites(Sprite[] sprites);

    /// <summary>
    ///   <para>Clone all the Sprite matching the name in this atlas and fill them into the supplied array.</para>
    /// </summary>
    /// <param name="sprites">Array of Sprite that will be filled.</param>
    /// <param name="name">The name of the Sprite.</param>
    public int GetSprites(Sprite[] sprites, string name)
    {
      return this.GetSpritesByName(sprites, name);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern int GetSpritesByName(Sprite[] sprites, string name);

    /// <summary>
    ///   <para>Clone the first Sprite in this atlas that matches the name packed in this atlas and return it.</para>
    /// </summary>
    /// <param name="name">The name of the Sprite.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Sprite GetSprite(string name);
  }
}
