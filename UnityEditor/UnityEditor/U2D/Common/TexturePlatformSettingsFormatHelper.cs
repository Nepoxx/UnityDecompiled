// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.Common.TexturePlatformSettingsFormatHelper
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.U2D.Interface;

namespace UnityEditor.U2D.Common
{
  internal class TexturePlatformSettingsFormatHelper : ITexturePlatformSettingsFormatHelper
  {
    public void AcquireTextureFormatValuesAndStrings(BuildTarget buildTarget, out int[] formatValues, out string[] formatStrings)
    {
      if (TextureImporterInspector.IsGLESMobileTargetPlatform(buildTarget))
      {
        if (buildTarget == BuildTarget.iOS || buildTarget == BuildTarget.tvOS)
        {
          formatValues = TextureImportPlatformSettings.kTextureFormatsValueApplePVR;
          formatStrings = TextureImporterInspector.s_TextureFormatStringsApplePVR;
        }
        else
        {
          formatValues = TextureImportPlatformSettings.kTextureFormatsValueAndroid;
          formatStrings = TextureImporterInspector.s_TextureFormatStringsAndroid;
        }
      }
      else
      {
        switch (buildTarget)
        {
          case BuildTarget.WebGL:
            formatValues = TextureImportPlatformSettings.kTextureFormatsValueWebGL;
            formatStrings = TextureImporterInspector.s_TextureFormatStringsWebGL;
            break;
          case BuildTarget.PSP2:
            formatValues = TextureImportPlatformSettings.kTextureFormatsValuePSP2;
            formatStrings = TextureImporterInspector.s_TextureFormatStringsPSP2;
            break;
          case BuildTarget.WiiU:
            formatValues = TextureImportPlatformSettings.kTextureFormatsValueWiiU;
            formatStrings = TextureImporterInspector.s_TextureFormatStringsWiiU;
            break;
          case BuildTarget.Switch:
            formatValues = TextureImportPlatformSettings.kTextureFormatsValueSwitch;
            formatStrings = TextureImporterInspector.s_TextureFormatStringsSwitch;
            break;
          default:
            formatValues = TextureImportPlatformSettings.kTextureFormatsValueDefault;
            formatStrings = TextureImporterInspector.s_TextureFormatStringsDefault;
            break;
        }
      }
    }

    public bool TextureFormatRequireCompressionQualityInput(TextureImporterFormat format)
    {
      return TextureImporterInspector.IsFormatRequireCompressionSetting(format);
    }
  }
}
