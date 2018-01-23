// Decompiled with JetBrains decompiler
// Type: UnityEditor.ShapeEditorRectSelectionTool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class ShapeEditorRectSelectionTool
  {
    private Vector2 m_SelectStartPoint;
    private Vector2 m_SelectMousePoint;
    private bool m_RectSelecting;
    private int m_RectSelectionID;
    private const float k_MinSelectionSize = 6f;

    public ShapeEditorRectSelectionTool(IGUIUtility gu)
    {
      this.guiUtility = gu;
      this.m_RectSelectionID = this.guiUtility.GetPermanentControlID();
    }

    public event Action<Rect, ShapeEditor.SelectionType> RectSelect = (i, p) => {};

    public event Action ClearSelection = () => {};

    public void OnGUI()
    {
      Event current = Event.current;
      Handles.BeginGUI();
      Vector2 mousePosition = current.mousePosition;
      int rectSelectionId = this.m_RectSelectionID;
      switch (current.GetTypeForControl(rectSelectionId))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == rectSelectionId && current.button == 0)
          {
            this.guiUtility.hotControl = rectSelectionId;
            this.m_SelectStartPoint = mousePosition;
            break;
          }
          break;
        case EventType.MouseUp:
          if (this.guiUtility.hotControl == rectSelectionId && current.button == 0)
          {
            this.guiUtility.hotControl = 0;
            this.guiUtility.keyboardControl = 0;
            if (this.m_RectSelecting)
            {
              this.m_SelectMousePoint = new Vector2(mousePosition.x, mousePosition.y);
              ShapeEditor.SelectionType selectionType = ShapeEditor.SelectionType.Normal;
              if (Event.current.control)
                selectionType = ShapeEditor.SelectionType.Subtractive;
              else if (Event.current.shift)
                selectionType = ShapeEditor.SelectionType.Additive;
              // ISSUE: reference to a compiler-generated field
              this.RectSelect(EditorGUIExt.FromToRect(this.m_SelectStartPoint, this.m_SelectMousePoint), selectionType);
              this.m_RectSelecting = false;
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.ClearSelection();
            }
            current.Use();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (this.guiUtility.hotControl == rectSelectionId)
          {
            if (!this.m_RectSelecting && (double) (mousePosition - this.m_SelectStartPoint).magnitude > 6.0)
              this.m_RectSelecting = true;
            if (this.m_RectSelecting)
            {
              this.m_SelectMousePoint = mousePosition;
              ShapeEditor.SelectionType selectionType = ShapeEditor.SelectionType.Normal;
              if (Event.current.control)
                selectionType = ShapeEditor.SelectionType.Subtractive;
              else if (Event.current.shift)
                selectionType = ShapeEditor.SelectionType.Additive;
              // ISSUE: reference to a compiler-generated field
              this.RectSelect(EditorGUIExt.FromToRect(this.m_SelectStartPoint, this.m_SelectMousePoint), selectionType);
            }
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (this.guiUtility.hotControl == rectSelectionId && this.m_RectSelecting)
          {
            EditorStyles.selectionRect.Draw(EditorGUIExt.FromToRect(this.m_SelectStartPoint, this.m_SelectMousePoint), GUIContent.none, false, false, false, false);
            break;
          }
          break;
        case EventType.Layout:
          if (!Tools.viewToolActive)
          {
            HandleUtility.AddDefaultControl(rectSelectionId);
            break;
          }
          break;
      }
      Handles.EndGUI();
    }

    public bool isSelecting
    {
      get
      {
        return this.guiUtility.hotControl == this.m_RectSelectionID;
      }
    }

    private IGUIUtility guiUtility { get; set; }
  }
}
