// Decompiled with JetBrains decompiler
// Type: UnityEditor.DeleteWindowLayout
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections;
using System.IO;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [EditorWindowTitle(title = "Delete Layout")]
  internal class DeleteWindowLayout : EditorWindow
  {
    internal string[] m_Paths;
    private const int kMaxLayoutNameLength = 15;
    private Vector2 m_ScrollPos;

    private void OnEnable()
    {
      this.titleContent = this.GetLocalizedTitleContent();
    }

    private void InitializePaths()
    {
      string[] files = Directory.GetFiles(WindowLayout.layoutsPreferencesPath);
      ArrayList arrayList = new ArrayList();
      foreach (string path in files)
      {
        if (Path.GetExtension(Path.GetFileName(path)) == ".wlt")
          arrayList.Add((object) path);
      }
      this.m_Paths = arrayList.ToArray(typeof (string)) as string[];
    }

    private void OnGUI()
    {
      if (this.m_Paths == null)
        this.InitializePaths();
      this.m_ScrollPos = EditorGUILayout.BeginScrollView(this.m_ScrollPos);
      foreach (string path in this.m_Paths)
      {
        string text = Path.GetFileNameWithoutExtension(path);
        if (text.Length > 15)
          text = text.Substring(0, 15) + "...";
        if (GUILayout.Button(text))
        {
          if (Toolbar.lastLoadedLayoutName == text)
            Toolbar.lastLoadedLayoutName = (string) null;
          File.Delete(path);
          InternalEditorUtility.ReloadWindowLayoutMenu();
          this.InitializePaths();
        }
      }
      EditorGUILayout.EndScrollView();
    }
  }
}
