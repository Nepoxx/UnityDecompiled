// Decompiled with JetBrains decompiler
// Type: UnityEditor.WheelColliderEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (WheelCollider))]
  [CanEditMultipleObjects]
  internal class WheelColliderEditor : Editor
  {
    private SerializedProperty m_Center;
    private SerializedProperty m_Radius;
    private SerializedProperty m_SuspensionDistance;
    private SerializedProperty m_SuspensionSpring;
    private SerializedProperty m_ForceAppPointDistance;
    private SerializedProperty m_Mass;
    private SerializedProperty m_WheelDampingRate;
    private SerializedProperty m_ForwardFriction;
    private SerializedProperty m_SidewaysFriction;

    public void OnEnable()
    {
      this.m_Center = this.serializedObject.FindProperty("m_Center");
      this.m_Radius = this.serializedObject.FindProperty("m_Radius");
      this.m_SuspensionDistance = this.serializedObject.FindProperty("m_SuspensionDistance");
      this.m_SuspensionSpring = this.serializedObject.FindProperty("m_SuspensionSpring");
      this.m_Mass = this.serializedObject.FindProperty("m_Mass");
      this.m_ForceAppPointDistance = this.serializedObject.FindProperty("m_ForceAppPointDistance");
      this.m_WheelDampingRate = this.serializedObject.FindProperty("m_WheelDampingRate");
      this.m_ForwardFriction = this.serializedObject.FindProperty("m_ForwardFriction");
      this.m_SidewaysFriction = this.serializedObject.FindProperty("m_SidewaysFriction");
    }

    public override void OnInspectorGUI()
    {
      this.serializedObject.Update();
      EditorGUILayout.PropertyField(this.m_Mass);
      EditorGUILayout.PropertyField(this.m_Radius);
      EditorGUILayout.PropertyField(this.m_WheelDampingRate);
      EditorGUILayout.PropertyField(this.m_SuspensionDistance);
      EditorGUILayout.PropertyField(this.m_ForceAppPointDistance);
      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(this.m_Center);
      EditorGUILayout.Space();
      StructPropertyGUILayout.GenericStruct(this.m_SuspensionSpring);
      StructPropertyGUILayout.GenericStruct(this.m_ForwardFriction);
      StructPropertyGUILayout.GenericStruct(this.m_SidewaysFriction);
      this.serializedObject.ApplyModifiedProperties();
    }
  }
}
