// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.SliderScale
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class SliderScale
  {
    private static float s_ScaleDrawLength = 1f;
    private static float s_StartScale;
    private static float s_ValueDrag;
    private static Vector2 s_StartMousePosition;
    private static Vector2 s_CurrentMousePosition;

    public static float DoAxis(int id, float scale, Vector3 position, Vector3 direction, Quaternion rotation, float size, float snap)
    {
      return SliderScale.DoAxis(id, scale, position, direction, rotation, size, snap, 0.0f, 1f);
    }

    internal static float DoAxis(int id, float scale, Vector3 position, Vector3 direction, Quaternion rotation, float size, float snap, float handleOffset, float lineScale)
    {
      Vector3 vector3_1 = direction * size * handleOffset;
      float num1 = GUIUtility.hotControl != id ? size : size * scale / SliderScale.s_StartScale;
      Vector3 p1 = position + vector3_1;
      Vector3 vector3_2 = position + direction * (num1 * SliderScale.s_ScaleDrawLength * lineScale) + vector3_1;
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && current.button == 0)
          {
            GUIUtility.hotControl = id;
            SliderScale.s_CurrentMousePosition = SliderScale.s_StartMousePosition = current.mousePosition;
            SliderScale.s_StartScale = scale;
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
            SliderScale.s_CurrentMousePosition += current.delta;
            float num2 = Handles.SnapValue((float) (1.0 + (double) HandleUtility.CalcLineTranslation(SliderScale.s_StartMousePosition, SliderScale.s_CurrentMousePosition, position, direction) / (double) size), snap);
            scale = SliderScale.s_StartScale * num2;
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          Color color = Color.white;
          if (id == GUIUtility.hotControl)
          {
            color = Handles.color;
            Handles.color = Handles.selectedColor;
          }
          else if (id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
          {
            color = Handles.color;
            Handles.color = Handles.preselectionColor;
          }
          Handles.DrawLine(p1, vector3_2);
          Handles.CubeHandleCap(id, vector3_2, rotation, size * 0.1f, EventType.Repaint);
          if (id == GUIUtility.hotControl || id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
          {
            Handles.color = color;
            break;
          }
          break;
        case EventType.Layout:
          HandleUtility.AddControl(id, HandleUtility.DistanceToLine(p1, vector3_2));
          HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(vector3_2, size * 0.3f));
          break;
      }
      return scale;
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static float DoCenter(int id, float value, Vector3 position, Quaternion rotation, float size, Handles.DrawCapFunction capFunc, float snap)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && current.button == 0)
          {
            GUIUtility.hotControl = id;
            SliderScale.s_StartScale = value;
            SliderScale.s_ValueDrag = 0.0f;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(1);
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = 0;
            SliderScale.s_ScaleDrawLength = 1f;
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
            SliderScale.s_ValueDrag += HandleUtility.niceMouseDelta * 0.01f;
            value = (Handles.SnapValue(SliderScale.s_ValueDrag, snap) + 1f) * SliderScale.s_StartScale;
            SliderScale.s_ScaleDrawLength = value / SliderScale.s_StartScale;
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.hotControl == id && current.keyCode == KeyCode.Escape)
          {
            value = SliderScale.s_StartScale;
            SliderScale.s_ScaleDrawLength = 1f;
            GUIUtility.hotControl = 0;
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          Color color = Color.white;
          if (id == GUIUtility.hotControl)
          {
            color = Handles.color;
            Handles.color = Handles.selectedColor;
          }
          else if (id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
          {
            color = Handles.color;
            Handles.color = Handles.preselectionColor;
          }
          capFunc(id, position, rotation, size * 0.15f);
          if (id == GUIUtility.hotControl || id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
          {
            Handles.color = color;
            break;
          }
          break;
        case EventType.Layout:
          HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(position, size * 0.15f));
          break;
      }
      return value;
    }

    public static float DoCenter(int id, float value, Vector3 position, Quaternion rotation, float size, Handles.CapFunction capFunction, float snap)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && current.button == 0)
          {
            GUIUtility.hotControl = id;
            SliderScale.s_StartScale = value;
            SliderScale.s_ValueDrag = 0.0f;
            current.Use();
            EditorGUIUtility.SetWantsMouseJumping(1);
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = 0;
            SliderScale.s_ScaleDrawLength = 1f;
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
            SliderScale.s_ValueDrag += HandleUtility.niceMouseDelta * 0.01f;
            value = (Handles.SnapValue(SliderScale.s_ValueDrag, snap) + 1f) * SliderScale.s_StartScale;
            SliderScale.s_ScaleDrawLength = value / SliderScale.s_StartScale;
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.KeyDown:
          if (GUIUtility.hotControl == id && current.keyCode == KeyCode.Escape)
          {
            value = SliderScale.s_StartScale;
            SliderScale.s_ScaleDrawLength = 1f;
            GUIUtility.hotControl = 0;
            GUI.changed = true;
            current.Use();
            break;
          }
          break;
        case EventType.Repaint:
          Color color = Color.white;
          if (id == GUIUtility.hotControl)
          {
            color = Handles.color;
            Handles.color = Handles.selectedColor;
          }
          else if (id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
          {
            color = Handles.color;
            Handles.color = Handles.preselectionColor;
          }
          capFunction(id, position, rotation, size * 0.15f, EventType.Repaint);
          if (id == GUIUtility.hotControl || id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
          {
            Handles.color = color;
            break;
          }
          break;
        case EventType.Layout:
          capFunction(id, position, rotation, size * 0.15f, EventType.Layout);
          break;
      }
      return value;
    }
  }
}
