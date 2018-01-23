// Decompiled with JetBrains decompiler
// Type: UnityEditor.TrailModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class TrailModuleUI : ModuleUI
  {
    private static TrailModuleUI.Texts s_Texts;
    private SerializedProperty m_Mode;
    private SerializedProperty m_Ratio;
    private SerializedMinMaxCurve m_Lifetime;
    private SerializedProperty m_MinVertexDistance;
    private SerializedProperty m_TextureMode;
    private SerializedProperty m_WorldSpace;
    private SerializedProperty m_DieWithParticles;
    private SerializedProperty m_SizeAffectsWidth;
    private SerializedProperty m_SizeAffectsLifetime;
    private SerializedProperty m_InheritParticleColor;
    private SerializedMinMaxGradient m_ColorOverLifetime;
    private SerializedMinMaxCurve m_WidthOverTrail;
    private SerializedMinMaxGradient m_ColorOverTrail;
    private SerializedProperty m_GenerateLightingData;
    private SerializedProperty m_RibbonCount;
    private SerializedProperty m_SplitSubEmitterRibbons;

    public TrailModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "TrailModule", displayName)
    {
      this.m_ToolTip = "Attach trails to the particles.";
    }

    protected override void Init()
    {
      if (this.m_Ratio != null)
        return;
      if (TrailModuleUI.s_Texts == null)
        TrailModuleUI.s_Texts = new TrailModuleUI.Texts();
      this.m_Mode = this.GetProperty("mode");
      this.m_Ratio = this.GetProperty("ratio");
      this.m_Lifetime = new SerializedMinMaxCurve((ModuleUI) this, TrailModuleUI.s_Texts.lifetime, "lifetime");
      this.m_MinVertexDistance = this.GetProperty("minVertexDistance");
      this.m_TextureMode = this.GetProperty("textureMode");
      this.m_WorldSpace = this.GetProperty("worldSpace");
      this.m_DieWithParticles = this.GetProperty("dieWithParticles");
      this.m_SizeAffectsWidth = this.GetProperty("sizeAffectsWidth");
      this.m_SizeAffectsLifetime = this.GetProperty("sizeAffectsLifetime");
      this.m_InheritParticleColor = this.GetProperty("inheritParticleColor");
      this.m_ColorOverLifetime = new SerializedMinMaxGradient((SerializedModule) this, "colorOverLifetime");
      this.m_WidthOverTrail = new SerializedMinMaxCurve((ModuleUI) this, TrailModuleUI.s_Texts.widthOverTrail, "widthOverTrail");
      this.m_ColorOverTrail = new SerializedMinMaxGradient((SerializedModule) this, "colorOverTrail");
      this.m_GenerateLightingData = this.GetProperty("generateLightingData");
      this.m_RibbonCount = this.GetProperty("ribbonCount");
      this.m_SplitSubEmitterRibbons = this.GetProperty("splitSubEmitterRibbons");
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      ParticleSystemTrailMode particleSystemTrailMode = (ParticleSystemTrailMode) ModuleUI.GUIPopup(TrailModuleUI.s_Texts.mode, this.m_Mode, TrailModuleUI.s_Texts.trailModeOptions);
      if (!this.m_Mode.hasMultipleDifferentValues)
      {
        if (particleSystemTrailMode == ParticleSystemTrailMode.PerParticle)
        {
          double num1 = (double) ModuleUI.GUIFloat(TrailModuleUI.s_Texts.ratio, this.m_Ratio);
          ModuleUI.GUIMinMaxCurve(TrailModuleUI.s_Texts.lifetime, this.m_Lifetime);
          double num2 = (double) ModuleUI.GUIFloat(TrailModuleUI.s_Texts.minVertexDistance, this.m_MinVertexDistance);
          ModuleUI.GUIToggle(TrailModuleUI.s_Texts.worldSpace, this.m_WorldSpace);
          ModuleUI.GUIToggle(TrailModuleUI.s_Texts.dieWithParticles, this.m_DieWithParticles);
        }
        else
        {
          ModuleUI.GUIInt(TrailModuleUI.s_Texts.ribbonCount, this.m_RibbonCount);
          ModuleUI.GUIToggle(TrailModuleUI.s_Texts.splitSubEmitterRibbons, this.m_SplitSubEmitterRibbons);
        }
      }
      ModuleUI.GUIPopup(TrailModuleUI.s_Texts.textureMode, this.m_TextureMode, TrailModuleUI.s_Texts.textureModeOptions);
      ModuleUI.GUIToggle(TrailModuleUI.s_Texts.sizeAffectsWidth, this.m_SizeAffectsWidth);
      if (!this.m_Mode.hasMultipleDifferentValues && particleSystemTrailMode == ParticleSystemTrailMode.PerParticle)
        ModuleUI.GUIToggle(TrailModuleUI.s_Texts.sizeAffectsLifetime, this.m_SizeAffectsLifetime);
      ModuleUI.GUIToggle(TrailModuleUI.s_Texts.inheritParticleColor, this.m_InheritParticleColor);
      this.GUIMinMaxGradient(TrailModuleUI.s_Texts.colorOverLifetime, this.m_ColorOverLifetime, false);
      ModuleUI.GUIMinMaxCurve(TrailModuleUI.s_Texts.widthOverTrail, this.m_WidthOverTrail);
      this.GUIMinMaxGradient(TrailModuleUI.s_Texts.colorOverTrail, this.m_ColorOverTrail, false);
      ModuleUI.GUIToggle(TrailModuleUI.s_Texts.generateLightingData, this.m_GenerateLightingData);
      foreach (ParticleSystem particleSystem in this.m_ParticleSystemUI.m_ParticleSystems)
      {
        if (particleSystem.trails.enabled)
        {
          ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
          if ((Object) component != (Object) null && (Object) component.trailMaterial == (Object) null)
          {
            EditorGUILayout.HelpBox("Assign a Trail Material in the Renderer Module", MessageType.Warning, true);
            break;
          }
        }
      }
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      this.Init();
      if (this.m_Mode.intValue != 0)
        return;
      text += "\nTrails module is enabled.";
    }

    private class Texts
    {
      public GUIContent mode = new GUIContent("Mode", "Select how trails are generated on the particles.");
      public GUIContent ratio = new GUIContent("Ratio", "Choose what proportion of particles will receive a trail.");
      public GUIContent lifetime = EditorGUIUtility.TextContent("Lifetime|How long each trail will last, relative to the life of the particle.");
      public GUIContent minVertexDistance = EditorGUIUtility.TextContent("Minimum Vertex Distance|The minimum distance each trail can travel before adding a new vertex.");
      public GUIContent textureMode = EditorGUIUtility.TextContent("Texture Mode|Should the U coordinate be stretched or tiled?");
      public GUIContent worldSpace = EditorGUIUtility.TextContent("World Space|Trail points will be dropped in world space, even if the particle system is simulating in local space.");
      public GUIContent dieWithParticles = EditorGUIUtility.TextContent("Die with Particles|The trails will disappear when their owning particles die.");
      public GUIContent sizeAffectsWidth = EditorGUIUtility.TextContent("Size affects Width|The trails will use the particle size to control their width.");
      public GUIContent sizeAffectsLifetime = EditorGUIUtility.TextContent("Size affects Lifetime|The trails will use the particle size to control their lifetime.");
      public GUIContent inheritParticleColor = EditorGUIUtility.TextContent("Inherit Particle Color|The trails will use the particle color as their base color.");
      public GUIContent colorOverLifetime = EditorGUIUtility.TextContent("Color over Lifetime|The color of the trails during the lifetime of the particle they are attached to.");
      public GUIContent widthOverTrail = EditorGUIUtility.TextContent("Width over Trail|Select a width for the trail from its start to end vertex.");
      public GUIContent colorOverTrail = EditorGUIUtility.TextContent("Color over Trail|Select a color for the trail from its start to end vertex.");
      public GUIContent generateLightingData = EditorGUIUtility.TextContent("Generate Lighting Data|Toggle generation of normal and tangent data, for use in lit shaders.");
      public GUIContent ribbonCount = EditorGUIUtility.TextContent("Ribbon Count|Select how many ribbons to render throughout the Particle System.");
      public GUIContent splitSubEmitterRibbons = EditorGUIUtility.TextContent("Split Sub Emitter Ribbons|When used on a sub emitter, ribbons will connect particles from each parent particle independently.");
      public GUIContent[] trailModeOptions = new GUIContent[2]{ EditorGUIUtility.TextContent("Particles"), EditorGUIUtility.TextContent("Ribbon") };
      public GUIContent[] textureModeOptions = new GUIContent[4]{ EditorGUIUtility.TextContent("Stretch"), EditorGUIUtility.TextContent("Tile"), EditorGUIUtility.TextContent("DistributePerSegment"), EditorGUIUtility.TextContent("RepeatPerSegment") };
    }
  }
}
