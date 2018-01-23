// Decompiled with JetBrains decompiler
// Type: UnityEditor.TransformTool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class TransformTool : ManipulationTool
  {
    private static TransformTool s_Instance;
    private static Vector3 s_Scale;

    public static void OnGUI(SceneView view)
    {
      if (TransformTool.s_Instance == null)
        TransformTool.s_Instance = new TransformTool();
      TransformTool.s_Instance.OnToolGUI(view);
    }

    public override void ToolGUI(SceneView view, Vector3 handlePosition, bool isStatic)
    {
      Handles.TransformHandleIds ids = Handles.TransformHandleIds.Default;
      TransformManipulator.BeginManipulationHandling(false);
      if (ids.scale.Has(GUIUtility.hotControl) || ids.rotation.Has(GUIUtility.hotControl))
        Tools.LockHandlePosition();
      else
        Tools.UnlockHandlePosition();
      EditorGUI.BeginChangeCheck();
      if (Event.current.type == EventType.MouseDown)
        TransformTool.s_Scale = Vector3.one;
      Vector3 position = handlePosition;
      Quaternion handleRotation = Tools.handleRotation;
      Quaternion rotation = handleRotation;
      Vector3 scale1 = TransformTool.s_Scale;
      Vector3 scale2 = scale1;
      Handles.TransformHandle(ids, ref position, ref rotation, ref scale2, Handles.TransformHandleParam.Default);
      TransformTool.s_Scale = scale2;
      if (EditorGUI.EndChangeCheck() && !isStatic)
      {
        Undo.RecordObjects((Object[]) Selection.transforms, "Transform Manipulation");
        Vector3 positionDelta = position - TransformManipulator.mouseDownHandlePosition;
        if (positionDelta != Vector3.zero)
        {
          ManipulationToolUtility.SetMinDragDifferenceForPos(handlePosition);
          TransformManipulator.SetPositionDelta(positionDelta);
        }
        float angle;
        Vector3 axis;
        (Quaternion.Inverse(handleRotation) * rotation).ToAngleAxis(out angle, out axis);
        if (!Mathf.Approximately(angle, 0.0f))
        {
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
          Tools.handleRotation = rotation;
        }
        if (scale2 != scale1)
          TransformManipulator.SetScaleDelta(scale2, rotation);
      }
      int num = (int) TransformManipulator.EndManipulationHandling();
    }
  }
}
