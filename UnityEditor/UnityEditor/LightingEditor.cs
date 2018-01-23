// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightingEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [CustomEditor(typeof (RenderSettings))]
  internal class LightingEditor : Editor
  {
    protected SerializedProperty m_Sun;
    protected SerializedProperty m_AmbientSource;
    protected SerializedProperty m_AmbientSkyColor;
    protected SerializedProperty m_AmbientEquatorColor;
    protected SerializedProperty m_AmbientGroundColor;
    protected SerializedProperty m_AmbientIntensity;
    protected SerializedProperty m_AmbientLightingMode;
    protected SerializedProperty m_ReflectionIntensity;
    protected SerializedProperty m_ReflectionBounces;
    protected SerializedProperty m_SkyboxMaterial;
    protected SerializedProperty m_DefaultReflectionMode;
    protected SerializedProperty m_DefaultReflectionResolution;
    protected SerializedProperty m_CustomReflection;
    protected SerializedProperty m_ReflectionCompression;
    protected SerializedObject m_LightmapSettings;
    private bool m_bShowEnvironment;
    private const string kShowEnvironment = "ShowEnvironment";

    public virtual void OnEnable()
    {
      this.m_Sun = this.serializedObject.FindProperty("m_Sun");
      this.m_AmbientSource = this.serializedObject.FindProperty("m_AmbientMode");
      this.m_AmbientSkyColor = this.serializedObject.FindProperty("m_AmbientSkyColor");
      this.m_AmbientEquatorColor = this.serializedObject.FindProperty("m_AmbientEquatorColor");
      this.m_AmbientGroundColor = this.serializedObject.FindProperty("m_AmbientGroundColor");
      this.m_AmbientIntensity = this.serializedObject.FindProperty("m_AmbientIntensity");
      this.m_ReflectionIntensity = this.serializedObject.FindProperty("m_ReflectionIntensity");
      this.m_ReflectionBounces = this.serializedObject.FindProperty("m_ReflectionBounces");
      this.m_SkyboxMaterial = this.serializedObject.FindProperty("m_SkyboxMaterial");
      this.m_DefaultReflectionMode = this.serializedObject.FindProperty("m_DefaultReflectionMode");
      this.m_DefaultReflectionResolution = this.serializedObject.FindProperty("m_DefaultReflectionResolution");
      this.m_CustomReflection = this.serializedObject.FindProperty("m_CustomReflection");
      this.m_LightmapSettings = new SerializedObject(LightmapEditorSettings.GetLightmapSettings());
      this.m_ReflectionCompression = this.m_LightmapSettings.FindProperty("m_LightmapEditorSettings.m_ReflectionCompression");
      this.m_AmbientLightingMode = this.m_LightmapSettings.FindProperty("m_GISettings.m_EnvironmentLightingMode");
      this.m_bShowEnvironment = SessionState.GetBool("ShowEnvironment", true);
    }

    public virtual void OnDisable()
    {
      SessionState.SetBool("ShowEnvironment", this.m_bShowEnvironment);
    }

    private void DrawGUI()
    {
      Material objectReferenceValue = this.m_SkyboxMaterial.objectReferenceValue as Material;
      this.m_bShowEnvironment = EditorGUILayout.FoldoutTitlebar(this.m_bShowEnvironment, LightingEditor.Styles.env_top, true);
      if (!this.m_bShowEnvironment)
        return;
      ++EditorGUI.indentLevel;
      EditorGUILayout.PropertyField(this.m_SkyboxMaterial, LightingEditor.Styles.env_skybox_mat, new GUILayoutOption[0]);
      if ((bool) ((Object) objectReferenceValue) && !EditorMaterialUtility.IsBackgroundMaterial(objectReferenceValue))
        EditorGUILayout.HelpBox(LightingEditor.Styles.skyboxWarning.text, MessageType.Warning);
      EditorGUILayout.PropertyField(this.m_Sun, LightingEditor.Styles.env_skybox_sun, new GUILayoutOption[0]);
      EditorGUILayout.Space();
      EditorGUILayout.LabelField(LightingEditor.Styles.env_amb_top);
      ++EditorGUI.indentLevel;
      EditorGUILayout.IntPopup(this.m_AmbientSource, LightingEditor.Styles.kFullAmbientSource, LightingEditor.Styles.kFullAmbientSourceValues, LightingEditor.Styles.env_amb_src, new GUILayoutOption[0]);
      switch ((AmbientMode) this.m_AmbientSource.intValue)
      {
        case AmbientMode.Skybox:
          if ((Object) objectReferenceValue == (Object) null)
          {
            EditorGUI.BeginChangeCheck();
            Color color = EditorGUILayout.ColorField(LightingEditor.Styles.ambient, this.m_AmbientSkyColor.colorValue, true, false, true, ColorPicker.defaultHDRConfig);
            if (EditorGUI.EndChangeCheck())
            {
              this.m_AmbientSkyColor.colorValue = color;
              break;
            }
            break;
          }
          EditorGUILayout.Slider(this.m_AmbientIntensity, 0.0f, 8f, LightingEditor.Styles.env_amb_int, new GUILayoutOption[0]);
          break;
        case AmbientMode.Trilight:
          EditorGUI.BeginChangeCheck();
          Color color1 = EditorGUILayout.ColorField(LightingEditor.Styles.ambientUp, this.m_AmbientSkyColor.colorValue, true, false, true, ColorPicker.defaultHDRConfig);
          Color color2 = EditorGUILayout.ColorField(LightingEditor.Styles.ambientMid, this.m_AmbientEquatorColor.colorValue, true, false, true, ColorPicker.defaultHDRConfig);
          Color color3 = EditorGUILayout.ColorField(LightingEditor.Styles.ambientDown, this.m_AmbientGroundColor.colorValue, true, false, true, ColorPicker.defaultHDRConfig);
          if (EditorGUI.EndChangeCheck())
          {
            this.m_AmbientSkyColor.colorValue = color1;
            this.m_AmbientEquatorColor.colorValue = color2;
            this.m_AmbientGroundColor.colorValue = color3;
            break;
          }
          break;
        case AmbientMode.Flat:
          EditorGUI.BeginChangeCheck();
          Color color4 = EditorGUILayout.ColorField(LightingEditor.Styles.ambient, this.m_AmbientSkyColor.colorValue, true, false, true, ColorPicker.defaultHDRConfig);
          if (EditorGUI.EndChangeCheck())
          {
            this.m_AmbientSkyColor.colorValue = color4;
            break;
          }
          break;
      }
      if (LightModeUtil.Get().IsAnyGIEnabled())
      {
        int mode;
        bool ambientLightingMode = LightModeUtil.Get().GetAmbientLightingMode(out mode);
        using (new EditorGUI.DisabledScope(!ambientLightingMode))
        {
          int[] optionValues = new int[2]{ 0, 1 };
          if (ambientLightingMode)
            this.m_AmbientLightingMode.intValue = EditorGUILayout.IntPopup(LightingEditor.Styles.AmbientLightingMode, this.m_AmbientLightingMode.intValue, LightingEditor.Styles.AmbientLightingModes, optionValues, new GUILayoutOption[0]);
          else
            EditorGUILayout.IntPopup(LightingEditor.Styles.AmbientLightingMode, mode, LightingEditor.Styles.AmbientLightingModes, optionValues, new GUILayoutOption[0]);
        }
      }
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
      EditorGUILayout.LabelField(LightingEditor.Styles.env_refl_top);
      ++EditorGUI.indentLevel;
      EditorGUILayout.PropertyField(this.m_DefaultReflectionMode, LightingEditor.Styles.env_refl_src, new GUILayoutOption[0]);
      switch ((DefaultReflectionMode) this.m_DefaultReflectionMode.intValue)
      {
        case DefaultReflectionMode.FromSkybox:
          int[] resolutionList = (int[]) null;
          GUIContent[] resolutionStringList = (GUIContent[]) null;
          ReflectionProbeEditor.GetResolutionArray(ref resolutionList, ref resolutionStringList);
          EditorGUILayout.IntPopup(this.m_DefaultReflectionResolution, resolutionStringList, resolutionList, LightingEditor.Styles.env_refl_res, new GUILayoutOption[1]
          {
            GUILayout.MinWidth(40f)
          });
          break;
        case DefaultReflectionMode.Custom:
          EditorGUILayout.PropertyField(this.m_CustomReflection, LightingEditor.Styles.customReflection, new GUILayoutOption[0]);
          break;
      }
      EditorGUILayout.PropertyField(this.m_ReflectionCompression, LightingEditor.Styles.env_refl_cmp, new GUILayoutOption[0]);
      EditorGUILayout.Slider(this.m_ReflectionIntensity, 0.0f, 1f, LightingEditor.Styles.env_refl_int, new GUILayoutOption[0]);
      EditorGUILayout.IntSlider(this.m_ReflectionBounces, 1, 5, LightingEditor.Styles.env_refl_bnc, new GUILayoutOption[0]);
      --EditorGUI.indentLevel;
      --EditorGUI.indentLevel;
      EditorGUILayout.Space();
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.m_LightmapSettings.Update();
      this.DrawGUI();
      this.serializedObject.ApplyModifiedProperties();
      this.m_LightmapSettings.ApplyModifiedProperties();
    }

    internal static class Styles
    {
      public static readonly GUIContent env_top = EditorGUIUtility.TextContent("Environment");
      public static readonly GUIContent env_skybox_mat = EditorGUIUtility.TextContent("Skybox Material|Specifies the material that is used to simulate the sky or other distant background in the Scene.");
      public static readonly GUIContent env_skybox_sun = EditorGUIUtility.TextContent("Sun Source|Specifies the directional light that is used to indicate the direction of the sun when a procedural skybox is used. If set to None, the brightest directional light in the Scene is used to represent the sun.");
      public static readonly GUIContent env_amb_top = EditorGUIUtility.TextContent("Environment Lighting");
      public static readonly GUIContent env_amb_src = EditorGUIUtility.TextContent("Source|Specifies whether to use a skybox, gradient, or color for ambient light contributed to the Scene.");
      public static readonly GUIContent env_amb_int = EditorGUIUtility.TextContent("Intensity Multiplier|Controls the brightness of the skybox lighting in the Scene.");
      public static readonly GUIContent env_refl_top = EditorGUIUtility.TextContent("Environment Reflections");
      public static readonly GUIContent env_refl_src = EditorGUIUtility.TextContent("Source|Specifies whether to use the skybox or a custom cube map for reflection effects in the Scene.");
      public static readonly GUIContent env_refl_res = EditorGUIUtility.TextContent("Resolution|Controls the resolution for the cube map assigned to the skybox material for reflection effects in the Scene.");
      public static readonly GUIContent env_refl_cmp = EditorGUIUtility.TextContent("Compression|Controls how Unity compresses the reflection cube maps. Options are Auto, Compressed, and Uncompressed. Auto compresses the cube maps if the compression format is suitable.");
      public static readonly GUIContent env_refl_int = EditorGUIUtility.TextContent("Intensity Multiplier|Controls how much the skybox or custom cubemap affects reflections in the Scene. A value of 1 produces physically correct results.");
      public static readonly GUIContent env_refl_bnc = EditorGUIUtility.TextContent("Bounces|Controls how many times a reflection includes other reflections. A value of 1 results in the Scene being rendered once so mirrored reflections will be black. A value of 2 results in mirrored reflections being visible in the Scene.");
      public static readonly GUIContent skyboxWarning = EditorGUIUtility.TextContent("Shader of this material does not support skybox rendering.");
      public static readonly GUIContent createLight = EditorGUIUtility.TextContent("Create Light");
      public static readonly GUIContent ambientUp = EditorGUIUtility.TextContent("Sky Color|Controls the color of light emitted from the sky in the Scene.");
      public static readonly GUIContent ambientMid = EditorGUIUtility.TextContent("Equator Color|Controls the color of light emitted from the sides of the Scene.");
      public static readonly GUIContent ambientDown = EditorGUIUtility.TextContent("Ground Color|Controls the color of light emitted from the ground of the Scene.");
      public static readonly GUIContent ambient = EditorGUIUtility.TextContent("Ambient Color|Controls the color of the ambient light contributed to the Scene.");
      public static readonly GUIContent customReflection = EditorGUIUtility.TextContent("Cubemap|Specifies the custom cube map used for reflection effects in the Scene.");
      public static readonly GUIContent AmbientLightingMode = EditorGUIUtility.TextContent("Ambient Mode|Specifies the Global Illumination mode that should be used for handling ambient light in the Scene. Options are Realtime or Baked. This property is not editable unless both Realtime Global Illumination and Baked Global Illumination are enabled for the scene.");
      public static readonly GUIContent[] kFullAmbientSource = new GUIContent[3]{ EditorGUIUtility.TextContent("Skybox"), EditorGUIUtility.TextContent("Gradient"), EditorGUIUtility.TextContent("Color") };
      public static readonly GUIContent[] AmbientLightingModes = new GUIContent[2]{ EditorGUIUtility.TextContent("Realtime"), EditorGUIUtility.TextContent("Baked") };
      public static readonly int[] kFullAmbientSourceValues = new int[3]{ 0, 1, 3 };
    }
  }
}
