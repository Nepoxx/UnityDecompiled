// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.GraphView.SimpleElementPresenter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
  [Serializable]
  internal class SimpleElementPresenter : GraphElementPresenter
  {
    [SerializeField]
    private string m_Title;

    protected SimpleElementPresenter()
    {
    }

    public string title
    {
      get
      {
        return this.m_Title;
      }
      set
      {
        this.m_Title = value;
      }
    }

    protected new void OnEnable()
    {
      base.OnEnable();
      this.title = string.Empty;
    }
  }
}
