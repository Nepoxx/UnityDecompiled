// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightingWindowObjectTab
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;
using UnityEngineInternal;

namespace UnityEditor
{
  internal class LightingWindowObjectTab
  {
    private GITextureType[] kObjectPreviewTextureTypes = new GITextureType[12]{ GITextureType.Charting, GITextureType.Albedo, GITextureType.Emissive, GITextureType.Irradiance, GITextureType.Directionality, GITextureType.Baked, GITextureType.BakedDirectional, GITextureType.BakedShadowMask, GITextureType.BakedAlbedo, GITextureType.BakedEmissive, GITextureType.BakedCharting, GITextureType.BakedTexelValidity };
    private AnimBool m_ShowClampedSize = new AnimBool();
    private ZoomableArea m_ZoomablePreview;
    private GUIContent m_SelectedObjectPreviewTexture;
    private int m_PreviousSelection;
    private VisualisationGITexture m_CachedTexture;

    public void OnEnable(EditorWindow window)
    {
      this.m_ShowClampedSize.value = false;
      this.m_ShowClampedSize.valueChanged.AddListener(new UnityAction(window.Repaint));
    }

    public void OnDisable()
    {
    }

    public void ObjectPreview(Rect r)
    {
      if ((double) r.height <= 0.0)
        return;
      if (this.m_ZoomablePreview == null)
      {
        this.m_ZoomablePreview = new ZoomableArea(true);
        this.m_ZoomablePreview.hRangeMin = 0.0f;
        this.m_ZoomablePreview.vRangeMin = 0.0f;
        this.m_ZoomablePreview.hRangeMax = 1f;
        this.m_ZoomablePreview.vRangeMax = 1f;
        this.m_ZoomablePreview.SetShownHRange(0.0f, 1f);
        this.m_ZoomablePreview.SetShownVRange(0.0f, 1f);
        this.m_ZoomablePreview.uniformScale = true;
        this.m_ZoomablePreview.scaleWithWindow = true;
      }
      GUI.Box(r, "", (GUIStyle) "PreBackground");
      Rect position1 = new Rect(r);
      ++position1.y;
      position1.height = 18f;
      GUI.Box(position1, "", EditorStyles.toolbar);
      Rect position2 = new Rect(r);
      ++position2.y;
      position2.height = 18f;
      position2.width = 120f;
      Rect rect1 = new Rect(r);
      rect1.yMin += position2.height;
      rect1.yMax -= 14f;
      rect1.width -= 11f;
      int index1 = Array.IndexOf<GUIContent>(LightingWindowObjectTab.Styles.ObjectPreviewTextureOptions, this.m_SelectedObjectPreviewTexture);
      if (index1 < 0 || !LightmapVisualizationUtility.IsTextureTypeEnabled(this.kObjectPreviewTextureTypes[index1]))
      {
        index1 = 0;
        this.m_SelectedObjectPreviewTexture = LightingWindowObjectTab.Styles.ObjectPreviewTextureOptions[index1];
      }
      if (EditorGUI.DropdownButton(position2, this.m_SelectedObjectPreviewTexture, FocusType.Passive, EditorStyles.toolbarPopup))
      {
        GenericMenu genericMenu = new GenericMenu();
        for (int index2 = 0; index2 < LightingWindowObjectTab.Styles.ObjectPreviewTextureOptions.Length; ++index2)
        {
          if (LightmapVisualizationUtility.IsTextureTypeEnabled(this.kObjectPreviewTextureTypes[index2]))
            genericMenu.AddItem(LightingWindowObjectTab.Styles.ObjectPreviewTextureOptions[index2], index1 == index2, new GenericMenu.MenuFunction2(this.SelectPreviewTextureOption), (object) ((IEnumerable<GUIContent>) LightingWindowObjectTab.Styles.ObjectPreviewTextureOptions).ElementAt<GUIContent>(index2));
          else
            genericMenu.AddDisabledItem(((IEnumerable<GUIContent>) LightingWindowObjectTab.Styles.ObjectPreviewTextureOptions).ElementAt<GUIContent>(index2));
        }
        genericMenu.DropDown(position2);
      }
      GITextureType previewTextureType = this.kObjectPreviewTextureTypes[Array.IndexOf<GUIContent>(LightingWindowObjectTab.Styles.ObjectPreviewTextureOptions, this.m_SelectedObjectPreviewTexture)];
      if (this.m_CachedTexture.type != previewTextureType || this.m_CachedTexture.contentHash != LightmapVisualizationUtility.GetSelectedObjectGITextureHash(previewTextureType) || this.m_CachedTexture.contentHash == new Hash128())
        this.m_CachedTexture = LightmapVisualizationUtility.GetSelectedObjectGITexture(previewTextureType);
      if (this.m_CachedTexture.textureAvailability == GITextureAvailability.GITextureNotAvailable || this.m_CachedTexture.textureAvailability == GITextureAvailability.GITextureUnknown)
      {
        if (LightmapVisualizationUtility.IsBakedTextureType(previewTextureType))
        {
          if (previewTextureType == GITextureType.BakedShadowMask)
            GUI.Label(rect1, LightingWindowObjectTab.Styles.TextureNotAvailableBakedShadowmask);
          else
            GUI.Label(rect1, LightingWindowObjectTab.Styles.TextureNotAvailableBaked);
        }
        else
          GUI.Label(rect1, LightingWindowObjectTab.Styles.TextureNotAvailableRealtime);
      }
      else if (this.m_CachedTexture.textureAvailability == GITextureAvailability.GITextureLoading && (UnityEngine.Object) this.m_CachedTexture.texture == (UnityEngine.Object) null)
      {
        GUI.Label(rect1, LightingWindowObjectTab.Styles.TextureLoading);
      }
      else
      {
        LightmapType lightmapType = LightmapVisualizationUtility.GetLightmapType(previewTextureType);
        switch (Event.current.type)
        {
          case EventType.Repaint:
            Texture2D texture = this.m_CachedTexture.texture;
            if ((bool) ((UnityEngine.Object) texture) && Event.current.type == EventType.Repaint)
            {
              Rect position3 = new Rect(this.ScaleRectByZoomableArea(this.CenterToRect(this.ResizeRectToFit(new Rect(0.0f, 0.0f, (float) texture.width, (float) texture.height), rect1), rect1), this.m_ZoomablePreview));
              position3.x += 3f;
              position3.y += rect1.y + 20f;
              Rect drawableArea = new Rect(rect1);
              drawableArea.y += position2.height + 3f;
              float num = drawableArea.y - 14f;
              position3.y -= num;
              drawableArea.y -= num;
              UnityEngine.FilterMode filterMode = texture.filterMode;
              texture.filterMode = UnityEngine.FilterMode.Point;
              LightmapVisualizationUtility.DrawTextureWithUVOverlay(texture, Selection.activeGameObject, drawableArea, position3, previewTextureType);
              texture.filterMode = filterMode;
              break;
            }
            break;
          case EventType.ValidateCommand:
          case EventType.ExecuteCommand:
            if (Event.current.commandName == "FrameSelected")
            {
              Vector4 lightmapTilingOffset = LightmapVisualizationUtility.GetLightmapTilingOffset(lightmapType);
              Vector2 lhs1 = new Vector2(lightmapTilingOffset.z, lightmapTilingOffset.w);
              Vector2 lhs2 = lhs1 + new Vector2(lightmapTilingOffset.x, lightmapTilingOffset.y);
              lhs1 = Vector2.Max(lhs1, Vector2.zero);
              Vector2 vector2 = Vector2.Min(lhs2, Vector2.one);
              float num1 = 1f - lhs1.y;
              lhs1.y = 1f - vector2.y;
              vector2.y = num1;
              Rect rect2 = new Rect(lhs1.x, lhs1.y, vector2.x - lhs1.x, vector2.y - lhs1.y);
              rect2.x -= Mathf.Clamp(rect2.height - rect2.width, 0.0f, float.MaxValue) / 2f;
              rect2.y -= Mathf.Clamp(rect2.width - rect2.height, 0.0f, float.MaxValue) / 2f;
              // ISSUE: explicit reference operation
              // ISSUE: variable of a reference type
              Rect& local = @rect2;
              float num2 = Mathf.Max(rect2.width, rect2.height);
              rect2.height = num2;
              double num3 = (double) num2;
              // ISSUE: explicit reference operation
              (^local).width = (float) num3;
              this.m_ZoomablePreview.shownArea = rect2;
              Event.current.Use();
              break;
            }
            break;
        }
        if (this.m_PreviousSelection != Selection.activeInstanceID)
        {
          this.m_PreviousSelection = Selection.activeInstanceID;
          this.m_ZoomablePreview.SetShownHRange(0.0f, 1f);
          this.m_ZoomablePreview.SetShownVRange(0.0f, 1f);
        }
        Rect rect3 = new Rect(r);
        rect3.yMin += position2.height;
        this.m_ZoomablePreview.rect = rect3;
        this.m_ZoomablePreview.BeginViewGUI();
        this.m_ZoomablePreview.EndViewGUI();
        GUILayoutUtility.GetRect(r.width, r.height);
      }
    }

    private void SelectPreviewTextureOption(object textureOption)
    {
      this.m_SelectedObjectPreviewTexture = (GUIContent) textureOption;
    }

    private Rect ResizeRectToFit(Rect rect, Rect to)
    {
      float num = Mathf.Min(to.width / rect.width, to.height / rect.height);
      float width = (float) (int) Mathf.Round(rect.width * num);
      float height = (float) (int) Mathf.Round(rect.height * num);
      return new Rect(rect.x, rect.y, width, height);
    }

    private Rect CenterToRect(Rect rect, Rect to)
    {
      float num1 = Mathf.Clamp((float) (int) ((double) to.width - (double) rect.width) / 2f, 0.0f, (float) int.MaxValue);
      float num2 = Mathf.Clamp((float) (int) ((double) to.height - (double) rect.height) / 2f, 0.0f, (float) int.MaxValue);
      return new Rect(rect.x + num1, rect.y + num2, rect.width, rect.height);
    }

    private Rect ScaleRectByZoomableArea(Rect rect, ZoomableArea zoomableArea)
    {
      float num1 = (float) -((double) zoomableArea.shownArea.x / (double) zoomableArea.shownArea.width) * rect.width;
      float num2 = (zoomableArea.shownArea.y - (1f - zoomableArea.shownArea.height)) / zoomableArea.shownArea.height * rect.height;
      float width = rect.width / zoomableArea.shownArea.width;
      float height = rect.height / zoomableArea.shownArea.height;
      return new Rect(rect.x + num1, rect.y + num2, width, height);
    }

    private class Styles
    {
      public static readonly GUIContent[] ObjectPreviewTextureOptions = new GUIContent[12]{ EditorGUIUtility.TextContent("UV Charts"), EditorGUIUtility.TextContent("Realtime Albedo"), EditorGUIUtility.TextContent("Realtime Emissive"), EditorGUIUtility.TextContent("Realtime Indirect"), EditorGUIUtility.TextContent("Realtime Directionality"), EditorGUIUtility.TextContent("Baked Lightmap"), EditorGUIUtility.TextContent("Baked Directionality"), EditorGUIUtility.TextContent("Baked Shadowmask"), EditorGUIUtility.TextContent("Baked Albedo"), EditorGUIUtility.TextContent("Baked Emissive"), EditorGUIUtility.TextContent("Baked UV Charts"), EditorGUIUtility.TextContent("Baked Texel Validity") };
      public static readonly GUIContent TextureNotAvailableRealtime = EditorGUIUtility.TextContent("The texture is not available at the moment.");
      public static readonly GUIContent TextureNotAvailableBaked = EditorGUIUtility.TextContent("The texture is not available at the moment.\nPlease try to rebake the current scene or turn on Auto, and make sure that this object is set to Lightmap Static if it's meant to be baked.");
      public static readonly GUIContent TextureNotAvailableBakedShadowmask = EditorGUIUtility.TextContent("The texture is not available at the moment.\nPlease make sure that Mixed Lights affect this GameObject and that it is set to Lightmap Static.");
      public static readonly GUIContent TextureLoading = EditorGUIUtility.TextContent("Loading...");
    }
  }
}
