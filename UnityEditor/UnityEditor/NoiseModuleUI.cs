// Decompiled with JetBrains decompiler
// Type: UnityEditor.NoiseModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class NoiseModuleUI : ModuleUI
  {
    private static bool s_PreviewTextureDirty = true;
    private SerializedMinMaxCurve m_StrengthX;
    private SerializedMinMaxCurve m_StrengthY;
    private SerializedMinMaxCurve m_StrengthZ;
    private SerializedProperty m_SeparateAxes;
    private SerializedProperty m_Frequency;
    private SerializedProperty m_Damping;
    private SerializedProperty m_Octaves;
    private SerializedProperty m_OctaveMultiplier;
    private SerializedProperty m_OctaveScale;
    private SerializedProperty m_Quality;
    private SerializedMinMaxCurve m_ScrollSpeed;
    private SerializedMinMaxCurve m_RemapX;
    private SerializedMinMaxCurve m_RemapY;
    private SerializedMinMaxCurve m_RemapZ;
    private SerializedProperty m_RemapEnabled;
    private SerializedMinMaxCurve m_PositionAmount;
    private SerializedMinMaxCurve m_RotationAmount;
    private SerializedMinMaxCurve m_SizeAmount;
    private const int k_PreviewSize = 96;
    private static Texture2D s_PreviewTexture;
    private GUIStyle previewTextureStyle;
    private static NoiseModuleUI.Texts s_Texts;

    public NoiseModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "NoiseModule", displayName)
    {
      this.m_ToolTip = "Add noise/turbulence to particle movement.";
    }

    protected override void Init()
    {
      if (this.m_StrengthX != null)
        return;
      if (NoiseModuleUI.s_Texts == null)
        NoiseModuleUI.s_Texts = new NoiseModuleUI.Texts();
      this.m_StrengthX = new SerializedMinMaxCurve((ModuleUI) this, NoiseModuleUI.s_Texts.x, "strength", ModuleUI.kUseSignedRange);
      this.m_StrengthY = new SerializedMinMaxCurve((ModuleUI) this, NoiseModuleUI.s_Texts.y, "strengthY", ModuleUI.kUseSignedRange);
      this.m_StrengthZ = new SerializedMinMaxCurve((ModuleUI) this, NoiseModuleUI.s_Texts.z, "strengthZ", ModuleUI.kUseSignedRange);
      this.m_SeparateAxes = this.GetProperty("separateAxes");
      this.m_Damping = this.GetProperty("damping");
      this.m_Frequency = this.GetProperty("frequency");
      this.m_Octaves = this.GetProperty("octaves");
      this.m_OctaveMultiplier = this.GetProperty("octaveMultiplier");
      this.m_OctaveScale = this.GetProperty("octaveScale");
      this.m_Quality = this.GetProperty("quality");
      this.m_ScrollSpeed = new SerializedMinMaxCurve((ModuleUI) this, NoiseModuleUI.s_Texts.scrollSpeed, "scrollSpeed", ModuleUI.kUseSignedRange);
      this.m_ScrollSpeed.m_AllowRandom = false;
      this.m_RemapX = new SerializedMinMaxCurve((ModuleUI) this, NoiseModuleUI.s_Texts.x, "remap", ModuleUI.kUseSignedRange);
      this.m_RemapY = new SerializedMinMaxCurve((ModuleUI) this, NoiseModuleUI.s_Texts.y, "remapY", ModuleUI.kUseSignedRange);
      this.m_RemapZ = new SerializedMinMaxCurve((ModuleUI) this, NoiseModuleUI.s_Texts.z, "remapZ", ModuleUI.kUseSignedRange);
      this.m_RemapX.m_AllowRandom = false;
      this.m_RemapY.m_AllowRandom = false;
      this.m_RemapZ.m_AllowRandom = false;
      this.m_RemapX.m_AllowConstant = false;
      this.m_RemapY.m_AllowConstant = false;
      this.m_RemapZ.m_AllowConstant = false;
      this.m_RemapEnabled = this.GetProperty("remapEnabled");
      this.m_PositionAmount = new SerializedMinMaxCurve((ModuleUI) this, NoiseModuleUI.s_Texts.positionAmount, "positionAmount", ModuleUI.kUseSignedRange);
      this.m_RotationAmount = new SerializedMinMaxCurve((ModuleUI) this, NoiseModuleUI.s_Texts.rotationAmount, "rotationAmount", ModuleUI.kUseSignedRange);
      this.m_SizeAmount = new SerializedMinMaxCurve((ModuleUI) this, NoiseModuleUI.s_Texts.sizeAmount, "sizeAmount", ModuleUI.kUseSignedRange);
      if ((Object) NoiseModuleUI.s_PreviewTexture == (Object) null)
      {
        NoiseModuleUI.s_PreviewTexture = new Texture2D(96, 96, TextureFormat.RGBA32, false, true);
        NoiseModuleUI.s_PreviewTexture.name = "ParticleNoisePreview";
        NoiseModuleUI.s_PreviewTexture.filterMode = UnityEngine.FilterMode.Bilinear;
        NoiseModuleUI.s_PreviewTexture.hideFlags = HideFlags.HideAndDontSave;
        NoiseModuleUI.s_Texts.previewTexture.image = (Texture) NoiseModuleUI.s_PreviewTexture;
        NoiseModuleUI.s_Texts.previewTextureMultiEdit.image = (Texture) NoiseModuleUI.s_PreviewTexture;
      }
      NoiseModuleUI.s_PreviewTextureDirty = true;
      this.previewTextureStyle = new GUIStyle(ParticleSystemStyles.Get().label);
      this.previewTextureStyle.alignment = TextAnchor.LowerCenter;
      this.previewTextureStyle.imagePosition = ImagePosition.ImageAbove;
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      if (NoiseModuleUI.s_PreviewTextureDirty)
      {
        if (this.m_ParticleSystemUI.multiEdit)
        {
          Color32[] colors = new Color32[NoiseModuleUI.s_PreviewTexture.width * NoiseModuleUI.s_PreviewTexture.height];
          for (int index = 0; index < colors.Length; ++index)
            colors[index] = new Color32((byte) 120, (byte) 120, (byte) 120, byte.MaxValue);
          NoiseModuleUI.s_PreviewTexture.SetPixels32(colors);
          NoiseModuleUI.s_PreviewTexture.Apply(false);
        }
        else
          this.m_ParticleSystemUI.m_ParticleSystems[0].GenerateNoisePreviewTexture(NoiseModuleUI.s_PreviewTexture);
        NoiseModuleUI.s_PreviewTextureDirty = false;
      }
      if (!this.isWindowView)
      {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
      }
      EditorGUI.BeginChangeCheck();
      bool addToCurveEditor = ModuleUI.GUIToggle(NoiseModuleUI.s_Texts.separateAxes, this.m_SeparateAxes);
      bool flag1 = EditorGUI.EndChangeCheck();
      EditorGUI.BeginChangeCheck();
      if (flag1 && !addToCurveEditor)
      {
        this.m_StrengthY.RemoveCurveFromEditor();
        this.m_StrengthZ.RemoveCurveFromEditor();
        this.m_RemapY.RemoveCurveFromEditor();
        this.m_RemapZ.RemoveCurveFromEditor();
      }
      if (!this.m_StrengthX.stateHasMultipleDifferentValues)
      {
        this.m_StrengthZ.SetMinMaxState(this.m_StrengthX.state, addToCurveEditor);
        this.m_StrengthY.SetMinMaxState(this.m_StrengthX.state, addToCurveEditor);
      }
      if (!this.m_RemapX.stateHasMultipleDifferentValues)
      {
        this.m_RemapZ.SetMinMaxState(this.m_RemapX.state, addToCurveEditor);
        this.m_RemapY.SetMinMaxState(this.m_RemapX.state, addToCurveEditor);
      }
      if (addToCurveEditor)
      {
        this.m_StrengthX.m_DisplayName = NoiseModuleUI.s_Texts.x;
        this.GUITripleMinMaxCurve(GUIContent.none, NoiseModuleUI.s_Texts.x, this.m_StrengthX, NoiseModuleUI.s_Texts.y, this.m_StrengthY, NoiseModuleUI.s_Texts.z, this.m_StrengthZ, (SerializedProperty) null);
      }
      else
      {
        this.m_StrengthX.m_DisplayName = NoiseModuleUI.s_Texts.strength;
        ModuleUI.GUIMinMaxCurve(NoiseModuleUI.s_Texts.strength, this.m_StrengthX);
      }
      double num1 = (double) ModuleUI.GUIFloat(NoiseModuleUI.s_Texts.frequency, this.m_Frequency);
      ModuleUI.GUIMinMaxCurve(NoiseModuleUI.s_Texts.scrollSpeed, this.m_ScrollSpeed);
      ModuleUI.GUIToggle(NoiseModuleUI.s_Texts.damping, this.m_Damping);
      using (new EditorGUI.DisabledScope(ModuleUI.GUIInt(NoiseModuleUI.s_Texts.octaves, this.m_Octaves) == 1))
      {
        double num2 = (double) ModuleUI.GUIFloat(NoiseModuleUI.s_Texts.octaveMultiplier, this.m_OctaveMultiplier);
        double num3 = (double) ModuleUI.GUIFloat(NoiseModuleUI.s_Texts.octaveScale, this.m_OctaveScale);
      }
      ModuleUI.GUIPopup(NoiseModuleUI.s_Texts.quality, this.m_Quality, NoiseModuleUI.s_Texts.qualityDropdown);
      EditorGUI.BeginChangeCheck();
      bool flag2 = ModuleUI.GUIToggle(NoiseModuleUI.s_Texts.remap, this.m_RemapEnabled);
      if (EditorGUI.EndChangeCheck() && !flag2)
      {
        this.m_RemapX.RemoveCurveFromEditor();
        this.m_RemapY.RemoveCurveFromEditor();
        this.m_RemapZ.RemoveCurveFromEditor();
      }
      using (new EditorGUI.DisabledScope(!flag2))
      {
        if (addToCurveEditor)
        {
          this.m_RemapX.m_DisplayName = NoiseModuleUI.s_Texts.x;
          this.GUITripleMinMaxCurve(GUIContent.none, NoiseModuleUI.s_Texts.x, this.m_RemapX, NoiseModuleUI.s_Texts.y, this.m_RemapY, NoiseModuleUI.s_Texts.z, this.m_RemapZ, (SerializedProperty) null);
        }
        else
        {
          this.m_RemapX.m_DisplayName = NoiseModuleUI.s_Texts.remap;
          ModuleUI.GUIMinMaxCurve(NoiseModuleUI.s_Texts.remapCurve, this.m_RemapX);
        }
      }
      ModuleUI.GUIMinMaxCurve(NoiseModuleUI.s_Texts.positionAmount, this.m_PositionAmount);
      ModuleUI.GUIMinMaxCurve(NoiseModuleUI.s_Texts.rotationAmount, this.m_RotationAmount);
      ModuleUI.GUIMinMaxCurve(NoiseModuleUI.s_Texts.sizeAmount, this.m_SizeAmount);
      if (!this.isWindowView)
        GUILayout.EndVertical();
      if (EditorGUI.EndChangeCheck() || (double) this.m_ScrollSpeed.scalar.floatValue != 0.0 || (flag2 || flag1))
      {
        NoiseModuleUI.s_PreviewTextureDirty = true;
        this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner.Repaint();
      }
      if (this.m_ParticleSystemUI.multiEdit)
        GUILayout.Label(NoiseModuleUI.s_Texts.previewTextureMultiEdit, this.previewTextureStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
      else
        GUILayout.Label(NoiseModuleUI.s_Texts.previewTexture, this.previewTextureStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
      if (this.isWindowView)
        return;
      GUILayout.EndHorizontal();
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      text += "\nNoise module is enabled.";
    }

    private class Texts
    {
      public GUIContent separateAxes = EditorGUIUtility.TextContent("Separate Axes|If enabled, you can control the noise separately for each axis.");
      public GUIContent strength = EditorGUIUtility.TextContent("Strength|How strong the overall noise effect is.");
      public GUIContent frequency = EditorGUIUtility.TextContent("Frequency|Low values create soft, smooth noise, and high values create rapidly changing noise.");
      public GUIContent damping = EditorGUIUtility.TextContent("Damping|If enabled, strength is proportional to frequency.");
      public GUIContent octaves = EditorGUIUtility.TextContent("Octaves|Layers of noise that combine to produce final noise (Adding octaves increases the performance cost substantially!)");
      public GUIContent octaveMultiplier = EditorGUIUtility.TextContent("Octave Multiplier|When combining each octave, scale the intensity by this amount.");
      public GUIContent octaveScale = EditorGUIUtility.TextContent("Octave Scale|When combining each octave, zoom in by this amount.");
      public GUIContent quality = EditorGUIUtility.TextContent("Quality|Generate 1D, 2D or 3D noise.");
      public GUIContent scrollSpeed = EditorGUIUtility.TextContent("Scroll Speed|Scroll the noise map over the particle system.");
      public GUIContent remap = EditorGUIUtility.TextContent("Remap|Remap the final noise values into a new range.");
      public GUIContent remapCurve = EditorGUIUtility.TextContent("Remap Curve");
      public GUIContent positionAmount = EditorGUIUtility.TextContent("Position Amount|What proportion of the noise is applied to the particle positions.");
      public GUIContent rotationAmount = EditorGUIUtility.TextContent("Rotation Amount|What proportion of the noise is applied to the particle rotations, in degrees per second.");
      public GUIContent sizeAmount = EditorGUIUtility.TextContent("Size Amount|Multiply the size of the particle by a proportion of the noise.");
      public GUIContent x = EditorGUIUtility.TextContent("X");
      public GUIContent y = EditorGUIUtility.TextContent("Y");
      public GUIContent z = EditorGUIUtility.TextContent("Z");
      public GUIContent previewTexture = EditorGUIUtility.TextContent("Preview|Preview the noise as a texture.");
      public GUIContent previewTextureMultiEdit = EditorGUIUtility.TextContent("Preview (Disabled)|Preview is disabled in multi-object editing mode.");
      public GUIContent[] qualityDropdown = new GUIContent[3]{ EditorGUIUtility.TextContent("Low (1D)"), EditorGUIUtility.TextContent("Medium (2D)"), EditorGUIUtility.TextContent("High (3D)") };
    }
  }
}
