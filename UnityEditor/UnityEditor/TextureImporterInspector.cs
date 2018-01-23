// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEditor.Build;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.Modules;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (TextureImporter))]
  internal class TextureImporterInspector : AssetImporterEditor
  {
    public static string s_DefaultPlatformName = "DefaultTexturePlatform";
    internal static readonly TextureImporterFormat[] kFormatsWithCompressionSettings = new TextureImporterFormat[26]{ TextureImporterFormat.DXT1Crunched, TextureImporterFormat.DXT5Crunched, TextureImporterFormat.ETC_RGB4Crunched, TextureImporterFormat.ETC2_RGBA8Crunched, TextureImporterFormat.PVRTC_RGB2, TextureImporterFormat.PVRTC_RGB4, TextureImporterFormat.PVRTC_RGBA2, TextureImporterFormat.PVRTC_RGBA4, TextureImporterFormat.ATC_RGB4, TextureImporterFormat.ATC_RGBA8, TextureImporterFormat.ETC_RGB4, TextureImporterFormat.ETC2_RGB4, TextureImporterFormat.ETC2_RGB4_PUNCHTHROUGH_ALPHA, TextureImporterFormat.ETC2_RGBA8, TextureImporterFormat.ASTC_RGB_4x4, TextureImporterFormat.ASTC_RGB_5x5, TextureImporterFormat.ASTC_RGB_6x6, TextureImporterFormat.ASTC_RGB_8x8, TextureImporterFormat.ASTC_RGB_10x10, TextureImporterFormat.ASTC_RGB_12x12, TextureImporterFormat.ASTC_RGBA_4x4, TextureImporterFormat.ASTC_RGBA_5x5, TextureImporterFormat.ASTC_RGBA_6x6, TextureImporterFormat.ASTC_RGBA_8x8, TextureImporterFormat.ASTC_RGBA_10x10, TextureImporterFormat.ASTC_RGBA_12x12 };
    private Dictionary<TextureImporterInspector.TextureInspectorGUIElement, TextureImporterInspector.GUIMethod> m_GUIElementMethods = new Dictionary<TextureImporterInspector.TextureInspectorGUIElement, TextureImporterInspector.GUIMethod>();
    private readonly AnimBool m_ShowBumpGenerationSettings = new AnimBool();
    private readonly AnimBool m_ShowCubeMapSettings = new AnimBool();
    private readonly AnimBool m_ShowGenericSpriteSettings = new AnimBool();
    private readonly AnimBool m_ShowMipMapSettings = new AnimBool();
    private readonly AnimBool m_ShowSpriteMeshTypeOption = new AnimBool();
    private readonly GUIContent m_EmptyContent = new GUIContent(" ");
    private readonly int[] m_FilterModeOptions = (int[]) Enum.GetValues(typeof (UnityEngine.FilterMode));
    private string m_ImportWarning = (string) null;
    private TextureImporterInspector.TextureInspectorTypeGUIProperties[] m_TextureTypeGUIElements = new TextureImporterInspector.TextureInspectorTypeGUIProperties[Enum.GetValues(typeof (TextureImporterType)).Length];
    private List<TextureImporterInspector.TextureInspectorGUIElement> m_GUIElementsDisplayOrder = new List<TextureImporterInspector.TextureInspectorGUIElement>();
    private bool m_ShowAdvanced = false;
    private int m_TextureWidth = 0;
    private int m_TextureHeight = 0;
    private bool m_IsPOT = false;
    private bool m_ShowPerAxisWrapModes = false;
    private SerializedProperty m_TextureType;
    [SerializeField]
    internal List<TextureImportPlatformSettings> m_PlatformSettings;
    internal static int[] s_TextureFormatsValueAll;
    internal static int[] s_NormalFormatsValueAll;
    internal static string[] s_TextureFormatStringsAll;
    internal static string[] s_TextureFormatStringsWiiU;
    internal static string[] s_TextureFormatStringsPSP2;
    internal static string[] s_TextureFormatStringsSwitch;
    internal static string[] s_TextureFormatStringsWebGL;
    internal static string[] s_TextureFormatStringsApplePVR;
    internal static string[] s_TextureFormatStringsAndroid;
    internal static string[] s_TextureFormatStringsTizen;
    internal static string[] s_TextureFormatStringsSingleChannel;
    internal static string[] s_TextureFormatStringsDefault;
    internal static string[] s_NormalFormatStringsDefault;
    internal static TextureImporterInspector.Styles s_Styles;
    private SerializedProperty m_AlphaSource;
    private SerializedProperty m_ConvertToNormalMap;
    private SerializedProperty m_HeightScale;
    private SerializedProperty m_NormalMapFilter;
    private SerializedProperty m_GenerateCubemap;
    private SerializedProperty m_CubemapConvolution;
    private SerializedProperty m_SeamlessCubemap;
    private SerializedProperty m_BorderMipMap;
    private SerializedProperty m_MipMapsPreserveCoverage;
    private SerializedProperty m_AlphaTestReferenceValue;
    private SerializedProperty m_NPOTScale;
    private SerializedProperty m_IsReadable;
    private SerializedProperty m_sRGBTexture;
    private SerializedProperty m_EnableMipMap;
    private SerializedProperty m_MipMapMode;
    private SerializedProperty m_FadeOut;
    private SerializedProperty m_MipMapFadeDistanceStart;
    private SerializedProperty m_MipMapFadeDistanceEnd;
    private SerializedProperty m_Aniso;
    private SerializedProperty m_FilterMode;
    private SerializedProperty m_WrapU;
    private SerializedProperty m_WrapV;
    private SerializedProperty m_WrapW;
    private SerializedProperty m_SpritePackingTag;
    private SerializedProperty m_SpritePixelsToUnits;
    private SerializedProperty m_SpriteExtrude;
    private SerializedProperty m_SpriteMeshType;
    private SerializedProperty m_Alignment;
    private SerializedProperty m_SpritePivot;
    private SerializedProperty m_AlphaIsTransparency;
    private SerializedProperty m_TextureShape;
    private SerializedProperty m_SpriteMode;

    internal TextureImporterType textureType
    {
      get
      {
        if (this.m_TextureType.hasMultipleDifferentValues)
          return TextureImporterType.Default;
        return (TextureImporterType) this.m_TextureType.intValue;
      }
    }

    internal bool textureTypeHasMultipleDifferentValues
    {
      get
      {
        return this.m_TextureType.hasMultipleDifferentValues;
      }
    }

    public new void OnDisable()
    {
      base.OnDisable();
      EditorPrefs.SetBool("TextureImporterShowAdvanced", this.m_ShowAdvanced);
    }

    public override bool showImportedObject
    {
      get
      {
        return false;
      }
    }

    public static bool IsCompressedDXTTextureFormat(TextureImporterFormat format)
    {
      return format == TextureImporterFormat.DXT1 || format == TextureImporterFormat.DXT5;
    }

    internal static bool IsGLESMobileTargetPlatform(BuildTarget target)
    {
      return target == BuildTarget.iOS || target == BuildTarget.tvOS || target == BuildTarget.Android || target == BuildTarget.Tizen;
    }

    internal static int[] TextureFormatsValueAll
    {
      get
      {
        if (TextureImporterInspector.s_TextureFormatsValueAll != null)
          return TextureImporterInspector.s_TextureFormatsValueAll;
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        bool flag5 = false;
        foreach (BuildPlatform playerValidPlatform in TextureImporterInspector.GetBuildPlayerValidPlatforms())
        {
          switch (playerValidPlatform.defaultTarget)
          {
            case BuildTarget.iOS:
              flag2 = true;
              break;
            case BuildTarget.Android:
              flag2 = true;
              flag1 = true;
              flag3 = true;
              flag4 = true;
              flag5 = true;
              break;
            case BuildTarget.Tizen:
              flag1 = true;
              break;
            case BuildTarget.tvOS:
              flag2 = true;
              flag5 = true;
              break;
          }
        }
        List<int> intList = new List<int>();
        intList.AddRange((IEnumerable<int>) new int[2]
        {
          10,
          12
        });
        if (flag1)
          intList.Add(34);
        if (flag2)
          intList.AddRange((IEnumerable<int>) new int[4]
          {
            30,
            31,
            32,
            33
          });
        if (flag3)
          intList.AddRange((IEnumerable<int>) new int[2]
          {
            35,
            36
          });
        if (flag4)
          intList.AddRange((IEnumerable<int>) new int[3]
          {
            45,
            46,
            47
          });
        if (flag5)
          intList.AddRange((IEnumerable<int>) new int[12]
          {
            48,
            49,
            50,
            51,
            52,
            53,
            54,
            55,
            56,
            57,
            58,
            59
          });
        intList.AddRange((IEnumerable<int>) new int[14]
        {
          7,
          2,
          13,
          3,
          1,
          5,
          4,
          17,
          24,
          25,
          28,
          29,
          64,
          65
        });
        TextureImporterInspector.s_TextureFormatsValueAll = intList.ToArray();
        return TextureImporterInspector.s_TextureFormatsValueAll;
      }
    }

    internal static int[] NormalFormatsValueAll
    {
      get
      {
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        bool flag5 = false;
        foreach (BuildPlatform playerValidPlatform in TextureImporterInspector.GetBuildPlayerValidPlatforms())
        {
          switch (playerValidPlatform.defaultTarget)
          {
            case BuildTarget.iOS:
            case BuildTarget.tvOS:
              flag2 = true;
              flag1 = true;
              break;
            case BuildTarget.Android:
              flag2 = true;
              flag3 = true;
              flag1 = true;
              flag4 = true;
              flag5 = true;
              break;
            case BuildTarget.Tizen:
              flag1 = true;
              break;
          }
        }
        List<int> intList = new List<int>();
        intList.AddRange((IEnumerable<int>) new int[1]
        {
          12
        });
        if (flag2)
          intList.AddRange((IEnumerable<int>) new int[4]
          {
            30,
            31,
            32,
            33
          });
        if (flag3)
          intList.AddRange((IEnumerable<int>) new int[2]
          {
            35,
            36
          });
        if (flag1)
          intList.AddRange((IEnumerable<int>) new int[1]
          {
            34
          });
        if (flag4)
          intList.AddRange((IEnumerable<int>) new int[3]
          {
            45,
            46,
            47
          });
        if (flag5)
          intList.AddRange((IEnumerable<int>) new int[12]
          {
            48,
            49,
            50,
            51,
            52,
            53,
            54,
            55,
            56,
            57,
            58,
            59
          });
        intList.AddRange((IEnumerable<int>) new int[4]
        {
          2,
          13,
          4,
          29
        });
        TextureImporterInspector.s_NormalFormatsValueAll = intList.ToArray();
        return TextureImporterInspector.s_NormalFormatsValueAll;
      }
    }

    private void UpdateImportWarning()
    {
      TextureImporter target = this.target as TextureImporter;
      this.m_ImportWarning = !(bool) ((UnityEngine.Object) target) ? (string) null : target.GetImportWarnings();
    }

    private void ToggleFromInt(SerializedProperty property, GUIContent label)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
      int num = !EditorGUILayout.Toggle(label, property.intValue > 0, new GUILayoutOption[0]) ? 0 : 1;
      EditorGUI.showMixedValue = false;
      if (!EditorGUI.EndChangeCheck())
        return;
      property.intValue = num;
    }

    private void EnumPopup(SerializedProperty property, System.Type type, GUIContent label)
    {
      EditorGUILayout.IntPopup(property, EditorGUIUtility.TempContent(Enum.GetNames(type)), Enum.GetValues(type) as int[], label, new GUILayoutOption[0]);
    }

    internal SpriteImportMode spriteImportMode
    {
      get
      {
        return (SpriteImportMode) this.m_SpriteMode.intValue;
      }
    }

    private void CacheSerializedProperties()
    {
      this.m_AlphaSource = this.serializedObject.FindProperty("m_AlphaUsage");
      this.m_ConvertToNormalMap = this.serializedObject.FindProperty("m_ConvertToNormalMap");
      this.m_HeightScale = this.serializedObject.FindProperty("m_HeightScale");
      this.m_NormalMapFilter = this.serializedObject.FindProperty("m_NormalMapFilter");
      this.m_GenerateCubemap = this.serializedObject.FindProperty("m_GenerateCubemap");
      this.m_SeamlessCubemap = this.serializedObject.FindProperty("m_SeamlessCubemap");
      this.m_BorderMipMap = this.serializedObject.FindProperty("m_BorderMipMap");
      this.m_MipMapsPreserveCoverage = this.serializedObject.FindProperty("m_MipMapsPreserveCoverage");
      this.m_AlphaTestReferenceValue = this.serializedObject.FindProperty("m_AlphaTestReferenceValue");
      this.m_NPOTScale = this.serializedObject.FindProperty("m_NPOTScale");
      this.m_IsReadable = this.serializedObject.FindProperty("m_IsReadable");
      this.m_sRGBTexture = this.serializedObject.FindProperty("m_sRGBTexture");
      this.m_EnableMipMap = this.serializedObject.FindProperty("m_EnableMipMap");
      this.m_MipMapMode = this.serializedObject.FindProperty("m_MipMapMode");
      this.m_FadeOut = this.serializedObject.FindProperty("m_FadeOut");
      this.m_MipMapFadeDistanceStart = this.serializedObject.FindProperty("m_MipMapFadeDistanceStart");
      this.m_MipMapFadeDistanceEnd = this.serializedObject.FindProperty("m_MipMapFadeDistanceEnd");
      this.m_Aniso = this.serializedObject.FindProperty("m_TextureSettings.m_Aniso");
      this.m_FilterMode = this.serializedObject.FindProperty("m_TextureSettings.m_FilterMode");
      this.m_WrapU = this.serializedObject.FindProperty("m_TextureSettings.m_WrapU");
      this.m_WrapV = this.serializedObject.FindProperty("m_TextureSettings.m_WrapV");
      this.m_WrapW = this.serializedObject.FindProperty("m_TextureSettings.m_WrapW");
      this.m_CubemapConvolution = this.serializedObject.FindProperty("m_CubemapConvolution");
      this.m_SpriteMode = this.serializedObject.FindProperty("m_SpriteMode");
      this.m_SpritePackingTag = this.serializedObject.FindProperty("m_SpritePackingTag");
      this.m_SpritePixelsToUnits = this.serializedObject.FindProperty("m_SpritePixelsToUnits");
      this.m_SpriteExtrude = this.serializedObject.FindProperty("m_SpriteExtrude");
      this.m_SpriteMeshType = this.serializedObject.FindProperty("m_SpriteMeshType");
      this.m_Alignment = this.serializedObject.FindProperty("m_Alignment");
      this.m_SpritePivot = this.serializedObject.FindProperty("m_SpritePivot");
      this.m_AlphaIsTransparency = this.serializedObject.FindProperty("m_AlphaIsTransparency");
      this.m_TextureType = this.serializedObject.FindProperty("m_TextureType");
      this.m_TextureShape = this.serializedObject.FindProperty("m_TextureShape");
    }

    private void InitializeGUI()
    {
      TextureImporterShape _shapeCaps = TextureImporterShape.Texture2D | TextureImporterShape.TextureCube;
      this.m_TextureTypeGUIElements[0] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling | TextureImporterInspector.TextureInspectorGUIElement.ColorSpace | TextureImporterInspector.TextureInspectorGUIElement.CubeMapConvolution | TextureImporterInspector.TextureInspectorGUIElement.CubeMapping, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, _shapeCaps);
      this.m_TextureTypeGUIElements[1] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.NormalMap | TextureImporterInspector.TextureInspectorGUIElement.CubeMapping, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, _shapeCaps);
      this.m_TextureTypeGUIElements[8] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.Sprite, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling | TextureImporterInspector.TextureInspectorGUIElement.ColorSpace | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, TextureImporterShape.Texture2D);
      this.m_TextureTypeGUIElements[4] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling | TextureImporterInspector.TextureInspectorGUIElement.Cookie | TextureImporterInspector.TextureInspectorGUIElement.CubeMapping, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, TextureImporterShape.Texture2D | TextureImporterShape.TextureCube);
      this.m_TextureTypeGUIElements[10] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling | TextureImporterInspector.TextureInspectorGUIElement.CubeMapping, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, _shapeCaps);
      this.m_TextureTypeGUIElements[2] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.None, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, TextureImporterShape.Texture2D);
      this.m_TextureTypeGUIElements[7] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.None, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, TextureImporterShape.Texture2D);
      this.m_TextureTypeGUIElements[6] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.None, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, TextureImporterShape.Texture2D);
      this.m_GUIElementMethods.Clear();
      this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo, new TextureImporterInspector.GUIMethod(this.POTScaleGUI));
      this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.Readable, new TextureImporterInspector.GUIMethod(this.ReadableGUI));
      this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.ColorSpace, new TextureImporterInspector.GUIMethod(this.ColorSpaceGUI));
      this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling, new TextureImporterInspector.GUIMethod(this.AlphaHandlingGUI));
      this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.MipMaps, new TextureImporterInspector.GUIMethod(this.MipMapGUI));
      this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.NormalMap, new TextureImporterInspector.GUIMethod(this.BumpGUI));
      this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.Sprite, new TextureImporterInspector.GUIMethod(this.SpriteGUI));
      this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.Cookie, new TextureImporterInspector.GUIMethod(this.CookieGUI));
      this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.CubeMapping, new TextureImporterInspector.GUIMethod(this.CubemapMappingGUI));
      this.m_GUIElementsDisplayOrder.Clear();
      this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.CubeMapping);
      this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.CubeMapConvolution);
      this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.Cookie);
      this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.ColorSpace);
      this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling);
      this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.NormalMap);
      this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.Sprite);
      this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo);
      this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.Readable);
      this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.MipMaps);
    }

    public override void OnEnable()
    {
      TextureImporterInspector.s_DefaultPlatformName = TextureImporter.defaultPlatformName;
      this.m_ShowAdvanced = EditorPrefs.GetBool("TextureImporterShowAdvanced", this.m_ShowAdvanced);
      this.CacheSerializedProperties();
      this.m_ShowBumpGenerationSettings.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowCubeMapSettings.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowCubeMapSettings.value = this.m_TextureShape.intValue == 2;
      this.m_ShowGenericSpriteSettings.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowGenericSpriteSettings.value = this.m_SpriteMode.intValue != 0;
      this.m_ShowSpriteMeshTypeOption.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowSpriteMeshTypeOption.value = this.ShouldShowSpriteMeshTypeOption();
      this.m_ShowMipMapSettings.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ShowMipMapSettings.value = this.m_EnableMipMap.boolValue;
      this.InitializeGUI();
      TextureImporter target = this.target as TextureImporter;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      target.GetWidthAndHeight(ref this.m_TextureWidth, ref this.m_TextureHeight);
      this.m_IsPOT = TextureImporterInspector.IsPowerOfTwo(this.m_TextureWidth) && TextureImporterInspector.IsPowerOfTwo(this.m_TextureHeight);
      TextureImporterInspector.InitializeTextureFormatStrings();
    }

    private void SetSerializedPropertySettings(TextureImporterSettings settings)
    {
      this.m_AlphaSource.intValue = (int) settings.alphaSource;
      this.m_ConvertToNormalMap.intValue = !settings.convertToNormalMap ? 0 : 1;
      this.m_HeightScale.floatValue = settings.heightmapScale;
      this.m_NormalMapFilter.intValue = (int) settings.normalMapFilter;
      this.m_GenerateCubemap.intValue = (int) settings.generateCubemap;
      this.m_CubemapConvolution.intValue = (int) settings.cubemapConvolution;
      this.m_SeamlessCubemap.intValue = !settings.seamlessCubemap ? 0 : 1;
      this.m_BorderMipMap.intValue = !settings.borderMipmap ? 0 : 1;
      this.m_MipMapsPreserveCoverage.intValue = !settings.mipMapsPreserveCoverage ? 0 : 1;
      this.m_AlphaTestReferenceValue.floatValue = settings.alphaTestReferenceValue;
      this.m_NPOTScale.intValue = (int) settings.npotScale;
      this.m_IsReadable.intValue = !settings.readable ? 0 : 1;
      this.m_EnableMipMap.intValue = !settings.mipmapEnabled ? 0 : 1;
      this.m_sRGBTexture.intValue = !settings.sRGBTexture ? 0 : 1;
      this.m_MipMapMode.intValue = (int) settings.mipmapFilter;
      this.m_FadeOut.intValue = !settings.fadeOut ? 0 : 1;
      this.m_MipMapFadeDistanceStart.intValue = settings.mipmapFadeDistanceStart;
      this.m_MipMapFadeDistanceEnd.intValue = settings.mipmapFadeDistanceEnd;
      this.m_SpriteMode.intValue = settings.spriteMode;
      this.m_SpritePixelsToUnits.floatValue = settings.spritePixelsPerUnit;
      this.m_SpriteExtrude.intValue = (int) settings.spriteExtrude;
      this.m_SpriteMeshType.intValue = (int) settings.spriteMeshType;
      this.m_Alignment.intValue = settings.spriteAlignment;
      this.m_WrapU.intValue = (int) settings.wrapMode;
      this.m_WrapV.intValue = (int) settings.wrapMode;
      this.m_FilterMode.intValue = (int) settings.filterMode;
      this.m_Aniso.intValue = settings.aniso;
      this.m_AlphaIsTransparency.intValue = !settings.alphaIsTransparency ? 0 : 1;
      this.m_TextureType.intValue = (int) settings.textureType;
      this.m_TextureShape.intValue = (int) settings.textureShape;
    }

    internal TextureImporterSettings GetSerializedPropertySettings()
    {
      return this.GetSerializedPropertySettings(new TextureImporterSettings());
    }

    internal TextureImporterSettings GetSerializedPropertySettings(TextureImporterSettings settings)
    {
      if (!this.m_AlphaSource.hasMultipleDifferentValues)
        settings.alphaSource = (TextureImporterAlphaSource) this.m_AlphaSource.intValue;
      if (!this.m_ConvertToNormalMap.hasMultipleDifferentValues)
        settings.convertToNormalMap = this.m_ConvertToNormalMap.intValue > 0;
      if (!this.m_HeightScale.hasMultipleDifferentValues)
        settings.heightmapScale = this.m_HeightScale.floatValue;
      if (!this.m_NormalMapFilter.hasMultipleDifferentValues)
        settings.normalMapFilter = (TextureImporterNormalFilter) this.m_NormalMapFilter.intValue;
      if (!this.m_GenerateCubemap.hasMultipleDifferentValues)
        settings.generateCubemap = (TextureImporterGenerateCubemap) this.m_GenerateCubemap.intValue;
      if (!this.m_CubemapConvolution.hasMultipleDifferentValues)
        settings.cubemapConvolution = (TextureImporterCubemapConvolution) this.m_CubemapConvolution.intValue;
      if (!this.m_SeamlessCubemap.hasMultipleDifferentValues)
        settings.seamlessCubemap = this.m_SeamlessCubemap.intValue > 0;
      if (!this.m_BorderMipMap.hasMultipleDifferentValues)
        settings.borderMipmap = this.m_BorderMipMap.intValue > 0;
      if (!this.m_MipMapsPreserveCoverage.hasMultipleDifferentValues)
        settings.mipMapsPreserveCoverage = this.m_MipMapsPreserveCoverage.intValue > 0;
      if (!this.m_AlphaTestReferenceValue.hasMultipleDifferentValues)
        settings.alphaTestReferenceValue = this.m_AlphaTestReferenceValue.floatValue;
      if (!this.m_NPOTScale.hasMultipleDifferentValues)
        settings.npotScale = (TextureImporterNPOTScale) this.m_NPOTScale.intValue;
      if (!this.m_IsReadable.hasMultipleDifferentValues)
        settings.readable = this.m_IsReadable.intValue > 0;
      if (!this.m_sRGBTexture.hasMultipleDifferentValues)
        settings.sRGBTexture = this.m_sRGBTexture.intValue > 0;
      if (!this.m_EnableMipMap.hasMultipleDifferentValues)
        settings.mipmapEnabled = this.m_EnableMipMap.intValue > 0;
      if (!this.m_MipMapMode.hasMultipleDifferentValues)
        settings.mipmapFilter = (TextureImporterMipFilter) this.m_MipMapMode.intValue;
      if (!this.m_FadeOut.hasMultipleDifferentValues)
        settings.fadeOut = this.m_FadeOut.intValue > 0;
      if (!this.m_MipMapFadeDistanceStart.hasMultipleDifferentValues)
        settings.mipmapFadeDistanceStart = this.m_MipMapFadeDistanceStart.intValue;
      if (!this.m_MipMapFadeDistanceEnd.hasMultipleDifferentValues)
        settings.mipmapFadeDistanceEnd = this.m_MipMapFadeDistanceEnd.intValue;
      if (!this.m_SpriteMode.hasMultipleDifferentValues)
        settings.spriteMode = this.m_SpriteMode.intValue;
      if (!this.m_SpritePixelsToUnits.hasMultipleDifferentValues)
        settings.spritePixelsPerUnit = this.m_SpritePixelsToUnits.floatValue;
      if (!this.m_SpriteExtrude.hasMultipleDifferentValues)
        settings.spriteExtrude = (uint) this.m_SpriteExtrude.intValue;
      if (!this.m_SpriteMeshType.hasMultipleDifferentValues)
        settings.spriteMeshType = (SpriteMeshType) this.m_SpriteMeshType.intValue;
      if (!this.m_Alignment.hasMultipleDifferentValues)
        settings.spriteAlignment = this.m_Alignment.intValue;
      if (!this.m_SpritePivot.hasMultipleDifferentValues)
        settings.spritePivot = this.m_SpritePivot.vector2Value;
      if (!this.m_WrapU.hasMultipleDifferentValues)
        settings.wrapModeU = (TextureWrapMode) this.m_WrapU.intValue;
      if (!this.m_WrapV.hasMultipleDifferentValues)
        settings.wrapModeU = (TextureWrapMode) this.m_WrapV.intValue;
      if (!this.m_WrapW.hasMultipleDifferentValues)
        settings.wrapModeU = (TextureWrapMode) this.m_WrapW.intValue;
      if (!this.m_FilterMode.hasMultipleDifferentValues)
        settings.filterMode = (UnityEngine.FilterMode) this.m_FilterMode.intValue;
      if (!this.m_Aniso.hasMultipleDifferentValues)
        settings.aniso = this.m_Aniso.intValue;
      if (!this.m_AlphaIsTransparency.hasMultipleDifferentValues)
        settings.alphaIsTransparency = this.m_AlphaIsTransparency.intValue > 0;
      if (!this.m_TextureType.hasMultipleDifferentValues)
        settings.textureType = (TextureImporterType) this.m_TextureType.intValue;
      if (!this.m_TextureShape.hasMultipleDifferentValues)
        settings.textureShape = (TextureImporterShape) this.m_TextureShape.intValue;
      return settings;
    }

    private void CookieGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
    {
      EditorGUI.BeginChangeCheck();
      TextureImporterInspector.CookieMode cookieMode = this.m_BorderMipMap.intValue <= 0 ? (this.m_TextureShape.intValue != 2 ? TextureImporterInspector.CookieMode.Directional : TextureImporterInspector.CookieMode.Point) : TextureImporterInspector.CookieMode.Spot;
      TextureImporterInspector.CookieMode cm = (TextureImporterInspector.CookieMode) EditorGUILayout.Popup(TextureImporterInspector.s_Styles.cookieType, (int) cookieMode, TextureImporterInspector.s_Styles.cookieOptions, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        this.SetCookieMode(cm);
      if (cm == TextureImporterInspector.CookieMode.Point)
        this.m_TextureShape.intValue = 2;
      else
        this.m_TextureShape.intValue = 1;
    }

    private void CubemapMappingGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
    {
      this.m_ShowCubeMapSettings.target = this.m_TextureShape.intValue == 2;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowCubeMapSettings.faded) && this.m_TextureShape.intValue == 2)
      {
        using (new EditorGUI.DisabledScope(!this.m_IsPOT && this.m_NPOTScale.intValue == 0))
        {
          EditorGUI.showMixedValue = this.m_GenerateCubemap.hasMultipleDifferentValues || this.m_SeamlessCubemap.hasMultipleDifferentValues;
          EditorGUI.BeginChangeCheck();
          int num = EditorGUILayout.IntPopup(TextureImporterInspector.s_Styles.cubemap, this.m_GenerateCubemap.intValue, TextureImporterInspector.s_Styles.cubemapOptions, TextureImporterInspector.s_Styles.cubemapValues2, new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck())
            this.m_GenerateCubemap.intValue = num;
          ++EditorGUI.indentLevel;
          if (this.ShouldDisplayGUIElement(guiElements, TextureImporterInspector.TextureInspectorGUIElement.CubeMapConvolution))
            EditorGUILayout.IntPopup(this.m_CubemapConvolution, TextureImporterInspector.s_Styles.cubemapConvolutionOptions, TextureImporterInspector.s_Styles.cubemapConvolutionValues, TextureImporterInspector.s_Styles.cubemapConvolution, new GUILayoutOption[0]);
          this.ToggleFromInt(this.m_SeamlessCubemap, TextureImporterInspector.s_Styles.seamlessCubemap);
          --EditorGUI.indentLevel;
          EditorGUI.showMixedValue = false;
          EditorGUILayout.Space();
        }
      }
      EditorGUILayout.EndFadeGroup();
    }

    private void ColorSpaceGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
    {
      this.ToggleFromInt(this.m_sRGBTexture, TextureImporterInspector.s_Styles.sRGBTexture);
    }

    private void POTScaleGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
    {
      using (new EditorGUI.DisabledScope(this.m_IsPOT))
        this.EnumPopup(this.m_NPOTScale, typeof (TextureImporterNPOTScale), TextureImporterInspector.s_Styles.npot);
    }

    private void ReadableGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
    {
      this.ToggleFromInt(this.m_IsReadable, TextureImporterInspector.s_Styles.readWrite);
    }

    private void AlphaHandlingGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
    {
      int count1 = 0;
      int count2 = 0;
      bool flag = TextureImporterInspector.CountImportersWithAlpha(this.targets, out count1) && TextureImporterInspector.CountImportersWithHDR(this.targets, out count2);
      EditorGUI.showMixedValue = this.m_AlphaSource.hasMultipleDifferentValues;
      EditorGUI.BeginChangeCheck();
      int num = EditorGUILayout.IntPopup(TextureImporterInspector.s_Styles.alphaSource, this.m_AlphaSource.intValue, TextureImporterInspector.s_Styles.alphaSourceOptions, TextureImporterInspector.s_Styles.alphaSourceValues, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        this.m_AlphaSource.intValue = num;
      using (new EditorGUI.DisabledScope(!flag || this.m_AlphaSource.intValue == 0 || count2 != 0))
        this.ToggleFromInt(this.m_AlphaIsTransparency, TextureImporterInspector.s_Styles.alphaIsTransparency);
    }

    private bool ShouldShowSpriteMeshTypeOption()
    {
      return this.m_SpriteMode.intValue != 3 && !this.m_SpriteMode.hasMultipleDifferentValues;
    }

    private void SpriteGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
    {
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.IntPopup(this.m_SpriteMode, TextureImporterInspector.s_Styles.spriteModeOptions, new int[3]
      {
        1,
        2,
        3
      }, TextureImporterInspector.s_Styles.spriteMode, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        GUIUtility.keyboardControl = 0;
      ++EditorGUI.indentLevel;
      this.m_ShowGenericSpriteSettings.target = this.m_SpriteMode.intValue != 0;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowGenericSpriteSettings.faded))
      {
        EditorGUILayout.PropertyField(this.m_SpritePackingTag, TextureImporterInspector.s_Styles.spritePackingTag, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_SpritePixelsToUnits, TextureImporterInspector.s_Styles.spritePixelsPerUnit, new GUILayoutOption[0]);
        this.m_ShowSpriteMeshTypeOption.target = this.ShouldShowSpriteMeshTypeOption();
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowSpriteMeshTypeOption.faded))
          EditorGUILayout.IntPopup(this.m_SpriteMeshType, TextureImporterInspector.s_Styles.spriteMeshTypeOptions, new int[2]
          {
            0,
            1
          }, TextureImporterInspector.s_Styles.spriteMeshType, new GUILayoutOption[0]);
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.IntSlider(this.m_SpriteExtrude, 0, 32, TextureImporterInspector.s_Styles.spriteExtrude, new GUILayoutOption[0]);
        if (this.m_SpriteMode.intValue == 1)
        {
          EditorGUILayout.Popup(this.m_Alignment, TextureImporterInspector.s_Styles.spriteAlignmentOptions, TextureImporterInspector.s_Styles.spriteAlignment, new GUILayoutOption[0]);
          if (this.m_Alignment.intValue == 9)
          {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(this.m_SpritePivot, this.m_EmptyContent, new GUILayoutOption[0]);
            GUILayout.EndHorizontal();
          }
        }
        using (new EditorGUI.DisabledScope(this.targets.Length != 1))
        {
          GUILayout.BeginHorizontal();
          GUILayout.FlexibleSpace();
          if (GUILayout.Button("Sprite Editor"))
          {
            if (this.HasModified())
            {
              if (EditorUtility.DisplayDialog("Unapplied import settings", "Unapplied import settings for '" + ((AssetImporter) this.target).assetPath + "'.\n" + "Apply and continue to sprite editor or cancel.", "Apply", "Cancel"))
              {
                this.ApplyAndImport();
                SpriteEditorWindow.GetWindow();
                GUIUtility.ExitGUI();
              }
            }
            else
              SpriteEditorWindow.GetWindow();
          }
          GUILayout.EndHorizontal();
        }
      }
      EditorGUILayout.EndFadeGroup();
      --EditorGUI.indentLevel;
    }

    private void MipMapGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
    {
      this.ToggleFromInt(this.m_EnableMipMap, TextureImporterInspector.s_Styles.generateMipMaps);
      this.m_ShowMipMapSettings.target = this.m_EnableMipMap.boolValue && !this.m_EnableMipMap.hasMultipleDifferentValues;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowMipMapSettings.faded))
      {
        ++EditorGUI.indentLevel;
        this.ToggleFromInt(this.m_BorderMipMap, TextureImporterInspector.s_Styles.borderMipMaps);
        EditorGUILayout.Popup(this.m_MipMapMode, TextureImporterInspector.s_Styles.mipMapFilterOptions, TextureImporterInspector.s_Styles.mipMapFilter, new GUILayoutOption[0]);
        this.ToggleFromInt(this.m_MipMapsPreserveCoverage, TextureImporterInspector.s_Styles.mipMapsPreserveCoverage);
        if (this.m_MipMapsPreserveCoverage.intValue != 0 && !this.m_MipMapsPreserveCoverage.hasMultipleDifferentValues)
        {
          ++EditorGUI.indentLevel;
          EditorGUILayout.PropertyField(this.m_AlphaTestReferenceValue, TextureImporterInspector.s_Styles.alphaTestReferenceValue, new GUILayoutOption[0]);
          --EditorGUI.indentLevel;
        }
        this.ToggleFromInt(this.m_FadeOut, TextureImporterInspector.s_Styles.mipmapFadeOutToggle);
        if (this.m_FadeOut.intValue > 0)
        {
          ++EditorGUI.indentLevel;
          EditorGUI.BeginChangeCheck();
          float intValue1 = (float) this.m_MipMapFadeDistanceStart.intValue;
          float intValue2 = (float) this.m_MipMapFadeDistanceEnd.intValue;
          EditorGUILayout.MinMaxSlider(TextureImporterInspector.s_Styles.mipmapFadeOut, ref intValue1, ref intValue2, 0.0f, 10f, new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck())
          {
            this.m_MipMapFadeDistanceStart.intValue = Mathf.RoundToInt(intValue1);
            this.m_MipMapFadeDistanceEnd.intValue = Mathf.RoundToInt(intValue2);
          }
          --EditorGUI.indentLevel;
        }
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
    }

    private void BumpGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
    {
      EditorGUI.BeginChangeCheck();
      this.ToggleFromInt(this.m_ConvertToNormalMap, TextureImporterInspector.s_Styles.generateFromBump);
      this.m_ShowBumpGenerationSettings.target = this.m_ConvertToNormalMap.intValue > 0;
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowBumpGenerationSettings.faded))
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.Slider(this.m_HeightScale, 0.0f, 0.3f, TextureImporterInspector.s_Styles.bumpiness, new GUILayoutOption[0]);
        EditorGUILayout.Popup(this.m_NormalMapFilter, TextureImporterInspector.s_Styles.bumpFilteringOptions, TextureImporterInspector.s_Styles.bumpFiltering, new GUILayoutOption[0]);
        --EditorGUI.indentLevel;
      }
      EditorGUILayout.EndFadeGroup();
      if (!EditorGUI.EndChangeCheck())
        return;
      this.SyncPlatformSettings();
    }

    private void TextureSettingsGUI()
    {
      EditorGUI.BeginChangeCheck();
      TextureInspector.WrapModePopup(this.m_WrapU, this.m_WrapV, this.m_WrapW, false, ref this.m_ShowPerAxisWrapModes);
      if (this.m_NPOTScale.intValue == 0 && (this.m_WrapU.intValue == 0 || this.m_WrapV.intValue == 0) && !ShaderUtil.hardwareSupportsFullNPOT)
      {
        bool flag = false;
        foreach (UnityEngine.Object target in this.targets)
        {
          int width = -1;
          int height = -1;
          ((TextureImporter) target).GetWidthAndHeight(ref width, ref height);
          if (!Mathf.IsPowerOfTwo(width) || !Mathf.IsPowerOfTwo(height))
          {
            flag = true;
            break;
          }
        }
        if (flag)
          EditorGUILayout.HelpBox(EditorGUIUtility.TextContent("Graphics device doesn't support Repeat wrap mode on NPOT textures. Falling back to Clamp.").text, MessageType.Warning, true);
      }
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = this.m_FilterMode.hasMultipleDifferentValues;
      UnityEngine.FilterMode filterMode1 = (UnityEngine.FilterMode) this.m_FilterMode.intValue;
      if (filterMode1 == ~UnityEngine.FilterMode.Point)
        filterMode1 = this.m_FadeOut.intValue > 0 || this.m_ConvertToNormalMap.intValue > 0 ? UnityEngine.FilterMode.Trilinear : UnityEngine.FilterMode.Bilinear;
      UnityEngine.FilterMode filterMode2 = (UnityEngine.FilterMode) EditorGUILayout.IntPopup(TextureImporterInspector.s_Styles.filterMode, (int) filterMode1, TextureImporterInspector.s_Styles.filterModeOptions, this.m_FilterModeOptions, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        this.m_FilterMode.intValue = (int) filterMode2;
      using (new EditorGUI.DisabledScope(this.m_FilterMode.intValue == 0 || this.m_EnableMipMap.intValue <= 0 || this.m_TextureShape.intValue == 2))
      {
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = this.m_Aniso.hasMultipleDifferentValues;
        int num = this.m_Aniso.intValue;
        if (num == -1)
          num = 1;
        int anisoLevel = EditorGUILayout.IntSlider("Aniso Level", num, 0, 16, new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
          this.m_Aniso.intValue = anisoLevel;
        TextureInspector.DoAnisoGlobalSettingNote(anisoLevel);
      }
      if (!EditorGUI.EndChangeCheck())
        return;
      this.ApplySettingsToTexture();
    }

    public override void OnInspectorGUI()
    {
      if (TextureImporterInspector.s_Styles == null)
        TextureImporterInspector.s_Styles = new TextureImporterInspector.Styles();
      bool enabled = GUI.enabled;
      EditorGUILayout.Space();
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = this.m_TextureType.hasMultipleDifferentValues;
      int index = EditorGUILayout.IntPopup(TextureImporterInspector.s_Styles.textureTypeTitle, this.m_TextureType.intValue, TextureImporterInspector.s_Styles.textureTypeOptions, TextureImporterInspector.s_Styles.textureTypeValues, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck() && this.m_TextureType.intValue != index)
      {
        TextureImporterSettings propertySettings = this.GetSerializedPropertySettings();
        propertySettings.ApplyTextureType((TextureImporterType) index);
        this.m_TextureType.intValue = index;
        this.SetSerializedPropertySettings(propertySettings);
        this.SyncPlatformSettings();
        this.ApplySettingsToTexture();
      }
      int[] array = TextureImporterInspector.s_Styles.textureShapeValuesDictionnary[this.m_TextureTypeGUIElements[index].shapeCaps];
      using (new EditorGUI.DisabledScope(array.Length == 1 || this.m_TextureType.intValue == 4))
      {
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = this.m_TextureShape.hasMultipleDifferentValues;
        int num = EditorGUILayout.IntPopup(TextureImporterInspector.s_Styles.textureShape, this.m_TextureShape.intValue, TextureImporterInspector.s_Styles.textureShapeOptionsDictionnary[this.m_TextureTypeGUIElements[index].shapeCaps], TextureImporterInspector.s_Styles.textureShapeValuesDictionnary[this.m_TextureTypeGUIElements[index].shapeCaps], new GUILayoutOption[0]);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
          this.m_TextureShape.intValue = num;
      }
      if (Array.IndexOf<int>(array, this.m_TextureShape.intValue) == -1)
        this.m_TextureShape.intValue = array[0];
      EditorGUILayout.Space();
      if (!this.m_TextureType.hasMultipleDifferentValues)
      {
        this.DoGUIElements(this.m_TextureTypeGUIElements[index].commonElements, this.m_GUIElementsDisplayOrder);
        if (this.m_TextureTypeGUIElements[index].advancedElements != TextureImporterInspector.TextureInspectorGUIElement.None)
        {
          EditorGUILayout.Space();
          this.m_ShowAdvanced = EditorGUILayout.Foldout(this.m_ShowAdvanced, TextureImporterInspector.s_Styles.showAdvanced, true);
          if (this.m_ShowAdvanced)
          {
            ++EditorGUI.indentLevel;
            this.DoGUIElements(this.m_TextureTypeGUIElements[index].advancedElements, this.m_GUIElementsDisplayOrder);
            --EditorGUI.indentLevel;
          }
        }
      }
      EditorGUILayout.Space();
      this.TextureSettingsGUI();
      this.ShowPlatformSpecificSettings();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      this.ApplyRevertGUI();
      GUILayout.EndHorizontal();
      this.UpdateImportWarning();
      if (this.m_ImportWarning != null)
        EditorGUILayout.HelpBox(this.m_ImportWarning, MessageType.Warning);
      GUI.enabled = enabled;
    }

    private bool ShouldDisplayGUIElement(TextureImporterInspector.TextureInspectorGUIElement guiElements, TextureImporterInspector.TextureInspectorGUIElement guiElement)
    {
      return (guiElements & guiElement) == guiElement;
    }

    private void DoGUIElements(TextureImporterInspector.TextureInspectorGUIElement guiElements, List<TextureImporterInspector.TextureInspectorGUIElement> guiElementsDisplayOrder)
    {
      foreach (TextureImporterInspector.TextureInspectorGUIElement index in guiElementsDisplayOrder)
      {
        if (this.ShouldDisplayGUIElement(guiElements, index) && this.m_GUIElementMethods.ContainsKey(index))
          this.m_GUIElementMethods[index](guiElements);
      }
    }

    private void ApplySettingsToTexture()
    {
      foreach (AssetImporter target in this.targets)
      {
        Texture tex = AssetDatabase.LoadMainAssetAtPath(target.assetPath) as Texture;
        if ((UnityEngine.Object) tex != (UnityEngine.Object) null)
        {
          if (this.m_Aniso.intValue != -1)
            TextureUtil.SetAnisoLevelNoDirty(tex, this.m_Aniso.intValue);
          if (this.m_FilterMode.intValue != -1)
            TextureUtil.SetFilterModeNoDirty(tex, (UnityEngine.FilterMode) this.m_FilterMode.intValue);
          if ((this.m_WrapU.intValue != -1 || this.m_WrapV.intValue != -1 || this.m_WrapW.intValue != -1) && (!this.m_WrapU.hasMultipleDifferentValues && !this.m_WrapV.hasMultipleDifferentValues && !this.m_WrapW.hasMultipleDifferentValues))
            TextureUtil.SetWrapModeNoDirty(tex, (TextureWrapMode) this.m_WrapU.intValue, (TextureWrapMode) this.m_WrapV.intValue, (TextureWrapMode) this.m_WrapW.intValue);
        }
      }
      SceneView.RepaintAll();
    }

    private static bool CountImportersWithAlpha(UnityEngine.Object[] importers, out int count)
    {
      try
      {
        count = 0;
        foreach (UnityEngine.Object importer in importers)
        {
          if ((importer as TextureImporter).DoesSourceTextureHaveAlpha())
            ++count;
        }
        return true;
      }
      catch
      {
        count = importers.Length;
        return false;
      }
    }

    private static bool CountImportersWithHDR(UnityEngine.Object[] importers, out int count)
    {
      try
      {
        count = 0;
        foreach (UnityEngine.Object importer in importers)
        {
          if ((importer as TextureImporter).IsSourceTextureHDR())
            ++count;
        }
        return true;
      }
      catch
      {
        count = importers.Length;
        return false;
      }
    }

    private void SetCookieMode(TextureImporterInspector.CookieMode cm)
    {
      switch (cm)
      {
        case TextureImporterInspector.CookieMode.Spot:
          this.m_BorderMipMap.intValue = 1;
          SerializedProperty wrapU1 = this.m_WrapU;
          int num1 = 1;
          this.m_WrapW.intValue = num1;
          int num2 = num1;
          this.m_WrapV.intValue = num2;
          int num3 = num2;
          wrapU1.intValue = num3;
          this.m_GenerateCubemap.intValue = 6;
          this.m_TextureShape.intValue = 1;
          break;
        case TextureImporterInspector.CookieMode.Directional:
          this.m_BorderMipMap.intValue = 0;
          SerializedProperty wrapU2 = this.m_WrapU;
          int num4 = 0;
          this.m_WrapW.intValue = num4;
          int num5 = num4;
          this.m_WrapV.intValue = num5;
          int num6 = num5;
          wrapU2.intValue = num6;
          this.m_GenerateCubemap.intValue = 6;
          this.m_TextureShape.intValue = 1;
          break;
        case TextureImporterInspector.CookieMode.Point:
          this.m_BorderMipMap.intValue = 0;
          SerializedProperty wrapU3 = this.m_WrapU;
          int num7 = 1;
          this.m_WrapW.intValue = num7;
          int num8 = num7;
          this.m_WrapV.intValue = num8;
          int num9 = num8;
          wrapU3.intValue = num9;
          this.m_GenerateCubemap.intValue = 1;
          this.m_TextureShape.intValue = 2;
          break;
      }
    }

    private void SyncPlatformSettings()
    {
      foreach (TextureImportPlatformSettings platformSetting in this.m_PlatformSettings)
        platformSetting.Sync();
    }

    internal static string[] BuildTextureStrings(int[] texFormatValues)
    {
      string[] strArray = new string[texFormatValues.Length];
      for (int index = 0; index < texFormatValues.Length; ++index)
      {
        int texFormatValue = texFormatValues[index];
        strArray[index] = " " + TextureUtil.GetTextureFormatString((TextureFormat) texFormatValue);
      }
      return strArray;
    }

    internal static void InitializeTextureFormatStrings()
    {
      if (TextureImporterInspector.s_TextureFormatStringsApplePVR == null)
        TextureImporterInspector.s_TextureFormatStringsApplePVR = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueApplePVR);
      if (TextureImporterInspector.s_TextureFormatStringsAndroid == null)
        TextureImporterInspector.s_TextureFormatStringsAndroid = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueAndroid);
      if (TextureImporterInspector.s_TextureFormatStringsTizen == null)
        TextureImporterInspector.s_TextureFormatStringsTizen = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueTizen);
      if (TextureImporterInspector.s_TextureFormatStringsWebGL == null)
        TextureImporterInspector.s_TextureFormatStringsWebGL = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueWebGL);
      if (TextureImporterInspector.s_TextureFormatStringsWiiU == null)
        TextureImporterInspector.s_TextureFormatStringsWiiU = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueWiiU);
      if (TextureImporterInspector.s_TextureFormatStringsPSP2 == null)
        TextureImporterInspector.s_TextureFormatStringsPSP2 = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValuePSP2);
      if (TextureImporterInspector.s_TextureFormatStringsSwitch == null)
        TextureImporterInspector.s_TextureFormatStringsSwitch = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueSwitch);
      if (TextureImporterInspector.s_TextureFormatStringsDefault == null)
        TextureImporterInspector.s_TextureFormatStringsDefault = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueDefault);
      if (TextureImporterInspector.s_NormalFormatStringsDefault == null)
        TextureImporterInspector.s_NormalFormatStringsDefault = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kNormalFormatsValueDefault);
      if (TextureImporterInspector.s_TextureFormatStringsSingleChannel != null)
        return;
      TextureImporterInspector.s_TextureFormatStringsSingleChannel = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueSingleChannel);
    }

    internal static bool IsFormatRequireCompressionSetting(TextureImporterFormat format)
    {
      return ArrayUtility.Contains<TextureImporterFormat>(TextureImporterInspector.kFormatsWithCompressionSettings, format);
    }

    protected void ShowPlatformSpecificSettings()
    {
      BuildPlatform[] array = ((IEnumerable<BuildPlatform>) TextureImporterInspector.GetBuildPlayerValidPlatforms()).ToArray<BuildPlatform>();
      GUILayout.Space(10f);
      int index = EditorGUILayout.BeginPlatformGrouping(array, TextureImporterInspector.s_Styles.defaultPlatform);
      TextureImportPlatformSettings platformSetting = this.m_PlatformSettings[index + 1];
      if (!platformSetting.isDefault)
      {
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = platformSetting.overriddenIsDifferent;
        bool overridden = EditorGUILayout.ToggleLeft("Override for " + array[index].title.text, platformSetting.overridden);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
          platformSetting.SetOverriddenForAll(overridden);
          this.SyncPlatformSettings();
        }
      }
      using (new EditorGUI.DisabledScope(!platformSetting.isDefault && !platformSetting.allAreOverridden))
      {
        ModuleManager.GetTextureImportSettingsExtension(platformSetting.m_Target).ShowImportSettings((Editor) this, platformSetting);
        this.SyncPlatformSettings();
      }
      EditorGUILayout.EndPlatformGrouping();
    }

    private static bool IsPowerOfTwo(int f)
    {
      return (f & f - 1) == 0;
    }

    public static BuildPlatform[] GetBuildPlayerValidPlatforms()
    {
      return BuildPlatforms.instance.GetValidPlatforms().ToArray();
    }

    public virtual void BuildTargetList()
    {
      BuildPlatform[] playerValidPlatforms = TextureImporterInspector.GetBuildPlayerValidPlatforms();
      this.m_PlatformSettings = new List<TextureImportPlatformSettings>();
      this.m_PlatformSettings.Add(new TextureImportPlatformSettings(TextureImporterInspector.s_DefaultPlatformName, BuildTarget.StandaloneWindows, this));
      foreach (BuildPlatform buildPlatform in playerValidPlatforms)
        this.m_PlatformSettings.Add(new TextureImportPlatformSettings(buildPlatform.name, buildPlatform.defaultTarget, this));
    }

    public override bool HasModified()
    {
      if (base.HasModified())
        return true;
      foreach (TextureImportPlatformSettings platformSetting in this.m_PlatformSettings)
      {
        if (platformSetting.HasChanged())
          return true;
      }
      return false;
    }

    public static void SelectMainAssets(UnityEngine.Object[] targets)
    {
      ArrayList arrayList = new ArrayList();
      foreach (AssetImporter target in targets)
      {
        Texture texture = AssetDatabase.LoadMainAssetAtPath(target.assetPath) as Texture;
        if ((bool) ((UnityEngine.Object) texture))
          arrayList.Add((object) texture);
      }
      if (arrayList.Count <= 0)
        return;
      Selection.objects = arrayList.ToArray(typeof (UnityEngine.Object)) as UnityEngine.Object[];
    }

    protected override void ResetValues()
    {
      base.ResetValues();
      this.CacheSerializedProperties();
      this.BuildTargetList();
      this.ApplySettingsToTexture();
      TextureImporterInspector.SelectMainAssets(this.targets);
    }

    protected override void Apply()
    {
      SpriteEditorWindow.TextureImporterApply(this.serializedObject);
      base.Apply();
      this.SyncPlatformSettings();
      foreach (TextureImportPlatformSettings platformSetting in this.m_PlatformSettings)
        platformSetting.Apply();
    }

    [System.Flags]
    private enum TextureInspectorGUIElement
    {
      None = 0,
      PowerOfTwo = 1,
      Readable = 2,
      AlphaHandling = 4,
      ColorSpace = 8,
      MipMaps = 16, // 0x00000010
      NormalMap = 32, // 0x00000020
      Sprite = 64, // 0x00000040
      Cookie = 128, // 0x00000080
      CubeMapConvolution = 256, // 0x00000100
      CubeMapping = 512, // 0x00000200
    }

    private struct TextureInspectorTypeGUIProperties
    {
      public TextureImporterInspector.TextureInspectorGUIElement commonElements;
      public TextureImporterInspector.TextureInspectorGUIElement advancedElements;
      public TextureImporterShape shapeCaps;

      public TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement _commonElements, TextureImporterInspector.TextureInspectorGUIElement _advancedElements, TextureImporterShape _shapeCaps)
      {
        this.commonElements = _commonElements;
        this.advancedElements = _advancedElements;
        this.shapeCaps = _shapeCaps;
      }
    }

    private delegate void GUIMethod(TextureImporterInspector.TextureInspectorGUIElement guiElements);

    private enum CookieMode
    {
      Spot,
      Directional,
      Point,
    }

    internal class Styles
    {
      public readonly GUIContent textureTypeTitle = EditorGUIUtility.TextContent("Texture Type|What will this texture be used for?");
      public readonly GUIContent[] textureTypeOptions = new GUIContent[8]{ EditorGUIUtility.TextContent("Default|Texture is a normal image such as a diffuse texture or other."), EditorGUIUtility.TextContent("Normal map|Texture is a bump or normal map."), EditorGUIUtility.TextContent("Editor GUI and Legacy GUI|Texture is used for a GUI element."), EditorGUIUtility.TextContent("Sprite (2D and UI)|Texture is used for a sprite."), EditorGUIUtility.TextContent("Cursor|Texture is used for a cursor."), EditorGUIUtility.TextContent("Cookie|Texture is a cookie you put on a light."), EditorGUIUtility.TextContent("Lightmap|Texture is a lightmap."), EditorGUIUtility.TextContent("Single Channel|Texture is a one component texture.") };
      public readonly int[] textureTypeValues = new int[8]{ 0, 1, 2, 8, 7, 4, 6, 10 };
      public readonly GUIContent textureShape = EditorGUIUtility.TextContent("Texture Shape|What shape is this texture?");
      private readonly GUIContent textureShape2D = EditorGUIUtility.TextContent("2D|Texture is 2D.");
      private readonly GUIContent textureShapeCube = EditorGUIUtility.TextContent("Cube|Texture is a Cubemap.");
      public readonly Dictionary<TextureImporterShape, GUIContent[]> textureShapeOptionsDictionnary = new Dictionary<TextureImporterShape, GUIContent[]>();
      public readonly Dictionary<TextureImporterShape, int[]> textureShapeValuesDictionnary = new Dictionary<TextureImporterShape, int[]>();
      public readonly GUIContent filterMode = EditorGUIUtility.TextContent("Filter Mode");
      public readonly GUIContent[] filterModeOptions = new GUIContent[3]{ EditorGUIUtility.TextContent("Point (no filter)"), EditorGUIUtility.TextContent("Bilinear"), EditorGUIUtility.TextContent("Trilinear") };
      public readonly GUIContent cookieType = EditorGUIUtility.TextContent("Light Type");
      public readonly GUIContent[] cookieOptions = new GUIContent[3]{ EditorGUIUtility.TextContent("Spotlight"), EditorGUIUtility.TextContent("Directional"), EditorGUIUtility.TextContent("Point") };
      public readonly GUIContent generateFromBump = EditorGUIUtility.TextContent("Create from Grayscale|The grayscale of the image is used as a heightmap for generating the normal map.");
      public readonly GUIContent bumpiness = EditorGUIUtility.TextContent("Bumpiness");
      public readonly GUIContent bumpFiltering = EditorGUIUtility.TextContent("Filtering");
      public readonly GUIContent[] bumpFilteringOptions = new GUIContent[2]{ EditorGUIUtility.TextContent("Sharp"), EditorGUIUtility.TextContent("Smooth") };
      public readonly GUIContent cubemap = EditorGUIUtility.TextContent("Mapping");
      public readonly GUIContent[] cubemapOptions = new GUIContent[4]{ EditorGUIUtility.TextContent("Auto"), EditorGUIUtility.TextContent("6 Frames Layout (Cubic Environment)|Texture contains 6 images arranged in one of the standard cubemap layouts - cross or sequence (+x,-x, +y, -y, +z, -z). Texture can be in vertical or horizontal orientation."), EditorGUIUtility.TextContent("Latitude-Longitude Layout (Cylindrical)|Texture contains an image of a ball unwrapped such that latitude and longitude are mapped to horizontal and vertical dimensions (as on a globe)."), EditorGUIUtility.TextContent("Mirrored Ball (Spheremap)|Texture contains an image of a mirrored ball.") };
      public readonly int[] cubemapValues2 = new int[4]{ 6, 5, 2, 1 };
      public readonly GUIContent cubemapConvolution = EditorGUIUtility.TextContent("Convolution Type");
      public readonly GUIContent[] cubemapConvolutionOptions = new GUIContent[3]{ EditorGUIUtility.TextContent("None"), EditorGUIUtility.TextContent("Specular (Glossy Reflection)|Convolve cubemap for specular reflections with varying smoothness (Glossy Reflections)."), EditorGUIUtility.TextContent("Diffuse (Irradiance)|Convolve cubemap for diffuse-only reflection (Irradiance Cubemap).") };
      public readonly int[] cubemapConvolutionValues = new int[3]{ 0, 1, 2 };
      public readonly GUIContent seamlessCubemap = EditorGUIUtility.TextContent("Fixup Edge Seams|Enable if this texture is used for glossy reflections.");
      public readonly GUIContent textureFormat = EditorGUIUtility.TextContent("Format");
      public readonly GUIContent defaultPlatform = EditorGUIUtility.TextContent("Default");
      public readonly GUIContent mipmapFadeOutToggle = EditorGUIUtility.TextContent("Fadeout Mip Maps");
      public readonly GUIContent mipmapFadeOut = EditorGUIUtility.TextContent("Fade Range");
      public readonly GUIContent readWrite = EditorGUIUtility.TextContent("Read/Write Enabled|Enable to be able to access the raw pixel data from code.");
      public readonly GUIContent alphaSource = EditorGUIUtility.TextContent("Alpha Source|How is the alpha generated for the imported texture.");
      public readonly GUIContent[] alphaSourceOptions = new GUIContent[3]{ EditorGUIUtility.TextContent("None|No Alpha will be used."), EditorGUIUtility.TextContent("Input Texture Alpha|Use Alpha from the input texture if one is provided."), EditorGUIUtility.TextContent("From Gray Scale|Generate Alpha from image gray scale.") };
      public readonly int[] alphaSourceValues = new int[3]{ 0, 1, 2 };
      public readonly GUIContent generateMipMaps = EditorGUIUtility.TextContent("Generate Mip Maps");
      public readonly GUIContent sRGBTexture = EditorGUIUtility.TextContent("sRGB (Color Texture)|Texture content is stored in gamma space. Non-HDR color textures should enable this flag (except if used for IMGUI).");
      public readonly GUIContent borderMipMaps = EditorGUIUtility.TextContent("Border Mip Maps");
      public readonly GUIContent mipMapsPreserveCoverage = EditorGUIUtility.TextContent("Mip Maps Preserve Coverage|The alpha channel of generated Mip Maps will preserve coverage during the alpha test.");
      public readonly GUIContent alphaTestReferenceValue = EditorGUIUtility.TextContent("Alpha Cutoff Value|The reference value used during the alpha test. Controls Mip Map coverage.");
      public readonly GUIContent mipMapFilter = EditorGUIUtility.TextContent("Mip Map Filtering");
      public readonly GUIContent[] mipMapFilterOptions = new GUIContent[2]{ EditorGUIUtility.TextContent("Box"), EditorGUIUtility.TextContent("Kaiser") };
      public readonly GUIContent npot = EditorGUIUtility.TextContent("Non Power of 2|How non-power-of-two textures are scaled on import.");
      public readonly GUIContent generateCubemap = EditorGUIUtility.TextContent("Generate Cubemap");
      public readonly GUIContent compressionQuality = EditorGUIUtility.TextContent("Compressor Quality");
      public readonly GUIContent compressionQualitySlider = EditorGUIUtility.TextContent("Compressor Quality|Use the slider to adjust compression quality from 0 (Fastest) to 100 (Best)");
      public readonly GUIContent[] mobileCompressionQualityOptions = new GUIContent[3]{ EditorGUIUtility.TextContent("Fast"), EditorGUIUtility.TextContent("Normal"), EditorGUIUtility.TextContent("Best") };
      public readonly GUIContent spriteMode = EditorGUIUtility.TextContent("Sprite Mode");
      public readonly GUIContent[] spriteModeOptions = new GUIContent[3]{ EditorGUIUtility.TextContent("Single"), EditorGUIUtility.TextContent("Multiple"), EditorGUIUtility.TextContent("Polygon") };
      public readonly GUIContent[] spriteMeshTypeOptions = new GUIContent[2]{ EditorGUIUtility.TextContent("Full Rect"), EditorGUIUtility.TextContent("Tight") };
      public readonly GUIContent spritePackingTag = EditorGUIUtility.TextContent("Packing Tag|Tag for the Sprite Packing system.");
      public readonly GUIContent spritePixelsPerUnit = EditorGUIUtility.TextContent("Pixels Per Unit|How many pixels in the sprite correspond to one unit in the world.");
      public readonly GUIContent spriteExtrude = EditorGUIUtility.TextContent("Extrude Edges|How much empty area to leave around the sprite in the generated mesh.");
      public readonly GUIContent spriteMeshType = EditorGUIUtility.TextContent("Mesh Type|Type of sprite mesh to generate.");
      public readonly GUIContent spriteAlignment = EditorGUIUtility.TextContent("Pivot|Sprite pivot point in its localspace. May be used for syncing animation frames of different sizes.");
      public readonly GUIContent[] spriteAlignmentOptions = new GUIContent[10]{ EditorGUIUtility.TextContent("Center"), EditorGUIUtility.TextContent("Top Left"), EditorGUIUtility.TextContent("Top"), EditorGUIUtility.TextContent("Top Right"), EditorGUIUtility.TextContent("Left"), EditorGUIUtility.TextContent("Right"), EditorGUIUtility.TextContent("Bottom Left"), EditorGUIUtility.TextContent("Bottom"), EditorGUIUtility.TextContent("Bottom Right"), EditorGUIUtility.TextContent("Custom") };
      public readonly GUIContent alphaIsTransparency = EditorGUIUtility.TextContent("Alpha Is Transparency|If the provided alpha channel is transparency, enable this to pre-filter the color to avoid texture filtering artifacts. This is not supported for HDR textures.");
      public readonly GUIContent etc1Compression = EditorGUIUtility.TextContent("Compress using ETC1 (split alpha channel)|Alpha for this texture will be preserved by splitting the alpha channel to another texture, and both resulting textures will be compressed using ETC1.");
      public readonly GUIContent crunchedCompression = EditorGUIUtility.TextContent("Use Crunch Compression|Texture is crunch-compressed to save space on disk when applicable.");
      public readonly GUIContent showAdvanced = EditorGUIUtility.TextContent("Advanced|Show advanced settings.");

      public Styles()
      {
        GUIContent[] guiContentArray1 = new GUIContent[1]{ this.textureShape2D };
        GUIContent[] guiContentArray2 = new GUIContent[1]{ this.textureShapeCube };
        GUIContent[] guiContentArray3 = new GUIContent[2]{ this.textureShape2D, this.textureShapeCube };
        this.textureShapeOptionsDictionnary.Add(TextureImporterShape.Texture2D, guiContentArray1);
        this.textureShapeOptionsDictionnary.Add(TextureImporterShape.TextureCube, guiContentArray2);
        this.textureShapeOptionsDictionnary.Add(TextureImporterShape.Texture2D | TextureImporterShape.TextureCube, guiContentArray3);
        int[] numArray1 = new int[1]{ 1 };
        int[] numArray2 = new int[1]{ 2 };
        int[] numArray3 = new int[2]{ 1, 2 };
        this.textureShapeValuesDictionnary.Add(TextureImporterShape.Texture2D, numArray1);
        this.textureShapeValuesDictionnary.Add(TextureImporterShape.TextureCube, numArray2);
        this.textureShapeValuesDictionnary.Add(TextureImporterShape.Texture2D | TextureImporterShape.TextureCube, numArray3);
      }
    }
  }
}
