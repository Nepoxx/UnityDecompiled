// Decompiled with JetBrains decompiler
// Type: UnityEditor.TierSettingsWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  internal class TierSettingsWindow : EditorWindow
  {
    private static TierSettingsWindow s_Instance;
    private Editor m_TierSettingsEditor;

    public static void CreateWindow()
    {
      TierSettingsWindow.s_Instance = EditorWindow.GetWindow<TierSettingsWindow>();
      TierSettingsWindow.s_Instance.minSize = new Vector2(600f, 300f);
      TierSettingsWindow.s_Instance.titleContent = EditorGUIUtility.TextContent("Tier Settings");
    }

    internal static TierSettingsWindow GetInstance()
    {
      return TierSettingsWindow.s_Instance;
    }

    private void OnEnable()
    {
      TierSettingsWindow.s_Instance = this;
    }

    private void OnDisable()
    {
      Object.DestroyImmediate((Object) this.m_TierSettingsEditor);
      this.m_TierSettingsEditor = (Editor) null;
      if (!((Object) TierSettingsWindow.s_Instance == (Object) this))
        return;
      TierSettingsWindow.s_Instance = (TierSettingsWindow) null;
    }

    private Object graphicsSettings
    {
      get
      {
        return GraphicsSettings.GetGraphicsSettings();
      }
    }

    private Editor tierSettingsEditor
    {
      get
      {
        Editor.CreateCachedEditor(this.graphicsSettings, typeof (GraphicsSettingsWindow.TierSettingsEditor), ref this.m_TierSettingsEditor);
        ((GraphicsSettingsWindow.TierSettingsEditor) this.m_TierSettingsEditor).verticalLayout = false;
        return this.m_TierSettingsEditor;
      }
    }

    private void OnGUI()
    {
      this.tierSettingsEditor.OnInspectorGUI();
    }
  }
}
