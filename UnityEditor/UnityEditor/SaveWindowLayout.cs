// Decompiled with JetBrains decompiler
// Type: UnityEditor.SaveWindowLayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.IO;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Save Layout")]
  internal class SaveWindowLayout : EditorWindow
  {
    internal string m_LayoutName = Toolbar.lastLoadedLayoutName;
    internal bool didFocus = false;

    private void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
    }

    private void OnGUI()
    {
      GUILayout.Space(5f);
      Event current = Event.current;
      bool flag = current.type == EventType.KeyDown && (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter);
      GUI.SetNextControlName("m_PreferencesName");
      this.m_LayoutName = EditorGUILayout.TextField(this.m_LayoutName);
      if (!this.didFocus)
      {
        this.didFocus = true;
        EditorGUI.FocusTextInControl("m_PreferencesName");
      }
      GUI.enabled = this.m_LayoutName.Length != 0;
      if (!GUILayout.Button("Save") && !flag)
        return;
      this.Close();
      string path = Path.Combine(WindowLayout.layoutsPreferencesPath, this.m_LayoutName + ".wlt");
      Toolbar.lastLoadedLayoutName = this.m_LayoutName;
      WindowLayout.SaveWindowLayout(path);
      InternalEditorUtility.ReloadWindowLayoutMenu();
      GUIUtility.ExitGUI();
    }
  }
}
