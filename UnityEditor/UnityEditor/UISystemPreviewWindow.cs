// Decompiled with JetBrains decompiler
// Type: UnityEditor.UISystemPreviewWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class UISystemPreviewWindow : EditorWindow
  {
    public UISystemProfiler profiler;

    public void OnGUI()
    {
      EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
      UISystemProfiler.DrawPreviewToolbarButtons();
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
      if (this.profiler == null)
        this.Close();
      else
        this.profiler.DrawRenderUI();
    }
  }
}
