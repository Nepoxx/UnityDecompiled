// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.GraphViewEditorWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal abstract class GraphViewEditorWindow : EditorWindow
  {
    private GraphViewPresenter m_Presenter;
    private IUIElementDataWatchRequest dataWatchHandle;

    public UnityEditor.Experimental.UIElements.GraphView.GraphView graphView { get; private set; }

    public GraphViewPresenter presenter
    {
      get
      {
        return this.m_Presenter;
      }
      private set
      {
        if (this.dataWatchHandle != null && this.graphView != null)
          this.graphView.dataWatch.UnregisterWatch(this.dataWatchHandle);
        this.m_Presenter = value;
        if (this.graphView == null)
          return;
        this.dataWatchHandle = this.graphView.dataWatch.RegisterWatch((UnityEngine.Object) this.m_Presenter, new System.Action<UnityEngine.Object>(this.OnChanged));
      }
    }

    public T GetPresenter<T>() where T : GraphViewPresenter
    {
      return this.presenter as T;
    }

    protected void OnEnable()
    {
      this.presenter = this.BuildPresenters();
      this.graphView = this.BuildView();
      this.graphView.name = "theView";
      this.graphView.persistenceKey = "theView";
      this.graphView.presenter = this.presenter;
      this.graphView.StretchToParentSize();
      this.graphView.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnEnterPanel), Capture.NoCapture);
      if (this.dataWatchHandle == null)
        this.dataWatchHandle = this.graphView.dataWatch.RegisterWatch((UnityEngine.Object) this.m_Presenter, new System.Action<UnityEngine.Object>(this.OnChanged));
      this.GetRootVisualContainer().Add((VisualElement) this.graphView);
    }

    protected void OnDisable()
    {
      this.GetRootVisualContainer().Remove((VisualElement) this.graphView);
    }

    protected abstract UnityEditor.Experimental.UIElements.GraphView.GraphView BuildView();

    protected abstract GraphViewPresenter BuildPresenters();

    private void OnEnterPanel(AttachToPanelEvent e)
    {
      if (!((UnityEngine.Object) this.presenter == (UnityEngine.Object) null))
        return;
      this.presenter = this.BuildPresenters();
      this.graphView.presenter = this.presenter;
    }

    private void OnChanged(UnityEngine.Object changedObject)
    {
      if (!((UnityEngine.Object) this.presenter == (UnityEngine.Object) null) || this.graphView.panel == null)
        return;
      this.presenter = this.BuildPresenters();
      this.graphView.presenter = this.presenter;
    }
  }
}
