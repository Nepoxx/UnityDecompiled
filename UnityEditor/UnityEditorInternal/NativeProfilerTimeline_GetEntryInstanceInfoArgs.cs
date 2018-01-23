// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.NativeProfilerTimeline_GetEntryInstanceInfoArgs
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditorInternal
{
  public struct NativeProfilerTimeline_GetEntryInstanceInfoArgs
  {
    public int frameIndex;
    public int threadIndex;
    public int entryIndex;
    public int out_Id;
    public string out_Path;
    public string out_CallstackInfo;
    public string out_MetaData;

    public void Reset()
    {
      this.frameIndex = -1;
      this.threadIndex = -1;
      this.entryIndex = -1;
      this.out_Id = 0;
      this.out_Path = string.Empty;
      this.out_CallstackInfo = string.Empty;
      this.out_MetaData = string.Empty;
    }
  }
}
