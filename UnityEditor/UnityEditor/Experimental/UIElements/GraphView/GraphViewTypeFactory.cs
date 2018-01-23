// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.GraphViewTypeFactory
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class GraphViewTypeFactory : BaseTypeFactory<GraphElementPresenter, GraphElement>
  {
    public GraphViewTypeFactory()
      : base(typeof (FallbackGraphElement))
    {
    }

    public override GraphElement Create(GraphElementPresenter key)
    {
      GraphElement graphElement = base.Create(key);
      if (graphElement != null)
        graphElement.presenter = key;
      return graphElement;
    }
  }
}
