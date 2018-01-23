// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AudioProfilerGroupViewBackend
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditorInternal
{
  internal class AudioProfilerGroupViewBackend
  {
    public AudioProfilerGroupViewBackend.DataUpdateDelegate OnUpdate;
    public AudioProfilerGroupTreeViewState m_TreeViewState;

    public AudioProfilerGroupViewBackend(AudioProfilerGroupTreeViewState state)
    {
      this.m_TreeViewState = state;
      this.items = new List<AudioProfilerGroupInfoWrapper>();
    }

    public List<AudioProfilerGroupInfoWrapper> items { get; private set; }

    public void SetData(List<AudioProfilerGroupInfoWrapper> data)
    {
      this.items = data;
      this.UpdateSorting();
    }

    public void UpdateSorting()
    {
      this.items.Sort((IComparer<AudioProfilerGroupInfoWrapper>) new AudioProfilerGroupInfoHelper.AudioProfilerGroupInfoComparer((AudioProfilerGroupInfoHelper.ColumnIndices) this.m_TreeViewState.selectedColumn, (AudioProfilerGroupInfoHelper.ColumnIndices) this.m_TreeViewState.prevSelectedColumn, this.m_TreeViewState.sortByDescendingOrder));
      if (this.OnUpdate == null)
        return;
      this.OnUpdate();
    }

    public delegate void DataUpdateDelegate();
  }
}
