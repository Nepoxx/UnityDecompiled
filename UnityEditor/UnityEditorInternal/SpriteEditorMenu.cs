// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.SpriteEditorMenu
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditorInternal
{
  internal class SpriteEditorMenu : EditorWindow
  {
    private static SpriteEditorMenu.Styles s_Styles;
    private static long s_LastClosedTime;
    private static SpriteEditorMenuSetting s_Setting;
    private ITexture2D m_PreviewTexture;
    private ITexture2D m_SelectedTexture;
    private SpriteFrameModule m_SpriteFrameModule;

    private void Init(Rect buttonRect, SpriteFrameModule sf, ITexture2D previewTexture, ITexture2D selectedTexture)
    {
      if ((UnityEngine.Object) SpriteEditorMenu.s_Setting == (UnityEngine.Object) null)
        SpriteEditorMenu.s_Setting = ScriptableObject.CreateInstance<SpriteEditorMenuSetting>();
      this.m_SpriteFrameModule = sf;
      this.m_PreviewTexture = previewTexture;
      this.m_SelectedTexture = selectedTexture;
      buttonRect = GUIUtility.GUIToScreenRect(buttonRect);
      Vector2 windowSize = new Vector2(300f, 145f);
      this.ShowAsDropDown(buttonRect, windowSize, (PopupLocationHelper.PopupLocation[]) null, ShowMode.PopupMenuWithKeyboardFocus);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    private void UndoRedoPerformed()
    {
      this.Repaint();
    }

    private void OnEnable()
    {
      AssemblyReloadEvents.beforeAssemblyReload += new AssemblyReloadEvents.AssemblyReloadCallback(((EditorWindow) this).Close);
    }

    private void OnDisable()
    {
      AssemblyReloadEvents.beforeAssemblyReload -= new AssemblyReloadEvents.AssemblyReloadCallback(((EditorWindow) this).Close);
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      SpriteEditorMenu.s_LastClosedTime = DateTime.Now.Ticks / 10000L;
    }

    internal static bool ShowAtPosition(Rect buttonRect, SpriteFrameModule sf, ITexture2D previewTexture, ITexture2D selectedTexture)
    {
      if (DateTime.Now.Ticks / 10000L < SpriteEditorMenu.s_LastClosedTime + 50L)
        return false;
      if (UnityEngine.Event.current != null)
        UnityEngine.Event.current.Use();
      ScriptableObject.CreateInstance<SpriteEditorMenu>().Init(buttonRect, sf, previewTexture, selectedTexture);
      return true;
    }

    private void OnGUI()
    {
      if (SpriteEditorMenu.s_Styles == null)
        SpriteEditorMenu.s_Styles = new SpriteEditorMenu.Styles();
      GUILayout.Space(4f);
      EditorGUIUtility.labelWidth = 124f;
      EditorGUIUtility.wideMode = true;
      GUI.Label(new Rect(0.0f, 0.0f, this.position.width, this.position.height), GUIContent.none, SpriteEditorMenu.s_Styles.background);
      EditorGUI.BeginChangeCheck();
      SpriteEditorMenuSetting.SlicingType slicingType1 = SpriteEditorMenu.s_Setting.slicingType;
      SpriteEditorMenuSetting.SlicingType slicingType2 = (SpriteEditorMenuSetting.SlicingType) EditorGUILayout.EnumPopup(SpriteEditorMenu.s_Styles.typeLabel, (Enum) slicingType1, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change slicing type");
        SpriteEditorMenu.s_Setting.slicingType = slicingType2;
      }
      switch (slicingType2)
      {
        case SpriteEditorMenuSetting.SlicingType.Automatic:
          this.OnAutomaticGUI();
          break;
        case SpriteEditorMenuSetting.SlicingType.GridByCellSize:
        case SpriteEditorMenuSetting.SlicingType.GridByCellCount:
          this.OnGridGUI();
          break;
      }
      GUILayout.BeginHorizontal();
      GUILayout.Space(EditorGUIUtility.labelWidth + 4f);
      if (GUILayout.Button(SpriteEditorMenu.s_Styles.sliceButtonLabel))
        this.DoSlicing();
      GUILayout.EndHorizontal();
    }

    private void DoSlicing()
    {
      this.DoAnalytics();
      switch (SpriteEditorMenu.s_Setting.slicingType)
      {
        case SpriteEditorMenuSetting.SlicingType.Automatic:
          this.DoAutomaticSlicing();
          break;
        case SpriteEditorMenuSetting.SlicingType.GridByCellSize:
        case SpriteEditorMenuSetting.SlicingType.GridByCellCount:
          this.DoGridSlicing();
          break;
      }
    }

    private void DoAnalytics()
    {
      UsabilityAnalytics.Event("Sprite Editor", "Slice", "Type", (int) SpriteEditorMenu.s_Setting.slicingType);
      if (this.m_SelectedTexture != (ITexture2D) null)
      {
        UsabilityAnalytics.Event("Sprite Editor", "Slice", "Texture Width", this.m_SelectedTexture.width);
        UsabilityAnalytics.Event("Sprite Editor", "Slice", "Texture Height", this.m_SelectedTexture.height);
      }
      if (SpriteEditorMenu.s_Setting.slicingType == SpriteEditorMenuSetting.SlicingType.Automatic)
      {
        UsabilityAnalytics.Event("Sprite Editor", "Slice", "Auto Slicing Method", SpriteEditorMenu.s_Setting.autoSlicingMethod);
      }
      else
      {
        UsabilityAnalytics.Event("Sprite Editor", "Slice", "Grid Slicing Size X", (int) SpriteEditorMenu.s_Setting.gridSpriteSize.x);
        UsabilityAnalytics.Event("Sprite Editor", "Slice", "Grid Slicing Size Y", (int) SpriteEditorMenu.s_Setting.gridSpriteSize.y);
        UsabilityAnalytics.Event("Sprite Editor", "Slice", "Grid Slicing Offset X", (int) SpriteEditorMenu.s_Setting.gridSpriteOffset.x);
        UsabilityAnalytics.Event("Sprite Editor", "Slice", "Grid Slicing Offset Y", (int) SpriteEditorMenu.s_Setting.gridSpriteOffset.y);
        UsabilityAnalytics.Event("Sprite Editor", "Slice", "Grid Slicing Padding X", (int) SpriteEditorMenu.s_Setting.gridSpritePadding.x);
        UsabilityAnalytics.Event("Sprite Editor", "Slice", "Grid Slicing Padding Y", (int) SpriteEditorMenu.s_Setting.gridSpritePadding.y);
      }
    }

    private void TwoIntFields(GUIContent label, GUIContent labelX, GUIContent labelY, ref int x, ref int y)
    {
      float num = 16f;
      Rect rect = GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, num, num, EditorStyles.numberField);
      Rect position1 = rect;
      position1.width = EditorGUIUtility.labelWidth;
      position1.height = 16f;
      GUI.Label(position1, label);
      Rect position2 = rect;
      position2.width -= EditorGUIUtility.labelWidth;
      position2.height = 16f;
      position2.x += EditorGUIUtility.labelWidth;
      position2.width /= 2f;
      position2.width -= 2f;
      EditorGUIUtility.labelWidth = 12f;
      x = EditorGUI.IntField(position2, labelX, x);
      position2.x += position2.width + 3f;
      y = EditorGUI.IntField(position2, labelY, y);
      EditorGUIUtility.labelWidth = position1.width;
    }

    private void OnGridGUI()
    {
      int max1 = !(this.m_PreviewTexture != (ITexture2D) null) ? 4096 : this.m_PreviewTexture.width;
      int max2 = !(this.m_PreviewTexture != (ITexture2D) null) ? 4096 : this.m_PreviewTexture.height;
      if (SpriteEditorMenu.s_Setting.slicingType == SpriteEditorMenuSetting.SlicingType.GridByCellCount)
      {
        int x = (int) SpriteEditorMenu.s_Setting.gridCellCount.x;
        int y = (int) SpriteEditorMenu.s_Setting.gridCellCount.y;
        EditorGUI.BeginChangeCheck();
        this.TwoIntFields(SpriteEditorMenu.s_Styles.columnAndRowLabel, SpriteEditorMenu.s_Styles.columnLabel, SpriteEditorMenu.s_Styles.rowLabel, ref x, ref y);
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change column & row");
          SpriteEditorMenu.s_Setting.gridCellCount.x = (float) Mathf.Clamp(x, 1, max1);
          SpriteEditorMenu.s_Setting.gridCellCount.y = (float) Mathf.Clamp(y, 1, max2);
        }
      }
      else
      {
        int x = (int) SpriteEditorMenu.s_Setting.gridSpriteSize.x;
        int y = (int) SpriteEditorMenu.s_Setting.gridSpriteSize.y;
        EditorGUI.BeginChangeCheck();
        this.TwoIntFields(SpriteEditorMenu.s_Styles.pixelSizeLabel, SpriteEditorMenu.s_Styles.xLabel, SpriteEditorMenu.s_Styles.yLabel, ref x, ref y);
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change grid size");
          SpriteEditorMenu.s_Setting.gridSpriteSize.x = (float) Mathf.Clamp(x, 1, max1);
          SpriteEditorMenu.s_Setting.gridSpriteSize.y = (float) Mathf.Clamp(y, 1, max2);
        }
      }
      int x1 = (int) SpriteEditorMenu.s_Setting.gridSpriteOffset.x;
      int y1 = (int) SpriteEditorMenu.s_Setting.gridSpriteOffset.y;
      EditorGUI.BeginChangeCheck();
      this.TwoIntFields(SpriteEditorMenu.s_Styles.offsetLabel, SpriteEditorMenu.s_Styles.xLabel, SpriteEditorMenu.s_Styles.yLabel, ref x1, ref y1);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change grid offset");
        SpriteEditorMenu.s_Setting.gridSpriteOffset.x = Mathf.Clamp((float) x1, 0.0f, (float) max1 - SpriteEditorMenu.s_Setting.gridSpriteSize.x);
        SpriteEditorMenu.s_Setting.gridSpriteOffset.y = Mathf.Clamp((float) y1, 0.0f, (float) max2 - SpriteEditorMenu.s_Setting.gridSpriteSize.y);
      }
      int x2 = (int) SpriteEditorMenu.s_Setting.gridSpritePadding.x;
      int y2 = (int) SpriteEditorMenu.s_Setting.gridSpritePadding.y;
      EditorGUI.BeginChangeCheck();
      this.TwoIntFields(SpriteEditorMenu.s_Styles.paddingLabel, SpriteEditorMenu.s_Styles.xLabel, SpriteEditorMenu.s_Styles.yLabel, ref x2, ref y2);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change grid padding");
        SpriteEditorMenu.s_Setting.gridSpritePadding.x = (float) Mathf.Clamp(x2, 0, max1);
        SpriteEditorMenu.s_Setting.gridSpritePadding.y = (float) Mathf.Clamp(y2, 0, max2);
      }
      this.DoPivotGUI();
      GUILayout.Space(2f);
    }

    private void OnAutomaticGUI()
    {
      float pixels = 38f;
      if (this.m_SelectedTexture != (ITexture2D) null && TextureUtil.IsCompressedTextureFormat(this.m_SelectedTexture.format))
      {
        EditorGUILayout.LabelField(SpriteEditorMenu.s_Styles.automaticSlicingHintLabel, SpriteEditorMenu.s_Styles.notice, new GUILayoutOption[0]);
        pixels -= 31f;
      }
      this.DoPivotGUI();
      EditorGUI.BeginChangeCheck();
      int autoSlicingMethod = SpriteEditorMenu.s_Setting.autoSlicingMethod;
      int num = EditorGUILayout.Popup(SpriteEditorMenu.s_Styles.methodLabel, autoSlicingMethod, SpriteEditorMenu.s_Styles.slicingMethodOptions, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change Slicing Method");
        SpriteEditorMenu.s_Setting.autoSlicingMethod = num;
      }
      GUILayout.Space(pixels);
    }

    private void DoPivotGUI()
    {
      EditorGUI.BeginChangeCheck();
      int spriteAlignment = SpriteEditorMenu.s_Setting.spriteAlignment;
      int num = EditorGUILayout.Popup(SpriteEditorMenu.s_Styles.pivotLabel, spriteAlignment, SpriteEditorMenu.s_Styles.spriteAlignmentOptions, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change Alignment");
        SpriteEditorMenu.s_Setting.spriteAlignment = num;
        SpriteEditorMenu.s_Setting.pivot = SpriteEditorUtility.GetPivotValue((SpriteAlignment) num, SpriteEditorMenu.s_Setting.pivot);
      }
      Vector2 vector2 = SpriteEditorMenu.s_Setting.pivot;
      EditorGUI.BeginChangeCheck();
      using (new EditorGUI.DisabledScope(num != 9))
        vector2 = EditorGUILayout.Vector2Field(SpriteEditorMenu.s_Styles.customPivotLabel, vector2);
      if (!EditorGUI.EndChangeCheck())
        return;
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) SpriteEditorMenu.s_Setting, "Change custom pivot");
      SpriteEditorMenu.s_Setting.pivot = vector2;
    }

    private void DoAutomaticSlicing()
    {
      this.m_SpriteFrameModule.DoAutomaticSlicing(4, SpriteEditorMenu.s_Setting.spriteAlignment, SpriteEditorMenu.s_Setting.pivot, (SpriteFrameModule.AutoSlicingMethod) SpriteEditorMenu.s_Setting.autoSlicingMethod);
    }

    private void DoGridSlicing()
    {
      if (SpriteEditorMenu.s_Setting.slicingType == SpriteEditorMenuSetting.SlicingType.GridByCellCount)
        this.DetemineGridCellSizeWithCellCount();
      this.m_SpriteFrameModule.DoGridSlicing(SpriteEditorMenu.s_Setting.gridSpriteSize, SpriteEditorMenu.s_Setting.gridSpriteOffset, SpriteEditorMenu.s_Setting.gridSpritePadding, SpriteEditorMenu.s_Setting.spriteAlignment, SpriteEditorMenu.s_Setting.pivot);
    }

    private void DetemineGridCellSizeWithCellCount()
    {
      int num1 = !(this.m_PreviewTexture != (ITexture2D) null) ? 4096 : this.m_PreviewTexture.width;
      int num2 = !(this.m_PreviewTexture != (ITexture2D) null) ? 4096 : this.m_PreviewTexture.height;
      SpriteEditorMenu.s_Setting.gridSpriteSize.x = (float) ((double) num1 - (double) SpriteEditorMenu.s_Setting.gridSpriteOffset.x - (double) SpriteEditorMenu.s_Setting.gridSpritePadding.x * (double) SpriteEditorMenu.s_Setting.gridCellCount.x) / SpriteEditorMenu.s_Setting.gridCellCount.x;
      SpriteEditorMenu.s_Setting.gridSpriteSize.y = (float) ((double) num2 - (double) SpriteEditorMenu.s_Setting.gridSpriteOffset.y - (double) SpriteEditorMenu.s_Setting.gridSpritePadding.y * (double) SpriteEditorMenu.s_Setting.gridCellCount.y) / SpriteEditorMenu.s_Setting.gridCellCount.y;
      SpriteEditorMenu.s_Setting.gridSpriteSize.x = Mathf.Clamp(SpriteEditorMenu.s_Setting.gridSpriteSize.x, 1f, (float) num1);
      SpriteEditorMenu.s_Setting.gridSpriteSize.y = Mathf.Clamp(SpriteEditorMenu.s_Setting.gridSpriteSize.y, 1f, (float) num2);
    }

    private class Styles
    {
      public GUIStyle background = (GUIStyle) "grey_border";
      public readonly GUIContent[] spriteAlignmentOptions = new GUIContent[10]{ EditorGUIUtility.TextContent("Center"), EditorGUIUtility.TextContent("Top Left"), EditorGUIUtility.TextContent("Top"), EditorGUIUtility.TextContent("Top Right"), EditorGUIUtility.TextContent("Left"), EditorGUIUtility.TextContent("Right"), EditorGUIUtility.TextContent("Bottom Left"), EditorGUIUtility.TextContent("Bottom"), EditorGUIUtility.TextContent("Bottom Right"), EditorGUIUtility.TextContent("Custom") };
      public readonly GUIContent[] slicingMethodOptions = new GUIContent[3]{ EditorGUIUtility.TextContent("Delete Existing|Delete all existing sprite assets before the slicing operation"), EditorGUIUtility.TextContent("Smart|Try to match existing sprite rects to sliced rects from the slicing operation"), EditorGUIUtility.TextContent("Safe|Keep existing sprite rects intact") };
      public readonly GUIContent methodLabel = EditorGUIUtility.TextContent("Method");
      public readonly GUIContent pivotLabel = EditorGUIUtility.TextContent("Pivot");
      public readonly GUIContent typeLabel = EditorGUIUtility.TextContent("Type");
      public readonly GUIContent sliceButtonLabel = EditorGUIUtility.TextContent("Slice");
      public readonly GUIContent columnAndRowLabel = EditorGUIUtility.TextContent("Column & Row");
      public readonly GUIContent columnLabel = EditorGUIUtility.TextContent("C");
      public readonly GUIContent rowLabel = EditorGUIUtility.TextContent("R");
      public readonly GUIContent pixelSizeLabel = EditorGUIUtility.TextContent("Pixel Size");
      public readonly GUIContent xLabel = EditorGUIUtility.TextContent("X");
      public readonly GUIContent yLabel = EditorGUIUtility.TextContent("Y");
      public readonly GUIContent offsetLabel = EditorGUIUtility.TextContent("Offset");
      public readonly GUIContent paddingLabel = EditorGUIUtility.TextContent("Padding");
      public readonly GUIContent automaticSlicingHintLabel = EditorGUIUtility.TextContent("To obtain more accurate slicing results, manual slicing is recommended!");
      public readonly GUIContent customPivotLabel = EditorGUIUtility.TextContent("Custom Pivot");
      public GUIStyle notice;

      public Styles()
      {
        this.notice = new GUIStyle(GUI.skin.label);
        this.notice.alignment = TextAnchor.MiddleCenter;
        this.notice.wordWrap = true;
      }
    }
  }
}
