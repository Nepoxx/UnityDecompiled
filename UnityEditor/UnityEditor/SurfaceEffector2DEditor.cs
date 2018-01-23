// Decompiled with JetBrains decompiler
// Type: UnityEditor.SurfaceEffector2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
  [CustomEditor(typeof (SurfaceEffector2D), true)]
  [CanEditMultipleObjects]
  internal class SurfaceEffector2DEditor : Effector2DEditor
  {
    private static readonly AnimBool m_ShowOptionsRollout = new AnimBool();
    private readonly AnimBool m_ShowForceRollout = new AnimBool();
    private SerializedProperty m_Speed;
    private SerializedProperty m_SpeedVariation;
    private SerializedProperty m_ForceScale;
    private SerializedProperty m_UseContactForce;
    private SerializedProperty m_UseFriction;
    private SerializedProperty m_UseBounce;

    public override void OnEnable()
    {
      base.OnEnable();
      this.m_ShowForceRollout.value = true;
      this.m_ShowForceRollout.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_Speed = this.serializedObject.FindProperty("m_Speed");
      this.m_SpeedVariation = this.serializedObject.FindProperty("m_SpeedVariation");
      this.m_ForceScale = this.serializedObject.FindProperty("m_ForceScale");
      SurfaceEffector2DEditor.m_ShowOptionsRollout.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
      this.m_UseContactForce = this.serializedObject.FindProperty("m_UseContactForce");
      this.m_UseFriction = this.serializedObject.FindProperty("m_UseFriction");
      this.m_UseBounce = this.serializedObject.FindProperty("m_UseBounce");
    }

    public override void OnDisable()
    {
      base.OnDisable();
      this.m_ShowForceRollout.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
      SurfaceEffector2DEditor.m_ShowOptionsRollout.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      this.serializedObject.Update();
      this.m_ShowForceRollout.target = EditorGUILayout.Foldout(this.m_ShowForceRollout.target, "Force", true);
      if (EditorGUILayout.BeginFadeGroup(this.m_ShowForceRollout.faded))
      {
        EditorGUILayout.PropertyField(this.m_Speed);
        EditorGUILayout.PropertyField(this.m_SpeedVariation);
        EditorGUILayout.PropertyField(this.m_ForceScale);
        EditorGUILayout.Space();
      }
      EditorGUILayout.EndFadeGroup();
      SurfaceEffector2DEditor.m_ShowOptionsRollout.target = EditorGUILayout.Foldout(SurfaceEffector2DEditor.m_ShowOptionsRollout.target, "Options", true);
      if (EditorGUILayout.BeginFadeGroup(SurfaceEffector2DEditor.m_ShowOptionsRollout.faded))
      {
        EditorGUILayout.PropertyField(this.m_UseContactForce);
        EditorGUILayout.PropertyField(this.m_UseFriction);
        EditorGUILayout.PropertyField(this.m_UseBounce);
      }
      EditorGUILayout.EndFadeGroup();
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
