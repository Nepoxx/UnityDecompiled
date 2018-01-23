// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.IDropTarget
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal interface IDropTarget
  {
    bool CanAcceptDrop(List<ISelectable> selection);

    EventPropagation DragUpdated(IMGUIEvent evt, IEnumerable<ISelectable> selection, IDropTarget dropTarget);

    EventPropagation DragPerform(IMGUIEvent evt, IEnumerable<ISelectable> selection, IDropTarget dropTarget);

    EventPropagation DragExited();
  }
}
