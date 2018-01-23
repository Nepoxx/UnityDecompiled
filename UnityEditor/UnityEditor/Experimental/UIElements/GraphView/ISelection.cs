// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.ISelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal interface ISelection
  {
    List<ISelectable> selection { get; }

    void AddToSelection(ISelectable selectable);

    void RemoveFromSelection(ISelectable selectable);

    void ClearSelection();
  }
}
