// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColorByVelocityModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ColorByVelocityModuleUI : ModuleUI
  {
    private static ColorByVelocityModuleUI.Texts s_Texts;
    private SerializedMinMaxGradient m_Gradient;
    private SerializedProperty m_Range;

    public ColorByVelocityModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "ColorBySpeedModule", displayName)
    {
      this.m_ToolTip = "Controls the color of each particle based on its speed.";
    }

    protected override void Init()
    {
      if (this.m_Gradient != null)
        return;
      if (ColorByVelocityModuleUI.s_Texts == null)
        ColorByVelocityModuleUI.s_Texts = new ColorByVelocityModuleUI.Texts();
      this.m_Gradient = new SerializedMinMaxGradient((SerializedModule) this);
      this.m_Gradient.m_AllowColor = false;
      this.m_Gradient.m_AllowRandomBetweenTwoColors = false;
      this.m_Range = this.GetProperty("range");
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      this.GUIMinMaxGradient(ColorByVelocityModuleUI.s_Texts.color, this.m_Gradient, false);
      ModuleUI.GUIMinMaxRange(ColorByVelocityModuleUI.s_Texts.velocityRange, this.m_Range);
    }

    private class Texts
    {
      public GUIContent color = EditorGUIUtility.TextContent("Color|Controls the color of each particle based on its speed.");
      public GUIContent velocityRange = EditorGUIUtility.TextContent("Speed Range|Remaps speed in the defined range to a color.");
    }
  }
}
