// Decompiled with JetBrains decompiler
// Type: UnityEditor.SubstanceImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>The SubstanceImporter class lets you access the imported ProceduralMaterial instances.</para>
  /// </summary>
  public sealed class SubstanceImporter : AssetImporter
  {
    /// <summary>
    ///   <para>Get a list of the names of the ProceduralMaterial prototypes in the package.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string[] GetPrototypeNames();

    /// <summary>
    ///   <para>Get the number of ProceduralMaterial instances.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int GetMaterialCount();

    /// <summary>
    ///   <para>Get an array with the ProceduralMaterial instances.</para>
    /// </summary>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern ProceduralMaterial[] GetMaterials();

    /// <summary>
    ///   <para>Clone an existing ProceduralMaterial instance.</para>
    /// </summary>
    /// <param name="material"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string CloneMaterial(ProceduralMaterial material);

    /// <summary>
    ///   <para>Instantiate a new ProceduralMaterial instance from a prototype.</para>
    /// </summary>
    /// <param name="prototypeName"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern string InstantiateMaterial(string prototypeName);

    /// <summary>
    ///   <para>Destroy an existing ProceduralMaterial instance.</para>
    /// </summary>
    /// <param name="material"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void DestroyMaterial(ProceduralMaterial material);

    /// <summary>
    ///   <para>Reset the ProceduralMaterial to its default values.</para>
    /// </summary>
    /// <param name="material"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void ResetMaterial(ProceduralMaterial material);

    /// <summary>
    ///   <para>Rename an existing ProceduralMaterial instance.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="name"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool RenameMaterial(ProceduralMaterial material, string name);

    /// <summary>
    ///   <para>After modifying the shader of a ProceduralMaterial, call this function to apply the changes to the importer.</para>
    /// </summary>
    /// <param name="material"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void OnShaderModified(ProceduralMaterial material);

    /// <summary>
    ///   <para>Get the alpha source of the given texture in the ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="textureName"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern ProceduralOutputType GetTextureAlphaSource(ProceduralMaterial material, string textureName);

    /// <summary>
    ///   <para>Set the alpha source of the given texture in the ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="textureName"></param>
    /// <param name="alphaSource"></param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetTextureAlphaSource(ProceduralMaterial material, string textureName, ProceduralOutputType alphaSource);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool GetPlatformTextureSettings(string materialName, string platform, out int maxTextureWidth, out int maxTextureHeight, out int textureFormat, out int loadBehavior);

    /// <summary>
    ///   <para>Set the import settings for the input ProceduralMaterial for the input platform.</para>
    /// </summary>
    /// <param name="material">The name of the Procedural Material.</param>
    /// <param name="platform">The name of the platform (can be empty).</param>
    /// <param name="maxTextureWidth">The maximum texture width for this Procedural Material.</param>
    /// <param name="maxTextureHeight">The maximum texture height for this Procedural Material.</param>
    /// <param name="textureFormat">The texture format (0=Compressed, 1=RAW) for this Procedural Material.</param>
    /// <param name="loadBehavior">The load behavior for this Procedural Material.
    /// Values match the ProceduralMaterial::ProceduralLoadingBehavior enum.</param>
    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern void SetPlatformTextureSettings(ProceduralMaterial material, string platform, int maxTextureWidth, int maxTextureHeight, int textureFormat, int loadBehavior);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern ScriptingProceduralMaterialInformation GetMaterialInformation(ProceduralMaterial material);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void SetMaterialInformation(ProceduralMaterial material, ScriptingProceduralMaterialInformation scriptingProcMatInfo);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool CanShaderPropertyHostProceduralOutput(string name, ProceduralOutputType substanceType);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void ClearPlatformTextureSettings(string materialName, string platform);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void OnTextureInformationsChanged(ProceduralTexture texture);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern void ExportBitmapsInternal(ProceduralMaterial material, string exportPath, bool alphaRemap);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern bool IsSubstanceParented(ProceduralTexture texture, ProceduralMaterial material);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern MonoScript GetSubstanceArchive();

    /// <summary>
    ///   <para>Get the material offset, which is used for all the textures that are part of this ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    public Vector2 GetMaterialOffset(ProceduralMaterial material)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      return this.GetMaterialInformation(material).offset;
    }

    /// <summary>
    ///   <para>Set the material offset, which is used for all the textures that are part of this ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="offset"></param>
    public void SetMaterialOffset(ProceduralMaterial material, Vector2 offset)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      ScriptingProceduralMaterialInformation materialInformation = this.GetMaterialInformation(material);
      materialInformation.offset = offset;
      this.SetMaterialInformation(material, materialInformation);
    }

    /// <summary>
    ///   <para>Get the material scale, which is used for all the textures that are part of this ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    public Vector2 GetMaterialScale(ProceduralMaterial material)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      return this.GetMaterialInformation(material).scale;
    }

    /// <summary>
    ///   <para>Set the material scale, which is used for all the textures that are part of this ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="scale"></param>
    public void SetMaterialScale(ProceduralMaterial material, Vector2 scale)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      ScriptingProceduralMaterialInformation materialInformation = this.GetMaterialInformation(material);
      materialInformation.scale = scale;
      this.SetMaterialInformation(material, materialInformation);
    }

    /// <summary>
    ///   <para>Check if the ProceduralMaterial needs to force generation of all its outputs.</para>
    /// </summary>
    /// <param name="material"></param>
    public bool GetGenerateAllOutputs(ProceduralMaterial material)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      return this.GetMaterialInformation(material).generateAllOutputs;
    }

    /// <summary>
    ///   <para>Specify if the ProceduralMaterial needs to force generation of all its outputs.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="generated"></param>
    public void SetGenerateAllOutputs(ProceduralMaterial material, bool generated)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      ScriptingProceduralMaterialInformation materialInformation = this.GetMaterialInformation(material);
      materialInformation.generateAllOutputs = generated;
      this.SetMaterialInformation(material, materialInformation);
    }

    /// <summary>
    ///   <para>Get the ProceduralMaterial animation update rate in millisecond.</para>
    /// </summary>
    /// <param name="material"></param>
    public int GetAnimationUpdateRate(ProceduralMaterial material)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      return this.GetMaterialInformation(material).animationUpdateRate;
    }

    /// <summary>
    ///   <para>Set the ProceduralMaterial animation update rate in millisecond.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="animation_update_rate"></param>
    public void SetAnimationUpdateRate(ProceduralMaterial material, int animation_update_rate)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      ScriptingProceduralMaterialInformation materialInformation = this.GetMaterialInformation(material);
      materialInformation.animationUpdateRate = animation_update_rate;
      this.SetMaterialInformation(material, materialInformation);
    }

    /// <summary>
    ///   <para>Return true if the mipmaps are generated for this ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    public bool GetGenerateMipMaps(ProceduralMaterial material)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      return this.GetMaterialInformation(material).generateMipMaps;
    }

    /// <summary>
    ///   <para>Force the generation of mipmaps for this ProceduralMaterial.</para>
    /// </summary>
    /// <param name="material"></param>
    /// <param name="mode"></param>
    public void SetGenerateMipMaps(ProceduralMaterial material, bool mode)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      ScriptingProceduralMaterialInformation materialInformation = this.GetMaterialInformation(material);
      materialInformation.generateMipMaps = mode;
      this.SetMaterialInformation(material, materialInformation);
    }

    internal static bool IsProceduralTextureSlot(Material material, Texture tex, string name)
    {
      return material is ProceduralMaterial && tex is ProceduralTexture && SubstanceImporter.CanShaderPropertyHostProceduralOutput(name, (tex as ProceduralTexture).GetProceduralOutputType()) && SubstanceImporter.IsSubstanceParented(tex as ProceduralTexture, material as ProceduralMaterial);
    }

    /// <summary>
    ///   <para>Export the bitmaps generated by a ProceduralMaterial as TGA files.</para>
    /// </summary>
    /// <param name="material">The ProceduralMaterial whose output textures will be saved.</param>
    /// <param name="exportPath">Path to a folder where the output bitmaps will be saved. The folder will be created if it doesn't already exist.</param>
    /// <param name="alphaRemap">Indicates whether alpha channel remapping should be performed.</param>
    public void ExportBitmaps(ProceduralMaterial material, string exportPath, bool alphaRemap)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      if (exportPath == "")
        throw new ArgumentException("Invalid export path specified");
      if (!Directory.CreateDirectory(exportPath).Exists)
        throw new ArgumentException("Export folder " + exportPath + " doesn't exist and cannot be created.");
      this.ExportBitmapsInternal(material, exportPath, alphaRemap);
    }

    /// <summary>
    ///   <para>Export a XML preset string with the value of all parameters of a given ProceduralMaterial to the specified folder.</para>
    /// </summary>
    /// <param name="material">The ProceduralMaterial whose preset string will be saved.</param>
    /// <param name="exportPath">Path to a folder where the preset file will be saved. The folder will be created if it doesn't already exist.</param>
    public void ExportPreset(ProceduralMaterial material, string exportPath)
    {
      if ((UnityEngine.Object) material == (UnityEngine.Object) null)
        throw new ArgumentException("Invalid ProceduralMaterial");
      if (exportPath == "")
        throw new ArgumentException("Invalid export path specified");
      if (!Directory.CreateDirectory(exportPath).Exists)
        throw new ArgumentException("Export folder " + exportPath + " doesn't exist and cannot be created.");
      File.WriteAllText(Path.Combine(exportPath, material.name + ".sbsprs"), material.preset);
    }
  }
}
