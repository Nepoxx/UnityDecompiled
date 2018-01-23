// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureImportPlatformSettings
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  [Serializable]
  internal class TextureImportPlatformSettings
  {
    public static readonly int[] kTextureFormatsValueWiiU = new int[6]{ 10, 12, 7, 1, 4, 13 };
    public static readonly int[] kTextureFormatsValuePSP2 = new int[8]{ 10, 12, 7, 3, 13, 4, 1, 17 };
    public static readonly int[] kTextureFormatsValueSwitch = new int[24]{ 10, 12, 28, 29, 7, 3, 1, 2, 4, 17, 26, 27, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 };
    public static readonly int[] kTextureFormatsValueApplePVR = new int[25]{ 30, 31, 32, 33, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 34, 64, 47, 65, 7, 3, 1, 13, 4 };
    public static readonly int[] kTextureFormatsValueAndroid = new int[33]{ 10, 28, 12, 29, 34, 64, 45, 46, 47, 65, 30, 31, 32, 33, 35, 36, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 7, 3, 1, 13, 4 };
    public static readonly int[] kTextureFormatsValueTizen = new int[6]{ 34, 7, 3, 1, 13, 4 };
    public static readonly int[] kTextureFormatsValueWebGL = new int[9]{ 10, 12, 28, 29, 7, 3, 1, 2, 4 };
    public static readonly int[] kNormalFormatsValueDefault = new int[6]{ 27, 25, 12, 29, 2, 4 };
    public static readonly int[] kTextureFormatsValueDefault = new int[14]{ 10, 12, 28, 29, 7, 3, 1, 2, 4, 17, 26, 27, 24, 25 };
    public static readonly int[] kTextureFormatsValueSingleChannel = new int[2]{ 1, 26 };
    public static readonly int[] kAndroidETC2FallbackOverrideValues = new int[4]{ 0, 1, 2, 3 };
    [SerializeField]
    private TextureImporterPlatformSettings m_PlatformSettings = new TextureImporterPlatformSettings();
    [SerializeField]
    private bool m_OverriddenIsDifferent = false;
    [SerializeField]
    private bool m_MaxTextureSizeIsDifferent = false;
    [SerializeField]
    private bool m_ResizeAlgorithmIsDifferent = false;
    [SerializeField]
    private bool m_TextureCompressionIsDifferent = false;
    [SerializeField]
    private bool m_CompressionQualityIsDifferent = false;
    [SerializeField]
    private bool m_CrunchedCompressionIsDifferent = false;
    [SerializeField]
    private bool m_TextureFormatIsDifferent = false;
    [SerializeField]
    private bool m_AlphaSplitIsDifferent = false;
    [SerializeField]
    private bool m_AndroidETC2FallbackOverrideIsDifferent = false;
    [SerializeField]
    private bool m_HasChanged = false;
    [SerializeField]
    public BuildTarget m_Target;
    [SerializeField]
    private TextureImporter[] m_Importers;
    [SerializeField]
    private TextureImporterInspector m_Inspector;

    public TextureImportPlatformSettings(string name, BuildTarget target, TextureImporterInspector inspector)
    {
      this.m_PlatformSettings.name = name;
      this.m_Target = target;
      this.m_Inspector = inspector;
      this.m_PlatformSettings.overridden = false;
      this.m_Importers = ((IEnumerable<UnityEngine.Object>) inspector.targets).Select<UnityEngine.Object, TextureImporter>((Func<UnityEngine.Object, TextureImporter>) (x => x as TextureImporter)).ToArray<TextureImporter>();
      for (int index = 0; index < this.importers.Length; ++index)
      {
        TextureImporterPlatformSettings platformTextureSettings = this.importers[index].GetPlatformTextureSettings(name);
        if (index == 0)
        {
          this.m_PlatformSettings = platformTextureSettings;
        }
        else
        {
          if (platformTextureSettings.overridden != this.m_PlatformSettings.overridden)
            this.m_OverriddenIsDifferent = true;
          if (platformTextureSettings.format != this.m_PlatformSettings.format)
            this.m_TextureFormatIsDifferent = true;
          if (platformTextureSettings.maxTextureSize != this.m_PlatformSettings.maxTextureSize)
            this.m_MaxTextureSizeIsDifferent = true;
          if (platformTextureSettings.resizeAlgorithm != this.m_PlatformSettings.resizeAlgorithm)
            this.m_ResizeAlgorithmIsDifferent = true;
          if (platformTextureSettings.textureCompression != this.m_PlatformSettings.textureCompression)
            this.m_TextureCompressionIsDifferent = true;
          if (platformTextureSettings.compressionQuality != this.m_PlatformSettings.compressionQuality)
            this.m_CompressionQualityIsDifferent = true;
          if (platformTextureSettings.crunchedCompression != this.m_PlatformSettings.crunchedCompression)
            this.m_CrunchedCompressionIsDifferent = true;
          if (platformTextureSettings.allowsAlphaSplitting != this.m_PlatformSettings.allowsAlphaSplitting)
            this.m_AlphaSplitIsDifferent = true;
          if (platformTextureSettings.androidETC2FallbackOverride != this.m_PlatformSettings.androidETC2FallbackOverride)
            this.m_AndroidETC2FallbackOverrideIsDifferent = true;
        }
      }
      this.Sync();
    }

    public TextureImporterPlatformSettings platformTextureSettings
    {
      get
      {
        return this.m_PlatformSettings;
      }
    }

    public string name
    {
      get
      {
        return this.m_PlatformSettings.name;
      }
    }

    public bool overridden
    {
      get
      {
        return this.m_PlatformSettings.overridden;
      }
    }

    public bool overriddenIsDifferent
    {
      get
      {
        return this.m_OverriddenIsDifferent;
      }
    }

    public bool allAreOverridden
    {
      get
      {
        return this.isDefault || this.overridden && !this.m_OverriddenIsDifferent;
      }
    }

    public void SetOverriddenForAll(bool overridden)
    {
      this.m_PlatformSettings.overridden = overridden;
      this.m_OverriddenIsDifferent = false;
      this.SetChanged();
    }

    public int maxTextureSize
    {
      get
      {
        return this.m_PlatformSettings.maxTextureSize;
      }
    }

    public bool maxTextureSizeIsDifferent
    {
      get
      {
        return this.m_MaxTextureSizeIsDifferent;
      }
    }

    public void SetMaxTextureSizeForAll(int maxTextureSize)
    {
      this.m_PlatformSettings.maxTextureSize = maxTextureSize;
      this.m_MaxTextureSizeIsDifferent = false;
      this.SetChanged();
    }

    public TextureResizeAlgorithm resizeAlgorithm
    {
      get
      {
        return this.m_PlatformSettings.resizeAlgorithm;
      }
    }

    public bool resizeAlgorithmIsDifferent
    {
      get
      {
        return this.m_ResizeAlgorithmIsDifferent;
      }
    }

    public void SetResizeAlgorithmForAll(TextureResizeAlgorithm algorithm)
    {
      this.m_PlatformSettings.resizeAlgorithm = algorithm;
      this.m_ResizeAlgorithmIsDifferent = false;
      this.SetChanged();
    }

    public TextureImporterCompression textureCompression
    {
      get
      {
        return this.m_PlatformSettings.textureCompression;
      }
    }

    public bool textureCompressionIsDifferent
    {
      get
      {
        return this.m_TextureCompressionIsDifferent;
      }
    }

    public void SetTextureCompressionForAll(TextureImporterCompression textureCompression)
    {
      this.m_PlatformSettings.textureCompression = textureCompression;
      this.m_TextureCompressionIsDifferent = false;
      this.m_HasChanged = true;
    }

    public int compressionQuality
    {
      get
      {
        return this.m_PlatformSettings.compressionQuality;
      }
    }

    public bool compressionQualityIsDifferent
    {
      get
      {
        return this.m_CompressionQualityIsDifferent;
      }
    }

    public void SetCompressionQualityForAll(int quality)
    {
      this.m_PlatformSettings.compressionQuality = quality;
      this.m_CompressionQualityIsDifferent = false;
      this.SetChanged();
    }

    public bool crunchedCompression
    {
      get
      {
        return this.m_PlatformSettings.crunchedCompression;
      }
    }

    public bool crunchedCompressionIsDifferent
    {
      get
      {
        return this.m_CrunchedCompressionIsDifferent;
      }
    }

    public void SetCrunchedCompressionForAll(bool crunched)
    {
      this.m_PlatformSettings.crunchedCompression = crunched;
      this.m_CrunchedCompressionIsDifferent = false;
      this.SetChanged();
    }

    public TextureImporterFormat format
    {
      get
      {
        return this.m_PlatformSettings.format;
      }
    }

    public bool textureFormatIsDifferent
    {
      get
      {
        return this.m_TextureFormatIsDifferent;
      }
    }

    public void SetTextureFormatForAll(TextureImporterFormat format)
    {
      this.m_PlatformSettings.format = format;
      this.m_TextureFormatIsDifferent = false;
      this.SetChanged();
    }

    public bool allowsAlphaSplitting
    {
      get
      {
        return this.m_PlatformSettings.allowsAlphaSplitting;
      }
    }

    public bool allowsAlphaSplitIsDifferent
    {
      get
      {
        return this.m_AlphaSplitIsDifferent;
      }
    }

    public void SetAllowsAlphaSplitForAll(bool value)
    {
      this.m_PlatformSettings.allowsAlphaSplitting = value;
      this.m_AlphaSplitIsDifferent = false;
      this.SetChanged();
    }

    public AndroidETC2FallbackOverride androidETC2FallbackOverride
    {
      get
      {
        return this.m_PlatformSettings.androidETC2FallbackOverride;
      }
    }

    public bool androidETC2FallbackOverrideIsDifferent
    {
      get
      {
        return this.m_AndroidETC2FallbackOverrideIsDifferent;
      }
    }

    public void SetAndroidETC2FallbackOverrideForAll(AndroidETC2FallbackOverride value)
    {
      this.m_PlatformSettings.androidETC2FallbackOverride = value;
      this.m_AndroidETC2FallbackOverrideIsDifferent = false;
      this.SetChanged();
    }

    public TextureImporter[] importers
    {
      get
      {
        return this.m_Importers;
      }
    }

    public bool isDefault
    {
      get
      {
        return this.name == TextureImporterInspector.s_DefaultPlatformName;
      }
    }

    public bool SupportsFormat(TextureImporterFormat format, TextureImporter importer)
    {
      TextureImporterSettings settings = this.GetSettings(importer);
      int[] numArray;
      switch (this.m_Target)
      {
        case BuildTarget.iOS:
        case BuildTarget.tvOS:
          numArray = TextureImportPlatformSettings.kTextureFormatsValueApplePVR;
          break;
        case BuildTarget.Android:
          numArray = TextureImportPlatformSettings.kTextureFormatsValueAndroid;
          break;
        case BuildTarget.Tizen:
          numArray = TextureImportPlatformSettings.kTextureFormatsValueTizen;
          break;
        case BuildTarget.PSP2:
          numArray = TextureImportPlatformSettings.kTextureFormatsValuePSP2;
          break;
        case BuildTarget.WiiU:
          numArray = TextureImportPlatformSettings.kTextureFormatsValueWiiU;
          break;
        case BuildTarget.Switch:
          numArray = TextureImportPlatformSettings.kTextureFormatsValueSwitch;
          break;
        default:
          numArray = settings.textureType != TextureImporterType.NormalMap ? TextureImportPlatformSettings.kTextureFormatsValueDefault : TextureImportPlatformSettings.kNormalFormatsValueDefault;
          break;
      }
      return numArray.Contains((object) format);
    }

    public TextureImporterSettings GetSettings(TextureImporter importer)
    {
      TextureImporterSettings importerSettings = new TextureImporterSettings();
      importer.ReadTextureSettings(importerSettings);
      this.m_Inspector.GetSerializedPropertySettings(importerSettings);
      return importerSettings;
    }

    public virtual void SetChanged()
    {
      this.m_HasChanged = true;
    }

    public virtual bool HasChanged()
    {
      return this.m_HasChanged;
    }

    public void Sync()
    {
      if (!this.isDefault && (!this.overridden || this.m_OverriddenIsDifferent))
      {
        TextureImportPlatformSettings platformSetting = this.m_Inspector.m_PlatformSettings[0];
        this.m_PlatformSettings.maxTextureSize = platformSetting.maxTextureSize;
        this.m_MaxTextureSizeIsDifferent = platformSetting.m_MaxTextureSizeIsDifferent;
        this.m_PlatformSettings.resizeAlgorithm = platformSetting.resizeAlgorithm;
        this.m_ResizeAlgorithmIsDifferent = platformSetting.m_ResizeAlgorithmIsDifferent;
        this.m_PlatformSettings.textureCompression = platformSetting.textureCompression;
        this.m_TextureCompressionIsDifferent = platformSetting.m_TextureCompressionIsDifferent;
        this.m_PlatformSettings.format = platformSetting.format;
        this.m_TextureFormatIsDifferent = platformSetting.m_TextureFormatIsDifferent;
        this.m_PlatformSettings.compressionQuality = platformSetting.compressionQuality;
        this.m_CompressionQualityIsDifferent = platformSetting.m_CompressionQualityIsDifferent;
        this.m_PlatformSettings.crunchedCompression = platformSetting.crunchedCompression;
        this.m_CrunchedCompressionIsDifferent = platformSetting.m_CrunchedCompressionIsDifferent;
        this.m_PlatformSettings.allowsAlphaSplitting = platformSetting.allowsAlphaSplitting;
        this.m_AlphaSplitIsDifferent = platformSetting.m_AlphaSplitIsDifferent;
        this.m_AndroidETC2FallbackOverrideIsDifferent = platformSetting.m_AndroidETC2FallbackOverrideIsDifferent;
      }
      if (!this.overridden && !this.m_OverriddenIsDifferent || this.m_PlatformSettings.format >= ~TextureImporterFormat.Automatic)
        return;
      this.m_PlatformSettings.format = TextureImporter.FormatFromTextureParameters(this.GetSettings(this.importers[0]), this.m_PlatformSettings, this.importers[0].DoesSourceTextureHaveAlpha(), this.importers[0].IsSourceTextureHDR(), this.m_Target);
      this.m_TextureFormatIsDifferent = false;
      for (int index = 1; index < this.importers.Length; ++index)
      {
        TextureImporter importer = this.importers[index];
        if (TextureImporter.FormatFromTextureParameters(this.GetSettings(importer), this.m_PlatformSettings, importer.DoesSourceTextureHaveAlpha(), importer.IsSourceTextureHDR(), this.m_Target) != this.m_PlatformSettings.format)
          this.m_TextureFormatIsDifferent = true;
      }
    }

    private bool GetOverridden(TextureImporter importer)
    {
      if (!this.m_OverriddenIsDifferent)
        return this.overridden;
      return importer.GetPlatformTextureSettings(this.name).overridden;
    }

    public void Apply()
    {
      for (int index = 0; index < this.importers.Length; ++index)
      {
        TextureImporter importer = this.importers[index];
        TextureImporterPlatformSettings platformTextureSettings = importer.GetPlatformTextureSettings(this.name);
        if (!this.m_OverriddenIsDifferent)
          platformTextureSettings.overridden = this.m_PlatformSettings.overridden;
        if (!this.m_TextureFormatIsDifferent)
          platformTextureSettings.format = this.m_PlatformSettings.format;
        if (!this.m_MaxTextureSizeIsDifferent)
          platformTextureSettings.maxTextureSize = this.m_PlatformSettings.maxTextureSize;
        if (!this.m_ResizeAlgorithmIsDifferent)
          platformTextureSettings.resizeAlgorithm = this.m_PlatformSettings.resizeAlgorithm;
        if (!this.m_TextureCompressionIsDifferent)
          platformTextureSettings.textureCompression = this.m_PlatformSettings.textureCompression;
        if (!this.m_CompressionQualityIsDifferent)
          platformTextureSettings.compressionQuality = this.m_PlatformSettings.compressionQuality;
        if (!this.m_CrunchedCompressionIsDifferent)
          platformTextureSettings.crunchedCompression = this.m_PlatformSettings.crunchedCompression;
        if (!this.m_AlphaSplitIsDifferent)
          platformTextureSettings.allowsAlphaSplitting = this.m_PlatformSettings.allowsAlphaSplitting;
        if (!this.m_AndroidETC2FallbackOverrideIsDifferent)
          platformTextureSettings.androidETC2FallbackOverride = this.m_PlatformSettings.androidETC2FallbackOverride;
        importer.SetPlatformTextureSettings(platformTextureSettings);
      }
    }
  }
}
