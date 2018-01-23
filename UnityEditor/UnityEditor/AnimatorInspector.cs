// Decompiled with JetBrains decompiler
// Type: UnityEditor.AnimatorInspector
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (Animator))]
  internal class AnimatorInspector : Editor
  {
    private AnimBool m_ShowWarningMessage = new AnimBool();
    private SerializedProperty m_Avatar;
    private SerializedProperty m_ApplyRootMotion;
    private SerializedProperty m_CullingMode;
    private SerializedProperty m_UpdateMode;
    private SerializedProperty m_WarningMessage;
    private bool m_IsRootPositionOrRotationControlledByCurves;
    private static AnimatorInspector.Styles styles;

    private bool IsWarningMessageEmpty
    {
      get
      {
        return this.m_WarningMessage != null && this.m_WarningMessage.stringValue.Length > 0;
      }
    }

    private string WarningMessage
    {
      get
      {
        return this.m_WarningMessage == null ? "" : this.m_WarningMessage.stringValue;
      }
    }

    private void Init()
    {
      if (AnimatorInspector.styles == null)
        AnimatorInspector.styles = new AnimatorInspector.Styles();
      this.InitShowOptions();
    }

    private void InitShowOptions()
    {
      this.m_ShowWarningMessage.value = this.IsWarningMessageEmpty;
      this.m_ShowWarningMessage.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    private void UpdateShowOptions()
    {
      this.m_ShowWarningMessage.target = this.IsWarningMessageEmpty;
    }

    private void OnEnable()
    {
      this.m_Avatar = this.serializedObject.FindProperty("m_Avatar");
      this.m_ApplyRootMotion = this.serializedObject.FindProperty("m_ApplyRootMotion");
      this.m_CullingMode = this.serializedObject.FindProperty("m_CullingMode");
      this.m_UpdateMode = this.serializedObject.FindProperty("m_UpdateMode");
      this.m_WarningMessage = this.serializedObject.FindProperty("m_WarningMessage");
      this.Init();
    }

    public override void OnInspectorGUI()
    {
      bool flag1 = this.targets.Length > 1;
      Animator target1 = this.target as Animator;
      this.serializedObject.UpdateIfRequiredOrScript();
      this.UpdateShowOptions();
      EditorGUI.BeginChangeCheck();
      RuntimeAnimatorController animatorController = EditorGUILayout.ObjectField("Controller", (Object) target1.runtimeAnimatorController, typeof (RuntimeAnimatorController), false, new GUILayoutOption[0]) as RuntimeAnimatorController;
      if (EditorGUI.EndChangeCheck())
      {
        foreach (Animator target2 in this.targets)
        {
          Undo.RecordObject((Object) target2, "Changed AnimatorController");
          target2.runtimeAnimatorController = animatorController;
        }
        AnimationWindowUtility.ControllerChanged();
      }
      EditorGUILayout.PropertyField(this.m_Avatar);
      if (target1.supportsOnAnimatorMove && !flag1)
      {
        EditorGUILayout.LabelField("Apply Root Motion", "Handled by Script", new GUILayoutOption[0]);
      }
      else
      {
        EditorGUILayout.PropertyField(this.m_ApplyRootMotion, AnimatorInspector.styles.applyRootMotion, new GUILayoutOption[0]);
        if (Event.current.type == EventType.Layout)
          this.m_IsRootPositionOrRotationControlledByCurves = target1.isRootPositionOrRotationControlledByCurves;
        if (!this.m_ApplyRootMotion.boolValue && this.m_IsRootPositionOrRotationControlledByCurves)
          EditorGUILayout.HelpBox("Root position or rotation are controlled by curves", MessageType.Info, true);
      }
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_UpdateMode, AnimatorInspector.styles.updateMode, new GUILayoutOption[0]);
      bool flag2 = EditorGUI.EndChangeCheck();
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.m_CullingMode, AnimatorInspector.styles.cullingMode, new GUILayoutOption[0]);
      bool flag3 = EditorGUI.EndChangeCheck();
      if (!flag1)
        EditorGUILayout.HelpBox(target1.GetStats(), MessageType.Info, true);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowWarningMessage.faded))
        EditorGUILayout.HelpBox(this.WarningMessage, MessageType.Warning, true);
      EditorGUILayout.EndFadeGroup();
      this.serializedObject.ApplyModifiedProperties();
      foreach (Animator target2 in this.targets)
      {
        if (flag3)
          target2.OnCullingModeChanged();
        if (flag2)
          target2.OnUpdateModeChanged();
      }
    }

    private class Styles
    {
      public GUIContent applyRootMotion = new GUIContent(EditorGUIUtility.TextContent("Apply Root Motion"));
      public GUIContent updateMode = new GUIContent(EditorGUIUtility.TextContent("Update Mode"));
      public GUIContent cullingMode = new GUIContent(EditorGUIUtility.TextContent("Culling Mode"));

      public Styles()
      {
        this.applyRootMotion.tooltip = "Automatically move the object using the root motion from the animations";
        this.updateMode.tooltip = "Controls when and how often the Animator is updated";
        this.cullingMode.tooltip = "Controls what is updated when the object has been culled";
      }
    }
  }
}
