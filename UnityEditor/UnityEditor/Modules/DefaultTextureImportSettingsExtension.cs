// Decompiled with JetBrains decompiler
// Type: UnityEditor.Modules.DefaultTextureImportSettingsExtension
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Modules
{
  internal class DefaultTextureImportSettingsExtension : ITextureImportSettingsExtension
  {
    private static readonly string[] kMaxTextureSizeStrings = new string[9]{ "32", "64", "128", "256", "512", "1024", "2048", "4096", "8192" };
    private static readonly int[] kMaxTextureSizeValues = new int[9]{ 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 };
    private static readonly GUIContent maxSize = EditorGUIUtility.TextContent("Max Size|Textures larger than this will be scaled down.");
    private static readonly string[] kResizeAlgorithmStrings = new string[2]{ "Mitchell", "Bilinear" };
    private static readonly int[] kResizeAlgorithmValues = new int[2]{ 0, 1 };
    private static readonly GUIContent resizeAlgorithm = EditorGUIUtility.TextContent("Resize Algorithm|Select algorithm to apply for textures when scaled down.");
    private static readonly GUIContent kTextureCompression = EditorGUIUtility.TextContent("Compression|How will this texture be compressed?");
    private static readonly GUIContent[] kTextureCompressionOptions = new GUIContent[4]{ EditorGUIUtility.TextContent("None|Texture is not compressed."), EditorGUIUtility.TextContent("Low Quality|Texture compressed with low quality but high performance, high compression format."), EditorGUIUtility.TextContent("Normal Quality|Texture is compressed with a standard format."), EditorGUIUtility.TextContent("High Quality|Texture compressed with a high quality format.") };
    private static readonly int[] kTextureCompressionValues = new int[4]{ 0, 3, 1, 2 };

    public virtual void ShowImportSettings(Editor baseEditor, TextureImportPlatformSettings platformSettings)
    {
      TextureImporterInspector importerInspector = baseEditor as TextureImporterInspector;
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = platformSettings.overriddenIsDifferent || platformSettings.maxTextureSizeIsDifferent;
      int maxTextureSize = EditorGUILayout.IntPopup(DefaultTextureImportSettingsExtension.maxSize.text, platformSettings.maxTextureSize, DefaultTextureImportSettingsExtension.kMaxTextureSizeStrings, DefaultTextureImportSettingsExtension.kMaxTextureSizeValues, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        platformSettings.SetMaxTextureSizeForAll(maxTextureSize);
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = platformSettings.overriddenIsDifferent || platformSettings.resizeAlgorithmIsDifferent;
      int num1 = EditorGUILayout.IntPopup(DefaultTextureImportSettingsExtension.resizeAlgorithm.text, (int) platformSettings.resizeAlgorithm, DefaultTextureImportSettingsExtension.kResizeAlgorithmStrings, DefaultTextureImportSettingsExtension.kResizeAlgorithmValues, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        platformSettings.SetResizeAlgorithmForAll((TextureResizeAlgorithm) num1);
      using (new EditorGUI.DisabledScope(platformSettings.overridden && !platformSettings.isDefault))
      {
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = platformSettings.overriddenIsDifferent || platformSettings.textureCompressionIsDifferent || platformSettings.overridden && !platformSettings.isDefault;
        TextureImporterCompression textureCompression = (TextureImporterCompression) EditorGUILayout.IntPopup(DefaultTextureImportSettingsExtension.kTextureCompression, (int) platformSettings.textureCompression, DefaultTextureImportSettingsExtension.kTextureCompressionOptions, DefaultTextureImportSettingsExtension.kTextureCompressionValues, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
          platformSettings.SetTextureCompressionForAll(textureCompression);
      }
      int[] numArray1 = (int[]) null;
      string[] texts = (string[]) null;
      bool flag1 = false;
      int selectedValue = 0;
      for (int index = 0; index < importerInspector.targets.Length; ++index)
      {
        TextureImporter target = importerInspector.targets[index] as TextureImporter;
        TextureImporterSettings settings = platformSettings.GetSettings(target);
        TextureImporterType textureImporterType = !importerInspector.textureTypeHasMultipleDifferentValues ? importerInspector.textureType : settings.textureType;
        int num2 = (int) platformSettings.format;
        int[] numArray2;
        string[] strArray;
        if (platformSettings.isDefault)
        {
          num2 = -1;
          numArray2 = new int[1]{ -1 };
          strArray = new string[1]{ "Auto" };
        }
        else if (!platformSettings.overridden)
        {
          num2 = (int) TextureImporter.FormatFromTextureParameters(settings, platformSettings.platformTextureSettings, target.DoesSourceTextureHaveAlpha(), target.IsSourceTextureHDR(), platformSettings.m_Target);
          numArray2 = new int[1]{ num2 };
          strArray = new string[1]
          {
            TextureUtil.GetTextureFormatString((TextureFormat) num2)
          };
        }
        else if (textureImporterType == TextureImporterType.Cookie || textureImporterType == TextureImporterType.SingleChannel)
        {
          numArray2 = TextureImportPlatformSettings.kTextureFormatsValueSingleChannel;
          strArray = TextureImporterInspector.s_TextureFormatStringsSingleChannel;
        }
        else if (TextureImporterInspector.IsGLESMobileTargetPlatform(platformSettings.m_Target))
        {
          if (platformSettings.m_Target == BuildTarget.iOS || platformSettings.m_Target == BuildTarget.tvOS)
          {
            numArray2 = TextureImportPlatformSettings.kTextureFormatsValueApplePVR;
            strArray = TextureImporterInspector.s_TextureFormatStringsApplePVR;
          }
          else
          {
            numArray2 = TextureImportPlatformSettings.kTextureFormatsValueAndroid;
            strArray = TextureImporterInspector.s_TextureFormatStringsAndroid;
          }
        }
        else if (textureImporterType == TextureImporterType.NormalMap)
        {
          numArray2 = TextureImportPlatformSettings.kNormalFormatsValueDefault;
          strArray = TextureImporterInspector.s_NormalFormatStringsDefault;
        }
        else if (platformSettings.m_Target == BuildTarget.WebGL)
        {
          numArray2 = TextureImportPlatformSettings.kTextureFormatsValueWebGL;
          strArray = TextureImporterInspector.s_TextureFormatStringsWebGL;
        }
        else if (platformSettings.m_Target == BuildTarget.WiiU)
        {
          numArray2 = TextureImportPlatformSettings.kTextureFormatsValueWiiU;
          strArray = TextureImporterInspector.s_TextureFormatStringsWiiU;
        }
        else if (platformSettings.m_Target == BuildTarget.PSP2)
        {
          numArray2 = TextureImportPlatformSettings.kTextureFormatsValuePSP2;
          strArray = TextureImporterInspector.s_TextureFormatStringsPSP2;
        }
        else if (platformSettings.m_Target == BuildTarget.Switch)
        {
          numArray2 = TextureImportPlatformSettings.kTextureFormatsValueSwitch;
          strArray = TextureImporterInspector.s_TextureFormatStringsSwitch;
        }
        else
        {
          numArray2 = TextureImportPlatformSettings.kTextureFormatsValueDefault;
          strArray = TextureImporterInspector.s_TextureFormatStringsDefault;
        }
        if (index == 0)
        {
          numArray1 = numArray2;
          texts = strArray;
          selectedValue = num2;
        }
        else if (!((IEnumerable<int>) numArray2).SequenceEqual<int>((IEnumerable<int>) numArray1) || !((IEnumerable<string>) strArray).SequenceEqual<string>((IEnumerable<string>) texts))
        {
          flag1 = true;
          break;
        }
      }
      using (new EditorGUI.DisabledScope(flag1 || texts.Length == 1))
      {
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = flag1 || platformSettings.textureFormatIsDifferent;
        selectedValue = EditorGUILayout.IntPopup(TextureImporterInspector.s_Styles.textureFormat, selectedValue, EditorGUIUtility.TempContent(texts), numArray1, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
          platformSettings.SetTextureFormatForAll((TextureImporterFormat) selectedValue);
        if (Array.IndexOf<int>(numArray1, selectedValue) == -1)
          platformSettings.SetTextureFormatForAll((TextureImporterFormat) numArray1[0]);
      }
      if (platformSettings.isDefault && platformSettings.textureCompression != TextureImporterCompression.Uncompressed)
      {
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = platformSettings.overriddenIsDifferent || platformSettings.crunchedCompressionIsDifferent;
        bool crunched = EditorGUILayout.Toggle(TextureImporterInspector.s_Styles.crunchedCompression, platformSettings.crunchedCompression, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
          platformSettings.SetCrunchedCompressionForAll(crunched);
      }
      bool isCrunchedFormat = selectedValue == 28 || (selectedValue == 29 || selectedValue == 64) || selectedValue == 65;
      if (platformSettings.isDefault && platformSettings.textureCompression != TextureImporterCompression.Uncompressed && platformSettings.crunchedCompression || (!platformSettings.isDefault && isCrunchedFormat || !platformSettings.textureFormatIsDifferent && ArrayUtility.Contains<TextureImporterFormat>(TextureImporterInspector.kFormatsWithCompressionSettings, (TextureImporterFormat) selectedValue)))
      {
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = platformSettings.overriddenIsDifferent || platformSettings.compressionQualityIsDifferent;
        int quality = this.EditCompressionQuality(platformSettings.m_Target, platformSettings.compressionQuality, isCrunchedFormat);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
          platformSettings.SetCompressionQualityForAll(quality);
      }
      if (!TextureImporter.IsETC1SupportedByBuildTarget(BuildPipeline.GetBuildTargetByName(platformSettings.name)) || importerInspector.spriteImportMode == SpriteImportMode.None || !TextureImporter.IsTextureFormatETC1Compression((TextureFormat) selectedValue))
        return;
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = platformSettings.overriddenIsDifferent || platformSettings.allowsAlphaSplitIsDifferent;
      bool flag2 = GUILayout.Toggle(platformSettings.allowsAlphaSplitting, TextureImporterInspector.s_Styles.etc1Compression);
      if (EditorGUI.EndChangeCheck())
        platformSettings.SetAllowsAlphaSplitForAll(flag2);
    }

    private int EditCompressionQuality(BuildTarget target, int compression, bool isCrunchedFormat)
    {
      if (!isCrunchedFormat && (target == BuildTarget.iOS || target == BuildTarget.tvOS || target == BuildTarget.Android || target == BuildTarget.Tizen))
      {
        int selectedIndex = 1;
        switch (compression)
        {
          case 0:
            selectedIndex = 0;
            break;
          case 100:
            selectedIndex = 2;
            break;
        }
        switch (EditorGUILayout.Popup(TextureImporterInspector.s_Styles.compressionQuality, selectedIndex, TextureImporterInspector.s_Styles.mobileCompressionQualityOptions, new GUILayoutOption[0]))
        {
          case 0:
            return 0;
          case 1:
            return 50;
          case 2:
            return 100;
          default:
            return 50;
        }
      }
      else
      {
        compression = EditorGUILayout.IntSlider(TextureImporterInspector.s_Styles.compressionQualitySlider, compression, 0, 100, new GUILayoutOption[0]);
        return compression;
      }
    }
  }
}
