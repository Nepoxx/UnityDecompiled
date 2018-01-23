// Decompiled with JetBrains decompiler
// Type: UnityEditor.UnityBuildPostprocessor
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.Build;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
  internal class UnityBuildPostprocessor : IProcessScene, IOrderedCallback
  {
    public int callbackOrder
    {
      get
      {
        return 0;
      }
    }

    public void OnProcessScene(Scene scene)
    {
      int staticBatching;
      int dynamicBatching;
      PlayerSettings.GetBatchingForPlatform(EditorUserBuildSettings.activeBuildTarget, out staticBatching, out dynamicBatching);
      if (staticBatching == 0)
        return;
      InternalStaticBatchingUtility.Combine((GameObject) null, true, true);
    }
  }
}
