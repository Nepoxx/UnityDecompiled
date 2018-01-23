// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Slider1D
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class Slider1D
  {
    private static Vector2 s_StartMousePosition;
    private static Vector2 s_CurrentMousePosition;
    private static Vector3 s_StartPosition;

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    internal static Vector3 Do(int id, Vector3 position, Vector3 direction, float size, Handles.DrawCapFunction drawFunc, float snap)
    {
      return Slider1D.Do(id, position, direction, direction, size, drawFunc, snap);
    }

    internal static Vector3 Do(int id, Vector3 position, Vector3 direction, float size, Handles.CapFunction capFunction, float snap)
    {
      return Slider1D.Do(id, position, Vector3.zero, direction, direction, size, capFunction, snap);
    }

    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    internal static Vector3 Do(int id, Vector3 position, Vector3 handleDirection, Vector3 slideDirection, float size, Handles.DrawCapFunction drawFunc, float snap)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && current.button == 0 && GUIUtility.hotControl == 0)
          {
            GUIUtility.hotControl = id;
            Slider1D.s_CurrentMousePosition = Slider1D.s_StartMousePosition = current.mousePosition;
            Slider1D.s_StartPosition = position;
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
            Slider1D.s_CurrentMousePosition += current.delta;
            float num = Handles.SnapValue(HandleUtility.CalcLineTranslation(Slider1D.s_StartMousePosition, Slider1D.s_CurrentMousePosition, Slider1D.s_StartPosition, slideDirection), snap);
            Vector3 vector3 = Handles.matrix.MultiplyVector(slideDirection);
            position = Handles.inverseMatrix.MultiplyPoint(Handles.matrix.MultiplyPoint(Slider1D.s_StartPosition) + vector3 * num);
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
          drawFunc(id, position, Quaternion.LookRotation(handleDirection), size);
          if (id == GUIUtility.hotControl || id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
          {
            Handles.color = color;
            break;
          }
          break;
        case EventType.Layout:
          Handles.DrawCapFunction drawCapFunction = drawFunc;
          // ISSUE: reference to a compiler-generated field
          if (Slider1D.\u003C\u003Ef__mg\u0024cache0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            Slider1D.\u003C\u003Ef__mg\u0024cache0 = new Handles.DrawCapFunction(Handles.ArrowCap);
          }
          // ISSUE: reference to a compiler-generated field
          Handles.DrawCapFunction fMgCache0 = Slider1D.\u003C\u003Ef__mg\u0024cache0;
          if (drawCapFunction == fMgCache0)
          {
            HandleUtility.AddControl(id, HandleUtility.DistanceToLine(position, position + slideDirection * size));
            HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(position + slideDirection * size, size * 0.2f));
            break;
          }
          HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(position, size * 0.2f));
          break;
      }
      return position;
    }

    internal static Vector3 Do(int id, Vector3 position, Vector3 offset, Vector3 handleDirection, Vector3 slideDirection, float size, Handles.CapFunction capFunction, float snap)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && current.button == 0 && GUIUtility.hotControl == 0)
          {
            GUIUtility.hotControl = id;
            Slider1D.s_CurrentMousePosition = Slider1D.s_StartMousePosition = current.mousePosition;
            Slider1D.s_StartPosition = position;
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
            Slider1D.s_CurrentMousePosition += current.delta;
            float num = Handles.SnapValue(HandleUtility.CalcLineTranslation(Slider1D.s_StartMousePosition, Slider1D.s_CurrentMousePosition, Slider1D.s_StartPosition, slideDirection), snap);
            Vector3 vector3 = Handles.matrix.MultiplyVector(slideDirection);
            position = Handles.inverseMatrix.MultiplyPoint(Handles.matrix.MultiplyPoint(Slider1D.s_StartPosition) + vector3 * num);
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
          capFunction(id, position + offset, Quaternion.LookRotation(handleDirection), size, EventType.Repaint);
          if (id == GUIUtility.hotControl || id == HandleUtility.nearestControl && GUIUtility.hotControl == 0)
          {
            Handles.color = color;
            break;
          }
          break;
        case EventType.Layout:
          if (capFunction != null)
          {
            capFunction(id, position + offset, Quaternion.LookRotation(handleDirection), size, EventType.Layout);
            break;
          }
          HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(position + offset, size * 0.2f));
          break;
      }
      return position;
    }
  }
}
