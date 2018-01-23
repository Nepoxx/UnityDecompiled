// Decompiled with JetBrains decompiler
// Type: UnityEditor.ManipulationTool
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  internal abstract class ManipulationTool
  {
    protected virtual void OnToolGUI(SceneView view)
    {
      if (!(bool) ((Object) Selection.activeTransform) || Tools.s_Hidden)
        return;
      bool flag = !Tools.s_Hidden && EditorApplication.isPlaying && GameObjectUtility.ContainsStatic(Selection.gameObjects);
      using (new EditorGUI.DisabledScope(flag))
      {
        Vector3 handlePosition = Tools.handlePosition;
        this.ToolGUI(view, handlePosition, flag);
        Handles.ShowStaticLabelIfNeeded(handlePosition);
      }
    }

    public abstract void ToolGUI(SceneView view, Vector3 handlePosition, bool isStatic);
  }
}
