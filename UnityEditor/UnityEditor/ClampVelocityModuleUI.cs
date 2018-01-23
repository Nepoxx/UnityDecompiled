// Decompiled with JetBrains decompiler
// Type: UnityEditor.ClampVelocityModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ClampVelocityModuleUI : ModuleUI
  {
    private SerializedMinMaxCurve m_X;
    private SerializedMinMaxCurve m_Y;
    private SerializedMinMaxCurve m_Z;
    private SerializedMinMaxCurve m_Magnitude;
    private SerializedProperty m_SeparateAxes;
    private SerializedProperty m_InWorldSpace;
    private SerializedProperty m_Dampen;
    private SerializedMinMaxCurve m_Drag;
    private SerializedProperty m_MultiplyDragByParticleSize;
    private SerializedProperty m_MultiplyDragByParticleVelocity;
    private static ClampVelocityModuleUI.Texts s_Texts;

    public ClampVelocityModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "ClampVelocityModule", displayName)
    {
      this.m_ToolTip = "Controls the velocity limit and damping of each particle during its lifetime.";
    }

    protected override void Init()
    {
      if (this.m_X != null)
        return;
      if (ClampVelocityModuleUI.s_Texts == null)
        ClampVelocityModuleUI.s_Texts = new ClampVelocityModuleUI.Texts();
      this.m_X = new SerializedMinMaxCurve((ModuleUI) this, ClampVelocityModuleUI.s_Texts.x, "x");
      this.m_Y = new SerializedMinMaxCurve((ModuleUI) this, ClampVelocityModuleUI.s_Texts.y, "y");
      this.m_Z = new SerializedMinMaxCurve((ModuleUI) this, ClampVelocityModuleUI.s_Texts.z, "z");
      this.m_Magnitude = new SerializedMinMaxCurve((ModuleUI) this, ClampVelocityModuleUI.s_Texts.magnitude, "magnitude");
      this.m_SeparateAxes = this.GetProperty("separateAxis");
      this.m_InWorldSpace = this.GetProperty("inWorldSpace");
      this.m_Dampen = this.GetProperty("dampen");
      this.m_Drag = new SerializedMinMaxCurve((ModuleUI) this, ClampVelocityModuleUI.s_Texts.drag, "drag");
      this.m_MultiplyDragByParticleSize = this.GetProperty("multiplyDragByParticleSize");
      this.m_MultiplyDragByParticleVelocity = this.GetProperty("multiplyDragByParticleVelocity");
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      EditorGUI.BeginChangeCheck();
      bool addToCurveEditor = ModuleUI.GUIToggle(ClampVelocityModuleUI.s_Texts.separateAxes, this.m_SeparateAxes);
      if (EditorGUI.EndChangeCheck())
      {
        if (addToCurveEditor)
        {
          this.m_Magnitude.RemoveCurveFromEditor();
        }
        else
        {
          this.m_X.RemoveCurveFromEditor();
          this.m_Y.RemoveCurveFromEditor();
          this.m_Z.RemoveCurveFromEditor();
        }
      }
      if (!this.m_X.stateHasMultipleDifferentValues)
      {
        this.m_Y.SetMinMaxState(this.m_X.state, addToCurveEditor);
        this.m_Z.SetMinMaxState(this.m_X.state, addToCurveEditor);
      }
      if (addToCurveEditor)
      {
        this.GUITripleMinMaxCurve(GUIContent.none, ClampVelocityModuleUI.s_Texts.x, this.m_X, ClampVelocityModuleUI.s_Texts.y, this.m_Y, ClampVelocityModuleUI.s_Texts.z, this.m_Z, (SerializedProperty) null);
        ++EditorGUI.indentLevel;
        ModuleUI.GUIBoolAsPopup(ClampVelocityModuleUI.s_Texts.space, this.m_InWorldSpace, ClampVelocityModuleUI.s_Texts.spaces);
        --EditorGUI.indentLevel;
      }
      else
        ModuleUI.GUIMinMaxCurve(ClampVelocityModuleUI.s_Texts.magnitude, this.m_Magnitude);
      ++EditorGUI.indentLevel;
      double num = (double) ModuleUI.GUIFloat(ClampVelocityModuleUI.s_Texts.dampen, this.m_Dampen);
      --EditorGUI.indentLevel;
      ModuleUI.GUIMinMaxCurve(ClampVelocityModuleUI.s_Texts.drag, this.m_Drag);
      ++EditorGUI.indentLevel;
      ModuleUI.GUIToggle(ClampVelocityModuleUI.s_Texts.multiplyDragByParticleSize, this.m_MultiplyDragByParticleSize);
      ModuleUI.GUIToggle(ClampVelocityModuleUI.s_Texts.multiplyDragByParticleVelocity, this.m_MultiplyDragByParticleVelocity);
      --EditorGUI.indentLevel;
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      text += "\nLimit Velocity over Lifetime module is enabled.";
    }

    private class Texts
    {
      public GUIContent x = EditorGUIUtility.TextContent("X");
      public GUIContent y = EditorGUIUtility.TextContent("Y");
      public GUIContent z = EditorGUIUtility.TextContent("Z");
      public GUIContent dampen = EditorGUIUtility.TextContent("Dampen|Controls how much the velocity that exceeds the velocity limit should be dampened. A value of 0.5 will dampen the exceeding velocity by 50%.");
      public GUIContent magnitude = EditorGUIUtility.TextContent("Speed|The speed limit of particles over the particle lifetime.");
      public GUIContent separateAxes = EditorGUIUtility.TextContent("Separate Axes|If enabled, you can control the velocity limit separately for each axis.");
      public GUIContent space = EditorGUIUtility.TextContent("Space|Specifies if the velocity values are in local space (rotated with the transform) or world space.");
      public string[] spaces = new string[2]{ "Local", "World" };
      public GUIContent drag = EditorGUIUtility.TextContent("Drag|Control the amount of drag applied to each particle during its lifetime.");
      public GUIContent multiplyDragByParticleSize = EditorGUIUtility.TextContent("Multiply by Size|Adjust the drag based on the size of the particles.");
      public GUIContent multiplyDragByParticleVelocity = EditorGUIUtility.TextContent("Multiply by Velocity|Adjust the drag based on the velocity of the particles.");
    }
  }
}
