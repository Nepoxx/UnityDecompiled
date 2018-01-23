// Decompiled with JetBrains decompiler
// Type: UnityEditor.ProjectWindowCallback.DoCreateScene
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEditor.SceneManagement;

namespace UnityEditor.ProjectWindowCallback
{
  internal class DoCreateScene : EndNameEditAction
  {
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
      bool createDefaultGameObjects = true;
      if (!EditorSceneManager.CreateSceneAsset(pathName, createDefaultGameObjects))
        return;
      ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath(pathName, typeof (SceneAsset)));
    }
  }
}
