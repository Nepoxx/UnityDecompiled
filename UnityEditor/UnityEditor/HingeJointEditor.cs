// Decompiled with JetBrains decompiler
// Type: UnityEditor.HingeJointEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (HingeJoint))]
  [CanEditMultipleObjects]
  internal class HingeJointEditor : JointEditor<HingeJoint>
  {
    private static readonly GUIContent s_WarningMessage = EditorGUIUtility.TextContent("Min and max limits must be within the range [-180, 180].");
    private SerializedProperty m_MinLimit;
    private SerializedProperty m_MaxLimit;

    private void OnEnable()
    {
      this.angularLimitHandle.yMotion = ConfigurableJointMotion.Locked;
      this.angularLimitHandle.zMotion = ConfigurableJointMotion.Locked;
      this.angularLimitHandle.yHandleColor = Color.clear;
      this.angularLimitHandle.zHandleColor = Color.clear;
      this.angularLimitHandle.xRange = new Vector2(-3.402823E+38f, 3.402823E+38f);
      this.m_MinLimit = this.serializedObject.FindProperty("m_Limits.min");
      this.m_MaxLimit = this.serializedObject.FindProperty("m_Limits.max");
    }

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      float floatValue1 = this.m_MinLimit.floatValue;
      float floatValue2 = this.m_MaxLimit.floatValue;
      if ((double) floatValue1 >= -180.0 && (double) floatValue1 <= 180.0 && ((double) floatValue2 >= -180.0 && (double) floatValue2 <= 180.0))
        return;
      EditorGUILayout.HelpBox(HingeJointEditor.s_WarningMessage.text, MessageType.Warning);
    }

    protected override void GetActors(HingeJoint joint, out Rigidbody dynamicActor, out Rigidbody connectedActor, out int jointFrameActorIndex, out bool rightHandedLimit)
    {
      base.GetActors(joint, out dynamicActor, out connectedActor, out jointFrameActorIndex, out rightHandedLimit);
      rightHandedLimit = true;
    }

    protected override void DoAngularLimitHandles(HingeJoint joint)
    {
      base.DoAngularLimitHandles(joint);
      this.angularLimitHandle.xMotion = !joint.useLimits ? ConfigurableJointMotion.Free : ConfigurableJointMotion.Limited;
      JointLimits limits1 = joint.limits;
      this.angularLimitHandle.xMin = limits1.min;
      this.angularLimitHandle.xMax = limits1.max;
      EditorGUI.BeginChangeCheck();
      this.angularLimitHandle.radius = JointEditor<HingeJoint>.GetAngularLimitHandleSize(Vector3.zero);
      this.angularLimitHandle.DrawHandle();
      if (!EditorGUI.EndChangeCheck())
        return;
      Undo.RecordObject((Object) joint, JointEditor<HingeJoint>.Styles.editAngularLimitsUndoMessage);
      JointLimits limits2 = joint.limits;
      limits2.min = this.angularLimitHandle.xMin;
      limits2.max = this.angularLimitHandle.xMax;
      joint.limits = limits2;
    }
  }
}
