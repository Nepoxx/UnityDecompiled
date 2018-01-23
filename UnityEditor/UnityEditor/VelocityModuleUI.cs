// Decompiled with JetBrains decompiler
// Type: UnityEditor.VelocityModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class VelocityModuleUI : ModuleUI
  {
    private SerializedMinMaxCurve m_X;
    private SerializedMinMaxCurve m_Y;
    private SerializedMinMaxCurve m_Z;
    private SerializedProperty m_InWorldSpace;
    private SerializedMinMaxCurve m_SpeedModifier;
    private static VelocityModuleUI.Texts s_Texts;

    public VelocityModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "VelocityModule", displayName)
    {
      this.m_ToolTip = "Controls the velocity of each particle during its lifetime.";
    }

    protected override void Init()
    {
      if (this.m_X != null)
        return;
      if (VelocityModuleUI.s_Texts == null)
        VelocityModuleUI.s_Texts = new VelocityModuleUI.Texts();
      this.m_X = new SerializedMinMaxCurve((ModuleUI) this, VelocityModuleUI.s_Texts.x, "x", ModuleUI.kUseSignedRange);
      this.m_Y = new SerializedMinMaxCurve((ModuleUI) this, VelocityModuleUI.s_Texts.y, "y", ModuleUI.kUseSignedRange);
      this.m_Z = new SerializedMinMaxCurve((ModuleUI) this, VelocityModuleUI.s_Texts.z, "z", ModuleUI.kUseSignedRange);
      this.m_InWorldSpace = this.GetProperty("inWorldSpace");
      this.m_SpeedModifier = new SerializedMinMaxCurve((ModuleUI) this, VelocityModuleUI.s_Texts.speedMultiplier, "speedModifier", ModuleUI.kUseSignedRange);
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      this.GUITripleMinMaxCurve(GUIContent.none, VelocityModuleUI.s_Texts.x, this.m_X, VelocityModuleUI.s_Texts.y, this.m_Y, VelocityModuleUI.s_Texts.z, this.m_Z, (SerializedProperty) null);
      ModuleUI.GUIBoolAsPopup(VelocityModuleUI.s_Texts.space, this.m_InWorldSpace, VelocityModuleUI.s_Texts.spaces);
      ModuleUI.GUIMinMaxCurve(VelocityModuleUI.s_Texts.speedMultiplier, this.m_SpeedModifier);
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      this.Init();
      string empty1 = string.Empty;
      if (!this.m_X.SupportsProcedural(ref empty1))
        text = text + "\nVelocity over Lifetime module curve X: " + empty1;
      string empty2 = string.Empty;
      if (!this.m_Y.SupportsProcedural(ref empty2))
        text = text + "\nVelocity over Lifetime module curve Y: " + empty2;
      empty2 = string.Empty;
      if (!this.m_Z.SupportsProcedural(ref empty2))
        text = text + "\nVelocity over Lifetime module curve Z: " + empty2;
      empty2 = string.Empty;
      if (this.m_SpeedModifier.state == MinMaxCurveState.k_Scalar && (double) this.m_SpeedModifier.maxConstant == 1.0)
        return;
      text += "\nVelocity over Lifetime module curve Speed Multiplier is being used";
    }

    private class Texts
    {
      public GUIContent x = EditorGUIUtility.TextContent("X");
      public GUIContent y = EditorGUIUtility.TextContent("Y");
      public GUIContent z = EditorGUIUtility.TextContent("Z");
      public GUIContent space = EditorGUIUtility.TextContent("Space|Specifies if the velocity values are in local space (rotated with the transform) or world space.");
      public GUIContent speedMultiplier = EditorGUIUtility.TextContent("Speed Modifier|Multiply the particle speed by this value");
      public string[] spaces = new string[2]{ "Local", "World" };
    }
  }
}
