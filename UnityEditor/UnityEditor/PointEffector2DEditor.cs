// Decompiled with JetBrains decompiler
// Type: UnityEditor.PointEffector2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (PointEffector2D), true)]
  internal class PointEffector2DEditor : Effector2DEditor
  {
    private static readonly AnimBool m_ShowDampingRollout = new AnimBool();
    private readonly AnimBool m_ShowForceRollout = new AnimBool();
    private SerializedProperty m_ForceMagnitude;
    private SerializedProperty m_ForceVariation;
    private SerializedProperty m_ForceSource;
    private SerializedProperty m_ForceTarget;
    private SerializedProperty m_ForceMode;
    private SerializedProperty m_DistanceScale;
    private SerializedProperty m_Drag;
    private SerializedProperty m_AngularDrag;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_ShowForceRollout.value = true;
      this.m_ShowForceRollout.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_ForceMagnitude = this.serializedObject.FindProperty("m_ForceMagnitude");
      this.m_ForceVariation = this.serializedObject.FindProperty("m_ForceVariation");
      this.m_ForceSource = this.serializedObject.FindProperty("m_ForceSource");
      this.m_ForceTarget = this.serializedObject.FindProperty("m_ForceTarget");
      this.m_ForceMode = this.serializedObject.FindProperty("m_ForceMode");
      this.m_DistanceScale = this.serializedObject.FindProperty("m_DistanceScale");
      PointEffector2DEditor.m_ShowDampingRollout.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_Drag = this.serializedObject.FindProperty("m_Drag");
      this.m_AngularDrag = this.serializedObject.FindProperty("m_AngularDrag");
    }

    public override void OnDisable()
    {
      base.OnDisable();
      this.m_ShowForceRollout.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      PointEffector2DEditor.m_ShowDampingRollout.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      this.serializedObject.Update();
      this.m_ShowForceRollout.target = EditorGUILayout.Foldout(this.m_ShowForceRollout.target, "Force", true);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowForceRollout.faded))
      {
        EditorGUILayout.PropertyField(this.m_ForceMagnitude);
        EditorGUILayout.PropertyField(this.m_ForceVariation);
        EditorGUILayout.PropertyField(this.m_DistanceScale);
        EditorGUILayout.PropertyField(this.m_ForceSource);
        EditorGUILayout.PropertyField(this.m_ForceTarget);
        EditorGUILayout.PropertyField(this.m_ForceMode);
        EditorGUILayout.Space();
      }
      EditorGUILayout.EndFadeGroup();
      PointEffector2DEditor.m_ShowDampingRollout.target = EditorGUILayout.Foldout(PointEffector2DEditor.m_ShowDampingRollout.target, "Damping", true);
      if (EditorGUILayout.BeginFadeGroup(PointEffector2DEditor.m_ShowDampingRollout.faded))
      {
        EditorGUILayout.PropertyField(this.m_Drag);
        EditorGUILayout.PropertyField(this.m_AngularDrag);
      }
      EditorGUILayout.EndFadeGroup();
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
