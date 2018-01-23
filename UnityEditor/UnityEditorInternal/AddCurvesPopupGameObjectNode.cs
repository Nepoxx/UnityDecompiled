// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AddCurvesPopupGameObjectNode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class AddCurvesPopupGameObjectNode : TreeViewItem
  {
    public AddCurvesPopupGameObjectNode(GameObject gameObject, TreeViewItem parent, string displayName)
      : base(gameObject.GetInstanceID(), parent == null ? -1 : parent.depth + 1, parent, displayName)
    {
    }
  }
}
