// Decompiled with JetBrains decompiler
// Type: UnityEditor.CharacterJointEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (CharacterJoint))]
  [CanEditMultipleObjects]
  internal class CharacterJointEditor : JointEditor<CharacterJoint>
  {
    protected override void DoAngularLimitHandles(CharacterJoint joint)
    {
      base.DoAngularLimitHandles(joint);
      this.angularLimitHandle.xMotion = ConfigurableJointMotion.Limited;
      this.angularLimitHandle.yMotion = ConfigurableJointMotion.Limited;
      this.angularLimitHandle.zMotion = ConfigurableJointMotion.Limited;
      this.angularLimitHandle.xMin = joint.lowTwistLimit.limit;
      this.angularLimitHandle.xMax = joint.highTwistLimit.limit;
      SoftJointLimit swing1Limit = joint.swing1Limit;
      this.angularLimitHandle.yMax = swing1Limit.limit;
      this.angularLimitHandle.yMin = -swing1Limit.limit;
      SoftJointLimit swing2Limit = joint.swing2Limit;
      this.angularLimitHandle.zMax = swing2Limit.limit;
      this.angularLimitHandle.zMin = -swing2Limit.limit;
      EditorGUI.BeginChangeCheck();
      this.angularLimitHandle.radius = JointEditor<CharacterJoint>.GetAngularLimitHandleSize(Vector3.zero);
      this.angularLimitHandle.DrawHandle();
      if (!EditorGUI.EndChangeCheck())
        return;
      Undo.RecordObject((Object) joint, JointEditor<CharacterJoint>.Styles.editAngularLimitsUndoMessage);
      SoftJointLimit softJointLimit = joint.lowTwistLimit;
      softJointLimit.limit = this.angularLimitHandle.xMin;
      joint.lowTwistLimit = softJointLimit;
      softJointLimit = joint.highTwistLimit;
      softJointLimit.limit = this.angularLimitHandle.xMax;
      joint.highTwistLimit = softJointLimit;
      softJointLimit = joint.swing1Limit;
      softJointLimit.limit = (double) this.angularLimitHandle.yMax != (double) softJointLimit.limit ? this.angularLimitHandle.yMax : -this.angularLimitHandle.yMin;
      joint.swing1Limit = softJointLimit;
      softJointLimit = joint.swing2Limit;
      softJointLimit.limit = (double) this.angularLimitHandle.zMax != (double) softJointLimit.limit ? this.angularLimitHandle.zMax : -this.angularLimitHandle.zMin;
      joint.swing2Limit = softJointLimit;
    }
  }
}
