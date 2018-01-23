// Decompiled with JetBrains decompiler
// Type: UnityEditor.StandardParticlesShaderGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
  internal class StandardParticlesShaderGUI : ShaderGUI
  {
    private MaterialProperty blendMode = (MaterialProperty) null;
    private MaterialProperty colorMode = (MaterialProperty) null;
    private MaterialProperty flipbookMode = (MaterialProperty) null;
    private MaterialProperty cullMode = (MaterialProperty) null;
    private MaterialProperty distortionEnabled = (MaterialProperty) null;
    private MaterialProperty distortionStrength = (MaterialProperty) null;
    private MaterialProperty distortionBlend = (MaterialProperty) null;
    private MaterialProperty albedoMap = (MaterialProperty) null;
    private MaterialProperty albedoColor = (MaterialProperty) null;
    private MaterialProperty alphaCutoff = (MaterialProperty) null;
    private MaterialProperty metallicMap = (MaterialProperty) null;
    private MaterialProperty metallic = (MaterialProperty) null;
    private MaterialProperty smoothness = (MaterialProperty) null;
    private MaterialProperty bumpScale = (MaterialProperty) null;
    private MaterialProperty bumpMap = (MaterialProperty) null;
    private MaterialProperty emissionEnabled = (MaterialProperty) null;
    private MaterialProperty emissionColorForRendering = (MaterialProperty) null;
    private MaterialProperty emissionMap = (MaterialProperty) null;
    private MaterialProperty softParticlesEnabled = (MaterialProperty) null;
    private MaterialProperty cameraFadingEnabled = (MaterialProperty) null;
    private MaterialProperty softParticlesNearFadeDistance = (MaterialProperty) null;
    private MaterialProperty softParticlesFarFadeDistance = (MaterialProperty) null;
    private MaterialProperty cameraNearFadeDistance = (MaterialProperty) null;
    private MaterialProperty cameraFarFadeDistance = (MaterialProperty) null;
    private ColorPickerHDRConfig m_ColorPickerHDRConfig = new ColorPickerHDRConfig(0.0f, 99f, 1f / 99f, 3f);
    private List<ParticleSystemRenderer> m_RenderersUsingThisMaterial = new List<ParticleSystemRenderer>();
    private bool m_FirstTimeApply = true;
    private MaterialEditor m_MaterialEditor;

    public void FindProperties(MaterialProperty[] props)
    {
      this.blendMode = ShaderGUI.FindProperty("_Mode", props);
      this.colorMode = ShaderGUI.FindProperty("_ColorMode", props, false);
      this.flipbookMode = ShaderGUI.FindProperty("_FlipbookMode", props);
      this.cullMode = ShaderGUI.FindProperty("_Cull", props);
      this.distortionEnabled = ShaderGUI.FindProperty("_DistortionEnabled", props);
      this.distortionStrength = ShaderGUI.FindProperty("_DistortionStrength", props);
      this.distortionBlend = ShaderGUI.FindProperty("_DistortionBlend", props);
      this.albedoMap = ShaderGUI.FindProperty("_MainTex", props);
      this.albedoColor = ShaderGUI.FindProperty("_Color", props);
      this.alphaCutoff = ShaderGUI.FindProperty("_Cutoff", props);
      this.metallicMap = ShaderGUI.FindProperty("_MetallicGlossMap", props, false);
      this.metallic = ShaderGUI.FindProperty("_Metallic", props, false);
      this.smoothness = ShaderGUI.FindProperty("_Glossiness", props, false);
      this.bumpScale = ShaderGUI.FindProperty("_BumpScale", props);
      this.bumpMap = ShaderGUI.FindProperty("_BumpMap", props);
      this.emissionEnabled = ShaderGUI.FindProperty("_EmissionEnabled", props);
      this.emissionColorForRendering = ShaderGUI.FindProperty("_EmissionColor", props);
      this.emissionMap = ShaderGUI.FindProperty("_EmissionMap", props);
      this.softParticlesEnabled = ShaderGUI.FindProperty("_SoftParticlesEnabled", props);
      this.cameraFadingEnabled = ShaderGUI.FindProperty("_CameraFadingEnabled", props);
      this.softParticlesNearFadeDistance = ShaderGUI.FindProperty("_SoftParticlesNearFadeDistance", props);
      this.softParticlesFarFadeDistance = ShaderGUI.FindProperty("_SoftParticlesFarFadeDistance", props);
      this.cameraNearFadeDistance = ShaderGUI.FindProperty("_CameraNearFadeDistance", props);
      this.cameraFarFadeDistance = ShaderGUI.FindProperty("_CameraFarFadeDistance", props);
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
      this.FindProperties(props);
      this.m_MaterialEditor = materialEditor;
      Material target = materialEditor.target as Material;
      if (this.m_FirstTimeApply)
      {
        this.MaterialChanged(target);
        this.CacheRenderersUsingThisMaterial(target);
        this.m_FirstTimeApply = false;
      }
      this.ShaderPropertiesGUI(target);
    }

    public void ShaderPropertiesGUI(Material material)
    {
      EditorGUIUtility.labelWidth = 0.0f;
      EditorGUI.BeginChangeCheck();
      GUILayout.Label(StandardParticlesShaderGUI.Styles.blendingOptionsText, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.BlendModePopup();
      this.ColorModePopup();
      EditorGUILayout.Space();
      GUILayout.Label(StandardParticlesShaderGUI.Styles.mainOptionsText, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.FlipbookModePopup();
      this.TwoSidedPopup(material);
      this.FadingPopup(material);
      this.DistortionPopup(material);
      EditorGUILayout.Space();
      GUILayout.Label(StandardParticlesShaderGUI.Styles.mapsOptionsText, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.DoAlbedoArea(material);
      this.DoSpecularMetallicArea(material);
      this.DoNormalMapArea(material);
      this.DoEmissionArea(material);
      if (!this.flipbookMode.hasMixedValue && (int) this.flipbookMode.floatValue != 1)
      {
        EditorGUI.BeginChangeCheck();
        this.m_MaterialEditor.TextureScaleOffsetProperty(this.albedoMap);
        if (EditorGUI.EndChangeCheck())
          this.emissionMap.textureScaleAndOffset = this.albedoMap.textureScaleAndOffset;
      }
      if (EditorGUI.EndChangeCheck())
      {
        foreach (Material target in this.blendMode.targets)
          this.MaterialChanged(target);
      }
      EditorGUILayout.Space();
      GUILayout.Label(StandardParticlesShaderGUI.Styles.advancedOptionsText, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.m_MaterialEditor.RenderQueueField();
      EditorGUILayout.Space();
      GUILayout.Label(StandardParticlesShaderGUI.Styles.requiredVertexStreamsText, EditorStyles.boldLabel, new GUILayoutOption[0]);
      this.DoVertexStreamsArea(material);
    }

    public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
    {
      if (newShader.name.Contains("Unlit"))
        material.SetFloat("_LightingEnabled", 0.0f);
      else
        material.SetFloat("_LightingEnabled", 1f);
      if (material.HasProperty("_Emission"))
        material.SetColor("_EmissionColor", material.GetColor("_Emission"));
      base.AssignNewShaderToMaterial(material, oldShader, newShader);
      if ((UnityEngine.Object) oldShader == (UnityEngine.Object) null || !oldShader.name.Contains("Legacy Shaders/"))
      {
        StandardParticlesShaderGUI.SetupMaterialWithBlendMode(material, (StandardParticlesShaderGUI.BlendMode) material.GetFloat("_Mode"));
      }
      else
      {
        StandardParticlesShaderGUI.BlendMode blendMode = StandardParticlesShaderGUI.BlendMode.Opaque;
        if (oldShader.name.Contains("/Transparent/Cutout/"))
          blendMode = StandardParticlesShaderGUI.BlendMode.Cutout;
        else if (oldShader.name.Contains("/Transparent/"))
          blendMode = StandardParticlesShaderGUI.BlendMode.Fade;
        material.SetFloat("_Mode", (float) blendMode);
        this.MaterialChanged(material);
      }
    }

    private void BlendModePopup()
    {
      EditorGUI.showMixedValue = this.blendMode.hasMixedValue;
      StandardParticlesShaderGUI.BlendMode floatValue = (StandardParticlesShaderGUI.BlendMode) this.blendMode.floatValue;
      EditorGUI.BeginChangeCheck();
      StandardParticlesShaderGUI.BlendMode blendMode = (StandardParticlesShaderGUI.BlendMode) EditorGUILayout.Popup(StandardParticlesShaderGUI.Styles.renderingMode, (int) floatValue, StandardParticlesShaderGUI.Styles.blendNames, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_MaterialEditor.RegisterPropertyChangeUndo("Rendering Mode");
        this.blendMode.floatValue = (float) blendMode;
      }
      EditorGUI.showMixedValue = false;
    }

    private void ColorModePopup()
    {
      if (this.colorMode == null)
        return;
      EditorGUI.showMixedValue = this.colorMode.hasMixedValue;
      StandardParticlesShaderGUI.ColorMode floatValue = (StandardParticlesShaderGUI.ColorMode) this.colorMode.floatValue;
      EditorGUI.BeginChangeCheck();
      StandardParticlesShaderGUI.ColorMode colorMode = (StandardParticlesShaderGUI.ColorMode) EditorGUILayout.Popup(StandardParticlesShaderGUI.Styles.colorMode, (int) floatValue, StandardParticlesShaderGUI.Styles.colorNames, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_MaterialEditor.RegisterPropertyChangeUndo("Color Mode");
        this.colorMode.floatValue = (float) colorMode;
      }
      EditorGUI.showMixedValue = false;
    }

    private void FlipbookModePopup()
    {
      EditorGUI.showMixedValue = this.flipbookMode.hasMixedValue;
      StandardParticlesShaderGUI.FlipbookMode floatValue = (StandardParticlesShaderGUI.FlipbookMode) this.flipbookMode.floatValue;
      EditorGUI.BeginChangeCheck();
      StandardParticlesShaderGUI.FlipbookMode flipbookMode = (StandardParticlesShaderGUI.FlipbookMode) EditorGUILayout.Popup(StandardParticlesShaderGUI.Styles.flipbookMode, (int) floatValue, StandardParticlesShaderGUI.Styles.flipbookNames, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_MaterialEditor.RegisterPropertyChangeUndo("Flip-Book Mode");
        this.flipbookMode.floatValue = (float) flipbookMode;
      }
      EditorGUI.showMixedValue = false;
    }

    private void TwoSidedPopup(Material material)
    {
      EditorGUI.showMixedValue = this.cullMode.hasMixedValue;
      bool flag1 = (double) this.cullMode.floatValue == 0.0;
      EditorGUI.BeginChangeCheck();
      bool flag2 = EditorGUILayout.Toggle(StandardParticlesShaderGUI.Styles.twoSidedEnabled, flag1, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_MaterialEditor.RegisterPropertyChangeUndo("Two Sided Enabled");
        this.cullMode.floatValue = !flag2 ? 2f : 0.0f;
      }
      EditorGUI.showMixedValue = false;
    }

    private void FadingPopup(Material material)
    {
      if (material.GetInt("_ZWrite") != 0)
        return;
      EditorGUI.showMixedValue = this.softParticlesEnabled.hasMixedValue;
      float floatValue1 = this.softParticlesEnabled.floatValue;
      EditorGUI.BeginChangeCheck();
      float num1 = !EditorGUILayout.Toggle(StandardParticlesShaderGUI.Styles.softParticlesEnabled, (double) floatValue1 != 0.0, new GUILayoutOption[0]) ? 0.0f : 1f;
      if (EditorGUI.EndChangeCheck())
      {
        this.m_MaterialEditor.RegisterPropertyChangeUndo("Soft Particles Enabled");
        this.softParticlesEnabled.floatValue = num1;
      }
      if ((double) num1 != 0.0)
      {
        int labelIndent = 2;
        this.m_MaterialEditor.ShaderProperty(this.softParticlesNearFadeDistance, StandardParticlesShaderGUI.Styles.softParticlesNearFadeDistanceText, labelIndent);
        this.m_MaterialEditor.ShaderProperty(this.softParticlesFarFadeDistance, StandardParticlesShaderGUI.Styles.softParticlesFarFadeDistanceText, labelIndent);
      }
      EditorGUI.showMixedValue = this.cameraFadingEnabled.hasMixedValue;
      float floatValue2 = this.cameraFadingEnabled.floatValue;
      EditorGUI.BeginChangeCheck();
      float num2 = !EditorGUILayout.Toggle(StandardParticlesShaderGUI.Styles.cameraFadingEnabled, (double) floatValue2 != 0.0, new GUILayoutOption[0]) ? 0.0f : 1f;
      if (EditorGUI.EndChangeCheck())
      {
        this.m_MaterialEditor.RegisterPropertyChangeUndo("Camera Fading Enabled");
        this.cameraFadingEnabled.floatValue = num2;
      }
      if ((double) num2 != 0.0)
      {
        int labelIndent = 2;
        this.m_MaterialEditor.ShaderProperty(this.cameraNearFadeDistance, StandardParticlesShaderGUI.Styles.cameraNearFadeDistanceText, labelIndent);
        this.m_MaterialEditor.ShaderProperty(this.cameraFarFadeDistance, StandardParticlesShaderGUI.Styles.cameraFarFadeDistanceText, labelIndent);
      }
      EditorGUI.showMixedValue = false;
    }

    private void DistortionPopup(Material material)
    {
      if (material.GetInt("_ZWrite") != 0)
        return;
      EditorGUI.showMixedValue = this.distortionEnabled.hasMixedValue;
      bool flag1 = (double) this.distortionEnabled.floatValue != 0.0;
      EditorGUI.BeginChangeCheck();
      bool flag2 = EditorGUILayout.Toggle(StandardParticlesShaderGUI.Styles.distortionEnabled, flag1, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_MaterialEditor.RegisterPropertyChangeUndo("Distortion Enabled");
        this.distortionEnabled.floatValue = !flag2 ? 0.0f : 1f;
      }
      if (flag2)
      {
        int labelIndent = 2;
        this.m_MaterialEditor.ShaderProperty(this.distortionStrength, StandardParticlesShaderGUI.Styles.distortionStrengthText, labelIndent);
        this.m_MaterialEditor.ShaderProperty(this.distortionBlend, StandardParticlesShaderGUI.Styles.distortionBlendText, labelIndent);
      }
      EditorGUI.showMixedValue = false;
    }

    private void DoAlbedoArea(Material material)
    {
      this.m_MaterialEditor.TexturePropertyWithHDRColor(StandardParticlesShaderGUI.Styles.albedoText, this.albedoMap, this.albedoColor, this.m_ColorPickerHDRConfig, true);
      if ((int) material.GetFloat("_Mode") != 1)
        return;
      this.m_MaterialEditor.ShaderProperty(this.alphaCutoff, StandardParticlesShaderGUI.Styles.alphaCutoffText, 2);
    }

    private void DoEmissionArea(Material material)
    {
      EditorGUI.showMixedValue = this.emissionEnabled.hasMixedValue;
      bool flag1 = (double) this.emissionEnabled.floatValue != 0.0;
      EditorGUI.BeginChangeCheck();
      bool flag2 = EditorGUILayout.Toggle(StandardParticlesShaderGUI.Styles.emissionEnabled, flag1, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_MaterialEditor.RegisterPropertyChangeUndo("Emission Enabled");
        this.emissionEnabled.floatValue = !flag2 ? 0.0f : 1f;
      }
      if (!flag2)
        return;
      bool flag3 = (UnityEngine.Object) this.emissionMap.textureValue != (UnityEngine.Object) null;
      this.m_MaterialEditor.TexturePropertyWithHDRColor(StandardParticlesShaderGUI.Styles.emissionText, this.emissionMap, this.emissionColorForRendering, this.m_ColorPickerHDRConfig, false);
      float maxColorComponent = this.emissionColorForRendering.colorValue.maxColorComponent;
      if ((UnityEngine.Object) this.emissionMap.textureValue != (UnityEngine.Object) null && !flag3 && (double) maxColorComponent <= 0.0)
        this.emissionColorForRendering.colorValue = Color.white;
    }

    private void DoSpecularMetallicArea(Material material)
    {
      if (this.metallicMap == null || (double) material.GetFloat("_LightingEnabled") <= 0.0)
        return;
      bool flag = (UnityEngine.Object) this.metallicMap.textureValue != (UnityEngine.Object) null;
      this.m_MaterialEditor.TexturePropertySingleLine(StandardParticlesShaderGUI.Styles.metallicMapText, this.metallicMap, !flag ? this.metallic : (MaterialProperty) null);
      int labelIndent = 2;
      this.m_MaterialEditor.ShaderProperty(this.smoothness, !flag ? StandardParticlesShaderGUI.Styles.smoothnessText : StandardParticlesShaderGUI.Styles.smoothnessScaleText, labelIndent);
    }

    private void DoNormalMapArea(Material material)
    {
      bool flag = material.GetInt("_ZWrite") != 0;
      if ((double) material.GetFloat("_LightingEnabled") <= 0.0 && ((double) material.GetFloat("_DistortionEnabled") <= 0.0 || flag))
        return;
      this.m_MaterialEditor.TexturePropertySingleLine(StandardParticlesShaderGUI.Styles.normalMapText, this.bumpMap, !((UnityEngine.Object) this.bumpMap.textureValue != (UnityEngine.Object) null) ? (MaterialProperty) null : this.bumpScale);
    }

    private void DoVertexStreamsArea(Material material)
    {
      bool flag1 = (double) material.GetFloat("_LightingEnabled") > 0.0;
      bool flag2 = (double) material.GetFloat("_FlipbookMode") > 0.0;
      bool flag3 = (bool) ((UnityEngine.Object) material.GetTexture("_BumpMap")) && flag1;
      GUILayout.Label(StandardParticlesShaderGUI.Styles.streamPositionText, EditorStyles.label, new GUILayoutOption[0]);
      if (flag1)
        GUILayout.Label(StandardParticlesShaderGUI.Styles.streamNormalText, EditorStyles.label, new GUILayoutOption[0]);
      GUILayout.Label(StandardParticlesShaderGUI.Styles.streamColorText, EditorStyles.label, new GUILayoutOption[0]);
      GUILayout.Label(StandardParticlesShaderGUI.Styles.streamUVText, EditorStyles.label, new GUILayoutOption[0]);
      if (flag2)
      {
        GUILayout.Label(StandardParticlesShaderGUI.Styles.streamUV2Text, EditorStyles.label, new GUILayoutOption[0]);
        GUILayout.Label(StandardParticlesShaderGUI.Styles.streamAnimBlendText, EditorStyles.label, new GUILayoutOption[0]);
      }
      if (flag3)
        GUILayout.Label(StandardParticlesShaderGUI.Styles.streamTangentText, EditorStyles.label, new GUILayoutOption[0]);
      List<ParticleSystemVertexStream> streams = new List<ParticleSystemVertexStream>();
      streams.Add(ParticleSystemVertexStream.Position);
      if (flag1)
        streams.Add(ParticleSystemVertexStream.Normal);
      streams.Add(ParticleSystemVertexStream.Color);
      streams.Add(ParticleSystemVertexStream.UV);
      if (flag2)
      {
        streams.Add(ParticleSystemVertexStream.UV2);
        streams.Add(ParticleSystemVertexStream.AnimBlend);
      }
      if (flag3)
        streams.Add(ParticleSystemVertexStream.Tangent);
      if (GUILayout.Button(StandardParticlesShaderGUI.Styles.streamApplyToAllSystemsText, EditorStyles.miniButton, new GUILayoutOption[1]{ GUILayout.ExpandWidth(false) }))
      {
        foreach (ParticleSystemRenderer particleSystemRenderer in this.m_RenderersUsingThisMaterial)
          particleSystemRenderer.SetActiveVertexStreams(streams);
      }
      string str = "";
      List<ParticleSystemVertexStream> systemVertexStreamList = new List<ParticleSystemVertexStream>();
      foreach (ParticleSystemRenderer particleSystemRenderer in this.m_RenderersUsingThisMaterial)
      {
        particleSystemRenderer.GetActiveVertexStreams(systemVertexStreamList);
        if (!systemVertexStreamList.SequenceEqual<ParticleSystemVertexStream>((IEnumerable<ParticleSystemVertexStream>) streams))
          str = str + "  " + particleSystemRenderer.name + "\n";
      }
      if (str != "")
        EditorGUILayout.HelpBox("The following Particle System Renderers are using this material with incorrect Vertex Streams:\n" + str + "Use the Apply to Systems button to fix this", MessageType.Warning, true);
      EditorGUILayout.Space();
    }

    public static void SetupMaterialWithBlendMode(Material material, StandardParticlesShaderGUI.BlendMode blendMode)
    {
      switch (blendMode)
      {
        case StandardParticlesShaderGUI.BlendMode.Opaque:
          material.SetOverrideTag("RenderType", "");
          material.SetInt("_BlendOp", 0);
          material.SetInt("_SrcBlend", 1);
          material.SetInt("_DstBlend", 0);
          material.SetInt("_ZWrite", 1);
          material.DisableKeyword("_ALPHATEST_ON");
          material.DisableKeyword("_ALPHABLEND_ON");
          material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
          material.DisableKeyword("_ALPHAMODULATE_ON");
          material.renderQueue = -1;
          break;
        case StandardParticlesShaderGUI.BlendMode.Cutout:
          material.SetOverrideTag("RenderType", "TransparentCutout");
          material.SetInt("_BlendOp", 0);
          material.SetInt("_SrcBlend", 1);
          material.SetInt("_DstBlend", 0);
          material.SetInt("_ZWrite", 1);
          material.EnableKeyword("_ALPHATEST_ON");
          material.DisableKeyword("_ALPHABLEND_ON");
          material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
          material.DisableKeyword("_ALPHAMODULATE_ON");
          material.renderQueue = 2450;
          break;
        case StandardParticlesShaderGUI.BlendMode.Fade:
          material.SetOverrideTag("RenderType", "Transparent");
          material.SetInt("_BlendOp", 0);
          material.SetInt("_SrcBlend", 5);
          material.SetInt("_DstBlend", 10);
          material.SetInt("_ZWrite", 0);
          material.DisableKeyword("_ALPHATEST_ON");
          material.EnableKeyword("_ALPHABLEND_ON");
          material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
          material.DisableKeyword("_ALPHAMODULATE_ON");
          material.renderQueue = 3000;
          break;
        case StandardParticlesShaderGUI.BlendMode.Transparent:
          material.SetOverrideTag("RenderType", "Transparent");
          material.SetInt("_BlendOp", 0);
          material.SetInt("_SrcBlend", 1);
          material.SetInt("_DstBlend", 10);
          material.SetInt("_ZWrite", 0);
          material.DisableKeyword("_ALPHATEST_ON");
          material.DisableKeyword("_ALPHABLEND_ON");
          material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
          material.DisableKeyword("_ALPHAMODULATE_ON");
          material.renderQueue = 3000;
          break;
        case StandardParticlesShaderGUI.BlendMode.Additive:
          material.SetOverrideTag("RenderType", "Transparent");
          material.SetInt("_BlendOp", 0);
          material.SetInt("_SrcBlend", 5);
          material.SetInt("_DstBlend", 1);
          material.SetInt("_ZWrite", 0);
          material.DisableKeyword("_ALPHATEST_ON");
          material.EnableKeyword("_ALPHABLEND_ON");
          material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
          material.DisableKeyword("_ALPHAMODULATE_ON");
          material.renderQueue = 3000;
          break;
        case StandardParticlesShaderGUI.BlendMode.Subtractive:
          material.SetOverrideTag("RenderType", "Transparent");
          material.SetInt("_BlendOp", 2);
          material.SetInt("_SrcBlend", 5);
          material.SetInt("_DstBlend", 1);
          material.SetInt("_ZWrite", 0);
          material.DisableKeyword("_ALPHATEST_ON");
          material.EnableKeyword("_ALPHABLEND_ON");
          material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
          material.DisableKeyword("_ALPHAMODULATE_ON");
          material.renderQueue = 3000;
          break;
        case StandardParticlesShaderGUI.BlendMode.Modulate:
          material.SetOverrideTag("RenderType", "Transparent");
          material.SetInt("_BlendOp", 0);
          material.SetInt("_SrcBlend", 2);
          material.SetInt("_DstBlend", 10);
          material.SetInt("_ZWrite", 0);
          material.DisableKeyword("_ALPHATEST_ON");
          material.DisableKeyword("_ALPHABLEND_ON");
          material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
          material.EnableKeyword("_ALPHAMODULATE_ON");
          material.renderQueue = 3000;
          break;
      }
    }

    public static void SetupMaterialWithColorMode(Material material, StandardParticlesShaderGUI.ColorMode colorMode)
    {
      switch (colorMode)
      {
        case StandardParticlesShaderGUI.ColorMode.Multiply:
          material.DisableKeyword("_COLOROVERLAY_ON");
          material.DisableKeyword("_COLORCOLOR_ON");
          material.DisableKeyword("_COLORADDSUBDIFF_ON");
          break;
        case StandardParticlesShaderGUI.ColorMode.Additive:
          material.DisableKeyword("_COLOROVERLAY_ON");
          material.DisableKeyword("_COLORCOLOR_ON");
          material.EnableKeyword("_COLORADDSUBDIFF_ON");
          material.SetVector("_ColorAddSubDiff", new Vector4(1f, 0.0f, 0.0f, 0.0f));
          break;
        case StandardParticlesShaderGUI.ColorMode.Subtractive:
          material.DisableKeyword("_COLOROVERLAY_ON");
          material.DisableKeyword("_COLORCOLOR_ON");
          material.EnableKeyword("_COLORADDSUBDIFF_ON");
          material.SetVector("_ColorAddSubDiff", new Vector4(-1f, 0.0f, 0.0f, 0.0f));
          break;
        case StandardParticlesShaderGUI.ColorMode.Overlay:
          material.DisableKeyword("_COLORCOLOR_ON");
          material.DisableKeyword("_COLORADDSUBDIFF_ON");
          material.EnableKeyword("_COLOROVERLAY_ON");
          break;
        case StandardParticlesShaderGUI.ColorMode.Color:
          material.DisableKeyword("_COLOROVERLAY_ON");
          material.DisableKeyword("_COLORADDSUBDIFF_ON");
          material.EnableKeyword("_COLORCOLOR_ON");
          break;
        case StandardParticlesShaderGUI.ColorMode.Difference:
          material.DisableKeyword("_COLOROVERLAY_ON");
          material.DisableKeyword("_COLORCOLOR_ON");
          material.EnableKeyword("_COLORADDSUBDIFF_ON");
          material.SetVector("_ColorAddSubDiff", new Vector4(-1f, 1f, 0.0f, 0.0f));
          break;
      }
    }

    private void SetMaterialKeywords(Material material)
    {
      bool flag1 = material.GetInt("_ZWrite") != 0;
      bool flag2 = (double) material.GetFloat("_LightingEnabled") > 0.0;
      bool flag3 = (double) material.GetFloat("_DistortionEnabled") > 0.0 && !flag1;
      StandardParticlesShaderGUI.SetKeyword(material, "_NORMALMAP", (bool) ((UnityEngine.Object) material.GetTexture("_BumpMap")) && (flag2 || flag3));
      StandardParticlesShaderGUI.SetKeyword(material, "_METALLICGLOSSMAP", (UnityEngine.Object) material.GetTexture("_MetallicGlossMap") != (UnityEngine.Object) null && flag2);
      material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
      StandardParticlesShaderGUI.SetKeyword(material, "_EMISSION", (double) material.GetFloat("_EmissionEnabled") > 0.0);
      bool state1 = (double) material.GetFloat("_FlipbookMode") > 0.0;
      StandardParticlesShaderGUI.SetKeyword(material, "_REQUIRE_UV2", state1);
      bool flag4 = (double) material.GetFloat("_SoftParticlesEnabled") > 0.0;
      bool flag5 = (double) material.GetFloat("_CameraFadingEnabled") > 0.0;
      float x1 = material.GetFloat("_SoftParticlesNearFadeDistance");
      float num1 = material.GetFloat("_SoftParticlesFarFadeDistance");
      float x2 = material.GetFloat("_CameraNearFadeDistance");
      float num2 = material.GetFloat("_CameraFarFadeDistance");
      if ((double) x1 < 0.0)
      {
        x1 = 0.0f;
        material.SetFloat("_SoftParticlesNearFadeDistance", 0.0f);
      }
      if ((double) num1 < 0.0)
      {
        num1 = 0.0f;
        material.SetFloat("_SoftParticlesFarFadeDistance", 0.0f);
      }
      if ((double) x2 < 0.0)
      {
        x2 = 0.0f;
        material.SetFloat("_CameraNearFadeDistance", 0.0f);
      }
      if ((double) num2 < 0.0)
      {
        num2 = 0.0f;
        material.SetFloat("_CameraFarFadeDistance", 0.0f);
      }
      bool state2 = (flag4 || flag5) && !flag1;
      StandardParticlesShaderGUI.SetKeyword(material, "_FADING_ON", state2);
      if (flag4)
        material.SetVector("_SoftParticleFadeParams", new Vector4(x1, (float) (1.0 / ((double) num1 - (double) x1)), 0.0f, 0.0f));
      else
        material.SetVector("_SoftParticleFadeParams", new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
      if (flag5)
        material.SetVector("_CameraFadeParams", new Vector4(x2, (float) (1.0 / ((double) num2 - (double) x2)), 0.0f, 0.0f));
      else
        material.SetVector("_CameraFadeParams", new Vector4(0.0f, float.PositiveInfinity, 0.0f, 0.0f));
      StandardParticlesShaderGUI.SetKeyword(material, "EFFECT_BUMP", flag3);
      material.SetShaderPassEnabled("Always", flag3);
      if (!flag3)
        return;
      material.SetFloat("_DistortionStrengthScaled", material.GetFloat("_DistortionStrength") * 0.1f);
    }

    private void MaterialChanged(Material material)
    {
      StandardParticlesShaderGUI.SetupMaterialWithBlendMode(material, (StandardParticlesShaderGUI.BlendMode) material.GetFloat("_Mode"));
      if (this.colorMode != null)
        StandardParticlesShaderGUI.SetupMaterialWithColorMode(material, (StandardParticlesShaderGUI.ColorMode) material.GetFloat("_ColorMode"));
      this.SetMaterialKeywords(material);
    }

    private void CacheRenderersUsingThisMaterial(Material material)
    {
      this.m_RenderersUsingThisMaterial.Clear();
      foreach (ParticleSystemRenderer particleSystemRenderer in UnityEngine.Object.FindObjectsOfType(typeof (ParticleSystemRenderer)) as ParticleSystemRenderer[])
      {
        if ((UnityEngine.Object) particleSystemRenderer.sharedMaterial == (UnityEngine.Object) material)
          this.m_RenderersUsingThisMaterial.Add(particleSystemRenderer);
      }
    }

    private static void SetKeyword(Material m, string keyword, bool state)
    {
      if (state)
        m.EnableKeyword(keyword);
      else
        m.DisableKeyword(keyword);
    }

    public enum BlendMode
    {
      Opaque,
      Cutout,
      Fade,
      Transparent,
      Additive,
      Subtractive,
      Modulate,
    }

    public enum FlipbookMode
    {
      Simple,
      Blended,
    }

    public enum ColorMode
    {
      Multiply,
      Additive,
      Subtractive,
      Overlay,
      Color,
      Difference,
    }

    private static class Styles
    {
      public static GUIContent albedoText = new GUIContent("Albedo", "Albedo (RGB) and Transparency (A).");
      public static GUIContent alphaCutoffText = new GUIContent("Alpha Cutoff", "Threshold for alpha cutoff.");
      public static GUIContent metallicMapText = new GUIContent("Metallic", "Metallic (R) and Smoothness (A).");
      public static GUIContent smoothnessText = new GUIContent("Smoothness", "Smoothness value.");
      public static GUIContent smoothnessScaleText = new GUIContent("Smoothness", "Smoothness scale factor.");
      public static GUIContent normalMapText = new GUIContent("Normal Map", "Normal Map.");
      public static GUIContent emissionText = new GUIContent("Color", "Emission (RGB).");
      public static GUIContent renderingMode = new GUIContent("Rendering Mode", "Determines the transparency and blending method for drawing the object to the screen.");
      public static GUIContent[] blendNames = Array.ConvertAll<string, GUIContent>(Enum.GetNames(typeof (StandardParticlesShaderGUI.BlendMode)), (Converter<string, GUIContent>) (item => new GUIContent(item)));
      public static GUIContent colorMode = new GUIContent("Color Mode", "Determines the blending mode between the particle color and the texture albedo.");
      public static GUIContent[] colorNames = Array.ConvertAll<string, GUIContent>(Enum.GetNames(typeof (StandardParticlesShaderGUI.ColorMode)), (Converter<string, GUIContent>) (item => new GUIContent(item)));
      public static GUIContent flipbookMode = new GUIContent("Flip-Book Mode", "Determines the blending mode used for animated texture sheets.");
      public static GUIContent[] flipbookNames = Array.ConvertAll<string, GUIContent>(Enum.GetNames(typeof (StandardParticlesShaderGUI.FlipbookMode)), (Converter<string, GUIContent>) (item => new GUIContent(item)));
      public static GUIContent twoSidedEnabled = new GUIContent("Two Sided", "Render both front and back faces of the particle geometry.");
      public static GUIContent distortionEnabled = new GUIContent("Enable Distortion", "Use a grab pass and normal map to simulate refraction.");
      public static GUIContent distortionStrengthText = new GUIContent("Strength", "Distortion Strength.");
      public static GUIContent distortionBlendText = new GUIContent("Blend", "Weighting between albedo and grab pass.");
      public static GUIContent softParticlesEnabled = new GUIContent("Enable Soft Particles", "Fade out particle geometry when it gets close to the surface of objects written into the depth buffer.");
      public static GUIContent softParticlesNearFadeDistanceText = new GUIContent("Near fade", "Soft Particles near fade distance.");
      public static GUIContent softParticlesFarFadeDistanceText = new GUIContent("Far fade", "Soft Particles far fade distance.");
      public static GUIContent cameraFadingEnabled = new GUIContent("Enable Camera Fading", "Fade out particle geometry when it gets close to the camera.");
      public static GUIContent cameraNearFadeDistanceText = new GUIContent("Near fade", "Camera near fade distance.");
      public static GUIContent cameraFarFadeDistanceText = new GUIContent("Far fade", "Camera far fade distance.");
      public static GUIContent emissionEnabled = new GUIContent("Emission");
      public static string blendingOptionsText = "Blending Options";
      public static string mainOptionsText = "Main Options";
      public static string mapsOptionsText = "Maps";
      public static string advancedOptionsText = "Advanced Options";
      public static string requiredVertexStreamsText = "Required Vertex Streams";
      public static string streamPositionText = "Position (POSITION.xyz)";
      public static string streamNormalText = "Normal (NORMAL.xyz)";
      public static string streamColorText = "Color (COLOR.xyzw)";
      public static string streamUVText = "UV (TEXCOORD0.xy)";
      public static string streamUV2Text = "UV2 (TEXCOORD0.zw)";
      public static string streamAnimBlendText = "AnimBlend (TEXCOORD1.x)";
      public static string streamTangentText = "Tangent (TANGENT.xyzw)";
      public static GUIContent streamApplyToAllSystemsText = new GUIContent("Apply to Systems", "Apply the vertex stream layout to all Particle Systems using this material");
    }
  }
}
