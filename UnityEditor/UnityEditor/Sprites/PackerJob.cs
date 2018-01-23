// Decompiled with JetBrains decompiler
// Type: UnityEditor.Sprites.PackerJob
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Sprites
{
  /// <summary>
  ///   <para>Current Sprite Packer job definition.</para>
  /// </summary>
  public sealed class PackerJob
  {
    internal PackerJob()
    {
    }

    /// <summary>
    ///   <para>Registers a new atlas.</para>
    /// </summary>
    /// <param name="atlasName"></param>
    /// <param name="settings"></param>
    public void AddAtlas(string atlasName, AtlasSettings settings)
    {
      this.AddAtlas_Internal(atlasName, ref settings);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void AddAtlas_Internal(string atlasName, ref AtlasSettings settings);

    /// <summary>
    ///   <para>Assigns a Sprite to an already registered atlas.</para>
    /// </summary>
    /// <param name="atlasName"></param>
    /// <param name="sprite"></param>
    /// <param name="packingMode"></param>
    /// <param name="packingRotation"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void AssignToAtlas(string atlasName, Sprite sprite, SpritePackingMode packingMode, SpritePackingRotation packingRotation);
  }
}
