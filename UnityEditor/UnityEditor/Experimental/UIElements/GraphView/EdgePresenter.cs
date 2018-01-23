// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.EdgePresenter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  [Serializable]
  internal class EdgePresenter : GraphElementPresenter
  {
    [SerializeField]
    private NodeAnchorPresenter m_OutputPresenter;
    [SerializeField]
    private NodeAnchorPresenter m_InputPresenter;

    protected EdgePresenter()
    {
    }

    public virtual NodeAnchorPresenter output
    {
      get
      {
        return this.m_OutputPresenter;
      }
      set
      {
        if ((UnityEngine.Object) this.m_OutputPresenter != (UnityEngine.Object) null)
          this.m_OutputPresenter.Disconnect(this);
        this.m_OutputPresenter = value;
        if (!((UnityEngine.Object) this.m_OutputPresenter != (UnityEngine.Object) null))
          return;
        this.m_OutputPresenter.Connect(this);
      }
    }

    public virtual NodeAnchorPresenter input
    {
      get
      {
        return this.m_InputPresenter;
      }
      set
      {
        if ((UnityEngine.Object) this.m_InputPresenter != (UnityEngine.Object) null)
          this.m_InputPresenter.Disconnect(this);
        this.m_InputPresenter = value;
        if (!((UnityEngine.Object) this.m_InputPresenter != (UnityEngine.Object) null))
          return;
        this.m_InputPresenter.Connect(this);
      }
    }

    public Vector2 candidatePosition { get; set; }

    public bool candidate { get; set; }

    protected new void OnEnable()
    {
      base.OnEnable();
      this.capabilities = Capabilities.Selectable | Capabilities.Deletable;
    }
  }
}
