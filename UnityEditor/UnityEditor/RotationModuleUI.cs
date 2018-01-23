// Decompiled with JetBrains decompiler
// Type: UnityEditor.RotationModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class RotationModuleUI : ModuleUI
  {
    private SerializedMinMaxCurve m_X;
    private SerializedMinMaxCurve m_Y;
    private SerializedMinMaxCurve m_Z;
    private SerializedProperty m_SeparateAxes;
    private static RotationModuleUI.Texts s_Texts;

    public RotationModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "RotationModule", displayName)
    {
      this.m_ToolTip = "Controls the angular velocity of each particle during its lifetime.";
    }

    protected override void Init()
    {
      if (this.m_Z != null)
        return;
      if (RotationModuleUI.s_Texts == null)
        RotationModuleUI.s_Texts = new RotationModuleUI.Texts();
      this.m_SeparateAxes = this.GetProperty("separateAxes");
      this.m_X = new SerializedMinMaxCurve((ModuleUI) this, RotationModuleUI.s_Texts.x, "x", ModuleUI.kUseSignedRange, false, this.m_SeparateAxes.boolValue);
      this.m_Y = new SerializedMinMaxCurve((ModuleUI) this, RotationModuleUI.s_Texts.y, "y", ModuleUI.kUseSignedRange, false, this.m_SeparateAxes.boolValue);
      this.m_Z = new SerializedMinMaxCurve((ModuleUI) this, RotationModuleUI.s_Texts.z, "curve", ModuleUI.kUseSignedRange);
      this.m_X.m_RemapValue = 57.29578f;
      this.m_Y.m_RemapValue = 57.29578f;
      this.m_Z.m_RemapValue = 57.29578f;
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      EditorGUI.BeginChangeCheck();
      bool addToCurveEditor = ModuleUI.GUIToggle(RotationModuleUI.s_Texts.separateAxes, this.m_SeparateAxes);
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
      if (addToCurveEditor)
      {
        this.m_Z.m_DisplayName = RotationModuleUI.s_Texts.z;
        this.GUITripleMinMaxCurve(GUIContent.none, RotationModuleUI.s_Texts.x, this.m_X, RotationModuleUI.s_Texts.y, this.m_Y, RotationModuleUI.s_Texts.z, this.m_Z, (SerializedProperty) null);
      }
      else
      {
        this.m_Z.m_DisplayName = RotationModuleUI.s_Texts.rotation;
        ModuleUI.GUIMinMaxCurve(RotationModuleUI.s_Texts.rotation, this.m_Z);
      }
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      this.Init();
      string empty1 = string.Empty;
      if (!this.m_X.SupportsProcedural(ref empty1))
        text = text + "\nRotation over Lifetime module curve X: " + empty1;
      string empty2 = string.Empty;
      if (!this.m_Y.SupportsProcedural(ref empty2))
        text = text + "\nRotation over Lifetime module curve Y: " + empty2;
      empty2 = string.Empty;
      if (this.m_Z.SupportsProcedural(ref empty2))
        return;
      text = text + "\nRotation over Lifetime module curve Z: " + empty2;
    }

    private class Texts
    {
      public GUIContent rotation = EditorGUIUtility.TextContent("Angular Velocity|Controls the angular velocity of each particle during its lifetime.");
      public GUIContent separateAxes = EditorGUIUtility.TextContent("Separate Axes|If enabled, you can control the angular velocity limit separately for each axis.");
      public GUIContent x = EditorGUIUtility.TextContent("X");
      public GUIContent y = EditorGUIUtility.TextContent("Y");
      public GUIContent z = EditorGUIUtility.TextContent("Z");
    }
  }
}
