// Decompiled with JetBrains decompiler
// Type: UnityEditor.TextureInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Texture2D))]
  internal class TextureInspector : Editor
  {
    [SerializeField]
    private float m_MipLevel = 0.0f;
    private CubemapPreview m_CubemapPreview = new CubemapPreview();
    private bool m_ShowPerAxisWrapModes = false;
    private static TextureInspector.Styles s_Styles;
    private bool m_ShowAlpha;
    protected SerializedProperty m_WrapU;
    protected SerializedProperty m_WrapV;
    protected SerializedProperty m_WrapW;
    protected SerializedProperty m_FilterMode;
    protected SerializedProperty m_Aniso;
    [SerializeField]
    protected Vector2 m_Pos;

    public static bool IsNormalMap(Texture t)
    {
      TextureUsageMode usageMode = TextureUtil.GetUsageMode(t);
      return usageMode == TextureUsageMode.NormalmapPlain || usageMode == TextureUsageMode.NormalmapDXT5nm;
    }

    protected virtual void OnEnable()
    {
      this.m_WrapU = this.serializedObject.FindProperty("m_TextureSettings.m_WrapU");
      this.m_WrapV = this.serializedObject.FindProperty("m_TextureSettings.m_WrapV");
      this.m_WrapW = this.serializedObject.FindProperty("m_TextureSettings.m_WrapW");
      this.m_FilterMode = this.serializedObject.FindProperty("m_TextureSettings.m_FilterMode");
      this.m_Aniso = this.serializedObject.FindProperty("m_TextureSettings.m_Aniso");
    }

    protected virtual void OnDisable()
    {
      this.m_CubemapPreview.OnDisable();
    }

    internal void SetCubemapIntensity(float intensity)
    {
      if (this.m_CubemapPreview == null)
        return;
      this.m_CubemapPreview.SetIntensity(intensity);
    }

    public float GetMipLevelForRendering()
    {
      if (this.target == (UnityEngine.Object) null)
        return 0.0f;
      if (this.IsCubemap())
        return this.m_CubemapPreview.GetMipLevelForRendering(this.target as Texture);
      return Mathf.Min(this.m_MipLevel, (float) (TextureUtil.GetMipmapCount(this.target as Texture) - 1));
    }

    public float mipLevel
    {
      get
      {
        if (this.IsCubemap())
          return this.m_CubemapPreview.mipLevel;
        return this.m_MipLevel;
      }
      set
      {
        this.m_CubemapPreview.mipLevel = value;
        this.m_MipLevel = value;
      }
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.DoWrapModePopup();
      this.DoFilterModePopup();
      this.DoAnisoLevelSlider();
      this.serializedObject.ApplyModifiedProperties();
    }

    private static void WrapModeAxisPopup(GUIContent label, SerializedProperty wrapProperty)
    {
      TextureWrapMode textureWrapMode1 = (TextureWrapMode) Mathf.Max(wrapProperty.intValue, 0);
      Rect controlRect = EditorGUILayout.GetControlRect();
      EditorGUI.BeginChangeCheck();
      EditorGUI.BeginProperty(controlRect, label, wrapProperty);
      TextureWrapMode textureWrapMode2 = (TextureWrapMode) EditorGUI.EnumPopup(controlRect, label, (Enum) textureWrapMode1);
      EditorGUI.EndProperty();
      if (!EditorGUI.EndChangeCheck())
        return;
      wrapProperty.intValue = (int) textureWrapMode2;
    }

    private static bool IsAnyTextureObjectUsingPerAxisWrapMode(UnityEngine.Object[] objects, bool isVolumeTexture)
    {
      foreach (UnityEngine.Object @object in objects)
      {
        int b1 = 0;
        int b2 = 0;
        int b3 = 0;
        if (@object is Texture)
        {
          Texture texture = (Texture) @object;
          b1 = (int) texture.wrapModeU;
          b2 = (int) texture.wrapModeV;
          b3 = (int) texture.wrapModeW;
        }
        if (@object is TextureImporter)
        {
          TextureImporter textureImporter = (TextureImporter) @object;
          b1 = (int) textureImporter.wrapModeU;
          b2 = (int) textureImporter.wrapModeV;
          b3 = (int) textureImporter.wrapModeW;
        }
        if (@object is IHVImageFormatImporter)
        {
          IHVImageFormatImporter imageFormatImporter = (IHVImageFormatImporter) @object;
          b1 = (int) imageFormatImporter.wrapModeU;
          b2 = (int) imageFormatImporter.wrapModeV;
          b3 = (int) imageFormatImporter.wrapModeW;
        }
        int num1 = Mathf.Max(0, b1);
        int num2 = Mathf.Max(0, b2);
        int num3 = Mathf.Max(0, b3);
        if (num1 != num2 || isVolumeTexture && (num1 != num3 || num2 != num3))
          return true;
      }
      return false;
    }

    internal static void WrapModePopup(SerializedProperty wrapU, SerializedProperty wrapV, SerializedProperty wrapW, bool isVolumeTexture, ref bool showPerAxisWrapModes)
    {
      if (TextureInspector.s_Styles == null)
        TextureInspector.s_Styles = new TextureInspector.Styles();
      TextureWrapMode textureWrapMode1 = (TextureWrapMode) Mathf.Max(wrapU.intValue, 0);
      TextureWrapMode textureWrapMode2 = (TextureWrapMode) Mathf.Max(wrapV.intValue, 0);
      TextureWrapMode textureWrapMode3 = (TextureWrapMode) Mathf.Max(wrapW.intValue, 0);
      if (textureWrapMode1 != textureWrapMode2)
        showPerAxisWrapModes = true;
      if (isVolumeTexture && (textureWrapMode1 != textureWrapMode3 || textureWrapMode2 != textureWrapMode3))
        showPerAxisWrapModes = true;
      if (!showPerAxisWrapModes && (wrapU.hasMultipleDifferentValues || wrapV.hasMultipleDifferentValues || isVolumeTexture && wrapW.hasMultipleDifferentValues) && TextureInspector.IsAnyTextureObjectUsingPerAxisWrapMode(wrapU.serializedObject.targetObjects, isVolumeTexture))
        showPerAxisWrapModes = true;
      int selectedValue = !showPerAxisWrapModes ? (int) textureWrapMode1 : -1;
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = !showPerAxisWrapModes && (wrapU.hasMultipleDifferentValues || wrapV.hasMultipleDifferentValues || isVolumeTexture && wrapW.hasMultipleDifferentValues);
      int num = EditorGUILayout.IntPopup(TextureInspector.s_Styles.wrapModeLabel, selectedValue, TextureInspector.s_Styles.wrapModeContents, TextureInspector.s_Styles.wrapModeValues, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck() && num != -1)
      {
        wrapU.intValue = num;
        wrapV.intValue = num;
        wrapW.intValue = num;
        showPerAxisWrapModes = false;
      }
      if (num == -1)
      {
        showPerAxisWrapModes = true;
        ++EditorGUI.indentLevel;
        TextureInspector.WrapModeAxisPopup(TextureInspector.s_Styles.wrapU, wrapU);
        TextureInspector.WrapModeAxisPopup(TextureInspector.s_Styles.wrapV, wrapV);
        if (isVolumeTexture)
          TextureInspector.WrapModeAxisPopup(TextureInspector.s_Styles.wrapW, wrapW);
        --EditorGUI.indentLevel;
      }
      EditorGUI.showMixedValue = false;
    }

    protected void DoWrapModePopup()
    {
      TextureInspector.WrapModePopup(this.m_WrapU, this.m_WrapV, this.m_WrapW, this.IsVolume(), ref this.m_ShowPerAxisWrapModes);
    }

    protected void DoFilterModePopup()
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = this.m_FilterMode.hasMultipleDifferentValues;
      UnityEngine.FilterMode filterMode = (UnityEngine.FilterMode) EditorGUILayout.EnumPopup(EditorGUIUtility.TempContent("Filter Mode"), (Enum) (UnityEngine.FilterMode) this.m_FilterMode.intValue, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_FilterMode.intValue = (int) filterMode;
    }

    protected void DoAnisoLevelSlider()
    {
      EditorGUI.BeginChangeCheck();
      EditorGUI.showMixedValue = this.m_Aniso.hasMultipleDifferentValues;
      int anisoLevel = EditorGUILayout.IntSlider("Aniso Level", this.m_Aniso.intValue, 0, 16, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (EditorGUI.EndChangeCheck())
        this.m_Aniso.intValue = anisoLevel;
      TextureInspector.DoAnisoGlobalSettingNote(anisoLevel);
    }

    internal static void DoAnisoGlobalSettingNote(int anisoLevel)
    {
      if (anisoLevel <= 1)
        return;
      switch (QualitySettings.anisotropicFiltering)
      {
        case AnisotropicFiltering.Disable:
          EditorGUILayout.HelpBox("Anisotropic filtering is disabled for all textures in Quality Settings.", MessageType.Info);
          break;
        case AnisotropicFiltering.ForceEnable:
          EditorGUILayout.HelpBox("Anisotropic filtering is enabled for all textures in Quality Settings.", MessageType.Info);
          break;
      }
    }

    private bool IsCubemap()
    {
      Texture target = this.target as Texture;
      return (UnityEngine.Object) target != (UnityEngine.Object) null && target.dimension == TextureDimension.Cube;
    }

    private bool IsVolume()
    {
      Texture target = this.target as Texture;
      return (UnityEngine.Object) target != (UnityEngine.Object) null && target.dimension == TextureDimension.Tex3D;
    }

    public override void OnPreviewSettings()
    {
      if (this.IsCubemap())
      {
        this.m_CubemapPreview.OnPreviewSettings(this.targets);
      }
      else
      {
        if (TextureInspector.s_Styles == null)
          TextureInspector.s_Styles = new TextureInspector.Styles();
        Texture target1 = this.target as Texture;
        bool flag1 = true;
        bool flag2 = false;
        bool flag3 = true;
        int a = 1;
        if (this.target is Texture2D || this.target is ProceduralTexture)
        {
          flag2 = true;
          flag3 = false;
        }
        foreach (Texture target2 in this.targets)
        {
          if (!((UnityEngine.Object) target2 == (UnityEngine.Object) null))
          {
            TextureFormat format = ~(TextureFormat.PVRTC_2BPP_RGB | TextureFormat.RG16 | TextureFormat.ETC_RGB4Crunched);
            bool flag4 = false;
            if (target2 is Texture2D)
            {
              format = (target2 as Texture2D).format;
              flag4 = true;
            }
            else if (target2 is ProceduralTexture)
            {
              format = (target2 as ProceduralTexture).format;
              flag4 = true;
            }
            if (flag4)
            {
              if (!TextureUtil.IsAlphaOnlyTextureFormat(format))
                flag2 = false;
              if (TextureUtil.HasAlphaTextureFormat(format) && TextureUtil.GetUsageMode(target2) == TextureUsageMode.Default)
                flag3 = true;
            }
            a = Mathf.Max(a, TextureUtil.GetMipmapCount(target2));
          }
        }
        if (flag2)
        {
          this.m_ShowAlpha = true;
          flag1 = false;
        }
        else if (!flag3)
        {
          this.m_ShowAlpha = false;
          flag1 = false;
        }
        if (flag1 && (UnityEngine.Object) target1 != (UnityEngine.Object) null && !TextureInspector.IsNormalMap(target1))
          this.m_ShowAlpha = GUILayout.Toggle(this.m_ShowAlpha, !this.m_ShowAlpha ? TextureInspector.s_Styles.RGBIcon : TextureInspector.s_Styles.alphaIcon, TextureInspector.s_Styles.previewButton, new GUILayoutOption[0]);
        if (a <= 1)
          return;
        GUILayout.Box(TextureInspector.s_Styles.smallZoom, TextureInspector.s_Styles.previewLabel, new GUILayoutOption[0]);
        GUI.changed = false;
        this.m_MipLevel = Mathf.Round(GUILayout.HorizontalSlider(this.m_MipLevel, (float) (a - 1), 0.0f, TextureInspector.s_Styles.previewSlider, TextureInspector.s_Styles.previewSliderThumb, GUILayout.MaxWidth(64f)));
        GUILayout.Box(TextureInspector.s_Styles.largeZoom, TextureInspector.s_Styles.previewLabel, new GUILayoutOption[0]);
      }
    }

    public override bool HasPreviewGUI()
    {
      return this.target != (UnityEngine.Object) null;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
      if (Event.current.type == EventType.Repaint)
        background.Draw(r, false, false, false, false);
      Texture target = this.target as Texture;
      if ((UnityEngine.Object) target == (UnityEngine.Object) null)
        return;
      RenderTexture renderTexture = target as RenderTexture;
      if ((UnityEngine.Object) renderTexture != (UnityEngine.Object) null)
      {
        if (!SystemInfo.SupportsRenderTextureFormat(renderTexture.format))
          return;
        renderTexture.Create();
      }
      if (this.IsCubemap())
      {
        this.m_CubemapPreview.OnPreviewGUI(target, r, background);
      }
      else
      {
        int num1 = Mathf.Max(target.width, 1);
        int num2 = Mathf.Max(target.height, 1);
        float levelForRendering = this.GetMipLevelForRendering();
        float num3 = Mathf.Min(Mathf.Min(r.width / (float) num1, r.height / (float) num2), 1f);
        Rect rect1 = new Rect(r.x, r.y, (float) num1 * num3, (float) num2 * num3);
        PreviewGUI.BeginScrollView(r, this.m_Pos, rect1, (GUIStyle) "PreHorizontalScrollbar", (GUIStyle) "PreHorizontalScrollbarThumb");
        float mipMapBias = target.mipMapBias;
        TextureUtil.SetMipMapBiasNoDirty(target, levelForRendering - this.Log2(Mathf.Abs((float) num1 / rect1.width)));
        UnityEngine.FilterMode filterMode = target.filterMode;
        TextureUtil.SetFilterModeNoDirty(target, UnityEngine.FilterMode.Point);
        if (this.m_ShowAlpha)
        {
          EditorGUI.DrawTextureAlpha(rect1, target);
        }
        else
        {
          Texture2D texture2D = target as Texture2D;
          if ((UnityEngine.Object) texture2D != (UnityEngine.Object) null && texture2D.alphaIsTransparency)
            EditorGUI.DrawTextureTransparent(rect1, target);
          else
            EditorGUI.DrawPreviewTexture(rect1, target);
        }
        if ((double) rect1.width > 32.0 && (double) rect1.height > 32.0)
        {
          TextureImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) target)) as TextureImporter;
          SpriteMetaData[] spriteMetaDataArray = !((UnityEngine.Object) atPath != (UnityEngine.Object) null) ? (SpriteMetaData[]) null : atPath.spritesheet;
          if (spriteMetaDataArray != null && atPath.spriteImportMode == SpriteImportMode.Multiple)
          {
            Rect outScreenRect = new Rect();
            Rect outSourceRect = new Rect();
            GUI.CalculateScaledTextureRects(rect1, ScaleMode.StretchToFill, (float) target.width / (float) target.height, ref outScreenRect, ref outSourceRect);
            int width = target.width;
            int height = target.height;
            atPath.GetWidthAndHeight(ref width, ref height);
            float num4 = (float) target.width / (float) width;
            HandleUtility.ApplyWireMaterial();
            GL.PushMatrix();
            GL.MultMatrix(Handles.matrix);
            GL.Begin(1);
            GL.Color(new Color(1f, 1f, 1f, 0.5f));
            foreach (SpriteMetaData spriteMetaData in spriteMetaDataArray)
            {
              Rect rect2 = spriteMetaData.rect;
              this.DrawRect(new Rect()
              {
                xMin = outScreenRect.xMin + outScreenRect.width * (rect2.xMin / (float) target.width * num4),
                xMax = outScreenRect.xMin + outScreenRect.width * (rect2.xMax / (float) target.width * num4),
                yMin = outScreenRect.yMin + outScreenRect.height * (float) (1.0 - (double) rect2.yMin / (double) target.height * (double) num4),
                yMax = outScreenRect.yMin + outScreenRect.height * (float) (1.0 - (double) rect2.yMax / (double) target.height * (double) num4)
              });
            }
            GL.End();
            GL.PopMatrix();
          }
        }
        TextureUtil.SetMipMapBiasNoDirty(target, mipMapBias);
        TextureUtil.SetFilterModeNoDirty(target, filterMode);
        this.m_Pos = PreviewGUI.EndScrollView();
        if ((double) levelForRendering == 0.0)
          return;
        EditorGUI.DropShadowLabel(new Rect(r.x, r.y, r.width, 20f), "Mip " + (object) levelForRendering);
      }
    }

    private void DrawRect(Rect rect)
    {
      GL.Vertex(new Vector3(rect.xMin, rect.yMin, 0.0f));
      GL.Vertex(new Vector3(rect.xMax, rect.yMin, 0.0f));
      GL.Vertex(new Vector3(rect.xMax, rect.yMin, 0.0f));
      GL.Vertex(new Vector3(rect.xMax, rect.yMax, 0.0f));
      GL.Vertex(new Vector3(rect.xMax, rect.yMax, 0.0f));
      GL.Vertex(new Vector3(rect.xMin, rect.yMax, 0.0f));
      GL.Vertex(new Vector3(rect.xMin, rect.yMax, 0.0f));
      GL.Vertex(new Vector3(rect.xMin, rect.yMin, 0.0f));
    }

    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
      if (!ShaderUtil.hardwareSupportsRectRenderTexture)
        return (Texture2D) null;
      Texture target1 = this.target as Texture;
      if (this.IsCubemap())
        return this.m_CubemapPreview.RenderStaticPreview(target1, width, height);
      TextureImporter atPath = AssetImporter.GetAtPath(assetPath) as TextureImporter;
      if ((UnityEngine.Object) atPath != (UnityEngine.Object) null && atPath.textureType == TextureImporterType.Sprite && atPath.spriteImportMode == SpriteImportMode.Polygon)
      {
        if (subAssets.Length <= 0)
          return (Texture2D) null;
        Sprite subAsset = subAssets[0] as Sprite;
        if ((bool) ((UnityEngine.Object) subAsset))
          return SpriteInspector.BuildPreviewTexture(width, height, subAsset, (Material) null, true);
      }
      PreviewHelpers.AdjustWidthAndHeightForStaticPreview(target1.width, target1.height, ref width, ref height);
      RenderTexture active = RenderTexture.active;
      Rect rawViewportRect = ShaderUtil.rawViewportRect;
      bool flag = !TextureUtil.GetLinearSampled(target1);
      RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.Default, !flag ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB);
      Material material = EditorGUI.GetMaterialForSpecialTexture(target1);
      GL.sRGBWrite = QualitySettings.activeColorSpace == ColorSpace.Linear;
      if ((bool) ((UnityEngine.Object) material))
      {
        if (Unsupported.IsDeveloperBuild())
          material = new Material(material);
        Graphics.Blit(target1, temporary, material);
      }
      else
        Graphics.Blit(target1, temporary);
      GL.sRGBWrite = false;
      RenderTexture.active = temporary;
      Texture2D target2 = this.target as Texture2D;
      Texture2D texture2D = !((UnityEngine.Object) target2 != (UnityEngine.Object) null) || !target2.alphaIsTransparency ? new Texture2D(width, height, TextureFormat.RGB24, false) : new Texture2D(width, height, TextureFormat.RGBA32, false);
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) width, (float) height), 0, 0);
      texture2D.Apply();
      RenderTexture.ReleaseTemporary(temporary);
      EditorGUIUtility.SetRenderTextureNoViewport(active);
      ShaderUtil.rawViewportRect = rawViewportRect;
      if ((bool) ((UnityEngine.Object) material) && Unsupported.IsDeveloperBuild())
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) material);
      return texture2D;
    }

    private float Log2(float x)
    {
      return (float) (Math.Log((double) x) / Math.Log(2.0));
    }

    public override string GetInfoString()
    {
      Texture target1 = this.target as Texture;
      Texture2D target2 = this.target as Texture2D;
      TextureImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) target1)) as TextureImporter;
      string str1 = target1.width.ToString() + "x" + target1.height.ToString();
      if (QualitySettings.desiredColorSpace == ColorSpace.Linear)
        str1 = str1 + " " + TextureUtil.GetTextureColorSpaceString(target1);
      bool flag1 = (bool) ((UnityEngine.Object) atPath) && atPath.qualifiesForSpritePacking;
      bool flag2 = TextureInspector.IsNormalMap(target1);
      bool beCompressed = TextureUtil.DoesTextureStillNeedToBeCompressed(AssetDatabase.GetAssetPath((UnityEngine.Object) target1));
      bool flag3 = (UnityEngine.Object) target2 != (UnityEngine.Object) null && TextureUtil.IsNonPowerOfTwo(target2);
      TextureFormat textureFormat = TextureUtil.GetTextureFormat(target1);
      bool flag4 = !beCompressed;
      if (flag3)
        str1 += " (NPOT)";
      string str2;
      if (beCompressed)
        str2 = str1 + " (Not yet compressed)";
      else if (flag2)
      {
        switch (textureFormat)
        {
          case TextureFormat.ARGB4444:
            str2 = str1 + "  Nm 16 bit";
            break;
          case TextureFormat.RGBA32:
          case TextureFormat.ARGB32:
            str2 = str1 + "  Nm 32 bit";
            break;
          default:
            str2 = textureFormat == TextureFormat.DXT5 ? str1 + "  DXTnm" : str1 + "  " + TextureUtil.GetTextureFormatString(textureFormat);
            break;
        }
      }
      else if (flag1)
      {
        TextureFormat desiredFormat;
        ColorSpace colorSpace;
        int compressionQuality;
        atPath.ReadTextureImportInstructions(EditorUserBuildSettings.activeBuildTarget, out desiredFormat, out colorSpace, out compressionQuality);
        str2 = str1 + "\n " + TextureUtil.GetTextureFormatString(textureFormat) + "(Original) " + TextureUtil.GetTextureFormatString(desiredFormat) + "(Atlas)";
      }
      else
        str2 = str1 + "  " + TextureUtil.GetTextureFormatString(textureFormat);
      if (flag4)
        str2 = str2 + "\n" + EditorUtility.FormatBytes(TextureUtil.GetStorageMemorySizeLong(target1));
      if (TextureUtil.GetUsageMode(target1) == TextureUsageMode.AlwaysPadded)
      {
        int gpuWidth = TextureUtil.GetGPUWidth(target1);
        int gpuHeight = TextureUtil.GetGPUHeight(target1);
        if (target1.width != gpuWidth || target1.height != gpuHeight)
          str2 += string.Format("\nPadded to {0}x{1}", (object) gpuWidth, (object) gpuHeight);
      }
      return str2;
    }

    private class Styles
    {
      public readonly GUIContent wrapModeLabel = EditorGUIUtility.TextContent("Wrap Mode");
      public readonly GUIContent wrapU = EditorGUIUtility.TextContent("U axis");
      public readonly GUIContent wrapV = EditorGUIUtility.TextContent("V axis");
      public readonly GUIContent wrapW = EditorGUIUtility.TextContent("W axis");
      public readonly GUIContent[] wrapModeContents = new GUIContent[5]{ EditorGUIUtility.TextContent("Repeat"), EditorGUIUtility.TextContent("Clamp"), EditorGUIUtility.TextContent("Mirror"), EditorGUIUtility.TextContent("Mirror Once"), EditorGUIUtility.TextContent("Per-axis") };
      public readonly int[] wrapModeValues = new int[5]{ 0, 1, 2, 3, -1 };
      public GUIContent smallZoom;
      public GUIContent largeZoom;
      public GUIContent alphaIcon;
      public GUIContent RGBIcon;
      public GUIStyle previewButton;
      public GUIStyle previewSlider;
      public GUIStyle previewSliderThumb;
      public GUIStyle previewLabel;

      public Styles()
      {
        this.smallZoom = EditorGUIUtility.IconContent("PreTextureMipMapLow");
        this.largeZoom = EditorGUIUtility.IconContent("PreTextureMipMapHigh");
        this.alphaIcon = EditorGUIUtility.IconContent("PreTextureAlpha");
        this.RGBIcon = EditorGUIUtility.IconContent("PreTextureRGB");
        this.previewButton = (GUIStyle) "preButton";
        this.previewSlider = (GUIStyle) "preSlider";
        this.previewSliderThumb = (GUIStyle) "preSliderThumb";
        this.previewLabel = new GUIStyle((GUIStyle) "preLabel");
        this.previewLabel.alignment = TextAnchor.UpperCenter;
      }
    }
  }
}
