// Decompiled with JetBrains decompiler
// Type: UnityEditor.DrivenRectTransformUndo
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor
{
  [InitializeOnLoad]
  internal class DrivenRectTransformUndo
  {
    static DrivenRectTransformUndo()
    {
      Undo.WillFlushUndoRecord willFlushUndoRecord = Undo.willFlushUndoRecord;
      // ISSUE: reference to a compiler-generated field
      if (DrivenRectTransformUndo.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        DrivenRectTransformUndo.\u003C\u003Ef__mg\u0024cache0 = new Undo.WillFlushUndoRecord(DrivenRectTransformUndo.ForceUpdateCanvases);
      }
      // ISSUE: reference to a compiler-generated field
      Undo.WillFlushUndoRecord fMgCache0 = DrivenRectTransformUndo.\u003C\u003Ef__mg\u0024cache0;
      Undo.willFlushUndoRecord = willFlushUndoRecord + fMgCache0;
      Undo.UndoRedoCallback undoRedoPerformed = Undo.undoRedoPerformed;
      // ISSUE: reference to a compiler-generated field
      if (DrivenRectTransformUndo.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        DrivenRectTransformUndo.\u003C\u003Ef__mg\u0024cache1 = new Undo.UndoRedoCallback(DrivenRectTransformUndo.ForceUpdateCanvases);
      }
      // ISSUE: reference to a compiler-generated field
      Undo.UndoRedoCallback fMgCache1 = DrivenRectTransformUndo.\u003C\u003Ef__mg\u0024cache1;
      Undo.undoRedoPerformed = undoRedoPerformed + fMgCache1;
    }

    private static void ForceUpdateCanvases()
    {
      Canvas.ForceUpdateCanvases();
    }
  }
}
