// Decompiled with JetBrains decompiler
// Type: UnityEditor.DoubleCurvePresetLibraryEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (DoubleCurvePresetLibrary))]
  internal class DoubleCurvePresetLibraryEditor : Editor
  {
    private GenericPresetLibraryInspector<DoubleCurvePresetLibrary> m_GenericPresetLibraryInspector;

    public void OnEnable()
    {
      this.m_GenericPresetLibraryInspector = new GenericPresetLibraryInspector<DoubleCurvePresetLibrary>(this.target, this.GetHeader(), (Action<string>) null);
      this.m_GenericPresetLibraryInspector.presetSize = new Vector2(72f, 20f);
      this.m_GenericPresetLibraryInspector.lineSpacing = 5f;
    }

    private string GetHeader()
    {
      return "Particle Curve Preset Library";
    }

    public void OnDestroy()
    {
      if (this.m_GenericPresetLibraryInspector == null)
        return;
      this.m_GenericPresetLibraryInspector.OnDestroy();
    }

    public override void OnInspectorGUI()
    {
      if (this.m_GenericPresetLibraryInspector == null)
        return;
      this.m_GenericPresetLibraryInspector.OnInspectorGUI();
    }
  }
}
