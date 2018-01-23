// Decompiled with JetBrains decompiler
// Type: UnityEditor.GraphicsSettingsWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  internal class GraphicsSettingsWindow
  {
    internal class BuiltinShaderSettings
    {
      private readonly SerializedProperty m_Mode;
      private readonly SerializedProperty m_Shader;
      private readonly GUIContent m_Label;

      internal BuiltinShaderSettings(string label, string name, SerializedObject serializedObject)
      {
        this.m_Mode = serializedObject.FindProperty(name + ".m_Mode");
        this.m_Shader = serializedObject.FindProperty(name + ".m_Shader");
        this.m_Label = EditorGUIUtility.TextContent(label);
      }

      internal void DoGUI()
      {
        EditorGUILayout.PropertyField(this.m_Mode, this.m_Label, new GUILayoutOption[0]);
        if (this.m_Mode.intValue != 2)
          return;
        EditorGUILayout.PropertyField(this.m_Shader);
      }

      internal enum BuiltinShaderMode
      {
        None,
        Builtin,
        Custom,
      }
    }

    internal class BuiltinShadersEditor : Editor
    {
      private GraphicsSettingsWindow.BuiltinShaderSettings m_Deferred;
      private GraphicsSettingsWindow.BuiltinShaderSettings m_DeferredReflections;
      private GraphicsSettingsWindow.BuiltinShaderSettings m_LegacyDeferred;
      private GraphicsSettingsWindow.BuiltinShaderSettings m_ScreenSpaceShadows;
      private GraphicsSettingsWindow.BuiltinShaderSettings m_DepthNormals;
      private GraphicsSettingsWindow.BuiltinShaderSettings m_MotionVectors;
      private GraphicsSettingsWindow.BuiltinShaderSettings m_LightHalo;
      private GraphicsSettingsWindow.BuiltinShaderSettings m_LensFlare;

      private string deferredString
      {
        get
        {
          return LocalizationDatabase.GetLocalizedString("Deferred|Shader used for Deferred Shading.");
        }
      }

      private string deferredReflString
      {
        get
        {
          return LocalizationDatabase.GetLocalizedString("Deferred Reflections|Shader used for Deferred reflection probes.");
        }
      }

      private string legacyDeferredString
      {
        get
        {
          return LocalizationDatabase.GetLocalizedString("Legacy Deferred|Shader used for Legacy (light prepass) Deferred Lighting.");
        }
      }

      private string screenShadowsString
      {
        get
        {
          return LocalizationDatabase.GetLocalizedString("Screen Space Shadows|Shader used for screen-space cascaded shadows.");
        }
      }

      private string depthNormalsString
      {
        get
        {
          return LocalizationDatabase.GetLocalizedString("Depth Normals|Shader used for depth and normals texture when enabled on a Camera.");
        }
      }

      private string motionVectorsString
      {
        get
        {
          return LocalizationDatabase.GetLocalizedString("Motion Vectors|Shader for generation of Motion Vectors when the rendering camera has renderMotionVectors set to true.");
        }
      }

      private string lightHaloString
      {
        get
        {
          return LocalizationDatabase.GetLocalizedString("Light Halo|Default Shader used for light halos.");
        }
      }

      private string lensFlareString
      {
        get
        {
          return LocalizationDatabase.GetLocalizedString("Lens Flare|Default Shader used for lens flares.");
        }
      }

      public void OnEnable()
      {
        this.m_Deferred = new GraphicsSettingsWindow.BuiltinShaderSettings(this.deferredString, "m_Deferred", this.serializedObject);
        this.m_DeferredReflections = new GraphicsSettingsWindow.BuiltinShaderSettings(this.deferredReflString, "m_DeferredReflections", this.serializedObject);
        this.m_LegacyDeferred = new GraphicsSettingsWindow.BuiltinShaderSettings(this.legacyDeferredString, "m_LegacyDeferred", this.serializedObject);
        this.m_ScreenSpaceShadows = new GraphicsSettingsWindow.BuiltinShaderSettings(this.screenShadowsString, "m_ScreenSpaceShadows", this.serializedObject);
        this.m_DepthNormals = new GraphicsSettingsWindow.BuiltinShaderSettings(this.depthNormalsString, "m_DepthNormals", this.serializedObject);
        this.m_MotionVectors = new GraphicsSettingsWindow.BuiltinShaderSettings(this.motionVectorsString, "m_MotionVectors", this.serializedObject);
        this.m_LightHalo = new GraphicsSettingsWindow.BuiltinShaderSettings(this.lightHaloString, "m_LightHalo", this.serializedObject);
        this.m_LensFlare = new GraphicsSettingsWindow.BuiltinShaderSettings(this.lensFlareString, "m_LensFlare", this.serializedObject);
      }

      public override void OnInspectorGUI()
      {
        this.serializedObject.Update();
        this.m_Deferred.DoGUI();
        EditorGUI.BeginChangeCheck();
        this.m_DeferredReflections.DoGUI();
        if (EditorGUI.EndChangeCheck())
          ShaderUtil.ReloadAllShaders();
        this.m_LegacyDeferred.DoGUI();
        this.m_ScreenSpaceShadows.DoGUI();
        this.m_DepthNormals.DoGUI();
        this.m_MotionVectors.DoGUI();
        this.m_LightHalo.DoGUI();
        this.m_LensFlare.DoGUI();
        this.serializedObject.ApplyModifiedProperties();
      }
    }

    internal class AlwaysIncludedShadersEditor : Editor
    {
      private SerializedProperty m_AlwaysIncludedShaders;

      public void OnEnable()
      {
        this.m_AlwaysIncludedShaders = this.serializedObject.FindProperty("m_AlwaysIncludedShaders");
        this.m_AlwaysIncludedShaders.isExpanded = true;
      }

      public override void OnInspectorGUI()
      {
        this.serializedObject.Update();
        EditorGUILayout.PropertyField(this.m_AlwaysIncludedShaders, true, new GUILayoutOption[0]);
        this.serializedObject.ApplyModifiedProperties();
      }
    }

    internal class ShaderStrippingEditor : Editor
    {
      private SerializedProperty m_LightmapStripping;
      private SerializedProperty m_LightmapKeepPlain;
      private SerializedProperty m_LightmapKeepDirCombined;
      private SerializedProperty m_LightmapKeepDynamicPlain;
      private SerializedProperty m_LightmapKeepDynamicDirCombined;
      private SerializedProperty m_LightmapKeepShadowMask;
      private SerializedProperty m_LightmapKeepSubtractive;
      private SerializedProperty m_FogStripping;
      private SerializedProperty m_FogKeepLinear;
      private SerializedProperty m_FogKeepExp;
      private SerializedProperty m_FogKeepExp2;
      private SerializedProperty m_InstancingStripping;

      public void OnEnable()
      {
        this.m_LightmapStripping = this.serializedObject.FindProperty("m_LightmapStripping");
        this.m_LightmapKeepPlain = this.serializedObject.FindProperty("m_LightmapKeepPlain");
        this.m_LightmapKeepDirCombined = this.serializedObject.FindProperty("m_LightmapKeepDirCombined");
        this.m_LightmapKeepDynamicPlain = this.serializedObject.FindProperty("m_LightmapKeepDynamicPlain");
        this.m_LightmapKeepDynamicDirCombined = this.serializedObject.FindProperty("m_LightmapKeepDynamicDirCombined");
        this.m_LightmapKeepShadowMask = this.serializedObject.FindProperty("m_LightmapKeepShadowMask");
        this.m_LightmapKeepSubtractive = this.serializedObject.FindProperty("m_LightmapKeepSubtractive");
        this.m_FogStripping = this.serializedObject.FindProperty("m_FogStripping");
        this.m_FogKeepLinear = this.serializedObject.FindProperty("m_FogKeepLinear");
        this.m_FogKeepExp = this.serializedObject.FindProperty("m_FogKeepExp");
        this.m_FogKeepExp2 = this.serializedObject.FindProperty("m_FogKeepExp2");
        this.m_InstancingStripping = this.serializedObject.FindProperty("m_InstancingStripping");
      }

      public override void OnInspectorGUI()
      {
        this.serializedObject.Update();
        bool flag1 = false;
        bool flag2 = false;
        EditorGUILayout.PropertyField(this.m_LightmapStripping, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapModes, new GUILayoutOption[0]);
        if (this.m_LightmapStripping.intValue != 0)
        {
          ++EditorGUI.indentLevel;
          EditorGUILayout.PropertyField(this.m_LightmapKeepPlain, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapPlain, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_LightmapKeepDirCombined, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapDirCombined, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_LightmapKeepDynamicPlain, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapDynamicPlain, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_LightmapKeepDynamicDirCombined, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapDynamicDirCombined, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_LightmapKeepShadowMask, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapKeepShadowMask, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_LightmapKeepSubtractive, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapKeepSubtractive, new GUILayoutOption[0]);
          EditorGUILayout.Space();
          EditorGUILayout.BeginHorizontal();
          EditorGUILayout.PrefixLabel(GUIContent.Temp(" "), EditorStyles.miniButton);
          if (GUILayout.Button(GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapFromScene, EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
            flag1 = true;
          EditorGUILayout.EndHorizontal();
          --EditorGUI.indentLevel;
          EditorGUILayout.Space();
        }
        EditorGUILayout.PropertyField(this.m_FogStripping, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.fogModes, new GUILayoutOption[0]);
        if (this.m_FogStripping.intValue != 0)
        {
          ++EditorGUI.indentLevel;
          EditorGUILayout.PropertyField(this.m_FogKeepLinear, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.fogLinear, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_FogKeepExp, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.fogExp, new GUILayoutOption[0]);
          EditorGUILayout.PropertyField(this.m_FogKeepExp2, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.fogExp2, new GUILayoutOption[0]);
          EditorGUILayout.Space();
          EditorGUILayout.BeginHorizontal();
          EditorGUILayout.PrefixLabel(GUIContent.Temp(" "), EditorStyles.miniButton);
          if (GUILayout.Button(GraphicsSettingsWindow.ShaderStrippingEditor.Styles.fogFromScene, EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
            flag2 = true;
          EditorGUILayout.EndHorizontal();
          --EditorGUI.indentLevel;
          EditorGUILayout.Space();
        }
        EditorGUILayout.PropertyField(this.m_InstancingStripping, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.instancingVariants, new GUILayoutOption[0]);
        this.serializedObject.ApplyModifiedProperties();
        if (flag1)
          ShaderUtil.CalculateLightmapStrippingFromCurrentScene();
        if (!flag2)
          return;
        ShaderUtil.CalculateFogStrippingFromCurrentScene();
      }

      internal class Styles
      {
        public static readonly GUIContent shaderSettings = EditorGUIUtility.TextContent("Platform shader settings");
        public static readonly GUIContent builtinSettings = EditorGUIUtility.TextContent("Built-in shader settings");
        public static readonly GUIContent shaderPreloadSettings = EditorGUIUtility.TextContent("Shader preloading");
        public static readonly GUIContent lightmapModes = EditorGUIUtility.TextContent("Lightmap Modes");
        public static readonly GUIContent lightmapPlain = EditorGUIUtility.TextContent("Baked Non-Directional|Include support for baked non-directional lightmaps.");
        public static readonly GUIContent lightmapDirCombined = EditorGUIUtility.TextContent("Baked Directional|Include support for baked directional lightmaps.");
        public static readonly GUIContent lightmapKeepShadowMask = EditorGUIUtility.TextContent("Baked Shadowmask|Include support for baked shadow occlusion.");
        public static readonly GUIContent lightmapKeepSubtractive = EditorGUIUtility.TextContent("Baked Subtractive|Include support for baked substractive lightmaps.");
        public static readonly GUIContent lightmapDynamicPlain = EditorGUIUtility.TextContent("Realtime Non-Directional|Include support for realtime non-directional lightmaps.");
        public static readonly GUIContent lightmapDynamicDirCombined = EditorGUIUtility.TextContent("Realtime Directional|Include support for realtime directional lightmaps.");
        public static readonly GUIContent lightmapFromScene = EditorGUIUtility.TextContent("Import From Current Scene|Calculate lightmap modes used by the current scene.");
        public static readonly GUIContent fogModes = EditorGUIUtility.TextContent("Fog Modes");
        public static readonly GUIContent fogLinear = EditorGUIUtility.TextContent("Linear|Include support for Linear fog.");
        public static readonly GUIContent fogExp = EditorGUIUtility.TextContent("Exponential|Include support for Exponential fog.");
        public static readonly GUIContent fogExp2 = EditorGUIUtility.TextContent("Exponential Squared|Include support for Exponential Squared fog.");
        public static readonly GUIContent fogFromScene = EditorGUIUtility.TextContent("Import From Current Scene|Calculate fog modes used by the current scene.");
        public static readonly GUIContent instancingVariants = EditorGUIUtility.TextContent("Instancing Variants");
        public static readonly GUIContent shaderPreloadSave = EditorGUIUtility.TextContent("Save to asset...|Save currently tracked shaders into a Shader Variant Manifest asset.");
        public static readonly GUIContent shaderPreloadClear = EditorGUIUtility.TextContent("Clear|Clear currently tracked shader variant information.");
      }
    }

    internal class ShaderPreloadEditor : Editor
    {
      private SerializedProperty m_PreloadedShaders;

      public void OnEnable()
      {
        this.m_PreloadedShaders = this.serializedObject.FindProperty("m_PreloadedShaders");
        this.m_PreloadedShaders.isExpanded = true;
      }

      public override void OnInspectorGUI()
      {
        this.serializedObject.Update();
        this.serializedObject.ApplyModifiedProperties();
        EditorGUILayout.PropertyField(this.m_PreloadedShaders, true, new GUILayoutOption[0]);
        EditorGUILayout.Space();
        GUILayout.Label(string.Format("Currently tracked: {0} shaders {1} total variants", (object) ShaderUtil.GetCurrentShaderVariantCollectionShaderCount(), (object) ShaderUtil.GetCurrentShaderVariantCollectionVariantCount()));
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(GraphicsSettingsWindow.ShaderPreloadEditor.Styles.shaderPreloadSave, EditorStyles.miniButton, new GUILayoutOption[0]))
        {
          string path = EditorUtility.SaveFilePanelInProject("Save Shader Variant Collection", "NewShaderVariants", "shadervariants", "Save shader variant collection", ProjectWindowUtil.GetActiveFolderPath());
          if (!string.IsNullOrEmpty(path))
            ShaderUtil.SaveCurrentShaderVariantCollection(path);
          GUIUtility.ExitGUI();
        }
        if (GUILayout.Button(GraphicsSettingsWindow.ShaderPreloadEditor.Styles.shaderPreloadClear, EditorStyles.miniButton, new GUILayoutOption[0]))
          ShaderUtil.ClearCurrentShaderVariantCollection();
        EditorGUILayout.EndHorizontal();
        this.serializedObject.ApplyModifiedProperties();
      }

      internal class Styles
      {
        public static readonly GUIContent shaderPreloadSave = EditorGUIUtility.TextContent("Save to asset...|Save currently tracked shaders into a Shader Variant Manifest asset.");
        public static readonly GUIContent shaderPreloadClear = EditorGUIUtility.TextContent("Clear|Clear currently tracked shader variant information.");
      }
    }

    internal class TierSettingsEditor : Editor
    {
      public bool verticalLayout = false;

      internal void OnFieldLabelsGUI(bool vertical)
      {
        if (!vertical)
          EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.standardShaderSettings, EditorStyles.boldLabel, new GUILayoutOption[0]);
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.standardShaderQuality);
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.reflectionProbeBoxProjection);
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.reflectionProbeBlending);
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.detailNormalMap);
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.semitransparentShadows);
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.enableLPPV);
        if (!vertical)
        {
          EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.renderingSettings, EditorStyles.boldLabel, new GUILayoutOption[0]);
        }
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.cascadedShadowMaps);
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.prefer32BitShadowMaps);
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.useHDR);
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.hdrMode);
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.renderingPath);
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.realtimeGICPUUsage);
      }

      internal ShaderQuality ShaderQualityPopup(ShaderQuality sq)
      {
        return (ShaderQuality) EditorGUILayout.IntPopup((int) sq, GraphicsSettingsWindow.TierSettingsEditor.Styles.shaderQualityName, GraphicsSettingsWindow.TierSettingsEditor.Styles.shaderQualityValue);
      }

      internal RenderingPath RenderingPathPopup(RenderingPath rp)
      {
        return (RenderingPath) EditorGUILayout.IntPopup((int) rp, GraphicsSettingsWindow.TierSettingsEditor.Styles.renderingPathName, GraphicsSettingsWindow.TierSettingsEditor.Styles.renderingPathValue);
      }

      internal CameraHDRMode HDRModePopup(CameraHDRMode mode)
      {
        return (CameraHDRMode) EditorGUILayout.IntPopup((int) mode, GraphicsSettingsWindow.TierSettingsEditor.Styles.hdrModeName, GraphicsSettingsWindow.TierSettingsEditor.Styles.hdrModeValue);
      }

      internal RealtimeGICPUUsage RealtimeGICPUUsagePopup(RealtimeGICPUUsage usage)
      {
        return (RealtimeGICPUUsage) EditorGUILayout.IntPopup((int) usage, GraphicsSettingsWindow.TierSettingsEditor.Styles.realtimeGICPUUsageName, GraphicsSettingsWindow.TierSettingsEditor.Styles.realtimeGICPUUsageValue);
      }

      internal void OnTierGUI(BuildTargetGroup platform, GraphicsTier tier, bool vertical)
      {
        TierSettings tierSettings = EditorGraphicsSettings.GetTierSettings(platform, tier);
        EditorGUI.BeginChangeCheck();
        if (!vertical)
          EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
        tierSettings.standardShaderQuality = this.ShaderQualityPopup(tierSettings.standardShaderQuality);
        tierSettings.reflectionProbeBoxProjection = EditorGUILayout.Toggle(tierSettings.reflectionProbeBoxProjection);
        tierSettings.reflectionProbeBlending = EditorGUILayout.Toggle(tierSettings.reflectionProbeBlending);
        tierSettings.detailNormalMap = EditorGUILayout.Toggle(tierSettings.detailNormalMap);
        tierSettings.semitransparentShadows = EditorGUILayout.Toggle(tierSettings.semitransparentShadows);
        tierSettings.enableLPPV = EditorGUILayout.Toggle(tierSettings.enableLPPV);
        if (!vertical)
        {
          EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
          EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
        }
        tierSettings.cascadedShadowMaps = EditorGUILayout.Toggle(tierSettings.cascadedShadowMaps);
        tierSettings.prefer32BitShadowMaps = EditorGUILayout.Toggle(tierSettings.prefer32BitShadowMaps);
        tierSettings.hdr = EditorGUILayout.Toggle(tierSettings.hdr);
        tierSettings.hdrMode = this.HDRModePopup(tierSettings.hdrMode);
        tierSettings.renderingPath = this.RenderingPathPopup(tierSettings.renderingPath);
        tierSettings.realtimeGICPUUsage = this.RealtimeGICPUUsagePopup(tierSettings.realtimeGICPUUsage);
        if (!EditorGUI.EndChangeCheck())
          return;
        EditorGraphicsSettings.RegisterUndoForGraphicsSettings();
        EditorGraphicsSettings.SetTierSettings(platform, tier, tierSettings);
      }

      internal void OnGuiHorizontal(BuildTargetGroup platform)
      {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUIUtility.labelWidth = 140f;
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
        this.OnFieldLabelsGUI(false);
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
        EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.autoSettings, EditorStyles.boldLabel, new GUILayoutOption[0]);
        EditorGUILayout.EndVertical();
        EditorGUIUtility.labelWidth = 50f;
        IEnumerator enumerator = Enum.GetValues(typeof (GraphicsTier)).GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            GraphicsTier current = (GraphicsTier) enumerator.Current;
            bool disabled = EditorGraphicsSettings.AreTierSettingsAutomatic(platform, current);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.tierName[(int) current], EditorStyles.boldLabel, new GUILayoutOption[0]);
            using (new EditorGUI.DisabledScope(disabled))
              this.OnTierGUI(platform, current, false);
            EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
            EditorGUI.BeginChangeCheck();
            bool automatic = EditorGUILayout.Toggle(disabled);
            if (EditorGUI.EndChangeCheck())
            {
              EditorGraphicsSettings.RegisterUndoForGraphicsSettings();
              EditorGraphicsSettings.MakeTierSettingsAutomatic(platform, current, automatic);
              EditorGraphicsSettings.OnUpdateTierSettingsImpl(platform, true);
            }
            EditorGUILayout.EndVertical();
          }
        }
        finally
        {
          IDisposable disposable;
          if ((disposable = enumerator as IDisposable) != null)
            disposable.Dispose();
        }
        EditorGUIUtility.labelWidth = 0.0f;
        EditorGUILayout.EndHorizontal();
      }

      internal void OnGuiVertical(BuildTargetGroup platform)
      {
        IEnumerator enumerator = Enum.GetValues(typeof (GraphicsTier)).GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            GraphicsTier current = (GraphicsTier) enumerator.Current;
            bool flag1 = EditorGraphicsSettings.AreTierSettingsAutomatic(platform, current);
            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 80f;
            EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.tierName[(int) current], EditorStyles.boldLabel, new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            EditorGUIUtility.labelWidth = 75f;
            bool flag2 = EditorGUILayout.Toggle(GraphicsSettingsWindow.TierSettingsEditor.Styles.autoSettings, flag1, new GUILayoutOption[0]);
            GUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
              EditorGraphicsSettings.RegisterUndoForGraphicsSettings();
              EditorGraphicsSettings.MakeTierSettingsAutomatic(platform, current, flag2);
              EditorGraphicsSettings.OnUpdateTierSettingsImpl(platform, true);
            }
            using (new EditorGUI.DisabledScope(flag2))
            {
              ++EditorGUI.indentLevel;
              EditorGUILayout.BeginHorizontal();
              EditorGUILayout.BeginVertical();
              EditorGUIUtility.labelWidth = 140f;
              this.OnFieldLabelsGUI(true);
              EditorGUILayout.EndVertical();
              EditorGUILayout.BeginVertical();
              EditorGUIUtility.labelWidth = 50f;
              this.OnTierGUI(platform, current, true);
              EditorGUILayout.EndVertical();
              GUILayout.EndHorizontal();
              --EditorGUI.indentLevel;
            }
          }
        }
        finally
        {
          IDisposable disposable;
          if ((disposable = enumerator as IDisposable) != null)
            disposable.Dispose();
        }
        EditorGUIUtility.labelWidth = 0.0f;
      }

      public override void OnInspectorGUI()
      {
        BuildPlatform[] array = BuildPlatforms.instance.GetValidPlatforms().ToArray();
        BuildTargetGroup targetGroup = array[EditorGUILayout.BeginPlatformGrouping(array, (GUIContent) null, GUIStyle.none)].targetGroup;
        if (this.verticalLayout)
          this.OnGuiVertical(targetGroup);
        else
          this.OnGuiHorizontal(targetGroup);
        EditorGUILayout.EndPlatformGrouping();
      }

      internal class Styles
      {
        public static readonly GUIContent[] shaderQualityName = new GUIContent[3]{ new GUIContent("Low"), new GUIContent("Medium"), new GUIContent("High") };
        public static readonly int[] shaderQualityValue = new int[3]{ 0, 1, 2 };
        public static readonly GUIContent[] renderingPathName = new GUIContent[4]{ new GUIContent("Forward"), new GUIContent("Deferred"), new GUIContent("Legacy Vertex Lit"), new GUIContent("Legacy Deferred (light prepass)") };
        public static readonly int[] renderingPathValue = new int[4]{ 1, 3, 0, 2 };
        public static readonly GUIContent[] hdrModeName = new GUIContent[2]{ new GUIContent("FP16"), new GUIContent("R11G11B10") };
        public static readonly int[] hdrModeValue = new int[2]{ 1, 2 };
        public static readonly GUIContent[] realtimeGICPUUsageName = new GUIContent[4]{ new GUIContent("Low"), new GUIContent("Medium"), new GUIContent("High"), new GUIContent("Unlimited") };
        public static readonly int[] realtimeGICPUUsageValue = new int[4]{ 25, 50, 75, 100 };
        public static readonly GUIContent[] tierName = new GUIContent[3]{ new GUIContent("Low (Tier1)"), new GUIContent("Medium (Tier 2)"), new GUIContent("High (Tier 3)") };
        public static readonly GUIContent empty = EditorGUIUtility.TextContent("");
        public static readonly GUIContent autoSettings = EditorGUIUtility.TextContent("Use Defaults");
        public static readonly GUIContent standardShaderSettings = EditorGUIUtility.TextContent("Standard Shader");
        public static readonly GUIContent renderingSettings = EditorGUIUtility.TextContent("Rendering");
        public static readonly GUIContent standardShaderQuality = EditorGUIUtility.TextContent("Standard Shader Quality");
        public static readonly GUIContent reflectionProbeBoxProjection = EditorGUIUtility.TextContent("Reflection Probes Box Projection");
        public static readonly GUIContent reflectionProbeBlending = EditorGUIUtility.TextContent("Reflection Probes Blending");
        public static readonly GUIContent detailNormalMap = EditorGUIUtility.TextContent("Detail Normal Map");
        public static readonly GUIContent cascadedShadowMaps = EditorGUIUtility.TextContent("Cascaded Shadows");
        public static readonly GUIContent prefer32BitShadowMaps = EditorGUIUtility.TextContent("Prefer 32 bit shadow maps");
        public static readonly GUIContent semitransparentShadows = EditorGUIUtility.TextContent("Enable Semitransparent Shadows");
        public static readonly GUIContent enableLPPV = EditorGUIUtility.TextContent("Enable Light Probe Proxy Volume");
        public static readonly GUIContent renderingPath = EditorGUIUtility.TextContent("Rendering Path");
        public static readonly GUIContent useHDR = EditorGUIUtility.TextContent("Use HDR");
        public static readonly GUIContent hdrMode = EditorGUIUtility.TextContent("HDR Mode");
        public static readonly GUIContent realtimeGICPUUsage = EditorGUIUtility.TextContent("Realtime Global Illumination CPU Usage|How many CPU worker threads to create for Realtime Global Illumination lighting calculations in the Player. Increasing this makes the system react faster to changes in lighting at a cost of using more CPU time. The higher the CPU Usage value, the more worker threads are created for solving Realtime GI.");
      }
    }
  }
}
