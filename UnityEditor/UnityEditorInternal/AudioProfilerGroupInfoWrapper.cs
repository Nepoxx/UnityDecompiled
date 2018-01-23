// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerGroupInfoWrapper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditorInternal
{
  internal class AudioProfilerGroupInfoWrapper
  {
    public AudioProfilerGroupInfo info;
    public string assetName;
    public string objectName;
    public bool addToRoot;

    public AudioProfilerGroupInfoWrapper(AudioProfilerGroupInfo info, string assetName, string objectName, bool addToRoot)
    {
      this.info = info;
      this.assetName = assetName;
      this.objectName = objectName;
      this.addToRoot = addToRoot;
    }
  }
}
