// Decompiled with JetBrains decompiler
// Type: UnityEditor.SelectionMode
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

namespace UnityEditor
{
  /// <summary>
  ///   <para>SelectionMode can be used to tweak the selection returned by Selection.GetTransforms.</para>
  /// </summary>
  public enum SelectionMode
  {
    Unfiltered = 0,
    TopLevel = 1,
    Deep = 2,
    ExcludePrefab = 4,
    Editable = 8,
    OnlyUserModifiable = 8,
    Assets = 16, // 0x00000010
    DeepAssets = 32, // 0x00000020
  }
}
