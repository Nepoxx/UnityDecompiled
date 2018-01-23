// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.FallbackGraphElement
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class FallbackGraphElement : GraphElement
  {
    public FallbackGraphElement()
    {
      this.style.backgroundColor = (StyleValue<Color>) Color.grey;
      this.text = "";
    }

    public override void OnDataChanged()
    {
      this.text = "Fallback for " + (object) this.GetPresenter<GraphElementPresenter>().GetType() + ". No GraphElement registered for this type in this view.";
    }
  }
}
