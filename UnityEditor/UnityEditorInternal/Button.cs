// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.Button
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  internal class Button
  {
    [Obsolete("DrawCapFunction is obsolete. Use the version with CapFunction instead. Example: Change SphereCap to SphereHandleCap.")]
    public static bool Do(int id, Vector3 position, Quaternion direction, float size, float pickSize, Handles.DrawCapFunction capFunc)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = id;
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = 0;
            current.Use();
            if (HandleUtility.nearestControl == id)
              return true;
            break;
          }
          break;
        case EventType.MouseMove:
          if (HandleUtility.nearestControl == id && current.button == 0)
          {
            HandleUtility.Repaint();
            break;
          }
          break;
        case EventType.Repaint:
          Color color = Handles.color;
          if (HandleUtility.nearestControl == id && GUI.enabled && GUIUtility.hotControl == 0)
            Handles.color = Handles.preselectionColor;
          capFunc(id, position, direction, size);
          Handles.color = color;
          break;
        case EventType.Layout:
          if (GUI.enabled)
          {
            HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(position, pickSize));
            break;
          }
          break;
      }
      return false;
    }

    public static bool Do(int id, Vector3 position, Quaternion direction, float size, float pickSize, Handles.CapFunction capFunction)
    {
      Event current = Event.current;
      switch (current.GetTypeForControl(id))
      {
        case EventType.MouseDown:
          if (HandleUtility.nearestControl == id && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = id;
            current.Use();
            break;
          }
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl == id && (current.button == 0 || current.button == 2))
          {
            GUIUtility.hotControl = 0;
            current.Use();
            if (HandleUtility.nearestControl == id)
              return true;
            break;
          }
          break;
        case EventType.MouseMove:
          if (HandleUtility.nearestControl == id && current.button == 0)
          {
            HandleUtility.Repaint();
            break;
          }
          break;
        case EventType.Repaint:
          Color color = Handles.color;
          if (HandleUtility.nearestControl == id && GUI.enabled && GUIUtility.hotControl == 0)
            Handles.color = Handles.preselectionColor;
          capFunction(id, position, direction, size, EventType.Repaint);
          Handles.color = color;
          break;
        case EventType.Layout:
          if (GUI.enabled)
          {
            capFunction(id, position, direction, pickSize, EventType.Layout);
            break;
          }
          break;
      }
      return false;
    }
  }
}
