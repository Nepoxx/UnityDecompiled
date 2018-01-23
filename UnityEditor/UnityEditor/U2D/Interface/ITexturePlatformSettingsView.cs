// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.Interface.ITexturePlatformSettingsView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.U2D.Interface
{
  internal interface ITexturePlatformSettingsView
  {
    string buildPlatformTitle { get; set; }

    TextureImporterCompression DrawCompression(TextureImporterCompression defaultValue, bool isMixedValue, out bool changed);

    bool DrawUseCrunchedCompression(bool defaultValue, bool isMixedValue, out bool changed);

    bool DrawOverride(bool defaultValue, bool isMixedValue, out bool changed);

    int DrawMaxSize(int defaultValue, bool isMixedValue, out bool changed);

    TextureImporterFormat DrawFormat(TextureImporterFormat defaultValue, int[] displayValues, string[] displayStrings, bool isMixedValue, bool isDisabled, out bool changed);

    int DrawCompressionQualityPopup(int defaultValue, bool isMixedValue, out bool changed);

    int DrawCompressionQualitySlider(int defaultValue, bool isMixedValue, out bool changed);
  }
}
