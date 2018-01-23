// Decompiled with JetBrains decompiler
// Type: UnityEditor.Animations.PushUndoIfNeeded
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.Animations
{
  internal struct PushUndoIfNeeded
  {
    private PushUndoIfNeeded.PushUndoIfNeededImpl m_Impl;

    public PushUndoIfNeeded(bool pushUndo)
    {
      this.m_Impl = new PushUndoIfNeeded.PushUndoIfNeededImpl(pushUndo);
    }

    public bool pushUndo
    {
      get
      {
        return this.impl.m_PushUndo;
      }
      set
      {
        this.impl.m_PushUndo = value;
      }
    }

    public void DoUndo(Object target, string undoOperation)
    {
      this.impl.DoUndo(target, undoOperation);
    }

    private PushUndoIfNeeded.PushUndoIfNeededImpl impl
    {
      get
      {
        if (this.m_Impl == null)
          this.m_Impl = new PushUndoIfNeeded.PushUndoIfNeededImpl(true);
        return this.m_Impl;
      }
    }

    private class PushUndoIfNeededImpl
    {
      public bool m_PushUndo;

      public PushUndoIfNeededImpl(bool pushUndo)
      {
        this.m_PushUndo = pushUndo;
      }

      public void DoUndo(Object target, string undoOperation)
      {
        if (!this.m_PushUndo)
          return;
        Undo.RegisterCompleteObjectUndo(target, undoOperation);
      }
    }
  }
}
