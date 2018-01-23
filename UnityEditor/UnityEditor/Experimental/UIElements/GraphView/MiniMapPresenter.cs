// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.MiniMapPresenter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  [Serializable]
  internal class MiniMapPresenter : GraphElementPresenter
  {
    public float maxHeight;
    public float maxWidth;
    [SerializeField]
    private bool m_Anchored;

    protected MiniMapPresenter()
    {
      this.maxWidth = 200f;
      this.maxHeight = 180f;
    }

    public bool anchored
    {
      get
      {
        return this.m_Anchored;
      }
      set
      {
        this.m_Anchored = value;
      }
    }

    protected new void OnEnable()
    {
      base.OnEnable();
      this.capabilities = Capabilities.Floating | Capabilities.Movable;
    }
  }
}
