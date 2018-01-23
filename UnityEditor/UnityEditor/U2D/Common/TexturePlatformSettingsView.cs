// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.Common.TexturePlatformSettingsView
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.U2D.Interface;
using UnityEngine;

namespace UnityEditor.U2D.Common
{
  internal class TexturePlatformSettingsView : ITexturePlatformSettingsView
  {
    private static TexturePlatformSettingsView.Styles s_Styles;

    internal TexturePlatformSettingsView()
    {
      TexturePlatformSettingsView.s_Styles = TexturePlatformSettingsView.s_Styles ?? new TexturePlatformSettingsView.Styles();
    }

    public string buildPlatformTitle { get; set; }

    public virtual TextureImporterCompression DrawCompression(TextureImporterCompression defaultValue, bool isMixedValue, out bool changed)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = isMixedValue;
      defaultValue = (TextureImporterCompression) EditorGUILayout.IntPopup(TexturePlatformSettingsView.s_Styles.compressionLabel, (int) defaultValue, TexturePlatformSettingsView.s_Styles.kTextureCompressionOptions, TexturePlatformSettingsView.s_Styles.kTextureCompressionValues, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      changed = EditorGUI.EndChangeCheck();
      return defaultValue;
    }

    public virtual bool DrawUseCrunchedCompression(bool defaultValue, bool isMixedValue, out bool changed)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = isMixedValue;
      defaultValue = EditorGUILayout.Toggle(TexturePlatformSettingsView.s_Styles.useCrunchedCompressionLabel, defaultValue, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      changed = EditorGUI.EndChangeCheck();
      return defaultValue;
    }

    public virtual bool DrawOverride(bool defaultValue, bool isMixedValue, out bool changed)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = isMixedValue;
      defaultValue = EditorGUILayout.ToggleLeft(EditorGUIUtility.TempContent("Override for " + this.buildPlatformTitle), defaultValue);
      EditorGUI.showMixedValue = false;
      changed = EditorGUI.EndChangeCheck();
      return defaultValue;
    }

    public virtual int DrawMaxSize(int defaultValue, bool isMixedValue, out bool changed)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = isMixedValue;
      defaultValue = EditorGUILayout.IntPopup(TexturePlatformSettingsView.s_Styles.maxTextureSizeLabel, defaultValue, TexturePlatformSettingsView.s_Styles.kMaxTextureSizeStrings, TexturePlatformSettingsView.s_Styles.kMaxTextureSizeValues, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      changed = EditorGUI.EndChangeCheck();
      return defaultValue;
    }

    public virtual TextureImporterFormat DrawFormat(TextureImporterFormat defaultValue, int[] displayValues, string[] displayStrings, bool isMixedValue, bool isDisabled, out bool changed)
    {
      using (new EditorGUI.DisabledScope(isDisabled))
      {
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = isMixedValue;
        defaultValue = (TextureImporterFormat) EditorGUILayout.IntPopup(TexturePlatformSettingsView.s_Styles.textureFormatLabel, (int) defaultValue, EditorGUIUtility.TempContent(displayStrings), displayValues, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        changed = EditorGUI.EndChangeCheck();
        return defaultValue;
      }
    }

    public virtual int DrawCompressionQualityPopup(int defaultValue, bool isMixedValue, out bool changed)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = isMixedValue;
      defaultValue = EditorGUILayout.Popup(TexturePlatformSettingsView.s_Styles.compressionQualityLabel, defaultValue, TexturePlatformSettingsView.s_Styles.kMobileCompressionQualityOptions, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      changed = EditorGUI.EndChangeCheck();
      return defaultValue;
    }

    public virtual int DrawCompressionQualitySlider(int defaultValue, bool isMixedValue, out bool changed)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = isMixedValue;
      defaultValue = EditorGUILayout.IntSlider(TexturePlatformSettingsView.s_Styles.compressionQualitySliderLabel, defaultValue, 0, 100, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      changed = EditorGUI.EndChangeCheck();
      return defaultValue;
    }

    private class Styles
    {
      public readonly GUIContent textureFormatLabel = EditorGUIUtility.TextContent("Format");
      public readonly GUIContent maxTextureSizeLabel = EditorGUIUtility.TextContent("Max Texture Size|Maximum size of the packed texture.");
      public readonly GUIContent compressionLabel = EditorGUIUtility.TextContent("Compression|How will this texture be compressed?");
      public readonly GUIContent useCrunchedCompressionLabel = EditorGUIUtility.TextContent("Use Crunch Compression|Texture is crunch-compressed to save space on disk when applicable.");
      public readonly GUIContent compressionQualityLabel = EditorGUIUtility.TextContent("Compressor Quality");
      public readonly GUIContent compressionQualitySliderLabel = EditorGUIUtility.TextContent("Compressor Quality|Use the slider to adjust compression quality from 0 (Fastest) to 100 (Best)");
      public readonly int[] kMaxTextureSizeValues = new int[9]{ 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 };
      public readonly GUIContent[] kTextureCompressionOptions = new GUIContent[4]{ EditorGUIUtility.TextContent("None|Texture is not compressed."), EditorGUIUtility.TextContent("Low Quality|Texture compressed with low quality but high performance, high compression format."), EditorGUIUtility.TextContent("Normal Quality|Texture is compressed with a standard format."), EditorGUIUtility.TextContent("High Quality|Texture compressed with a high quality format.") };
      public readonly int[] kTextureCompressionValues = new int[4]{ 0, 3, 1, 2 };
      public readonly GUIContent[] kMobileCompressionQualityOptions = new GUIContent[3]{ EditorGUIUtility.TextContent("Fast"), EditorGUIUtility.TextContent("Normal"), EditorGUIUtility.TextContent("Best") };
      public readonly GUIContent[] kMaxTextureSizeStrings;

      public Styles()
      {
        this.kMaxTextureSizeStrings = new GUIContent[this.kMaxTextureSizeValues.Length];
        for (int index = 0; index < this.kMaxTextureSizeValues.Length; ++index)
          this.kMaxTextureSizeStrings[index] = EditorGUIUtility.TextContent(string.Format("{0}", (object) this.kMaxTextureSizeValues[index]));
      }
    }
  }
}
