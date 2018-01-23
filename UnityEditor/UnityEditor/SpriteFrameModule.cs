// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteFrameModule
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.U2D.Interface;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor
{
  internal class SpriteFrameModule : SpriteFrameModuleBase
  {
    internal static PrefKey k_SpriteEditorTrim = new PrefKey("Sprite Editor/Trim", "#t");
    private bool[] m_AlphaPixelCache;
    private const int kDefaultColliderAlphaCutoff = 254;
    private const float kDefaultColliderDetail = 0.25f;

    public SpriteFrameModule(ISpriteEditor sw, IEventSystem es, IUndoSystem us, IAssetDatabase ad)
      : base("Sprite Editor", sw, es, us, ad)
    {
    }

    public override void OnModuleActivate()
    {
      base.OnModuleActivate();
      this.m_RectsCache = this.spriteEditor.spriteRects;
      this.spriteEditor.enableMouseMoveEvent = true;
    }

    public override void OnModuleDeactivate()
    {
      this.m_RectsCache = (ISpriteRectCache) null;
    }

    public override bool CanBeActivated()
    {
      return SpriteUtility.GetSpriteImportMode(this.spriteEditor.spriteEditorDataProvider) != SpriteImportMode.Polygon;
    }

    private string GetUniqueName(string prefix, int startIndex)
    {
      string str;
      bool flag;
      do
      {
        str = prefix + "_" + (object) startIndex++;
        flag = false;
        for (int i = 0; i < this.m_RectsCache.Count; ++i)
        {
          if (this.m_RectsCache.RectAt(i).name == str)
          {
            flag = true;
            break;
          }
        }
      }
      while (flag);
      return str;
    }

    private List<Rect> SortRects(List<Rect> rects)
    {
      List<Rect> rectList1 = new List<Rect>();
      while (rects.Count > 0)
      {
        Rect rect = rects[rects.Count - 1];
        Rect sweepRect = new Rect(0.0f, rect.yMin, (float) this.previewTexture.width, rect.height);
        List<Rect> rectList2 = this.RectSweep(rects, sweepRect);
        if (rectList2.Count > 0)
        {
          rectList1.AddRange((IEnumerable<Rect>) rectList2);
        }
        else
        {
          rectList1.AddRange((IEnumerable<Rect>) rects);
          break;
        }
      }
      return rectList1;
    }

    private List<Rect> RectSweep(List<Rect> rects, Rect sweepRect)
    {
      if (rects == null || rects.Count == 0)
        return new List<Rect>();
      List<Rect> rectList = new List<Rect>();
      foreach (Rect rect in rects)
      {
        if (rect.Overlaps(sweepRect))
          rectList.Add(rect);
      }
      foreach (Rect rect in rectList)
        rects.Remove(rect);
      rectList.Sort((Comparison<Rect>) ((a, b) => a.x.CompareTo(b.x)));
      return rectList;
    }

    private void AddSprite(Rect frame, int alignment, Vector2 pivot, SpriteFrameModule.AutoSlicingMethod slicingMethod, ref int index)
    {
      if (slicingMethod != SpriteFrameModule.AutoSlicingMethod.DeleteAll)
      {
        SpriteRect overlappingSprite = this.GetExistingOverlappingSprite(frame);
        if (overlappingSprite != null)
        {
          if (slicingMethod != SpriteFrameModule.AutoSlicingMethod.Smart)
            return;
          overlappingSprite.rect = frame;
          overlappingSprite.alignment = (SpriteAlignment) alignment;
          overlappingSprite.pivot = pivot;
        }
        else
        {
          Rect rect = frame;
          int alignment1 = alignment;
          Vector2 pivot1 = pivot;
          int colliderAlphaCutoff = 254;
          double num1 = 0.25;
          int num2;
          index = (num2 = index) + 1;
          int nameIndexingHint = num2;
          this.AddSpriteWithUniqueName(rect, alignment1, pivot1, colliderAlphaCutoff, (float) num1, nameIndexingHint);
        }
      }
      else
      {
        Rect rect = frame;
        int alignment1 = alignment;
        Vector2 pivot1 = pivot;
        int colliderAlphaCutoff = 254;
        double num1 = 0.25;
        string spriteNamePrefix = this.GetSpriteNamePrefix();
        string str = "_";
        int num2;
        index = (num2 = index) + 1;
        // ISSUE: variable of a boxed type
        __Boxed<int> local = (ValueType) num2;
        string name = spriteNamePrefix + str + (object) local;
        this.AddSprite(rect, alignment1, pivot1, colliderAlphaCutoff, (float) num1, name);
      }
    }

    private SpriteRect GetExistingOverlappingSprite(Rect rect)
    {
      for (int i = 0; i < this.m_RectsCache.Count; ++i)
      {
        if (this.m_RectsCache.RectAt(i).rect.Overlaps(rect))
          return this.m_RectsCache.RectAt(i);
      }
      return (SpriteRect) null;
    }

    private bool PixelHasAlpha(int x, int y, ITexture2D texture)
    {
      if (this.m_AlphaPixelCache == null)
      {
        this.m_AlphaPixelCache = new bool[texture.width * texture.height];
        Color32[] pixels32 = texture.GetPixels32();
        for (int index = 0; index < pixels32.Length; ++index)
          this.m_AlphaPixelCache[index] = (int) pixels32[index].a != 0;
      }
      return this.m_AlphaPixelCache[y * texture.width + x];
    }

    private SpriteRect AddSprite(Rect rect, int alignment, Vector2 pivot, int colliderAlphaCutoff, float colliderDetail, string name)
    {
      SpriteRect r = new SpriteRect();
      r.rect = rect;
      r.alignment = (SpriteAlignment) alignment;
      r.pivot = pivot;
      r.name = name;
      r.originalName = r.name;
      r.border = Vector4.zero;
      this.spriteEditor.SetDataModified();
      this.m_RectsCache.AddRect(r);
      this.spriteEditor.SetDataModified();
      return r;
    }

    public SpriteRect AddSpriteWithUniqueName(Rect rect, int alignment, Vector2 pivot, int colliderAlphaCutoff, float colliderDetail, int nameIndexingHint)
    {
      string uniqueName = this.GetUniqueName(this.GetSpriteNamePrefix(), nameIndexingHint);
      return this.AddSprite(rect, alignment, pivot, colliderAlphaCutoff, colliderDetail, uniqueName);
    }

    private string GetSpriteNamePrefix()
    {
      return Path.GetFileNameWithoutExtension(this.spriteAssetPath);
    }

    public void DoAutomaticSlicing(int minimumSpriteSize, int alignment, Vector2 pivot, SpriteFrameModule.AutoSlicingMethod slicingMethod)
    {
      this.undoSystem.RegisterCompleteObjectUndo((IUndoableObject) this.m_RectsCache, "Automatic Slicing");
      if (slicingMethod == SpriteFrameModule.AutoSlicingMethod.DeleteAll)
        this.m_RectsCache.ClearAll();
      List<Rect> rectList = this.SortRects(new List<Rect>((IEnumerable<Rect>) InternalSpriteUtility.GenerateAutomaticSpriteRectangles((UnityEngine.Texture2D) this.spriteEditor.GetReadableTexture2D(), minimumSpriteSize, 0)));
      int index = 0;
      foreach (Rect frame in rectList)
        this.AddSprite(frame, alignment, pivot, slicingMethod, ref index);
      this.selected = (SpriteRect) null;
      this.spriteEditor.SetDataModified();
      this.Repaint();
    }

    public void DoGridSlicing(Vector2 size, Vector2 offset, Vector2 padding, int alignment, Vector2 pivot)
    {
      Rect[] spriteRectangles = InternalSpriteUtility.GenerateGridSpriteRectangles((UnityEngine.Texture2D) this.spriteEditor.GetReadableTexture2D(), offset, size, padding);
      int num = 0;
      this.undoSystem.RegisterCompleteObjectUndo((IUndoableObject) this.m_RectsCache, "Grid Slicing");
      this.m_RectsCache.ClearAll();
      foreach (Rect rect in spriteRectangles)
        this.AddSprite(rect, alignment, pivot, 254, 0.25f, this.GetSpriteNamePrefix() + "_" + (object) num++);
      this.selected = (SpriteRect) null;
      this.spriteEditor.SetDataModified();
      this.Repaint();
    }

    public void ScaleSpriteRect(Rect r)
    {
      if (this.selected == null)
        return;
      this.undoSystem.RegisterCompleteObjectUndo((IUndoableObject) this.m_RectsCache, "Scale sprite");
      this.selected.rect = SpriteFrameModuleBase.ClampSpriteRect(r, (float) this.previewTexture.width, (float) this.previewTexture.height);
      this.selected.border = SpriteFrameModuleBase.ClampSpriteBorderToRect(this.selected.border, this.selected.rect);
      this.spriteEditor.SetDataModified();
    }

    public void TrimAlpha()
    {
      ITexture2D readableTexture2D = this.spriteEditor.GetReadableTexture2D();
      if (readableTexture2D == (ITexture2D) null)
        return;
      Rect rect = this.selected.rect;
      int a1 = (int) rect.xMax;
      int a2 = (int) rect.xMin;
      int a3 = (int) rect.yMax;
      int a4 = (int) rect.yMin;
      for (int yMin = (int) rect.yMin; yMin < (int) rect.yMax; ++yMin)
      {
        for (int xMin = (int) rect.xMin; xMin < (int) rect.xMax; ++xMin)
        {
          if (this.PixelHasAlpha(xMin, yMin, readableTexture2D))
          {
            a1 = Mathf.Min(a1, xMin);
            a2 = Mathf.Max(a2, xMin);
            a3 = Mathf.Min(a3, yMin);
            a4 = Mathf.Max(a4, yMin);
          }
        }
      }
      rect = a1 > a2 || a3 > a4 ? new Rect(0.0f, 0.0f, 0.0f, 0.0f) : new Rect((float) a1, (float) a3, (float) (a2 - a1 + 1), (float) (a4 - a3 + 1));
      if ((double) rect.width <= 0.0 && (double) rect.height <= 0.0)
      {
        this.m_RectsCache.RemoveRect(this.selected);
        this.spriteEditor.SetDataModified();
        this.selected = (SpriteRect) null;
      }
      else
      {
        rect = SpriteFrameModuleBase.ClampSpriteRect(rect, (float) readableTexture2D.width, (float) readableTexture2D.height);
        if (this.selected.rect != rect)
          this.spriteEditor.SetDataModified();
        this.selected.rect = rect;
      }
    }

    public void DuplicateSprite()
    {
      if (this.selected == null)
        return;
      this.undoSystem.RegisterCompleteObjectUndo((IUndoableObject) this.m_RectsCache, "Duplicate sprite");
      this.selected = this.AddSpriteWithUniqueName(this.selected.rect, (int) this.selected.alignment, this.selected.pivot, 254, 0.25f, 0);
    }

    public void CreateSprite(Rect rect)
    {
      rect = SpriteFrameModuleBase.ClampSpriteRect(rect, (float) this.previewTexture.width, (float) this.previewTexture.height);
      this.undoSystem.RegisterCompleteObjectUndo((IUndoableObject) this.m_RectsCache, "Create sprite");
      this.selected = this.AddSpriteWithUniqueName(rect, 0, Vector2.zero, 254, 0.25f, 0);
    }

    public void DeleteSprite()
    {
      if (this.selected == null)
        return;
      this.undoSystem.RegisterCompleteObjectUndo((IUndoableObject) this.m_RectsCache, "Delete sprite");
      this.m_RectsCache.RemoveRect(this.selected);
      this.selected = (SpriteRect) null;
      this.spriteEditor.SetDataModified();
    }

    public override void DoTextureGUI()
    {
      base.DoTextureGUI();
      this.DrawSpriteRectGizmos();
      this.HandleGizmoMode();
      if (this.containsMultipleSprites)
        this.HandleRectCornerScalingHandles();
      this.HandleBorderCornerScalingHandles();
      this.HandleBorderSidePointScalingSliders();
      if (this.containsMultipleSprites)
        this.HandleRectSideScalingHandles();
      this.HandleBorderSideScalingHandles();
      this.HandlePivotHandle();
      if (this.containsMultipleSprites)
        this.HandleDragging();
      if (!this.MouseOnTopOfInspector())
        this.spriteEditor.HandleSpriteSelection();
      if (!this.containsMultipleSprites)
        return;
      this.HandleCreate();
      this.HandleDelete();
      this.HandleDuplicate();
    }

    public override void DrawToolbarGUI(Rect toolbarRect)
    {
      using (new EditorGUI.DisabledScope(!this.containsMultipleSprites || this.spriteEditor.editingDisabled))
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SpriteFrameModule.\u003CDrawToolbarGUI\u003Ec__AnonStorey0 toolbarGuiCAnonStorey0 = new SpriteFrameModule.\u003CDrawToolbarGUI\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        toolbarGuiCAnonStorey0.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        toolbarGuiCAnonStorey0.skin = EditorStyles.toolbarPopup;
        Rect drawRect = toolbarRect;
        // ISSUE: reference to a compiler-generated field
        drawRect.width = toolbarGuiCAnonStorey0.skin.CalcSize(SpriteFrameModule.SpriteFrameModuleStyles.sliceButtonLabel).x;
        // ISSUE: reference to a compiler-generated method
        SpriteUtilityWindow.DrawToolBarWidget(ref drawRect, ref toolbarRect, new Action<Rect>(toolbarGuiCAnonStorey0.\u003C\u003Em__0));
        using (new EditorGUI.DisabledScope(!this.hasSelected))
        {
          drawRect.x += drawRect.width;
          // ISSUE: reference to a compiler-generated field
          drawRect.width = toolbarGuiCAnonStorey0.skin.CalcSize(SpriteFrameModule.SpriteFrameModuleStyles.trimButtonLabel).x;
          // ISSUE: reference to a compiler-generated method
          SpriteUtilityWindow.DrawToolBarWidget(ref drawRect, ref toolbarRect, new Action<Rect>(toolbarGuiCAnonStorey0.\u003C\u003Em__1));
        }
      }
    }

    private void HandleRectCornerScalingHandles()
    {
      if (!this.hasSelected)
        return;
      GUIStyle dragdot = SpriteFrameModuleBase.styles.dragdot;
      GUIStyle dragdotactive = SpriteFrameModuleBase.styles.dragdotactive;
      Color white = Color.white;
      Rect r = new Rect(this.selectedSpriteRect);
      float xMin = r.xMin;
      float xMax = r.xMax;
      float yMax = r.yMax;
      float yMin = r.yMin;
      EditorGUI.BeginChangeCheck();
      this.HandleBorderPointSlider(ref xMin, ref yMax, MouseCursor.ResizeUpLeft, false, dragdot, dragdotactive, white);
      this.HandleBorderPointSlider(ref xMax, ref yMax, MouseCursor.ResizeUpRight, false, dragdot, dragdotactive, white);
      this.HandleBorderPointSlider(ref xMin, ref yMin, MouseCursor.ResizeUpRight, false, dragdot, dragdotactive, white);
      this.HandleBorderPointSlider(ref xMax, ref yMin, MouseCursor.ResizeUpLeft, false, dragdot, dragdotactive, white);
      if (!EditorGUI.EndChangeCheck())
        return;
      r.xMin = xMin;
      r.xMax = xMax;
      r.yMax = yMax;
      r.yMin = yMin;
      this.ScaleSpriteRect(r);
    }

    private void HandleRectSideScalingHandles()
    {
      if (!this.hasSelected)
        return;
      Rect r = new Rect(this.selectedSpriteRect);
      float xMin = r.xMin;
      float xMax = r.xMax;
      float yMax = r.yMax;
      float yMin = r.yMin;
      Vector2 vector2_1 = (Vector2) Handles.matrix.MultiplyPoint(new Vector3(r.xMin, r.yMin));
      Vector2 vector2_2 = (Vector2) Handles.matrix.MultiplyPoint(new Vector3(r.xMax, r.yMax));
      float width = Mathf.Abs(vector2_2.x - vector2_1.x);
      float height = Mathf.Abs(vector2_2.y - vector2_1.y);
      EditorGUI.BeginChangeCheck();
      float num1 = this.HandleBorderScaleSlider(xMin, r.yMax, width, height, true);
      float num2 = this.HandleBorderScaleSlider(xMax, r.yMax, width, height, true);
      float num3 = this.HandleBorderScaleSlider(r.xMin, yMax, width, height, false);
      float num4 = this.HandleBorderScaleSlider(r.xMin, yMin, width, height, false);
      if (!EditorGUI.EndChangeCheck())
        return;
      r.xMin = num1;
      r.xMax = num2;
      r.yMax = num3;
      r.yMin = num4;
      this.ScaleSpriteRect(r);
    }

    private void HandleDragging()
    {
      if (!this.hasSelected || this.MouseOnTopOfInspector())
        return;
      Rect clamp = new Rect(0.0f, 0.0f, (float) this.previewTexture.width, (float) this.previewTexture.height);
      EditorGUI.BeginChangeCheck();
      Rect rect = SpriteEditorUtility.ClampedRect(SpriteEditorUtility.RoundedRect(SpriteEditorHandles.SliderRect(this.selectedSpriteRect)), clamp, true);
      if (EditorGUI.EndChangeCheck())
        this.selectedSpriteRect = rect;
    }

    private void HandleCreate()
    {
      if (this.MouseOnTopOfInspector() || this.eventSystem.current.alt)
        return;
      EditorGUI.BeginChangeCheck();
      Rect rect = SpriteEditorHandles.RectCreator((float) this.previewTexture.width, (float) this.previewTexture.height, SpriteFrameModuleBase.styles.createRect);
      if (EditorGUI.EndChangeCheck() && (double) rect.width > 0.0 && (double) rect.height > 0.0)
      {
        this.CreateSprite(rect);
        GUIUtility.keyboardControl = 0;
      }
    }

    private void HandleDuplicate()
    {
      IEvent current = this.eventSystem.current;
      if (current.type != EventType.ValidateCommand && current.type != EventType.ExecuteCommand || !(current.commandName == "Duplicate"))
        return;
      if (current.type == EventType.ExecuteCommand)
        this.DuplicateSprite();
      current.Use();
    }

    private void HandleDelete()
    {
      IEvent current = this.eventSystem.current;
      if (current.type != EventType.ValidateCommand && current.type != EventType.ExecuteCommand || !(current.commandName == "SoftDelete") && !(current.commandName == "Delete"))
        return;
      if (current.type == EventType.ExecuteCommand && this.hasSelected)
        this.DeleteSprite();
      current.Use();
    }

    public enum AutoSlicingMethod
    {
      DeleteAll,
      Smart,
      Safe,
    }

    private static class SpriteFrameModuleStyles
    {
      public static readonly GUIContent sliceButtonLabel = EditorGUIUtility.TextContent("Slice");
      public static readonly GUIContent trimButtonLabel = EditorGUIUtility.TextContent("Trim|Trims selected rectangle (T)");
      public static readonly GUIContent okButtonLabel = EditorGUIUtility.TextContent("Ok");
      public static readonly GUIContent cancelButtonLabel = EditorGUIUtility.TextContent("Cancel");
    }
  }
}
