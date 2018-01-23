// Decompiled with JetBrains decompiler
// Type: UnityEditor.LightProbeGroupInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (LightProbeGroup))]
  internal class LightProbeGroupInspector : Editor
  {
    private LightProbeGroupEditor m_Editor;
    private bool m_EditingProbes;
    private bool m_ShouldFocus;

    public void OnEnable()
    {
      this.m_Editor = new LightProbeGroupEditor(this.target as LightProbeGroup, this);
      this.m_Editor.PullProbePositions();
      this.m_Editor.DeselectProbes();
      this.m_Editor.PushProbePositions();
      SceneView.onSceneGUIDelegate += new SceneView.OnSceneFunc(this.OnSceneGUIDelegate);
      Undo.undoRedoPerformed += new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      UnityEditorInternal.EditMode.editModeStarted += new Action<IToolModeOwner, UnityEditorInternal.EditMode.SceneViewEditMode>(this.OnEditModeStarted);
      UnityEditorInternal.EditMode.editModeEnded += new Action<IToolModeOwner>(this.OnEditModeEnded);
    }

    private void OnEditModeEnded(IToolModeOwner owner)
    {
      if (owner != this)
        return;
      this.EndEditProbes();
    }

    private void OnEditModeStarted(IToolModeOwner owner, UnityEditorInternal.EditMode.SceneViewEditMode mode)
    {
      if (owner != this || mode != UnityEditorInternal.EditMode.SceneViewEditMode.LightProbeGroup)
        return;
      this.StartEditProbes();
    }

    public void StartEditMode()
    {
      UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.LightProbeGroup, this.m_Editor.bounds, (Editor) this);
    }

    private void StartEditProbes()
    {
      if (this.m_EditingProbes)
        return;
      this.m_EditingProbes = true;
      this.m_Editor.SetEditing(true);
      Tools.s_Hidden = true;
      SceneView.RepaintAll();
    }

    private void EndEditProbes()
    {
      if (!this.m_EditingProbes)
        return;
      this.m_Editor.drawTetrahedra = true;
      this.m_Editor.DeselectProbes();
      this.m_Editor.SetEditing(false);
      this.m_EditingProbes = false;
      Tools.s_Hidden = false;
      SceneView.RepaintAll();
    }

    public void OnDisable()
    {
      this.EndEditProbes();
      Undo.undoRedoPerformed -= new Undo.UndoRedoCallback(this.UndoRedoPerformed);
      SceneView.onSceneGUIDelegate -= new SceneView.OnSceneFunc(this.OnSceneGUIDelegate);
      if (!(this.target != (UnityEngine.Object) null))
        return;
      this.m_Editor.PushProbePositions();
      this.m_Editor = (LightProbeGroupEditor) null;
    }

    private void UndoRedoPerformed()
    {
      this.m_Editor.PullProbePositions();
      this.m_Editor.MarkTetrahedraDirty();
    }

    public override void OnInspectorGUI()
    {
      EditorGUI.BeginChangeCheck();
      this.m_Editor.PullProbePositions();
      UnityEditorInternal.EditMode.DoEditModeInspectorModeButton(UnityEditorInternal.EditMode.SceneViewEditMode.LightProbeGroup, "Edit Light Probes", LightProbeGroupInspector.Styles.editModeButton, (IToolModeOwner) this);
      GUILayout.Space(3f);
      EditorGUI.BeginDisabledGroup(UnityEditorInternal.EditMode.editMode != UnityEditorInternal.EditMode.SceneViewEditMode.LightProbeGroup);
      this.m_Editor.drawTetrahedra = EditorGUILayout.Toggle(LightProbeGroupInspector.Styles.showWireframe, this.m_Editor.drawTetrahedra, new GUILayoutOption[0]);
      EditorGUI.BeginDisabledGroup(this.m_Editor.SelectedCount == 0);
      Vector3 vector3_1 = this.m_Editor.SelectedCount <= 0 ? Vector3.zero : this.m_Editor.GetSelectedPositions()[0];
      Vector3 vector3_2 = EditorGUILayout.Vector3Field(LightProbeGroupInspector.Styles.selectedProbePosition, vector3_1);
      if (vector3_2 != vector3_1)
      {
        Vector3[] selectedPositions = this.m_Editor.GetSelectedPositions();
        Vector3 vector3_3 = vector3_2 - vector3_1;
        for (int idx = 0; idx < selectedPositions.Length; ++idx)
          this.m_Editor.UpdateSelectedPosition(idx, selectedPositions[idx] + vector3_3);
      }
      EditorGUI.EndDisabledGroup();
      GUILayout.Space(3f);
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical();
      if (GUILayout.Button(LightProbeGroupInspector.Styles.addProbe))
      {
        Vector3 position = Vector3.zero;
        if ((bool) ((UnityEngine.Object) SceneView.lastActiveSceneView))
        {
          LightProbeGroup target = this.target as LightProbeGroup;
          if ((bool) ((UnityEngine.Object) target))
            position = target.transform.InverseTransformPoint(position);
        }
        this.StartEditProbes();
        this.m_Editor.DeselectProbes();
        this.m_Editor.AddProbe(position);
      }
      if (GUILayout.Button(LightProbeGroupInspector.Styles.deleteSelected))
      {
        this.StartEditProbes();
        this.m_Editor.RemoveSelectedProbes();
      }
      GUILayout.EndVertical();
      GUILayout.BeginVertical();
      if (GUILayout.Button(LightProbeGroupInspector.Styles.selectAll))
      {
        this.StartEditProbes();
        this.m_Editor.SelectAllProbes();
      }
      if (GUILayout.Button(LightProbeGroupInspector.Styles.duplicateSelected))
      {
        this.StartEditProbes();
        this.m_Editor.DuplicateSelectedProbes();
      }
      GUILayout.EndVertical();
      GUILayout.EndHorizontal();
      EditorGUI.EndDisabledGroup();
      this.m_Editor.HandleEditMenuHotKeyCommands();
      this.m_Editor.PushProbePositions();
      if (!EditorGUI.EndChangeCheck())
        return;
      this.m_Editor.MarkTetrahedraDirty();
      SceneView.RepaintAll();
    }

    internal override Bounds GetWorldBoundsOfTarget(UnityEngine.Object targetObject)
    {
      return this.m_Editor.bounds;
    }

    private void InternalOnSceneView()
    {
      if (!EditorGUIUtility.IsGizmosAllowedForObject(this.target))
        return;
      if ((UnityEngine.Object) SceneView.lastActiveSceneView != (UnityEngine.Object) null && this.m_ShouldFocus)
      {
        this.m_ShouldFocus = false;
        SceneView.lastActiveSceneView.FrameSelected();
      }
      this.m_Editor.PullProbePositions();
      LightProbeGroup target = this.target as LightProbeGroup;
      if ((UnityEngine.Object) target != (UnityEngine.Object) null)
      {
        if (this.m_Editor.OnSceneGUI(target.transform))
          this.StartEditProbes();
        else
          this.EndEditProbes();
      }
      this.m_Editor.PushProbePositions();
    }

    public void OnSceneGUI()
    {
      if (Event.current.type == EventType.Repaint)
        return;
      this.InternalOnSceneView();
    }

    public void OnSceneGUIDelegate(SceneView sceneView)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      this.InternalOnSceneView();
    }

    public bool HasFrameBounds()
    {
      return this.m_Editor.SelectedCount > 0;
    }

    public Bounds OnGetFrameBounds()
    {
      return this.m_Editor.selectedProbeBounds;
    }

    private static class Styles
    {
      public static readonly GUIContent showWireframe = new GUIContent("Show Wireframe", "Show the tetrahedron wireframe visualizing the blending between probes.");
      public static readonly GUIContent selectedProbePosition = new GUIContent("Selected Probe Position", "The local position of this probe relative to the parent group.");
      public static readonly GUIContent addProbe = new GUIContent("Add Probe");
      public static readonly GUIContent deleteSelected = new GUIContent("Delete Selected");
      public static readonly GUIContent selectAll = new GUIContent("Select All");
      public static readonly GUIContent duplicateSelected = new GUIContent("Duplicate Selected");
      public static readonly GUIContent editModeButton = EditorGUIUtility.IconContent("EditCollider");
    }
  }
}
