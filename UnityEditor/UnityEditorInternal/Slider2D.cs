// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Slider2D
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class Slider2D
  {
    private static Vector2 s_CurrentMousePosition;
    private static Vector3 s_StartPosition;
    private static Vector2 s_StartPlaneOffset;

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static Vector3 Do(int id, Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, float snap, bool drawHelper)
    {
      return Slider2D.Do(id, handlePos, new Vector3(0.0f, 0.0f, 0.0f), handleDir, slideDir1, slideDir2, handleSize, drawFunc, new Vector2(snap, snap), drawHelper);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static Vector3 Do(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, float snap, bool drawHelper)
    {
      return Slider2D.Do(id, handlePos, offset, handleDir, slideDir1, slideDir2, handleSize, drawFunc, new Vector2(snap, snap), drawHelper);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static Vector3 Do(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap, bool drawHelper)
    {
      bool changed = GUI.changed;
      GUI.changed = false;
      Vector2 vector2 = Slider2D.CalcDeltaAlongDirections(id, handlePos, offset, handleDir, slideDir1, slideDir2, handleSize, drawFunc, snap, drawHelper);
      if (GUI.changed)
        handlePos = Slider2D.s_StartPosition + slideDir1 * vector2.x + slideDir2 * vector2.y;
      GUI.changed |= changed;
      return handlePos;
    }

    public static Vector3 Do(int id, Vector3 handlePos, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.CapFunction capFunction, float snap, bool drawHelper)
    {
      return Slider2D.Do(id, handlePos, new Vector3(0.0f, 0.0f, 0.0f), handleDir, slideDir1, slideDir2, handleSize, capFunction, new Vector2(snap, snap), drawHelper);
    }

    public static Vector3 Do(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.CapFunction capFunction, float snap, bool drawHelper)
    {
      return Slider2D.Do(id, handlePos, offset, handleDir, slideDir1, slideDir2, handleSize, capFunction, new Vector2(snap, snap), drawHelper);
    }

    public static Vector3 Do(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.CapFunction capFunction, Vector2 snap, bool drawHelper)
    {
      bool changed = GUI.changed;
      GUI.changed = false;
      Vector2 vector2 = Slider2D.CalcDeltaAlongDirections(id, handlePos, offset, handleDir, slideDir1, slideDir2, handleSize, capFunction, snap, drawHelper);
      if (GUI.changed)
        handlePos = Slider2D.s_StartPosition + slideDir1 * vector2.x + slideDir2 * vector2.y;
      GUI.changed |= changed;
      return handlePos;
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    private static Vector2 CalcDeltaAlongDirections(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.DrawCapFunction drawFunc, Vector2 snap, bool drawHelper)
    {
      Vector2 vector2 = new Vector2(0.0f, 0.0f);
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && current.button == 0 && GUIUtility.hotControl == 0)
          {
            Plane plane = new Plane(Handles.matrix.MultiplyVector(handleDir), Handles.matrix.MultiplyPoint(handlePos));
            Ray worldRay = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            float enter = 0.0f;
            plane.Raycast(worldRay, out enter);
            GUIUtility.hotControl = id;
            Slider2D.s_CurrentMousePosition = current.mousePosition;
            Slider2D.s_StartPosition = handlePos;
            Vector3 lhs = Handles.inverseMatrix.MultiplyPoint(worldRay.GetPoint(enter)) - handlePos;
            Slider2D.s_StartPlaneOffset.x = Vector3.Dot(lhs, slideDir1);
            Slider2D.s_StartPlaneOffset.y = Vector3.Dot(lhs, slideDir2);
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(1);
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = 0;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(0);
            break;
          }
          break;
        case EventType.MouseMove:
          if (id == HandleUtility.nearestControl)
          {
            HandleUtility.Repaint();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == id)
          {
            Slider2D.s_CurrentMousePosition += current.delta;
            Vector3 a = Handles.matrix.MultiplyPoint(handlePos);
            Vector3 normalized1 = Handles.matrix.MultiplyVector(slideDir1).normalized;
            Vector3 normalized2 = Handles.matrix.MultiplyVector(slideDir2).normalized;
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Slider2D.s_CurrentMousePosition);
            Plane plane = new Plane(a, a + normalized1, a + normalized2);
            float enter = 0.0f;
            if (plane.Raycast(worldRay, out enter))
            {
              Vector3 point = Handles.inverseMatrix.MultiplyPoint(worldRay.GetPoint(enter));
              vector2.x = HandleUtility.PointOnLineParameter(point, Slider2D.s_StartPosition, slideDir1);
              vector2.y = HandleUtility.PointOnLineParameter(point, Slider2D.s_StartPosition, slideDir2);
              vector2 -= Slider2D.s_StartPlaneOffset;
              if ((double) snap.x > 0.0 || (double) snap.y > 0.0)
              {
                vector2.x = Handles.SnapValue(vector2.x, snap.x);
                vector2.y = Handles.SnapValue(vector2.y, snap.y);
              }
              GUI.changed = true;
            }
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (drawFunc != null)
          {
            Vector3 position = handlePos + offset;
            Quaternion rotation = Quaternion.LookRotation(handleDir, slideDir1);
            Color color1 = Color.white;
            if (id == GUIUtility.hotControl)
            {
              color1 = Handles.color;
              Handles.color = Handles.selectedColor;
            }
            else if (id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
            {
              color1 = Handles.color;
              Handles.color = Handles.preselectionColor;
            }
            drawFunc(id, position, rotation, handleSize);
            if (id == GUIUtility.hotControl || id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
              Handles.color = color1;
            if (drawHelper && GUIUtility.hotControl == id)
            {
              Vector3[] verts = new Vector3[4];
              float num1 = handleSize * 10f;
              verts[0] = position + (slideDir1 * num1 + slideDir2 * num1);
              verts[1] = verts[0] - slideDir1 * num1 * 2f;
              verts[2] = verts[1] - slideDir2 * num1 * 2f;
              verts[3] = verts[2] + slideDir1 * num1 * 2f;
              Color color2 = Handles.color;
              Handles.color = Color.white;
              float num2 = 0.6f;
              Handles.DrawSolidRectangleWithOutline(verts, new Color(1f, 1f, 1f, 0.05f), new Color(num2, num2, num2, 0.4f));
              Handles.color = color2;
              break;
            }
            break;
          }
          break;
        case EventType.Layout:
          Handles.DrawCapFunction drawCapFunction1 = drawFunc;
          // ISSUE: reference to a compiler-generated field
          if (Slider2D.\u003C\u003Ef__mg\u0024cache0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Slider2D.\u003C\u003Ef__mg\u0024cache0 = new Handles.DrawCapFunction(Handles.ArrowCap);
          }
          // ISSUE: reference to a compiler-generated field
          Handles.DrawCapFunction fMgCache0 = Slider2D.\u003C\u003Ef__mg\u0024cache0;
          if (drawCapFunction1 == fMgCache0)
          {
            HandleUtility.AddControl(id, HandleUtility.DistanceToLine(handlePos + offset, handlePos + handleDir * handleSize));
            HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(handlePos + offset + handleDir * handleSize, handleSize * 0.2f));
            break;
          }
          Handles.DrawCapFunction drawCapFunction2 = drawFunc;
          // ISSUE: reference to a compiler-generated field
          if (Slider2D.\u003C\u003Ef__mg\u0024cache1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Slider2D.\u003C\u003Ef__mg\u0024cache1 = new Handles.DrawCapFunction(Handles.RectangleCap);
          }
          // ISSUE: reference to a compiler-generated field
          Handles.DrawCapFunction fMgCache1 = Slider2D.\u003C\u003Ef__mg\u0024cache1;
          if (drawCapFunction2 == fMgCache1)
          {
            HandleUtility.AddControl(id, HandleUtility.DistanceToRectangle(handlePos + offset, Quaternion.LookRotation(handleDir, slideDir1), handleSize));
            break;
          }
          HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(handlePos + offset, handleSize * 0.5f));
          break;
      }
      return vector2;
    }

    private static Vector2 CalcDeltaAlongDirections(int id, Vector3 handlePos, Vector3 offset, Vector3 handleDir, Vector3 slideDir1, Vector3 slideDir2, float handleSize, Handles.CapFunction capFunction, Vector2 snap, bool drawHelper)
    {
      Vector3 position = handlePos + offset;
      Quaternion rotation = Quaternion.LookRotation(handleDir, slideDir1);
      Vector2 vector2 = new Vector2(0.0f, 0.0f);
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && current.button == 0 && GUIUtility.hotControl == 0)
          {
            Slider2D.s_CurrentMousePosition = current.mousePosition;
            bool success = true;
            Vector3 vector3 = Handles.inverseMatrix.MultiplyPoint(Slider2D.GetMousePosition(handleDir, handlePos, ref success));
            if (success)
            {
              GUIUtility.hotControl = id;
              Slider2D.s_StartPosition = handlePos;
              Vector3 lhs = vector3 - handlePos;
              Slider2D.s_StartPlaneOffset.x = Vector3.Dot(lhs, slideDir1);
              Slider2D.s_StartPlaneOffset.y = Vector3.Dot(lhs, slideDir2);
              current.Use();
              EditorGUIUtility.SetWantsMouseJumping(1);
            }
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = 0;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(0);
            break;
          }
          break;
        case EventType.MouseMove:
          if (id == HandleUtility.nearestControl)
          {
            HandleUtility.Repaint();
            break;
          }
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl == id)
          {
            Slider2D.s_CurrentMousePosition += current.delta;
            bool success = true;
            Vector3 point = Handles.inverseMatrix.MultiplyPoint(Slider2D.GetMousePosition(handleDir, handlePos, ref success));
            if (success)
            {
              vector2.x = HandleUtility.PointOnLineParameter(point, Slider2D.s_StartPosition, slideDir1);
              vector2.y = HandleUtility.PointOnLineParameter(point, Slider2D.s_StartPosition, slideDir2);
              vector2 -= Slider2D.s_StartPlaneOffset;
              if ((double) snap.x > 0.0 || (double) snap.y > 0.0)
              {
                vector2.x = Handles.SnapValue(vector2.x, snap.x);
                vector2.y = Handles.SnapValue(vector2.y, snap.y);
              }
              GUI.changed = true;
            }
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          if (capFunction != null)
          {
            Color color1 = Color.white;
            if (id == GUIUtility.hotControl)
            {
              color1 = Handles.color;
              Handles.color = Handles.selectedColor;
            }
            else if (id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
            {
              color1 = Handles.color;
              Handles.color = Handles.preselectionColor;
            }
            capFunction(id, position, rotation, handleSize, EventType.Repaint);
            if (id == GUIUtility.hotControl || id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
              Handles.color = color1;
            if (drawHelper && GUIUtility.hotControl == id)
            {
              Vector3[] verts = new Vector3[4];
              float num1 = handleSize * 10f;
              verts[0] = position + (slideDir1 * num1 + slideDir2 * num1);
              verts[1] = verts[0] - slideDir1 * num1 * 2f;
              verts[2] = verts[1] - slideDir2 * num1 * 2f;
              verts[3] = verts[2] + slideDir1 * num1 * 2f;
              Color color2 = Handles.color;
              Handles.color = Color.white;
              float num2 = 0.6f;
              Handles.DrawSolidRectangleWithOutline(verts, new Color(1f, 1f, 1f, 0.05f), new Color(num2, num2, num2, 0.4f));
              Handles.color = color2;
              break;
            }
            break;
          }
          break;
        case EventType.Layout:
          if (capFunction != null)
          {
            capFunction(id, position, rotation, handleSize, EventType.Layout);
            break;
          }
          HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(handlePos + offset, handleSize * 0.5f));
          break;
      }
      return vector2;
    }

    private static Vector3 GetMousePosition(Vector3 handleDirection, Vector3 handlePosition, ref bool success)
    {
      if ((UnityEngine.Object) Camera.current != (UnityEngine.Object) null)
      {
        Plane plane = new Plane(Handles.matrix.MultiplyVector(handleDirection), Handles.matrix.MultiplyPoint(handlePosition));
        Ray worldRay = HandleUtility.GUIPointToWorldRay(Slider2D.s_CurrentMousePosition);
        float enter = 0.0f;
        success = plane.Raycast(worldRay, out enter);
        return worldRay.GetPoint(enter);
      }
      success = true;
      return (Vector3) Slider2D.s_CurrentMousePosition;
    }
  }
}
