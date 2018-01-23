// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Texture importer lets you modify Texture2D import settings from editor scripts.</para>
  /// </summary>
  public sealed class TextureImporter : AssetImporter
  {
    /// <summary>
    ///   <para>Format of imported texture.</para>
    /// </summary>
    [Obsolete("textureFormat is no longer accessible at the TextureImporter level. For old 'simple' formats use the textureCompression property for the equivalent automatic choice (Uncompressed for TrueColor, Compressed and HQCommpressed for 16 bits). For platform specific formats use the [[PlatformTextureSettings]] API. Using this setter will setup various parameters to match the new automatic system as well as possible. Getter will return the last value set.")]
    public extern TextureImporterFormat textureFormat { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal static extern string defaultPlatformName { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Maximum texture size.</para>
    /// </summary>
    public extern int maxTextureSize { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Quality of Texture Compression in the range [0..100].</para>
    /// </summary>
    public extern int compressionQuality { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Use crunched compression when available.</para>
    /// </summary>
    public extern bool crunchedCompression { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Allows alpha splitting on relevant platforms for this texture.</para>
    /// </summary>
    public extern bool allowAlphaSplitting { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>ETC2 texture decompression fallback override on Android devices that don't support ETC2.</para>
    /// </summary>
    public extern AndroidETC2FallbackOverride androidETC2FallbackOverride { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Compression of imported texture.</para>
    /// </summary>
    public extern TextureImporterCompression textureCompression { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Select how the alpha of the imported texture is generated.</para>
    /// </summary>
    public extern TextureImporterAlphaSource alphaSource { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Generate alpha channel from intensity?</para>
    /// </summary>
    [Obsolete("Use UnityEditor.TextureImporter.alphaSource instead.")]
    public bool grayscaleToAlpha
    {
      get
      {
        return this.alphaSource == TextureImporterAlphaSource.FromGrayScale;
      }
      set
      {
        if (value)
          this.alphaSource = TextureImporterAlphaSource.FromGrayScale;
        else
          this.alphaSource = TextureImporterAlphaSource.FromInput;
      }
    }

    /// <summary>
    ///   <para>Getter for the flag that allows Alpha splitting on the imported texture when needed (for example ETC1 compression for textures with transparency).</para>
    /// </summary>
    /// <returns>
    ///   <para>True if the importer allows alpha split on the imported texture, False otherwise.</para>
    /// </returns>
    [Obsolete("Use UnityEditor.TextureImporter.GetPlatformTextureSettings() instead.")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetAllowsAlphaSplitting();

    /// <summary>
    ///   <para>Setter for the flag that allows Alpha splitting on the imported texture when needed (for example ETC1 compression for textures with transparency).</para>
    /// </summary>
    /// <param name="flag"></param>
    [Obsolete("Use UnityEditor.TextureImporter.SetPlatformTextureSettings() instead.")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetAllowsAlphaSplitting(bool flag);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetPlatformTextureSettings(string platform, out int maxTextureSize, out TextureImporterFormat textureFormat, out int compressionQuality, out bool etc1AlphaSplitEnabled);

    public bool GetPlatformTextureSettings(string platform, out int maxTextureSize, out TextureImporterFormat textureFormat, out int compressionQuality)
    {
      bool etc1AlphaSplitEnabled = false;
      return this.GetPlatformTextureSettings(platform, out maxTextureSize, out textureFormat, out compressionQuality, out etc1AlphaSplitEnabled);
    }

    public bool GetPlatformTextureSettings(string platform, out int maxTextureSize, out TextureImporterFormat textureFormat)
    {
      int compressionQuality = 0;
      bool etc1AlphaSplitEnabled = false;
      return this.GetPlatformTextureSettings(platform, out maxTextureSize, out textureFormat, out compressionQuality, out etc1AlphaSplitEnabled);
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void Internal_GetPlatformTextureSettings(string platform, TextureImporterPlatformSettings dest);

    /// <summary>
    ///   <para>Get platform specific texture settings.</para>
    /// </summary>
    /// <param name="platform">The platform whose settings are required (see below).</param>
    /// <returns>
    ///   <para>A TextureImporterPlatformSettings structure containing the platform parameters.</para>
    /// </returns>
    public TextureImporterPlatformSettings GetPlatformTextureSettings(string platform)
    {
      TextureImporterPlatformSettings dest = new TextureImporterPlatformSettings();
      this.Internal_GetPlatformTextureSettings(platform, dest);
      return dest;
    }

    /// <summary>
    ///   <para>Get the default platform specific texture settings.</para>
    /// </summary>
    /// <returns>
    ///   <para>A TextureImporterPlatformSettings structure containing the default platform parameters.</para>
    /// </returns>
    public TextureImporterPlatformSettings GetDefaultPlatformTextureSettings()
    {
      return this.GetPlatformTextureSettings(TextureImporterInspector.s_DefaultPlatformName);
    }

    /// <summary>
    ///   <para>Returns the TextureImporterFormat that would be automatically chosen for this platform.</para>
    /// </summary>
    /// <param name="platform"></param>
    /// <returns>
    ///   <para>Format chosen by the system for the provided platform, TextureImporterFormat.Automatic if the platform does not exist.</para>
    /// </returns>
    public TextureImporterFormat GetAutomaticFormat(string platform)
    {
      TextureImporterSettings importerSettings = new TextureImporterSettings();
      this.ReadTextureSettings(importerSettings);
      TextureImporterPlatformSettings platformTextureSettings = this.GetPlatformTextureSettings(platform);
      foreach (BuildPlatform validPlatform in BuildPlatforms.instance.GetValidPlatforms())
      {
        if (validPlatform.name == platform)
          return TextureImporter.FormatFromTextureParameters(importerSettings, platformTextureSettings, this.DoesSourceTextureHaveAlpha(), this.IsSourceTextureHDR(), validPlatform.defaultTarget);
      }
      return TextureImporterFormat.Automatic;
    }

    /// <summary>
    ///   <para>Set specific target platform settings.</para>
    /// </summary>
    /// <param name="platform">The platforms whose settings are to be changed (see below).</param>
    /// <param name="maxTextureSize">Maximum texture width/height in pixels.</param>
    /// <param name="textureFormat">Data format for the texture.</param>
    /// <param name="compressionQuality">Value from 0..100, with 0, 50 and 100 being respectively Fast, Normal, Best quality options in the texture importer UI. For Crunch texture formats, this roughly corresponds to JPEG quality levels.</param>
    /// <param name="allowsAlphaSplit">Allows splitting of imported texture into RGB+A so that ETC1 compression can be applied (Android only, and works only on textures that are a part of some atlas).</param>
    [Obsolete("Use UnityEditor.TextureImporter.SetPlatformTextureSettings(TextureImporterPlatformSettings) instead.")]
    public void SetPlatformTextureSettings(string platform, int maxTextureSize, TextureImporterFormat textureFormat, int compressionQuality, bool allowsAlphaSplit)
    {
      TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings();
      this.Internal_GetPlatformTextureSettings(platform, platformSettings);
      platformSettings.overridden = true;
      platformSettings.maxTextureSize = maxTextureSize;
      platformSettings.format = textureFormat;
      platformSettings.compressionQuality = compressionQuality;
      platformSettings.allowsAlphaSplitting = allowsAlphaSplit;
      this.SetPlatformTextureSettings(platformSettings);
    }

    [ExcludeFromDocs]
    [Obsolete("Use UnityEditor.TextureImporter.SetPlatformTextureSettings(TextureImporterPlatformSettings) instead.")]
    public void SetPlatformTextureSettings(string platform, int maxTextureSize, TextureImporterFormat textureFormat)
    {
      bool allowsAlphaSplit = false;
      this.SetPlatformTextureSettings(platform, maxTextureSize, textureFormat, allowsAlphaSplit);
    }

    /// <summary>
    ///   <para>Set specific target platform settings.</para>
    /// </summary>
    /// <param name="platform">The platforms whose settings are to be changed (see below).</param>
    /// <param name="maxTextureSize">Maximum texture width/height in pixels.</param>
    /// <param name="textureFormat">Data format for the texture.</param>
    /// <param name="compressionQuality">Value from 0..100, with 0, 50 and 100 being respectively Fast, Normal, Best quality options in the texture importer UI. For Crunch texture formats, this roughly corresponds to JPEG quality levels.</param>
    /// <param name="allowsAlphaSplit">Allows splitting of imported texture into RGB+A so that ETC1 compression can be applied (Android only, and works only on textures that are a part of some atlas).</param>
    [Obsolete("Use UnityEditor.TextureImporter.SetPlatformTextureSettings(TextureImporterPlatformSettings) instead.")]
    public void SetPlatformTextureSettings(string platform, int maxTextureSize, TextureImporterFormat textureFormat, [DefaultValue("false")] bool allowsAlphaSplit)
    {
      TextureImporterPlatformSettings platformSettings = new TextureImporterPlatformSettings();
      this.Internal_GetPlatformTextureSettings(platform, platformSettings);
      platformSettings.overridden = true;
      platformSettings.maxTextureSize = maxTextureSize;
      platformSettings.format = textureFormat;
      platformSettings.allowsAlphaSplitting = allowsAlphaSplit;
      this.SetPlatformTextureSettings(platformSettings);
    }

    /// <summary>
    ///   <para>Set specific target platform settings.</para>
    /// </summary>
    /// <param name="platformSettings">Structure containing the platform settings.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetPlatformTextureSettings(TextureImporterPlatformSettings platformSettings);

    /// <summary>
    ///   <para>Clear specific target platform settings.</para>
    /// </summary>
    /// <param name="platform">The platform whose settings are to be cleared (see below).</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ClearPlatformTextureSettings(string platform);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern TextureImporterFormat FormatFromTextureParameters(TextureImporterSettings settings, TextureImporterPlatformSettings platformSettings, bool doesTextureContainAlpha, bool sourceWasHDR, BuildTarget destinationPlatform);

    /// <summary>
    ///   <para>Cubemap generation mode.</para>
    /// </summary>
    public extern TextureImporterGenerateCubemap generateCubemap { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Scaling mode for non power of two textures.</para>
    /// </summary>
    public extern TextureImporterNPOTScale npotScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Set this to true if you want texture data to be readable from scripts. Set it to false to prevent scripts from reading texture data.</para>
    /// </summary>
    public extern bool isReadable { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Generate Mip Maps.</para>
    /// </summary>
    public extern bool mipmapEnabled { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Keep texture borders the same when generating mipmaps?</para>
    /// </summary>
    public extern bool borderMipmap { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is texture storing color data?</para>
    /// </summary>
    public extern bool sRGBTexture { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Enables or disables coverage-preserving alpha MIP mapping.</para>
    /// </summary>
    public extern bool mipMapsPreserveCoverage { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns or assigns the alpha test reference value.</para>
    /// </summary>
    public extern float alphaTestReferenceValue { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mipmap filtering mode.</para>
    /// </summary>
    public extern TextureImporterMipFilter mipmapFilter { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Fade out mip levels to gray color?</para>
    /// </summary>
    public extern bool fadeout { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mip level where texture begins to fade out.</para>
    /// </summary>
    public extern int mipmapFadeDistanceStart { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mip level where texture is faded out completely.</para>
    /// </summary>
    public extern int mipmapFadeDistanceEnd { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should mip maps be generated with gamma correction?</para>
    /// </summary>
    [Obsolete("generateMipsInLinearSpace Property deprecated. Mipmaps are always generated in linear space.")]
    public extern bool generateMipsInLinearSpace { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    [Obsolete("correctGamma Property deprecated. Mipmaps are always generated in linear space.")]
    public extern bool correctGamma { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is texture storing non-color data?</para>
    /// </summary>
    [Obsolete("linearTexture Property deprecated. Use sRGBTexture instead.")]
    public bool linearTexture
    {
      get
      {
        return !this.sRGBTexture;
      }
      set
      {
        this.sRGBTexture = !value;
      }
    }

    /// <summary>
    ///   <para>Is this texture a normal map?</para>
    /// </summary>
    [Obsolete("normalmap Property deprecated. Check [[TextureImporterSettings.textureType]] instead. Getter will work as expected. Setter will set textureType to NormalMap if true, nothing otherwise.")]
    public bool normalmap
    {
      get
      {
        return this.textureType == TextureImporterType.NormalMap;
      }
      set
      {
        if (value)
          this.textureType = TextureImporterType.NormalMap;
        else
          this.textureType = TextureImporterType.Default;
      }
    }

    /// <summary>
    ///   <para>Is this texture a lightmap?</para>
    /// </summary>
    [Obsolete("lightmap Property deprecated. Check [[TextureImporterSettings.textureType]] instead. Getter will work as expected. Setter will set textureType to Lightmap if true, nothing otherwise.")]
    public bool lightmap
    {
      get
      {
        return this.textureType == TextureImporterType.Lightmap;
      }
      set
      {
        if (value)
          this.textureType = TextureImporterType.Lightmap;
        else
          this.textureType = TextureImporterType.Default;
      }
    }

    /// <summary>
    ///   <para>Convert heightmap to normal map?</para>
    /// </summary>
    public extern bool convertToNormalmap { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Normal map filtering mode.</para>
    /// </summary>
    public extern TextureImporterNormalFilter normalmapFilter { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Amount of bumpyness in the heightmap.</para>
    /// </summary>
    public extern float heightmapScale { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Anisotropic filtering level of the texture.</para>
    /// </summary>
    public extern int anisoLevel { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Filtering mode of the texture.</para>
    /// </summary>
    public extern UnityEngine.FilterMode filterMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Texture coordinate wrapping mode.</para>
    /// </summary>
    public extern TextureWrapMode wrapMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Texture U coordinate wrapping mode.</para>
    /// </summary>
    public extern TextureWrapMode wrapModeU { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Texture V coordinate wrapping mode.</para>
    /// </summary>
    public extern TextureWrapMode wrapModeV { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Texture W coordinate wrapping mode for Texture3D.</para>
    /// </summary>
    public extern TextureWrapMode wrapModeW { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Mip map bias of the texture.</para>
    /// </summary>
    public extern float mipMapBias { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If the provided alpha channel is transparency, enable this to prefilter the color to avoid filtering artifacts.</para>
    /// </summary>
    public extern bool alphaIsTransparency { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Returns true if this TextureImporter is setup for Sprite packing.</para>
    /// </summary>
    public extern bool qualifiesForSpritePacking { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Selects Single or Manual import mode for Sprite textures.</para>
    /// </summary>
    public extern SpriteImportMode spriteImportMode { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Array representing the sections of the atlas corresponding to individual sprite graphics.</para>
    /// </summary>
    public extern SpriteMetaData[] spritesheet { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Selects the Sprite packing tag.</para>
    /// </summary>
    public extern string spritePackingTag { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The number of pixels in the sprite that correspond to one unit in world space.</para>
    /// </summary>
    public extern float spritePixelsPerUnit { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Scale factor for mapping pixels in the graphic to units in world space.</para>
    /// </summary>
    [Obsolete("Use spritePixelsPerUnit property instead.")]
    public extern float spritePixelsToUnits { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The point in the Sprite object's coordinate space where the graphic is located.</para>
    /// </summary>
    public Vector2 spritePivot
    {
      get
      {
        Vector2 vector2;
        this.INTERNAL_get_spritePivot(out vector2);
        return vector2;
      }
      set
      {
        this.INTERNAL_set_spritePivot(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_spritePivot(out Vector2 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_spritePivot(ref Vector2 value);

    /// <summary>
    ///   <para>Border sizes of the generated sprites.</para>
    /// </summary>
    public Vector4 spriteBorder
    {
      get
      {
        Vector4 vector4;
        this.INTERNAL_get_spriteBorder(out vector4);
        return vector4;
      }
      set
      {
        this.INTERNAL_set_spriteBorder(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_spriteBorder(out Vector4 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_spriteBorder(ref Vector4 value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void GetWidthAndHeight(ref int width, ref int height);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern bool IsSourceTextureHDR();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsTextureFormatETC1Compression(TextureFormat fmt);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsETC1SupportedByBuildTarget(BuildTarget target);

    /// <summary>
    ///   <para>Does textures source image have alpha channel.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool DoesSourceTextureHaveAlpha();

    /// <summary>
    ///   <para>Does textures source image have RGB channels.</para>
    /// </summary>
    [Obsolete("DoesSourceTextureHaveColor always returns true in Unity.")]
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool DoesSourceTextureHaveColor();

    /// <summary>
    ///   <para>Which type of texture are we dealing with here.</para>
    /// </summary>
    public extern TextureImporterType textureType { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Shape of imported texture.</para>
    /// </summary>
    public extern TextureImporterShape textureShape { [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator, MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Read texture settings into TextureImporterSettings class.</para>
    /// </summary>
    /// <param name="dest"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ReadTextureSettings(TextureImporterSettings dest);

    /// <summary>
    ///   <para>Set texture importers settings from TextureImporterSettings class.</para>
    /// </summary>
    /// <param name="src"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetTextureSettings(TextureImporterSettings src);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern string GetImportWarnings();

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ReadTextureImportInstructions(BuildTarget target, out TextureFormat desiredFormat, out ColorSpace colorSpace, out int compressionQuality);
  }
}
