// Decompiled with JetBrains decompiler
// Type: UnityEditor.WaveformPreviewFactory
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal static class WaveformPreviewFactory
  {
    public static WaveformPreview Create(int initialSize, AudioClip clip)
    {
      return (WaveformPreview) new StreamedAudioClipPreview(clip, initialSize);
    }
  }
}
