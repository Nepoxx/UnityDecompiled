// Decompiled with JetBrains decompiler
// Type: UnityEditor.RotationByVelocityModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class RotationByVelocityModuleUI : ModuleUI
  {
    private static RotationByVelocityModuleUI.Texts s_Texts;
    private SerializedMinMaxCurve m_X;
    private SerializedMinMaxCurve m_Y;
    private SerializedMinMaxCurve m_Z;
    private SerializedProperty m_SeparateAxes;
    private SerializedProperty m_Range;

    public RotationByVelocityModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "RotationBySpeedModule", displayName)
    {
      this.m_ToolTip = "Controls the angular velocity of each particle based on its speed.";
    }

    protected override void Init()
    {
      if (this.m_Z != null)
        return;
      if (RotationByVelocityModuleUI.s_Texts == null)
        RotationByVelocityModuleUI.s_Texts = new RotationByVelocityModuleUI.Texts();
      this.m_SeparateAxes = this.GetProperty("separateAxes");
      this.m_Range = this.GetProperty("range");
      this.m_X = new SerializedMinMaxCurve((ModuleUI) this, RotationByVelocityModuleUI.s_Texts.x, "x", ModuleUI.kUseSignedRange, false, this.m_SeparateAxes.boolValue);
      this.m_Y = new SerializedMinMaxCurve((ModuleUI) this, RotationByVelocityModuleUI.s_Texts.y, "y", ModuleUI.kUseSignedRange, false, this.m_SeparateAxes.boolValue);
      this.m_Z = new SerializedMinMaxCurve((ModuleUI) this, RotationByVelocityModuleUI.s_Texts.z, "curve", ModuleUI.kUseSignedRange);
      this.m_X.m_RemapValue = 57.29578f;
      this.m_Y.m_RemapValue = 57.29578f;
      this.m_Z.m_RemapValue = 57.29578f;
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      EditorGUI.BeginChangeCheck();
      bool addToCurveEditor = ModuleUI.GUIToggle(RotationByVelocityModuleUI.s_Texts.separateAxes, this.m_SeparateAxes);
      if (EditorGUI.EndChangeCheck() && !addToCurveEditor)
      {
        this.m_X.RemoveCurveFromEditor();
        this.m_Y.RemoveCurveFromEditor();
      }
      if (!this.m_Z.stateHasMultipleDifferentValues)
      {
        this.m_X.SetMinMaxState(this.m_Z.state, addToCurveEditor);
        this.m_Y.SetMinMaxState(this.m_Z.state, addToCurveEditor);
      }
      MinMaxCurveState state = this.m_Z.state;
      if (addToCurveEditor)
      {
        this.m_Z.m_DisplayName = RotationByVelocityModuleUI.s_Texts.z;
        this.GUITripleMinMaxCurve(GUIContent.none, RotationByVelocityModuleUI.s_Texts.x, this.m_X, RotationByVelocityModuleUI.s_Texts.y, this.m_Y, RotationByVelocityModuleUI.s_Texts.z, this.m_Z, (SerializedProperty) null);
      }
      else
      {
        this.m_Z.m_DisplayName = RotationByVelocityModuleUI.s_Texts.rotation;
        ModuleUI.GUIMinMaxCurve(RotationByVelocityModuleUI.s_Texts.rotation, this.m_Z);
      }
      using (new EditorGUI.DisabledScope(state == MinMaxCurveState.k_Scalar || state == MinMaxCurveState.k_TwoScalars))
        ModuleUI.GUIMinMaxRange(RotationByVelocityModuleUI.s_Texts.velocityRange, this.m_Range);
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      text += "\nRotation by Speed module is enabled.";
    }

    private class Texts
    {
      public GUIContent velocityRange = EditorGUIUtility.TextContent("Speed Range|Maps the speed to a value along the curve, when using one of the curve modes.");
      public GUIContent rotation = EditorGUIUtility.TextContent("Angular Velocity|Controls the angular velocity of each particle based on its speed.");
      public GUIContent separateAxes = EditorGUIUtility.TextContent("Separate Axes|If enabled, you can control the angular velocity limit separately for each axis.");
      public GUIContent x = EditorGUIUtility.TextContent("X");
      public GUIContent y = EditorGUIUtility.TextContent("Y");
      public GUIContent z = EditorGUIUtility.TextContent("Z");
    }
  }
}
