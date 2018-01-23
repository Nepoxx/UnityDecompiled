// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioImporterLoadType
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.ComponentModel;

namespace UnityEditor
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Obsolete("UnityEditor.AudioImporterLoadType has been deprecated. Use UnityEngine.AudioClipLoadType instead (UnityUpgradable) -> [UnityEngine] UnityEngine.AudioClipLoadType", true)]
  public enum AudioImporterLoadType
  {
    CompressedInMemory = -1,
    DecompressOnLoad = -1,
    [Obsolete("UnityEditor.AudioImporterLoadType.StreamFromDisc has been deprecated. Use UnityEngine.AudioClipLoadType.Streaming instead (UnityUpgradable) -> UnityEngine.AudioClipLoadType.Streaming", true)] StreamFromDisc = -1,
  }
}
