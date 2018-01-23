// Decompiled with JetBrains decompiler
// Type: UnityEditor.InheritVelocityModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class InheritVelocityModuleUI : ModuleUI
  {
    private SerializedProperty m_Mode;
    private SerializedMinMaxCurve m_Curve;
    private static InheritVelocityModuleUI.Texts s_Texts;

    public InheritVelocityModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "InheritVelocityModule", displayName)
    {
      this.m_ToolTip = "Controls the velocity inherited from the emitter, for each particle.";
    }

    protected override void Init()
    {
      if (this.m_Curve != null)
        return;
      if (InheritVelocityModuleUI.s_Texts == null)
        InheritVelocityModuleUI.s_Texts = new InheritVelocityModuleUI.Texts();
      this.m_Mode = this.GetProperty("m_Mode");
      this.m_Curve = new SerializedMinMaxCurve((ModuleUI) this, GUIContent.none, "m_Curve", ModuleUI.kUseSignedRange);
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      ModuleUI.GUIPopup(InheritVelocityModuleUI.s_Texts.mode, this.m_Mode, InheritVelocityModuleUI.s_Texts.modes);
      ModuleUI.GUIMinMaxCurve(InheritVelocityModuleUI.s_Texts.velocity, this.m_Curve);
    }

    public override void UpdateCullingSupportedString(ref string text)
    {
      this.Init();
      string empty = string.Empty;
      if (this.m_Curve.SupportsProcedural(ref empty))
        return;
      text = text + "\nInherit Velocity module curve: " + empty;
    }

    private enum Modes
    {
      Initial,
      Current,
    }

    private class Texts
    {
      public GUIContent mode = EditorGUIUtility.TextContent("Mode|Specifies whether the emitter velocity is inherited as a one-shot when a particle is born, always using the current emitter velocity, or using the emitter velocity when the particle was born.");
      public GUIContent velocity = EditorGUIUtility.TextContent("Multiplier|Controls the amount of emitter velocity inherited during each particle's lifetime.");
      public GUIContent[] modes = new GUIContent[2]{ EditorGUIUtility.TextContent("Initial"), EditorGUIUtility.TextContent("Current") };
    }
  }
}
