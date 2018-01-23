// Decompiled with JetBrains decompiler
// Type: UnityEditor.AvatarEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  [CustomEditor(typeof (Avatar))]
  internal class AvatarEditor : Editor
  {
    internal static bool s_EditImmediatelyOnNextOpen = false;
    internal Dictionary<Transform, bool> m_ModelBones = (Dictionary<Transform, bool>) null;
    private AvatarEditor.EditMode m_EditMode = AvatarEditor.EditMode.NotEditing;
    internal bool m_CameFromImportSettings = false;
    private bool m_SwitchToEditMode = false;
    private static AvatarEditor.Styles s_Styles;
    protected int m_TabIndex;
    internal GameObject m_GameObject;
    private SceneSetup[] sceneSetup;
    protected bool m_InspectorLocked;
    protected List<AvatarEditor.SceneStateCache> m_SceneStates;
    private AvatarMuscleEditor m_MuscleEditor;
    private AvatarHandleEditor m_HandleEditor;
    private AvatarColliderEditor m_ColliderEditor;
    private AvatarMappingEditor m_MappingEditor;
    private const int sMappingTab = 0;
    private const int sMuscleTab = 1;
    private const int sHandleTab = 2;
    private const int sColliderTab = 3;
    private const int sDefaultTab = 0;

    private static AvatarEditor.Styles styles
    {
      get
      {
        if (AvatarEditor.s_Styles == null)
          AvatarEditor.s_Styles = new AvatarEditor.Styles();
        return AvatarEditor.s_Styles;
      }
    }

    internal Avatar avatar
    {
      get
      {
        return this.target as Avatar;
      }
    }

    protected AvatarSubEditor editor
    {
      get
      {
        switch (this.m_TabIndex)
        {
          case 1:
            return (AvatarSubEditor) this.m_MuscleEditor;
          case 2:
            return (AvatarSubEditor) this.m_HandleEditor;
          case 3:
            return (AvatarSubEditor) this.m_ColliderEditor;
          default:
            return (AvatarSubEditor) this.m_MappingEditor;
        }
      }
      set
      {
        switch (this.m_TabIndex)
        {
          case 1:
            this.m_MuscleEditor = value as AvatarMuscleEditor;
            break;
          case 2:
            this.m_HandleEditor = value as AvatarHandleEditor;
            break;
          case 3:
            this.m_ColliderEditor = value as AvatarColliderEditor;
            break;
          default:
            this.m_MappingEditor = value as AvatarMappingEditor;
            break;
        }
      }
    }

    public GameObject prefab
    {
      get
      {
        return AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(this.target)) as GameObject;
      }
    }

    internal override SerializedObject GetSerializedObjectInternal()
    {
      if (this.m_SerializedObject == null)
        this.m_SerializedObject = SerializedObject.LoadFromCache(this.GetInstanceID());
      if (this.m_SerializedObject == null)
        this.m_SerializedObject = new SerializedObject((UnityEngine.Object) AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(this.target)));
      return this.m_SerializedObject;
    }

    private void OnEnable()
    {
      EditorApplication.update += new EditorApplication.CallbackFunction(this.Update);
      this.m_SwitchToEditMode = false;
      if (this.m_EditMode == AvatarEditor.EditMode.Editing)
      {
        this.m_ModelBones = AvatarSetupTool.GetModelBones(this.m_GameObject.transform, false, (AvatarSetupTool.BoneWrapper[]) null);
        this.editor.Enable(this);
      }
      else
      {
        if (this.m_EditMode != AvatarEditor.EditMode.NotEditing)
          return;
        this.editor = (AvatarSubEditor) null;
        if (AvatarEditor.s_EditImmediatelyOnNextOpen)
        {
          this.m_CameFromImportSettings = true;
          AvatarEditor.s_EditImmediatelyOnNextOpen = false;
        }
      }
    }

    private void OnDisable()
    {
      if (this.m_EditMode == AvatarEditor.EditMode.Editing)
        this.editor.Disable();
      EditorApplication.update -= new EditorApplication.CallbackFunction(this.Update);
      if (this.m_SerializedObject == null)
        return;
      this.m_SerializedObject.Cache(this.GetInstanceID());
      this.m_SerializedObject = (SerializedObject) null;
    }

    private void OnDestroy()
    {
      if (this.m_EditMode != AvatarEditor.EditMode.Editing)
        return;
      this.SwitchToAssetMode();
    }

    private void SelectAsset()
    {
      Selection.activeObject = !this.m_CameFromImportSettings ? this.target : AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(this.target));
    }

    protected void CreateEditor()
    {
      switch (this.m_TabIndex)
      {
        case 1:
          this.editor = (AvatarSubEditor) ScriptableObject.CreateInstance<AvatarMuscleEditor>();
          break;
        case 2:
          this.editor = (AvatarSubEditor) ScriptableObject.CreateInstance<AvatarHandleEditor>();
          break;
        case 3:
          this.editor = (AvatarSubEditor) ScriptableObject.CreateInstance<AvatarColliderEditor>();
          break;
        default:
          this.editor = (AvatarSubEditor) ScriptableObject.CreateInstance<AvatarMappingEditor>();
          break;
      }
      this.editor.hideFlags = HideFlags.HideAndDontSave;
      this.editor.Enable(this);
    }

    protected void DestroyEditor()
    {
      this.editor.OnDestroy();
      this.editor = (AvatarSubEditor) null;
    }

    public override bool UseDefaultMargins()
    {
      return false;
    }

    public override void OnInspectorGUI()
    {
      GUI.enabled = true;
      EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins, new GUILayoutOption[0]);
      if (this.m_EditMode == AvatarEditor.EditMode.Editing)
        this.EditingGUI();
      else if (!this.m_CameFromImportSettings)
        this.EditButtonGUI();
      else if (this.m_EditMode == AvatarEditor.EditMode.NotEditing && Event.current.type == EventType.Repaint)
        this.m_SwitchToEditMode = true;
      EditorGUILayout.EndVertical();
    }

    private void EditButtonGUI()
    {
      if ((UnityEngine.Object) this.avatar == (UnityEngine.Object) null || !this.avatar.isHuman || (UnityEngine.Object) (AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object) this.avatar)) as ModelImporter) == (UnityEngine.Object) null)
        return;
      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      if (GUILayout.Button(AvatarEditor.styles.editCharacter, new GUILayoutOption[1]{ GUILayout.Width(120f) }) && EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
      {
        this.SwitchToEditMode();
        GUIUtility.ExitGUI();
      }
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
    }

    private void EditingGUI()
    {
      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      int tabIndex = this.m_TabIndex;
      bool enabled = GUI.enabled;
      GUI.enabled = ((UnityEngine.Object) this.avatar == (UnityEngine.Object) null ? 1 : (!this.avatar.isHuman ? 1 : 0)) == 0;
      int num = GUILayout.Toolbar(tabIndex, AvatarEditor.styles.tabs, (GUIStyle) "LargeButton", GUI.ToolbarButtonSize.FitToContents, new GUILayoutOption[0]);
      GUI.enabled = enabled;
      if (num != this.m_TabIndex)
      {
        this.DestroyEditor();
        if ((UnityEngine.Object) this.avatar != (UnityEngine.Object) null && this.avatar.isHuman)
          this.m_TabIndex = num;
        this.CreateEditor();
      }
      GUILayout.FlexibleSpace();
      EditorGUILayout.EndHorizontal();
      this.editor.OnInspectorGUI();
    }

    public void OnSceneGUI()
    {
      if (this.m_EditMode != AvatarEditor.EditMode.Editing)
        return;
      this.editor.OnSceneGUI();
    }

    internal void SwitchToEditMode()
    {
      this.m_EditMode = AvatarEditor.EditMode.Starting;
      this.ChangeInspectorLock(true);
      this.sceneSetup = EditorSceneManager.GetSceneManagerSetup();
      EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects).name = "Avatar Configuration";
      this.m_GameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefab);
      if (this.serializedObject.FindProperty("m_OptimizeGameObjects").boolValue)
        AnimatorUtility.DeoptimizeTransformHierarchy(this.m_GameObject);
      Animator component = this.m_GameObject.GetComponent<Animator>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.runtimeAnimatorController == (UnityEngine.Object) null)
      {
        AnimatorController animatorController = new AnimatorController();
        animatorController.hideFlags = HideFlags.DontSave;
        animatorController.AddLayer("preview");
        animatorController.layers[0].stateMachine.hideFlags = HideFlags.DontSave;
        component.runtimeAnimatorController = (RuntimeAnimatorController) animatorController;
      }
      this.m_ModelBones = AvatarSetupTool.GetModelBones(this.m_GameObject.transform, false, AvatarSetupTool.GetHumanBones(this.serializedObject, AvatarSetupTool.GetModelBones(this.m_GameObject.transform, true, (AvatarSetupTool.BoneWrapper[]) null)));
      Selection.activeObject = (UnityEngine.Object) this.m_GameObject;
      foreach (SceneHierarchyWindow sceneHierarchyWindow in UnityEngine.Resources.FindObjectsOfTypeAll(typeof (SceneHierarchyWindow)))
        sceneHierarchyWindow.SetExpandedRecursive(this.m_GameObject.GetInstanceID(), true);
      this.CreateEditor();
      this.m_EditMode = AvatarEditor.EditMode.Editing;
      this.m_SceneStates = new List<AvatarEditor.SceneStateCache>();
      IEnumerator enumerator = SceneView.sceneViews.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          SceneView current = (SceneView) enumerator.Current;
          this.m_SceneStates.Add(new AvatarEditor.SceneStateCache()
          {
            state = new SceneView.SceneViewState(current.sceneViewState),
            view = current
          });
          current.sceneViewState.showFlares = false;
          current.sceneViewState.showMaterialUpdate = false;
          current.sceneViewState.showFog = false;
          current.sceneViewState.showSkybox = false;
          current.sceneViewState.showImageEffects = false;
          current.sceneViewState.showParticleSystems = false;
          current.FrameSelected();
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
    }

    internal void SwitchToAssetMode()
    {
      foreach (AvatarEditor.SceneStateCache sceneState in this.m_SceneStates)
      {
        if (!((UnityEngine.Object) sceneState.view == (UnityEngine.Object) null))
        {
          sceneState.view.sceneViewState.showFog = sceneState.state.showFog;
          sceneState.view.sceneViewState.showFlares = sceneState.state.showFlares;
          sceneState.view.sceneViewState.showMaterialUpdate = sceneState.state.showMaterialUpdate;
          sceneState.view.sceneViewState.showSkybox = sceneState.state.showSkybox;
          sceneState.view.sceneViewState.showImageEffects = sceneState.state.showImageEffects;
          sceneState.view.sceneViewState.showParticleSystems = sceneState.state.showParticleSystems;
        }
      }
      this.m_EditMode = AvatarEditor.EditMode.Stopping;
      this.DestroyEditor();
      this.ChangeInspectorLock(this.m_InspectorLocked);
      if (!EditorApplication.isUpdating && !Unsupported.IsDestroyScriptableObject((ScriptableObject) this))
      {
        if (SceneManager.GetActiveScene().path.Length <= 0)
        {
          if (this.sceneSetup != null && this.sceneSetup.Length > 0)
          {
            EditorSceneManager.RestoreSceneManagerSetup(this.sceneSetup);
            this.sceneSetup = (SceneSetup[]) null;
          }
          else
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        }
      }
      else if (Unsupported.IsDestroyScriptableObject((ScriptableObject) this))
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AvatarEditor.\u003CSwitchToAssetMode\u003Ec__AnonStorey0 modeCAnonStorey0 = new AvatarEditor.\u003CSwitchToAssetMode\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        modeCAnonStorey0.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        modeCAnonStorey0.CleanUpSceneOnDestroy = (EditorApplication.CallbackFunction) null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        modeCAnonStorey0.CleanUpSceneOnDestroy = new EditorApplication.CallbackFunction(modeCAnonStorey0.\u003C\u003Em__0);
        // ISSUE: reference to a compiler-generated field
        EditorApplication.update += modeCAnonStorey0.CleanUpSceneOnDestroy;
      }
      this.m_GameObject = (GameObject) null;
      this.m_ModelBones = (Dictionary<Transform, bool>) null;
      this.SelectAsset();
      if (this.m_CameFromImportSettings)
        return;
      this.m_EditMode = AvatarEditor.EditMode.NotEditing;
    }

    private void ChangeInspectorLock(bool locked)
    {
      foreach (InspectorWindow allInspectorWindow in InspectorWindow.GetAllInspectorWindows())
      {
        foreach (UnityEngine.Object activeEditor in allInspectorWindow.tracker.activeEditors)
        {
          if (activeEditor == (UnityEngine.Object) this)
          {
            this.m_InspectorLocked = allInspectorWindow.isLocked;
            allInspectorWindow.isLocked = locked;
          }
        }
      }
    }

    public void Update()
    {
      if (this.m_SwitchToEditMode)
      {
        this.m_SwitchToEditMode = false;
        this.SwitchToEditMode();
        this.Repaint();
      }
      if (this.m_EditMode != AvatarEditor.EditMode.Editing)
        return;
      if ((UnityEngine.Object) this.m_GameObject == (UnityEngine.Object) null || this.m_ModelBones == null || EditorApplication.isPlaying)
        this.SwitchToAssetMode();
      else if (this.m_ModelBones != null)
      {
        foreach (KeyValuePair<Transform, bool> modelBone in this.m_ModelBones)
        {
          if ((UnityEngine.Object) modelBone.Key == (UnityEngine.Object) null)
          {
            this.SwitchToAssetMode();
            break;
          }
        }
      }
    }

    public bool HasFrameBounds()
    {
      if (this.m_ModelBones != null)
      {
        foreach (KeyValuePair<Transform, bool> modelBone in this.m_ModelBones)
        {
          if ((UnityEngine.Object) modelBone.Key == (UnityEngine.Object) Selection.activeTransform)
            return true;
        }
      }
      return false;
    }

    public Bounds OnGetFrameBounds()
    {
      Transform activeTransform = Selection.activeTransform;
      Bounds bounds = new Bounds(activeTransform.position, new Vector3(0.0f, 0.0f, 0.0f));
      IEnumerator enumerator = activeTransform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          Transform current = (Transform) enumerator.Current;
          bounds.Encapsulate(current.position);
        }
      }
      finally
      {
        IDisposable disposable;
        if ((disposable = enumerator as IDisposable) != null)
          disposable.Dispose();
      }
      if ((bool) ((UnityEngine.Object) activeTransform.parent))
        bounds.Encapsulate(activeTransform.parent.position);
      return bounds;
    }

    private class Styles
    {
      public GUIContent[] tabs = new GUIContent[2]{ EditorGUIUtility.TextContent("Mapping"), EditorGUIUtility.TextContent("Muscles & Settings") };
      public GUIContent editCharacter = EditorGUIUtility.TextContent("Configure Avatar");
      public GUIContent reset = EditorGUIUtility.TextContent("Reset");
    }

    private enum EditMode
    {
      NotEditing,
      Starting,
      Editing,
      Stopping,
    }

    [Serializable]
    protected class SceneStateCache
    {
      public SceneView view;
      public SceneView.SceneViewState state;
    }
  }
}
