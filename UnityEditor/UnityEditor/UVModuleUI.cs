// Decompiled with JetBrains decompiler
// Type: UnityEditor.UVModuleUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  internal class UVModuleUI : ModuleUI
  {
    private SerializedProperty m_Mode;
    private SerializedMinMaxCurve m_FrameOverTime;
    private SerializedMinMaxCurve m_StartFrame;
    private SerializedProperty m_TilesX;
    private SerializedProperty m_TilesY;
    private SerializedProperty m_AnimationType;
    private SerializedProperty m_RandomRow;
    private SerializedProperty m_RowIndex;
    private SerializedProperty m_Sprites;
    private SerializedProperty m_Cycles;
    private SerializedProperty m_UVChannelMask;
    private SerializedProperty m_FlipU;
    private SerializedProperty m_FlipV;
    private static UVModuleUI.Texts s_Texts;

    public UVModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName)
      : base(owner, o, "UVModule", displayName)
    {
      this.m_ToolTip = "Particle UV animation. This allows you to specify a texture sheet (a texture with multiple tiles/sub frames) and animate or randomize over it per particle.";
    }

    protected override void Init()
    {
      if (this.m_TilesX != null)
        return;
      if (UVModuleUI.s_Texts == null)
        UVModuleUI.s_Texts = new UVModuleUI.Texts();
      this.m_Mode = this.GetProperty("mode");
      this.m_FrameOverTime = new SerializedMinMaxCurve((ModuleUI) this, UVModuleUI.s_Texts.frameOverTime, "frameOverTime");
      this.m_StartFrame = new SerializedMinMaxCurve((ModuleUI) this, UVModuleUI.s_Texts.startFrame, "startFrame");
      this.m_StartFrame.m_AllowCurves = false;
      this.m_TilesX = this.GetProperty("tilesX");
      this.m_TilesY = this.GetProperty("tilesY");
      this.m_AnimationType = this.GetProperty("animationType");
      this.m_RandomRow = this.GetProperty("randomRow");
      this.m_RowIndex = this.GetProperty("rowIndex");
      this.m_Sprites = this.GetProperty("sprites");
      this.m_Cycles = this.GetProperty("cycles");
      this.m_UVChannelMask = this.GetProperty("uvChannelMask");
      this.m_FlipU = this.GetProperty("flipU");
      this.m_FlipV = this.GetProperty("flipV");
    }

    public override void OnInspectorGUI(InitialModuleUI initial)
    {
      int num1 = ModuleUI.GUIPopup(UVModuleUI.s_Texts.mode, this.m_Mode, UVModuleUI.s_Texts.modes);
      if (!this.m_Mode.hasMultipleDifferentValues)
      {
        if (num1 == 0)
        {
          ModuleUI.GUIIntDraggableX2(UVModuleUI.s_Texts.tiles, UVModuleUI.s_Texts.tilesX, this.m_TilesX, UVModuleUI.s_Texts.tilesY, this.m_TilesY);
          if (ModuleUI.GUIPopup(UVModuleUI.s_Texts.animation, this.m_AnimationType, UVModuleUI.s_Texts.types) == 1)
          {
            ModuleUI.GUIToggle(UVModuleUI.s_Texts.randomRow, this.m_RandomRow);
            if (!this.m_RandomRow.boolValue)
              ModuleUI.GUIInt(UVModuleUI.s_Texts.row, this.m_RowIndex);
            this.m_FrameOverTime.m_RemapValue = (float) this.m_TilesX.intValue;
            this.m_StartFrame.m_RemapValue = (float) this.m_TilesX.intValue;
          }
          else
          {
            this.m_FrameOverTime.m_RemapValue = (float) (this.m_TilesX.intValue * this.m_TilesY.intValue);
            this.m_StartFrame.m_RemapValue = (float) (this.m_TilesX.intValue * this.m_TilesY.intValue);
          }
        }
        else
        {
          this.DoListOfSpritesGUI();
          this.ValidateSpriteList();
          this.m_FrameOverTime.m_RemapValue = (float) this.m_Sprites.arraySize;
          this.m_StartFrame.m_RemapValue = (float) this.m_Sprites.arraySize;
        }
        ModuleUI.GUIMinMaxCurve(UVModuleUI.s_Texts.frameOverTime, this.m_FrameOverTime);
        ModuleUI.GUIMinMaxCurve(UVModuleUI.s_Texts.startFrame, this.m_StartFrame);
      }
      double num2 = (double) ModuleUI.GUIFloat(UVModuleUI.s_Texts.cycles, this.m_Cycles);
      bool disabled = false;
      foreach (Component particleSystem in this.m_ParticleSystemUI.m_ParticleSystems)
      {
        ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.renderMode == ParticleSystemRenderMode.Mesh)
        {
          disabled = true;
          break;
        }
      }
      using (new EditorGUI.DisabledScope(disabled))
      {
        double num3 = (double) ModuleUI.GUIFloat(UVModuleUI.s_Texts.flipU, this.m_FlipU);
        double num4 = (double) ModuleUI.GUIFloat(UVModuleUI.s_Texts.flipV, this.m_FlipV);
      }
      this.m_UVChannelMask.intValue = (int) ModuleUI.GUIEnumMask(UVModuleUI.s_Texts.uvChannelMask, (Enum) (UVChannelFlags) this.m_UVChannelMask.intValue);
    }

    private void DoListOfSpritesGUI()
    {
      for (int index = 0; index < this.m_Sprites.arraySize; ++index)
      {
        GUILayout.BeginHorizontal();
        ModuleUI.GUIObject(new GUIContent(" "), this.m_Sprites.GetArrayElementAtIndex(index).FindPropertyRelative("sprite"), typeof (Sprite), new GUILayoutOption[0]);
        if (index == 0)
        {
          if (GUILayout.Button(GUIContent.none, new GUIStyle((GUIStyle) "OL Plus"), new GUILayoutOption[1]{ GUILayout.Width(16f) }))
          {
            this.m_Sprites.InsertArrayElementAtIndex(this.m_Sprites.arraySize);
            this.m_Sprites.GetArrayElementAtIndex(this.m_Sprites.arraySize - 1).FindPropertyRelative("sprite").objectReferenceValue = (UnityEngine.Object) null;
          }
        }
        else if (GUILayout.Button(GUIContent.none, new GUIStyle((GUIStyle) "OL Minus"), new GUILayoutOption[1]{ GUILayout.Width(16f) }))
          this.m_Sprites.DeleteArrayElementAtIndex(index);
        GUILayout.EndHorizontal();
      }
    }

    private void ValidateSpriteList()
    {
      if (this.m_Sprites.arraySize <= 1)
        return;
      Texture texture = (Texture) null;
      for (int index = 0; index < this.m_Sprites.arraySize; ++index)
      {
        Sprite objectReferenceValue = this.m_Sprites.GetArrayElementAtIndex(index).FindPropertyRelative("sprite").objectReferenceValue as Sprite;
        if ((UnityEngine.Object) objectReferenceValue != (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) texture == (UnityEngine.Object) null)
          {
            texture = objectReferenceValue.GetTextureForPlayMode();
          }
          else
          {
            if ((UnityEngine.Object) texture != (UnityEngine.Object) objectReferenceValue.GetTextureForPlayMode())
            {
              EditorGUILayout.HelpBox("All Sprites must share the same texture. Either pack all Sprites into one Texture by setting the Packing Tag, or use a Multiple Mode Sprite.", MessageType.Error, true);
              break;
            }
            if (objectReferenceValue.border != Vector4.zero)
            {
              EditorGUILayout.HelpBox("Sprite borders are not supported. They will be ignored.", MessageType.Warning, true);
              break;
            }
          }
        }
      }
    }

    private enum AnimationMode
    {
      Grid,
      Sprites,
    }

    private enum AnimationType
    {
      WholeSheet,
      SingleRow,
    }

    private class Texts
    {
      public GUIContent mode = EditorGUIUtility.TextContent("Mode|Animation frames can either be specified on a regular grid texture, or as a list of Sprites.");
      public GUIContent frameOverTime = EditorGUIUtility.TextContent("Frame over Time|Controls the uv animation frame of each particle over its lifetime. On the horisontal axis you will find the lifetime. On the vertical axis you will find the sheet index.");
      public GUIContent startFrame = EditorGUIUtility.TextContent("Start Frame|Phase the animation, so it starts on a frame other than 0.");
      public GUIContent tiles = EditorGUIUtility.TextContent("Tiles|Defines the tiling of the texture.");
      public GUIContent tilesX = EditorGUIUtility.TextContent("X");
      public GUIContent tilesY = EditorGUIUtility.TextContent("Y");
      public GUIContent animation = EditorGUIUtility.TextContent("Animation|Specifies the animation type: Whole Sheet or Single Row. Whole Sheet will animate over the whole texture sheet from left to right, top to bottom. Single Row will animate a single row in the sheet from left to right.");
      public GUIContent randomRow = EditorGUIUtility.TextContent("Random Row|If enabled, the animated row will be chosen randomly.");
      public GUIContent row = EditorGUIUtility.TextContent("Row|The row in the sheet which will be played.");
      public GUIContent sprites = EditorGUIUtility.TextContent("Sprites|The list of Sprites to be played.");
      public GUIContent frame = EditorGUIUtility.TextContent("Frame|The frame in the sheet which will be used.");
      public GUIContent cycles = EditorGUIUtility.TextContent("Cycles|Specifies how many times the animation will loop during the lifetime of the particle.");
      public GUIContent uvChannelMask = EditorGUIUtility.TextContent("Enabled UV Channels|Specifies which UV channels will be animated.");
      public GUIContent flipU = EditorGUIUtility.TextContent("Flip U|Cause some particle texture mapping to be flipped horizontally. (Set between 0 and 1, where a higher value causes more to flip)");
      public GUIContent flipV = EditorGUIUtility.TextContent("Flip V|Cause some particle texture mapping to be flipped vertically. (Set between 0 and 1, where a higher value causes more to flip)");
      public GUIContent[] modes = new GUIContent[2]{ EditorGUIUtility.TextContent("Grid"), EditorGUIUtility.TextContent("Sprites") };
      public GUIContent[] types = new GUIContent[2]{ EditorGUIUtility.TextContent("Whole Sheet"), EditorGUIUtility.TextContent("Single Row") };
    }
  }
}
