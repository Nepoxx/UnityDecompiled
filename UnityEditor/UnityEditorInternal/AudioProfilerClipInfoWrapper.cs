// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerClipInfoWrapper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditorInternal
{
  internal class AudioProfilerClipInfoWrapper
  {
    public AudioProfilerClipInfo info;
    public string assetName;

    public AudioProfilerClipInfoWrapper(AudioProfilerClipInfo info, string assetName)
    {
      this.info = info;
      this.assetName = assetName;
    }
  }
}
