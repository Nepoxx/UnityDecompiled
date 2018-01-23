// Decompiled with JetBrains decompiler
// Type: UnityEditor.Build.IPostprocessBuild
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Build
{
  public interface IPostprocessBuild : IOrderedCallback
  {
    /// <summary>
    ///   <para>Implement this function to receive a callback after the build is complete.</para>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="path"></param>
    void OnPostprocessBuild(BuildTarget target, string path);
  }
}
