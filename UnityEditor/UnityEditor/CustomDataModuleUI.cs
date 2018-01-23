// Decompiled with JetBrains decompiler
// Type: UnityEditor.CustomDataModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class CustomDataModuleUI : ModuleUI
  {
    private SerializedProperty[] m_Modes = new SerializedProperty[2];
    private SerializedProperty[] m_VectorComponentCount = new SerializedProperty[2];
    private SerializedMinMaxCurve[,] m_Vectors = new SerializedMinMaxCurve[2, 4];
    private SerializedMinMaxGradient[] m_Colors = new SerializedMinMaxGradient[2];
    private SerializedProperty[,] m_VectorLabels = new SerializedProperty[2, 4];
    private SerializedProperty[] m_ColorLabels = new SerializedProperty[2];
    private const int k_NumCustomDataStreams = 2;
    private const int k_NumChannelsPerStream = 4;
    private static CustomDataModuleUI.Texts s_Texts;

    public CustomDataModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "CustomDataModule", displayName)
    {
      this.m_ToolTip = "Configure custom data to be read in scripts or shaders. Use GetCustomParticleData from script, or send to shaders using the Custom Vertex Streams.";
    }

    protected override void Init()
    {
      if (this.m_Modes[0] != null)
        return;
      if (CustomDataModuleUI.s_Texts == null)
        CustomDataModuleUI.s_Texts = new CustomDataModuleUI.Texts();
      for (int index1 = 0; index1 < 2; ++index1)
      {
        this.m_Modes[index1] = this.GetProperty("mode" + (object) index1);
        this.m_VectorComponentCount[index1] = this.GetProperty("vectorComponentCount" + (object) index1);
        this.m_Colors[index1] = new SerializedMinMaxGradient((SerializedModule) this, "color" + (object) index1);
        this.m_ColorLabels[index1] = this.GetProperty("colorLabel" + (object) index1);
        for (int index2 = 0; index2 < 4; ++index2)
        {
          this.m_Vectors[index1, index2] = new SerializedMinMaxCurve((ModuleUI) this, GUIContent.none, "vector" + (object) index1 + "_" + (object) index2, (ModuleUI.kUseSignedRange ? 1 : 0) != 0);
          this.m_VectorLabels[index1, index2] = this.GetProperty("vectorLabel" + (object) index1 + "_" + (object) index2);
        }
      }
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      for (int index1 = 0; index1 < 2; ++index1)
      {
        GUILayout.BeginVertical("Custom" + (object) (index1 + 1), GUI.skin.window, new GUILayoutOption[0]);
        switch ((CustomDataModuleUI.Mode) ModuleUI.GUIPopup(CustomDataModuleUI.s_Texts.mode, this.m_Modes[index1], CustomDataModuleUI.s_Texts.modes))
        {
          case CustomDataModuleUI.Mode.Vector:
            int num = Mathf.Min(ModuleUI.GUIInt(CustomDataModuleUI.s_Texts.vectorComponentCount, this.m_VectorComponentCount[index1]), 4);
            for (int index2 = 0; index2 < num; ++index2)
              ModuleUI.GUIMinMaxCurve(this.m_VectorLabels[index1, index2], this.m_Vectors[index1, index2]);
            break;
          case CustomDataModuleUI.Mode.Color:
            this.GUIMinMaxGradient(this.m_ColorLabels[index1], this.m_Colors[index1], true);
            break;
        }
        GUILayout.EndVertical();
      }
    }

    private enum Mode
    {
      Disabled,
      Vector,
      Color,
    }

    private class Texts
    {
      public GUIContent mode = EditorGUIUtility.TextContent("Mode|Select the type of data to populate this stream with.");
      public GUIContent vectorComponentCount = EditorGUIUtility.TextContent("Number of Components|How many of the components (XYZW) to fill.");
      public GUIContent[] modes = new GUIContent[3]{ EditorGUIUtility.TextContent("Disabled"), EditorGUIUtility.TextContent("Vector"), EditorGUIUtility.TextContent("Color") };
    }
  }
}
