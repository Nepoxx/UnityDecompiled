// Decompiled with JetBrains decompiler
// Type: UnityEditor.AssetDeleteResult
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>Result of Asset delete operation</para>
  /// </summary>
  [System.Flags]
  public enum AssetDeleteResult
  {
    DidNotDelete = 0,
    FailedDelete = 1,
    DidDelete = 2,
  }
}
