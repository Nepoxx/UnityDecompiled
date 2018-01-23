// Decompiled with JetBrains decompiler
// Type: UnityEditor.TrueTypeFontImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (TrueTypeFontImporter))]
  [CanEditMultipleObjects]
  internal class TrueTypeFontImporterInspector : AssetImporterEditor
  {
    private static GUIContent[] kCharacterStrings = new GUIContent[6]{ new GUIContent("Dynamic"), new GUIContent("Unicode"), new GUIContent("ASCII default set"), new GUIContent("ASCII upper case"), new GUIContent("ASCII lower case"), new GUIContent("Custom set") };
    private static int[] kCharacterValues = new int[6]{ -2, -1, 0, 1, 2, 3 };
    private static GUIContent[] kRenderingModeStrings = new GUIContent[4]{ new GUIContent("Smooth"), new GUIContent("Hinted Smooth"), new GUIContent("Hinted Raster"), new GUIContent("OS Default") };
    private static int[] kRenderingModeValues = new int[4]{ 0, 1, 2, 3 };
    private static GUIContent[] kAscentCalculationModeStrings = new GUIContent[3]{ new GUIContent("Legacy version 2 mode (glyph bounding boxes)"), new GUIContent("Face ascender metric"), new GUIContent("Face bounding box metric") };
    private static int[] kAscentCalculationModeValues = new int[3]{ 0, 1, 2 };
    private string m_FontNamesString = "";
    private string m_DefaultFontNamesString = "";
    private bool? m_FormatSupported = new bool?();
    private bool m_ReferencesExpanded = false;
    private bool m_GotFontNamesFromImporter = false;
    private Font[] m_FallbackFontReferences = (Font[]) null;
    private SerializedProperty m_FontSize;
    private SerializedProperty m_TextureCase;
    private SerializedProperty m_IncludeFontData;
    private SerializedProperty m_FontNamesArraySize;
    private SerializedProperty m_CustomCharacters;
    private SerializedProperty m_FontRenderingMode;
    private SerializedProperty m_AscentCalculationMode;
    private SerializedProperty m_UseLegacyBoundsCalculation;
    private SerializedProperty m_FallbackFontReferencesArraySize;

    public override void OnEnable()
    {
      this.m_FontSize = this.serializedObject.FindProperty("m_FontSize");
      this.m_TextureCase = this.serializedObject.FindProperty("m_ForceTextureCase");
      this.m_IncludeFontData = this.serializedObject.FindProperty("m_IncludeFontData");
      this.m_FontNamesArraySize = this.serializedObject.FindProperty("m_FontNames.Array.size");
      this.m_CustomCharacters = this.serializedObject.FindProperty("m_CustomCharacters");
      this.m_FontRenderingMode = this.serializedObject.FindProperty("m_FontRenderingMode");
      this.m_AscentCalculationMode = this.serializedObject.FindProperty("m_AscentCalculationMode");
      this.m_UseLegacyBoundsCalculation = this.serializedObject.FindProperty("m_UseLegacyBoundsCalculation");
      this.m_FallbackFontReferencesArraySize = this.serializedObject.FindProperty("m_FallbackFontReferences.Array.size");
      if (this.targets.Length != 1)
        return;
      this.m_DefaultFontNamesString = this.GetDefaultFontNames();
      this.m_FontNamesString = this.GetFontNames();
      this.m_FallbackFontReferences = ((TrueTypeFontImporter) this.target).fontReferences;
    }

    protected override void Apply()
    {
      this.m_FallbackFontReferencesArraySize.intValue = this.m_FallbackFontReferences.Length;
      SerializedProperty serializedProperty = this.m_FallbackFontReferencesArraySize.Copy();
      for (int index = 0; index < this.m_FallbackFontReferences.Length; ++index)
      {
        serializedProperty.Next(false);
        serializedProperty.objectReferenceValue = (UnityEngine.Object) this.m_FallbackFontReferences[index];
      }
      base.Apply();
    }

    private string GetDefaultFontNames()
    {
      return ((TrueTypeFontImporter) this.target).fontTTFName;
    }

    private string GetFontNames()
    {
      string str = string.Join(", ", ((TrueTypeFontImporter) this.target).fontNames);
      if (string.IsNullOrEmpty(str))
        str = this.m_DefaultFontNamesString;
      else
        this.m_GotFontNamesFromImporter = true;
      return str;
    }

    private void SetFontNames(string fontNames)
    {
      string[] _names;
      if (!this.m_GotFontNamesFromImporter)
      {
        _names = new string[0];
      }
      else
      {
        _names = fontNames.Split(',');
        for (int index = 0; index < _names.Length; ++index)
          _names[index] = _names[index].Trim();
      }
      this.m_FontNamesArraySize.intValue = _names.Length;
      SerializedProperty serializedProperty = this.m_FontNamesArraySize.Copy();
      for (int index = 0; index < _names.Length; ++index)
      {
        serializedProperty.Next(false);
        serializedProperty.stringValue = _names[index];
      }
      this.m_FallbackFontReferences = ((TrueTypeFontImporter) this.target).LookupFallbackFontReferences(_names);
    }

    private void ShowFormatUnsupportedGUI()
    {
      GUILayout.Space(5f);
      EditorGUILayout.HelpBox("Format of selected font is not supported by Unity.", MessageType.Warning);
    }

    private static string GetUniquePath(string basePath, string extension)
    {
      for (int index = 0; index < 10000; ++index)
      {
        string path = string.Format("{0}{1}.{2}", (object) basePath, index != 0 ? (object) index.ToString() : (object) string.Empty, (object) extension);
        if (!File.Exists(path))
          return path;
      }
      return "";
    }

    [MenuItem("CONTEXT/TrueTypeFontImporter/Create Editable Copy")]
    private static void CreateEditableCopy(MenuCommand command)
    {
      TrueTypeFontImporter context = (TrueTypeFontImporter) command.context;
      if (context.fontTextureCase == FontTextureCase.Dynamic)
      {
        EditorUtility.DisplayDialog("Cannot generate editable font asset for dynamic fonts", "Please reimport the font in a different mode.", "Ok");
      }
      else
      {
        string str = Path.Combine(Path.GetDirectoryName(context.assetPath), Path.GetFileNameWithoutExtension(context.assetPath));
        EditorGUIUtility.PingObject((UnityEngine.Object) context.GenerateEditableFont(TrueTypeFontImporterInspector.GetUniquePath(str + "_copy", "fontsettings")));
      }
    }

    public override void OnInspectorGUI()
    {
      if (!this.m_FormatSupported.HasValue)
      {
        this.m_FormatSupported = new bool?(true);
        foreach (UnityEngine.Object target in this.targets)
        {
          TrueTypeFontImporter typeFontImporter = target as TrueTypeFontImporter;
          if ((UnityEngine.Object) typeFontImporter == (UnityEngine.Object) null || !typeFontImporter.IsFormatSupported())
            this.m_FormatSupported = new bool?(false);
        }
      }
      bool? formatSupported = this.m_FormatSupported;
      if ((formatSupported.GetValueOrDefault() ? 0 : (formatSupported.HasValue ? 1 : 0)) != 0)
      {
        this.ShowFormatUnsupportedGUI();
      }
      else
      {
        EditorGUILayout.PropertyField(this.m_FontSize);
        if (this.m_FontSize.intValue < 1)
          this.m_FontSize.intValue = 1;
        if (this.m_FontSize.intValue > 500)
          this.m_FontSize.intValue = 500;
        EditorGUILayout.IntPopup(this.m_FontRenderingMode, TrueTypeFontImporterInspector.kRenderingModeStrings, TrueTypeFontImporterInspector.kRenderingModeValues, new GUIContent("Rendering Mode"), new GUILayoutOption[0]);
        EditorGUILayout.IntPopup(this.m_TextureCase, TrueTypeFontImporterInspector.kCharacterStrings, TrueTypeFontImporterInspector.kCharacterValues, new GUIContent("Character"), new GUILayoutOption[0]);
        EditorGUILayout.IntPopup(this.m_AscentCalculationMode, TrueTypeFontImporterInspector.kAscentCalculationModeStrings, TrueTypeFontImporterInspector.kAscentCalculationModeValues, new GUIContent("Ascent Calculation Mode"), new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_UseLegacyBoundsCalculation, new GUIContent("Use Legacy Bounds"), new GUILayoutOption[0]);
        if (!this.m_TextureCase.hasMultipleDifferentValues)
        {
          if (this.m_TextureCase.intValue != -2)
          {
            if (this.m_TextureCase.intValue == 3)
            {
              EditorGUI.BeginChangeCheck();
              GUILayout.BeginHorizontal();
              EditorGUILayout.PrefixLabel("Custom Chars");
              EditorGUI.showMixedValue = this.m_CustomCharacters.hasMultipleDifferentValues;
              string source = EditorGUILayout.TextArea(this.m_CustomCharacters.stringValue, GUI.skin.textArea, GUILayout.MinHeight(32f));
              EditorGUI.showMixedValue = false;
              GUILayout.EndHorizontal();
              if (EditorGUI.EndChangeCheck())
                this.m_CustomCharacters.stringValue = new string(source.Distinct<char>().Where<char>((Func<char, bool>) (c => (int) c != 10 && (int) c != 13)).ToArray<char>());
            }
          }
          else
          {
            EditorGUILayout.PropertyField(this.m_IncludeFontData, new GUIContent("Incl. Font Data"), new GUILayoutOption[0]);
            if (this.targets.Length == 1)
            {
              EditorGUI.BeginChangeCheck();
              GUILayout.BeginHorizontal();
              EditorGUILayout.PrefixLabel("Font Names");
              GUI.SetNextControlName("fontnames");
              this.m_FontNamesString = EditorGUILayout.TextArea(this.m_FontNamesString, (GUIStyle) "TextArea", GUILayout.MinHeight(32f));
              GUILayout.EndHorizontal();
              GUILayout.BeginHorizontal();
              GUILayout.FlexibleSpace();
              using (new EditorGUI.DisabledScope(this.m_FontNamesString == this.m_DefaultFontNamesString))
              {
                if (GUILayout.Button("Reset", (GUIStyle) "MiniButton", new GUILayoutOption[0]))
                {
                  GUI.changed = true;
                  if (GUI.GetNameOfFocusedControl() == "fontnames")
                    GUIUtility.keyboardControl = 0;
                  this.m_FontNamesString = this.m_DefaultFontNamesString;
                }
              }
              GUILayout.EndHorizontal();
              if (EditorGUI.EndChangeCheck())
                this.SetFontNames(this.m_FontNamesString);
              this.m_ReferencesExpanded = EditorGUILayout.Foldout(this.m_ReferencesExpanded, "References to other fonts in project", true);
              if (this.m_ReferencesExpanded)
              {
                EditorGUILayout.HelpBox("These are automatically generated by the inspector if any of the font names you supplied match fonts present in your project, which will then be used as fallbacks for this font.", MessageType.Info);
                using (new EditorGUI.DisabledScope(true))
                {
                  if (this.m_FallbackFontReferences != null && this.m_FallbackFontReferences.Length > 0)
                  {
                    for (int index = 0; index < this.m_FallbackFontReferences.Length; ++index)
                      EditorGUILayout.ObjectField((UnityEngine.Object) this.m_FallbackFontReferences[index], typeof (Font), false, new GUILayoutOption[0]);
                  }
                  else
                    GUILayout.Label("No references to other fonts in project.");
                }
              }
            }
          }
        }
        this.ApplyRevertGUI();
      }
    }
  }
}
