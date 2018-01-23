// Decompiled with JetBrains decompiler
// Type: UnityEditor.DDSImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Texture importer lets you modify Texture2D import settings for DDS textures from editor scripts.</para>
  /// </summary>
  [Obsolete("DDSImporter is obsolete. Use IHVImageFormatImporter instead (UnityUpgradable) -> IHVImageFormatImporter", true)]
  public class DDSImporter : AssetImporter
  {
    /// <summary>
    ///   <para>Is texture data readable from scripts.</para>
    /// </summary>
    public bool isReadable
    {
      get
      {
        return false;
      }
      set
      {
      }
    }
  }
}
