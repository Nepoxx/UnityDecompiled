// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureImporterType
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Select this to set basic parameters depending on the purpose of your texture.</para>
  /// </summary>
  public enum TextureImporterType
  {
    Default = 0,
    [Obsolete("Use Default (UnityUpgradable) -> Default")] Image = 0,
    [Obsolete("Use NormalMap (UnityUpgradable) -> NormalMap")] Bump = 1,
    NormalMap = 1,
    GUI = 2,
    [Obsolete("Use importer.textureShape = TextureImporterShape.TextureCube")] Cubemap = 3,
    [Obsolete("Use a texture setup as a cubemap with glossy reflection instead")] Reflection = 3,
    Cookie = 4,
    [Obsolete("Use Default instead. All texture types now have an Advanced foldout (UnityUpgradable) -> Default")] Advanced = 5,
    Lightmap = 6,
    Cursor = 7,
    Sprite = 8,
    [Obsolete("HDRI is not supported anymore")] HDRI = 9,
    SingleChannel = 10, // 0x0000000A
  }
}
