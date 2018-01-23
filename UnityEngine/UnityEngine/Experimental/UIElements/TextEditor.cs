// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.TextEditor
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  public class TextEditor : UnityEngine.TextEditor, IManipulator
  {
    private VisualElement m_Target;

    protected TextEditor(TextField textField)
    {
      this.textField = textField;
      this.SyncTextEditor();
    }

    public int maxLength { get; set; }

    public char maskChar { get; set; }

    public bool doubleClickSelectsWord { get; set; }

    public bool tripleClickSelectsLine { get; set; }

    protected TextField textField { get; set; }

    internal override Rect localPosition
    {
      get
      {
        return new Rect(0.0f, 0.0f, this.position.width, this.position.height);
      }
    }

    protected virtual void RegisterCallbacksOnTarget()
    {
      this.target.RegisterCallback<FocusEvent>(new EventCallback<FocusEvent>(this.OnFocus), Capture.NoCapture);
      this.target.RegisterCallback<BlurEvent>(new EventCallback<BlurEvent>(this.OnBlur), Capture.NoCapture);
    }

    protected virtual void UnregisterCallbacksFromTarget()
    {
      this.target.UnregisterCallback<FocusEvent>(new EventCallback<FocusEvent>(this.OnFocus), Capture.NoCapture);
      this.target.UnregisterCallback<BlurEvent>(new EventCallback<BlurEvent>(this.OnBlur), Capture.NoCapture);
    }

    private void OnFocus(FocusEvent evt)
    {
      this.OnFocus();
    }

    private void OnBlur(BlurEvent evt)
    {
      this.OnLostFocus();
    }

    public VisualElement target
    {
      get
      {
        return this.m_Target;
      }
      set
      {
        if (this.target != null)
          this.UnregisterCallbacksFromTarget();
        this.m_Target = value;
        if (this.target == null)
          return;
        this.RegisterCallbacksOnTarget();
      }
    }

    protected void SyncTextEditor()
    {
      string str = this.textField.text;
      if (this.maxLength >= 0 && str != null && str.Length > this.maxLength)
        str = str.Substring(0, this.maxLength);
      this.text = str;
      this.SaveBackup();
      this.position = this.textField.layout;
      this.maxLength = this.textField.maxLength;
      this.multiline = this.textField.multiline;
      this.isPasswordField = this.textField.isPasswordField;
      this.maskChar = this.textField.maskChar;
      this.doubleClickSelectsWord = this.textField.doubleClickSelectsWord;
      this.tripleClickSelectsLine = this.textField.tripleClickSelectsLine;
      this.DetectFocusChange();
    }

    internal override void OnDetectFocusChange()
    {
      if (this.m_HasFocus && !this.textField.hasFocus)
        this.OnFocus();
      if (this.m_HasFocus || !this.textField.hasFocus)
        return;
      this.OnLostFocus();
    }
  }
}
