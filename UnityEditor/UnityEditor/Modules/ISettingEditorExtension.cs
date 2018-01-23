// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.ISettingEditorExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Modules
{
  internal interface ISettingEditorExtension
  {
    void OnEnable(PlayerSettingsEditor settingsEditor);

    bool HasPublishSection();

    void PublishSectionGUI(float h, float midWidth, float maxWidth);

    bool HasIdentificationGUI();

    void IdentificationSectionGUI();

    void ConfigurationSectionGUI();

    bool SupportsOrientation();

    bool SupportsStaticBatching();

    bool SupportsDynamicBatching();

    bool SupportsHighDynamicRangeDisplays();

    bool SupportsGfxJobModes();

    bool CanShowUnitySplashScreen();

    void SplashSectionGUI();

    bool UsesStandardIcons();

    void IconSectionGUI();

    bool HasResolutionSection();

    void ResolutionSectionGUI(float h, float midWidth, float maxWidth);

    bool HasBundleIdentifier();

    bool SupportsMultithreadedRendering();

    void MultithreadedRenderingGUI(BuildTargetGroup targetGroup);

    bool SupportsCustomLightmapEncoding();
  }
}
