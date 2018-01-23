// Decompiled with JetBrains decompiler
// Type: UnityEditor.ColorModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ColorModuleUI : ModuleUI
  {
    private static ColorModuleUI.Texts s_Texts;
    private SerializedMinMaxGradient m_Gradient;

    public ColorModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "ColorModule", displayName)
    {
      this.m_ToolTip = "Controls the color of each particle during its lifetime.";
    }

    protected override void Init()
    {
      if (this.m_Gradient != null)
        return;
      if (ColorModuleUI.s_Texts == null)
        ColorModuleUI.s_Texts = new ColorModuleUI.Texts();
      this.m_Gradient = new SerializedMinMaxGradient((SerializedModule) this);
      this.m_Gradient.m_AllowColor = false;
      this.m_Gradient.m_AllowRandomBetweenTwoColors = false;
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      this.GUIMinMaxGradient(ColorModuleUI.s_Texts.color, this.m_Gradient, false);
    }

    private class Texts
    {
      public GUIContent color = EditorGUIUtility.TextContent("Color|Controls the color of each particle during its lifetime.");
    }
  }
}
