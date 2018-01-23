// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneFXWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  internal class SceneFXWindow : PopupWindowContent
  {
    private static SceneFXWindow.Styles s_Styles;
    private readonly SceneView m_SceneView;
    private const float kFrameWidth = 1f;

    public SceneFXWindow(SceneView sceneView)
    {
      this.m_SceneView = sceneView;
    }

    public override Vector2 GetWindowSize()
    {
      return new Vector2(160f, 98f);
    }

    public override void OnGUI(Rect rect)
    {
      if ((UnityEngine.Object) this.m_SceneView == (UnityEngine.Object) null || this.m_SceneView.sceneViewState == null || Event.current.type == EventType.Layout)
        return;
      if (SceneFXWindow.s_Styles == null)
        SceneFXWindow.s_Styles = new SceneFXWindow.Styles();
      this.Draw(rect);
      if (Event.current.type == EventType.MouseMove)
        Event.current.Use();
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Escape)
        return;
      this.editorWindow.Close();
      GUIUtility.ExitGUI();
    }

    private void Draw(Rect rect)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SceneFXWindow.\u003CDraw\u003Ec__AnonStorey0 drawCAnonStorey0 = new SceneFXWindow.\u003CDraw\u003Ec__AnonStorey0();
      if ((UnityEngine.Object) this.m_SceneView == (UnityEngine.Object) null || this.m_SceneView.sceneViewState == null)
        return;
      Rect rect1 = new Rect(1f, 1f, rect.width - 2f, 16f);
      // ISSUE: reference to a compiler-generated field
      drawCAnonStorey0.state = this.m_SceneView.sceneViewState;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.DrawListElement(rect1, "Skybox", drawCAnonStorey0.state.showSkybox, new Action<bool>(drawCAnonStorey0.\u003C\u003Em__0));
      rect1.y += 16f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.DrawListElement(rect1, "Fog", drawCAnonStorey0.state.showFog, new Action<bool>(drawCAnonStorey0.\u003C\u003Em__1));
      rect1.y += 16f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.DrawListElement(rect1, "Flares", drawCAnonStorey0.state.showFlares, new Action<bool>(drawCAnonStorey0.\u003C\u003Em__2));
      rect1.y += 16f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.DrawListElement(rect1, "Animated Materials", drawCAnonStorey0.state.showMaterialUpdate, new Action<bool>(drawCAnonStorey0.\u003C\u003Em__3));
      rect1.y += 16f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.DrawListElement(rect1, "Image Effects", drawCAnonStorey0.state.showImageEffects, new Action<bool>(drawCAnonStorey0.\u003C\u003Em__4));
      rect1.y += 16f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.DrawListElement(rect1, "Particle Systems", drawCAnonStorey0.state.showParticleSystems, new Action<bool>(drawCAnonStorey0.\u003C\u003Em__5));
      rect1.y += 16f;
    }

    private void DrawListElement(Rect rect, string toggleName, bool value, Action<bool> setValue)
    {
      EditorGUI.BeginChangeCheck();
      bool flag = GUI.Toggle(rect, value, EditorGUIUtility.TempContent(toggleName), SceneFXWindow.s_Styles.menuItem);
      if (!EditorGUI.EndChangeCheck())
        return;
      setValue(flag);
      this.m_SceneView.Repaint();
    }

    private class Styles
    {
      public readonly GUIStyle menuItem = (GUIStyle) "MenuItem";
    }
  }
}
