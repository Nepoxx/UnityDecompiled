// Decompiled with JetBrains decompiler
// Type: UnityEditor.UndoPropertyModification
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Scripting;

namespace UnityEditor
{
  /// <summary>
  ///   <para>See Also: Undo.postprocessModifications.</para>
  /// </summary>
  [RequiredByNativeCode]
  public struct UndoPropertyModification
  {
    public PropertyModification previousValue;
    public PropertyModification currentValue;
    private int m_KeepPrefabOverride;

    public bool keepPrefabOverride
    {
      get
      {
        return this.m_KeepPrefabOverride != 0;
      }
      set
      {
        this.m_KeepPrefabOverride = !value ? 0 : 1;
      }
    }
  }
}
