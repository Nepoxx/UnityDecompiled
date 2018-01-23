// Decompiled with JetBrains decompiler
// Type: UnityEditor.PolygonEditorUtility
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class PolygonEditorUtility
  {
    private bool m_LoopingCollider = false;
    private int m_MinPathPoints = 3;
    private int m_SelectedPath = -1;
    private int m_SelectedVertex = -1;
    private int m_SelectedEdgePath = -1;
    private int m_SelectedEdgeVertex0 = -1;
    private int m_SelectedEdgeVertex1 = -1;
    private bool m_LeftIntersect = false;
    private bool m_RightIntersect = false;
    private bool m_DeleteMode = false;
    private bool m_HandlePoint = false;
    private bool m_HandleEdge = false;
    private const float k_HandlePointSnap = 10f;
    private const float k_HandlePickDistance = 50f;
    private Collider2D m_ActiveCollider;
    private bool m_FirstOnSceneGUIAfterReset;

    public void Reset()
    {
      this.m_SelectedPath = -1;
      this.m_SelectedVertex = -1;
      this.m_SelectedEdgePath = -1;
      this.m_SelectedEdgeVertex0 = -1;
      this.m_SelectedEdgeVertex1 = -1;
      this.m_LeftIntersect = false;
      this.m_RightIntersect = false;
      this.m_FirstOnSceneGUIAfterReset = true;
      this.m_HandlePoint = false;
      this.m_HandleEdge = false;
    }

    private void UndoRedoPerformed()
    {
      if (!((UnityEngine.Object) this.m_ActiveCollider != (UnityEngine.Object) null))
        return;
      Collider2D activeCollider = this.m_ActiveCollider;
      this.StopEditing();
      this.StartEditing(activeCollider);
    }

    public void StartEditing(Collider2D collider)
    {
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      this.Reset();
      PolygonCollider2D polygonCollider2D = collider as PolygonCollider2D;
      if ((bool) ((UnityEngine.Object) polygonCollider2D))
      {
        this.m_ActiveCollider = collider;
        this.m_LoopingCollider = true;
        this.m_MinPathPoints = 3;
        PolygonEditor.StartEditing((Collider2D) polygonCollider2D);
      }
      else
      {
        EdgeCollider2D edgeCollider2D = collider as EdgeCollider2D;
        if (!(bool) ((UnityEngine.Object) edgeCollider2D))
          throw new NotImplementedException(string.Format("PolygonEditorUtility does not support {0}", (object) collider));
        this.m_ActiveCollider = collider;
        this.m_LoopingCollider = false;
        this.m_MinPathPoints = 2;
        PolygonEditor.StartEditing((Collider2D) edgeCollider2D);
      }
    }

    public void StopEditing()
    {
      PolygonEditor.StopEditing();
      this.m_ActiveCollider = (Collider2D) null;
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
    }

    public void OnSceneGUI()
    {
      if ((UnityEngine.Object) this.m_ActiveCollider == (UnityEngine.Object) null || Tools.viewToolActive)
        return;
      Vector2 offset = this.m_ActiveCollider.offset;
      Event current = Event.current;
      this.m_DeleteMode = current.command || current.control;
      Transform transform = this.m_ActiveCollider.transform;
      GUIUtility.keyboardControl = 0;
      HandleUtility.s_CustomPickDistance = 50f;
      Plane plane = new Plane(-transform.forward, Vector3.zero);
      Ray worldRay = HandleUtility.GUIPointToWorldRay(current.mousePosition);
      float enter;
      plane.Raycast(worldRay, out enter);
      Vector3 point1 = worldRay.GetPoint(enter);
      Vector2 vector2_1 = (Vector2) transform.InverseTransformPoint(point1);
      if (current.type == EventType.MouseMove || this.m_FirstOnSceneGUIAfterReset)
      {
        int pathIndex;
        int num;
        float distance;
        if (PolygonEditor.GetNearestPoint(vector2_1 - offset, out pathIndex, out num, out distance))
        {
          this.m_SelectedPath = pathIndex;
          this.m_SelectedVertex = num;
        }
        else
          this.m_SelectedPath = -1;
        int pointIndex1;
        if (PolygonEditor.GetNearestEdge(vector2_1 - offset, out pathIndex, out num, out pointIndex1, out distance, this.m_LoopingCollider))
        {
          this.m_SelectedEdgePath = pathIndex;
          this.m_SelectedEdgeVertex0 = num;
          this.m_SelectedEdgeVertex1 = pointIndex1;
        }
        else
          this.m_SelectedEdgePath = -1;
        if (current.type == EventType.MouseMove)
          current.Use();
      }
      else if (current.type == EventType.MouseUp)
      {
        this.m_LeftIntersect = false;
        this.m_RightIntersect = false;
      }
      if (GUIUtility.hotControl == 0)
      {
        if (this.m_SelectedPath != -1 && this.m_SelectedEdgePath != -1)
        {
          Vector2 point2;
          PolygonEditor.GetPoint(this.m_SelectedPath, this.m_SelectedVertex, out point2);
          point2 += offset;
          this.m_HandleEdge = (double) (HandleUtility.WorldToGUIPoint(transform.TransformPoint((Vector3) point2)) - Event.current.mousePosition).sqrMagnitude > 100.0;
          this.m_HandlePoint = !this.m_HandleEdge;
        }
        else if (this.m_SelectedPath != -1)
          this.m_HandlePoint = true;
        else if (this.m_SelectedEdgePath != -1)
          this.m_HandleEdge = true;
        if (this.m_DeleteMode && this.m_HandleEdge)
        {
          this.m_HandleEdge = false;
          this.m_HandlePoint = true;
        }
      }
      bool flag = false;
      if (this.m_HandleEdge && !this.m_DeleteMode)
      {
        Vector2 point2;
        PolygonEditor.GetPoint(this.m_SelectedEdgePath, this.m_SelectedEdgeVertex0, out point2);
        Vector2 point3;
        PolygonEditor.GetPoint(this.m_SelectedEdgePath, this.m_SelectedEdgeVertex1, out point3);
        point2 += offset;
        Vector2 vector2_2 = point3 + offset;
        Vector3 vector3_1 = transform.TransformPoint((Vector3) point2);
        Vector3 vector3_2 = transform.TransformPoint((Vector3) vector2_2);
        vector3_1.z = vector3_2.z = 0.0f;
        Handles.color = Color.green;
        Handles.DrawAAPolyLine(4f, new Vector3[2]
        {
          vector3_1,
          vector3_2
        });
        Handles.color = Color.white;
        Vector2 nearestPointOnEdge = this.GetNearestPointOnEdge((Vector2) transform.TransformPoint((Vector3) vector2_1), (Vector2) vector3_1, (Vector2) vector3_2);
        EditorGUI.BeginChangeCheck();
        float num1 = HandleUtility.GetHandleSize((Vector3) nearestPointOnEdge) * 0.04f;
        Handles.color = Color.green;
        Vector3 handlePos = (Vector3) nearestPointOnEdge;
        Vector3 handleDir = new Vector3(0.0f, 0.0f, 1f);
        Vector3 slideDir1 = new Vector3(1f, 0.0f, 0.0f);
        Vector3 slideDir2 = new Vector3(0.0f, 1f, 0.0f);
        double num2 = (double) num1;
        // ISSUE: reference to a compiler-generated field
        if (PolygonEditorUtility.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PolygonEditorUtility.\u003C\u003Ef__mg\u0024cache0 = new Handles.CapFunction(Handles.DotHandleCap);
        }
        // ISSUE: reference to a compiler-generated field
        Handles.CapFunction fMgCache0 = PolygonEditorUtility.\u003C\u003Ef__mg\u0024cache0;
        Vector2 zero = (Vector2) Vector3.zero;
        Vector2 vector2_3 = (Vector2) Handles.Slider2D(handlePos, handleDir, slideDir1, slideDir2, (float) num2, fMgCache0, zero);
        Handles.color = Color.white;
        if (EditorGUI.EndChangeCheck())
        {
          PolygonEditor.InsertPoint(this.m_SelectedEdgePath, this.m_SelectedEdgeVertex1, (point2 + vector2_2) / 2f - offset);
          this.m_SelectedPath = this.m_SelectedEdgePath;
          this.m_SelectedVertex = this.m_SelectedEdgeVertex1;
          this.m_HandleEdge = false;
          this.m_HandlePoint = true;
          flag = true;
        }
      }
      if (this.m_HandlePoint)
      {
        Vector2 point2;
        PolygonEditor.GetPoint(this.m_SelectedPath, this.m_SelectedVertex, out point2);
        point2 += offset;
        Vector3 vector3_1 = transform.TransformPoint((Vector3) point2);
        vector3_1.z = 0.0f;
        Vector2 guiPoint = HandleUtility.WorldToGUIPoint(vector3_1);
        float num1 = HandleUtility.GetHandleSize(vector3_1) * 0.04f;
        if (this.m_DeleteMode && current.type == EventType.MouseDown && (double) Vector2.Distance(guiPoint, Event.current.mousePosition) < 50.0 || this.DeleteCommandEvent(current))
        {
          if (current.type != EventType.ValidateCommand && PolygonEditor.GetPointCount(this.m_SelectedPath) > this.m_MinPathPoints)
          {
            PolygonEditor.RemovePoint(this.m_SelectedPath, this.m_SelectedVertex);
            this.Reset();
            flag = true;
          }
          current.Use();
        }
        EditorGUI.BeginChangeCheck();
        Handles.color = !this.m_DeleteMode ? Color.green : Color.red;
        Vector3 handlePos = vector3_1;
        Vector3 handleDir = new Vector3(0.0f, 0.0f, 1f);
        Vector3 slideDir1 = new Vector3(1f, 0.0f, 0.0f);
        Vector3 slideDir2 = new Vector3(0.0f, 1f, 0.0f);
        double num2 = (double) num1;
        // ISSUE: reference to a compiler-generated field
        if (PolygonEditorUtility.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          PolygonEditorUtility.\u003C\u003Ef__mg\u0024cache1 = new Handles.CapFunction(Handles.DotHandleCap);
        }
        // ISSUE: reference to a compiler-generated field
        Handles.CapFunction fMgCache1 = PolygonEditorUtility.\u003C\u003Ef__mg\u0024cache1;
        Vector2 zero = (Vector2) Vector3.zero;
        Vector3 vector3_2 = Handles.Slider2D(handlePos, handleDir, slideDir1, slideDir2, (float) num2, fMgCache1, zero);
        Handles.color = Color.white;
        if (EditorGUI.EndChangeCheck() && !this.m_DeleteMode)
        {
          point2 = (Vector2) transform.InverseTransformPoint(vector3_2);
          point2 -= offset;
          PolygonEditor.TestPointMove(this.m_SelectedPath, this.m_SelectedVertex, point2, out this.m_LeftIntersect, out this.m_RightIntersect, this.m_LoopingCollider);
          PolygonEditor.SetPoint(this.m_SelectedPath, this.m_SelectedVertex, point2);
          flag = true;
        }
        if (!flag)
          this.DrawEdgesForSelectedPoint(vector3_2, transform, this.m_LeftIntersect, this.m_RightIntersect, this.m_LoopingCollider);
      }
      if (flag)
      {
        Undo.RecordObject((UnityEngine.Object) this.m_ActiveCollider, "Edit Collider");
        PolygonEditor.ApplyEditing(this.m_ActiveCollider);
      }
      if (this.DeleteCommandEvent(current))
        Event.current.Use();
      this.m_FirstOnSceneGUIAfterReset = false;
    }

    private bool DeleteCommandEvent(Event evt)
    {
      return (evt.type == EventType.ExecuteCommand || evt.type == EventType.ValidateCommand) && (evt.commandName == "Delete" || evt.commandName == "SoftDelete");
    }

    private void DrawEdgesForSelectedPoint(Vector3 worldPos, Transform transform, bool leftIntersect, bool rightIntersect, bool loop)
    {
      bool flag1 = true;
      bool flag2 = true;
      int pointCount = PolygonEditor.GetPointCount(this.m_SelectedPath);
      int pointIndex1 = this.m_SelectedVertex - 1;
      if (pointIndex1 == -1)
      {
        pointIndex1 = pointCount - 1;
        flag1 = loop;
      }
      int pointIndex2 = this.m_SelectedVertex + 1;
      if (pointIndex2 == pointCount)
      {
        pointIndex2 = 0;
        flag2 = loop;
      }
      Vector2 offset = this.m_ActiveCollider.offset;
      Vector2 point1;
      PolygonEditor.GetPoint(this.m_SelectedPath, pointIndex1, out point1);
      Vector2 point2;
      PolygonEditor.GetPoint(this.m_SelectedPath, pointIndex2, out point2);
      Vector2 vector2 = point1 + offset;
      point2 += offset;
      Vector3 vector3_1 = transform.TransformPoint((Vector3) vector2);
      Vector3 vector3_2 = transform.TransformPoint((Vector3) point2);
      vector3_1.z = vector3_2.z = worldPos.z;
      float width = 4f;
      if (flag1)
      {
        Handles.color = leftIntersect || this.m_DeleteMode ? Color.red : Color.green;
        Handles.DrawAAPolyLine(width, new Vector3[2]
        {
          worldPos,
          vector3_1
        });
      }
      if (flag2)
      {
        Handles.color = rightIntersect || this.m_DeleteMode ? Color.red : Color.green;
        Handles.DrawAAPolyLine(width, new Vector3[2]
        {
          worldPos,
          vector3_2
        });
      }
      Handles.color = Color.white;
    }

    private Vector2 GetNearestPointOnEdge(Vector2 point, Vector2 start, Vector2 end)
    {
      Vector2 rhs = point - start;
      Vector2 normalized = (end - start).normalized;
      float num = Vector2.Dot(normalized, rhs);
      if ((double) num <= 0.0)
        return start;
      if ((double) num >= (double) Vector2.Distance(start, end))
        return end;
      Vector2 vector2 = normalized * num;
      return start + vector2;
    }
  }
}
