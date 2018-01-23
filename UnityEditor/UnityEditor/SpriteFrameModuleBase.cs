// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteFrameModuleBase
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System.Collections.Generic;
using UnityEditor.U2D.Interface;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor
{
  internal abstract class SpriteFrameModuleBase : ISpriteEditorModule
  {
    private float m_Zoom = 1f;
    protected ISpriteRectCache m_RectsCache;
    private static SpriteFrameModuleBase.Styles s_Styles;
    private const float kScrollbarMargin = 16f;
    private const float kInspectorWindowMargin = 8f;
    private const float kInspectorWidth = 330f;
    private const float kInspectorHeight = 160f;
    private SpriteFrameModuleBase.GizmoMode m_GizmoMode;

    protected SpriteFrameModuleBase(string name, ISpriteEditor sw, IEventSystem es, IUndoSystem us, IAssetDatabase ad)
    {
      this.spriteEditor = sw;
      this.eventSystem = es;
      this.undoSystem = us;
      this.assetDatabase = ad;
      this.moduleName = name;
    }

    public abstract bool CanBeActivated();

    public virtual void OnModuleActivate()
    {
      this.spriteImportMode = SpriteUtility.GetSpriteImportMode(this.spriteEditor.spriteEditorDataProvider);
    }

    public abstract void OnModuleDeactivate();

    public string moduleName { get; private set; }

    protected IEventSystem eventSystem { get; private set; }

    protected IUndoSystem undoSystem { get; private set; }

    protected ISpriteEditor spriteEditor { get; private set; }

    protected IAssetDatabase assetDatabase { get; private set; }

    protected SpriteRect selected
    {
      get
      {
        return this.spriteEditor.selectedSpriteRect;
      }
      set
      {
        this.spriteEditor.selectedSpriteRect = value;
      }
    }

    protected SpriteImportMode spriteImportMode { get; private set; }

    protected string spriteAssetPath
    {
      get
      {
        return this.assetDatabase.GetAssetPath((Object) this.spriteEditor.selectedTexture);
      }
    }

    protected ITexture2D previewTexture
    {
      get
      {
        return this.spriteEditor.previewTexture;
      }
    }

    public bool hasSelected
    {
      get
      {
        return this.spriteEditor.selectedSpriteRect != null;
      }
    }

    public SpriteAlignment selectedSpriteAlignment
    {
      get
      {
        return this.selected.alignment;
      }
    }

    public Vector2 selectedSpritePivot
    {
      get
      {
        return this.selected.pivot;
      }
    }

    public int CurrentSelectedSpriteIndex()
    {
      for (int i = 0; i < this.m_RectsCache.Count; ++i)
      {
        if (this.m_RectsCache.RectAt(i) == this.selected)
          return i;
      }
      return -1;
    }

    public Vector4 selectedSpriteBorder
    {
      get
      {
        return SpriteFrameModuleBase.ClampSpriteBorderToRect(this.selected.border, this.selected.rect);
      }
      set
      {
        this.undoSystem.RegisterCompleteObjectUndo((IUndoableObject) this.m_RectsCache, "Change Sprite Border");
        this.spriteEditor.SetDataModified();
        this.selected.border = SpriteFrameModuleBase.ClampSpriteBorderToRect(value, this.selected.rect);
      }
    }

    public Rect selectedSpriteRect
    {
      get
      {
        return this.selected.rect;
      }
      set
      {
        this.undoSystem.RegisterCompleteObjectUndo((IUndoableObject) this.m_RectsCache, "Change Sprite rect");
        this.spriteEditor.SetDataModified();
        this.selected.rect = SpriteFrameModuleBase.ClampSpriteRect(value, (float) this.previewTexture.width, (float) this.previewTexture.height);
      }
    }

    public string selectedSpriteName
    {
      get
      {
        return this.selected.name;
      }
      set
      {
        this.undoSystem.RegisterCompleteObjectUndo((IUndoableObject) this.m_RectsCache, "Change Sprite Name");
        this.spriteEditor.SetDataModified();
        string name = this.selected.name;
        string str = InternalEditorUtility.RemoveInvalidCharsFromFileName(value, true);
        if (string.IsNullOrEmpty(this.selected.originalName) && str != name)
          this.selected.originalName = name;
        if (string.IsNullOrEmpty(str))
          str = name;
        for (int i = 0; i < this.m_RectsCache.Count; ++i)
        {
          if (this.m_RectsCache.RectAt(i).name == str)
          {
            str = this.selected.originalName;
            break;
          }
        }
        this.selected.name = str;
      }
    }

    public int spriteCount
    {
      get
      {
        return this.m_RectsCache.Count;
      }
    }

    public Vector4 GetSpriteBorderAt(int i)
    {
      return this.m_RectsCache.RectAt(i).border;
    }

    public Rect GetSpriteRectAt(int i)
    {
      return this.m_RectsCache.RectAt(i).rect;
    }

    public List<SpriteOutline> GetSpriteOutlineAt(int i)
    {
      return this.m_RectsCache.RectAt(i).outline;
    }

    public void SetSpritePivotAndAlignment(Vector2 pivot, SpriteAlignment alignment)
    {
      this.undoSystem.RegisterCompleteObjectUndo((IUndoableObject) this.m_RectsCache, "Change Sprite Pivot");
      this.spriteEditor.SetDataModified();
      this.selected.alignment = alignment;
      this.selected.pivot = SpriteEditorUtility.GetPivotValue(alignment, pivot);
    }

    public bool containsMultipleSprites
    {
      get
      {
        return this.spriteImportMode == SpriteImportMode.Multiple;
      }
    }

    protected void SnapPivot(Vector2 pivot, out Vector2 outPivot, out SpriteAlignment outAlignment)
    {
      Rect selectedSpriteRect = this.selectedSpriteRect;
      Vector2 vector2 = new Vector2(selectedSpriteRect.xMin + selectedSpriteRect.width * pivot.x, selectedSpriteRect.yMin + selectedSpriteRect.height * pivot.y);
      Vector2[] snapPointsArray = SpriteFrameModuleBase.GetSnapPointsArray(selectedSpriteRect);
      SpriteAlignment spriteAlignment = SpriteAlignment.Custom;
      float num1 = float.MaxValue;
      for (int index = 0; index < snapPointsArray.Length; ++index)
      {
        float num2 = (vector2 - snapPointsArray[index]).magnitude * this.m_Zoom;
        if ((double) num2 < (double) num1)
        {
          spriteAlignment = (SpriteAlignment) index;
          num1 = num2;
        }
      }
      outAlignment = spriteAlignment;
      outPivot = SpriteFrameModuleBase.ConvertFromTextureToNormalizedSpace(snapPointsArray[(int) spriteAlignment], selectedSpriteRect);
    }

    protected static Rect ClampSpriteRect(Rect rect, float maxX, float maxY)
    {
      Rect rect1 = new Rect();
      rect1.xMin = Mathf.Clamp(rect.xMin, 0.0f, maxX - 1f);
      rect1.yMin = Mathf.Clamp(rect.yMin, 0.0f, maxY - 1f);
      rect1.xMax = Mathf.Clamp(rect.xMax, 1f, maxX);
      rect1.yMax = Mathf.Clamp(rect.yMax, 1f, maxY);
      if (Mathf.RoundToInt(rect1.width) == 0)
        rect1.width = 1f;
      if (Mathf.RoundToInt(rect1.height) == 0)
        rect1.height = 1f;
      return SpriteEditorUtility.RoundedRect(rect1);
    }

    protected static Rect FlipNegativeRect(Rect rect)
    {
      return new Rect() { xMin = Mathf.Min(rect.xMin, rect.xMax), yMin = Mathf.Min(rect.yMin, rect.yMax), xMax = Mathf.Max(rect.xMin, rect.xMax), yMax = Mathf.Max(rect.yMin, rect.yMax) };
    }

    protected static Vector4 ClampSpriteBorderToRect(Vector4 border, Rect rect)
    {
      Rect rect1 = SpriteFrameModuleBase.FlipNegativeRect(rect);
      float width = rect1.width;
      float height = rect1.height;
      Vector4 vector4 = new Vector4();
      vector4.x = (float) Mathf.RoundToInt(Mathf.Clamp(border.x, 0.0f, Mathf.Min(Mathf.Abs(width - border.z), width)));
      vector4.z = (float) Mathf.RoundToInt(Mathf.Clamp(border.z, 0.0f, Mathf.Min(Mathf.Abs(width - vector4.x), width)));
      vector4.y = (float) Mathf.RoundToInt(Mathf.Clamp(border.y, 0.0f, Mathf.Min(Mathf.Abs(height - border.w), height)));
      vector4.w = (float) Mathf.RoundToInt(Mathf.Clamp(border.w, 0.0f, Mathf.Min(Mathf.Abs(height - vector4.y), height)));
      return vector4;
    }

    protected static SpriteFrameModuleBase.Styles styles
    {
      get
      {
        if (SpriteFrameModuleBase.s_Styles == null)
          SpriteFrameModuleBase.s_Styles = new SpriteFrameModuleBase.Styles();
        return SpriteFrameModuleBase.s_Styles;
      }
    }

    private bool ShouldShowRectScaling()
    {
      return this.hasSelected && this.m_GizmoMode == SpriteFrameModuleBase.GizmoMode.RectEditing;
    }

    private void DoPivotFields()
    {
      EditorGUI.BeginChangeCheck();
      SpriteAlignment alignment = (SpriteAlignment) EditorGUILayout.Popup(SpriteFrameModuleBase.styles.pivotLabel, (int) this.selectedSpriteAlignment, SpriteFrameModuleBase.styles.spriteAlignmentOptions, new GUILayoutOption[0]);
      Vector2 selectedSpritePivot = this.selectedSpritePivot;
      Vector2 pivot = selectedSpritePivot;
      using (new EditorGUI.DisabledScope(alignment != SpriteAlignment.Custom))
      {
        Rect rect = GUILayoutUtility.GetRect(322f, EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2, SpriteFrameModuleBase.styles.customPivotLabel));
        GUI.SetNextControlName("PivotField");
        pivot = EditorGUI.Vector2Field(rect, SpriteFrameModuleBase.styles.customPivotLabel, selectedSpritePivot);
      }
      if (!EditorGUI.EndChangeCheck())
        return;
      this.SetSpritePivotAndAlignment(pivot, alignment);
    }

    private void DoBorderFields()
    {
      EditorGUI.BeginChangeCheck();
      Vector4 selectedSpriteBorder = this.selectedSpriteBorder;
      int x = Mathf.RoundToInt(selectedSpriteBorder.x);
      int w = Mathf.RoundToInt(selectedSpriteBorder.y);
      int z = Mathf.RoundToInt(selectedSpriteBorder.z);
      int y = Mathf.RoundToInt(selectedSpriteBorder.w);
      SpriteEditorUtility.FourIntFields(new Vector2(322f, 32f), SpriteFrameModuleBase.styles.borderLabel, SpriteFrameModuleBase.styles.lLabel, SpriteFrameModuleBase.styles.tLabel, SpriteFrameModuleBase.styles.rLabel, SpriteFrameModuleBase.styles.bLabel, ref x, ref y, ref z, ref w);
      Vector4 vector4 = new Vector4((float) x, (float) w, (float) z, (float) y);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.selectedSpriteBorder = vector4;
    }

    private void DoPositionField()
    {
      EditorGUI.BeginChangeCheck();
      Rect selectedSpriteRect = this.selectedSpriteRect;
      int x = Mathf.RoundToInt(selectedSpriteRect.x);
      int y = Mathf.RoundToInt(selectedSpriteRect.y);
      int z = Mathf.RoundToInt(selectedSpriteRect.width);
      int w = Mathf.RoundToInt(selectedSpriteRect.height);
      SpriteEditorUtility.FourIntFields(new Vector2(322f, 32f), SpriteFrameModuleBase.styles.positionLabel, SpriteFrameModuleBase.styles.xLabel, SpriteFrameModuleBase.styles.yLabel, SpriteFrameModuleBase.styles.wLabel, SpriteFrameModuleBase.styles.hLabel, ref x, ref y, ref z, ref w);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.selectedSpriteRect = new Rect((float) x, (float) y, (float) z, (float) w);
    }

    private void DoNameField()
    {
      EditorGUI.BeginChangeCheck();
      string selectedSpriteName = this.selectedSpriteName;
      GUI.SetNextControlName("SpriteName");
      string str = EditorGUILayout.TextField(SpriteFrameModuleBase.styles.nameLabel, selectedSpriteName, new GUILayoutOption[0]);
      if (!EditorGUI.EndChangeCheck())
        return;
      this.selectedSpriteName = str;
    }

    private Rect inspectorRect
    {
      get
      {
        Rect windowDimension = this.spriteEditor.windowDimension;
        return new Rect((float) ((double) windowDimension.width - 330.0 - 8.0 - 16.0), (float) ((double) windowDimension.height - 160.0 - 8.0 - 16.0), 330f, 160f);
      }
    }

    private void DoSelectedFrameInspector()
    {
      if (!this.hasSelected)
        return;
      EditorGUIUtility.wideMode = true;
      float labelWidth = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = 135f;
      GUILayout.BeginArea(this.inspectorRect);
      GUILayout.BeginVertical(SpriteFrameModuleBase.styles.spriteLabel, GUI.skin.window, new GUILayoutOption[0]);
      using (new EditorGUI.DisabledScope(!this.containsMultipleSprites))
      {
        this.DoNameField();
        this.DoPositionField();
      }
      this.DoBorderFields();
      this.DoPivotFields();
      GUILayout.EndVertical();
      GUILayout.EndArea();
      EditorGUIUtility.labelWidth = labelWidth;
    }

    private static Vector2 ApplySpriteAlignmentToPivot(Vector2 pivot, Rect rect, SpriteAlignment alignment)
    {
      if (alignment != SpriteAlignment.Custom)
        return SpriteFrameModuleBase.ConvertFromTextureToNormalizedSpace(SpriteFrameModuleBase.GetSnapPointsArray(rect)[(int) alignment], rect);
      return pivot;
    }

    private static Vector2 ConvertFromTextureToNormalizedSpace(Vector2 texturePos, Rect rect)
    {
      return new Vector2((texturePos.x - rect.xMin) / rect.width, (texturePos.y - rect.yMin) / rect.height);
    }

    private static Vector2[] GetSnapPointsArray(Rect rect)
    {
      Vector2[] vector2Array = new Vector2[9];
      vector2Array[1] = new Vector2(rect.xMin, rect.yMax);
      vector2Array[2] = new Vector2(rect.center.x, rect.yMax);
      vector2Array[3] = new Vector2(rect.xMax, rect.yMax);
      vector2Array[4] = new Vector2(rect.xMin, rect.center.y);
      vector2Array[0] = new Vector2(rect.center.x, rect.center.y);
      vector2Array[5] = new Vector2(rect.xMax, rect.center.y);
      vector2Array[6] = new Vector2(rect.xMin, rect.yMin);
      vector2Array[7] = new Vector2(rect.center.x, rect.yMin);
      vector2Array[8] = new Vector2(rect.xMax, rect.yMin);
      return vector2Array;
    }

    protected void Repaint()
    {
      this.spriteEditor.RequestRepaint();
    }

    protected void HandleGizmoMode()
    {
      SpriteFrameModuleBase.GizmoMode gizmoMode = this.m_GizmoMode;
      IEvent current = this.eventSystem.current;
      this.m_GizmoMode = !current.control ? SpriteFrameModuleBase.GizmoMode.RectEditing : SpriteFrameModuleBase.GizmoMode.BorderEditing;
      if (gizmoMode == this.m_GizmoMode || current.type != EventType.KeyDown && current.type != EventType.KeyUp || current.keyCode != KeyCode.LeftControl && current.keyCode != KeyCode.RightControl && (current.keyCode != KeyCode.LeftAlt && current.keyCode != KeyCode.RightAlt))
        return;
      this.Repaint();
    }

    protected bool MouseOnTopOfInspector()
    {
      if (!this.hasSelected)
        return false;
      return this.inspectorRect.Contains(GUIClip.Unclip(this.eventSystem.current.mousePosition) - (GUIClip.topmostRect.position - GUIClip.GetTopRect().position));
    }

    protected void HandlePivotHandle()
    {
      if (!this.hasSelected)
        return;
      EditorGUI.BeginChangeCheck();
      SpriteAlignment outAlignment = this.selectedSpriteAlignment;
      Vector2 selectedSpritePivot = this.selectedSpritePivot;
      Rect selectedSpriteRect = this.selectedSpriteRect;
      Vector2 outPivot = SpriteFrameModuleBase.ApplySpriteAlignmentToPivot(selectedSpritePivot, selectedSpriteRect, outAlignment);
      Vector2 pivot = SpriteEditorHandles.PivotSlider(selectedSpriteRect, outPivot, SpriteFrameModuleBase.styles.pivotdot, SpriteFrameModuleBase.styles.pivotdotactive);
      if (!EditorGUI.EndChangeCheck())
        return;
      if (this.eventSystem.current.control)
      {
        this.SnapPivot(pivot, out outPivot, out outAlignment);
      }
      else
      {
        outPivot = pivot;
        outAlignment = SpriteAlignment.Custom;
      }
      this.SetSpritePivotAndAlignment(outPivot, outAlignment);
    }

    protected void HandleBorderSidePointScalingSliders()
    {
      if (!this.hasSelected)
        return;
      GUIStyle dragBorderdot = SpriteFrameModuleBase.styles.dragBorderdot;
      GUIStyle dragBorderDotActive = SpriteFrameModuleBase.styles.dragBorderDotActive;
      Color color = new Color(0.0f, 1f, 0.0f);
      Rect selectedSpriteRect = this.selectedSpriteRect;
      Vector4 selectedSpriteBorder = this.selectedSpriteBorder;
      float x1 = selectedSpriteRect.xMin + selectedSpriteBorder.x;
      float x2 = selectedSpriteRect.xMax - selectedSpriteBorder.z;
      float y1 = selectedSpriteRect.yMax - selectedSpriteBorder.w;
      float y2 = selectedSpriteRect.yMin + selectedSpriteBorder.y;
      EditorGUI.BeginChangeCheck();
      float num1 = y2 - (float) (((double) y2 - (double) y1) / 2.0);
      float num2 = x1 - (float) (((double) x1 - (double) x2) / 2.0);
      float num3 = num1;
      this.HandleBorderPointSlider(ref x1, ref num3, MouseCursor.ResizeHorizontal, false, dragBorderdot, dragBorderDotActive, color);
      num3 = num1;
      this.HandleBorderPointSlider(ref x2, ref num3, MouseCursor.ResizeHorizontal, false, dragBorderdot, dragBorderDotActive, color);
      num3 = num2;
      this.HandleBorderPointSlider(ref num3, ref y1, MouseCursor.ResizeVertical, false, dragBorderdot, dragBorderDotActive, color);
      num3 = num2;
      this.HandleBorderPointSlider(ref num3, ref y2, MouseCursor.ResizeVertical, false, dragBorderdot, dragBorderDotActive, color);
      if (!EditorGUI.EndChangeCheck())
        return;
      selectedSpriteBorder.x = x1 - selectedSpriteRect.xMin;
      selectedSpriteBorder.z = selectedSpriteRect.xMax - x2;
      selectedSpriteBorder.w = selectedSpriteRect.yMax - y1;
      selectedSpriteBorder.y = y2 - selectedSpriteRect.yMin;
      this.selectedSpriteBorder = selectedSpriteBorder;
    }

    protected void HandleBorderCornerScalingHandles()
    {
      if (!this.hasSelected)
        return;
      GUIStyle dragBorderdot = SpriteFrameModuleBase.styles.dragBorderdot;
      GUIStyle dragBorderDotActive = SpriteFrameModuleBase.styles.dragBorderDotActive;
      Color color = new Color(0.0f, 1f, 0.0f);
      Rect selectedSpriteRect = this.selectedSpriteRect;
      Vector4 selectedSpriteBorder = this.selectedSpriteBorder;
      float x1 = selectedSpriteRect.xMin + selectedSpriteBorder.x;
      float x2 = selectedSpriteRect.xMax - selectedSpriteBorder.z;
      float y1 = selectedSpriteRect.yMax - selectedSpriteBorder.w;
      float y2 = selectedSpriteRect.yMin + selectedSpriteBorder.y;
      EditorGUI.BeginChangeCheck();
      this.HandleBorderPointSlider(ref x1, ref y1, MouseCursor.ResizeUpLeft, (double) selectedSpriteBorder.x < 1.0 && (double) selectedSpriteBorder.w < 1.0, dragBorderdot, dragBorderDotActive, color);
      this.HandleBorderPointSlider(ref x2, ref y1, MouseCursor.ResizeUpRight, (double) selectedSpriteBorder.z < 1.0 && (double) selectedSpriteBorder.w < 1.0, dragBorderdot, dragBorderDotActive, color);
      this.HandleBorderPointSlider(ref x1, ref y2, MouseCursor.ResizeUpRight, (double) selectedSpriteBorder.x < 1.0 && (double) selectedSpriteBorder.y < 1.0, dragBorderdot, dragBorderDotActive, color);
      this.HandleBorderPointSlider(ref x2, ref y2, MouseCursor.ResizeUpLeft, (double) selectedSpriteBorder.z < 1.0 && (double) selectedSpriteBorder.y < 1.0, dragBorderdot, dragBorderDotActive, color);
      if (!EditorGUI.EndChangeCheck())
        return;
      selectedSpriteBorder.x = x1 - selectedSpriteRect.xMin;
      selectedSpriteBorder.z = selectedSpriteRect.xMax - x2;
      selectedSpriteBorder.w = selectedSpriteRect.yMax - y1;
      selectedSpriteBorder.y = y2 - selectedSpriteRect.yMin;
      this.selectedSpriteBorder = selectedSpriteBorder;
    }

    protected void HandleBorderSideScalingHandles()
    {
      if (!this.hasSelected)
        return;
      Rect rect = new Rect(this.selectedSpriteRect);
      Vector4 selectedSpriteBorder = this.selectedSpriteBorder;
      float x1 = rect.xMin + selectedSpriteBorder.x;
      float x2 = rect.xMax - selectedSpriteBorder.z;
      float y1 = rect.yMax - selectedSpriteBorder.w;
      float y2 = rect.yMin + selectedSpriteBorder.y;
      Vector2 vector2_1 = (Vector2) Handles.matrix.MultiplyPoint(new Vector3(rect.xMin, rect.yMin));
      Vector2 vector2_2 = (Vector2) Handles.matrix.MultiplyPoint(new Vector3(rect.xMax, rect.yMax));
      float width = Mathf.Abs(vector2_2.x - vector2_1.x);
      float height = Mathf.Abs(vector2_2.y - vector2_1.y);
      EditorGUI.BeginChangeCheck();
      float num1 = this.HandleBorderScaleSlider(x1, rect.yMax, width, height, true);
      float num2 = this.HandleBorderScaleSlider(x2, rect.yMax, width, height, true);
      float num3 = this.HandleBorderScaleSlider(rect.xMin, y1, width, height, false);
      float num4 = this.HandleBorderScaleSlider(rect.xMin, y2, width, height, false);
      if (!EditorGUI.EndChangeCheck())
        return;
      selectedSpriteBorder.x = num1 - rect.xMin;
      selectedSpriteBorder.z = rect.xMax - num2;
      selectedSpriteBorder.w = rect.yMax - num3;
      selectedSpriteBorder.y = num4 - rect.yMin;
      this.selectedSpriteBorder = selectedSpriteBorder;
    }

    protected void HandleBorderPointSlider(ref float x, ref float y, MouseCursor mouseCursor, bool isHidden, GUIStyle dragDot, GUIStyle dragDotActive, Color color)
    {
      Color color1 = GUI.color;
      GUI.color = !isHidden ? color : new Color(0.0f, 0.0f, 0.0f, 0.0f);
      Vector2 vector2 = SpriteEditorHandles.PointSlider(new Vector2(x, y), mouseCursor, dragDot, dragDotActive);
      x = vector2.x;
      y = vector2.y;
      GUI.color = color1;
    }

    protected float HandleBorderScaleSlider(float x, float y, float width, float height, bool isHorizontal)
    {
      float fixedWidth = SpriteFrameModuleBase.styles.dragBorderdot.fixedWidth;
      Vector2 pos = (Vector2) Handles.matrix.MultiplyPoint((Vector3) new Vector2(x, y));
      EditorGUI.BeginChangeCheck();
      float num;
      if (isHorizontal)
      {
        Rect cursorRect = new Rect(pos.x - fixedWidth * 0.5f, pos.y, fixedWidth, height);
        num = SpriteEditorHandles.ScaleSlider(pos, MouseCursor.ResizeHorizontal, cursorRect).x;
      }
      else
      {
        Rect cursorRect = new Rect(pos.x, pos.y - fixedWidth * 0.5f, width, fixedWidth);
        num = SpriteEditorHandles.ScaleSlider(pos, MouseCursor.ResizeVertical, cursorRect).y;
      }
      if (EditorGUI.EndChangeCheck())
        return num;
      return !isHorizontal ? y : x;
    }

    protected void DrawSpriteRectGizmos()
    {
      if (this.eventSystem.current.type != EventType.Repaint)
        return;
      SpriteEditorUtility.BeginLines(new Color(0.0f, 1f, 0.0f, 0.7f));
      int num = this.CurrentSelectedSpriteIndex();
      for (int i = 0; i < this.spriteCount; ++i)
      {
        Vector4 spriteBorderAt = this.GetSpriteBorderAt(i);
        if (num == i || this.m_GizmoMode == SpriteFrameModuleBase.GizmoMode.BorderEditing || !Mathf.Approximately(spriteBorderAt.sqrMagnitude, 0.0f))
        {
          Rect spriteRectAt = this.GetSpriteRectAt(i);
          SpriteEditorUtility.DrawLine(new Vector3(spriteRectAt.xMin + spriteBorderAt.x, spriteRectAt.yMin), new Vector3(spriteRectAt.xMin + spriteBorderAt.x, spriteRectAt.yMax));
          SpriteEditorUtility.DrawLine(new Vector3(spriteRectAt.xMax - spriteBorderAt.z, spriteRectAt.yMin), new Vector3(spriteRectAt.xMax - spriteBorderAt.z, spriteRectAt.yMax));
          SpriteEditorUtility.DrawLine(new Vector3(spriteRectAt.xMin, spriteRectAt.yMin + spriteBorderAt.y), new Vector3(spriteRectAt.xMax, spriteRectAt.yMin + spriteBorderAt.y));
          SpriteEditorUtility.DrawLine(new Vector3(spriteRectAt.xMin, spriteRectAt.yMax - spriteBorderAt.w), new Vector3(spriteRectAt.xMax, spriteRectAt.yMax - spriteBorderAt.w));
        }
      }
      SpriteEditorUtility.EndLines();
      if (!this.ShouldShowRectScaling())
        return;
      Rect selectedSpriteRect = this.selectedSpriteRect;
      SpriteEditorUtility.BeginLines(new Color(0.0f, 0.1f, 0.3f, 0.25f));
      SpriteEditorUtility.DrawBox(new Rect(selectedSpriteRect.xMin + 1f / this.m_Zoom, selectedSpriteRect.yMin + 1f / this.m_Zoom, selectedSpriteRect.width, selectedSpriteRect.height));
      SpriteEditorUtility.EndLines();
      SpriteEditorUtility.BeginLines(new Color(0.25f, 0.5f, 1f, 0.75f));
      SpriteEditorUtility.DrawBox(selectedSpriteRect);
      SpriteEditorUtility.EndLines();
    }

    public virtual void DoTextureGUI()
    {
      this.m_Zoom = Handles.matrix.GetColumn(0).magnitude;
    }

    public virtual void OnPostGUI()
    {
      this.DoSelectedFrameInspector();
    }

    public abstract void DrawToolbarGUI(Rect drawArea);

    protected enum GizmoMode
    {
      BorderEditing,
      RectEditing,
    }

    protected class Styles
    {
      public readonly GUIStyle dragdot = (GUIStyle) "U2D.dragDot";
      public readonly GUIStyle dragdotactive = (GUIStyle) "U2D.dragDotActive";
      public readonly GUIStyle createRect = (GUIStyle) "U2D.createRect";
      public readonly GUIStyle pivotdotactive = (GUIStyle) "U2D.pivotDotActive";
      public readonly GUIStyle pivotdot = (GUIStyle) "U2D.pivotDot";
      public readonly GUIStyle dragBorderdot = new GUIStyle();
      public readonly GUIStyle dragBorderDotActive = new GUIStyle();
      public readonly GUIContent[] spriteAlignmentOptions = new GUIContent[10]{ EditorGUIUtility.TextContent("Center"), EditorGUIUtility.TextContent("Top Left"), EditorGUIUtility.TextContent("Top"), EditorGUIUtility.TextContent("Top Right"), EditorGUIUtility.TextContent("Left"), EditorGUIUtility.TextContent("Right"), EditorGUIUtility.TextContent("Bottom Left"), EditorGUIUtility.TextContent("Bottom"), EditorGUIUtility.TextContent("Bottom Right"), EditorGUIUtility.TextContent("Custom") };
      public readonly GUIContent pivotLabel = EditorGUIUtility.TextContent("Pivot");
      public readonly GUIContent spriteLabel = EditorGUIUtility.TextContent("Sprite");
      public readonly GUIContent customPivotLabel = EditorGUIUtility.TextContent("Custom Pivot");
      public readonly GUIContent borderLabel = EditorGUIUtility.TextContent("Border");
      public readonly GUIContent lLabel = EditorGUIUtility.TextContent("L");
      public readonly GUIContent tLabel = EditorGUIUtility.TextContent("T");
      public readonly GUIContent rLabel = EditorGUIUtility.TextContent("R");
      public readonly GUIContent bLabel = EditorGUIUtility.TextContent("B");
      public readonly GUIContent positionLabel = EditorGUIUtility.TextContent("Position");
      public readonly GUIContent xLabel = EditorGUIUtility.TextContent("X");
      public readonly GUIContent yLabel = EditorGUIUtility.TextContent("Y");
      public readonly GUIContent wLabel = EditorGUIUtility.TextContent("W");
      public readonly GUIContent hLabel = EditorGUIUtility.TextContent("H");
      public readonly GUIContent nameLabel = EditorGUIUtility.TextContent("Name");
      public readonly GUIStyle toolbar;

      public Styles()
      {
        this.toolbar = new GUIStyle(EditorStyles.inspectorBig);
        this.toolbar.margin.top = 0;
        this.toolbar.margin.bottom = 0;
        this.createRect.border = new RectOffset(3, 3, 3, 3);
        this.dragBorderdot.fixedHeight = 5f;
        this.dragBorderdot.fixedWidth = 5f;
        this.dragBorderdot.normal.background = EditorGUIUtility.whiteTexture;
        this.dragBorderDotActive.fixedHeight = this.dragBorderdot.fixedHeight;
        this.dragBorderDotActive.fixedWidth = this.dragBorderdot.fixedWidth;
        this.dragBorderDotActive.normal.background = EditorGUIUtility.whiteTexture;
      }
    }
  }
}
