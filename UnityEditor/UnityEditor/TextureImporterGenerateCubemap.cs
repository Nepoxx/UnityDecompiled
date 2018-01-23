// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureImporterGenerateCubemap
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Cubemap generation mode for TextureImporter.</para>
  /// </summary>
  public enum TextureImporterGenerateCubemap
  {
    [Obsolete("This value is deprecated (use TextureImporter.textureShape instead).")] None,
    Spheremap,
    Cylindrical,
    [Obsolete("Obscure shperemap modes are not supported any longer (use TextureImporterGenerateCubemap.Spheremap instead).")] SimpleSpheremap,
    [Obsolete("Obscure shperemap modes are not supported any longer (use TextureImporterGenerateCubemap.Spheremap instead).")] NiceSpheremap,
    FullCubemap,
    AutoCubemap,
  }
}
