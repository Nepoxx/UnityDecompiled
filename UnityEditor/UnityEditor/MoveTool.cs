// Decompiled with JetBrains decompiler
// Type: UnityEditor.MoveTool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class MoveTool : ManipulationTool
  {
    private static MoveTool s_Instance;

    public static void OnGUI(SceneView view)
    {
      if (MoveTool.s_Instance == null)
        MoveTool.s_Instance = new MoveTool();
      MoveTool.s_Instance.OnToolGUI(view);
    }

    public override void ToolGUI(SceneView view, Vector3 handlePosition, bool isStatic)
    {
      TransformManipulator.BeginManipulationHandling(false);
      EditorGUI.BeginChangeCheck();
      Vector3 vector3 = Handles.PositionHandle(handlePosition, Tools.handleRotation);
      if (EditorGUI.EndChangeCheck() && !isStatic)
      {
        Vector3 positionDelta = vector3 - TransformManipulator.mouseDownHandlePosition;
        ManipulationToolUtility.SetMinDragDifferenceForPos(handlePosition);
        if (Tools.vertexDragging)
          ManipulationToolUtility.DisableMinDragDifference();
        TransformManipulator.SetPositionDelta(positionDelta);
      }
      int num = (int) TransformManipulator.EndManipulationHandling();
    }
  }
}
