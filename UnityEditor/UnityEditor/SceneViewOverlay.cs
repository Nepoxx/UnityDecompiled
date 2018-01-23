// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneViewOverlay
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
  internal class SceneViewOverlay
  {
    private Rect m_WindowRect = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
    private float k_WindowPadding = 9f;
    private static List<SceneViewOverlay.OverlayWindow> m_Windows;
    private SceneView m_SceneView;

    public SceneViewOverlay(SceneView sceneView)
    {
      this.m_SceneView = sceneView;
      SceneViewOverlay.m_Windows = new List<SceneViewOverlay.OverlayWindow>();
    }

    public void Begin()
    {
      if (!this.m_SceneView.m_ShowSceneViewWindows)
        return;
      if (Event.current.type == EventType.Layout)
        SceneViewOverlay.m_Windows.Clear();
      this.m_SceneView.BeginWindows();
    }

    public void End()
    {
      if (!this.m_SceneView.m_ShowSceneViewWindows)
        return;
      SceneViewOverlay.m_Windows.Sort();
      if (SceneViewOverlay.m_Windows.Count > 0)
      {
        this.m_WindowRect.x = 0.0f;
        this.m_WindowRect.y = 0.0f;
        this.m_WindowRect.width = this.m_SceneView.position.width;
        this.m_WindowRect.height = this.m_SceneView.position.height;
        this.m_WindowRect = GUILayout.Window(nameof (SceneViewOverlay).GetHashCode(), this.m_WindowRect, new GUI.WindowFunction(this.WindowTrampoline), "", (GUIStyle) "SceneViewOverlayTransparentBackground", new GUILayoutOption[0]);
      }
      this.m_SceneView.EndWindows();
    }

    private void WindowTrampoline(int id)
    {
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUILayout.BeginVertical();
      GUILayout.FlexibleSpace();
      GUILayout.BeginVertical();
      float num = -this.k_WindowPadding;
      foreach (SceneViewOverlay.OverlayWindow window in SceneViewOverlay.m_Windows)
      {
        GUILayout.Space(this.k_WindowPadding + num);
        num = 0.0f;
        EditorGUIUtility.ResetGUIState();
        GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
        EditorStyles.UpdateSkinCache(1);
        GUILayout.BeginVertical(window.m_Title, GUI.skin.window, new GUILayoutOption[0]);
        window.m_SceneViewFunc(window.m_Target, this.m_SceneView);
        GUILayout.EndVertical();
      }
      EditorStyles.UpdateSkinCache();
      GUILayout.EndVertical();
      this.EatMouseInput(GUILayoutUtility.GetLastRect());
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
    }

    private void EatMouseInput(Rect position)
    {
      SceneView.AddCursorRect(position, MouseCursor.Arrow);
      int controlId = GUIUtility.GetControlID(nameof (SceneViewOverlay).GetHashCode(), FocusType.Passive, position);
      switch (Event.current.GetTypeForControl(controlId))
      {
        case EventType.MouseDown:
          if (!position.Contains(Event.current.mousePosition))
            break;
          GUIUtility.hotControl = controlId;
          Event.current.Use();
          break;
        case EventType.MouseUp:
          if (GUIUtility.hotControl != controlId)
            break;
          GUIUtility.hotControl = 0;
          Event.current.Use();
          break;
        case EventType.MouseDrag:
          if (GUIUtility.hotControl != controlId)
            break;
          Event.current.Use();
          break;
        case EventType.ScrollWheel:
          if (!position.Contains(Event.current.mousePosition))
            break;
          Event.current.Use();
          break;
      }
    }

    public static void Window(GUIContent title, SceneViewOverlay.WindowFunction sceneViewFunc, int order, SceneViewOverlay.WindowDisplayOption option)
    {
      SceneViewOverlay.Window(title, sceneViewFunc, order, (UnityEngine.Object) null, option);
    }

    public static void Window(GUIContent title, SceneViewOverlay.WindowFunction sceneViewFunc, int order, UnityEngine.Object target, SceneViewOverlay.WindowDisplayOption option)
    {
      if (Event.current.type != EventType.Layout)
        return;
      foreach (SceneViewOverlay.OverlayWindow window in SceneViewOverlay.m_Windows)
      {
        if (option == SceneViewOverlay.WindowDisplayOption.OneWindowPerTarget && window.m_Target == target && target != (UnityEngine.Object) null || option == SceneViewOverlay.WindowDisplayOption.OneWindowPerTitle && (window.m_Title == title || window.m_Title.text == title.text))
          return;
      }
      SceneViewOverlay.m_Windows.Add(new SceneViewOverlay.OverlayWindow()
      {
        m_Title = title,
        m_SceneViewFunc = sceneViewFunc,
        m_PrimaryOrder = order,
        m_SecondaryOrder = SceneViewOverlay.m_Windows.Count,
        m_Target = target
      });
    }

    public enum Ordering
    {
      Camera = -100, // -0x00000064
      ClothConstraints = 0,
      ClothSelfAndInterCollision = 100, // 0x00000064
      OcclusionCulling = 200, // 0x000000C8
      Lightmapping = 300, // 0x0000012C
      NavMesh = 400, // 0x00000190
      PhysicsDebug = 450, // 0x000001C2
      TilemapRenderer = 500, // 0x000001F4
      ParticleEffect = 600, // 0x00000258
    }

    public enum WindowDisplayOption
    {
      MultipleWindowsPerTarget,
      OneWindowPerTarget,
      OneWindowPerTitle,
    }

    private class OverlayWindow : IComparable<SceneViewOverlay.OverlayWindow>
    {
      public GUIContent m_Title;
      public SceneViewOverlay.WindowFunction m_SceneViewFunc;
      public int m_PrimaryOrder;
      public int m_SecondaryOrder;
      public UnityEngine.Object m_Target;

      public int CompareTo(SceneViewOverlay.OverlayWindow other)
      {
        int num = other.m_PrimaryOrder.CompareTo(this.m_PrimaryOrder);
        if (num == 0)
          num = other.m_SecondaryOrder.CompareTo(this.m_SecondaryOrder);
        return num;
      }
    }

    public delegate void WindowFunction(UnityEngine.Object target, SceneView sceneView);
  }
}
