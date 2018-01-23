// Decompiled with JetBrains decompiler
// Type: UnityEditor.Build.IOrderedCallback
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor.Build
{
  public interface IOrderedCallback
  {
    /// <summary>
    ///   <para>Returns the relative callback order for callbacks.  Callbacks with lower values are called before ones with higher values.</para>
    /// </summary>
    int callbackOrder { get; }
  }
}
