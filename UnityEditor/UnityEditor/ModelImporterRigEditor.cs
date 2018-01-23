// Decompiled with JetBrains decompiler
// Type: UnityEditor.ModelImporterRigEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityEditor
{
  internal class ModelImporterRigEditor : BaseAssetImporterTabUI
  {
    private static bool importMessageFoldout = false;
    public int m_SelectedClipIndex = -1;
    private bool m_IsBiped = false;
    private List<string> m_BipedMappingReport = (List<string>) null;
    private ModelImporterRigEditor.MappingRelevantSettings[] oldModelSettings = (ModelImporterRigEditor.MappingRelevantSettings[]) null;
    private ModelImporterRigEditor.MappingRelevantSettings[] newModelSettings = (ModelImporterRigEditor.MappingRelevantSettings[]) null;
    private const float kDeleteWidth = 17f;
    private Avatar m_Avatar;
    private SerializedProperty m_OptimizeGameObjects;
    private SerializedProperty m_AnimationType;
    private SerializedProperty m_AvatarSource;
    private SerializedProperty m_CopyAvatar;
    private SerializedProperty m_LegacyGenerateAnimations;
    private SerializedProperty m_AnimationCompression;
    private SerializedProperty m_RootMotionBoneName;
    private SerializedProperty m_RootMotionBoneRotation;
    private SerializedProperty m_SrcHasExtraRoot;
    private SerializedProperty m_DstHasExtraRoot;
    private SerializedProperty m_RigImportErrors;
    private SerializedProperty m_RigImportWarnings;
    private GUIContent[] m_RootMotionBoneList;
    private ExposeTransformEditor m_ExposeTransformEditor;
    private bool m_AvatarCopyIsUpToDate;
    private bool m_CanMultiEditTransformList;
    private static ModelImporterRigEditor.Styles styles;

    public ModelImporterRigEditor(AssetImporterEditor panelContainer)
      : base(panelContainer)
    {
    }

    private ModelImporter singleImporter
    {
      get
      {
        return this.targets[0] as ModelImporter;
      }
    }

    private ModelImporterAnimationType animationType
    {
      get
      {
        return (ModelImporterAnimationType) this.m_AnimationType.intValue;
      }
      set
      {
        this.m_AnimationType.intValue = (int) value;
      }
    }

    public int rootIndex { get; set; }

    internal override void OnEnable()
    {
      this.m_AnimationType = this.serializedObject.FindProperty("m_AnimationType");
      this.m_AvatarSource = this.serializedObject.FindProperty("m_LastHumanDescriptionAvatarSource");
      this.m_OptimizeGameObjects = this.serializedObject.FindProperty("m_OptimizeGameObjects");
      this.m_RootMotionBoneName = this.serializedObject.FindProperty("m_HumanDescription.m_RootMotionBoneName");
      this.m_RootMotionBoneRotation = this.serializedObject.FindProperty("m_HumanDescription.m_RootMotionBoneRotation");
      this.m_ExposeTransformEditor = new ExposeTransformEditor();
      string[] transformPaths = this.singleImporter.transformPaths;
      this.m_RootMotionBoneList = new GUIContent[transformPaths.Length];
      for (int index = 0; index < transformPaths.Length; ++index)
        this.m_RootMotionBoneList[index] = new GUIContent(transformPaths[index]);
      if (this.m_RootMotionBoneList.Length > 0)
        this.m_RootMotionBoneList[0] = new GUIContent("None");
      this.rootIndex = ArrayUtility.FindIndex<GUIContent>(this.m_RootMotionBoneList, (Predicate<GUIContent>) (content => FileUtil.GetLastPathNameComponent(content.text) == this.m_RootMotionBoneName.stringValue));
      this.rootIndex = this.rootIndex >= 1 ? this.rootIndex : 0;
      this.m_SrcHasExtraRoot = this.serializedObject.FindProperty("m_HasExtraRoot");
      this.m_DstHasExtraRoot = this.serializedObject.FindProperty("m_HumanDescription.m_HasExtraRoot");
      this.m_CopyAvatar = this.serializedObject.FindProperty("m_CopyAvatar");
      this.m_LegacyGenerateAnimations = this.serializedObject.FindProperty("m_LegacyGenerateAnimations");
      this.m_AnimationCompression = this.serializedObject.FindProperty("m_AnimationCompression");
      this.m_RigImportErrors = this.serializedObject.FindProperty("m_RigImportErrors");
      this.m_RigImportWarnings = this.serializedObject.FindProperty("m_RigImportWarnings");
      this.m_ExposeTransformEditor.OnEnable(this.singleImporter.transformPaths, this.serializedObject);
      this.m_CanMultiEditTransformList = this.CanMultiEditTransformList();
      this.CheckIfAvatarCopyIsUpToDate();
      this.m_IsBiped = false;
      this.m_BipedMappingReport = new List<string>();
      if (this.m_AnimationType.intValue != 3)
        return;
      this.m_IsBiped = AvatarBipedMapper.IsBiped((AssetDatabase.LoadMainAssetAtPath(this.singleImporter.assetPath) as GameObject).transform, this.m_BipedMappingReport);
      if ((UnityEngine.Object) this.m_Avatar == (UnityEngine.Object) null)
        this.ResetAvatar();
    }

    private bool CanMultiEditTransformList()
    {
      string[] transformPaths = this.singleImporter.transformPaths;
      for (int index = 1; index < this.targets.Length; ++index)
      {
        ModelImporter target = this.targets[index] as ModelImporter;
        if (!ArrayUtility.ArrayEquals<string>(transformPaths, target.transformPaths))
          return false;
      }
      return true;
    }

    private void CheckIfAvatarCopyIsUpToDate()
    {
      if (this.animationType != ModelImporterAnimationType.Human && this.animationType != ModelImporterAnimationType.Generic || this.m_AvatarSource.objectReferenceValue == (UnityEngine.Object) null)
        this.m_AvatarCopyIsUpToDate = true;
      else
        this.m_AvatarCopyIsUpToDate = ModelImporterRigEditor.DoesHumanDescriptionMatch(this.singleImporter, AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(this.m_AvatarSource.objectReferenceValue)) as ModelImporter);
    }

    internal override void OnDestroy()
    {
      this.m_Avatar = (Avatar) null;
    }

    internal override void ResetValues()
    {
      base.ResetValues();
      this.ResetAvatar();
    }

    private void ResetAvatar()
    {
      this.m_Avatar = AssetDatabase.LoadAssetAtPath((this.target as ModelImporter).assetPath, typeof (Avatar)) as Avatar;
    }

    private void LegacyGUI()
    {
      EditorGUILayout.Popup(this.m_LegacyGenerateAnimations, ModelImporterRigEditor.styles.AnimationsOpt, ModelImporterRigEditor.styles.AnimLabel, new GUILayoutOption[0]);
      if (this.m_LegacyGenerateAnimations.intValue != 1 && this.m_LegacyGenerateAnimations.intValue != 2 && this.m_LegacyGenerateAnimations.intValue != 3)
        return;
      EditorGUILayout.HelpBox("The animation import setting \"" + ModelImporterRigEditor.styles.AnimationsOpt[this.m_LegacyGenerateAnimations.intValue].text + "\" is deprecated.", MessageType.Warning);
    }

    private void AvatarSourceGUI()
    {
      EditorGUI.BeginChangeCheck();
      int selectedIndex = !this.m_CopyAvatar.boolValue ? 0 : 1;
      EditorGUI.showMixedValue = this.m_CopyAvatar.hasMultipleDifferentValues;
      int num = EditorGUILayout.Popup(ModelImporterRigEditor.styles.AvatarDefinition, selectedIndex, ModelImporterRigEditor.styles.AvatarDefinitionOpt, new GUILayoutOption[0]);
      EditorGUI.showMixedValue = false;
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_CopyAvatar.boolValue = num == 1;
    }

    private void GenericGUI()
    {
      this.AvatarSourceGUI();
      if (this.m_CopyAvatar.hasMultipleDifferentValues)
        return;
      if (!this.m_CopyAvatar.boolValue)
      {
        EditorGUI.BeginChangeCheck();
        using (new EditorGUI.DisabledScope(!this.m_CanMultiEditTransformList))
          this.rootIndex = EditorGUILayout.Popup(ModelImporterRigEditor.styles.RootNode, this.rootIndex, this.m_RootMotionBoneList, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
          this.m_RootMotionBoneName.stringValue = this.rootIndex <= 0 || this.rootIndex >= this.m_RootMotionBoneList.Length ? "" : FileUtil.GetLastPathNameComponent(this.m_RootMotionBoneList[this.rootIndex].text);
      }
      else
        this.CopyAvatarGUI();
    }

    private void HumanoidGUI()
    {
      this.AvatarSourceGUI();
      if (!this.m_CopyAvatar.hasMultipleDifferentValues)
      {
        if (!this.m_CopyAvatar.boolValue)
          this.ConfigureAvatarGUI();
        else
          this.CopyAvatarGUI();
      }
      if (this.m_IsBiped)
      {
        if (this.m_BipedMappingReport.Count > 0)
        {
          string message = "A Biped was detected, but cannot be configured properly because of an unsupported hierarchy. Adjust Biped settings in 3DS Max before exporting to correct this problem.\n";
          for (int index = 0; index < this.m_BipedMappingReport.Count; ++index)
            message += this.m_BipedMappingReport[index];
          EditorGUILayout.HelpBox(message, MessageType.Warning);
        }
        else
          EditorGUILayout.HelpBox("A Biped was detected. Default Biped mapping and T-Pose have been configured for this avatar. Translation DoFs have been activated. Use Configure to modify default Biped setup.", MessageType.Info);
      }
      EditorGUILayout.Space();
    }

    private void ConfigureAvatarGUI()
    {
      if (this.targets.Length > 1)
      {
        GUILayout.Label("Can't configure avatar in multi-editing mode", EditorStyles.helpBox, new GUILayoutOption[0]);
      }
      else
      {
        if (this.singleImporter.transformPaths.Length <= HumanTrait.RequiredBoneCount)
          GUILayout.Label(string.Format("Not enough bones to create human avatar (requires {0})", (object) HumanTrait.RequiredBoneCount, (object) EditorStyles.helpBox));
        GUIContent content;
        if ((bool) ((UnityEngine.Object) this.m_Avatar) && !this.HasModified())
        {
          content = !this.m_Avatar.isHuman ? ModelImporterRigEditor.styles.avatarInvalid : ModelImporterRigEditor.styles.avatarValid;
        }
        else
        {
          content = ModelImporterRigEditor.styles.avatarPending;
          GUILayout.Label("The avatar can be configured after settings have been applied.", EditorStyles.helpBox, new GUILayoutOption[0]);
        }
        Rect controlRect = EditorGUILayout.GetControlRect();
        GUI.Label(new Rect((float) ((double) controlRect.xMax - 75.0 - 18.0), controlRect.y, 18f, controlRect.height), content, EditorStyles.label);
        using (new EditorGUI.DisabledScope((UnityEngine.Object) this.m_Avatar == (UnityEngine.Object) null))
        {
          if (!GUI.Button(new Rect(controlRect.xMax - 75f, controlRect.y + 1f, 75f, controlRect.height - 1f), ModelImporterRigEditor.styles.configureAvatar, EditorStyles.miniButton))
            return;
          if (!this.isLocked)
          {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
              Selection.activeObject = (UnityEngine.Object) this.m_Avatar;
              AvatarEditor.s_EditImmediatelyOnNextOpen = true;
            }
            GUIUtility.ExitGUI();
          }
          else
            Debug.Log((object) "Cannot configure avatar, inspector is locked");
        }
      }
    }

    private void CheckAvatar(Avatar sourceAvatar)
    {
      if (!((UnityEngine.Object) sourceAvatar != (UnityEngine.Object) null))
        return;
      if (sourceAvatar.isHuman && this.animationType != ModelImporterAnimationType.Human)
      {
        if (EditorUtility.DisplayDialog("Asigning an Humanoid Avatar on a Generic Rig", "Do you want to change Animation Type to Humanoid ?", "Yes", "No"))
          this.animationType = ModelImporterAnimationType.Human;
        else
          this.m_AvatarSource.objectReferenceValue = (UnityEngine.Object) null;
      }
      else if (!sourceAvatar.isHuman && this.animationType != ModelImporterAnimationType.Generic)
      {
        if (EditorUtility.DisplayDialog("Asigning an Generic Avatar on a Humanoid Rig", "Do you want to change Animation Type to Generic ?", "Yes", "No"))
          this.animationType = ModelImporterAnimationType.Generic;
        else
          this.m_AvatarSource.objectReferenceValue = (UnityEngine.Object) null;
      }
    }

    private void CopyAvatarGUI()
    {
      GUILayout.Label("If you have already created an Avatar for another model with a rig identical to this one, you can copy its Avatar definition.\nWith this option, this model will not create any avatar but only import animations.", EditorStyles.helpBox, new GUILayoutOption[0]);
      EditorGUILayout.BeginHorizontal();
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_AvatarSource, GUIContent.Temp("Source"), new GUILayoutOption[0]);
      Avatar objectReferenceValue = this.m_AvatarSource.objectReferenceValue as Avatar;
      if (EditorGUI.EndChangeCheck())
      {
        this.CheckAvatar(objectReferenceValue);
        AvatarSetupTool.ClearAll(this.serializedObject);
        if ((UnityEngine.Object) objectReferenceValue != (UnityEngine.Object) null)
          this.CopyHumanDescriptionFromOtherModel(objectReferenceValue);
        this.m_AvatarCopyIsUpToDate = true;
      }
      if ((UnityEngine.Object) objectReferenceValue != (UnityEngine.Object) null && !this.m_AvatarSource.hasMultipleDifferentValues && (!this.m_AvatarCopyIsUpToDate && GUILayout.Button(ModelImporterRigEditor.styles.UpdateMuscleDefinitionFromSource, EditorStyles.miniButton, new GUILayoutOption[0])))
      {
        AvatarSetupTool.ClearAll(this.serializedObject);
        this.CopyHumanDescriptionFromOtherModel(objectReferenceValue);
        this.m_AvatarCopyIsUpToDate = true;
      }
      EditorGUILayout.EndHorizontal();
    }

    private void ShowUpdateReferenceClip()
    {
      if (this.targets.Length > 1 || this.m_CopyAvatar.boolValue || (!(bool) ((UnityEngine.Object) this.m_Avatar) || !this.m_Avatar.isValid))
        return;
      string[] array = new string[0];
      ModelImporter target = this.target as ModelImporter;
      if (target.referencedClips.Length > 0)
      {
        foreach (string referencedClip in target.referencedClips)
          ArrayUtility.Add<string>(ref array, AssetDatabase.GUIDToAssetPath(referencedClip));
      }
      if (array.Length <= 0)
        return;
      if (!GUILayout.Button(ModelImporterRigEditor.styles.UpdateReferenceClips, new GUILayoutOption[1]{ GUILayout.Width(150f) }))
        return;
      foreach (string otherModelImporterPath in array)
        this.SetupReferencedClip(otherModelImporterPath);
      try
      {
        AssetDatabase.StartAssetEditing();
        foreach (string path in array)
          AssetDatabase.ImportAsset(path);
      }
      finally
      {
        AssetDatabase.StopAssetEditing();
      }
    }

    public override void OnInspectorGUI()
    {
      if (ModelImporterRigEditor.styles == null)
        ModelImporterRigEditor.styles = new ModelImporterRigEditor.Styles();
      string stringValue1 = this.m_RigImportErrors.stringValue;
      string stringValue2 = this.m_RigImportWarnings.stringValue;
      if (stringValue1.Length > 0)
      {
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Error(s) found while importing rig in this animation file. Open \"Import Messages\" foldout below for more details", MessageType.Error);
      }
      else if (stringValue2.Length > 0)
      {
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Warning(s) found while importing rig in this animation file. Open \"Import Messages\" foldout below for more details", MessageType.Warning);
      }
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.Popup(this.m_AnimationType, ModelImporterRigEditor.styles.AnimationTypeOpt, ModelImporterRigEditor.styles.AnimationType, new GUILayoutOption[0]);
      if (EditorGUI.EndChangeCheck())
      {
        this.m_AvatarSource.objectReferenceValue = (UnityEngine.Object) null;
        if (this.animationType == ModelImporterAnimationType.Legacy)
          this.m_AnimationCompression.intValue = 1;
        else if (this.animationType == ModelImporterAnimationType.Generic || this.animationType == ModelImporterAnimationType.Human)
          this.m_AnimationCompression.intValue = 3;
        this.m_DstHasExtraRoot.boolValue = this.m_SrcHasExtraRoot.boolValue;
      }
      EditorGUILayout.Space();
      if (!this.m_AnimationType.hasMultipleDifferentValues)
      {
        if (this.animationType == ModelImporterAnimationType.Human)
          this.HumanoidGUI();
        else if (this.animationType == ModelImporterAnimationType.Generic)
          this.GenericGUI();
        else if (this.animationType == ModelImporterAnimationType.Legacy)
          this.LegacyGUI();
      }
      this.ShowUpdateReferenceClip();
      bool flag = true;
      if (this.animationType != ModelImporterAnimationType.Human && this.animationType != ModelImporterAnimationType.Generic)
        flag = false;
      if (this.m_CopyAvatar.boolValue)
        flag = false;
      if (flag)
      {
        EditorGUILayout.PropertyField(this.m_OptimizeGameObjects);
        if (this.m_OptimizeGameObjects.boolValue && this.serializedObject.targetObjects.Length == 1)
        {
          EditorGUILayout.Space();
          using (new EditorGUI.DisabledScope(!this.m_CanMultiEditTransformList))
            this.m_ExposeTransformEditor.OnGUI();
        }
      }
      if (stringValue1.Length <= 0 && stringValue2.Length <= 0)
        return;
      EditorGUILayout.Space();
      ModelImporterRigEditor.importMessageFoldout = EditorGUILayout.Foldout(ModelImporterRigEditor.importMessageFoldout, ModelImporterRigEditor.styles.ImportMessages, true);
      if (ModelImporterRigEditor.importMessageFoldout)
      {
        if (stringValue1.Length > 0)
          EditorGUILayout.HelpBox(stringValue1, MessageType.None);
        else if (stringValue2.Length > 0)
          EditorGUILayout.HelpBox(stringValue2, MessageType.None);
      }
    }

    private static SerializedObject GetModelImporterSerializedObject(string assetPath)
    {
      ModelImporter atPath = AssetImporter.GetAtPath(assetPath) as ModelImporter;
      if ((UnityEngine.Object) atPath == (UnityEngine.Object) null)
        return (SerializedObject) null;
      return new SerializedObject((UnityEngine.Object) atPath);
    }

    private static bool DoesHumanDescriptionMatch(ModelImporter importer, ModelImporter otherImporter)
    {
      SerializedObject serializedObject = new SerializedObject(new UnityEngine.Object[2]{ (UnityEngine.Object) importer, (UnityEngine.Object) otherImporter });
      serializedObject.maxArraySizeForMultiEditing = Math.Max(importer.transformPaths.Length, otherImporter.transformPaths.Length);
      bool flag = !serializedObject.FindProperty("m_HumanDescription").hasMultipleDifferentValues;
      serializedObject.Dispose();
      return flag;
    }

    private static void CopyHumanDescriptionToDestination(SerializedObject sourceObject, SerializedObject targetObject)
    {
      targetObject.CopyFromSerializedProperty(sourceObject.FindProperty("m_HumanDescription"));
    }

    private void CopyHumanDescriptionFromOtherModel(Avatar sourceAvatar)
    {
      SerializedObject serializedObject = ModelImporterRigEditor.GetModelImporterSerializedObject(AssetDatabase.GetAssetPath((UnityEngine.Object) sourceAvatar));
      ModelImporterRigEditor.CopyHumanDescriptionToDestination(serializedObject, this.serializedObject);
      serializedObject.Dispose();
    }

    private void SetupReferencedClip(string otherModelImporterPath)
    {
      SerializedObject serializedObject = ModelImporterRigEditor.GetModelImporterSerializedObject(otherModelImporterPath);
      if (serializedObject == null)
        return;
      serializedObject.CopyFromSerializedProperty(this.serializedObject.FindProperty("m_AnimationType"));
      SerializedProperty property1 = serializedObject.FindProperty("m_CopyAvatar");
      if (property1 != null)
        property1.boolValue = true;
      SerializedProperty property2 = serializedObject.FindProperty("m_LastHumanDescriptionAvatarSource");
      if (property2 != null)
        property2.objectReferenceValue = (UnityEngine.Object) this.m_Avatar;
      ModelImporterRigEditor.CopyHumanDescriptionToDestination(this.serializedObject, serializedObject);
      serializedObject.ApplyModifiedProperties();
      serializedObject.Dispose();
    }

    public bool isLocked
    {
      get
      {
        foreach (InspectorWindow allInspectorWindow in InspectorWindow.GetAllInspectorWindows())
        {
          foreach (Editor activeEditor in allInspectorWindow.tracker.activeEditors)
          {
            if (activeEditor is ModelImporterEditor && ((AssetImporterTabbedEditor) activeEditor).activeTab == this)
              return allInspectorWindow.isLocked;
          }
        }
        return false;
      }
    }

    internal override void PreApply()
    {
      this.oldModelSettings = new ModelImporterRigEditor.MappingRelevantSettings[this.targets.Length];
      for (int index = 0; index < this.targets.Length; ++index)
      {
        SerializedObject serializedObject = new SerializedObject(this.targets[index]);
        SerializedProperty property1 = serializedObject.FindProperty("m_AnimationType");
        SerializedProperty property2 = serializedObject.FindProperty("m_CopyAvatar");
        this.oldModelSettings[index].humanoid = property1.intValue == 3;
        this.oldModelSettings[index].hasNoAnimation = property1.intValue == 0;
        this.oldModelSettings[index].copyAvatar = property2.boolValue;
      }
      this.newModelSettings = new ModelImporterRigEditor.MappingRelevantSettings[this.targets.Length];
      Array.Copy((Array) this.oldModelSettings, (Array) this.newModelSettings, this.targets.Length);
      for (int index = 0; index < this.targets.Length; ++index)
      {
        if (!this.m_AnimationType.hasMultipleDifferentValues)
          this.newModelSettings[index].humanoid = this.m_AnimationType.intValue == 3;
        if (!this.m_CopyAvatar.hasMultipleDifferentValues)
          this.newModelSettings[index].copyAvatar = this.m_CopyAvatar.boolValue;
      }
    }

    internal override void PostApply()
    {
      for (int index = 0; index < this.targets.Length; ++index)
      {
        if (this.oldModelSettings[index].usesOwnAvatar && !this.newModelSettings[index].usesOwnAvatar && !this.newModelSettings[index].copyAvatar)
        {
          SerializedObject serializedObject = new SerializedObject(this.targets[index]);
          AvatarSetupTool.ClearAll(serializedObject);
          serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
        if (!this.m_CopyAvatar.boolValue && !this.newModelSettings[index].humanoid && this.rootIndex > 0)
        {
          GameObject go = AssetDatabase.LoadMainAssetAtPath((this.targets[index] as ModelImporter).assetPath) as GameObject;
          Animator component = go.GetComponent<Animator>();
          bool flag = (bool) ((UnityEngine.Object) component) && !component.hasTransformHierarchy;
          if (flag)
          {
            go = this.Instantiate((UnityEngine.Object) go) as GameObject;
            AnimatorUtility.DeoptimizeTransformHierarchy(go);
          }
          Transform transform = go.transform.Find(this.m_RootMotionBoneList[this.rootIndex].text);
          if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
            this.m_RootMotionBoneRotation.quaternionValue = transform.rotation;
          new SerializedObject(this.targets[index]).ApplyModifiedPropertiesWithoutUndo();
          if (flag)
            this.DestroyImmediate((UnityEngine.Object) go);
        }
        if (!this.oldModelSettings[index].usesOwnAvatar && this.newModelSettings[index].usesOwnAvatar)
        {
          ModelImporter target = this.targets[index] as ModelImporter;
          if (this.oldModelSettings[index].hasNoAnimation)
          {
            ModelImporterAnimationType animationType = target.animationType;
            target.animationType = ModelImporterAnimationType.Generic;
            AssetDatabase.ImportAsset(target.assetPath);
            target.animationType = animationType;
          }
          SerializedObject modelImporterSerializedObject = new SerializedObject(this.targets[index]);
          GameObject gameObject = AssetDatabase.LoadMainAssetAtPath(target.assetPath) as GameObject;
          Animator component = gameObject.GetComponent<Animator>();
          bool flag = (bool) ((UnityEngine.Object) component) && !component.hasTransformHierarchy;
          if (flag)
          {
            gameObject = this.Instantiate((UnityEngine.Object) gameObject) as GameObject;
            AnimatorUtility.DeoptimizeTransformHierarchy(gameObject);
          }
          AvatarSetupTool.AutoSetupOnInstance(gameObject, modelImporterSerializedObject);
          this.m_IsBiped = AvatarBipedMapper.IsBiped(gameObject.transform, this.m_BipedMappingReport);
          if (flag)
            this.DestroyImmediate((UnityEngine.Object) gameObject);
          modelImporterSerializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
      }
      this.oldModelSettings = (ModelImporterRigEditor.MappingRelevantSettings[]) null;
      this.newModelSettings = (ModelImporterRigEditor.MappingRelevantSettings[]) null;
    }

    private class Styles
    {
      public GUIContent AnimationType = EditorGUIUtility.TextContent("Animation Type|The type of animation to support / import.");
      public GUIContent[] AnimationTypeOpt = new GUIContent[4]{ EditorGUIUtility.TextContent("None|No animation present."), EditorGUIUtility.TextContent("Legacy|Legacy animation system."), EditorGUIUtility.TextContent("Generic|Generic Mecanim animation."), EditorGUIUtility.TextContent("Humanoid|Humanoid Mecanim animation system.") };
      public GUIContent AnimLabel = EditorGUIUtility.TextContent("Generation|Controls how animations are imported.");
      public GUIContent[] AnimationsOpt = new GUIContent[5]{ EditorGUIUtility.TextContent("Don't Import|No animation or skinning is imported."), EditorGUIUtility.TextContent("Store in Original Roots (Deprecated)|Animations are stored in the root objects of your animation package (these might be different from the root objects in Unity)."), EditorGUIUtility.TextContent("Store in Nodes (Deprecated)|Animations are stored together with the objects they animate. Use this when you have a complex animation setup and want full scripting control."), EditorGUIUtility.TextContent("Store in Root (Deprecated)|Animations are stored in the scene's transform root objects. Use this when animating anything that has a hierarchy."), EditorGUIUtility.TextContent("Store in Root (New)") };
      public GUIStyle helpText = new GUIStyle(EditorStyles.helpBox);
      public GUIContent avatar = new GUIContent("Animator");
      public GUIContent configureAvatar = EditorGUIUtility.TextContent("Configure...");
      public GUIContent avatarValid = EditorGUIUtility.TextContent("✓");
      public GUIContent avatarInvalid = EditorGUIUtility.TextContent("✕");
      public GUIContent avatarPending = EditorGUIUtility.TextContent("...");
      public GUIContent UpdateMuscleDefinitionFromSource = EditorGUIUtility.TextContent("Update|Update the copy of the muscle definition from the source.");
      public GUIContent RootNode = EditorGUIUtility.TextContent("Root node|Specify the root node used to extract the animation translation.");
      public GUIContent AvatarDefinition = EditorGUIUtility.TextContent("Avatar Definition|Choose between Create From This Model or Copy From Other Avatar. The first one create an Avatar for this file and the second one use an Avatar from another file to import animation.");
      public GUIContent[] AvatarDefinitionOpt = new GUIContent[2]{ EditorGUIUtility.TextContent("Create From This Model|Create an Avatar based on the model from this file."), EditorGUIUtility.TextContent("Copy From Other Avatar|Copy an Avatar from another file to import muscle clip. No avatar will be created.") };
      public GUIContent UpdateReferenceClips = EditorGUIUtility.TextContent("Update reference clips|Click on this button to update all the @convention file referencing this file. Should set all these files to Copy From Other Avatar, set the source Avatar to this one and reimport all these files.");
      public GUIContent ImportMessages = EditorGUIUtility.TextContent("Import Messages");

      public Styles()
      {
        this.helpText.normal.background = (Texture2D) null;
        this.helpText.alignment = TextAnchor.MiddleLeft;
        this.helpText.padding = new RectOffset(0, 0, 0, 0);
      }
    }

    private struct MappingRelevantSettings
    {
      public bool humanoid;
      public bool copyAvatar;
      public bool hasNoAnimation;

      public bool usesOwnAvatar
      {
        get
        {
          return this.humanoid && !this.copyAvatar;
        }
      }
    }
  }
}
