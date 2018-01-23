// Decompiled with JetBrains decompiler
// Type: UnityEditor.Sprites.IPackerPolicy
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Sprites
{
  public interface IPackerPolicy
  {
    /// <summary>
    ///   <para>Specifies whether sequential processing of atlas tags is enabled. If enabled, sprite packing tags are processed one by one to reduce memory usage.</para>
    /// </summary>
    bool AllowSequentialPacking { get; }

    /// <summary>
    ///   <para>Implement custom atlas grouping here.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="job"></param>
    /// <param name="textureImporterInstanceIDs"></param>
    void OnGroupAtlases(BuildTarget target, PackerJob job, int[] textureImporterInstanceIDs);

    /// <summary>
    ///   <para>Return the version of your policy. Sprite Packer needs to know if atlas grouping logic changed.</para>
    /// </summary>
    int GetVersion();
  }
}
