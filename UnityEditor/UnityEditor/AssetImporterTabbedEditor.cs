// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetImporterTabbedEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UnityEditor
{
  internal abstract class AssetImporterTabbedEditor : AssetImporterEditor
  {
    protected string[] m_TabNames = (string[]) null;
    private int m_ActiveEditorIndex = 0;
    private BaseAssetImporterTabUI[] m_Tabs = (BaseAssetImporterTabUI[]) null;

    public BaseAssetImporterTabUI activeTab { get; private set; }

    protected BaseAssetImporterTabUI[] tabs
    {
      get
      {
        return this.m_Tabs;
      }
      set
      {
        this.m_Tabs = value;
      }
    }

    public override void OnEnable()
    {
      foreach (BaseAssetImporterTabUI tab in this.m_Tabs)
        tab.OnEnable();
      this.m_ActiveEditorIndex = EditorPrefs.GetInt(this.GetType().Name + "ActiveEditorIndex", 0);
      if (this.activeTab != null)
        return;
      this.activeTab = this.m_Tabs[this.m_ActiveEditorIndex];
    }

    private void OnDestroy()
    {
      if (this.m_Tabs == null)
        return;
      foreach (BaseAssetImporterTabUI tab in this.m_Tabs)
        tab.OnDestroy();
      this.m_Tabs = (BaseAssetImporterTabUI[]) null;
      this.activeTab = (BaseAssetImporterTabUI) null;
    }

    protected override void ResetValues()
    {
      base.ResetValues();
      if (this.m_Tabs == null)
        return;
      foreach (BaseAssetImporterTabUI tab in this.m_Tabs)
        tab.ResetValues();
    }

    public override void OnInspectorGUI()
    {
      using (new EditorGUI.DisabledScope(false))
      {
        GUI.enabled = true;
        using (new GUILayout.HorizontalScope(new GUILayoutOption[0]))
        {
          GUILayout.FlexibleSpace();
          using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
          {
            this.m_ActiveEditorIndex = GUILayout.Toolbar(this.m_ActiveEditorIndex, this.m_TabNames, (GUIStyle) "LargeButton", GUI.ToolbarButtonSize.FitToContents, new GUILayoutOption[0]);
            if (changeCheckScope.changed)
            {
              EditorPrefs.SetInt(this.GetType().Name + "ActiveEditorIndex", this.m_ActiveEditorIndex);
              this.activeTab = this.m_Tabs[this.m_ActiveEditorIndex];
              this.activeTab.OnInspectorGUI();
            }
          }
          GUILayout.FlexibleSpace();
        }
      }
      if (this.activeTab != null)
        this.activeTab.OnInspectorGUI();
      this.ApplyRevertGUI();
    }

    public override void OnPreviewSettings()
    {
      if (this.activeTab == null)
        return;
      this.activeTab.OnPreviewSettings();
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
      if (this.activeTab == null)
        return;
      this.activeTab.OnInteractivePreviewGUI(r, background);
    }

    public override bool HasPreviewGUI()
    {
      if (this.activeTab == null)
        return false;
      return this.activeTab.HasPreviewGUI();
    }

    protected override void Apply()
    {
      if (this.m_Tabs == null)
        return;
      foreach (BaseAssetImporterTabUI tab in this.m_Tabs)
        tab.PreApply();
      base.Apply();
      foreach (BaseAssetImporterTabUI tab in this.m_Tabs)
        tab.PostApply();
    }
  }
}
