// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.DefaultPlayerSettingsEditorExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Modules
{
  internal abstract class DefaultPlayerSettingsEditorExtension : ISettingEditorExtension
  {
    private static readonly GUIContent m_MTRenderingTooltip = EditorGUIUtility.TextContent("Multithreaded Rendering*");
    protected PlayerSettingsEditor m_playerSettingsEditor;
    protected SerializedProperty m_MTRendering;

    protected PlayerSettingsEditor playerSettingsEditor
    {
      get
      {
        return this.m_playerSettingsEditor;
      }
    }

    public virtual void OnEnable(PlayerSettingsEditor settingsEditor)
    {
      this.m_playerSettingsEditor = settingsEditor;
      this.m_MTRendering = this.playerSettingsEditor.FindPropertyAssert("m_MTRendering");
    }

    public virtual bool HasPublishSection()
    {
      return true;
    }

    public virtual void PublishSectionGUI(float h, float midWidth, float maxWidth)
    {
    }

    public virtual bool HasIdentificationGUI()
    {
      return false;
    }

    public virtual void IdentificationSectionGUI()
    {
    }

    public virtual void ConfigurationSectionGUI()
    {
    }

    public virtual bool SupportsOrientation()
    {
      return false;
    }

    public virtual bool CanShowUnitySplashScreen()
    {
      return false;
    }

    public virtual void SplashSectionGUI()
    {
    }

    public virtual bool UsesStandardIcons()
    {
      return true;
    }

    public virtual void IconSectionGUI()
    {
    }

    public virtual bool HasResolutionSection()
    {
      return false;
    }

    public virtual bool SupportsStaticBatching()
    {
      return true;
    }

    public virtual bool SupportsDynamicBatching()
    {
      return true;
    }

    public virtual void ResolutionSectionGUI(float h, float midWidth, float maxWidth)
    {
    }

    public virtual bool HasBundleIdentifier()
    {
      return true;
    }

    public virtual bool SupportsHighDynamicRangeDisplays()
    {
      return false;
    }

    public virtual bool SupportsGfxJobModes()
    {
      return false;
    }

    public string FixTargetOSVersion(string version)
    {
      int num = version.IndexOf('.');
      if (num < 0)
        return (version + ".0").Trim();
      if (num == version.Length - 1)
        return (version + "0").Trim();
      return version.Trim();
    }

    public virtual bool SupportsMultithreadedRendering()
    {
      return false;
    }

    protected virtual GUIContent MultithreadedRenderingGUITooltip()
    {
      return DefaultPlayerSettingsEditorExtension.m_MTRenderingTooltip;
    }

    public virtual void MultithreadedRenderingGUI(BuildTargetGroup targetGroup)
    {
      if (this.playerSettingsEditor.IsMobileTarget(targetGroup))
      {
        bool mobileMtRendering = PlayerSettings.GetMobileMTRendering(targetGroup);
        bool enable = EditorGUILayout.Toggle(this.MultithreadedRenderingGUITooltip(), mobileMtRendering, new GUILayoutOption[0]);
        if (mobileMtRendering == enable)
          return;
        PlayerSettings.SetMobileMTRendering(targetGroup, enable);
      }
      else
        EditorGUILayout.PropertyField(this.m_MTRendering, DefaultPlayerSettingsEditorExtension.m_MTRenderingTooltip, new GUILayoutOption[0]);
    }

    public virtual bool SupportsCustomLightmapEncoding()
    {
      return false;
    }
  }
}
