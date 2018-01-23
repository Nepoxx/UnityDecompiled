// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.Node
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  internal class Node : GraphElement
  {
    private readonly Label m_TitleLabel;
    protected readonly Button m_CollapseButton;

    public Node()
    {
      this.clippingOptions = VisualElement.ClippingOptions.ClipAndCacheContents;
      this.mainContainer = (EditorGUIUtility.Load("UXML/GraphView/Node.uxml") as VisualTreeAsset).CloneTree((Dictionary<string, VisualElement>) null);
      this.leftContainer = this.mainContainer.Q("left", (string) null);
      this.rightContainer = this.mainContainer.Q("right", (string) null);
      this.titleContainer = this.mainContainer.Q("title", (string) null);
      this.inputContainer = this.mainContainer.Q("input", (string) null);
      this.outputContainer = this.mainContainer.Q("output", (string) null);
      this.m_TitleLabel = this.mainContainer.Q<Label>("titleLabel", (string) null);
      this.m_CollapseButton = this.mainContainer.Q<Button>("collapseButton", (string) null);
      this.m_CollapseButton.clickable.clicked += new System.Action(this.ToggleCollapse);
      this.elementTypeColor = new Color(0.9f, 0.9f, 0.9f, 0.5f);
      this.Add(this.mainContainer);
      this.mainContainer.AddToClassList(nameof (mainContainer));
      this.ClearClassList();
      this.AddToClassList("node");
    }

    protected virtual VisualElement mainContainer { get; private set; }

    protected virtual VisualElement leftContainer { get; private set; }

    protected virtual VisualElement rightContainer { get; private set; }

    protected virtual VisualElement titleContainer { get; private set; }

    protected virtual VisualElement inputContainer { get; private set; }

    protected virtual VisualElement outputContainer { get; private set; }

    public override void SetPosition(Rect newPos)
    {
      if (this.ClassListContains("vertical"))
      {
        base.SetPosition(newPos);
      }
      else
      {
        this.style.positionType = (StyleValue<PositionType>) PositionType.Absolute;
        this.style.positionLeft = (StyleValue<float>) newPos.x;
        this.style.positionTop = (StyleValue<float>) newPos.y;
      }
    }

    protected virtual void SetLayoutClassLists(NodePresenter nodePresenter)
    {
      if (this.ClassListContains("vertical") || this.ClassListContains("horizontal"))
        return;
      this.AddToClassList(nodePresenter.orientation != Orientation.Vertical ? "horizontal" : "vertical");
    }

    protected virtual void OnAnchorRemoved(NodeAnchor anchor)
    {
    }

    private void ProcessRemovedAnchors(IList<NodeAnchor> currentAnchors, VisualElement anchorContainer, IList<NodeAnchorPresenter> currentPresenters)
    {
      foreach (NodeAnchor currentAnchor in (IEnumerable<NodeAnchor>) currentAnchors)
      {
        bool flag = false;
        NodeAnchorPresenter presenter = currentAnchor.GetPresenter<NodeAnchorPresenter>();
        foreach (UnityEngine.Object currentPresenter in (IEnumerable<NodeAnchorPresenter>) currentPresenters)
        {
          if (currentPresenter == (UnityEngine.Object) presenter)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          this.OnAnchorRemoved(currentAnchor);
          anchorContainer.Remove((VisualElement) currentAnchor);
        }
      }
    }

    private void ProcessAddedAnchors(IList<NodeAnchor> currentAnchors, VisualElement anchorContainer, IList<NodeAnchorPresenter> currentPresenters)
    {
      int index = 0;
      foreach (NodeAnchorPresenter currentPresenter in (IEnumerable<NodeAnchorPresenter>) currentPresenters)
      {
        bool flag = false;
        foreach (NodeAnchor currentAnchor in (IEnumerable<NodeAnchor>) currentAnchors)
        {
          if ((UnityEngine.Object) currentPresenter == (UnityEngine.Object) currentAnchor.GetPresenter<NodeAnchorPresenter>())
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          anchorContainer.Insert(index, (VisualElement) this.InstantiateNodeAnchor(currentPresenter));
        ++index;
      }
    }

    public virtual NodeAnchor InstantiateNodeAnchor(NodeAnchorPresenter newPres)
    {
      return NodeAnchor.Create<EdgePresenter>(newPres);
    }

    private int ShowAnchors(bool show, IList<NodeAnchor> currentAnchors)
    {
      int num = 0;
      foreach (NodeAnchor currentAnchor in (IEnumerable<NodeAnchor>) currentAnchors)
      {
        NodeAnchorPresenter presenter = currentAnchor.GetPresenter<NodeAnchorPresenter>();
        if ((show || presenter.connected) && !presenter.collapsed)
        {
          currentAnchor.visible = true;
          currentAnchor.RemoveFromClassList("hidden");
          ++num;
        }
        else
        {
          currentAnchor.visible = false;
          currentAnchor.AddToClassList("hidden");
        }
      }
      return num;
    }

    public void RefreshAnchors()
    {
      NodePresenter presenter = this.GetPresenter<NodePresenter>();
      List<NodeAnchor> list1 = this.inputContainer.Query<NodeAnchor>((string) null, (string) null).ToList();
      List<NodeAnchor> list2 = this.outputContainer.Query<NodeAnchor>((string) null, (string) null).ToList();
      this.ProcessRemovedAnchors((IList<NodeAnchor>) list1, this.inputContainer, (IList<NodeAnchorPresenter>) presenter.inputAnchors);
      this.ProcessRemovedAnchors((IList<NodeAnchor>) list2, this.outputContainer, (IList<NodeAnchorPresenter>) presenter.outputAnchors);
      this.ProcessAddedAnchors((IList<NodeAnchor>) list1, this.inputContainer, (IList<NodeAnchorPresenter>) presenter.inputAnchors);
      this.ProcessAddedAnchors((IList<NodeAnchor>) list2, this.outputContainer, (IList<NodeAnchorPresenter>) presenter.outputAnchors);
      List<NodeAnchor> list3 = this.inputContainer.Query<NodeAnchor>((string) null, (string) null).ToList();
      List<NodeAnchor> list4 = this.outputContainer.Query<NodeAnchor>((string) null, (string) null).ToList();
      this.ShowAnchors(presenter.expanded, (IList<NodeAnchor>) list3);
      if (this.ShowAnchors(presenter.expanded, (IList<NodeAnchor>) list4) > 0)
      {
        if (this.mainContainer.Contains(this.rightContainer))
          return;
        this.mainContainer.Add(this.rightContainer);
      }
      else if (this.mainContainer.Contains(this.rightContainer))
        this.mainContainer.Remove(this.rightContainer);
    }

    public override void OnDataChanged()
    {
      base.OnDataChanged();
      NodePresenter presenter = this.GetPresenter<NodePresenter>();
      this.RefreshAnchors();
      this.m_TitleLabel.text = presenter.title;
      this.m_CollapseButton.text = !presenter.expanded ? "expand" : "collapse";
      this.SetLayoutClassLists(presenter);
    }

    protected virtual void ToggleCollapse()
    {
      NodePresenter presenter = this.GetPresenter<NodePresenter>();
      presenter.expanded = !presenter.expanded;
    }
  }
}
