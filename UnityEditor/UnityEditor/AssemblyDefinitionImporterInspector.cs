// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssemblyDefinitionImporterInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Compilation;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.Scripting.ScriptCompilation;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (AssemblyDefinitionImporter))]
  [CanEditMultipleObjects]
  internal class AssemblyDefinitionImporterInspector : AssetImporterEditor
  {
    private GUIStyle m_TextStyle;
    private AssemblyDefinitionImporterInspector.AssemblyDefintionState[] m_TargetStates;
    private AssemblyDefinitionImporterInspector.AssemblyDefintionState m_State;
    private ReorderableList m_ReferencesList;

    public override bool showImportedObject
    {
      get
      {
        return false;
      }
    }

    public override void OnInspectorGUI()
    {
      if (this.m_State == null)
      {
        try
        {
          this.LoadAssemblyDefinitionFiles();
        }
        catch (Exception ex)
        {
          this.ShowLoadErrorExceptionGUI(ex);
          return;
        }
      }
      AssemblyDefinitionPlatform[] definitionPlatforms = CompilationPipeline.GetAssemblyDefinitionPlatforms();
      using (new EditorGUI.DisabledScope(false))
      {
        EditorGUI.BeginChangeCheck();
        if (this.targets.Length > 1)
        {
          using (new EditorGUI.DisabledScope(true))
          {
            string text = string.Join(", ", ((IEnumerable<AssemblyDefinitionImporterInspector.AssemblyDefintionState>) this.m_TargetStates).Select<AssemblyDefinitionImporterInspector.AssemblyDefintionState, string>((Func<AssemblyDefinitionImporterInspector.AssemblyDefintionState, string>) (t => t.name)).ToArray<string>());
            EditorGUILayout.TextField(AssemblyDefinitionImporterInspector.Styles.name, text, EditorStyles.textField, new GUILayoutOption[0]);
          }
        }
        else
          this.m_State.name = EditorGUILayout.TextField(AssemblyDefinitionImporterInspector.Styles.name, this.m_State.name, EditorStyles.textField, new GUILayoutOption[0]);
        GUILayout.Label(AssemblyDefinitionImporterInspector.Styles.references, EditorStyles.boldLabel, new GUILayoutOption[0]);
        this.m_ReferencesList.DoLayoutList();
        GUILayout.Label(AssemblyDefinitionImporterInspector.Styles.platforms, EditorStyles.boldLabel, new GUILayoutOption[0]);
        EditorGUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
        AssemblyDefinitionImporterInspector.MixedBool compatibleWithAnyPlatform = this.m_State.compatibleWithAnyPlatform;
        this.m_State.compatibleWithAnyPlatform = AssemblyDefinitionImporterInspector.ToggleWithMixedValue(AssemblyDefinitionImporterInspector.Styles.anyPlatform, this.m_State.compatibleWithAnyPlatform);
        if (compatibleWithAnyPlatform == AssemblyDefinitionImporterInspector.MixedBool.Mixed && this.m_State.compatibleWithAnyPlatform != AssemblyDefinitionImporterInspector.MixedBool.Mixed)
        {
          AssemblyDefinitionImporterInspector.UpdatePlatformCompatibility(this.m_State.compatibleWithAnyPlatform, this.m_TargetStates);
          this.UpdateCombinedCompatibility();
        }
        else if (this.m_State.compatibleWithAnyPlatform != compatibleWithAnyPlatform)
          AssemblyDefinitionImporterInspector.InversePlatformCompatibility(this.m_State);
        if (this.m_State.compatibleWithAnyPlatform != AssemblyDefinitionImporterInspector.MixedBool.Mixed)
        {
          GUILayout.Label(this.m_State.compatibleWithAnyPlatform != AssemblyDefinitionImporterInspector.MixedBool.True ? AssemblyDefinitionImporterInspector.Styles.includePlatforms : AssemblyDefinitionImporterInspector.Styles.excludePlatforms, EditorStyles.boldLabel, new GUILayoutOption[0]);
          for (int index = 0; index < definitionPlatforms.Length; ++index)
            this.m_State.platformCompatibility[index] = AssemblyDefinitionImporterInspector.ToggleWithMixedValue(new GUIContent(definitionPlatforms[index].DisplayName), this.m_State.platformCompatibility[index]);
          EditorGUILayout.Space();
          GUILayout.BeginHorizontal();
          if (GUILayout.Button(AssemblyDefinitionImporterInspector.Styles.selectAll))
            AssemblyDefinitionImporterInspector.SetPlatformCompatibility(this.m_State, AssemblyDefinitionImporterInspector.MixedBool.True);
          if (GUILayout.Button(AssemblyDefinitionImporterInspector.Styles.deselectAll))
            AssemblyDefinitionImporterInspector.SetPlatformCompatibility(this.m_State, AssemblyDefinitionImporterInspector.MixedBool.False);
          GUILayout.FlexibleSpace();
          GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        GUILayout.Space(10f);
        if (EditorGUI.EndChangeCheck())
          this.m_State.modified = true;
      }
      EditorGUILayout.Space();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      using (new EditorGUI.DisabledScope(!this.m_State.modified))
      {
        if (GUILayout.Button(AssemblyDefinitionImporterInspector.Styles.revert))
          this.LoadAssemblyDefinitionFiles();
        if (GUILayout.Button(AssemblyDefinitionImporterInspector.Styles.apply))
          AssemblyDefinitionImporterInspector.SaveAndUpdateAssemblyDefinitionStates(this.m_State, this.m_TargetStates);
      }
      GUILayout.EndHorizontal();
    }

    public override void OnDisable()
    {
      if (this.m_State == null || !this.m_State.modified)
        return;
      string message = "Unapplied import settings for '" + (this.target as AssetImporter).assetPath + "'";
      if (this.targets.Length > 1)
        message = "Unapplied import settings for '" + (object) this.targets.Length + "' files";
      if (EditorUtility.DisplayDialog("Unapplied import settings", message, "Apply", "Revert"))
        AssemblyDefinitionImporterInspector.SaveAndUpdateAssemblyDefinitionStates(this.m_State, this.m_TargetStates);
    }

    private static void UpdatePlatformCompatibility(AssemblyDefinitionImporterInspector.MixedBool compatibleWithAnyPlatform, AssemblyDefinitionImporterInspector.AssemblyDefintionState[] states)
    {
      if (compatibleWithAnyPlatform == AssemblyDefinitionImporterInspector.MixedBool.Mixed)
        throw new ArgumentOutOfRangeException(nameof (compatibleWithAnyPlatform));
      foreach (AssemblyDefinitionImporterInspector.AssemblyDefintionState state in states)
      {
        if (state.compatibleWithAnyPlatform != compatibleWithAnyPlatform)
        {
          state.compatibleWithAnyPlatform = compatibleWithAnyPlatform;
          AssemblyDefinitionImporterInspector.InversePlatformCompatibility(state);
        }
      }
    }

    private static AssemblyDefinitionImporterInspector.MixedBool ToggleWithMixedValue(GUIContent title, AssemblyDefinitionImporterInspector.MixedBool value)
    {
      EditorGUI.showMixedValue = value == AssemblyDefinitionImporterInspector.MixedBool.Mixed;
      EditorGUI.BeginChangeCheck();
      bool flag = EditorGUILayout.Toggle(title, value == AssemblyDefinitionImporterInspector.MixedBool.True, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
        return !flag ? AssemblyDefinitionImporterInspector.MixedBool.False : AssemblyDefinitionImporterInspector.MixedBool.True;
      EditorGUI.showMixedValue = false;
      return value;
    }

    private static void InversePlatformCompatibility(AssemblyDefinitionImporterInspector.AssemblyDefintionState state)
    {
      AssemblyDefinitionPlatform[] definitionPlatforms = CompilationPipeline.GetAssemblyDefinitionPlatforms();
      for (int index = 0; index < definitionPlatforms.Length; ++index)
        state.platformCompatibility[index] = AssemblyDefinitionImporterInspector.InverseCompability(state.platformCompatibility[index]);
    }

    private static void SetPlatformCompatibility(AssemblyDefinitionImporterInspector.AssemblyDefintionState state, AssemblyDefinitionImporterInspector.MixedBool compatibility)
    {
      AssemblyDefinitionPlatform[] definitionPlatforms = CompilationPipeline.GetAssemblyDefinitionPlatforms();
      for (int index = 0; index < definitionPlatforms.Length; ++index)
        state.platformCompatibility[index] = compatibility;
    }

    private static AssemblyDefinitionImporterInspector.MixedBool InverseCompability(AssemblyDefinitionImporterInspector.MixedBool compatibility)
    {
      if (compatibility == AssemblyDefinitionImporterInspector.MixedBool.True)
        return AssemblyDefinitionImporterInspector.MixedBool.False;
      return compatibility == AssemblyDefinitionImporterInspector.MixedBool.False ? AssemblyDefinitionImporterInspector.MixedBool.True : AssemblyDefinitionImporterInspector.MixedBool.Mixed;
    }

    private void ShowLoadErrorExceptionGUI(Exception e)
    {
      if (this.m_TextStyle == null)
        this.m_TextStyle = (GUIStyle) "ScriptText";
      GUILayout.Label(AssemblyDefinitionImporterInspector.Styles.loadError, EditorStyles.boldLabel, new GUILayoutOption[0]);
      EditorGUI.HelpBox(GUILayoutUtility.GetRect(EditorGUIUtility.TempContent(e.Message), this.m_TextStyle), e.Message, MessageType.Error);
    }

    private void LoadAssemblyDefinitionFiles()
    {
      this.m_TargetStates = new AssemblyDefinitionImporterInspector.AssemblyDefintionState[this.targets.Length];
      for (int index = 0; index < this.targets.Length; ++index)
      {
        AssetImporter target = this.targets[index] as AssetImporter;
        if (!((UnityEngine.Object) target == (UnityEngine.Object) null))
          this.m_TargetStates[index] = AssemblyDefinitionImporterInspector.LoadAssemblyDefintionState(target.assetPath);
      }
      int num = ((IEnumerable<AssemblyDefinitionImporterInspector.AssemblyDefintionState>) this.m_TargetStates).Min<AssemblyDefinitionImporterInspector.AssemblyDefintionState>((Func<AssemblyDefinitionImporterInspector.AssemblyDefintionState, int>) (t => t.references.Count<AssemblyDefinitionImporterInspector.AssemblyDefinitionReference>()));
      this.m_State = new AssemblyDefinitionImporterInspector.AssemblyDefintionState();
      this.m_State.name = this.m_TargetStates[0].name;
      this.m_State.references = new List<AssemblyDefinitionImporterInspector.AssemblyDefinitionReference>();
      this.m_State.modified = this.m_TargetStates[0].modified;
      for (int index = 0; index < num; ++index)
        this.m_State.references.Add(this.m_TargetStates[0].references[index]);
      for (int index1 = 1; index1 < this.m_TargetStates.Length; ++index1)
      {
        AssemblyDefinitionImporterInspector.AssemblyDefintionState targetState = this.m_TargetStates[index1];
        for (int index2 = 0; index2 < num; ++index2)
        {
          if (this.m_State.references[index2].displayValue != AssemblyDefinitionImporterInspector.MixedBool.Mixed && this.m_State.references[index2].path != targetState.references[index2].path)
            this.m_State.references[index2].displayValue = AssemblyDefinitionImporterInspector.MixedBool.Mixed;
        }
        this.m_State.modified |= targetState.modified;
      }
      this.UpdateCombinedCompatibility();
      this.m_ReferencesList = new ReorderableList((IList) this.m_State.references, typeof (AssemblyDefinitionImporterInspector.AssemblyDefinitionReference), false, false, true, true);
      this.m_ReferencesList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawReferenceListElement);
      this.m_ReferencesList.onAddCallback = new ReorderableList.AddCallbackDelegate(this.AddReferenceListElement);
      this.m_ReferencesList.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveReferenceListElement);
      this.m_ReferencesList.elementHeight = EditorGUIUtility.singleLineHeight + 2f;
      this.m_ReferencesList.headerHeight = 3f;
    }

    private void UpdateCombinedCompatibility()
    {
      this.m_State.compatibleWithAnyPlatform = this.m_TargetStates[0].compatibleWithAnyPlatform;
      AssemblyDefinitionPlatform[] definitionPlatforms = CompilationPipeline.GetAssemblyDefinitionPlatforms();
      this.m_State.platformCompatibility = new AssemblyDefinitionImporterInspector.MixedBool[definitionPlatforms.Length];
      Array.Copy((Array) this.m_TargetStates[0].platformCompatibility, (Array) this.m_State.platformCompatibility, definitionPlatforms.Length);
      for (int index1 = 1; index1 < this.m_TargetStates.Length; ++index1)
      {
        AssemblyDefinitionImporterInspector.AssemblyDefintionState targetState = this.m_TargetStates[index1];
        if (this.m_State.compatibleWithAnyPlatform != AssemblyDefinitionImporterInspector.MixedBool.Mixed && this.m_State.compatibleWithAnyPlatform != targetState.compatibleWithAnyPlatform)
          this.m_State.compatibleWithAnyPlatform = AssemblyDefinitionImporterInspector.MixedBool.Mixed;
        for (int index2 = 0; index2 < definitionPlatforms.Length; ++index2)
        {
          if (this.m_State.platformCompatibility[index2] != AssemblyDefinitionImporterInspector.MixedBool.Mixed && this.m_State.platformCompatibility[index2] != targetState.platformCompatibility[index2])
            this.m_State.platformCompatibility[index2] = AssemblyDefinitionImporterInspector.MixedBool.Mixed;
        }
      }
    }

    private static AssemblyDefinitionImporterInspector.AssemblyDefintionState LoadAssemblyDefintionState(string path)
    {
      AssemblyDefinitionAsset assemblyDefinitionAsset = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(path);
      if ((UnityEngine.Object) assemblyDefinitionAsset == (UnityEngine.Object) null)
        return (AssemblyDefinitionImporterInspector.AssemblyDefintionState) null;
      CustomScriptAssemblyData scriptAssemblyData = CustomScriptAssemblyData.FromJson(assemblyDefinitionAsset.text);
      if (scriptAssemblyData == null)
        return (AssemblyDefinitionImporterInspector.AssemblyDefintionState) null;
      AssemblyDefinitionImporterInspector.AssemblyDefintionState assemblyDefintionState = new AssemblyDefinitionImporterInspector.AssemblyDefintionState();
      assemblyDefintionState.asset = assemblyDefinitionAsset;
      assemblyDefintionState.name = scriptAssemblyData.name;
      assemblyDefintionState.references = new List<AssemblyDefinitionImporterInspector.AssemblyDefinitionReference>();
      if (scriptAssemblyData.references != null)
      {
        foreach (string reference in scriptAssemblyData.references)
        {
          try
          {
            AssemblyDefinitionImporterInspector.AssemblyDefinitionReference definitionReference = new AssemblyDefinitionImporterInspector.AssemblyDefinitionReference();
            string fromAssemblyName = CompilationPipeline.GetAssemblyDefinitionFilePathFromAssemblyName(reference);
            if (string.IsNullOrEmpty(fromAssemblyName))
              throw new AssemblyDefinitionException(string.Format("Could not find assembly reference '{0}'", (object) reference), new string[1]{ path });
            definitionReference.asset = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(fromAssemblyName);
            if ((UnityEngine.Object) definitionReference.asset == (UnityEngine.Object) null)
              throw new AssemblyDefinitionException(string.Format("Reference assembly definition file '{0}' not found", (object) fromAssemblyName), new string[1]{ path });
            definitionReference.data = CustomScriptAssemblyData.FromJson(definitionReference.asset.text);
            definitionReference.displayValue = AssemblyDefinitionImporterInspector.MixedBool.False;
            assemblyDefintionState.references.Add(definitionReference);
          }
          catch (AssemblyDefinitionException ex)
          {
            Debug.LogException((Exception) ex, (UnityEngine.Object) assemblyDefinitionAsset);
            assemblyDefintionState.references.Add(new AssemblyDefinitionImporterInspector.AssemblyDefinitionReference());
            assemblyDefintionState.modified = true;
          }
        }
      }
      AssemblyDefinitionPlatform[] definitionPlatforms = CompilationPipeline.GetAssemblyDefinitionPlatforms();
      assemblyDefintionState.platformCompatibility = new AssemblyDefinitionImporterInspector.MixedBool[definitionPlatforms.Length];
      assemblyDefintionState.compatibleWithAnyPlatform = AssemblyDefinitionImporterInspector.MixedBool.True;
      string[] strArray = (string[]) null;
      if (scriptAssemblyData.includePlatforms != null && scriptAssemblyData.includePlatforms.Length > 0)
      {
        assemblyDefintionState.compatibleWithAnyPlatform = AssemblyDefinitionImporterInspector.MixedBool.False;
        strArray = scriptAssemblyData.includePlatforms;
      }
      else if (scriptAssemblyData.excludePlatforms != null && scriptAssemblyData.excludePlatforms.Length > 0)
      {
        assemblyDefintionState.compatibleWithAnyPlatform = AssemblyDefinitionImporterInspector.MixedBool.True;
        strArray = scriptAssemblyData.excludePlatforms;
      }
      if (strArray != null)
      {
        foreach (string name in strArray)
        {
          int platformIndex = AssemblyDefinitionImporterInspector.GetPlatformIndex(definitionPlatforms, name);
          assemblyDefintionState.platformCompatibility[platformIndex] = AssemblyDefinitionImporterInspector.MixedBool.True;
        }
      }
      return assemblyDefintionState;
    }

    private static AssemblyDefinitionImporterInspector.AssemblyDefinitionReference CreateAssemblyDefinitionReference(string assemblyName)
    {
      AssemblyDefinitionImporterInspector.AssemblyDefinitionReference definitionReference = new AssemblyDefinitionImporterInspector.AssemblyDefinitionReference();
      string fromAssemblyName = CompilationPipeline.GetAssemblyDefinitionFilePathFromAssemblyName(assemblyName);
      if (string.IsNullOrEmpty(fromAssemblyName))
        throw new Exception(string.Format("Could not get assembly definition filename for assembly '{0}'", (object) assemblyName));
      definitionReference.asset = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(fromAssemblyName);
      if ((UnityEngine.Object) definitionReference.asset == (UnityEngine.Object) null)
        throw new FileNotFoundException(string.Format("Assembly definition file '{0}' not found", (object) definitionReference.path), definitionReference.path);
      definitionReference.data = CustomScriptAssemblyData.FromJson(definitionReference.asset.text);
      return definitionReference;
    }

    private static void SaveAndUpdateAssemblyDefinitionStates(AssemblyDefinitionImporterInspector.AssemblyDefintionState combinedState, AssemblyDefinitionImporterInspector.AssemblyDefintionState[] states)
    {
      int num = combinedState.references.Count<AssemblyDefinitionImporterInspector.AssemblyDefinitionReference>();
      if (states.Length == 1)
        states[0].name = combinedState.name;
      foreach (AssemblyDefinitionImporterInspector.AssemblyDefintionState state in states)
      {
        for (int index = 0; index < num; ++index)
        {
          if (combinedState.references[index].displayValue != AssemblyDefinitionImporterInspector.MixedBool.Mixed)
            state.references[index] = combinedState.references[index];
        }
        if (combinedState.compatibleWithAnyPlatform != AssemblyDefinitionImporterInspector.MixedBool.Mixed)
          state.compatibleWithAnyPlatform = combinedState.compatibleWithAnyPlatform;
        for (int index = 0; index < combinedState.platformCompatibility.Length; ++index)
        {
          if (combinedState.platformCompatibility[index] != AssemblyDefinitionImporterInspector.MixedBool.Mixed)
            state.platformCompatibility[index] = combinedState.platformCompatibility[index];
        }
        AssemblyDefinitionImporterInspector.SaveAssemblyDefinitionState(state);
      }
      combinedState.modified = false;
    }

    private static void SaveAssemblyDefinitionState(AssemblyDefinitionImporterInspector.AssemblyDefintionState state)
    {
      IEnumerable<AssemblyDefinitionImporterInspector.AssemblyDefinitionReference> source1 = state.references.Where<AssemblyDefinitionImporterInspector.AssemblyDefinitionReference>((Func<AssemblyDefinitionImporterInspector.AssemblyDefinitionReference, bool>) (r => (UnityEngine.Object) r.asset != (UnityEngine.Object) null));
      AssemblyDefinitionPlatform[] definitionPlatforms = CompilationPipeline.GetAssemblyDefinitionPlatforms();
      CustomScriptAssemblyData data = new CustomScriptAssemblyData();
      data.name = state.name;
      data.references = source1.Select<AssemblyDefinitionImporterInspector.AssemblyDefinitionReference, string>((Func<AssemblyDefinitionImporterInspector.AssemblyDefinitionReference, string>) (r => r.data.name)).ToArray<string>();
      List<string> source2 = new List<string>();
      for (int index = 0; index < definitionPlatforms.Length; ++index)
      {
        if (state.platformCompatibility[index] == AssemblyDefinitionImporterInspector.MixedBool.True)
          source2.Add(definitionPlatforms[index].Name);
      }
      if (source2.Any<string>())
      {
        if (state.compatibleWithAnyPlatform == AssemblyDefinitionImporterInspector.MixedBool.True)
          data.excludePlatforms = source2.ToArray();
        else
          data.includePlatforms = source2.ToArray();
      }
      string json = CustomScriptAssemblyData.ToJson(data);
      File.WriteAllText(state.path, json);
      state.modified = false;
      AssetDatabase.ImportAsset(state.path);
    }

    private static int GetPlatformIndex(AssemblyDefinitionPlatform[] platforms, string name)
    {
      for (int index = 0; index < platforms.Length; ++index)
      {
        if (string.Equals(platforms[index].Name, name, StringComparison.InvariantCultureIgnoreCase))
          return index;
      }
      throw new ArgumentException(string.Format("Unknown platform '{0}'", (object) name), name);
    }

    private void DrawReferenceListElement(Rect rect, int index, bool selected, bool focused)
    {
      AssemblyDefinitionImporterInspector.AssemblyDefinitionReference definitionReference = this.m_ReferencesList.list[index] as AssemblyDefinitionImporterInspector.AssemblyDefinitionReference;
      rect.height -= 2f;
      string str = definitionReference.data == null ? "(Missing Reference)" : definitionReference.data.name;
      AssemblyDefinitionAsset asset = definitionReference.asset;
      bool flag = definitionReference.displayValue == AssemblyDefinitionImporterInspector.MixedBool.Mixed;
      EditorGUI.showMixedValue = flag;
      definitionReference.asset = EditorGUI.ObjectField(rect, !flag ? str : "(Multiple Values)", (UnityEngine.Object) asset, typeof (AssemblyDefinitionAsset), false) as AssemblyDefinitionAsset;
      EditorGUI.showMixedValue = false;
      if (!((UnityEngine.Object) asset != (UnityEngine.Object) definitionReference.asset) || !((UnityEngine.Object) definitionReference.asset != (UnityEngine.Object) null))
        return;
      definitionReference.data = CustomScriptAssemblyData.FromJson(definitionReference.asset.text);
      foreach (AssemblyDefinitionImporterInspector.AssemblyDefintionState targetState in this.m_TargetStates)
        targetState.references[index] = definitionReference;
    }

    private void AddReferenceListElement(ReorderableList list)
    {
      ReorderableList.defaultBehaviours.DoAddButton(list);
      foreach (AssemblyDefinitionImporterInspector.AssemblyDefintionState targetState in this.m_TargetStates)
      {
        if (targetState.references.Count <= list.count)
        {
          int index = Math.Min(list.index, targetState.references.Count<AssemblyDefinitionImporterInspector.AssemblyDefinitionReference>());
          targetState.references.Insert(index, list.list[list.index] as AssemblyDefinitionImporterInspector.AssemblyDefinitionReference);
        }
      }
    }

    private void RemoveReferenceListElement(ReorderableList list)
    {
      foreach (AssemblyDefinitionImporterInspector.AssemblyDefintionState targetState in this.m_TargetStates)
        targetState.references.RemoveAt(list.index);
      ReorderableList.defaultBehaviours.DoRemoveButton(list);
    }

    internal class Styles
    {
      public static readonly GUIContent name = EditorGUIUtility.TrTextContent("Name");
      public static readonly GUIContent references = EditorGUIUtility.TrTextContent("References");
      public static readonly GUIContent platforms = EditorGUIUtility.TrTextContent("Platforms");
      public static readonly GUIContent anyPlatform = EditorGUIUtility.TrTextContent("Any Platform");
      public static readonly GUIContent includePlatforms = EditorGUIUtility.TrTextContent("Include Platforms");
      public static readonly GUIContent excludePlatforms = EditorGUIUtility.TrTextContent("Exclude Platforms");
      public static readonly GUIContent selectAll = EditorGUIUtility.TrTextContent("Select all");
      public static readonly GUIContent deselectAll = EditorGUIUtility.TrTextContent("Deselect all");
      public static readonly GUIContent apply = EditorGUIUtility.TrTextContent("Apply");
      public static readonly GUIContent revert = EditorGUIUtility.TrTextContent("Revert");
      public static readonly GUIContent loadError = EditorGUIUtility.TrTextContent("Load error");
    }

    internal enum MixedBool
    {
      Mixed = -1,
      False = 0,
      True = 1,
    }

    internal class AssemblyDefinitionReference
    {
      public AssemblyDefinitionAsset asset;
      public CustomScriptAssemblyData data;
      public AssemblyDefinitionImporterInspector.MixedBool displayValue;

      public string path
      {
        get
        {
          return AssetDatabase.GetAssetPath((UnityEngine.Object) this.asset);
        }
      }
    }

    internal class AssemblyDefintionState
    {
      public AssemblyDefinitionAsset asset;
      public string name;
      public List<AssemblyDefinitionImporterInspector.AssemblyDefinitionReference> references;
      public AssemblyDefinitionImporterInspector.MixedBool compatibleWithAnyPlatform;
      public AssemblyDefinitionImporterInspector.MixedBool[] platformCompatibility;
      public bool modified;

      public string path
      {
        get
        {
          return AssetDatabase.GetAssetPath((UnityEngine.Object) this.asset);
        }
      }
    }
  }
}
