// Decompiled with JetBrains decompiler
// Type: UnityEditor.GradientPresetLibraryEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (GradientPresetLibrary))]
  internal class GradientPresetLibraryEditor : Editor
  {
    private GenericPresetLibraryInspector<GradientPresetLibrary> m_GenericPresetLibraryInspector;

    public void OnEnable()
    {
      this.m_GenericPresetLibraryInspector = new GenericPresetLibraryInspector<GradientPresetLibrary>(this.target, "Gradient Preset Library", new Action<string>(this.OnEditButtonClicked));
      this.m_GenericPresetLibraryInspector.presetSize = new Vector2(72f, 16f);
      this.m_GenericPresetLibraryInspector.lineSpacing = 4f;
    }

    public void OnDestroy()
    {
      if (this.m_GenericPresetLibraryInspector == null)
        return;
      this.m_GenericPresetLibraryInspector.OnDestroy();
    }

    public override void OnInspectorGUI()
    {
      this.m_GenericPresetLibraryInspector.itemViewMode = PresetLibraryEditorState.GetItemViewMode("Gradient");
      if (this.m_GenericPresetLibraryInspector == null)
        return;
      this.m_GenericPresetLibraryInspector.OnInspectorGUI();
    }

    private void OnEditButtonClicked(string libraryPath)
    {
      GradientPicker.Show(new Gradient(), true);
      GradientPicker.instance.currentPresetLibrary = libraryPath;
    }
  }
}
