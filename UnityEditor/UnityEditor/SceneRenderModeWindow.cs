// Decompiled with JetBrains decompiler
// Type: UnityEditor.SceneRenderModeWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class SceneRenderModeWindow : PopupWindowContent
  {
    private static readonly int sRenderModeCount = SceneRenderModeWindow.Styles.sRenderModeOptions.Length;
    private static readonly int sMenuRowCount = SceneRenderModeWindow.sRenderModeCount + 7;
    private readonly float m_WindowHeight = (float) ((double) SceneRenderModeWindow.sMenuRowCount * 16.0 + 15.0 + 22.0);
    private const float m_WindowWidth = 205f;
    private const int kMenuHeaderCount = 7;
    private const float kSeparatorHeight = 3f;
    private const float kFrameWidth = 1f;
    private const float kHeaderHorizontalPadding = 5f;
    private const float kHeaderVerticalPadding = 1f;
    private const float kShowLightmapResolutionHeight = 22f;
    private const float kTogglePadding = 7f;
    private readonly SceneView m_SceneView;

    public SceneRenderModeWindow(SceneView sceneView)
    {
      this.m_SceneView = sceneView;
    }

    public override Vector2 GetWindowSize()
    {
      return new Vector2(205f, this.m_WindowHeight);
    }

    public override void OnGUI(Rect rect)
    {
      if ((Object) this.m_SceneView == (Object) null || this.m_SceneView.sceneViewState == null || Event.current.type == EventType.Layout)
        return;
      this.Draw(this.editorWindow, rect.width);
      if (Event.current.type == EventType.MouseMove)
        Event.current.Use();
      if (Event.current.type != EventType.KeyDown || Event.current.keyCode != KeyCode.Escape)
        return;
      this.editorWindow.Close();
      GUIUtility.ExitGUI();
    }

    private void DrawSeparator(ref Rect rect)
    {
      Rect position = rect;
      position.x += 5f;
      position.y += 3f;
      position.width -= 10f;
      position.height = 3f;
      GUI.Label(position, GUIContent.none, SceneRenderModeWindow.Styles.sSeparator);
      rect.y += 3f;
    }

    private void DrawHeader(ref Rect rect, GUIContent label)
    {
      Rect position = rect;
      ++position.y;
      position.x += 5f;
      position.width = EditorStyles.miniLabel.CalcSize(label).x;
      position.height = EditorStyles.miniLabel.CalcSize(label).y;
      GUI.Label(position, label, EditorStyles.miniLabel);
      rect.y += 16f;
    }

    private void Draw(EditorWindow caller, float listElementWidth)
    {
      Rect rect = new Rect(0.0f, 0.0f, listElementWidth, 16f);
      this.DrawHeader(ref rect, SceneRenderModeWindow.Styles.sShadedHeader);
      for (int index = 0; index < SceneRenderModeWindow.sRenderModeCount; ++index)
      {
        DrawCameraMode drawCameraMode = SceneRenderModeWindow.Styles.sRenderModeUIOrder[index];
        switch (drawCameraMode)
        {
          case DrawCameraMode.Systems:
            this.DrawSeparator(ref rect);
            this.DrawHeader(ref rect, SceneRenderModeWindow.Styles.sGlobalIlluminationHeader);
            break;
          case DrawCameraMode.RealtimeAlbedo:
            this.DrawSeparator(ref rect);
            this.DrawHeader(ref rect, SceneRenderModeWindow.Styles.sRealtimeGIHeader);
            break;
          case DrawCameraMode.BakedLightmap:
            this.DrawSeparator(ref rect);
            this.DrawHeader(ref rect, SceneRenderModeWindow.Styles.sBakedGIHeader);
            break;
          default:
            if (drawCameraMode != DrawCameraMode.ShadowCascades)
            {
              if (drawCameraMode != DrawCameraMode.DeferredDiffuse)
              {
                if (drawCameraMode == DrawCameraMode.ValidateAlbedo)
                {
                  this.DrawSeparator(ref rect);
                  this.DrawHeader(ref rect, SceneRenderModeWindow.Styles.sMaterialValidationHeader);
                  break;
                }
                break;
              }
              this.DrawSeparator(ref rect);
              this.DrawHeader(ref rect, SceneRenderModeWindow.Styles.sDeferredHeader);
              break;
            }
            this.DrawSeparator(ref rect);
            this.DrawHeader(ref rect, SceneRenderModeWindow.Styles.sMiscellaneous);
            break;
        }
        using (new EditorGUI.DisabledScope(!this.IsModeEnabled(drawCameraMode)))
          this.DoOneMode(caller, ref rect, drawCameraMode);
      }
      bool disabled = this.m_SceneView.renderMode < DrawCameraMode.RealtimeCharting || !this.IsModeEnabled(this.m_SceneView.renderMode);
      this.DoResolutionToggle(rect, disabled);
    }

    private bool IsModeEnabled(DrawCameraMode mode)
    {
      return this.m_SceneView.IsCameraDrawModeEnabled(mode);
    }

    private void DoResolutionToggle(Rect rect, bool disabled)
    {
      GUI.Label(new Rect(1f, rect.y, 203f, 22f), "", EditorStyles.inspectorBig);
      rect.y += 3f;
      rect.x += 7f;
      using (new EditorGUI.DisabledScope(disabled))
      {
        EditorGUI.BeginChangeCheck();
        bool flag = GUI.Toggle(rect, LightmapVisualization.showResolution, SceneRenderModeWindow.Styles.sResolutionToggle);
        if (!EditorGUI.EndChangeCheck())
          return;
        LightmapVisualization.showResolution = flag;
        SceneView.RepaintAll();
      }
    }

    private void DoOneMode(EditorWindow caller, ref Rect rect, DrawCameraMode drawCameraMode)
    {
      using (new EditorGUI.DisabledScope(!this.m_SceneView.CheckDrawModeForRenderingPath(drawCameraMode)))
      {
        EditorGUI.BeginChangeCheck();
        GUI.Toggle(rect, this.m_SceneView.renderMode == drawCameraMode, SceneRenderModeWindow.GetGUIContent(drawCameraMode), SceneRenderModeWindow.Styles.sMenuItem);
        if (EditorGUI.EndChangeCheck())
        {
          this.m_SceneView.renderMode = drawCameraMode;
          this.m_SceneView.Repaint();
          GUIUtility.ExitGUI();
        }
        rect.y += 16f;
      }
    }

    public static GUIContent GetGUIContent(DrawCameraMode drawCameraMode)
    {
      return SceneRenderModeWindow.Styles.sRenderModeOptions[(int) drawCameraMode];
    }

    private class Styles
    {
      public static readonly GUIStyle sMenuItem = (GUIStyle) "MenuItem";
      public static readonly GUIStyle sSeparator = (GUIStyle) "sv_iconselector_sep";
      public static readonly GUIContent sShadedHeader = EditorGUIUtility.TextContent("Shading Mode");
      public static readonly GUIContent sMiscellaneous = EditorGUIUtility.TextContent("Miscellaneous");
      public static readonly GUIContent sDeferredHeader = EditorGUIUtility.TextContent("Deferred");
      public static readonly GUIContent sGlobalIlluminationHeader = EditorGUIUtility.TextContent("Global Illumination");
      public static readonly GUIContent sRealtimeGIHeader = EditorGUIUtility.TextContent("Realtime Global Illumination");
      public static readonly GUIContent sBakedGIHeader = EditorGUIUtility.TextContent("Baked Global Illumination");
      public static readonly GUIContent sMaterialValidationHeader = EditorGUIUtility.TextContent("Material Validation");
      public static readonly GUIContent sResolutionToggle = EditorGUIUtility.TextContent("Show Lightmap Resolution");
      public static DrawCameraMode[] sRenderModeUIOrder = new DrawCameraMode[32]{ DrawCameraMode.Textured, DrawCameraMode.Wireframe, DrawCameraMode.TexturedWire, DrawCameraMode.ShadowCascades, DrawCameraMode.RenderPaths, DrawCameraMode.AlphaChannel, DrawCameraMode.Overdraw, DrawCameraMode.Mipmaps, DrawCameraMode.SpriteMask, DrawCameraMode.DeferredDiffuse, DrawCameraMode.DeferredSpecular, DrawCameraMode.DeferredSmoothness, DrawCameraMode.DeferredNormal, DrawCameraMode.Systems, DrawCameraMode.Clustering, DrawCameraMode.LitClustering, DrawCameraMode.RealtimeCharting, DrawCameraMode.RealtimeAlbedo, DrawCameraMode.RealtimeEmissive, DrawCameraMode.RealtimeIndirect, DrawCameraMode.RealtimeDirectionality, DrawCameraMode.BakedLightmap, DrawCameraMode.BakedDirectionality, DrawCameraMode.ShadowMasks, DrawCameraMode.BakedAlbedo, DrawCameraMode.BakedEmissive, DrawCameraMode.BakedCharting, DrawCameraMode.BakedTexelValidity, DrawCameraMode.BakedIndices, DrawCameraMode.LightOverlap, DrawCameraMode.ValidateAlbedo, DrawCameraMode.ValidateMetalSpecular };
      public static readonly GUIContent[] sRenderModeOptions = new GUIContent[32]{ EditorGUIUtility.TextContent("Shaded"), EditorGUIUtility.TextContent("Wireframe"), EditorGUIUtility.TextContent("Shaded Wireframe"), EditorGUIUtility.TextContent("Shadow Cascades"), EditorGUIUtility.TextContent("Render Paths"), EditorGUIUtility.TextContent("Alpha Channel"), EditorGUIUtility.TextContent("Overdraw"), EditorGUIUtility.TextContent("Mipmaps"), EditorGUIUtility.TextContent("Albedo"), EditorGUIUtility.TextContent("Specular"), EditorGUIUtility.TextContent("Smoothness"), EditorGUIUtility.TextContent("Normal"), EditorGUIUtility.TextContent("UV Charts"), EditorGUIUtility.TextContent("Systems"), EditorGUIUtility.TextContent("Albedo"), EditorGUIUtility.TextContent("Emissive"), EditorGUIUtility.TextContent("Indirect"), EditorGUIUtility.TextContent("Directionality"), EditorGUIUtility.TextContent("Baked Lightmap"), EditorGUIUtility.TextContent("Clustering"), EditorGUIUtility.TextContent("Lit Clustering"), EditorGUIUtility.TextContent("Validate Albedo"), EditorGUIUtility.TextContent("Validate Metal Specular"), EditorGUIUtility.TextContent("Shadowmask"), EditorGUIUtility.TextContent("Light Overlap"), EditorGUIUtility.TextContent("Albedo"), EditorGUIUtility.TextContent("Emissive"), EditorGUIUtility.TextContent("Directionality"), EditorGUIUtility.TextContent("Texel Validity"), EditorGUIUtility.TextContent("Lightmap Indices"), EditorGUIUtility.TextContent("UV Charts"), EditorGUIUtility.TextContent("Sprite Mask") };
    }
  }
}
