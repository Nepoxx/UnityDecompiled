// Decompiled with JetBrains decompiler
// Type: UnityEditor.LookDevEnvironmentWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UnityEditor
{
  internal class LookDevEnvironmentWindow
  {
    private static LookDevEnvironmentWindow.Styles s_Styles = new LookDevEnvironmentWindow.Styles();
    private Vector2 m_ScrollPosition = new Vector2(0.0f, 0.0f);
    private Cubemap m_SelectedCubemap = (Cubemap) null;
    private CubemapInfo m_SelectedCubemapInfo = (CubemapInfo) null;
    private CubemapInfo m_SelectedShadowCubemapOwnerInfo = (CubemapInfo) null;
    private int m_SelectedLightIconIndex = -1;
    private ShadowInfo m_SelectedShadowInfo = (ShadowInfo) null;
    private bool m_RenderOverlayThumbnailOnce = false;
    private int m_SelectedCubeMapOffsetIndex = -1;
    private int m_HoveringCubeMapIndex = -1;
    private float m_SelectedCubeMapOffsetValue = 0.0f;
    private Vector2 m_SelectedPositionOffset = new Vector2(0.0f, 0.0f);
    private bool m_DragBeingPerformed = false;
    private LookDevView m_LookDevView;
    private Rect m_PositionInLookDev;
    private Rect m_GUIRect;
    private Rect m_displayRect;
    public const float m_latLongHeight = 125f;
    public const float m_HDRIHeaderHeight = 18f;
    public const float m_HDRIHeight = 146f;
    public const float m_HDRIWidth = 250f;
    private const float kButtonWidth = 16f;
    private const float kButtonHeight = 16f;

    public LookDevEnvironmentWindow(LookDevView lookDevView)
    {
      this.m_LookDevView = lookDevView;
    }

    public static LookDevEnvironmentWindow.Styles styles
    {
      get
      {
        if (LookDevEnvironmentWindow.s_Styles == null)
          LookDevEnvironmentWindow.s_Styles = new LookDevEnvironmentWindow.Styles();
        return LookDevEnvironmentWindow.s_Styles;
      }
    }

    public void SetRects(Rect positionInLookDev, Rect GUIRect, Rect displayRect)
    {
      this.m_PositionInLookDev = positionInLookDev;
      this.m_GUIRect = GUIRect;
      this.m_displayRect = displayRect;
    }

    public Cubemap GetCurrentSelection()
    {
      return this.m_SelectedCubemap;
    }

    public Vector2 GetSelectedPositionOffset()
    {
      return this.m_SelectedPositionOffset;
    }

    public void CancelSelection()
    {
      this.m_SelectedCubemap = (Cubemap) null;
      this.m_SelectedCubemapInfo = (CubemapInfo) null;
      this.m_SelectedShadowCubemapOwnerInfo = (CubemapInfo) null;
      this.m_SelectedLightIconIndex = -1;
      this.m_SelectedShadowInfo = (ShadowInfo) null;
      this.m_HoveringCubeMapIndex = -1;
      this.m_DragBeingPerformed = false;
    }

    private float ComputeAngleOffsetFromMouseCoord(Vector2 mousePosition)
    {
      return (float) ((double) mousePosition.x / 250.0 * 360.0);
    }

    private Vector2 LatLongToPosition(float latitude, float longitude)
    {
      latitude = (float) (((double) latitude + 90.0) % 180.0 - 90.0);
      if ((double) latitude < -90.0)
        latitude = -90f;
      if ((double) latitude > 89.0)
        latitude = 89f;
      longitude %= 360f;
      if ((double) longitude < 0.0)
        longitude = 360f + longitude;
      return new Vector2((float) ((double) longitude * (Math.PI / 180.0) / 6.28318548202515 * 2.0 - 1.0), (float) ((double) latitude * (Math.PI / 180.0) / 1.57079637050629));
    }

    public static Vector2 PositionToLatLong(Vector2 position)
    {
      Vector2 vector2 = new Vector2();
      vector2.x = (float) ((double) position.y * 3.14159274101257 * 0.5 * 57.2957801818848);
      vector2.y = (float) (((double) position.x * 0.5 + 0.5) * 2.0 * 3.14159274101257 * 57.2957801818848);
      if ((double) vector2.x < -90.0)
        vector2.x = -90f;
      if ((double) vector2.x > 89.0)
        vector2.x = 89f;
      return vector2;
    }

    private Rect GetInsertionRect(int envIndex)
    {
      Rect guiRect = this.m_GUIRect;
      guiRect.height = 21f;
      guiRect.y = 146f * (float) envIndex;
      return guiRect;
    }

    private int IsPositionInInsertionArea(Vector2 pos)
    {
      for (int envIndex = 0; envIndex <= this.m_LookDevView.envLibrary.hdriCount; ++envIndex)
      {
        if (this.GetInsertionRect(envIndex).Contains(pos))
          return envIndex;
      }
      return -1;
    }

    private Rect GetThumbnailRect(int envIndex)
    {
      Rect guiRect = this.m_GUIRect;
      guiRect.height = 125f;
      guiRect.y = (float) ((double) envIndex * 146.0 + 18.0);
      return guiRect;
    }

    private int IsPositionInThumbnailArea(Vector2 pos)
    {
      for (int envIndex = 0; envIndex < this.m_LookDevView.envLibrary.hdriCount; ++envIndex)
      {
        if (this.GetThumbnailRect(envIndex).Contains(pos))
          return envIndex;
      }
      return -1;
    }

    private void RenderOverlayThumbnailIfNeeded()
    {
      if (!this.m_RenderOverlayThumbnailOnce || Event.current.type != EventType.Repaint || this.m_SelectedCubemapInfo == null)
        return;
      this.m_SelectedCubemap = this.m_SelectedCubemapInfo.cubemap;
      RenderTexture active = RenderTexture.active;
      RenderTexture.active = LookDevResources.m_SelectionTexture;
      LookDevResources.m_LookDevCubeToLatlong.SetTexture("_MainTex", (Texture) this.m_SelectedCubemap);
      LookDevResources.m_LookDevCubeToLatlong.SetVector("_WindowParams", new Vector4(this.m_displayRect.height, -1000f, 2f, 1f));
      LookDevResources.m_LookDevCubeToLatlong.SetVector("_CubeToLatLongParams", new Vector4((float) Math.PI / 180f * this.m_SelectedCubemapInfo.angleOffset, 0.5f, 1f, 0.0f));
      LookDevResources.m_LookDevCubeToLatlong.SetPass(0);
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      LookDevView.DrawFullScreenQuad(new Rect(0.0f, 0.0f, 250f, 125f));
      GL.sRGBWrite = false;
      RenderTexture.active = active;
      this.m_RenderOverlayThumbnailOnce = false;
    }

    private void DrawLatLongThumbnail(CubemapInfo infos, float angleOffset, float intensity, float alpha, Rect textureRect)
    {
      GUIStyle guiStyle = (GUIStyle) "dockarea";
      LookDevResources.m_LookDevCubeToLatlong.SetVector("_WindowParams", new Vector4(this.m_displayRect.height, this.m_PositionInLookDev.y + 17f, (float) guiStyle.margin.top, EditorGUIUtility.pixelsPerPoint));
      LookDevResources.m_LookDevCubeToLatlong.SetVector("_CubeToLatLongParams", new Vector4((float) Math.PI / 180f * angleOffset, alpha, intensity, 0.0f));
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      Graphics.DrawTexture(textureRect, (Texture) infos.cubemap, LookDevResources.m_LookDevCubeToLatlong, 1);
      GL.sRGBWrite = false;
    }

    private void DrawSelectionFeedback(Rect textureRect, Color selectionColor1, Color selectionColor2)
    {
      float x1 = 0.5f;
      float x2 = textureRect.width - 0.5f;
      float y1 = textureRect.y + 0.5f;
      float y2 = (float) ((double) textureRect.y + (double) textureRect.height - 1.0);
      float x3 = textureRect.width * 0.5f;
      Vector3[] lineSegments1 = new Vector3[6]{ new Vector3(x3, y1, 0.0f), new Vector3(x1, y1, 0.0f), new Vector3(x1, y1, 0.0f), new Vector3(x1, y2, 0.0f), new Vector3(x1, y2, 0.0f), new Vector3(x3, y2, 0.0f) };
      Vector3[] lineSegments2 = new Vector3[6]{ new Vector3(x3, y1, 0.0f), new Vector3(x2, y1, 0.0f), new Vector3(x2, y1, 0.0f), new Vector3(x2, y2, 0.0f), new Vector3(x2, y2, 0.0f), new Vector3(x3, y2, 0.0f) };
      Handles.color = selectionColor1;
      Handles.DrawLines(lineSegments1);
      Handles.color = selectionColor2;
      Handles.DrawLines(lineSegments2);
    }

    public void ResetShadowCubemap()
    {
      if (this.m_SelectedShadowCubemapOwnerInfo == null)
        return;
      this.m_SelectedShadowCubemapOwnerInfo.SetCubemapShadowInfo(this.m_SelectedShadowCubemapOwnerInfo);
    }

    private void HandleMouseInput()
    {
      List<CubemapInfo> hdriList = this.m_LookDevView.envLibrary.hdriList;
      Vector2 vector2 = new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y + this.m_ScrollPosition.y);
      Event current = Event.current;
      EventType typeForControl = current.GetTypeForControl(this.m_LookDevView.hotControl);
      switch (typeForControl)
      {
        case EventType.MouseUp:
          if ((UnityEngine.Object) this.m_SelectedCubemap != (UnityEngine.Object) null)
          {
            Rect guiRect = this.m_GUIRect;
            guiRect.yMax += 16f;
            if (guiRect.Contains(Event.current.mousePosition))
            {
              int insertionIndex = this.IsPositionInInsertionArea(vector2);
              if (insertionIndex != -1)
              {
                this.ResetShadowCubemap();
                this.m_LookDevView.envLibrary.InsertHDRI(this.m_SelectedCubemap, insertionIndex);
              }
              else
              {
                int index = this.IsPositionInThumbnailArea(vector2);
                if (index != -1 && this.m_LookDevView.config.enableShadowCubemap)
                {
                  Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.envLibrary, "Update shadow cubemap");
                  CubemapInfo hdri = this.m_LookDevView.envLibrary.hdriList[index];
                  if (hdri != this.m_SelectedCubemapInfo)
                    hdri.SetCubemapShadowInfo(this.m_SelectedCubemapInfo);
                  this.m_LookDevView.envLibrary.dirty = true;
                }
              }
              this.CancelSelection();
            }
          }
          this.m_LookDevView.Repaint();
          if (this.m_SelectedCubeMapOffsetIndex != -1 && (double) Mathf.Abs(hdriList[this.m_SelectedCubeMapOffsetIndex].angleOffset) <= 10.0)
          {
            Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.envLibrary, "");
            hdriList[this.m_SelectedCubeMapOffsetIndex].angleOffset = 0.0f;
            this.m_LookDevView.envLibrary.dirty = true;
          }
          this.m_SelectedCubemapInfo = (CubemapInfo) null;
          this.m_SelectedShadowCubemapOwnerInfo = (CubemapInfo) null;
          this.m_SelectedLightIconIndex = -1;
          this.m_SelectedShadowInfo = (ShadowInfo) null;
          this.m_SelectedCubeMapOffsetIndex = -1;
          this.m_HoveringCubeMapIndex = -1;
          this.m_SelectedCubeMapOffsetValue = 0.0f;
          GUIUtility.hotControl = 0;
          break;
        case EventType.MouseDrag:
          if (this.m_SelectedCubeMapOffsetIndex != -1)
          {
            Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.envLibrary, "");
            hdriList[this.m_SelectedCubeMapOffsetIndex].angleOffset = this.ComputeAngleOffsetFromMouseCoord(vector2) + this.m_SelectedCubeMapOffsetValue;
            this.m_LookDevView.envLibrary.dirty = true;
            Event.current.Use();
          }
          if (this.m_SelectedCubemapInfo != null)
            this.m_HoveringCubeMapIndex = this.IsPositionInInsertionArea(vector2) != -1 ? -1 : this.IsPositionInThumbnailArea(vector2);
          this.m_LookDevView.Repaint();
          break;
        case EventType.KeyDown:
          if (Event.current.keyCode != KeyCode.Escape)
            break;
          this.CancelSelection();
          this.m_LookDevView.Repaint();
          break;
        case EventType.Repaint:
          if (this.m_SelectedCubeMapOffsetIndex == -1)
            break;
          EditorGUIUtility.AddCursorRect(this.m_displayRect, MouseCursor.SlideArrow);
          break;
        case EventType.DragUpdated:
          bool flag = false;
          foreach (UnityEngine.Object objectReference in DragAndDrop.objectReferences)
          {
            if ((bool) ((UnityEngine.Object) (objectReference as Cubemap)))
              flag = true;
          }
          DragAndDrop.visualMode = !flag ? DragAndDropVisualMode.Rejected : DragAndDropVisualMode.Link;
          if (flag)
            this.m_DragBeingPerformed = true;
          current.Use();
          break;
        case EventType.DragPerform:
          int insertionIndex1 = this.IsPositionInInsertionArea(vector2);
          foreach (UnityEngine.Object objectReference in DragAndDrop.objectReferences)
          {
            Cubemap cubemap = objectReference as Cubemap;
            if ((bool) ((UnityEngine.Object) cubemap))
              this.m_LookDevView.envLibrary.InsertHDRI(cubemap, insertionIndex1);
          }
          DragAndDrop.AcceptDrag();
          this.m_DragBeingPerformed = false;
          current.Use();
          break;
        default:
          if (typeForControl != EventType.DragExited)
            break;
          break;
      }
    }

    private void GetFrameAndShadowTextureRect(Rect textureRect, out Rect frameTextureRect, out Rect shadowTextureRect)
    {
      frameTextureRect = textureRect;
      frameTextureRect.x += textureRect.width - (float) LookDevEnvironmentWindow.styles.sLatlongFrameTexture.width;
      frameTextureRect.y += textureRect.height - (float) LookDevEnvironmentWindow.styles.sLatlongFrameTexture.height * 1.05f;
      frameTextureRect.width = (float) LookDevEnvironmentWindow.styles.sLatlongFrameTexture.width;
      frameTextureRect.height = (float) LookDevEnvironmentWindow.styles.sLatlongFrameTexture.height;
      shadowTextureRect = frameTextureRect;
      shadowTextureRect.x += 6f;
      shadowTextureRect.y += 4f;
      shadowTextureRect.width = 105f;
      shadowTextureRect.height = 52f;
    }

    public void OnGUI(int windowID)
    {
      if ((UnityEngine.Object) this.m_LookDevView == (UnityEngine.Object) null)
        return;
      List<CubemapInfo> hdriList = this.m_LookDevView.envLibrary.hdriList;
      bool flag1 = 146.0 * (double) hdriList.Count > (double) this.m_PositionInLookDev.height;
      this.m_ScrollPosition = !flag1 ? new Vector2(0.0f, 0.0f) : EditorGUILayout.BeginScrollView(this.m_ScrollPosition);
      if (hdriList.Count == 1)
      {
        Color color = GUI.color;
        GUI.color = Color.gray;
        Vector2 vector2 = GUI.skin.label.CalcSize(LookDevEnvironmentWindow.styles.sDragAndDropHDRIText);
        GUI.Label(new Rect((float) ((double) this.m_PositionInLookDev.width * 0.5 - (double) vector2.x * 0.5), (float) ((double) this.m_PositionInLookDev.height * 0.5 - (double) vector2.y * 0.5), vector2.x, vector2.y), LookDevEnvironmentWindow.styles.sDragAndDropHDRIText);
        GUI.color = color;
      }
      for (int envIndex = 0; envIndex < hdriList.Count; ++envIndex)
      {
        CubemapInfo infos1 = hdriList[envIndex];
        ShadowInfo shadowInfo = infos1.shadowInfo;
        int intProperty = this.m_LookDevView.config.GetIntProperty(LookDevProperty.HDRI, LookDevEditionContext.Left);
        int num1 = this.m_LookDevView.config.GetIntProperty(LookDevProperty.HDRI, LookDevEditionContext.Right);
        if (this.m_LookDevView.config.lookDevMode == LookDevMode.Single1 || this.m_LookDevView.config.lookDevMode == LookDevMode.Single2)
          num1 = -1;
        bool flag2 = envIndex == intProperty || envIndex == num1;
        Color selectionColor1 = Color.black;
        Color selectionColor2 = Color.black;
        GUIStyle style = EditorStyles.miniLabel;
        if (flag2)
        {
          if (envIndex == intProperty)
          {
            selectionColor1 = (Color) LookDevView.m_FirstViewGizmoColor;
            selectionColor2 = (Color) LookDevView.m_FirstViewGizmoColor;
            style = LookDevEnvironmentWindow.styles.sLabelStyleFirstContext;
          }
          else if (envIndex == num1)
          {
            selectionColor1 = (Color) LookDevView.m_SecondViewGizmoColor;
            selectionColor2 = (Color) LookDevView.m_SecondViewGizmoColor;
            style = LookDevEnvironmentWindow.styles.sLabelStyleSecondContext;
          }
          if (intProperty == num1)
          {
            selectionColor1 = (Color) LookDevView.m_FirstViewGizmoColor;
            selectionColor2 = (Color) LookDevView.m_SecondViewGizmoColor;
            style = LookDevEnvironmentWindow.styles.sLabelStyleBothContext;
          }
        }
        GUILayout.BeginVertical(GUILayout.Width(250f));
        int index = hdriList.FindIndex((Predicate<CubemapInfo>) (x => x == this.m_SelectedCubemapInfo));
        if (((UnityEngine.Object) this.m_SelectedCubemap != (UnityEngine.Object) null || this.m_DragBeingPerformed) && (this.GetInsertionRect(envIndex).Contains(Event.current.mousePosition) && (index - envIndex != 0 && index - envIndex != -1 || index == -1)))
        {
          GUILayout.Label(GUIContent.none, LookDevEnvironmentWindow.styles.sSeparatorStyle, new GUILayoutOption[0]);
          GUILayoutUtility.GetRect(250f, 16f);
        }
        GUILayout.Label(GUIContent.none, LookDevEnvironmentWindow.styles.sSeparatorStyle, new GUILayoutOption[0]);
        GUILayout.BeginHorizontal(new GUILayoutOption[2]
        {
          GUILayout.Width(250f),
          GUILayout.Height(18f)
        });
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(envIndex.ToString());
        stringBuilder.Append(" - ");
        stringBuilder.Append(infos1.cubemap.name);
        GUILayout.Label(stringBuilder.ToString(), style, GUILayout.Height(18f), GUILayout.MaxWidth(175f));
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(LookDevEnvironmentWindow.styles.sEnvControlIcon, LookDevView.styles.sToolBarButton, new GUILayoutOption[0]))
        {
          PopupWindow.Show(GUILayoutUtility.topLevel.GetLast(), (PopupWindowContent) new LookDevEnvironmentWindow.EnvSettingsWindow(this.m_LookDevView, infos1));
          GUIUtility.ExitGUI();
        }
        using (new EditorGUI.DisabledScope((UnityEngine.Object) infos1.cubemap == (UnityEngine.Object) LookDevResources.m_DefaultHDRI))
        {
          if (GUILayout.Button(LookDevEnvironmentWindow.styles.sCloseIcon, LookDevView.styles.sToolBarButton, new GUILayoutOption[0]))
            this.m_LookDevView.envLibrary.RemoveHDRI(infos1.cubemap);
        }
        GUILayout.EndHorizontal();
        if (Event.current.type == EventType.MouseDown && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
          Event.current.Use();
        Rect rect1 = GUILayoutUtility.GetRect(250f, 125f);
        rect1.width = 253f;
        float num2 = 24f;
        float num3 = num2 * 0.5f;
        Vector2 vector2 = this.LatLongToPosition(shadowInfo.latitude, shadowInfo.longitude + infos1.angleOffset) * 0.5f + new Vector2(0.5f, 0.5f);
        Rect position1 = rect1;
        position1.x = position1.x + vector2.x * rect1.width - num3;
        position1.y = position1.y + (1f - vector2.y) * rect1.height - num3;
        position1.width = num2;
        position1.height = num2;
        Rect position2 = rect1;
        position2.x = (float) ((double) position2.x + (double) vector2.x * (double) rect1.width - (double) num3 * 0.5);
        position2.y = (float) ((double) position2.y + (1.0 - (double) vector2.y) * (double) rect1.height - (double) num3 * 0.5);
        position2.width = num2 * 0.5f;
        position2.height = num2 * 0.5f;
        Rect frameTextureRect;
        Rect shadowTextureRect;
        this.GetFrameAndShadowTextureRect(rect1, out frameTextureRect, out shadowTextureRect);
        if (this.m_LookDevView.config.enableShadowCubemap)
          EditorGUIUtility.AddCursorRect(position2, MouseCursor.Pan);
        if (Event.current.type == EventType.MouseDown && rect1.Contains(Event.current.mousePosition))
        {
          if (!Event.current.control && Event.current.button == 0 && this.m_SelectedCubeMapOffsetIndex == -1)
          {
            if (this.m_LookDevView.config.enableShadowCubemap && position2.Contains(Event.current.mousePosition))
            {
              this.m_SelectedLightIconIndex = envIndex;
              this.m_SelectedShadowInfo = shadowInfo;
              Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.envLibrary, "Light Icon selection");
              this.m_SelectedShadowInfo.latitude += 0.0001f;
              this.m_SelectedShadowInfo.longitude += 0.0001f;
            }
            if (this.m_SelectedShadowInfo == null)
            {
              Rect rect2 = frameTextureRect;
              rect2.x += 100f;
              rect2.y += 4f;
              rect2.width = 11f;
              rect2.height = 11f;
              if (this.m_LookDevView.config.enableShadowCubemap && rect2.Contains(Event.current.mousePosition))
              {
                Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.envLibrary, "Update shadow cubemap");
                hdriList[envIndex].SetCubemapShadowInfo(hdriList[envIndex]);
                this.m_LookDevView.envLibrary.dirty = true;
              }
              else
              {
                if (this.m_LookDevView.config.enableShadowCubemap && shadowTextureRect.Contains(Event.current.mousePosition))
                {
                  this.m_SelectedShadowCubemapOwnerInfo = hdriList[envIndex];
                  this.m_SelectedCubemapInfo = this.m_SelectedShadowCubemapOwnerInfo.cubemapShadowInfo;
                }
                else
                  this.m_SelectedCubemapInfo = hdriList[envIndex];
                this.m_SelectedPositionOffset = Event.current.mousePosition - new Vector2(rect1.x, rect1.y);
                this.m_RenderOverlayThumbnailOnce = true;
              }
            }
          }
          else if (Event.current.control && Event.current.button == 0 && (this.m_SelectedCubemapInfo == null && this.m_SelectedShadowInfo == null))
          {
            this.m_SelectedCubeMapOffsetIndex = envIndex;
            this.m_SelectedCubeMapOffsetValue = infos1.angleOffset - this.ComputeAngleOffsetFromMouseCoord(Event.current.mousePosition);
          }
          GUIUtility.hotControl = this.m_LookDevView.hotControl;
          Event.current.Use();
        }
        if (Event.current.GetTypeForControl(this.m_LookDevView.hotControl) == EventType.MouseDrag && this.m_SelectedShadowInfo == shadowInfo && this.m_SelectedLightIconIndex == envIndex)
        {
          Vector2 mousePosition = Event.current.mousePosition;
          mousePosition.x = (float) (((double) mousePosition.x - (double) rect1.x) / (double) rect1.width * 2.0 - 1.0);
          mousePosition.y = (float) ((1.0 - ((double) mousePosition.y - (double) rect1.y) / (double) rect1.height) * 2.0 - 1.0);
          Vector2 latLong = LookDevEnvironmentWindow.PositionToLatLong(mousePosition);
          this.m_SelectedShadowInfo.latitude = latLong.x;
          this.m_SelectedShadowInfo.longitude = latLong.y - infos1.angleOffset;
          this.m_LookDevView.envLibrary.dirty = true;
        }
        if (Event.current.type == EventType.Repaint)
        {
          this.DrawLatLongThumbnail(infos1, infos1.angleOffset, 1f, 1f, rect1);
          if (this.m_LookDevView.config.enableShadowCubemap)
          {
            if (infos1.cubemapShadowInfo != infos1 || this.m_HoveringCubeMapIndex == envIndex && this.m_SelectedCubemapInfo != infos1)
            {
              CubemapInfo infos2 = infos1.cubemapShadowInfo;
              if (this.m_HoveringCubeMapIndex == envIndex && this.m_SelectedCubemapInfo != infos1)
                infos2 = this.m_SelectedCubemapInfo;
              float alpha = 1f;
              if (this.m_SelectedShadowInfo == shadowInfo)
                alpha = 0.1f;
              else if (this.m_HoveringCubeMapIndex == envIndex && this.m_SelectedCubemapInfo != infos1 && infos1.cubemapShadowInfo != this.m_SelectedCubemapInfo)
                alpha = 0.5f;
              this.DrawLatLongThumbnail(infos2, infos1.angleOffset, 0.3f, alpha, shadowTextureRect);
              GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
              GUI.DrawTexture(frameTextureRect, LookDevEnvironmentWindow.styles.sLatlongFrameTexture);
              GL.sRGBWrite = false;
            }
            GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
            GUI.DrawTexture(position1, LookDevEnvironmentWindow.styles.sLightTexture);
            GL.sRGBWrite = false;
          }
          if (flag2)
            this.DrawSelectionFeedback(rect1, selectionColor1, selectionColor2);
        }
        GUILayout.EndVertical();
      }
      GUILayout.BeginVertical(GUILayout.Width(250f));
      if (((UnityEngine.Object) this.m_SelectedCubemap != (UnityEngine.Object) null || this.m_DragBeingPerformed) && this.GetInsertionRect(hdriList.Count).Contains(Event.current.mousePosition))
      {
        GUILayout.Label(GUIContent.none, LookDevEnvironmentWindow.styles.sSeparatorStyle, new GUILayoutOption[0]);
        GUILayoutUtility.GetRect(250f, 16f);
        GUILayout.Label(GUIContent.none, LookDevEnvironmentWindow.styles.sSeparatorStyle, new GUILayoutOption[0]);
      }
      GUILayout.EndVertical();
      if (flag1)
        EditorGUILayout.EndScrollView();
      this.HandleMouseInput();
      this.RenderOverlayThumbnailIfNeeded();
      if (Event.current.type != EventType.Repaint || !((UnityEngine.Object) this.m_SelectedCubemap != (UnityEngine.Object) null))
        return;
      this.m_LookDevView.Repaint();
    }

    internal class EnvSettingsWindow : PopupWindowContent
    {
      private static LookDevEnvironmentWindow.EnvSettingsWindow.Styles s_Styles = (LookDevEnvironmentWindow.EnvSettingsWindow.Styles) null;
      private float kShadowSettingWidth = 200f;
      private float kShadowSettingHeight = 157f;
      private CubemapInfo m_CubemapInfo;
      private LookDevView m_LookDevView;

      public EnvSettingsWindow(LookDevView lookDevView, CubemapInfo infos)
      {
        this.m_LookDevView = lookDevView;
        this.m_CubemapInfo = infos;
      }

      public static LookDevEnvironmentWindow.EnvSettingsWindow.Styles styles
      {
        get
        {
          if (LookDevEnvironmentWindow.EnvSettingsWindow.s_Styles == null)
            LookDevEnvironmentWindow.EnvSettingsWindow.s_Styles = new LookDevEnvironmentWindow.EnvSettingsWindow.Styles();
          return LookDevEnvironmentWindow.EnvSettingsWindow.s_Styles;
        }
      }

      public override Vector2 GetWindowSize()
      {
        return new Vector2(this.kShadowSettingWidth, this.kShadowSettingHeight);
      }

      private void DrawSeparator()
      {
        GUILayout.Space(3f);
        GUILayout.Label(GUIContent.none, LookDevEnvironmentWindow.EnvSettingsWindow.styles.sSeparator, new GUILayoutOption[0]);
      }

      public override void OnGUI(Rect rect)
      {
        GUILayout.BeginVertical();
        GUILayout.Label(LookDevEnvironmentWindow.EnvSettingsWindow.styles.sEnvironment, EditorStyles.miniLabel, new GUILayoutOption[0]);
        EditorGUI.BeginChangeCheck();
        float num1 = EditorGUILayout.Slider(LookDevEnvironmentWindow.EnvSettingsWindow.styles.sAngleOffset, this.m_CubemapInfo.angleOffset % 360f, -360f, 360f, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.envLibrary, "Changed environment settings");
          this.m_CubemapInfo.angleOffset = num1;
          this.m_LookDevView.envLibrary.dirty = true;
          this.m_LookDevView.Repaint();
        }
        if (GUILayout.Button(LookDevEnvironmentWindow.EnvSettingsWindow.styles.sResetEnv, EditorStyles.toolbarButton, new GUILayoutOption[0]))
        {
          Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.envLibrary, "Changed environment settings");
          this.m_CubemapInfo.ResetEnvInfos();
          this.m_LookDevView.envLibrary.dirty = true;
          this.m_LookDevView.Repaint();
        }
        using (new EditorGUI.DisabledScope(!this.m_LookDevView.config.enableShadowCubemap))
        {
          this.DrawSeparator();
          GUILayout.Label(LookDevEnvironmentWindow.EnvSettingsWindow.styles.sShadows, EditorStyles.miniLabel, new GUILayoutOption[0]);
          EditorGUI.BeginChangeCheck();
          float num2 = EditorGUILayout.Slider(LookDevEnvironmentWindow.EnvSettingsWindow.styles.sShadowIntensity, this.m_CubemapInfo.shadowInfo.shadowIntensity, 0.0f, 5f, new GUILayoutOption[0]);
          Color color = EditorGUILayout.ColorField(LookDevEnvironmentWindow.EnvSettingsWindow.styles.sShadowColor, this.m_CubemapInfo.shadowInfo.shadowColor, new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck())
          {
            Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.envLibrary, "Changed shadow settings");
            this.m_CubemapInfo.shadowInfo.shadowIntensity = num2;
            this.m_CubemapInfo.shadowInfo.shadowColor = color;
            this.m_LookDevView.envLibrary.dirty = true;
            this.m_LookDevView.Repaint();
          }
          if (GUILayout.Button(LookDevEnvironmentWindow.EnvSettingsWindow.styles.sBrightest, EditorStyles.toolbarButton, new GUILayoutOption[0]))
          {
            Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.envLibrary, "Changed shadow settings");
            LookDevResources.UpdateShadowInfoWithBrightestSpot(this.m_CubemapInfo);
            this.m_LookDevView.envLibrary.dirty = true;
            this.m_LookDevView.Repaint();
          }
          if (GUILayout.Button(LookDevEnvironmentWindow.EnvSettingsWindow.styles.sResetShadow, EditorStyles.toolbarButton, new GUILayoutOption[0]))
          {
            Undo.RecordObject((UnityEngine.Object) this.m_LookDevView.envLibrary, "Changed shadow settings");
            this.m_CubemapInfo.SetCubemapShadowInfo(this.m_CubemapInfo);
            this.m_LookDevView.envLibrary.dirty = true;
            this.m_LookDevView.Repaint();
          }
        }
        GUILayout.EndVertical();
      }

      public class Styles
      {
        public readonly GUIStyle sMenuItem = (GUIStyle) "MenuItem";
        public readonly GUIStyle sSeparator = (GUIStyle) "sv_iconselector_sep";
        public readonly GUIContent sEnvironment = EditorGUIUtility.TextContent("Environment");
        public readonly GUIContent sAngleOffset = EditorGUIUtility.TextContent("Angle offset|Rotate the environment");
        public readonly GUIContent sResetEnv = EditorGUIUtility.TextContent("Reset Environment|Reset environment settings");
        public readonly GUIContent sShadows = EditorGUIUtility.TextContent("Shadows");
        public readonly GUIContent sShadowIntensity = EditorGUIUtility.TextContent("Shadow brightness|Shadow brightness");
        public readonly GUIContent sShadowColor = EditorGUIUtility.TextContent("Color|Shadow color");
        public readonly GUIContent sBrightest = EditorGUIUtility.TextContent("Set position to brightest point|Set the shadow direction to the brightest (higher value) point of the latLong map");
        public readonly GUIContent sResetShadow = EditorGUIUtility.TextContent("Reset Shadows|Reset shadow properties");
      }
    }

    public class Styles
    {
      public readonly GUIContent sTitle = EditorGUIUtility.TextContent("HDRI View|Manage your list of HDRI environments.");
      public readonly GUIContent sCloseIcon = new GUIContent(EditorGUIUtility.IconContent("LookDevClose"));
      public readonly GUIStyle sSeparatorStyle = (GUIStyle) "sv_iconselector_sep";
      public readonly GUIStyle sLabelStyleFirstContext = new GUIStyle(EditorStyles.miniLabel);
      public readonly GUIStyle sLabelStyleSecondContext = new GUIStyle(EditorStyles.miniLabel);
      public readonly GUIStyle sLabelStyleBothContext = new GUIStyle(EditorStyles.miniLabel);
      public readonly Texture sLightTexture = (Texture) EditorGUIUtility.FindTexture("LookDevLight");
      public readonly Texture sLatlongFrameTexture = (Texture) EditorGUIUtility.FindTexture("LookDevShadowFrame");
      public readonly GUIContent sEnvControlIcon = new GUIContent(EditorGUIUtility.IconContent("LookDevPaneOption"));
      public readonly GUIContent sDragAndDropHDRIText = EditorGUIUtility.TextContent("Drag and drop HDR panorama here.");

      public Styles()
      {
        this.sLabelStyleFirstContext.normal.textColor = (Color) LookDevView.m_FirstViewGizmoColor;
        this.sLabelStyleSecondContext.normal.textColor = (Color) LookDevView.m_SecondViewGizmoColor;
        this.sLabelStyleBothContext.normal.textColor = !EditorGUIUtility.isProSkin ? Color.black : Color.white;
        this.sEnvControlIcon.tooltip = "Environment parameters";
      }
    }
  }
}
