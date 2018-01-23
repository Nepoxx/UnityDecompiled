// Decompiled with JetBrains decompiler
// Type: UnityEditor.RenderTextureEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
  [CustomEditor(typeof (RenderTexture))]
  [CanEditMultipleObjects]
  internal class RenderTextureEditor : TextureInspector
  {
    private static RenderTextureEditor.Styles s_Styles = (RenderTextureEditor.Styles) null;
    private const RenderTextureEditor.GUIElements s_AllGUIElements = RenderTextureEditor.GUIElements.RenderTargetDepthGUI | RenderTextureEditor.GUIElements.RenderTargetAAGUI;
    private SerializedProperty m_Width;
    private SerializedProperty m_Height;
    private SerializedProperty m_Depth;
    private SerializedProperty m_ColorFormat;
    private SerializedProperty m_DepthFormat;
    private SerializedProperty m_AntiAliasing;
    private SerializedProperty m_EnableMipmaps;
    private SerializedProperty m_AutoGeneratesMipmaps;
    private SerializedProperty m_Dimension;
    private SerializedProperty m_sRGB;
    private SerializedProperty m_UseDynamicScale;

    private static RenderTextureEditor.Styles styles
    {
      get
      {
        if (RenderTextureEditor.s_Styles == null)
          RenderTextureEditor.s_Styles = new RenderTextureEditor.Styles();
        return RenderTextureEditor.s_Styles;
      }
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      this.m_Width = this.serializedObject.FindProperty("m_Width");
      this.m_Height = this.serializedObject.FindProperty("m_Height");
      this.m_Depth = this.serializedObject.FindProperty("m_VolumeDepth");
      this.m_AntiAliasing = this.serializedObject.FindProperty("m_AntiAliasing");
      this.m_ColorFormat = this.serializedObject.FindProperty("m_ColorFormat");
      this.m_DepthFormat = this.serializedObject.FindProperty("m_DepthFormat");
      this.m_EnableMipmaps = this.serializedObject.FindProperty("m_MipMap");
      this.m_AutoGeneratesMipmaps = this.serializedObject.FindProperty("m_GenerateMips");
      this.m_Dimension = this.serializedObject.FindProperty("m_Dimension");
      this.m_sRGB = this.serializedObject.FindProperty("m_SRGB");
      this.m_UseDynamicScale = this.serializedObject.FindProperty("m_UseDynamicScale");
    }

    public static bool IsHDRFormat(RenderTextureFormat format)
    {
      return format == RenderTextureFormat.ARGBHalf || format == RenderTextureFormat.RGB111110Float || (format == RenderTextureFormat.RGFloat || format == RenderTextureFormat.ARGBFloat) || (format == RenderTextureFormat.ARGBFloat || format == RenderTextureFormat.RFloat || format == RenderTextureFormat.RGHalf) || format == RenderTextureFormat.RHalf;
    }

    protected void OnRenderTextureGUI(RenderTextureEditor.GUIElements guiElements)
    {
      GUI.changed = false;
      bool disabled = this.m_Dimension.intValue == 3;
      EditorGUILayout.IntPopup(this.m_Dimension, RenderTextureEditor.styles.dimensionStrings, RenderTextureEditor.styles.dimensionValues, RenderTextureEditor.styles.dimension, new GUILayoutOption[0]);
      GUILayout.BeginHorizontal();
      EditorGUILayout.PrefixLabel(RenderTextureEditor.styles.size, EditorStyles.popup);
      EditorGUILayout.DelayedIntField(this.m_Width, GUIContent.none, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(40f)
      });
      GUILayout.Label(RenderTextureEditor.styles.cross);
      EditorGUILayout.DelayedIntField(this.m_Height, GUIContent.none, new GUILayoutOption[1]
      {
        GUILayout.MinWidth(40f)
      });
      if (disabled)
      {
        GUILayout.Label(RenderTextureEditor.styles.cross);
        EditorGUILayout.DelayedIntField(this.m_Depth, GUIContent.none, new GUILayoutOption[1]
        {
          GUILayout.MinWidth(40f)
        });
      }
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      if ((guiElements & RenderTextureEditor.GUIElements.RenderTargetAAGUI) != RenderTextureEditor.GUIElements.RenderTargetNoneGUI)
        EditorGUILayout.IntPopup(this.m_AntiAliasing, RenderTextureEditor.styles.renderTextureAntiAliasing, RenderTextureEditor.styles.renderTextureAntiAliasingValues, RenderTextureEditor.styles.antiAliasing, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_ColorFormat, RenderTextureEditor.styles.colorFormat, new GUILayoutOption[0]);
      if ((guiElements & RenderTextureEditor.GUIElements.RenderTargetDepthGUI) != RenderTextureEditor.GUIElements.RenderTargetNoneGUI)
        EditorGUILayout.PropertyField(this.m_DepthFormat, RenderTextureEditor.styles.depthBuffer, new GUILayoutOption[0]);
      using (new EditorGUI.DisabledScope(RenderTextureEditor.IsHDRFormat((RenderTextureFormat) this.m_ColorFormat.intValue)))
        EditorGUILayout.PropertyField(this.m_sRGB, RenderTextureEditor.styles.sRGBTexture, new GUILayoutOption[0]);
      using (new EditorGUI.DisabledScope(disabled))
      {
        EditorGUILayout.PropertyField(this.m_EnableMipmaps, RenderTextureEditor.styles.enableMipmaps, new GUILayoutOption[0]);
        using (new EditorGUI.DisabledScope(!this.m_EnableMipmaps.boolValue))
          EditorGUILayout.PropertyField(this.m_AutoGeneratesMipmaps, RenderTextureEditor.styles.autoGeneratesMipmaps, new GUILayoutOption[0]);
      }
      if (disabled)
        EditorGUILayout.HelpBox("3D RenderTextures do not support Mip Maps.", MessageType.Info);
      EditorGUILayout.PropertyField(this.m_UseDynamicScale, RenderTextureEditor.styles.useDynamicScale, new GUILayoutOption[0]);
      RenderTexture target = this.target as RenderTexture;
      if (GUI.changed && (UnityEngine.Object) target != (UnityEngine.Object) null)
        target.Release();
      EditorGUILayout.Space();
      this.DoWrapModePopup();
      this.DoFilterModePopup();
      using (new EditorGUI.DisabledScope(this.RenderTextureHasDepth()))
        this.DoAnisoLevelSlider();
      if (!this.RenderTextureHasDepth())
        return;
      this.m_Aniso.intValue = 0;
      EditorGUILayout.HelpBox("RenderTextures with depth must have an Aniso Level of 0.", MessageType.Info);
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      this.OnRenderTextureGUI(RenderTextureEditor.GUIElements.RenderTargetDepthGUI | RenderTextureEditor.GUIElements.RenderTargetAAGUI);
      this.serializedObject.ApplyModifiedProperties();
    }

    private bool RenderTextureHasDepth()
    {
      if (TextureUtil.IsDepthRTFormat((RenderTextureFormat) this.m_ColorFormat.enumValueIndex))
        return true;
      return this.m_DepthFormat.enumValueIndex != 0;
    }

    public override string GetInfoString()
    {
      RenderTexture target = this.target as RenderTexture;
      string str = target.width.ToString() + "x" + (object) target.height;
      if (target.dimension == TextureDimension.Tex3D)
        str = str + "x" + (object) target.volumeDepth;
      if (!target.isPowerOfTwo)
        str += "(NPOT)";
      if (QualitySettings.desiredColorSpace == ColorSpace.Linear)
      {
        bool flag1 = RenderTextureEditor.IsHDRFormat(target.format);
        bool flag2 = target.sRGB && !flag1;
        str = str + " " + (!flag2 ? "Linear" : "sRGB");
      }
      return str + "  " + (object) target.format + "  " + EditorUtility.FormatBytes(TextureUtil.GetRuntimeMemorySizeLong((Texture) target));
    }

    private class Styles
    {
      public readonly GUIContent size = EditorGUIUtility.TextContent("Size|Size of the render texture in pixels.");
      public readonly GUIContent cross = EditorGUIUtility.TextContent("x");
      public readonly GUIContent antiAliasing = EditorGUIUtility.TextContent("Anti-Aliasing|Number of anti-aliasing samples.");
      public readonly GUIContent colorFormat = EditorGUIUtility.TextContent("Color Format|Format of the color buffer.");
      public readonly GUIContent depthBuffer = EditorGUIUtility.TextContent("Depth Buffer|Format of the depth buffer.");
      public readonly GUIContent dimension = EditorGUIUtility.TextContent("Dimension|Is the texture 2D, Cube or 3D?");
      public readonly GUIContent enableMipmaps = EditorGUIUtility.TextContent("Enable Mip Maps|This render texture will have Mip Maps.");
      public readonly GUIContent bindMS = EditorGUIUtility.TextContent("Bind multisampled|If enabled, the texture will not go through an AA resolve if bound to a shader.");
      public readonly GUIContent autoGeneratesMipmaps = EditorGUIUtility.TextContent("Auto generate Mip Maps|This render texture automatically generate its Mip Maps.");
      public readonly GUIContent sRGBTexture = EditorGUIUtility.TextContent("sRGB (Color RenderTexture)|RenderTexture content is stored in gamma space. Non-HDR color textures should enable this flag.");
      public readonly GUIContent useDynamicScale = EditorGUIUtility.TextContent("Dynamic Scaling|Allow the texture to be automatically resized by ScalableBufferManager, to support dynamic resolution.");
      public readonly GUIContent[] renderTextureAntiAliasing = new GUIContent[4]{ new GUIContent("None"), new GUIContent("2 samples"), new GUIContent("4 samples"), new GUIContent("8 samples") };
      public readonly int[] renderTextureAntiAliasingValues = new int[4]{ 1, 2, 4, 8 };
      public readonly GUIContent[] dimensionStrings = new GUIContent[3]{ EditorGUIUtility.TextContent("2D"), EditorGUIUtility.TextContent("Cube"), EditorGUIUtility.TextContent("3D") };
      public readonly int[] dimensionValues = new int[3]{ 2, 4, 3 };
    }

    [System.Flags]
    protected enum GUIElements
    {
      RenderTargetNoneGUI = 0,
      RenderTargetDepthGUI = 2,
      RenderTargetAAGUI = 4,
    }
  }
}
