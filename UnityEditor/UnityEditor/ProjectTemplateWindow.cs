// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectTemplateWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ProjectTemplateWindow : EditorWindow
  {
    private string m_Name;
    private string m_DisplayName;
    private string m_Description;
    private string m_Version;

    [MenuItem("internal:Project/Save As Template...")]
    internal static void SaveAsTemplate()
    {
      EditorWindow.GetWindow<ProjectTemplateWindow>().Show();
    }

    private void OnEnable()
    {
      this.titleContent = new GUIContent("Save Template");
    }

    private void OnGUI()
    {
      this.m_Name = EditorGUILayout.TextField("Name:", this.m_Name, new GUILayoutOption[0]);
      this.m_DisplayName = EditorGUILayout.TextField("Display name:", this.m_DisplayName, new GUILayoutOption[0]);
      this.m_Description = EditorGUILayout.TextField("Description:", this.m_Description, new GUILayoutOption[0]);
      this.m_Version = EditorGUILayout.TextField("Version:", this.m_Version, new GUILayoutOption[0]);
      if (!GUILayout.Button("Save As...", new GUILayoutOption[1]{ GUILayout.Width(100f) }))
        return;
      string targetPath = EditorUtility.SaveFolderPanel("Save template to folder", "", "");
      if (targetPath.Length > 0)
      {
        AssetDatabase.SaveAssets();
        EditorUtility.SaveProjectAsTemplate(targetPath, this.m_Name, this.m_DisplayName, this.m_Description, this.m_Version);
      }
    }
  }
}
