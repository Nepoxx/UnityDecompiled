// Decompiled with JetBrains decompiler
// Type: UnityEditor.RotateTool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class RotateTool : ManipulationTool
  {
    private static RotateTool s_Instance;

    public static void OnGUI(SceneView view)
    {
      if (RotateTool.s_Instance == null)
        RotateTool.s_Instance = new RotateTool();
      RotateTool.s_Instance.OnToolGUI(view);
    }

    public override void ToolGUI(SceneView view, Vector3 handlePosition, bool isStatic)
    {
      Quaternion handleRotation = Tools.handleRotation;
      EditorGUI.BeginChangeCheck();
      Quaternion quaternion = Handles.RotationHandle(handleRotation, handlePosition);
      if (!EditorGUI.EndChangeCheck() || isStatic)
        return;
      float angle;
      Vector3 axis;
      (Quaternion.Inverse(handleRotation) * quaternion).ToAngleAxis(out angle, out axis);
      Undo.RecordObjects((Object[]) Selection.transforms, "Rotate");
      foreach (Transform transform in Selection.transforms)
      {
        if (Tools.pivotMode == PivotMode.Center)
          transform.RotateAround(handlePosition, handleRotation * axis, angle);
        else if (TransformManipulator.individualSpace)
          transform.Rotate(transform.rotation * axis, angle, Space.World);
        else
          transform.Rotate(handleRotation * axis, angle, Space.World);
        transform.SetLocalEulerHint(transform.GetLocalEulerAngles(transform.rotationOrder));
        if ((Object) transform.parent != (Object) null)
          transform.SendTransformChangedScale();
      }
      Tools.handleRotation = quaternion;
    }
  }
}
