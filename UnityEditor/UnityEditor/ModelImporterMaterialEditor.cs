// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModelImporterMaterialEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.AssetImporters;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  internal class ModelImporterMaterialEditor : BaseAssetImporterTabUI
  {
    private bool m_ShowAllMaterialNameOptions = true;
    private bool m_ShowMaterialRemapOptions = false;
    private bool m_HasEmbeddedMaterials = false;
    private SerializedProperty m_ImportMaterials;
    private SerializedProperty m_MaterialName;
    private SerializedProperty m_MaterialSearch;
    private SerializedProperty m_MaterialLocation;
    private SerializedProperty m_Materials;
    private SerializedProperty m_ExternalObjects;
    private SerializedProperty m_HasEmbeddedTextures;
    private SerializedProperty m_SupportsEmbeddedMaterials;

    public ModelImporterMaterialEditor(AssetImporterEditor panelContainer)
      : base(panelContainer)
    {
    }

    private void UpdateShowAllMaterialNameOptions()
    {
      this.m_MaterialName = this.serializedObject.FindProperty("m_MaterialName");
      this.m_ShowAllMaterialNameOptions = this.m_MaterialName.intValue == 3;
    }

    private bool HasEmbeddedMaterials()
    {
      if (this.m_Materials.arraySize == 0)
        return false;
      if (this.m_ExternalObjects.serializedObject.hasModifiedProperties)
        return this.m_HasEmbeddedMaterials;
      this.m_HasEmbeddedMaterials = true;
      foreach (UnityEngine.Object targetObject in this.m_ExternalObjects.serializedObject.targetObjects)
      {
        ModelImporter modelImporter = targetObject as ModelImporter;
        Dictionary<AssetImporter.SourceAssetIdentifier, UnityEngine.Object> externalObjectMap = modelImporter.GetExternalObjectMap();
        AssetImporter.SourceAssetIdentifier[] sourceMaterials = modelImporter.sourceMaterials;
        int num = 0;
        foreach (KeyValuePair<AssetImporter.SourceAssetIdentifier, UnityEngine.Object> keyValuePair in externalObjectMap)
        {
          if (keyValuePair.Key.type == typeof (Material))
            ++num;
        }
        this.m_HasEmbeddedMaterials = this.m_HasEmbeddedMaterials && num != sourceMaterials.Length;
      }
      return this.m_HasEmbeddedMaterials;
    }

    internal override void OnEnable()
    {
      this.m_ImportMaterials = this.serializedObject.FindProperty("m_ImportMaterials");
      this.m_MaterialName = this.serializedObject.FindProperty("m_MaterialName");
      this.m_MaterialSearch = this.serializedObject.FindProperty("m_MaterialSearch");
      this.m_MaterialLocation = this.serializedObject.FindProperty("m_MaterialLocation");
      this.m_Materials = this.serializedObject.FindProperty("m_Materials");
      this.m_ExternalObjects = this.serializedObject.FindProperty("m_ExternalObjects");
      this.m_HasEmbeddedTextures = this.serializedObject.FindProperty("m_HasEmbeddedTextures");
      this.m_SupportsEmbeddedMaterials = this.serializedObject.FindProperty("m_SupportsEmbeddedMaterials");
      this.UpdateShowAllMaterialNameOptions();
    }

    internal override void ResetValues()
    {
      base.ResetValues();
      this.UpdateShowAllMaterialNameOptions();
    }

    internal override void PostApply()
    {
      this.UpdateShowAllMaterialNameOptions();
    }

    public override void OnInspectorGUI()
    {
      this.DoMaterialsGUI();
    }

    private void ExtractTexturesGUI()
    {
      using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
      {
        EditorGUILayout.PrefixLabel(ModelImporterMaterialEditor.Styles.Textures);
        using (new EditorGUI.DisabledScope(!this.m_HasEmbeddedTextures.boolValue && !this.m_HasEmbeddedTextures.hasMultipleDifferentValues))
        {
          if (!GUILayout.Button(ModelImporterMaterialEditor.Styles.ExtractEmbeddedTextures))
            return;
          List<Tuple<UnityEngine.Object, string>> tupleList = new List<Tuple<UnityEngine.Object, string>>();
          string path = EditorUtility.SaveFolderPanel("Select Textures Folder", FileUtil.DeleteLastPathNameComponent((this.target as ModelImporter).assetPath), "");
          if (string.IsNullOrEmpty(path))
            return;
          string projectRelativePath = FileUtil.GetProjectRelativePath(path);
          try
          {
            AssetDatabase.StartAssetEditing();
            foreach (UnityEngine.Object target in this.targets)
            {
              string folderPath = FileUtil.GetUniqueTempPathInProject().Replace("Temp", InternalEditorUtility.GetAssetsFolder());
              tupleList.Add(Tuple.Create<UnityEngine.Object, string>(target, folderPath));
              (target as ModelImporter).ExtractTextures(folderPath);
            }
          }
          finally
          {
            AssetDatabase.StopAssetEditing();
          }
          try
          {
            AssetDatabase.Refresh();
            AssetDatabase.StartAssetEditing();
            foreach (Tuple<UnityEngine.Object, string> tuple in tupleList)
            {
              ModelImporter modelImporter = tuple.Item1 as ModelImporter;
              string filter = "t:Texture";
              string[] searchInFolders = new string[1]{ tuple.Item2 };
              foreach (string asset in AssetDatabase.FindAssets(filter, searchInFolders))
              {
                string assetPath = AssetDatabase.GUIDToAssetPath(asset);
                Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(assetPath);
                if (!((UnityEngine.Object) texture == (UnityEngine.Object) null))
                {
                  modelImporter.AddRemap(new AssetImporter.SourceAssetIdentifier((UnityEngine.Object) texture), (UnityEngine.Object) texture);
                  string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(projectRelativePath, FileUtil.UnityGetFileName(assetPath)));
                  AssetDatabase.MoveAsset(assetPath, uniqueAssetPath);
                }
              }
              AssetDatabase.ImportAsset(modelImporter.assetPath, ImportAssetOptions.ForceUpdate);
              AssetDatabase.DeleteAsset(tuple.Item2);
            }
          }
          finally
          {
            AssetDatabase.StopAssetEditing();
          }
        }
      }
    }

    private bool ExtractMaterialsGUI()
    {
      using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
      {
        EditorGUILayout.PrefixLabel(ModelImporterMaterialEditor.Styles.Materials);
        using (new EditorGUI.DisabledScope(!this.HasEmbeddedMaterials()))
        {
          if (GUILayout.Button(ModelImporterMaterialEditor.Styles.ExtractEmbeddedMaterials))
          {
            string path = EditorUtility.SaveFolderPanel("Select Materials Folder", FileUtil.DeleteLastPathNameComponent((this.target as ModelImporter).assetPath), "");
            if (string.IsNullOrEmpty(path))
              return false;
            string projectRelativePath = FileUtil.GetProjectRelativePath(path);
            try
            {
              AssetDatabase.StartAssetEditing();
              PrefabUtility.ExtractMaterialsFromAsset(this.targets, projectRelativePath);
            }
            finally
            {
              AssetDatabase.StopAssetEditing();
            }
            return true;
          }
        }
      }
      return false;
    }

    private bool MaterialRemapOptons()
    {
      this.m_ShowMaterialRemapOptions = EditorGUILayout.Foldout(this.m_ShowMaterialRemapOptions, ModelImporterMaterialEditor.Styles.RemapOptions);
      if (this.m_ShowMaterialRemapOptions)
      {
        ++EditorGUI.indentLevel;
        EditorGUILayout.Popup(this.m_MaterialName, !this.m_ShowAllMaterialNameOptions ? ModelImporterMaterialEditor.Styles.MaterialNameOptMain : ModelImporterMaterialEditor.Styles.MaterialNameOptAll, ModelImporterMaterialEditor.Styles.MaterialName, new GUILayoutOption[0]);
        EditorGUILayout.Popup(this.m_MaterialSearch, ModelImporterMaterialEditor.Styles.MaterialSearchOpt, ModelImporterMaterialEditor.Styles.MaterialSearch, new GUILayoutOption[0]);
        EditorGUILayout.HelpBox(ModelImporterMaterialEditor.Styles.ExternalMaterialHelpStart.text.Replace("%MAT%", ModelImporterMaterialEditor.Styles.ExternalMaterialNameHelp[this.m_MaterialName.intValue].text) + "\n" + ModelImporterMaterialEditor.Styles.ExternalMaterialSearchHelp[this.m_MaterialSearch.intValue].text, MessageType.Info);
        --EditorGUI.indentLevel;
        using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
        {
          GUILayout.FlexibleSpace();
          if (GUILayout.Button(ModelImporterMaterialEditor.Styles.RemapMaterialsInProject))
          {
            try
            {
              AssetDatabase.StartAssetEditing();
              foreach (UnityEngine.Object target in this.targets)
              {
                ModelImporter modelImporter = target as ModelImporter;
                modelImporter.SearchAndRemapMaterials((ModelImporterMaterialName) this.m_MaterialName.intValue, (ModelImporterMaterialSearch) this.m_MaterialSearch.intValue);
                AssetDatabase.WriteImportSettingsIfDirty(modelImporter.assetPath);
                AssetDatabase.ImportAsset(modelImporter.assetPath, ImportAssetOptions.ForceUpdate);
              }
            }
            finally
            {
              AssetDatabase.StopAssetEditing();
            }
            return true;
          }
        }
        EditorGUILayout.Space();
      }
      return false;
    }

    private void DoMaterialsGUI()
    {
      this.serializedObject.UpdateIfRequiredOrScript();
      EditorGUILayout.PropertyField(this.m_ImportMaterials, ModelImporterMaterialEditor.Styles.ImportMaterials, new GUILayoutOption[0]);
      string message = string.Empty;
      if (!this.m_ImportMaterials.hasMultipleDifferentValues)
      {
        if (this.m_ImportMaterials.boolValue)
        {
          EditorGUILayout.Popup(this.m_MaterialLocation, ModelImporterMaterialEditor.Styles.MaterialLocationOpt, ModelImporterMaterialEditor.Styles.MaterialLocation, new GUILayoutOption[0]);
          if (!this.m_MaterialLocation.hasMultipleDifferentValues)
          {
            if (this.m_MaterialLocation.intValue == 0)
            {
              EditorGUILayout.Popup(this.m_MaterialName, !this.m_ShowAllMaterialNameOptions ? ModelImporterMaterialEditor.Styles.MaterialNameOptMain : ModelImporterMaterialEditor.Styles.MaterialNameOptAll, ModelImporterMaterialEditor.Styles.MaterialName, new GUILayoutOption[0]);
              EditorGUILayout.Popup(this.m_MaterialSearch, ModelImporterMaterialEditor.Styles.MaterialSearchOpt, ModelImporterMaterialEditor.Styles.MaterialSearch, new GUILayoutOption[0]);
              message = ModelImporterMaterialEditor.Styles.ExternalMaterialHelpStart.text.Replace("%MAT%", ModelImporterMaterialEditor.Styles.ExternalMaterialNameHelp[this.m_MaterialName.intValue].text) + "\n" + ModelImporterMaterialEditor.Styles.ExternalMaterialSearchHelp[this.m_MaterialSearch.intValue].text + "\n" + ModelImporterMaterialEditor.Styles.ExternalMaterialHelpEnd.text;
            }
            else if (this.m_Materials.arraySize > 0 && this.HasEmbeddedMaterials())
              message = ModelImporterMaterialEditor.Styles.InternalMaterialHelp.text;
          }
          if (this.targets.Length == 1 && this.m_Materials.arraySize > 0 && this.m_MaterialLocation.intValue != 0)
            message = message + " " + ModelImporterMaterialEditor.Styles.MaterialAssignmentsHelp.text;
          if (this.m_MaterialLocation.intValue != 0 && !this.m_MaterialLocation.hasMultipleDifferentValues)
          {
            this.ExtractTexturesGUI();
            if (this.ExtractMaterialsGUI())
              return;
          }
        }
        else
          message = ModelImporterMaterialEditor.Styles.NoMaterialHelp.text;
      }
      if (!string.IsNullOrEmpty(message))
        EditorGUILayout.HelpBox(message, MessageType.Info);
      if ((this.targets.Length == 1 || !this.m_SupportsEmbeddedMaterials.hasMultipleDifferentValues) && (!this.m_SupportsEmbeddedMaterials.boolValue && this.m_MaterialLocation.intValue != 0) && !this.m_MaterialLocation.hasMultipleDifferentValues)
      {
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(ModelImporterMaterialEditor.Styles.NoMaterialMappingsHelp.text, MessageType.Warning);
      }
      if (!this.m_ImportMaterials.boolValue || this.targets.Length != 1 || (this.m_Materials.arraySize <= 0 || this.m_MaterialLocation.intValue == 0) || this.m_MaterialLocation.hasMultipleDifferentValues)
        return;
      GUILayout.Label(ModelImporterMaterialEditor.Styles.ExternalMaterialMappings, EditorStyles.boldLabel, new GUILayoutOption[0]);
      if (this.MaterialRemapOptons())
        return;
      for (int index1 = 0; index1 < this.m_Materials.arraySize; ++index1)
      {
        SerializedProperty arrayElementAtIndex1 = this.m_Materials.GetArrayElementAtIndex(index1);
        string stringValue1 = arrayElementAtIndex1.FindPropertyRelative("name").stringValue;
        string stringValue2 = arrayElementAtIndex1.FindPropertyRelative("type").stringValue;
        string stringValue3 = arrayElementAtIndex1.FindPropertyRelative("assembly").stringValue;
        SerializedProperty property = (SerializedProperty) null;
        Material material1 = (Material) null;
        int index2 = 0;
        int index3 = 0;
        for (int arraySize = this.m_ExternalObjects.arraySize; index3 < arraySize; ++index3)
        {
          SerializedProperty arrayElementAtIndex2 = this.m_ExternalObjects.GetArrayElementAtIndex(index3);
          string stringValue4 = arrayElementAtIndex2.FindPropertyRelative("first.name").stringValue;
          string stringValue5 = arrayElementAtIndex2.FindPropertyRelative("first.type").stringValue;
          if (stringValue4 == stringValue1 && stringValue5 == stringValue2)
          {
            property = arrayElementAtIndex2.FindPropertyRelative("second");
            material1 = property == null ? (Material) null : property.objectReferenceValue as Material;
            index2 = index3;
            break;
          }
        }
        GUIContent label = EditorGUIUtility.TextContent(stringValue1);
        label.tooltip = stringValue1;
        if (property != null)
        {
          EditorGUI.BeginChangeCheck();
          EditorGUILayout.ObjectField(property, typeof (Material), label, new GUILayoutOption[0]);
          if (EditorGUI.EndChangeCheck() && property.objectReferenceValue == (UnityEngine.Object) null)
            this.m_ExternalObjects.DeleteArrayElementAtIndex(index2);
        }
        else
        {
          EditorGUI.BeginChangeCheck();
          Material material2 = EditorGUILayout.ObjectField(label, (UnityEngine.Object) material1, typeof (Material), false, new GUILayoutOption[0]) as Material;
          if (EditorGUI.EndChangeCheck() && (UnityEngine.Object) material2 != (UnityEngine.Object) null)
          {
            SerializedProperty arrayElementAtIndex2 = this.m_ExternalObjects.GetArrayElementAtIndex(this.m_ExternalObjects.arraySize++);
            arrayElementAtIndex2.FindPropertyRelative("first.name").stringValue = stringValue1;
            arrayElementAtIndex2.FindPropertyRelative("first.type").stringValue = stringValue2;
            arrayElementAtIndex2.FindPropertyRelative("first.assembly").stringValue = stringValue3;
            arrayElementAtIndex2.FindPropertyRelative("second").objectReferenceValue = (UnityEngine.Object) material2;
          }
        }
      }
    }

    private static class Styles
    {
      public static GUIContent ImportMaterials = EditorGUIUtility.TextContent("Import Materials");
      public static GUIContent MaterialLocation = EditorGUIUtility.TextContent("Location");
      public static GUIContent[] MaterialLocationOpt = new GUIContent[2]{ EditorGUIUtility.TextContent("Use External Materials (Legacy)|Use external materials if found in the project."), EditorGUIUtility.TextContent("Use Embedded Materials|Embed the material inside the imported asset.") };
      public static GUIContent MaterialName = EditorGUIUtility.TextContent("Naming");
      public static GUIContent[] MaterialNameOptMain = new GUIContent[3]{ EditorGUIUtility.TextContent("By Base Texture Name"), EditorGUIUtility.TextContent("From Model's Material"), EditorGUIUtility.TextContent("Model Name + Model's Material") };
      public static GUIContent[] MaterialNameOptAll = new GUIContent[4]{ EditorGUIUtility.TextContent("By Base Texture Name"), EditorGUIUtility.TextContent("From Model's Material"), EditorGUIUtility.TextContent("Model Name + Model's Material"), EditorGUIUtility.TextContent("Texture Name or Model Name + Model's Material (Obsolete)") };
      public static GUIContent MaterialSearch = EditorGUIUtility.TextContent("Search");
      public static GUIContent[] MaterialSearchOpt = new GUIContent[3]{ EditorGUIUtility.TextContent("Local Materials Folder"), EditorGUIUtility.TextContent("Recursive-Up"), EditorGUIUtility.TextContent("Project-Wide") };
      public static GUIContent NoMaterialHelp = EditorGUIUtility.TextContent("Do not generate materials. Use Unity's default material instead.");
      public static GUIContent ExternalMaterialHelpStart = EditorGUIUtility.TextContent("For each imported material, Unity first looks for an existing material named %MAT%.");
      public static GUIContent[] ExternalMaterialNameHelp = new GUIContent[4]{ EditorGUIUtility.TextContent("[BaseTextureName]"), EditorGUIUtility.TextContent("[MaterialName]"), EditorGUIUtility.TextContent("[ModelFileName]-[MaterialName]"), EditorGUIUtility.TextContent("[BaseTextureName] or [ModelFileName]-[MaterialName] if no base texture can be found") };
      public static GUIContent[] ExternalMaterialSearchHelp = new GUIContent[3]{ EditorGUIUtility.TextContent("Unity will look for it in the local Materials folder."), EditorGUIUtility.TextContent("Unity will do a recursive-up search for it in all Materials folders up to the Assets folder."), EditorGUIUtility.TextContent("Unity will search for it anywhere inside the Assets folder.") };
      public static GUIContent ExternalMaterialHelpEnd = EditorGUIUtility.TextContent("If it doesn't exist, a new one is created in the local Materials folder.");
      public static GUIContent InternalMaterialHelp = EditorGUIUtility.TextContent("Materials are embedded inside the imported asset.");
      public static GUIContent MaterialAssignmentsHelp = EditorGUIUtility.TextContent("Material assignments can be remapped below.");
      public static GUIContent ExternalMaterialMappings = EditorGUIUtility.TextContent("Remapped Materials|External materials to use for each embedded material.");
      public static GUIContent NoMaterialMappingsHelp = EditorGUIUtility.TextContent("Re-import the asset to see the list of used materials.");
      public static GUIContent Textures = EditorGUIUtility.TextContent(nameof (Textures));
      public static GUIContent ExtractEmbeddedTextures = EditorGUIUtility.TextContent("Extract Textures...|Click on this button to extract the embedded textures.");
      public static GUIContent Materials = EditorGUIUtility.TextContent(nameof (Materials));
      public static GUIContent ExtractEmbeddedMaterials = EditorGUIUtility.TextContent("Extract Materials...|Click on this button to extract the embedded materials.");
      public static GUIContent RemapOptions = EditorGUIUtility.TextContent("On Demand Remap");
      public static GUIContent RemapMaterialsInProject = EditorGUIUtility.TextContent("Search and Remap|Click on this button to search and remap the materials from the project.");
    }
  }
}
