// Decompiled with JetBrains decompiler
// Type: UnityEditor.ScaleTool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal class ScaleTool : ManipulationTool
  {
    private static Vector3 s_CurrentScale = Vector3.one;
    private static ScaleTool s_Instance;

    public static void OnGUI(SceneView view)
    {
      if (ScaleTool.s_Instance == null)
        ScaleTool.s_Instance = new ScaleTool();
      ScaleTool.s_Instance.OnToolGUI(view);
    }

    public override void ToolGUI(SceneView view, Vector3 handlePosition, bool isStatic)
    {
      Quaternion quaternion = Selection.transforms.Length <= 1 ? Tools.handleLocalRotation : Tools.handleRotation;
      TransformManipulator.DebugAlignment(quaternion);
      if (Event.current.type == EventType.MouseDown)
        ScaleTool.s_CurrentScale = Vector3.one;
      EditorGUI.BeginChangeCheck();
      TransformManipulator.BeginManipulationHandling(true);
      ScaleTool.s_CurrentScale = Handles.ScaleHandle(ScaleTool.s_CurrentScale, handlePosition, quaternion, HandleUtility.GetHandleSize(handlePosition));
      int num = (int) TransformManipulator.EndManipulationHandling();
      if (!EditorGUI.EndChangeCheck() || isStatic)
        return;
      TransformManipulator.SetScaleDelta(ScaleTool.s_CurrentScale, quaternion);
    }
  }
}
