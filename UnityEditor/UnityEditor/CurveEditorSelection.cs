// Decompiled with JetBrains decompiler
// Type: UnityEditor.CurveEditorSelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class CurveEditorSelection : ScriptableObject
  {
    [SerializeField]
    private List<CurveSelection> m_SelectedCurves;

    public List<CurveSelection> selectedCurves
    {
      get
      {
        return this.m_SelectedCurves ?? (this.m_SelectedCurves = new List<CurveSelection>());
      }
      set
      {
        this.m_SelectedCurves = value;
      }
    }
  }
}
