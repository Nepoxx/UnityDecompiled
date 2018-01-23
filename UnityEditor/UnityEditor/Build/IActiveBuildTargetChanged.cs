// Decompiled with JetBrains decompiler
// Type: UnityEditor.Build.IActiveBuildTargetChanged
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Build
{
  public interface IActiveBuildTargetChanged : IOrderedCallback
  {
    /// <summary>
    ///   <para>This function is called automatically when the active build platform has changed.</para>
    /// </summary>
    /// <param name="previousTarget">The build target before the change.</param>
    /// <param name="newTarget">The new active build target.</param>
    void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget);
  }
}
