// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.TextField
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using System;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
  /// <summary>
  ///   <para>A textfield is a rectangular area where the user can edit a string.</para>
  /// </summary>
  public class TextField : VisualContainer
  {
    /// <summary>
    ///   <para>Action that is called whenever the text changes in the textfield.</para>
    /// </summary>
    public Action<string> OnTextChanged;
    /// <summary>
    ///   <para>Action that is called when the user validates the text in the textfield.</para>
    /// </summary>
    public Action OnTextChangeValidated;
    private const string SelectionColorProperty = "selection-color";
    private const string CursorColorProperty = "cursor-color";
    private StyleValue<Color> m_SelectionColor;
    private StyleValue<Color> m_CursorColor;
    private bool m_Multiline;
    private bool m_IsPasswordField;
    internal const int kMaxLengthNone = -1;

    /// <summary>
    ///   <para>Creates a new textfield.</para>
    /// </summary>
    /// <param name="maxLength">The maximum number of characters this textfield can hold. If 0, there is no limit.</param>
    /// <param name="multiline">Set this to true to allow multiple lines in the textfield and false if otherwise.</param>
    /// <param name="isPasswordField">Set this to true to mask the characters and false if otherwise.</param>
    /// <param name="maskChar">The character used for masking in a password field.</param>
    public TextField()
      : this(-1, false, false, char.MinValue)
    {
    }

    /// <summary>
    ///   <para>Creates a new textfield.</para>
    /// </summary>
    /// <param name="maxLength">The maximum number of characters this textfield can hold. If 0, there is no limit.</param>
    /// <param name="multiline">Set this to true to allow multiple lines in the textfield and false if otherwise.</param>
    /// <param name="isPasswordField">Set this to true to mask the characters and false if otherwise.</param>
    /// <param name="maskChar">The character used for masking in a password field.</param>
    public TextField(int maxLength, bool multiline, bool isPasswordField, char maskChar)
    {
      this.maxLength = maxLength;
      this.multiline = multiline;
      this.isPasswordField = isPasswordField;
      this.maskChar = maskChar;
      if (this.touchScreenTextField)
      {
        this.editor = (TextEditor) new TouchScreenTextEditor(this);
      }
      else
      {
        this.doubleClickSelectsWord = true;
        this.tripleClickSelectsLine = true;
        this.editor = (TextEditor) new KeyboardTextEditor(this);
      }
      this.editor.style = new GUIStyle(this.editor.style);
      this.focusIndex = 0;
      this.AddManipulator((IManipulator) this.editor);
    }

    /// <summary>
    ///   <para>The color of the text selection.</para>
    /// </summary>
    public Color selectionColor
    {
      get
      {
        return this.m_SelectionColor.GetSpecifiedValueOrDefault(Color.clear);
      }
    }

    /// <summary>
    ///   <para>The color of the cursor.</para>
    /// </summary>
    public Color cursorColor
    {
      get
      {
        return this.m_CursorColor.GetSpecifiedValueOrDefault(Color.clear);
      }
    }

    /// <summary>
    ///   <para>Set this to true to allow multiple lines in the textfield and false if otherwise.</para>
    /// </summary>
    public bool multiline
    {
      get
      {
        return this.m_Multiline;
      }
      set
      {
        this.m_Multiline = value;
        if (value)
          return;
        this.text = this.text.Replace("\n", "");
      }
    }

    /// <summary>
    ///   <para>Set this to true to mask the characters and false if otherwise.</para>
    /// </summary>
    public bool isPasswordField
    {
      get
      {
        return this.m_IsPasswordField;
      }
      set
      {
        this.m_IsPasswordField = value;
        if (!value)
          return;
        this.multiline = false;
      }
    }

    /// <summary>
    ///   <para>The character used for masking in a password field.</para>
    /// </summary>
    public char maskChar { get; set; }

    /// <summary>
    ///   <para>Set this to true to allow double-clicks to select the word under the mouse and false if otherwise.</para>
    /// </summary>
    public bool doubleClickSelectsWord { get; set; }

    /// <summary>
    ///   <para>Set this to true to allow triple-clicks to select the line under the mouse and false if otherwise.</para>
    /// </summary>
    public bool tripleClickSelectsLine { get; set; }

    /// <summary>
    ///   <para>The maximum number of characters this textfield can hold. If 0, there is no limit.</para>
    /// </summary>
    public int maxLength { get; set; }

    private bool touchScreenTextField
    {
      get
      {
        return TouchScreenKeyboard.isSupported;
      }
    }

    /// <summary>
    ///   <para>Returns true if the textfield has the focus and false if otherwise.</para>
    /// </summary>
    public bool hasFocus
    {
      get
      {
        return this.elementPanel != null && this.elementPanel.focusController.focusedElement == this;
      }
    }

    internal TextEditor editor { get; set; }

    internal void TextFieldChanged()
    {
      if (this.OnTextChanged == null)
        return;
      this.OnTextChanged(this.text);
    }

    internal void TextFieldChangeValidated()
    {
      if (this.OnTextChangeValidated == null)
        return;
      this.OnTextChangeValidated();
    }

    /// <summary>
    ///   <para>Called when the persistent data is accessible and/or when the data or persistence key have changed (VisualElement is properly parented).</para>
    /// </summary>
    public override void OnPersistentDataReady()
    {
      base.OnPersistentDataReady();
      this.OverwriteFromPersistedData((object) this, this.GetFullHierarchicalPersistenceKey());
    }

    public override void OnStyleResolved(ICustomStyle style)
    {
      base.OnStyleResolved(style);
      this.effectiveStyle.ApplyCustomProperty("selection-color", ref this.m_SelectionColor);
      this.effectiveStyle.ApplyCustomProperty("cursor-color", ref this.m_CursorColor);
      this.effectiveStyle.WriteToGUIStyle(this.editor.style);
    }

    internal override void DoRepaint(IStylePainter painter)
    {
      if (this.touchScreenTextField)
      {
        TouchScreenTextEditor editor = this.editor as TouchScreenTextEditor;
        if (editor != null && editor.keyboardOnScreen != null)
        {
          this.text = editor.keyboardOnScreen.text;
          if (this.editor.maxLength >= 0 && this.text != null && this.text.Length > this.editor.maxLength)
            this.text = this.text.Substring(0, this.editor.maxLength);
          if (editor.keyboardOnScreen.done)
          {
            editor.keyboardOnScreen = (TouchScreenKeyboard) null;
            GUI.changed = true;
          }
        }
        string str = this.text;
        if (editor != null && !string.IsNullOrEmpty(editor.secureText))
          str = "".PadRight(editor.secureText.Length, this.maskChar);
        base.DoRepaint(painter);
        this.text = str;
      }
      else if (this.isPasswordField)
      {
        string text = this.text;
        this.text = "".PadRight(this.text.Length, this.maskChar);
        if (!this.hasFocus)
          base.DoRepaint(painter);
        else
          this.DrawWithTextSelectionAndCursor(painter, this.text);
        this.text = text;
      }
      else if (!this.hasFocus)
        base.DoRepaint(painter);
      else
        this.DrawWithTextSelectionAndCursor(painter, this.text);
    }

    private void DrawWithTextSelectionAndCursor(IStylePainter painter, string newText)
    {
      KeyboardTextEditor editor = this.editor as KeyboardTextEditor;
      if (editor == null)
        return;
      editor.PreDrawCursor(newText);
      int cursorIndex = editor.cursorIndex;
      int selectIndex = editor.selectIndex;
      Rect localPosition = editor.localPosition;
      Vector2 scrollOffset = editor.scrollOffset;
      IStyle style = this.style;
      TextStylePainterParameters defaultTextParameters = painter.GetDefaultTextParameters((VisualElement) this);
      defaultTextParameters.text = " ";
      defaultTextParameters.wordWrapWidth = 0.0f;
      defaultTextParameters.wordWrap = false;
      float textHeight = painter.ComputeTextHeight(defaultTextParameters);
      float width = !(bool) style.wordWrap ? 0.0f : this.contentRect.width;
      Input.compositionCursorPos = editor.graphicalCursorPos - scrollOffset + new Vector2(localPosition.x, localPosition.y + textHeight);
      Color color = !(this.cursorColor != Color.clear) ? GUI.skin.settings.cursorColor : this.cursorColor;
      int num1 = !string.IsNullOrEmpty(Input.compositionString) ? cursorIndex + Input.compositionString.Length : selectIndex;
      painter.DrawBackground((VisualElement) this);
      CursorPositionStylePainterParameters positionParameters;
      if (cursorIndex != num1)
      {
        RectStylePainterParameters defaultRectParameters = painter.GetDefaultRectParameters((VisualElement) this);
        defaultRectParameters.color = this.selectionColor;
        defaultRectParameters.borderLeftWidth = 0.0f;
        defaultRectParameters.borderTopWidth = 0.0f;
        defaultRectParameters.borderRightWidth = 0.0f;
        defaultRectParameters.borderBottomWidth = 0.0f;
        defaultRectParameters.borderTopLeftRadius = 0.0f;
        defaultRectParameters.borderTopRightRadius = 0.0f;
        defaultRectParameters.borderBottomRightRadius = 0.0f;
        defaultRectParameters.borderBottomLeftRadius = 0.0f;
        int num2 = cursorIndex >= num1 ? num1 : cursorIndex;
        int num3 = cursorIndex <= num1 ? num1 : cursorIndex;
        positionParameters = painter.GetDefaultCursorPositionParameters((VisualElement) this);
        positionParameters.text = editor.text;
        positionParameters.wordWrapWidth = width;
        positionParameters.cursorIndex = num2;
        Vector2 cursorPosition1 = painter.GetCursorPosition(positionParameters);
        positionParameters.cursorIndex = num3;
        Vector2 cursorPosition2 = painter.GetCursorPosition(positionParameters);
        Vector2 vector2_1 = cursorPosition1 - scrollOffset;
        Vector2 vector2_2 = cursorPosition2 - scrollOffset;
        if (Mathf.Approximately(vector2_1.y, vector2_2.y))
        {
          defaultRectParameters.layout = new Rect(vector2_1.x, vector2_1.y, vector2_2.x - vector2_1.x, textHeight);
          painter.DrawRect(defaultRectParameters);
        }
        else
        {
          defaultRectParameters.layout = new Rect(vector2_1.x, vector2_1.y, width - vector2_1.x, textHeight);
          painter.DrawRect(defaultRectParameters);
          float height = vector2_2.y - vector2_1.y - textHeight;
          if ((double) height > 0.0)
          {
            defaultRectParameters.layout = new Rect(0.0f, vector2_1.y + textHeight, width, height);
            painter.DrawRect(defaultRectParameters);
          }
          defaultRectParameters.layout = new Rect(0.0f, vector2_2.y, vector2_2.x, textHeight);
          painter.DrawRect(defaultRectParameters);
        }
      }
      painter.DrawBorder((VisualElement) this);
      if (!string.IsNullOrEmpty(editor.text) && (double) this.contentRect.width > 0.0 && (double) this.contentRect.height > 0.0)
      {
        defaultTextParameters = painter.GetDefaultTextParameters((VisualElement) this);
        defaultTextParameters.layout = new Rect(this.contentRect.x - scrollOffset.x, this.contentRect.y - scrollOffset.y, this.contentRect.width, this.contentRect.height);
        defaultTextParameters.text = editor.text;
        painter.DrawText(defaultTextParameters);
      }
      RectStylePainterParameters painterParameters;
      if (cursorIndex == num1 && (UnityEngine.Object) (Font) style.font != (UnityEngine.Object) null)
      {
        positionParameters = painter.GetDefaultCursorPositionParameters((VisualElement) this);
        positionParameters.text = editor.text;
        positionParameters.wordWrapWidth = width;
        positionParameters.cursorIndex = cursorIndex;
        Vector2 vector2 = painter.GetCursorPosition(positionParameters) - scrollOffset;
        painterParameters = new RectStylePainterParameters();
        painterParameters.layout = new Rect(vector2.x, vector2.y, 1f, textHeight);
        painterParameters.color = color;
        RectStylePainterParameters painterParams = painterParameters;
        painter.DrawRect(painterParams);
      }
      if (editor.altCursorPosition != -1)
      {
        positionParameters = painter.GetDefaultCursorPositionParameters((VisualElement) this);
        positionParameters.text = editor.text.Substring(0, editor.altCursorPosition);
        positionParameters.wordWrapWidth = width;
        positionParameters.cursorIndex = editor.altCursorPosition;
        Vector2 vector2 = painter.GetCursorPosition(positionParameters) - scrollOffset;
        painterParameters = new RectStylePainterParameters();
        painterParameters.layout = new Rect(vector2.x, vector2.y, 1f, textHeight);
        painterParameters.color = color;
        RectStylePainterParameters painterParams = painterParameters;
        painter.DrawRect(painterParams);
      }
      editor.PostDrawCursor();
    }
  }
}
