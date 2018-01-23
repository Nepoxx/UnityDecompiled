// Decompiled with JetBrains decompiler
// Type: UnityEditor.FallbackEditorWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class FallbackEditorWindow : EditorWindow
  {
    private FallbackEditorWindow()
    {
    }

    private void OnEnable()
    {
      this.titleContent = new GUIContent("Failed to load");
    }

    private void OnGUI()
    {
      GUILayout.BeginVertical();
      GUILayout.FlexibleSpace();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUILayout.Label("EditorWindow could not be loaded because the script is not found in the project", (GUIStyle) "WordWrapLabel", new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      GUILayout.FlexibleSpace();
      GUILayout.EndVertical();
    }
  }
}
