// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModelImporter
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>Model importer lets you modify import settings from editor scripts.</para>
  /// </summary>
  [NativeType(Header = "Editor/Src/AssetPipeline/ModelImporting/ModelImporter.h")]
  public class ModelImporter : AssetImporter
  {
    /// <summary>
    ///   <para>The human description that is used to generate an Avatar during the import process.</para>
    /// </summary>
    public HumanDescription humanDescription
    {
      get
      {
        HumanDescription humanDescription;
        this.INTERNAL_get_humanDescription(out humanDescription);
        return humanDescription;
      }
      set
      {
        this.INTERNAL_set_humanDescription(ref value);
      }
    }

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_get_humanDescription(out HumanDescription value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern void INTERNAL_set_humanDescription(ref HumanDescription value);

    [GeneratedByOldBindingsGenerator]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateSkeletonPose(SkeletonBone[] skeletonBones, SerializedProperty serializedProperty);

    /// <summary>
    ///   <para>Material generation options.</para>
    /// </summary>
    [Obsolete("Use importMaterials, materialName and materialSearch instead")]
    public extern ModelImporterGenerateMaterials generateMaterials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Import materials from file.</para>
    /// </summary>
    public extern bool importMaterials { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Material naming setting.</para>
    /// </summary>
    public extern ModelImporterMaterialName materialName { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Existing material search setting.</para>
    /// </summary>
    public extern ModelImporterMaterialSearch materialSearch { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Material import location options.</para>
    /// </summary>
    public extern ModelImporterMaterialLocation materialLocation { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    internal extern AssetImporter.SourceAssetIdentifier[] sourceMaterials { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Global scale factor for importing.</para>
    /// </summary>
    public extern float globalScale { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is useFileUnits supported for this asset.</para>
    /// </summary>
    public extern bool isUseFileUnitsSupported { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Use visibility properties to enable or disable MeshRenderer components.</para>
    /// </summary>
    public extern bool importVisibility { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Detect file units and import as 1FileUnit=1UnityUnit, otherwise it will import as 1cm=1UnityUnit.</para>
    /// </summary>
    public extern bool useFileUnits { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Use FileScale when importing.</para>
    /// </summary>
    public extern bool useFileScale { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is FileScale used when importing.</para>
    /// </summary>
    [Obsolete("Use useFileScale instead")]
    public bool isFileScaleUsed
    {
      get
      {
        return this.useFileScale;
      }
    }

    /// <summary>
    ///   <para>Controls import of BlendShapes.</para>
    /// </summary>
    public extern bool importBlendShapes { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Controls import of cameras. Basic properties like field of view, near plane distance and far plane distance can be animated.</para>
    /// </summary>
    public extern bool importCameras { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Controls import of lights. Note that because light are defined differently in DCC tools, some light types or properties may not be exported. Basic properties like color and intensity can be animated.</para>
    /// </summary>
    public extern bool importLights { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Add to imported meshes.</para>
    /// </summary>
    public extern bool addCollider { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Smoothing angle (in degrees) for calculating normals.</para>
    /// </summary>
    public extern float normalSmoothingAngle { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Should tangents be split across UV seams.</para>
    /// </summary>
    [Obsolete("Please use tangentImportMode instead")]
    public bool splitTangentsAcrossSeams
    {
      get
      {
        return this.importTangents == ModelImporterTangents.CalculateLegacyWithSplitTangents;
      }
      set
      {
        if (this.importTangents == ModelImporterTangents.CalculateLegacyWithSplitTangents && !value)
        {
          this.importTangents = ModelImporterTangents.CalculateLegacy;
        }
        else
        {
          if (this.importTangents != ModelImporterTangents.CalculateLegacy || !value)
            return;
          this.importTangents = ModelImporterTangents.CalculateLegacyWithSplitTangents;
        }
      }
    }

    /// <summary>
    ///   <para>Swap primary and secondary UV channels when importing.</para>
    /// </summary>
    public extern bool swapUVChannels { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Combine vertices that share the same position in space.</para>
    /// </summary>
    public extern bool weldVertices { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If this is true, any quad faces that exist in the mesh data before it is imported are kept as quads instead of being split into two triangles, for the purposes of tessellation. Set this to false to disable this behavior.</para>
    /// </summary>
    public extern bool keepQuads { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Format of the imported mesh index buffer data.</para>
    /// </summary>
    public extern ModelImporterIndexFormat indexFormat { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>If true, always create an explicit prefab root. Otherwise, if the model has a single root, it is reused as the prefab root.</para>
    /// </summary>
    public extern bool preserveHierarchy { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Generate secondary UV set for lightmapping.</para>
    /// </summary>
    public extern bool generateSecondaryUV { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Threshold for angle distortion (in degrees) when generating secondary UV.</para>
    /// </summary>
    public extern float secondaryUVAngleDistortion { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Threshold for area distortion when generating secondary UV.</para>
    /// </summary>
    public extern float secondaryUVAreaDistortion { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Hard angle (in degrees) for generating secondary UV.</para>
    /// </summary>
    public extern float secondaryUVHardAngle { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Margin to be left between charts when packing secondary UV.</para>
    /// </summary>
    public extern float secondaryUVPackMargin { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Animation generation options.</para>
    /// </summary>
    public extern ModelImporterGenerateAnimations generateAnimations { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Generates the list of all imported take.</para>
    /// </summary>
    public extern TakeInfo[] importedTakeInfos { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Generates the list of all imported Transforms.</para>
    /// </summary>
    public extern string[] transformPaths { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>
    ///   <para>Generates the list of all imported Animations.</para>
    /// </summary>
    public string[] referencedClips
    {
      get
      {
        return ModelImporter.INTERNAL_GetReferencedClips(this);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern string[] INTERNAL_GetReferencedClips(ModelImporter self);

    /// <summary>
    ///   <para>Are mesh vertices and indices accessible from script?</para>
    /// </summary>
    public extern bool isReadable { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Vertex optimization setting.</para>
    /// </summary>
    public extern bool optimizeMesh { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Normals import mode.</para>
    /// </summary>
    [Obsolete("normalImportMode is deprecated. Use importNormals instead")]
    public ModelImporterTangentSpaceMode normalImportMode
    {
      get
      {
        return (ModelImporterTangentSpaceMode) this.importNormals;
      }
      set
      {
        this.importNormals = (ModelImporterNormals) value;
      }
    }

    /// <summary>
    ///   <para>Tangents import mode.</para>
    /// </summary>
    [Obsolete("tangentImportMode is deprecated. Use importTangents instead")]
    public ModelImporterTangentSpaceMode tangentImportMode
    {
      get
      {
        return (ModelImporterTangentSpaceMode) this.importTangents;
      }
      set
      {
        this.importTangents = (ModelImporterTangents) value;
      }
    }

    /// <summary>
    ///   <para>Vertex normal import options.</para>
    /// </summary>
    public extern ModelImporterNormals importNormals { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Normal generation options for ModelImporter.</para>
    /// </summary>
    public extern ModelImporterNormalCalculationMode normalCalculationMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Vertex tangent import options.</para>
    /// </summary>
    public extern ModelImporterTangents importTangents { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Bake Inverse Kinematics (IK) when importing.</para>
    /// </summary>
    public extern bool bakeIK { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is Bake Inverse Kinematics (IK) supported by this importer.</para>
    /// </summary>
    public extern bool isBakeIKSupported { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("use resampleCurves instead.")]
    public extern bool resampleRotations { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///         <para>If set to false, the importer will not resample curves when possible.
    /// Read more about.
    /// 
    /// Notes:
    /// 
    /// - Some unsupported FBX features (such as PreRotation or PostRotation on transforms) will override this setting. In these situations, animation curves will still be resampled even if the setting is disabled. For best results, avoid using PreRotation, PostRotation and GetRotationPivot.
    /// 
    /// - This option was introduced in Version 5.3. Prior to this version, Unity's import behaviour was as if this option was always enabled. Therefore enabling the option gives the same behaviour as pre-5.3 animation import.
    /// </para>
    ///       </summary>
    public extern bool resampleCurves { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Is import of tangents supported by this importer.</para>
    /// </summary>
    public extern bool isTangentImportSupported { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [Obsolete("Use animationCompression instead", true)]
    private bool reduceKeyframes
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Mesh compression setting.</para>
    /// </summary>
    public ModelImporterMeshCompression meshCompression
    {
      get
      {
        return (ModelImporterMeshCompression) this.internal_meshCompression;
      }
      set
      {
        this.internal_meshCompression = (int) value;
      }
    }

    private extern int internal_meshCompression { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Import animation from file.</para>
    /// </summary>
    public extern bool importAnimation { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Animation optimization setting.</para>
    /// </summary>
    public extern bool optimizeGameObjects { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Animation optimization setting.</para>
    /// </summary>
    public string[] extraExposedTransformPaths
    {
      get
      {
        return this.GetExtraExposedTransformPaths();
      }
      set
      {
        ModelImporter.INTERNAL_set_extraExposedTransformPaths(this, value);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern string[] GetExtraExposedTransformPaths();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_extraExposedTransformPaths([Writable] ModelImporter self, string[] value);

    /// <summary>
    ///   <para>Additional properties to treat as user properties.</para>
    /// </summary>
    public string[] extraUserProperties
    {
      get
      {
        return this.GetExtraUserProperties();
      }
      set
      {
        ModelImporter.INTERNAL_set_extraUserProperties(this, value);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern string[] GetExtraUserProperties();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void INTERNAL_set_extraUserProperties([Writable] ModelImporter self, string[] value);

    /// <summary>
    ///   <para>Animation compression setting.</para>
    /// </summary>
    public extern ModelImporterAnimationCompression animationCompression { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Import animated custom properties from file.</para>
    /// </summary>
    public extern bool importAnimatedCustomProperties { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Allowed error of animation rotation compression.</para>
    /// </summary>
    public extern float animationRotationError { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Allowed error of animation position compression.</para>
    /// </summary>
    public extern float animationPositionError { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Allowed error of animation scale compression.</para>
    /// </summary>
    public extern float animationScaleError { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The default wrap mode for the generated animation clips.</para>
    /// </summary>
    public extern WrapMode animationWrapMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Animator generation mode.</para>
    /// </summary>
    public extern ModelImporterAnimationType animationType { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Controls how much oversampling is used when importing humanoid animations for retargeting.</para>
    /// </summary>
    public extern ModelImporterHumanoidOversampling humanoidOversampling { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>The path of the transform used to generation the motion of the animation.</para>
    /// </summary>
    public extern string motionNodeName { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    /// <summary>
    ///   <para>Imports the HumanDescription from the given Avatar.</para>
    /// </summary>
    public Avatar sourceAvatar
    {
      get
      {
        return this.GetSourceAvatar();
      }
      set
      {
        Avatar avatar = value;
        if ((UnityEngine.Object) value != (UnityEngine.Object) null)
        {
          ModelImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) value)) as ModelImporter;
          if ((UnityEngine.Object) atPath != (UnityEngine.Object) null)
          {
            this.humanDescription = atPath.humanDescription;
          }
          else
          {
            Debug.LogError((object) "Avatar must be from a ModelImporter, otherwise use ModelImporter.humanDescription");
            avatar = (Avatar) null;
          }
        }
        ModelImporter.SetSourceAvatarInternal(this, avatar);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern Avatar GetSourceAvatar();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetSourceAvatarInternal(ModelImporter self, Avatar value);

    [Obsolete("splitAnimations has been deprecated please use clipAnimations instead.", true)]
    public bool splitAnimations
    {
      get
      {
        return this.clipAnimations.Length != 0;
      }
      set
      {
      }
    }

    /// <summary>
    ///   <para>Animation clips to split animation into. See Also: ModelImporterClipAnimation.</para>
    /// </summary>
    public ModelImporterClipAnimation[] clipAnimations
    {
      get
      {
        return ModelImporter.GetClipAnimations(this);
      }
      set
      {
        ModelImporter.SetClipAnimations(this, value);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern ModelImporterClipAnimation[] GetClipAnimations(ModelImporter self);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void SetClipAnimations([Writable] ModelImporter self, ModelImporterClipAnimation[] value);

    /// <summary>
    ///   <para>Generate a list of all default animation clip based on TakeInfo.</para>
    /// </summary>
    public ModelImporterClipAnimation[] defaultClipAnimations
    {
      get
      {
        return ModelImporter.GetDefaultClipAnimations(this);
      }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern ModelImporterClipAnimation[] GetDefaultClipAnimations(ModelImporter self);

    internal extern bool isAssetOlderOr42 { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void UpdateTransformMask(AvatarMask mask, SerializedProperty serializedProperty);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern AnimationClip GetPreviewAnimationClipForTake(string takeName);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal extern string CalculateBestFittingPreviewGameObject();

    /// <summary>
    ///   <para>Creates a mask that matches the model hierarchy, and applies it to the provided ModelImporterClipAnimation.</para>
    /// </summary>
    /// <param name="clip">Clip to which the mask will be applied.</param>
    public void CreateDefaultMaskForClip(ModelImporterClipAnimation clip)
    {
      if (this.defaultClipAnimations.Length > 0)
      {
        AvatarMask mask = new AvatarMask();
        this.defaultClipAnimations[0].ConfigureMaskFromClip(ref mask);
        clip.ConfigureClipFromMask(mask);
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) mask);
      }
      else
        Debug.LogError((object) "Cannot create default mask because the current importer doesn't have any animation information");
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool ExtractTexturesInternal(string folderPath);

    /// <summary>
    ///   <para>Extracts the embedded textures from a model file (such as FBX or SketchUp).</para>
    /// </summary>
    /// <param name="folderPath">The directory where the textures will be extracted.</param>
    /// <returns>
    ///   <para>Returns true if the textures are extracted successfully, otherwise false.</para>
    /// </returns>
    public bool ExtractTextures(string folderPath)
    {
      if (string.IsNullOrEmpty(folderPath))
        throw new ArgumentException("The path cannot be empty", folderPath);
      return this.ExtractTexturesInternal(folderPath);
    }

    /// <summary>
    ///   <para>Search the project for matching materials and use them instead of the internal materials.</para>
    /// </summary>
    /// <param name="nameOption">The name matching option.</param>
    /// <param name="searchOption">The search type option.</param>
    /// <returns>
    ///   <para>Returns true if the materials have been successfly remapped, otherwise false.</para>
    /// </returns>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern bool SearchAndRemapMaterials(ModelImporterMaterialName nameOption, ModelImporterMaterialSearch searchOption);
  }
}
