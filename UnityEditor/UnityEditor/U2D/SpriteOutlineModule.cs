// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.SpriteOutlineModule
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.Experimental.U2D;
using UnityEditor.U2D.Interface;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor.U2D
{
  internal class SpriteOutlineModule : ISpriteEditorModule
  {
    private readonly string k_DeleteCommandName = "Delete";
    private readonly string k_SoftDeleteCommandName = "SoftDelete";
    private bool m_WasRectSelecting = false;
    protected SpriteRect m_Selected;
    private const float k_HandleSize = 5f;
    private ShapeEditor[] m_ShapeEditors;
    private bool m_RequestRepaint;
    private Matrix4x4 m_HandleMatrix;
    private Vector2 m_MousePosition;
    private bool m_Snap;
    private ShapeEditorRectSelectionTool m_ShapeSelectionUI;
    private Rect? m_SelectionRect;
    private ITexture2D m_OutlineTexture;
    private SpriteOutlineModule.Styles m_Styles;

    public SpriteOutlineModule(ISpriteEditor sem, IEventSystem es, IUndoSystem us, IAssetDatabase ad, IGUIUtility gu, IShapeEditorFactory sef, ITexture2D outlineTexture)
    {
      this.spriteEditorWindow = sem;
      this.undoSystem = us;
      this.eventSystem = es;
      this.assetDatabase = ad;
      this.guiUtility = gu;
      this.shapeEditorFactory = sef;
      this.m_OutlineTexture = outlineTexture;
      this.m_ShapeSelectionUI = new ShapeEditorRectSelectionTool(gu);
      this.m_ShapeSelectionUI.RectSelect += new Action<Rect, ShapeEditor.SelectionType>(this.RectSelect);
      this.m_ShapeSelectionUI.ClearSelection += new Action(this.ClearSelection);
    }

    public virtual string moduleName
    {
      get
      {
        return "Edit Outline";
      }
    }

    private SpriteOutlineModule.Styles styles
    {
      get
      {
        if (this.m_Styles == null)
          this.m_Styles = new SpriteOutlineModule.Styles();
        return this.m_Styles;
      }
    }

    protected virtual List<SpriteOutline> selectedShapeOutline
    {
      get
      {
        return this.m_Selected.outline;
      }
      set
      {
        this.m_Selected.outline = value;
      }
    }

    private bool shapeEditorDirty { get; set; }

    private bool editingDisabled
    {
      get
      {
        return this.spriteEditorWindow.editingDisabled;
      }
    }

    private ISpriteEditor spriteEditorWindow { get; set; }

    private IUndoSystem undoSystem { get; set; }

    private IEventSystem eventSystem { get; set; }

    private IAssetDatabase assetDatabase { get; set; }

    private IGUIUtility guiUtility { get; set; }

    private IShapeEditorFactory shapeEditorFactory { get; set; }

    private void RectSelect(Rect r, ShapeEditor.SelectionType selectionType)
    {
      this.m_SelectionRect = new Rect?(EditorGUIExt.FromToRect((Vector2) this.ScreenToLocal(r.min), (Vector2) this.ScreenToLocal(r.max)));
    }

    private void ClearSelection()
    {
      this.m_RequestRepaint = true;
    }

    public void OnModuleActivate()
    {
      this.GenerateOutlineIfNotExist();
      this.undoSystem.RegisterUndoCallback(new Undo.UndoRedoCallback(this.UndoRedoPerformed));
      this.shapeEditorDirty = true;
      this.SetupShapeEditor();
      this.spriteEditorWindow.enableMouseMoveEvent = true;
    }

    private void GenerateOutlineIfNotExist()
    {
      ISpriteRectCache spriteRects = this.spriteEditorWindow.spriteRects;
      if (spriteRects == null)
        return;
      bool flag = false;
      for (int i = 0; i < spriteRects.Count; ++i)
      {
        SpriteRect spriteRect = spriteRects.RectAt(i);
        if (!this.HasShapeOutline(spriteRect))
        {
          this.spriteEditorWindow.DisplayProgressBar(this.styles.generatingOutlineDialogTitle.text, string.Format(this.styles.generatingOutlineDialogContent.text, (object) (i + 1), (object) spriteRects.Count), (float) i / (float) spriteRects.Count);
          this.SetupShapeEditorOutline(spriteRect);
          flag = true;
        }
      }
      if (flag)
      {
        this.spriteEditorWindow.ClearProgressBar();
        this.spriteEditorWindow.ApplyOrRevertModification(true);
      }
    }

    public void OnModuleDeactivate()
    {
      this.undoSystem.UnregisterUndoCallback(new Undo.UndoRedoCallback(this.UndoRedoPerformed));
      this.CleanupShapeEditors();
      this.m_Selected = (SpriteRect) null;
      this.spriteEditorWindow.enableMouseMoveEvent = false;
    }

    public void DoTextureGUI()
    {
      IEvent current = this.eventSystem.current;
      this.m_RequestRepaint = false;
      this.m_HandleMatrix = Handles.matrix;
      this.m_MousePosition = (Vector2) Handles.inverseMatrix.MultiplyPoint((Vector3) this.eventSystem.current.mousePosition);
      if (this.m_Selected == null || !this.m_Selected.rect.Contains(this.m_MousePosition) && !this.IsMouseOverOutlinePoints() && !current.shift)
        this.spriteEditorWindow.HandleSpriteSelection();
      this.HandleCreateNewOutline();
      this.m_WasRectSelecting = this.m_ShapeSelectionUI.isSelecting;
      this.UpdateShapeEditors();
      this.m_ShapeSelectionUI.OnGUI();
      this.DrawGizmos();
      if (!this.m_RequestRepaint && current.type != EventType.MouseMove)
        return;
      this.spriteEditorWindow.RequestRepaint();
    }

    public void DrawToolbarGUI(Rect drawArea)
    {
      SpriteOutlineModule.Styles styles = this.styles;
      Rect position = new Rect(drawArea.x, drawArea.y, EditorStyles.toolbarButton.CalcSize(styles.snapButtonLabel).x, drawArea.height);
      this.m_Snap = GUI.Toggle(position, this.m_Snap, styles.snapButtonLabel, EditorStyles.toolbarButton);
      using (new EditorGUI.DisabledScope(this.editingDisabled || this.m_Selected == null))
      {
        float num1 = drawArea.width - position.width;
        drawArea.x = position.xMax;
        drawArea.width = EditorStyles.toolbarButton.CalcSize(styles.outlineTolerance).x;
        float num2 = num1 - drawArea.width;
        if ((double) num2 < 0.0)
          drawArea.width += num2;
        if ((double) drawArea.width > 0.0)
          GUI.Label(drawArea, styles.outlineTolerance, EditorStyles.miniLabel);
        drawArea.x += drawArea.width;
        drawArea.width = 100f;
        float num3 = num2 - drawArea.width;
        if ((double) num3 < 0.0)
          drawArea.width += num3;
        if ((double) drawArea.width > 0.0)
        {
          float num4 = this.m_Selected == null ? 0.0f : this.m_Selected.tessellationDetail;
          EditorGUI.BeginChangeCheck();
          float fieldWidth = EditorGUIUtility.fieldWidth;
          float labelWidth = EditorGUIUtility.labelWidth;
          EditorGUIUtility.fieldWidth = 30f;
          EditorGUIUtility.labelWidth = 1f;
          float num5 = EditorGUI.Slider(drawArea, Mathf.Clamp01(num4), 0.0f, 1f);
          if (EditorGUI.EndChangeCheck())
          {
            this.RecordUndo();
            this.m_Selected.tessellationDetail = num5;
          }
          EditorGUIUtility.fieldWidth = fieldWidth;
          EditorGUIUtility.labelWidth = labelWidth;
        }
        drawArea.x += drawArea.width;
        drawArea.width = EditorStyles.toolbarButton.CalcSize(styles.generateOutlineLabel).x;
        float num6 = num3 - drawArea.width;
        if ((double) num6 < 0.0)
          drawArea.width += num6;
        if ((double) drawArea.width <= 0.0 || !GUI.Button(drawArea, styles.generateOutlineLabel, EditorStyles.toolbarButton))
          return;
        this.RecordUndo();
        this.selectedShapeOutline.Clear();
        this.SetupShapeEditorOutline(this.m_Selected);
        this.spriteEditorWindow.SetDataModified();
        this.shapeEditorDirty = true;
      }
    }

    public void OnPostGUI()
    {
    }

    public bool CanBeActivated()
    {
      return UnityEditor.SpriteUtility.GetSpriteImportMode(this.spriteEditorWindow.spriteEditorDataProvider) != SpriteImportMode.None;
    }

    private void RecordUndo()
    {
      this.undoSystem.RegisterCompleteObjectUndo((IUndoableObject) this.spriteEditorWindow.spriteRects, "Outline changed");
    }

    public void CreateNewOutline(Rect rectOutline)
    {
      Rect rect1 = this.m_Selected.rect;
      if (!rect1.Contains(rectOutline.min) || !rect1.Contains(rectOutline.max))
        return;
      this.RecordUndo();
      SpriteOutline spriteOutline = new SpriteOutline();
      Vector2 vector2 = new Vector2(0.5f * rect1.width + rect1.x, 0.5f * rect1.height + rect1.y);
      Rect rect2 = new Rect(rectOutline);
      rect2.min = (Vector2) this.SnapPoint((Vector3) rectOutline.min);
      rect2.max = (Vector2) this.SnapPoint((Vector3) rectOutline.max);
      spriteOutline.Add(SpriteOutlineModule.CapPointToRect(new Vector2(rect2.xMin, rect2.yMin), rect1) - vector2);
      spriteOutline.Add(SpriteOutlineModule.CapPointToRect(new Vector2(rect2.xMin, rect2.yMax), rect1) - vector2);
      spriteOutline.Add(SpriteOutlineModule.CapPointToRect(new Vector2(rect2.xMax, rect2.yMax), rect1) - vector2);
      spriteOutline.Add(SpriteOutlineModule.CapPointToRect(new Vector2(rect2.xMax, rect2.yMin), rect1) - vector2);
      this.selectedShapeOutline.Add(spriteOutline);
      this.spriteEditorWindow.SetDataModified();
      this.shapeEditorDirty = true;
    }

    private void HandleCreateNewOutline()
    {
      if (this.m_WasRectSelecting && !this.m_ShapeSelectionUI.isSelecting && (this.m_SelectionRect.HasValue && this.m_Selected != null))
      {
        bool flag = true;
        foreach (ShapeEditor shapeEditor in this.m_ShapeEditors)
        {
          if (shapeEditor.selectedPoints.Count != 0)
          {
            flag = false;
            break;
          }
        }
        if (flag)
          this.CreateNewOutline(this.m_SelectionRect.Value);
      }
      this.m_SelectionRect = new Rect?();
    }

    public void UpdateShapeEditors()
    {
      this.SetupShapeEditor();
      if (this.m_Selected == null)
        return;
      IEvent current = this.eventSystem.current;
      bool flag = current.type == EventType.ExecuteCommand && (current.commandName == this.k_SoftDeleteCommandName || current.commandName == this.k_DeleteCommandName);
      for (int index = 0; index < this.m_ShapeEditors.Length; ++index)
      {
        if (this.m_ShapeEditors[index].GetPointsCount() != 0)
        {
          this.m_ShapeEditors[index].inEditMode = true;
          this.m_ShapeEditors[index].OnGUI();
          if (this.shapeEditorDirty)
            break;
        }
      }
      if (flag)
      {
        for (int index = this.selectedShapeOutline.Count - 1; index >= 0; --index)
        {
          if (this.selectedShapeOutline[index].Count < 3)
          {
            this.selectedShapeOutline.RemoveAt(index);
            this.shapeEditorDirty = true;
          }
        }
      }
    }

    private bool IsMouseOverOutlinePoints()
    {
      if (this.m_Selected == null)
        return false;
      Vector2 vector2 = new Vector2(0.5f * this.m_Selected.rect.width + this.m_Selected.rect.x, 0.5f * this.m_Selected.rect.height + this.m_Selected.rect.y);
      float handleSize = this.GetHandleSize();
      Rect rect = new Rect(0.0f, 0.0f, handleSize * 2f, handleSize * 2f);
      for (int index1 = 0; index1 < this.selectedShapeOutline.Count; ++index1)
      {
        SpriteOutline spriteOutline = this.selectedShapeOutline[index1];
        for (int index2 = 0; index2 < spriteOutline.Count; ++index2)
        {
          rect.center = spriteOutline[index2] + vector2;
          if (rect.Contains(this.m_MousePosition))
            return true;
        }
      }
      return false;
    }

    private float GetHandleSize()
    {
      return 5f / this.m_HandleMatrix.m00;
    }

    private void CleanupShapeEditors()
    {
      if (this.m_ShapeEditors != null)
      {
        for (int index1 = 0; index1 < this.m_ShapeEditors.Length; ++index1)
        {
          for (int index2 = 0; index2 < this.m_ShapeEditors.Length; ++index2)
          {
            if (index1 != index2)
              this.m_ShapeEditors[index2].UnregisterFromShapeEditor(this.m_ShapeEditors[index1]);
          }
          this.m_ShapeEditors[index1].OnDisable();
        }
      }
      this.m_ShapeEditors = (ShapeEditor[]) null;
    }

    public void SetupShapeEditor()
    {
      if (this.shapeEditorDirty || this.m_Selected != this.spriteEditorWindow.selectedSpriteRect)
      {
        this.m_Selected = this.spriteEditorWindow.selectedSpriteRect;
        this.CleanupShapeEditors();
        if (this.m_Selected != null)
        {
          this.SetupShapeEditorOutline(this.m_Selected);
          this.m_ShapeEditors = new ShapeEditor[this.selectedShapeOutline.Count];
          for (int index = 0; index < this.selectedShapeOutline.Count; ++index)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            SpriteOutlineModule.\u003CSetupShapeEditor\u003Ec__AnonStorey0 editorCAnonStorey0 = new SpriteOutlineModule.\u003CSetupShapeEditor\u003Ec__AnonStorey0();
            // ISSUE: reference to a compiler-generated field
            editorCAnonStorey0.\u0024this = this;
            // ISSUE: reference to a compiler-generated field
            editorCAnonStorey0.outlineIndex = index;
            this.m_ShapeEditors[index] = this.shapeEditorFactory.CreateShapeEditor();
            this.m_ShapeEditors[index].SetRectSelectionTool(this.m_ShapeSelectionUI);
            // ISSUE: reference to a compiler-generated method
            this.m_ShapeEditors[index].LocalToWorldMatrix = new Func<Matrix4x4>(editorCAnonStorey0.\u003C\u003Em__0);
            this.m_ShapeEditors[index].LocalToScreen = (Func<Vector3, Vector2>) (point => (Vector2) Handles.matrix.MultiplyPoint(point));
            this.m_ShapeEditors[index].ScreenToLocal = new Func<Vector2, Vector3>(this.ScreenToLocal);
            this.m_ShapeEditors[index].RecordUndo = new Action(this.RecordUndo);
            this.m_ShapeEditors[index].GetHandleSize = new Func<float>(this.GetHandleSize);
            this.m_ShapeEditors[index].lineTexture = this.m_OutlineTexture;
            this.m_ShapeEditors[index].Snap = new Func<Vector3, Vector3>(this.SnapPoint);
            // ISSUE: reference to a compiler-generated method
            this.m_ShapeEditors[index].GetPointPosition = new Func<int, Vector3>(editorCAnonStorey0.\u003C\u003Em__1);
            // ISSUE: reference to a compiler-generated method
            this.m_ShapeEditors[index].SetPointPosition = new Action<int, Vector3>(editorCAnonStorey0.\u003C\u003Em__2);
            // ISSUE: reference to a compiler-generated method
            this.m_ShapeEditors[index].InsertPointAt = new Action<int, Vector3>(editorCAnonStorey0.\u003C\u003Em__3);
            // ISSUE: reference to a compiler-generated method
            this.m_ShapeEditors[index].RemovePointAt = new Action<int>(editorCAnonStorey0.\u003C\u003Em__4);
            // ISSUE: reference to a compiler-generated method
            this.m_ShapeEditors[index].GetPointsCount = new Func<int>(editorCAnonStorey0.\u003C\u003Em__5);
          }
          for (int index1 = 0; index1 < this.selectedShapeOutline.Count; ++index1)
          {
            for (int index2 = 0; index2 < this.selectedShapeOutline.Count; ++index2)
            {
              if (index1 != index2)
                this.m_ShapeEditors[index2].RegisterToShapeEditor(this.m_ShapeEditors[index1]);
            }
          }
        }
        else
          this.m_ShapeEditors = new ShapeEditor[0];
      }
      this.shapeEditorDirty = false;
    }

    protected virtual bool HasShapeOutline(SpriteRect spriteRect)
    {
      return spriteRect.outline != null && spriteRect.outline.Count > 0;
    }

    protected virtual void SetupShapeEditorOutline(SpriteRect spriteRect)
    {
      if (spriteRect.outline != null && spriteRect.outline.Count != 0)
        return;
      spriteRect.outline = SpriteOutlineModule.GenerateSpriteRectOutline(spriteRect.rect, this.spriteEditorWindow.selectedTexture, spriteRect.tessellationDetail, (byte) 0, this.spriteEditorWindow.spriteEditorDataProvider);
      if (spriteRect.outline.Count == 0)
      {
        Vector2 vector2 = spriteRect.rect.size * 0.5f;
        spriteRect.outline = new List<SpriteOutline>()
        {
          new SpriteOutline()
          {
            m_Path = new List<Vector2>()
            {
              new Vector2(-vector2.x, -vector2.y),
              new Vector2(-vector2.x, vector2.y),
              new Vector2(vector2.x, vector2.y),
              new Vector2(vector2.x, -vector2.y)
            }
          }
        };
      }
    }

    public Vector3 SnapPoint(Vector3 position)
    {
      if (this.m_Snap)
      {
        position.x = (float) Mathf.RoundToInt(position.x);
        position.y = (float) Mathf.RoundToInt(position.y);
      }
      return position;
    }

    public Vector3 GetPointPosition(int outlineIndex, int pointIndex)
    {
      if (outlineIndex >= 0 && outlineIndex < this.selectedShapeOutline.Count)
      {
        SpriteOutline spriteOutline = this.selectedShapeOutline[outlineIndex];
        if (pointIndex >= 0 && pointIndex < spriteOutline.Count)
          return (Vector3) this.ConvertSpriteRectSpaceToTextureSpace(spriteOutline[pointIndex]);
      }
      return new Vector3(float.NaN, float.NaN, float.NaN);
    }

    public void SetPointPosition(int outlineIndex, int pointIndex, Vector3 position)
    {
      this.selectedShapeOutline[outlineIndex][pointIndex] = this.ConvertTextureSpaceToSpriteRectSpace(SpriteOutlineModule.CapPointToRect((Vector2) position, this.m_Selected.rect));
      this.spriteEditorWindow.SetDataModified();
    }

    public void InsertPointAt(int outlineIndex, int pointIndex, Vector3 position)
    {
      this.selectedShapeOutline[outlineIndex].Insert(pointIndex, this.ConvertTextureSpaceToSpriteRectSpace(SpriteOutlineModule.CapPointToRect((Vector2) position, this.m_Selected.rect)));
      this.spriteEditorWindow.SetDataModified();
    }

    public void RemovePointAt(int outlineIndex, int i)
    {
      this.selectedShapeOutline[outlineIndex].RemoveAt(i);
      this.spriteEditorWindow.SetDataModified();
    }

    public int GetPointsCount(int outlineIndex)
    {
      return this.selectedShapeOutline[outlineIndex].Count;
    }

    private Vector2 ConvertSpriteRectSpaceToTextureSpace(Vector2 value)
    {
      Vector2 vector2 = new Vector2(0.5f * this.m_Selected.rect.width + this.m_Selected.rect.x, 0.5f * this.m_Selected.rect.height + this.m_Selected.rect.y);
      value += vector2;
      return value;
    }

    private Vector2 ConvertTextureSpaceToSpriteRectSpace(Vector2 value)
    {
      Vector2 vector2 = new Vector2(0.5f * this.m_Selected.rect.width + this.m_Selected.rect.x, 0.5f * this.m_Selected.rect.height + this.m_Selected.rect.y);
      value -= vector2;
      return value;
    }

    private Vector3 ScreenToLocal(Vector2 point)
    {
      return Handles.inverseMatrix.MultiplyPoint((Vector3) point);
    }

    private void UndoRedoPerformed()
    {
      this.shapeEditorDirty = true;
    }

    private void DrawGizmos()
    {
      if (this.eventSystem.current.type != EventType.Repaint)
        return;
      SpriteRect selectedSpriteRect = this.spriteEditorWindow.selectedSpriteRect;
      if (selectedSpriteRect != null)
      {
        SpriteEditorUtility.BeginLines(this.styles.spriteBorderColor);
        SpriteEditorUtility.DrawBox(selectedSpriteRect.rect);
        SpriteEditorUtility.EndLines();
      }
    }

    protected static List<SpriteOutline> GenerateSpriteRectOutline(Rect rect, ITexture2D texture, float detail, byte alphaTolerance, ISpriteEditorDataProvider spriteEditorDataProvider)
    {
      List<SpriteOutline> spriteOutlineList = new List<SpriteOutline>();
      if (texture != (ITexture2D) null)
      {
        int width1 = 0;
        int height1 = 0;
        spriteEditorDataProvider.GetTextureActualWidthAndHeight(out width1, out height1);
        int width2 = texture.width;
        int height2 = texture.height;
        Vector2 vector2_1 = new Vector2((float) width2 / (float) width1, (float) height2 / (float) height1);
        Rect rect1 = rect;
        rect1.xMin *= vector2_1.x;
        rect1.xMax *= vector2_1.x;
        rect1.yMin *= vector2_1.y;
        rect1.yMax *= vector2_1.y;
        Vector2[][] paths;
        UnityEditor.Sprites.SpriteUtility.GenerateOutline((UnityEngine.Texture2D) texture, rect1, detail, alphaTolerance, true, out paths);
        Rect r = new Rect();
        r.size = rect.size;
        r.center = Vector2.zero;
        for (int index = 0; index < paths.Length; ++index)
        {
          SpriteOutline spriteOutline = new SpriteOutline();
          foreach (Vector2 vector2_2 in paths[index])
            spriteOutline.Add(SpriteOutlineModule.CapPointToRect(new Vector2(vector2_2.x / vector2_1.x, vector2_2.y / vector2_1.y), r));
          spriteOutlineList.Add(spriteOutline);
        }
      }
      return spriteOutlineList;
    }

    private static Vector2 CapPointToRect(Vector2 so, Rect r)
    {
      so.x = Mathf.Min(r.xMax, so.x);
      so.x = Mathf.Max(r.xMin, so.x);
      so.y = Mathf.Min(r.yMax, so.y);
      so.y = Mathf.Max(r.yMin, so.y);
      return so;
    }

    private class Styles
    {
      public GUIContent generateOutlineLabel = EditorGUIUtility.TextContent("Update|Update new outline based on mesh detail value.");
      public GUIContent outlineTolerance = EditorGUIUtility.TextContent("Outline Tolerance|Sets how tight the outline should be from the sprite.");
      public GUIContent snapButtonLabel = EditorGUIUtility.TextContent("Snap|Snap points to nearest pixel");
      public GUIContent generatingOutlineDialogTitle = EditorGUIUtility.TextContent("Outline");
      public GUIContent generatingOutlineDialogContent = EditorGUIUtility.TextContent("Generating outline {0}/{1}");
      public Color spriteBorderColor = new Color(0.25f, 0.5f, 1f, 0.75f);
    }
  }
}
