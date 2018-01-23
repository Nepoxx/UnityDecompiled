// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowHierarchyState
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace UnityEditorInternal
{
  [Serializable]
  internal class AnimationWindowHierarchyState : TreeViewState
  {
    private List<int> m_TallInstanceIDs = new List<int>();

    public bool GetTallMode(AnimationWindowHierarchyNode node)
    {
      return this.m_TallInstanceIDs.Contains(node.id);
    }

    public void SetTallMode(AnimationWindowHierarchyNode node, bool tallMode)
    {
      if (tallMode)
        this.m_TallInstanceIDs.Add(node.id);
      else
        this.m_TallInstanceIDs.Remove(node.id);
    }

    public int GetTallInstancesCount()
    {
      return this.m_TallInstanceIDs.Count;
    }

    public void AddTallInstance(int id)
    {
      if (this.m_TallInstanceIDs.Contains(id))
        return;
      this.m_TallInstanceIDs.Add(id);
    }
  }
}
