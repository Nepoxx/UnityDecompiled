// Decompiled with JetBrains decompiler
// Type: UnityEditor.IHVImageFormatImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (IHVImageFormatImporter))]
  internal class IHVImageFormatImporterInspector : AssetImporterEditor
  {
    private bool m_ShowPerAxisWrapModes = false;
    private SerializedProperty m_IsReadable;
    private SerializedProperty m_sRGBTexture;
    private SerializedProperty m_FilterMode;
    private SerializedProperty m_WrapU;
    private SerializedProperty m_WrapV;
    private SerializedProperty m_WrapW;

    public override bool showImportedObject
    {
      get
      {
        return false;
      }
    }

    public override void OnEnable()
    {
      this.m_IsReadable = this.serializedObject.FindProperty("m_IsReadable");
      this.m_sRGBTexture = this.serializedObject.FindProperty("m_sRGBTexture");
      this.m_FilterMode = this.serializedObject.FindProperty("m_TextureSettings.m_FilterMode");
      this.m_WrapU = this.serializedObject.FindProperty("m_TextureSettings.m_WrapU");
      this.m_WrapV = this.serializedObject.FindProperty("m_TextureSettings.m_WrapV");
      this.m_WrapW = this.serializedObject.FindProperty("m_TextureSettings.m_WrapW");
    }

    public void TextureSettingsGUI()
    {
      TextureInspector.WrapModePopup(this.m_WrapU, this.m_WrapV, this.m_WrapW, false, ref this.m_ShowPerAxisWrapModes);
      Rect controlRect = EditorGUILayout.GetControlRect();
      EditorGUI.BeginProperty(controlRect, IHVImageFormatImporterInspector.Styles.filterMode, this.m_FilterMode);
      EditorGUI.BeginChangeCheck();
      UnityEngine.FilterMode filterMode1 = this.m_FilterMode.intValue != -1 ? (UnityEngine.FilterMode) this.m_FilterMode.intValue : UnityEngine.FilterMode.Bilinear;
      UnityEngine.FilterMode filterMode2 = (UnityEngine.FilterMode) EditorGUI.IntPopup(controlRect, IHVImageFormatImporterInspector.Styles.filterMode, (int) filterMode1, IHVImageFormatImporterInspector.Styles.filterModeOptions, IHVImageFormatImporterInspector.Styles.filterModeValues);
      if (EditorGUI.EndChangeCheck())
        this.m_FilterMode.intValue = (int) filterMode2;
      EditorGUI.EndProperty();
    }

    public override void OnInspectorGUI()
    {
      EditorGUILayout.PropertyField(this.m_IsReadable, IHVImageFormatImporterInspector.Styles.readWrite, new GUILayoutOption[0]);
      EditorGUILayout.PropertyField(this.m_sRGBTexture, IHVImageFormatImporterInspector.Styles.sRGBTexture, new GUILayoutOption[0]);
      EditorGUI.BeginChangeCheck();
      this.TextureSettingsGUI();
      if (EditorGUI.EndChangeCheck())
      {
        foreach (AssetImporter target in this.targets)
        {
          Texture tex = AssetDatabase.LoadMainAssetAtPath(target.assetPath) as Texture;
          if (this.m_FilterMode.intValue != -1)
            TextureUtil.SetFilterModeNoDirty(tex, (UnityEngine.FilterMode) this.m_FilterMode.intValue);
          if ((this.m_WrapU.intValue != -1 || this.m_WrapV.intValue != -1 || this.m_WrapW.intValue != -1) && (!this.m_WrapU.hasMultipleDifferentValues && !this.m_WrapV.hasMultipleDifferentValues && !this.m_WrapW.hasMultipleDifferentValues))
            TextureUtil.SetWrapModeNoDirty(tex, (TextureWrapMode) this.m_WrapU.intValue, (TextureWrapMode) this.m_WrapV.intValue, (TextureWrapMode) this.m_WrapW.intValue);
        }
        SceneView.RepaintAll();
      }
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      this.ApplyRevertGUI();
      GUILayout.EndHorizontal();
    }

    internal class Styles
    {
      public static readonly GUIContent readWrite = EditorGUIUtility.TextContent("Read/Write Enabled|Enable to be able to access the raw pixel data from code.");
      public static readonly GUIContent sRGBTexture = EditorGUIUtility.TextContent("sRGB (Color Texture)|Texture content is stored in gamma space. Non-HDR color textures should enable this flag (except if used for IMGUI).");
      public static readonly GUIContent wrapMode = EditorGUIUtility.TextContent("Wrap Mode");
      public static readonly GUIContent filterMode = EditorGUIUtility.TextContent("Filter Mode");
      public static readonly int[] filterModeValues = new int[3]{ 0, 1, 2 };
      public static readonly GUIContent[] filterModeOptions = new GUIContent[3]{ EditorGUIUtility.TextContent("Point (no filter)"), EditorGUIUtility.TextContent("Bilinear"), EditorGUIUtility.TextContent("Trilinear") };
    }
  }
}
