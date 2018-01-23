// Decompiled with JetBrains decompiler
// Type: UnityEditor.TargetJoint2DEditor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [CustomEditor(typeof (TargetJoint2D))]
  [CanEditMultipleObjects]
  internal class TargetJoint2DEditor : Joint2DEditor
  {
    public void OnSceneGUI()
    {
      TargetJoint2D target1 = (TargetJoint2D) this.target;
      if (!target1.enabled)
        return;
      Vector3 position = Joint2DEditor.TransformPoint(target1.transform, (Vector3) target1.anchor);
      Vector3 target2 = (Vector3) target1.target;
      Handles.color = Color.green;
      Handles.DrawDottedLine(position, target2, 5f);
      if (this.HandleAnchor(ref position, false))
      {
        Undo.RecordObject((Object) target1, "Move Anchor");
        target1.anchor = (Vector2) Joint2DEditor.InverseTransformPoint(target1.transform, position);
      }
      float num = HandleUtility.GetHandleSize(target2) * 0.3f;
      Vector3 vector3_1 = Vector3.left * num;
      Vector3 vector3_2 = Vector3.up * num;
      Joint2DEditor.DrawAALine(target2 - vector3_1, target2 + vector3_1);
      Joint2DEditor.DrawAALine(target2 - vector3_2, target2 + vector3_2);
      if (!this.HandleAnchor(ref target2, true))
        return;
      Undo.RecordObject((Object) target1, "Move Target");
      target1.target = (Vector2) target2;
    }
  }
}
