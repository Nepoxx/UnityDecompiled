// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.TouchScreenTextEditor
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  internal class TouchScreenTextEditor : TextEditor
  {
    private string m_SecureText;

    public TouchScreenTextEditor(TextField textField)
      : base(textField)
    {
      this.secureText = string.Empty;
    }

    public string secureText
    {
      get
      {
        return this.m_SecureText;
      }
      set
      {
        string str = value ?? string.Empty;
        if (!(str != this.m_SecureText))
          return;
        this.m_SecureText = str;
      }
    }

    protected override void RegisterCallbacksOnTarget()
    {
      this.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseUpDownEvent), Capture.NoCapture);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
      this.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseUpDownEvent), Capture.NoCapture);
    }

    private void OnMouseUpDownEvent(MouseDownEvent evt)
    {
      this.SyncTextEditor();
      this.textField.TakeCapture();
      this.keyboardOnScreen = TouchScreenKeyboard.Open(string.IsNullOrEmpty(this.secureText) ? this.textField.text : this.secureText, TouchScreenKeyboardType.Default, true, this.multiline, !string.IsNullOrEmpty(this.secureText));
      this.UpdateScrollOffset();
      evt.StopPropagation();
    }
  }
}
