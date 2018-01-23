// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.ProfilerCaptureFlags
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditorInternal
{
  [Flags]
  public enum ProfilerCaptureFlags
  {
    None = 0,
    Channels = 1,
    DSPNodes = 2,
    Clips = 4,
    All = Clips | DSPNodes | Channels, // 0x00000007
  }
}
