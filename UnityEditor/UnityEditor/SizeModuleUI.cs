// Decompiled with JetBrains decompiler
// Type: UnityEditor.SizeModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SizeModuleUI : ModuleUI
  {
    private SerializedMinMaxCurve m_X;
    private SerializedMinMaxCurve m_Y;
    private SerializedMinMaxCurve m_Z;
    private SerializedProperty m_SeparateAxes;
    private static SizeModuleUI.Texts s_Texts;

    public SizeModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "SizeModule", displayName)
    {
      this.m_ToolTip = "Controls the size of each particle during its lifetime.";
    }

    protected override void Init()
    {
      if (this.m_X != null)
        return;
      if (SizeModuleUI.s_Texts == null)
        SizeModuleUI.s_Texts = new SizeModuleUI.Texts();
      this.m_SeparateAxes = this.GetProperty("separateAxes");
      this.m_X = new SerializedMinMaxCurve((ModuleUI) this, SizeModuleUI.s_Texts.x, "curve");
      this.m_Y = new SerializedMinMaxCurve((ModuleUI) this, SizeModuleUI.s_Texts.y, "y", false, false, this.m_SeparateAxes.boolValue);
      this.m_Z = new SerializedMinMaxCurve((ModuleUI) this, SizeModuleUI.s_Texts.z, "z", false, false, this.m_SeparateAxes.boolValue);
      this.m_X.m_AllowConstant = false;
      this.m_Y.m_AllowConstant = false;
      this.m_Z.m_AllowConstant = false;
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      EditorGUI.BeginChangeCheck();
      bool addToCurveEditor = ModuleUI.GUIToggle(SizeModuleUI.s_Texts.separateAxes, this.m_SeparateAxes);
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
      if (addToCurveEditor)
      {
        this.m_X.m_DisplayName = SizeModuleUI.s_Texts.x;
        this.GUITripleMinMaxCurve(GUIContent.none, SizeModuleUI.s_Texts.x, this.m_X, SizeModuleUI.s_Texts.y, this.m_Y, SizeModuleUI.s_Texts.z, this.m_Z, (SerializedProperty) null);
      }
      else
      {
        this.m_X.m_DisplayName = SizeModuleUI.s_Texts.size;
        ModuleUI.GUIMinMaxCurve(SizeModuleUI.s_Texts.size, this.m_X);
      }
    }

    private class Texts
    {
      public GUIContent size = EditorGUIUtility.TextContent("Size|Controls the size of each particle during its lifetime.");
      public GUIContent separateAxes = EditorGUIUtility.TextContent("Separate Axes|If enabled, you can control the angular velocity limit separately for each axis.");
      public GUIContent x = EditorGUIUtility.TextContent("X");
      public GUIContent y = EditorGUIUtility.TextContent("Y");
      public GUIContent z = EditorGUIUtility.TextContent("Z");
    }
  }
}
