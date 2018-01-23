// Decompiled with JetBrains decompiler
// Type: UnityEditor.AudioImporterChannels
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  [Obsolete("Setting and getting import channels is not used anymore (use forceToMono instead)", true)]
  public enum AudioImporterChannels
  {
    Automatic,
    Mono,
    Stereo,
  }
}
