// Decompiled with JetBrains decompiler
// Type: UnityEditor.SizeByVelocityModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SizeByVelocityModuleUI : ModuleUI
  {
    private static SizeByVelocityModuleUI.Texts s_Texts;
    private SerializedMinMaxCurve m_X;
    private SerializedMinMaxCurve m_Y;
    private SerializedMinMaxCurve m_Z;
    private SerializedProperty m_SeparateAxes;
    private SerializedProperty m_Range;

    public SizeByVelocityModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "SizeBySpeedModule", displayName)
    {
      this.m_ToolTip = "Controls the size of each particle based on its speed.";
    }

    protected override void Init()
    {
      if (this.m_X != null)
        return;
      if (SizeByVelocityModuleUI.s_Texts == null)
        SizeByVelocityModuleUI.s_Texts = new SizeByVelocityModuleUI.Texts();
      this.m_SeparateAxes = this.GetProperty("separateAxes");
      this.m_Range = this.GetProperty("range");
      this.m_X = new SerializedMinMaxCurve((ModuleUI) this, SizeByVelocityModuleUI.s_Texts.x, "curve");
      this.m_X.m_AllowConstant = false;
      this.m_Y = new SerializedMinMaxCurve((ModuleUI) this, SizeByVelocityModuleUI.s_Texts.y, "y", false, false, this.m_SeparateAxes.boolValue);
      this.m_Y.m_AllowConstant = false;
      this.m_Z = new SerializedMinMaxCurve((ModuleUI) this, SizeByVelocityModuleUI.s_Texts.z, "z", false, false, this.m_SeparateAxes.boolValue);
      this.m_Z.m_AllowConstant = false;
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      EditorGUI.BeginChangeCheck();
      bool addToCurveEditor = ModuleUI.GUIToggle(SizeByVelocityModuleUI.s_Texts.separateAxes, this.m_SeparateAxes);
      if (EditorGUI.EndChangeCheck() && !addToCurveEditor)
      {
        this.m_Y.RemoveCurveFromEditor();
        this.m_Z.RemoveCurveFromEditor();
      }
      if (!this.m_X.stateHasMultipleDifferentValues)
      {
        this.m_Z.SetMinMaxState(this.m_X.state, addToCurveEditor);
        this.m_Y.SetMinMaxState(this.m_X.state, addToCurveEditor);
      }
      MinMaxCurveState state = this.m_Z.state;
      if (addToCurveEditor)
      {
        this.m_X.m_DisplayName = SizeByVelocityModuleUI.s_Texts.x;
        this.GUITripleMinMaxCurve(GUIContent.none, SizeByVelocityModuleUI.s_Texts.x, this.m_X, SizeByVelocityModuleUI.s_Texts.y, this.m_Y, SizeByVelocityModuleUI.s_Texts.z, this.m_Z, (SerializedProperty) null);
      }
      else
      {
        this.m_X.m_DisplayName = SizeByVelocityModuleUI.s_Texts.size;
        ModuleUI.GUIMinMaxCurve(SizeByVelocityModuleUI.s_Texts.size, this.m_X);
      }
      using (new EditorGUI.DisabledScope(state == MinMaxCurveState.k_Scalar || state == MinMaxCurveState.k_TwoScalars))
        ModuleUI.GUIMinMaxRange(SizeByVelocityModuleUI.s_Texts.velocityRange, this.m_Range);
    }

    private class Texts
    {
      public GUIContent velocityRange = EditorGUIUtility.TextContent("Speed Range|Remaps speed in the defined range to a size.");
      public GUIContent size = EditorGUIUtility.TextContent("Size|Controls the size of each particle based on its speed.");
      public GUIContent separateAxes = new GUIContent("Separate Axes", "If enabled, you can control the angular velocity limit separately for each axis.");
      public GUIContent x = new GUIContent("X");
      public GUIContent y = new GUIContent("Y");
      public GUIContent z = new GUIContent("Z");
    }
  }
}
