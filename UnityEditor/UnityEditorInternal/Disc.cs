// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Disc
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class Disc
  {
    private const int k_MaxSnapMarkers = 72;
    private const float k_RotationUnitSnapMajorMarkerStep = 45f;
    private const float k_RotationUnitSnapMarkerSize = 0.1f;
    private const float k_RotationUnitSnapMajorMarkerSize = 0.2f;
    private const float k_GrabZoneScale = 0.3f;
    private static Vector2 s_StartMousePosition;
    private static Vector2 s_CurrentMousePosition;
    private static Vector3 s_StartPosition;
    private static Vector3 s_StartAxis;
    private static Quaternion s_StartRotation;
    private static float s_RotationDist;

    public static Quaternion Do(int id, Quaternion rotation, Vector3 position, Vector3 axis, float size, bool cutoffPlane, float snap)
    {
      return Disc.Do(id, rotation, position, axis, size, cutoffPlane, snap, true, true, Handles.secondaryColor);
    }

    public static Quaternion Do(int id, Quaternion rotation, Vector3 position, Vector3 axis, float size, bool cutoffPlane, float snap, bool enableRayDrag, bool showHotArc, Color fillColor)
    {
      if ((double) Mathf.Abs(Vector3.Dot(Camera.current.transform.forward, axis)) > 0.999000012874603)
        cutoffPlane = false;
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && current.button == 0)
          {
            GUIUtility.hotControl = id;
            Tools.LockHandlePosition();
            if (cutoffPlane)
            {
              Vector3 normalized = Vector3.Cross(axis, Camera.current.transform.forward).normalized;
              Disc.s_StartPosition = HandleUtility.ClosestPointToArc(position, axis, normalized, 180f, size);
            }
            else
              Disc.s_StartPosition = HandleUtility.ClosestPointToDisc(position, axis, size);
            Disc.s_RotationDist = 0.0f;
            Disc.s_StartRotation = rotation;
            Disc.s_StartAxis = axis;
            Disc.s_CurrentMousePosition = Disc.s_StartMousePosition = Event.current.mousePosition;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(1);
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id && (current.button == 0 || current.button == 2))
          {
            Tools.UnlockHandlePosition();
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
            if (EditorGUI.actionKey && current.shift && enableRayDrag)
            {
              if (HandleUtility.ignoreRaySnapObjects == null)
                Handles.SetupIgnoreRaySnapObjects();
              object obj = HandleUtility.RaySnap(HandleUtility.GUIPointToWorldRay(current.mousePosition));
              if (obj != null && (double) Vector3.Dot(axis.normalized, rotation * Vector3.forward) < 0.999)
              {
                Vector3 lhs = ((RaycastHit) obj).point - position;
                rotation = Quaternion.LookRotation(lhs - Vector3.Dot(lhs, axis.normalized) * axis.normalized, rotation * Vector3.up);
              }
            }
            else
            {
              Vector3 normalized = Vector3.Cross(axis, position - Disc.s_StartPosition).normalized;
              Disc.s_CurrentMousePosition += current.delta;
              Disc.s_RotationDist = (float) ((double) HandleUtility.CalcLineTranslation(Disc.s_StartMousePosition, Disc.s_CurrentMousePosition, Disc.s_StartPosition, normalized) / (double) size * 30.0);
              Disc.s_RotationDist = Handles.SnapValue(Disc.s_RotationDist, snap);
              rotation = Quaternion.AngleAxis(Disc.s_RotationDist * -1f, Disc.s_StartAxis) * Disc.s_StartRotation;
            }
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (current.keyCode == KeyCode.Escape && GUIUtility.hotControl == id)
          {
            Tools.UnlockHandlePosition();
            EditorGUIUtility.SetWantsMouseJumping(0);
            break;
          }
          break;
        case EventType.Repaint:
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
          if (GUIUtility.hotControl == id)
          {
            Color color2 = Handles.color;
            Vector3 normalized = (Disc.s_StartPosition - position).normalized;
            Handles.color = fillColor;
            Handles.DrawLine(position, position + normalized * size);
            float angle = -Mathf.Sign(Disc.s_RotationDist) * Mathf.Repeat(Mathf.Abs(Disc.s_RotationDist), 360f);
            Vector3 vector3 = Quaternion.AngleAxis(angle, axis) * normalized;
            Handles.DrawLine(position, position + vector3 * size);
            Handles.color = fillColor * new Color(1f, 1f, 1f, 0.2f);
            int num = 0;
            for (int index = (int) Mathf.Abs(Disc.s_RotationDist * (1f / 360f)); num < index; ++num)
              Handles.DrawSolidDisc(position, axis, size);
            Handles.DrawSolidArc(position, axis, normalized, angle, size);
            if (EditorGUI.actionKey && (double) snap > 0.0)
            {
              Disc.DrawRotationUnitSnapMarkers(position, axis, size, 0.1f, snap, normalized);
              Disc.DrawRotationUnitSnapMarkers(position, axis, size, 0.2f, 45f, normalized);
            }
            Handles.color = color2;
          }
          if (showHotArc && GUIUtility.hotControl == id || GUIUtility.hotControl != id && !cutoffPlane)
            Handles.DrawWireDisc(position, axis, size);
          else if (GUIUtility.hotControl != id && cutoffPlane)
          {
            Vector3 normalized = Vector3.Cross(axis, Camera.current.transform.forward).normalized;
            Handles.DrawWireArc(position, axis, normalized, 180f, size);
          }
          if (id == GUIUtility.hotControl || id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
          {
            Handles.color = color1;
            break;
          }
          break;
        case EventType.Layout:
          float distance;
          if (cutoffPlane)
          {
            Vector3 normalized = Vector3.Cross(axis, Camera.current.transform.forward).normalized;
            distance = HandleUtility.DistanceToArc(position, axis, normalized, 180f, size) * 0.3f;
          }
          else
            distance = HandleUtility.DistanceToDisc(position, axis, size) * 0.3f;
          HandleUtility.AddControl(id, distance);
          break;
      }
      return rotation;
    }

    private static void DrawRotationUnitSnapMarkers(Vector3 position, Vector3 axis, float handleSize, float markerSize, float snap, Vector3 from)
    {
      int a = Mathf.FloorToInt(360f / snap);
      bool flag = a > 72;
      int num1 = Mathf.Min(a, 72);
      int num2 = Mathf.RoundToInt((float) num1 * 0.5f);
      for (int index = -num2; index < num2; ++index)
      {
        Vector3 vector3 = Quaternion.AngleAxis((float) index * snap, axis) * from;
        Vector3 p1 = position + (1f - markerSize) * handleSize * vector3;
        Vector3 p2 = position + 1f * handleSize * vector3;
        Handles.color = Handles.selectedColor;
        if (flag)
          Handles.color = new Color(Handles.color.r, Handles.color.g, Handles.color.b, 1f - Mathf.SmoothStep(0.0f, 1f, Mathf.Abs((float) ((double) index / ((double) num1 - 1.0) - 0.5)) * 2f));
        Handles.DrawLine(p1, p2);
      }
    }
  }
}
