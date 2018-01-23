// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.Build.AssetBundle.BuildCompression
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.AssetBundle
{
  [UsedByNativeCode]
  [Serializable]
  public struct BuildCompression
  {
    public static readonly BuildCompression DefaultUncompressed = new BuildCompression() { compression = CompressionType.None, level = CompressionLevel.Maximum, blockSize = 131072 };
    public static readonly BuildCompression DefaultLZ4 = new BuildCompression() { compression = CompressionType.Lz4HC, level = CompressionLevel.Maximum, blockSize = 131072 };
    public static readonly BuildCompression DefaultLZMA = new BuildCompression() { compression = CompressionType.Lzma, level = CompressionLevel.Maximum, blockSize = 131072 };
    public CompressionType compression;
    public CompressionLevel level;
    public uint blockSize;
  }
}
