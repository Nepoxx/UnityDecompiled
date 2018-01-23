// Decompiled with JetBrains decompiler
// Type: UnityEditorInternal.AnimationWindowKeySelection
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
  [Serializable]
  internal class AnimationWindowKeySelection : ScriptableObject, ISerializationCallbackReceiver
  {
    private HashSet<int> m_SelectedKeyHashes;
    [SerializeField]
    private List<int> m_SelectedKeyHashesSerialized;

    public HashSet<int> selectedKeyHashes
    {
      get
      {
        return this.m_SelectedKeyHashes ?? (this.m_SelectedKeyHashes = new HashSet<int>());
      }
      set
      {
        this.m_SelectedKeyHashes = value;
      }
    }

    public void SaveSelection(string undoLabel)
    {
      Undo.RegisterCompleteObjectUndo((UnityEngine.Object) this, undoLabel);
    }

    public void OnBeforeSerialize()
    {
      this.m_SelectedKeyHashesSerialized = this.m_SelectedKeyHashes.ToList<int>();
    }

    public void OnAfterDeserialize()
    {
      this.m_SelectedKeyHashes = new HashSet<int>((IEnumerable<int>) this.m_SelectedKeyHashesSerialized);
    }
  }
}
