// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.InputNodeAnchorPresenter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  [Serializable]
  internal class InputNodeAnchorPresenter : NodeAnchorPresenter
  {
    public override Direction direction
    {
      get
      {
        return Direction.Input;
      }
    }
  }
}
