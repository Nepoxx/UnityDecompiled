// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.Common.TexturePlatformSettingsViewController
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.U2D.Interface;

namespace UnityEditor.U2D.Common
{
  internal class TexturePlatformSettingsViewController : ITexturePlatformSettingsController
  {
    public bool HandleDefaultSettings(List<TextureImporterPlatformSettings> platformSettings, ITexturePlatformSettingsView view)
    {
      int maxTextureSize = platformSettings[0].maxTextureSize;
      TextureImporterCompression textureCompression = platformSettings[0].textureCompression;
      bool crunchedCompression = platformSettings[0].crunchedCompression;
      int compressionQuality = platformSettings[0].compressionQuality;
      bool flag = crunchedCompression;
      int num1 = compressionQuality;
      bool isMixedValue1 = false;
      bool isMixedValue2 = false;
      bool isMixedValue3 = false;
      bool isMixedValue4 = false;
      bool changed1 = false;
      bool changed2 = false;
      bool changed3 = false;
      bool changed4 = false;
      for (int index = 1; index < platformSettings.Count; ++index)
      {
        TextureImporterPlatformSettings platformSetting = platformSettings[index];
        if (platformSetting.maxTextureSize != maxTextureSize)
          isMixedValue1 = true;
        if (platformSetting.textureCompression != textureCompression)
          isMixedValue2 = true;
        if (platformSetting.crunchedCompression != crunchedCompression)
          isMixedValue3 = true;
        if (platformSetting.compressionQuality != compressionQuality)
          isMixedValue4 = true;
      }
      int num2 = view.DrawMaxSize(maxTextureSize, isMixedValue1, out changed1);
      TextureImporterCompression importerCompression = view.DrawCompression(textureCompression, isMixedValue2, out changed2);
      if (!isMixedValue2 && textureCompression != TextureImporterCompression.Uncompressed)
      {
        flag = view.DrawUseCrunchedCompression(crunchedCompression, isMixedValue3, out changed3);
        if (!isMixedValue3 && crunchedCompression)
          num1 = view.DrawCompressionQualitySlider(compressionQuality, isMixedValue4, out changed4);
      }
      if (!changed1 && !changed2 && (!changed3 && !changed4))
        return false;
      for (int index = 0; index < platformSettings.Count; ++index)
      {
        if (changed1)
          platformSettings[index].maxTextureSize = num2;
        if (changed2)
          platformSettings[index].textureCompression = importerCompression;
        if (changed3)
          platformSettings[index].crunchedCompression = flag;
        if (changed4)
          platformSettings[index].compressionQuality = num1;
      }
      return true;
    }

    public bool HandlePlatformSettings(BuildTarget buildTarget, List<TextureImporterPlatformSettings> platformSettings, ITexturePlatformSettingsView view, ITexturePlatformSettingsFormatHelper formatHelper)
    {
      bool overridden = platformSettings[0].overridden;
      int maxTextureSize = platformSettings[0].maxTextureSize;
      TextureImporterFormat format = platformSettings[0].format;
      int compressionQuality = platformSettings[0].compressionQuality;
      int num1 = maxTextureSize;
      int num2 = compressionQuality;
      bool isMixedValue1 = false;
      bool isMixedValue2 = false;
      bool isMixedValue3 = false;
      bool isMixedValue4 = false;
      bool changed1 = false;
      bool changed2 = false;
      bool changed3 = false;
      bool changed4 = false;
      for (int index = 1; index < platformSettings.Count; ++index)
      {
        TextureImporterPlatformSettings platformSetting = platformSettings[index];
        if (platformSetting.overridden != overridden)
          isMixedValue1 = true;
        if (platformSetting.maxTextureSize != maxTextureSize)
          isMixedValue2 = true;
        if (platformSetting.format != format)
          isMixedValue3 = true;
        if (platformSetting.compressionQuality != compressionQuality)
          isMixedValue4 = true;
      }
      bool flag = view.DrawOverride(overridden, isMixedValue1, out changed1);
      if (!isMixedValue1 && overridden)
        num1 = view.DrawMaxSize(maxTextureSize, isMixedValue2, out changed2);
      int[] displayValues = (int[]) null;
      string[] displayStrings = (string[]) null;
      formatHelper.AcquireTextureFormatValuesAndStrings(buildTarget, out displayValues, out displayStrings);
      TextureImporterFormat textureImporterFormat = view.DrawFormat(format, displayValues, displayStrings, isMixedValue3, isMixedValue1 || !overridden, out changed3);
      if (!isMixedValue3 && !isMixedValue1 && (overridden && formatHelper.TextureFormatRequireCompressionQualityInput(format)))
      {
        if (buildTarget == BuildTarget.iOS || buildTarget == BuildTarget.tvOS || buildTarget == BuildTarget.Android || buildTarget == BuildTarget.Tizen)
        {
          int defaultValue = 1;
          switch (compressionQuality)
          {
            case 0:
              defaultValue = 0;
              break;
            case 100:
              defaultValue = 2;
              break;
          }
          int num3 = view.DrawCompressionQualityPopup(defaultValue, isMixedValue4, out changed4);
          if (changed4)
          {
            switch (num3)
            {
              case 0:
                num2 = 0;
                break;
              case 1:
                num2 = 50;
                break;
              case 2:
                num2 = 100;
                break;
            }
          }
        }
        else
          num2 = view.DrawCompressionQualitySlider(compressionQuality, isMixedValue4, out changed4);
      }
      if (!changed1 && !changed2 && (!changed3 && !changed4))
        return false;
      for (int index = 0; index < platformSettings.Count; ++index)
      {
        if (changed1)
          platformSettings[index].overridden = flag;
        if (changed2)
          platformSettings[index].maxTextureSize = num1;
        if (changed3)
          platformSettings[index].format = textureImporterFormat;
        if (changed4)
          platformSettings[index].compressionQuality = num2;
      }
      return true;
    }
  }
}
