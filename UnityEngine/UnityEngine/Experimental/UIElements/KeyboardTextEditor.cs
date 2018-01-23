// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.KeyboardTextEditor
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;

namespace UnityEngine.Experimental.UIElements
{
  internal class KeyboardTextEditor : TextEditor
  {
    private bool m_DragToPosition = true;
    private bool m_SelectAllOnMouseUp = true;
    internal bool m_Changed;
    private bool m_Dragged;
    private bool m_PostPoneMove;
    private string m_PreDrawCursorText;

    public KeyboardTextEditor(TextField textField)
      : base(textField)
    {
    }

    protected override void RegisterCallbacksOnTarget()
    {
      base.RegisterCallbacksOnTarget();
      this.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
      this.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
      this.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
      this.target.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), Capture.NoCapture);
      this.target.RegisterCallback<IMGUIEvent>(new EventCallback<IMGUIEvent>(this.OnIMGUIEvent), Capture.NoCapture);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
      base.UnregisterCallbacksFromTarget();
      this.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
      this.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
      this.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
      this.target.UnregisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), Capture.NoCapture);
      this.target.UnregisterCallback<IMGUIEvent>(new EventCallback<IMGUIEvent>(this.OnIMGUIEvent), Capture.NoCapture);
    }

    private void OnMouseDown(MouseDownEvent evt)
    {
      this.SyncTextEditor();
      this.m_Changed = false;
      this.target.TakeCapture();
      if (!this.m_HasFocus)
      {
        this.m_HasFocus = true;
        this.MoveCursorToPosition_Internal(evt.localMousePosition, evt.shiftKey);
        evt.StopPropagation();
      }
      else
      {
        if (evt.clickCount == 2 && this.doubleClickSelectsWord)
        {
          this.SelectCurrentWord();
          this.DblClickSnap(UnityEngine.TextEditor.DblClickSnapping.WORDS);
          this.MouseDragSelectsWholeWords(true);
          this.m_DragToPosition = false;
        }
        else if (evt.clickCount == 3 && this.tripleClickSelectsLine)
        {
          this.SelectCurrentParagraph();
          this.MouseDragSelectsWholeWords(true);
          this.DblClickSnap(UnityEngine.TextEditor.DblClickSnapping.PARAGRAPHS);
          this.m_DragToPosition = false;
        }
        else
        {
          this.MoveCursorToPosition_Internal(evt.localMousePosition, evt.shiftKey);
          this.m_SelectAllOnMouseUp = false;
        }
        evt.StopPropagation();
      }
      if (this.m_Changed)
      {
        if (this.maxLength >= 0 && this.text != null && this.text.Length > this.maxLength)
          this.text = this.text.Substring(0, this.maxLength);
        this.textField.text = this.text;
        this.textField.TextFieldChanged();
        evt.StopPropagation();
      }
      this.UpdateScrollOffset();
    }

    private void OnMouseUp(MouseUpEvent evt)
    {
      if (!this.target.HasCapture())
        return;
      this.SyncTextEditor();
      this.m_Changed = false;
      if (this.m_Dragged && this.m_DragToPosition)
        this.MoveSelectionToAltCursor();
      else if (this.m_PostPoneMove)
        this.MoveCursorToPosition_Internal(evt.localMousePosition, evt.shiftKey);
      else if (this.m_SelectAllOnMouseUp)
        this.m_SelectAllOnMouseUp = false;
      this.MouseDragSelectsWholeWords(false);
      this.target.ReleaseCapture();
      this.m_DragToPosition = true;
      this.m_Dragged = false;
      this.m_PostPoneMove = false;
      evt.StopPropagation();
      if (this.m_Changed)
      {
        if (this.maxLength >= 0 && this.text != null && this.text.Length > this.maxLength)
          this.text = this.text.Substring(0, this.maxLength);
        this.textField.text = this.text;
        this.textField.TextFieldChanged();
        evt.StopPropagation();
      }
      this.UpdateScrollOffset();
    }

    private void OnMouseMove(MouseMoveEvent evt)
    {
      if (!this.target.HasCapture())
        return;
      this.SyncTextEditor();
      this.m_Changed = false;
      if (!evt.shiftKey && this.hasSelection && this.m_DragToPosition)
      {
        this.MoveAltCursorToPosition(evt.localMousePosition);
      }
      else
      {
        if (evt.shiftKey)
          this.MoveCursorToPosition_Internal(evt.localMousePosition, evt.shiftKey);
        else
          this.SelectToPosition(evt.localMousePosition);
        this.m_DragToPosition = false;
        this.m_SelectAllOnMouseUp = !this.hasSelection;
      }
      this.m_Dragged = true;
      evt.StopPropagation();
      if (this.m_Changed)
      {
        if (this.maxLength >= 0 && this.text != null && this.text.Length > this.maxLength)
          this.text = this.text.Substring(0, this.maxLength);
        this.textField.text = this.text;
        this.textField.TextFieldChanged();
        evt.StopPropagation();
      }
      this.UpdateScrollOffset();
    }

    private void OnKeyDown(KeyDownEvent evt)
    {
      if (!this.textField.hasFocus)
        return;
      this.SyncTextEditor();
      this.m_Changed = false;
      if (this.HandleKeyEvent(evt.imguiEvent))
      {
        this.m_Changed = true;
        this.textField.text = this.text;
        evt.StopPropagation();
      }
      else
      {
        if (evt.keyCode == KeyCode.Tab || (int) evt.character == 9)
          return;
        char character = evt.character;
        if ((int) character == 10 && !this.multiline && !evt.altKey)
        {
          this.textField.TextFieldChangeValidated();
          return;
        }
        Font font = this.textField.editor.style.font;
        if ((UnityEngine.Object) font != (UnityEngine.Object) null && font.HasCharacter(character) || (int) character == 10)
        {
          this.Insert(character);
          this.m_Changed = true;
        }
        else if ((int) character == 0)
        {
          if (!string.IsNullOrEmpty(Input.compositionString))
          {
            this.ReplaceSelection("");
            this.m_Changed = true;
          }
          evt.StopPropagation();
        }
      }
      if (this.m_Changed)
      {
        if (this.maxLength >= 0 && this.text != null && this.text.Length > this.maxLength)
          this.text = this.text.Substring(0, this.maxLength);
        this.textField.text = this.text;
        this.textField.TextFieldChanged();
        evt.StopPropagation();
      }
      this.UpdateScrollOffset();
    }

    private void OnIMGUIEvent(IMGUIEvent evt)
    {
      if (!this.textField.hasFocus)
        return;
      this.SyncTextEditor();
      this.m_Changed = false;
      switch (evt.imguiEvent.type)
      {
        case EventType.ValidateCommand:
          switch (evt.imguiEvent.commandName)
          {
            case "Cut":
            case "Copy":
              if (!this.hasSelection)
                return;
              break;
            case "Paste":
              if (!this.CanPaste())
                return;
              break;
          }
          evt.StopPropagation();
          break;
        case EventType.ExecuteCommand:
          bool flag = false;
          string text = this.text;
          if (!this.textField.hasFocus)
            return;
          switch (evt.imguiEvent.commandName)
          {
            case "OnLostFocus":
              evt.StopPropagation();
              return;
            case "Cut":
              this.Cut();
              flag = true;
              break;
            case "Copy":
              this.Copy();
              evt.StopPropagation();
              return;
            case "Paste":
              this.Paste();
              flag = true;
              break;
            case "SelectAll":
              this.SelectAll();
              evt.StopPropagation();
              return;
            case "Delete":
              if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX)
                this.Delete();
              else
                this.Cut();
              flag = true;
              break;
          }
          if (flag)
          {
            if (text != this.text)
              this.m_Changed = true;
            evt.StopPropagation();
            break;
          }
          break;
      }
      if (this.m_Changed)
      {
        if (this.maxLength >= 0 && this.text != null && this.text.Length > this.maxLength)
          this.text = this.text.Substring(0, this.maxLength);
        this.textField.text = this.text;
        this.textField.TextFieldChanged();
        evt.StopPropagation();
      }
      this.UpdateScrollOffset();
    }

    public void PreDrawCursor(string newText)
    {
      this.SyncTextEditor();
      this.m_PreDrawCursorText = this.text;
      int num = this.cursorIndex;
      if (!string.IsNullOrEmpty(Input.compositionString))
      {
        this.text = newText.Substring(0, this.cursorIndex) + Input.compositionString + newText.Substring(this.selectIndex);
        num += Input.compositionString.Length;
      }
      else
        this.text = newText;
      if (this.maxLength >= 0 && this.text != null && this.text.Length > this.maxLength)
      {
        this.text = this.text.Substring(0, this.maxLength);
        num = Math.Min(num, this.maxLength - 1);
      }
      this.graphicalCursorPos = this.style.GetCursorPixelPosition(this.localPosition, new GUIContent(this.text), num);
    }

    public void PostDrawCursor()
    {
      this.text = this.m_PreDrawCursorText;
    }
  }
}
