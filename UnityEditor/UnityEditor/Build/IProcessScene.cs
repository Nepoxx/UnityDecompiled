// Decompiled with JetBrains decompiler
// Type: UnityEditor.Build.IProcessScene
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.SceneManagement;

namespace UnityEditor.Build
{
  public interface IProcessScene : IOrderedCallback
  {
    /// <summary>
    ///   <para>Implement this function to receive a callback for each Scene during the build.</para>
    /// </summary>
    /// <param name="scene">The current Scene being processed.</param>
    void OnProcessScene(Scene scene);
  }
}
