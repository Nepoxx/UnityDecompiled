// Decompiled with JetBrains decompiler
// Type: UnityEditor.Experimental.UIElements.EditorTextField
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 53BAA40C-AA1D-48D3-AA10-3FCF36D212BC
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEditor.dll

using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
  internal class EditorTextField : TextField
  {
    protected EditorTextField(int maxLength, bool multiline, bool isPasswordField, char maskChar)
      : base(maxLength, multiline, isPasswordField, maskChar)
    {
      ContextualMenu contextualMenu = new ContextualMenu();
      contextualMenu.AddAction("Cut", new GenericMenu.MenuFunction(this.Cut), new ContextualMenu.ActionStatusCallback(this.CutCopyActionStatus));
      contextualMenu.AddAction("Copy", new GenericMenu.MenuFunction(this.Copy), new ContextualMenu.ActionStatusCallback(this.CutCopyActionStatus));
      contextualMenu.AddAction("Paste", new GenericMenu.MenuFunction(this.Paste), new ContextualMenu.ActionStatusCallback(this.PasteActionStatus));
      this.AddManipulator((IManipulator) contextualMenu);
    }

    private ContextualMenu.ActionStatus CutCopyActionStatus()
    {
      return !this.editor.hasSelection || this.isPasswordField ? ContextualMenu.ActionStatus.Disabled : ContextualMenu.ActionStatus.Enabled;
    }

    private ContextualMenu.ActionStatus PasteActionStatus()
    {
      return !this.editor.CanPaste() ? ContextualMenu.ActionStatus.Off : ContextualMenu.ActionStatus.Enabled;
    }

    private void Cut()
    {
      this.editor.Cut();
    }

    private void Copy()
    {
      this.editor.Copy();
    }

    private void Paste()
    {
      this.editor.Paste();
    }
  }
}
