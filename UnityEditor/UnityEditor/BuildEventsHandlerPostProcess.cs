// Decompiled with JetBrains decompiler
// Type: UnityEditor.BuildEventsHandlerPostProcess
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Build;
using UnityEngine;

namespace UnityEditor
{
  internal class BuildEventsHandlerPostProcess : IPostprocessBuild, IOrderedCallback
  {
    private static bool s_EventSent = false;
    private static int s_NumOfSceneViews = 0;
    private static int s_NumOf2dSceneViews = 0;

    public int callbackOrder
    {
      get
      {
        return 0;
      }
    }

    public void OnPostprocessBuild(BuildTarget target, string path)
    {
      Object[] objectsOfTypeAll = UnityEngine.Resources.FindObjectsOfTypeAll(typeof (SceneView));
      int num = 0;
      foreach (SceneView sceneView in objectsOfTypeAll)
      {
        if (sceneView.in2DMode)
          ++num;
      }
      if (BuildEventsHandlerPostProcess.s_NumOfSceneViews == objectsOfTypeAll.Length && BuildEventsHandlerPostProcess.s_NumOf2dSceneViews == num && BuildEventsHandlerPostProcess.s_EventSent)
        return;
      BuildEventsHandlerPostProcess.s_EventSent = true;
      BuildEventsHandlerPostProcess.s_NumOfSceneViews = objectsOfTypeAll.Length;
      BuildEventsHandlerPostProcess.s_NumOf2dSceneViews = num;
      EditorAnalytics.SendEventSceneViewInfo((object) new SceneViewInfo()
      {
        total_scene_views = BuildEventsHandlerPostProcess.s_NumOfSceneViews,
        num_of_2d_views = BuildEventsHandlerPostProcess.s_NumOf2dSceneViews,
        is_default_2d_mode = (EditorSettings.defaultBehaviorMode == EditorBehaviorMode.Mode2D)
      });
    }
  }
}
