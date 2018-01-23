// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerClipViewBackend
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditorInternal
{
  internal class AudioProfilerClipViewBackend
  {
    public AudioProfilerClipViewBackend.DataUpdateDelegate OnUpdate;
    public AudioProfilerClipTreeViewState m_TreeViewState;

    public AudioProfilerClipViewBackend(AudioProfilerClipTreeViewState state)
    {
      this.m_TreeViewState = state;
      this.items = new List<AudioProfilerClipInfoWrapper>();
    }

    public List<AudioProfilerClipInfoWrapper> items { get; private set; }

    public void SetData(List<AudioProfilerClipInfoWrapper> data)
    {
      this.items = data;
      this.UpdateSorting();
    }

    public void UpdateSorting()
    {
      this.items.Sort((IComparer<AudioProfilerClipInfoWrapper>) new AudioProfilerClipInfoHelper.AudioProfilerClipInfoComparer((AudioProfilerClipInfoHelper.ColumnIndices) this.m_TreeViewState.selectedColumn, (AudioProfilerClipInfoHelper.ColumnIndices) this.m_TreeViewState.prevSelectedColumn, this.m_TreeViewState.sortByDescendingOrder));
      if (this.OnUpdate == null)
        return;
      this.OnUpdate();
    }

    public delegate void DataUpdateDelegate();
  }
}
