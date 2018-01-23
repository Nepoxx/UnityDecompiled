// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.GraphView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal abstract class GraphView : DataWatchContainer, ISelection
  {
    private bool m_FrameAnimate = false;
    private readonly Dictionary<int, UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer> m_ContainerLayers = new Dictionary<int, UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer>();
    private int m_ZoomerMaxElementCountWithPixelCacheRegen = 100;
    private Vector3 m_MinScale = ContentZoomer.DefaultMinScale;
    private Vector3 m_MaxScale = ContentZoomer.DefaultMaxScale;
    private GraphViewPresenter m_Presenter;
    private UnityEditor.Experimental.UIElements.GraphView.GraphView.PersistedViewTransform m_PersistedViewTransform;
    private ContentZoomer m_Zoomer;

    protected GraphView()
    {
      this.selection = new List<ISelectable>();
      this.clippingOptions = VisualElement.ClippingOptions.ClipContents;
      UnityEditor.Experimental.UIElements.GraphView.GraphView.ContentViewContainer contentViewContainer = new UnityEditor.Experimental.UIElements.GraphView.GraphView.ContentViewContainer();
      contentViewContainer.name = nameof (contentViewContainer);
      contentViewContainer.clippingOptions = VisualElement.ClippingOptions.NoClipping;
      contentViewContainer.pickingMode = PickingMode.Ignore;
      this.contentViewContainer = (VisualElement) contentViewContainer;
      this.Add(this.contentViewContainer);
      this.typeFactory = new GraphViewTypeFactory();
      this.typeFactory[typeof (EdgePresenter)] = typeof (Edge);
      this.AddStyleSheetPath("StyleSheets/GraphView/GraphView.uss");
      this.graphElements = this.contentViewContainer.Query().Children<UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer>((string) null, (string) null).Children<GraphElement>((string) null, (string) null).Build();
      this.nodes = this.Query<UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer>((string) null, (string) null).Children<Node>((string) null, (string) null).Build();
    }

    public T GetPresenter<T>() where T : GraphViewPresenter
    {
      return this.presenter as T;
    }

    public GraphViewPresenter presenter
    {
      get
      {
        return this.m_Presenter;
      }
      set
      {
        if ((UnityEngine.Object) this.m_Presenter == (UnityEngine.Object) value)
          return;
        this.RemoveWatch();
        this.m_Presenter = value;
        this.OnDataChanged();
        this.AddWatch();
      }
    }

    protected GraphViewTypeFactory typeFactory { get; set; }

    public VisualElement contentViewContainer { get; private set; }

    public VisualElement viewport
    {
      get
      {
        return (VisualElement) this;
      }
    }

    public ITransform viewTransform
    {
      get
      {
        return this.contentViewContainer.transform;
      }
    }

    public void UpdateViewTransform(Vector3 newPosition, Vector3 newScale)
    {
      this.contentViewContainer.transform.position = newPosition;
      this.contentViewContainer.transform.scale = newScale;
      if ((UnityEngine.Object) this.m_Presenter != (UnityEngine.Object) null)
      {
        this.m_Presenter.position = newPosition;
        this.m_Presenter.scale = newScale;
      }
      this.UpdatePersistedViewTransform();
    }

    private void AddLayer(int index)
    {
      Dictionary<int, UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer> containerLayers = this.m_ContainerLayers;
      int key = index;
      UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer layer1 = new UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer();
      layer1.clippingOptions = VisualElement.ClippingOptions.NoClipping;
      layer1.pickingMode = PickingMode.Ignore;
      UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer layer2 = layer1;
      containerLayers.Add(key, layer2);
      foreach (UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer layer3 in this.m_ContainerLayers.OrderBy<KeyValuePair<int, UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer>, int>((Func<KeyValuePair<int, UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer>, int>) (t => t.Key)).Select<KeyValuePair<int, UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer>, UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer>((Func<KeyValuePair<int, UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer>, UnityEditor.Experimental.UIElements.GraphView.GraphView.Layer>) (t => t.Value)))
      {
        if (layer3.parent != null)
          this.contentViewContainer.Remove((VisualElement) layer3);
        this.contentViewContainer.Add((VisualElement) layer3);
      }
    }

    private VisualElement GetLayer(int index)
    {
      return (VisualElement) this.m_ContainerLayers[index];
    }

    public UQuery.QueryState<GraphElement> graphElements { get; private set; }

    public UQuery.QueryState<Node> nodes { get; private set; }

    public Vector3 minScale
    {
      get
      {
        return this.m_MinScale;
      }
    }

    public Vector3 maxScale
    {
      get
      {
        return this.m_MaxScale;
      }
    }

    public float scale
    {
      get
      {
        return this.viewTransform.scale.x;
      }
    }

    public int zoomerMaxElementCountWithPixelCacheRegen
    {
      get
      {
        return this.m_ZoomerMaxElementCountWithPixelCacheRegen;
      }
      set
      {
        if (this.m_ZoomerMaxElementCountWithPixelCacheRegen == value)
          return;
        this.m_ZoomerMaxElementCountWithPixelCacheRegen = value;
        if (!((UnityEngine.Object) this.m_Presenter != (UnityEngine.Object) null))
          return;
        this.m_Zoomer.keepPixelCacheOnZoom = this.m_Presenter.elements.Count<GraphElementPresenter>() > this.m_ZoomerMaxElementCountWithPixelCacheRegen;
      }
    }

    public void SetupZoom(Vector3 minScaleSetup, Vector3 maxScaleSetup)
    {
      this.m_MinScale = minScaleSetup;
      this.m_MaxScale = maxScaleSetup;
      this.UpdateContentZoomer();
    }

    private void UpdatePersistedViewTransform()
    {
      if (this.m_PersistedViewTransform == null)
        return;
      this.m_PersistedViewTransform.position = this.contentViewContainer.transform.position;
      this.m_PersistedViewTransform.scale = this.contentViewContainer.transform.scale;
      this.SavePersistentData();
    }

    public override void OnPersistentDataReady()
    {
      base.OnPersistentDataReady();
      this.m_PersistedViewTransform = this.GetOrCreatePersistentData<UnityEditor.Experimental.UIElements.GraphView.GraphView.PersistedViewTransform>((object) this.m_PersistedViewTransform, this.GetFullHierarchicalPersistenceKey());
      this.UpdateViewTransform(this.m_PersistedViewTransform.position, this.m_PersistedViewTransform.scale);
    }

    private void UpdateContentZoomer()
    {
      if (this.m_MinScale != this.m_MaxScale)
      {
        if (this.m_Zoomer == null)
        {
          this.m_Zoomer = new ContentZoomer(this.m_MinScale, this.m_MaxScale);
          this.AddManipulator((IManipulator) this.m_Zoomer);
        }
        else
        {
          this.m_Zoomer.minScale = this.m_MinScale;
          this.m_Zoomer.maxScale = this.m_MaxScale;
        }
      }
      else if (this.m_Zoomer != null)
        this.RemoveManipulator((IManipulator) this.m_Zoomer);
      this.ValidateTransform();
    }

    private void ValidateTransform()
    {
      if (this.contentViewContainer == null)
        return;
      Vector3 scale = this.viewTransform.scale;
      scale.x = Mathf.Max(Mathf.Min(this.maxScale.x, scale.x), this.minScale.x);
      scale.y = Mathf.Max(Mathf.Min(this.maxScale.y, scale.y), this.minScale.y);
      this.viewTransform.scale = scale;
    }

    public override void OnDataChanged()
    {
      if ((UnityEngine.Object) this.m_Presenter == (UnityEngine.Object) null)
        return;
      this.contentViewContainer.transform.position = this.m_Presenter.position;
      this.contentViewContainer.transform.scale = !(this.m_Presenter.scale != Vector3.zero) ? Vector3.one : this.m_Presenter.scale;
      this.ValidateTransform();
      this.UpdatePersistedViewTransform();
      List<GraphElement> list = this.graphElements.ToList();
      foreach (GraphElement graphElement in list)
      {
        if (!this.m_Presenter.elements.Contains<GraphElementPresenter>(graphElement.presenter))
        {
          graphElement.parent.Remove((VisualElement) graphElement);
          this.selection.Remove((ISelectable) graphElement);
        }
      }
      int num = 0;
      foreach (GraphElementPresenter element in this.m_Presenter.elements)
      {
        ++num;
        bool flag = false;
        if ((element.capabilities & Capabilities.Floating) == (Capabilities) 0)
        {
          foreach (GraphElement graphElement in list)
          {
            if (graphElement != null && (UnityEngine.Object) graphElement.presenter == (UnityEngine.Object) element)
            {
              flag = true;
              break;
            }
          }
        }
        else
        {
          foreach (VisualElement child in this.Children())
          {
            if (child != this.contentViewContainer)
            {
              GraphElement graphElement = child as GraphElement;
              if (graphElement != null && (UnityEngine.Object) graphElement.presenter == (UnityEngine.Object) element)
              {
                flag = true;
                break;
              }
            }
          }
        }
        if (!flag)
          this.InstantiateElement(element);
      }
      this.m_Zoomer.keepPixelCacheOnZoom = num > this.m_ZoomerMaxElementCountWithPixelCacheRegen;
    }

    protected override UnityEngine.Object[] toWatch
    {
      get
      {
        return new UnityEngine.Object[1]{ (UnityEngine.Object) this.presenter };
      }
    }

    public List<ISelectable> selection { get; protected set; }

    public virtual void AddToSelection(ISelectable selectable)
    {
      GraphElement graphElement = selectable as GraphElement;
      if (graphElement == null)
        return;
      graphElement.OnSelected();
      if ((UnityEngine.Object) graphElement.presenter != (UnityEngine.Object) null)
        graphElement.presenter.selected = true;
      this.selection.Add(selectable);
      this.contentViewContainer.Dirty(ChangeType.Repaint);
    }

    public virtual void RemoveFromSelection(ISelectable selectable)
    {
      GraphElement graphElement = selectable as GraphElement;
      if (graphElement == null)
        return;
      if ((UnityEngine.Object) graphElement.presenter != (UnityEngine.Object) null)
        graphElement.presenter.selected = false;
      this.selection.Remove(selectable);
      this.contentViewContainer.Dirty(ChangeType.Repaint);
    }

    public virtual void ClearSelection()
    {
      foreach (GraphElement graphElement in this.selection.OfType<GraphElement>())
      {
        if ((UnityEngine.Object) graphElement.presenter != (UnityEngine.Object) null)
          graphElement.presenter.selected = false;
      }
      this.selection.Clear();
      this.contentViewContainer.Dirty(ChangeType.Repaint);
    }

    private void InstantiateElement(GraphElementPresenter elementPresenter)
    {
      GraphElement graphElement = this.typeFactory.Create(elementPresenter);
      if (graphElement == null)
        return;
      graphElement.SetPosition(elementPresenter.position);
      graphElement.presenter = elementPresenter;
      if ((elementPresenter.capabilities & Capabilities.Resizable) != (Capabilities) 0)
      {
        graphElement.Add((VisualElement) new Resizer());
        graphElement.style.borderBottom = (StyleValue<float>) 6f;
      }
      if ((elementPresenter.capabilities & Capabilities.Floating) == (Capabilities) 0)
      {
        int layer = graphElement.layer;
        if (!this.m_ContainerLayers.ContainsKey(layer))
          this.AddLayer(layer);
        this.GetLayer(layer).Add((VisualElement) graphElement);
      }
      else
        this.Add((VisualElement) graphElement);
    }

    public EventPropagation DeleteSelection()
    {
      if ((UnityEngine.Object) this.presenter == (UnityEngine.Object) null)
        return EventPropagation.Stop;
      HashSet<GraphElementPresenter> source = new HashSet<GraphElementPresenter>();
      foreach (GraphElement graphElement in this.selection.Cast<GraphElement>().Where<GraphElement>((Func<GraphElement, bool>) (e => e != null && (UnityEngine.Object) e.presenter != (UnityEngine.Object) null)))
      {
        if ((graphElement.presenter.capabilities & Capabilities.Deletable) != (Capabilities) 0)
        {
          source.Add(graphElement.presenter);
          NodePresenter presenter = graphElement.GetPresenter<NodePresenter>();
          if (!((UnityEngine.Object) presenter == (UnityEngine.Object) null))
          {
            source.UnionWith(presenter.inputAnchors.SelectMany<NodeAnchorPresenter, EdgePresenter>((Func<NodeAnchorPresenter, IEnumerable<EdgePresenter>>) (c => c.connections)).Where<EdgePresenter>((Func<EdgePresenter, bool>) (d => (d.capabilities & Capabilities.Deletable) != (Capabilities) 0)).Cast<GraphElementPresenter>());
            source.UnionWith(presenter.outputAnchors.SelectMany<NodeAnchorPresenter, EdgePresenter>((Func<NodeAnchorPresenter, IEnumerable<EdgePresenter>>) (c => c.connections)).Where<EdgePresenter>((Func<EdgePresenter, bool>) (d => (d.capabilities & Capabilities.Deletable) != (Capabilities) 0)).Cast<GraphElementPresenter>());
          }
        }
      }
      foreach (GraphElementPresenter element in source)
        this.presenter.RemoveElement(element);
      foreach (EdgePresenter edgePresenter in source.OfType<EdgePresenter>())
      {
        edgePresenter.output = (NodeAnchorPresenter) null;
        edgePresenter.input = (NodeAnchorPresenter) null;
        if ((UnityEngine.Object) edgePresenter.output != (UnityEngine.Object) null)
          edgePresenter.output.Disconnect(edgePresenter);
        if ((UnityEngine.Object) edgePresenter.input != (UnityEngine.Object) null)
          edgePresenter.input.Disconnect(edgePresenter);
      }
      return source.Count <= 0 ? EventPropagation.Continue : EventPropagation.Stop;
    }

    public EventPropagation FrameAll()
    {
      return this.Frame(UnityEditor.Experimental.UIElements.GraphView.GraphView.FrameType.All);
    }

    public EventPropagation FrameSelection()
    {
      return this.Frame(UnityEditor.Experimental.UIElements.GraphView.GraphView.FrameType.Selection);
    }

    public EventPropagation FrameOrigin()
    {
      return this.Frame(UnityEditor.Experimental.UIElements.GraphView.GraphView.FrameType.Origin);
    }

    public EventPropagation FramePrev()
    {
      if (this.contentViewContainer.childCount == 0)
        return EventPropagation.Continue;
      List<GraphElement> list = this.graphElements.ToList();
      list.Reverse();
      return this.FramePrevNext(list);
    }

    public EventPropagation FrameNext()
    {
      if (this.contentViewContainer.childCount == 0)
        return EventPropagation.Continue;
      return this.FramePrevNext(this.graphElements.ToList());
    }

    private EventPropagation FramePrevNext(List<GraphElement> childrenEnum)
    {
      GraphElement graphElement = (GraphElement) null;
      if (this.selection.Count != 0)
        graphElement = this.selection[0] as GraphElement;
      for (int index = 0; index < childrenEnum.Count; ++index)
      {
        if (childrenEnum[index] == graphElement)
        {
          graphElement = index >= childrenEnum.Count - 1 ? childrenEnum[0] : childrenEnum[index + 1];
          break;
        }
      }
      if (graphElement == null)
        return EventPropagation.Continue;
      this.ClearSelection();
      this.AddToSelection((ISelectable) graphElement);
      return this.Frame(UnityEditor.Experimental.UIElements.GraphView.GraphView.FrameType.Selection);
    }

    private EventPropagation Frame(UnityEditor.Experimental.UIElements.GraphView.GraphView.FrameType frameType)
    {
      this.contentViewContainer.transform.position = Vector3.zero;
      this.contentViewContainer.transform.scale = Vector3.one;
      this.contentViewContainer.Dirty(ChangeType.Repaint);
      if (frameType == UnityEditor.Experimental.UIElements.GraphView.GraphView.FrameType.Origin)
        return EventPropagation.Stop;
      Rect seed = this.contentViewContainer.layout;
      Rect rectToFit;
      if (frameType == UnityEditor.Experimental.UIElements.GraphView.GraphView.FrameType.Selection)
      {
        if (this.selection.Count == 0)
          return EventPropagation.Continue;
        GraphElement graphElement = this.selection[0] as GraphElement;
        if (graphElement != null)
          seed = graphElement.localBound;
        rectToFit = this.selection.OfType<GraphElement>().Aggregate<GraphElement, Rect>(seed, (Func<Rect, GraphElement, Rect>) ((current, e) => RectUtils.Encompass(current, e.localBound)));
      }
      else
        rectToFit = this.CalculateRectToFitAll();
      int border = 30;
      Vector3 frameTranslation;
      Vector3 frameScaling;
      UnityEditor.Experimental.UIElements.GraphView.GraphView.CalculateFrameTransform(rectToFit, this.layout, border, out frameTranslation, out frameScaling);
      if (!this.m_FrameAnimate)
      {
        Matrix4x4.TRS(frameTranslation, Quaternion.identity, frameScaling);
        this.UpdateViewTransform(frameTranslation, frameScaling);
      }
      this.contentViewContainer.Dirty(ChangeType.Repaint);
      this.UpdatePersistedViewTransform();
      return EventPropagation.Stop;
    }

    public Rect CalculateRectToFitAll()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UnityEditor.Experimental.UIElements.GraphView.GraphView.\u003CCalculateRectToFitAll\u003Ec__AnonStorey0 fitAllCAnonStorey0 = new UnityEditor.Experimental.UIElements.GraphView.GraphView.\u003CCalculateRectToFitAll\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      fitAllCAnonStorey0.rectToFit = this.contentViewContainer.layout;
      // ISSUE: reference to a compiler-generated field
      fitAllCAnonStorey0.reachedFirstChild = false;
      // ISSUE: reference to a compiler-generated method
      this.graphElements.ForEach(new System.Action<GraphElement>(fitAllCAnonStorey0.\u003C\u003Em__0));
      // ISSUE: reference to a compiler-generated field
      return fitAllCAnonStorey0.rectToFit;
    }

    public static void CalculateFrameTransform(Rect rectToFit, Rect clientRect, int border, out Vector3 frameTranslation, out Vector3 frameScaling)
    {
      Rect screenRect = new Rect() { xMin = (float) border, xMax = clientRect.width - (float) border, yMin = (float) border, yMax = clientRect.height - (float) border };
      Matrix4x4 matrix = GUI.matrix;
      GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
      Rect guiRect = GUIUtility.ScreenToGUIRect(screenRect);
      float num = Mathf.Clamp(Math.Min(guiRect.width / rectToFit.width, guiRect.height / rectToFit.height), ContentZoomer.DefaultMinScale.y, 1f);
      Matrix4x4 matrix4x4 = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(num, num, 1f));
      Vector2 vector2_1 = new Vector2(clientRect.width, clientRect.height);
      Vector2 vector2_2 = new Vector2(0.0f, 0.0f);
      Rect rect = new Rect() { min = vector2_2, max = vector2_1 };
      Vector3 vector3 = new Vector3(matrix4x4.GetColumn(0).magnitude, matrix4x4.GetColumn(1).magnitude, matrix4x4.GetColumn(2).magnitude);
      Vector2 vector2_3 = rect.center - rectToFit.center * vector3.x;
      frameTranslation = new Vector3(vector2_3.x, vector2_3.y, 0.0f);
      frameScaling = vector3;
      GUI.matrix = matrix;
    }

    private class Layer : VisualElement
    {
    }

    private class ContentViewContainer : VisualElement
    {
      public override bool Overlaps(Rect r)
      {
        return true;
      }
    }

    public enum FrameType
    {
      All,
      Selection,
      Origin,
    }

    [Serializable]
    private class PersistedViewTransform
    {
      public Vector3 position = Vector3.zero;
      public Vector3 scale = Vector3.one;
    }
  }
}
