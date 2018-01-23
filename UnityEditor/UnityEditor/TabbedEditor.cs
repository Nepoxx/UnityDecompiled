// Decompiled with JetBrains decompiler
// Type: UnityEditor.TabbedEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal abstract class TabbedEditor : Editor
  {
    protected System.Type[] m_SubEditorTypes = (System.Type[]) null;
    protected string[] m_SubEditorNames = (string[]) null;
    private int m_ActiveEditorIndex = 0;
    private Editor m_ActiveEditor;

    public Editor activeEditor
    {
      get
      {
        return this.m_ActiveEditor;
      }
    }

    internal virtual void OnEnable()
    {
      this.m_ActiveEditorIndex = EditorPrefs.GetInt(this.GetType().Name + "ActiveEditorIndex", 0);
      if (!((UnityEngine.Object) this.m_ActiveEditor == (UnityEngine.Object) null))
        return;
      this.m_ActiveEditor = Editor.CreateEditor(this.targets, this.m_SubEditorTypes[this.m_ActiveEditorIndex]);
    }

    private void OnDestroy()
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.activeEditor);
    }

    public override void OnInspectorGUI()
    {
      using (new GUILayout.HorizontalScope(new GUILayoutOption[0]))
      {
        GUILayout.FlexibleSpace();
        using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
        {
          this.m_ActiveEditorIndex = GUILayout.Toolbar(this.m_ActiveEditorIndex, this.m_SubEditorNames, (GUIStyle) "LargeButton", GUI.ToolbarButtonSize.FitToContents, new GUILayoutOption[0]);
          if (changeCheckScope.changed)
          {
            EditorPrefs.SetInt(this.GetType().Name + "ActiveEditorIndex", this.m_ActiveEditorIndex);
            Editor activeEditor = this.activeEditor;
            this.m_ActiveEditor = (Editor) null;
            UnityEngine.Object.DestroyImmediate((UnityEngine.Object) activeEditor);
            this.m_ActiveEditor = Editor.CreateEditor(this.targets, this.m_SubEditorTypes[this.m_ActiveEditorIndex]);
          }
        }
        GUILayout.FlexibleSpace();
      }
      this.activeEditor.OnInspectorGUI();
    }

    public override void OnPreviewSettings()
    {
      this.activeEditor.OnPreviewSettings();
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      this.activeEditor.OnInteractivePreviewGUI(r, background);
    }

    public override bool HasPreviewGUI()
    {
      return this.activeEditor.HasPreviewGUI();
    }
  }
}
