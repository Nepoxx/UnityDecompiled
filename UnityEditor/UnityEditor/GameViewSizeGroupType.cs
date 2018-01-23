// Decompiled with JetBrains decompiler
// Type: UnityEditor.GameViewSizeGroupType
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor
{
  public enum GameViewSizeGroupType
  {
    Standalone,
    [Obsolete("WebPlayer has been removed in 5.4")] WebPlayer,
    iOS,
    Android,
    [Obsolete("PS3 has been removed in 5.5", false)] PS3,
    WiiU,
    Tizen,
    [Obsolete("Windows Phone 8 was removed in 5.3", false)] WP8,
    N3DS,
    HMD,
  }
}
