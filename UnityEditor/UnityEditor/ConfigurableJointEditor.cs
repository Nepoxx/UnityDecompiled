// Decompiled with JetBrains decompiler
// Type: UnityEditor.ConfigurableJointEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof (ConfigurableJoint))]
  internal class ConfigurableJointEditor : JointEditor<ConfigurableJoint>
  {
    protected override void GetActors(ConfigurableJoint joint, out Rigidbody dynamicActor, out Rigidbody connectedActor, out int jointFrameActorIndex, out bool rightHandedLimit)
    {
      base.GetActors(joint, out dynamicActor, out connectedActor, out jointFrameActorIndex, out rightHandedLimit);
      if (!joint.swapBodies)
        return;
      jointFrameActorIndex = 0;
      rightHandedLimit = true;
    }

    protected override void DoAngularLimitHandles(ConfigurableJoint joint)
    {
      base.DoAngularLimitHandles(joint);
      this.angularLimitHandle.xMotion = joint.angularXMotion;
      this.angularLimitHandle.yMotion = joint.angularYMotion;
      this.angularLimitHandle.zMotion = joint.angularZMotion;
      this.angularLimitHandle.xMin = joint.lowAngularXLimit.limit;
      this.angularLimitHandle.xMax = joint.highAngularXLimit.limit;
      SoftJointLimit angularYlimit = joint.angularYLimit;
      this.angularLimitHandle.yMax = angularYlimit.limit;
      this.angularLimitHandle.yMin = -angularYlimit.limit;
      SoftJointLimit angularZlimit = joint.angularZLimit;
      this.angularLimitHandle.zMax = angularZlimit.limit;
      this.angularLimitHandle.zMin = -angularZlimit.limit;
      EditorGUI.BeginChangeCheck();
      this.angularLimitHandle.radius = JointEditor<ConfigurableJoint>.GetAngularLimitHandleSize(Vector3.zero);
      this.angularLimitHandle.DrawHandle();
      if (!EditorGUI.EndChangeCheck())
        return;
      Undo.RecordObject((Object) joint, JointEditor<ConfigurableJoint>.Styles.editAngularLimitsUndoMessage);
      SoftJointLimit lowAngularXlimit = joint.lowAngularXLimit;
      lowAngularXlimit.limit = this.angularLimitHandle.xMin;
      joint.lowAngularXLimit = lowAngularXlimit;
      SoftJointLimit softJointLimit = joint.highAngularXLimit;
      softJointLimit.limit = this.angularLimitHandle.xMax;
      joint.highAngularXLimit = softJointLimit;
      softJointLimit = joint.angularYLimit;
      softJointLimit.limit = (double) this.angularLimitHandle.yMax != (double) softJointLimit.limit ? this.angularLimitHandle.yMax : -this.angularLimitHandle.yMin;
      joint.angularYLimit = softJointLimit;
      softJointLimit = joint.angularZLimit;
      softJointLimit.limit = (double) this.angularLimitHandle.zMax != (double) softJointLimit.limit ? this.angularLimitHandle.zMax : -this.angularLimitHandle.zMin;
      joint.angularZLimit = softJointLimit;
    }
  }
}
