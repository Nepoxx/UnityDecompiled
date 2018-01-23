// Decompiled with JetBrains decompiler
// Type: UnityEditor.ForceModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ForceModuleUI : ModuleUI
  {
    private SerializedMinMaxCurve m_X;
    private SerializedMinMaxCurve m_Y;
    private SerializedMinMaxCurve m_Z;
    private SerializedProperty m_RandomizePerFrame;
    private SerializedProperty m_InWorldSpace;
    private static ForceModuleUI.Texts s_Texts;

    public ForceModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "ForceModule", displayName)
    {
      this.m_ToolTip = "Controls the force of each particle during its lifetime.";
    }

    protected override void Init()
    {
      if (this.m_X != null)
        return;
      if (ForceModuleUI.s_Texts == null)
        ForceModuleUI.s_Texts = new ForceModuleUI.Texts();
      this.m_X = new SerializedMinMaxCurve((ModuleUI) this, ForceModuleUI.s_Texts.x, "x", ModuleUI.kUseSignedRange);
      this.m_Y = new SerializedMinMaxCurve((ModuleUI) this, ForceModuleUI.s_Texts.y, "y", ModuleUI.kUseSignedRange);
      this.m_Z = new SerializedMinMaxCurve((ModuleUI) this, ForceModuleUI.s_Texts.z, "z", ModuleUI.kUseSignedRange);
      this.m_RandomizePerFrame = this.GetProperty("randomizePerFrame");
      this.m_InWorldSpace = this.GetProperty("inWorldSpace");
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      MinMaxCurveState state = this.m_X.state;
      this.GUITripleMinMaxCurve(GUIContent.none, ForceModuleUI.s_Texts.x, this.m_X, ForceModuleUI.s_Texts.y, this.m_Y, ForceModuleUI.s_Texts.z, this.m_Z, this.m_RandomizePerFrame);
      ModuleUI.GUIBoolAsPopup(ForceModuleUI.s_Texts.space, this.m_InWorldSpace, ForceModuleUI.s_Texts.spaces);
      using (new EditorGUI.DisabledScope(state != MinMaxCurveState.k_TwoScalars && state != MinMaxCurveState.k_TwoCurves))
        ModuleUI.GUIToggle(ForceModuleUI.s_Texts.randomizePerFrame, this.m_RandomizePerFrame);
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      this.Init();
      string empty1 = string.Empty;
      if (!this.m_X.SupportsProcedural(ref empty1))
        text = text + "\nForce over Lifetime module curve X: " + empty1;
      string empty2 = string.Empty;
      if (!this.m_Y.SupportsProcedural(ref empty2))
        text = text + "\nForce over Lifetime module curve Y: " + empty2;
      empty2 = string.Empty;
      if (!this.m_Z.SupportsProcedural(ref empty2))
        text = text + "\nForce over Lifetime module curve Z: " + empty2;
      if (!this.m_RandomizePerFrame.boolValue)
        return;
      text += "\nRandomize is enabled in the Force over Lifetime module.";
    }

    private class Texts
    {
      public GUIContent x = EditorGUIUtility.TextContent("X");
      public GUIContent y = EditorGUIUtility.TextContent("Y");
      public GUIContent z = EditorGUIUtility.TextContent("Z");
      public GUIContent randomizePerFrame = EditorGUIUtility.TextContent("Randomize|Randomize force every frame. Only available when using random between two constants or random between two curves.");
      public GUIContent space = EditorGUIUtility.TextContent("Space|Specifies if the force values are in local space (rotated with the transform) or world space.");
      public string[] spaces = new string[2]{ "Local", "World" };
    }
  }
}
