// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.ISelectable
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal interface ISelectable
  {
    bool IsSelectable();

    bool Overlaps(Rect rectangle);

    void Select(UnityEditor.Experimental.UIElements.GraphView.GraphView selectionContainer, bool additive);

    void Unselect(UnityEditor.Experimental.UIElements.GraphView.GraphView selectionContainer);

    bool IsSelected(UnityEditor.Experimental.UIElements.GraphView.GraphView selectionContainer);
  }
}
