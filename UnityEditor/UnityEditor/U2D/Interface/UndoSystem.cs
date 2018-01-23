// Decompiled with JetBrains decompiler
// Type: UnityEditor.U2D.Interface.UndoSystem
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine;

namespace UnityEditor.U2D.Interface
{
  internal class UndoSystem : IUndoSystem
  {
    public void RegisterUndoCallback(Undo.UndoRedoCallback undoCallback)
    {
      Undo.undoRedoPerformed += undoCallback;
    }

    public void UnregisterUndoCallback(Undo.UndoRedoCallback undoCallback)
    {
      Undo.undoRedoPerformed -= undoCallback;
    }

    public void RegisterCompleteObjectUndo(IUndoableObject obj, string undoText)
    {
      ScriptableObject scriptableObject = this.CheckUndoObjectType(obj);
      if (!((Object) scriptableObject != (Object) null))
        return;
      Undo.RegisterCompleteObjectUndo((Object) scriptableObject, undoText);
    }

    private ScriptableObject CheckUndoObjectType(IUndoableObject obj)
    {
      ScriptableObject scriptableObject = obj as ScriptableObject;
      if ((Object) scriptableObject == (Object) null)
        Debug.LogError((object) "Register Undo object is not a ScriptableObject");
      return scriptableObject;
    }

    public void ClearUndo(IUndoableObject obj)
    {
      ScriptableObject scriptableObject = this.CheckUndoObjectType(obj);
      if (!((Object) scriptableObject != (Object) null))
        return;
      Undo.ClearUndo((Object) scriptableObject);
    }
  }
}
