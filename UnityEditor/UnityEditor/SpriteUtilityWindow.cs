// Decompiled with JetBrains decompiler
// Type: UnityEditor.SpriteUtilityWindow
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor
{
  internal class SpriteUtilityWindow : EditorWindow
  {
    protected bool m_ShowAlpha = false;
    protected float m_Zoom = -1f;
    protected float m_MipLevel = 0.0f;
    protected Vector2 m_ScrollPosition = new Vector2();
    protected SpriteUtilityWindow.Styles m_Styles;
    protected const float k_BorderMargin = 10f;
    protected const float k_ScrollbarMargin = 16f;
    protected const float k_InspectorWindowMargin = 8f;
    protected const float k_InspectorWidth = 330f;
    protected const float k_MinZoomPercentage = 0.9f;
    protected const float k_MaxZoom = 50f;
    protected const float k_WheelZoomSpeed = 0.03f;
    protected const float k_MouseZoomSpeed = 0.005f;
    protected const float k_ToolbarHeight = 17f;
    protected ITexture2D m_Texture;
    protected ITexture2D m_TextureAlphaOverride;
    protected Rect m_TextureViewRect;
    protected Rect m_TextureRect;

    protected void InitStyles()
    {
      if (this.m_Styles != null)
        return;
      this.m_Styles = new SpriteUtilityWindow.Styles();
    }

    protected float GetMinZoom()
    {
      if (this.m_Texture == (ITexture2D) null)
        return 1f;
      return Mathf.Min(this.m_TextureViewRect.width / (float) this.m_Texture.width, this.m_TextureViewRect.height / (float) this.m_Texture.height, 50f) * 0.9f;
    }

    protected void HandleZoom()
    {
      bool flag = UnityEngine.Event.current.alt && UnityEngine.Event.current.button == 1;
      if (flag)
        EditorGUIUtility.AddCursorRect(this.m_TextureViewRect, MouseCursor.Zoom);
      if ((UnityEngine.Event.current.type == EventType.MouseUp || UnityEngine.Event.current.type == EventType.MouseDown) && flag || (UnityEngine.Event.current.type == EventType.KeyUp || UnityEngine.Event.current.type == EventType.KeyDown) && UnityEngine.Event.current.keyCode == KeyCode.LeftAlt)
        this.Repaint();
      if (UnityEngine.Event.current.type != EventType.ScrollWheel && (UnityEngine.Event.current.type != EventType.MouseDrag || !UnityEngine.Event.current.alt || UnityEngine.Event.current.button != 1))
        return;
      float num1 = (float) (1.0 - (double) UnityEngine.Event.current.delta.y * (UnityEngine.Event.current.type != EventType.ScrollWheel ? -0.00499999988824129 : 0.0299999993294477));
      float num2 = this.m_Zoom * num1;
      float num3 = Mathf.Clamp(num2, this.GetMinZoom(), 50f);
      if ((double) num3 != (double) this.m_Zoom)
      {
        this.m_Zoom = num3;
        if ((double) num2 != (double) num3)
          num1 /= num2 / num3;
        this.m_ScrollPosition *= num1;
        float num4 = (float) ((double) UnityEngine.Event.current.mousePosition.x / (double) this.m_TextureViewRect.width - 0.5);
        float num5 = (float) ((double) UnityEngine.Event.current.mousePosition.y / (double) this.m_TextureViewRect.height - 0.5);
        float num6 = num4 * (num1 - 1f);
        float num7 = num5 * (num1 - 1f);
        Rect maxScrollRect = this.maxScrollRect;
        this.m_ScrollPosition.x += num6 * (maxScrollRect.width / 2f);
        this.m_ScrollPosition.y += num7 * (maxScrollRect.height / 2f);
        UnityEngine.Event.current.Use();
      }
    }

    protected void HandlePanning()
    {
      bool flag = !UnityEngine.Event.current.alt && UnityEngine.Event.current.button > 0 || UnityEngine.Event.current.alt && UnityEngine.Event.current.button <= 0;
      if (flag && GUIUtility.hotControl == 0)
      {
        EditorGUIUtility.AddCursorRect(this.m_TextureViewRect, MouseCursor.Pan);
        if (UnityEngine.Event.current.type == EventType.MouseDrag)
        {
          this.m_ScrollPosition -= UnityEngine.Event.current.delta;
          UnityEngine.Event.current.Use();
        }
      }
      if ((UnityEngine.Event.current.type != EventType.MouseUp && UnityEngine.Event.current.type != EventType.MouseDown || !flag) && (UnityEngine.Event.current.type != EventType.KeyUp && UnityEngine.Event.current.type != EventType.KeyDown || UnityEngine.Event.current.keyCode != KeyCode.LeftAlt))
        return;
      this.Repaint();
    }

    protected Rect maxScrollRect
    {
      get
      {
        float num1 = (float) this.m_Texture.width * 0.5f * this.m_Zoom;
        float num2 = (float) this.m_Texture.height * 0.5f * this.m_Zoom;
        return new Rect(-num1, -num2, this.m_TextureViewRect.width + num1 * 2f, this.m_TextureViewRect.height + num2 * 2f);
      }
    }

    protected Rect maxRect
    {
      get
      {
        float num1 = this.m_TextureViewRect.width * 0.5f / this.GetMinZoom();
        float num2 = this.m_TextureViewRect.height * 0.5f / this.GetMinZoom();
        return new Rect(-num1, -num2, (float) this.m_Texture.width + num1 * 2f, (float) this.m_Texture.height + num2 * 2f);
      }
    }

    protected void DrawTexturespaceBackground()
    {
      float num1 = Mathf.Max(this.maxRect.width, this.maxRect.height);
      Vector2 vector2 = new Vector2(this.maxRect.xMin, this.maxRect.yMin);
      float num2 = num1 * 0.5f;
      float a = !EditorGUIUtility.isProSkin ? 0.08f : 0.15f;
      float num3 = 8f;
      SpriteEditorUtility.BeginLines(new Color(0.0f, 0.0f, 0.0f, a));
      float num4 = 0.0f;
      while ((double) num4 <= (double) num1)
      {
        SpriteEditorUtility.DrawLine((Vector3) (new Vector2(-num2 + num4, num2 + num4) + vector2), (Vector3) (new Vector2(num2 + num4, -num2 + num4) + vector2));
        num4 += num3;
      }
      SpriteEditorUtility.EndLines();
    }

    private float Log2(float x)
    {
      return (float) (Math.Log((double) x) / Math.Log(2.0));
    }

    protected void DrawTexture()
    {
      int num1 = Mathf.Max(this.m_Texture.width, 1);
      float num2 = Mathf.Min(this.m_MipLevel, (float) (TextureUtil.GetMipmapCount((Texture) (UnityEngine.Texture2D) this.m_Texture) - 1));
      float mipMapBias = this.m_Texture.mipMapBias;
      TextureUtil.SetMipMapBiasNoDirty((Texture) (UnityEngine.Texture2D) this.m_Texture, num2 - this.Log2((float) num1 / this.m_TextureRect.width));
      UnityEngine.FilterMode filterMode = this.m_Texture.filterMode;
      TextureUtil.SetFilterModeNoDirty((Texture) (UnityEngine.Texture2D) this.m_Texture, UnityEngine.FilterMode.Point);
      if (this.m_ShowAlpha)
      {
        if (this.m_TextureAlphaOverride != (ITexture2D) null)
          EditorGUI.DrawTextureTransparent(this.m_TextureRect, (Texture) (UnityEngine.Texture2D) this.m_TextureAlphaOverride);
        else
          EditorGUI.DrawTextureAlpha(this.m_TextureRect, (Texture) (UnityEngine.Texture2D) this.m_Texture);
      }
      else
        EditorGUI.DrawTextureTransparent(this.m_TextureRect, (Texture) (UnityEngine.Texture2D) this.m_Texture);
      TextureUtil.SetMipMapBiasNoDirty((Texture) (UnityEngine.Texture2D) this.m_Texture, mipMapBias);
      TextureUtil.SetFilterModeNoDirty((Texture) (UnityEngine.Texture2D) this.m_Texture, filterMode);
    }

    protected void DrawScreenspaceBackground()
    {
      if (UnityEngine.Event.current.type != EventType.Repaint)
        return;
      this.m_Styles.preBackground.Draw(this.m_TextureViewRect, false, false, false, false);
    }

    protected void HandleScrollbars()
    {
      this.m_ScrollPosition.x = GUI.HorizontalScrollbar(new Rect(this.m_TextureViewRect.xMin, this.m_TextureViewRect.yMax, this.m_TextureViewRect.width, 16f), this.m_ScrollPosition.x, this.m_TextureViewRect.width, this.maxScrollRect.xMin, this.maxScrollRect.xMax);
      this.m_ScrollPosition.y = GUI.VerticalScrollbar(new Rect(this.m_TextureViewRect.xMax, this.m_TextureViewRect.yMin, 16f, this.m_TextureViewRect.height), this.m_ScrollPosition.y, this.m_TextureViewRect.height, this.maxScrollRect.yMin, this.maxScrollRect.yMax);
    }

    protected void SetupHandlesMatrix()
    {
      Handles.matrix = Matrix4x4.TRS(new Vector3(this.m_TextureRect.x, this.m_TextureRect.yMax, 0.0f), Quaternion.identity, new Vector3(this.m_Zoom, -this.m_Zoom, 1f));
    }

    protected Rect DoAlphaZoomToolbarGUI(Rect area)
    {
      int a = 1;
      if (this.m_Texture != (ITexture2D) null)
        a = Mathf.Max(a, TextureUtil.GetMipmapCount((Texture) (UnityEngine.Texture2D) this.m_Texture));
      Rect position = new Rect(area.width, 0.0f, 0.0f, area.height);
      using (new EditorGUI.DisabledScope(a == 1))
      {
        position.width = (float) this.m_Styles.largeMip.image.width;
        position.x -= position.width;
        GUI.Box(position, this.m_Styles.largeMip, this.m_Styles.preLabel);
        position.width = 60f;
        position.x -= position.width;
        this.m_MipLevel = Mathf.Round(GUI.HorizontalSlider(position, this.m_MipLevel, (float) (a - 1), 0.0f, this.m_Styles.preSlider, this.m_Styles.preSliderThumb));
        position.width = (float) this.m_Styles.smallMip.image.width;
        position.x -= position.width;
        GUI.Box(position, this.m_Styles.smallMip, this.m_Styles.preLabel);
      }
      position.width = 60f;
      position.x -= position.width;
      this.m_Zoom = GUI.HorizontalSlider(position, this.m_Zoom, this.GetMinZoom(), 50f, this.m_Styles.preSlider, this.m_Styles.preSliderThumb);
      position.width = 32f;
      position.x -= position.width + 5f;
      this.m_ShowAlpha = GUI.Toggle(position, this.m_ShowAlpha, !this.m_ShowAlpha ? this.m_Styles.RGBIcon : this.m_Styles.alphaIcon, (GUIStyle) "toolbarButton");
      return new Rect(area.x, area.y, position.x, area.height);
    }

    protected void DoTextureGUI()
    {
      if (this.m_Texture == (ITexture2D) null)
        return;
      if ((double) this.m_Zoom < 0.0)
        this.m_Zoom = this.GetMinZoom();
      this.m_TextureRect = new Rect((float) ((double) this.m_TextureViewRect.width / 2.0 - (double) this.m_Texture.width * (double) this.m_Zoom / 2.0), (float) ((double) this.m_TextureViewRect.height / 2.0 - (double) this.m_Texture.height * (double) this.m_Zoom / 2.0), (float) this.m_Texture.width * this.m_Zoom, (float) this.m_Texture.height * this.m_Zoom);
      this.HandleScrollbars();
      this.SetupHandlesMatrix();
      this.HandleZoom();
      this.HandlePanning();
      this.DrawScreenspaceBackground();
      GUIClip.Push(this.m_TextureViewRect, -this.m_ScrollPosition, Vector2.zero, false);
      if (UnityEngine.Event.current.type == EventType.Repaint)
      {
        this.DrawTexturespaceBackground();
        this.DrawTexture();
        this.DrawGizmos();
      }
      this.DoTextureGUIExtras();
      GUIClip.Pop();
    }

    protected virtual void DoTextureGUIExtras()
    {
    }

    protected virtual void DrawGizmos()
    {
    }

    protected void SetNewTexture(UnityEngine.Texture2D texture)
    {
      if (!((UnityEngine.Object) texture != (UnityEngine.Object) this.m_Texture))
        return;
      this.m_Texture = (ITexture2D) new UnityEngine.U2D.Interface.Texture2D(texture);
      this.m_Zoom = -1f;
      this.m_TextureAlphaOverride = (ITexture2D) null;
    }

    protected void SetAlphaTextureOverride(UnityEngine.Texture2D alphaTexture)
    {
      if (!((UnityEngine.Object) alphaTexture != (UnityEngine.Object) this.m_TextureAlphaOverride))
        return;
      this.m_TextureAlphaOverride = (ITexture2D) new UnityEngine.U2D.Interface.Texture2D(alphaTexture);
      this.m_Zoom = -1f;
    }

    internal override void OnResized()
    {
      if (!(this.m_Texture != (ITexture2D) null) || UnityEngine.Event.current == null)
        return;
      this.HandleZoom();
    }

    internal static void DrawToolBarWidget(ref Rect drawRect, ref Rect toolbarRect, Action<Rect> drawAction)
    {
      toolbarRect.width -= drawRect.width;
      if ((double) toolbarRect.width < 0.0)
        drawRect.width += toolbarRect.width;
      if ((double) drawRect.width <= 0.0)
        return;
      drawAction(drawRect);
    }

    protected class Styles
    {
      public readonly GUIStyle dragdot = (GUIStyle) "U2D.dragDot";
      public readonly GUIStyle dragdotDimmed = (GUIStyle) "U2D.dragDotDimmed";
      public readonly GUIStyle dragdotactive = (GUIStyle) "U2D.dragDotActive";
      public readonly GUIStyle createRect = (GUIStyle) "U2D.createRect";
      public readonly GUIStyle preToolbar = (GUIStyle) nameof (preToolbar);
      public readonly GUIStyle preButton = (GUIStyle) nameof (preButton);
      public readonly GUIStyle preLabel = (GUIStyle) nameof (preLabel);
      public readonly GUIStyle preSlider = (GUIStyle) nameof (preSlider);
      public readonly GUIStyle preSliderThumb = (GUIStyle) nameof (preSliderThumb);
      public readonly GUIStyle preBackground = (GUIStyle) nameof (preBackground);
      public readonly GUIStyle pivotdotactive = (GUIStyle) "U2D.pivotDotActive";
      public readonly GUIStyle pivotdot = (GUIStyle) "U2D.pivotDot";
      public readonly GUIStyle dragBorderdot = new GUIStyle();
      public readonly GUIStyle dragBorderDotActive = new GUIStyle();
      public readonly GUIStyle toolbar;
      public readonly GUIContent alphaIcon;
      public readonly GUIContent RGBIcon;
      public readonly GUIStyle notice;
      public readonly GUIContent smallMip;
      public readonly GUIContent largeMip;

      public Styles()
      {
        this.toolbar = new GUIStyle(EditorStyles.inspectorBig);
        this.toolbar.margin.top = 0;
        this.toolbar.margin.bottom = 0;
        this.alphaIcon = EditorGUIUtility.IconContent("PreTextureAlpha");
        this.RGBIcon = EditorGUIUtility.IconContent("PreTextureRGB");
        this.preToolbar.border.top = 0;
        this.createRect.border = new RectOffset(3, 3, 3, 3);
        this.notice = new GUIStyle(GUI.skin.label);
        this.notice.alignment = TextAnchor.MiddleCenter;
        this.notice.normal.textColor = Color.yellow;
        this.dragBorderdot.fixedHeight = 5f;
        this.dragBorderdot.fixedWidth = 5f;
        this.dragBorderdot.normal.background = EditorGUIUtility.whiteTexture;
        this.dragBorderDotActive.fixedHeight = this.dragBorderdot.fixedHeight;
        this.dragBorderDotActive.fixedWidth = this.dragBorderdot.fixedWidth;
        this.dragBorderDotActive.normal.background = EditorGUIUtility.whiteTexture;
        this.smallMip = EditorGUIUtility.IconContent("PreTextureMipMapLow");
        this.largeMip = EditorGUIUtility.IconContent("PreTextureMipMapHigh");
      }
    }
  }
}
