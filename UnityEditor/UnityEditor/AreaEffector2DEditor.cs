// Decompiled with JetBrains decompiler
// Type: UnityEditor.AreaEffector2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CustomEditor(typeof (AreaEffector2D), true)]
  [CanEditMultipleObjects]
  internal class AreaEffector2DEditor : Effector2DEditor
  {
    private static readonly AnimBool m_ShowDampingRollout = new AnimBool();
    private readonly AnimBool m_ShowForceRollout = new AnimBool();
    private SerializedProperty m_UseGlobalAngle;
    private SerializedProperty m_ForceAngle;
    private SerializedProperty m_ForceMagnitude;
    private SerializedProperty m_ForceVariation;
    private SerializedProperty m_ForceTarget;
    private SerializedProperty m_Drag;
    private SerializedProperty m_AngularDrag;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_ShowForceRollout.value = true;
      this.m_ShowForceRollout.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_UseGlobalAngle = this.serializedObject.FindProperty("m_UseGlobalAngle");
      this.m_ForceAngle = this.serializedObject.FindProperty("m_ForceAngle");
      this.m_ForceMagnitude = this.serializedObject.FindProperty("m_ForceMagnitude");
      this.m_ForceVariation = this.serializedObject.FindProperty("m_ForceVariation");
      this.m_ForceTarget = this.serializedObject.FindProperty("m_ForceTarget");
      AreaEffector2DEditor.m_ShowDampingRollout.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_Drag = this.serializedObject.FindProperty("m_Drag");
      this.m_AngularDrag = this.serializedObject.FindProperty("m_AngularDrag");
    }

    public override void OnDisable()
    {
      base.OnDisable();
      this.m_ShowForceRollout.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      AreaEffector2DEditor.m_ShowDampingRollout.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      this.serializedObject.Update();
      this.m_ShowForceRollout.target = EditorGUILayout.Foldout(this.m_ShowForceRollout.target, "Force", true);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowForceRollout.faded))
      {
        EditorGUILayout.PropertyField(this.m_UseGlobalAngle);
        EditorGUILayout.PropertyField(this.m_ForceAngle);
        EditorGUILayout.PropertyField(this.m_ForceMagnitude);
        EditorGUILayout.PropertyField(this.m_ForceVariation);
        EditorGUILayout.PropertyField(this.m_ForceTarget);
        EditorGUILayout.Space();
      }
      EditorGUILayout.EndFadeGroup();
      AreaEffector2DEditor.m_ShowDampingRollout.target = EditorGUILayout.Foldout(AreaEffector2DEditor.m_ShowDampingRollout.target, "Damping", true);
      if (EditorGUILayout.BeginFadeGroup(AreaEffector2DEditor.m_ShowDampingRollout.faded))
      {
        EditorGUILayout.PropertyField(this.m_Drag);
        EditorGUILayout.PropertyField(this.m_AngularDrag);
      }
      EditorGUILayout.EndFadeGroup();
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
