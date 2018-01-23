// Decompiled with JetBrains decompiler
// Type: UnityEngine.Experimental.UIElements.IMouseEvent
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

namespace UnityEngine.Experimental.UIElements
{
  public interface IMouseEvent
  {
    /// <summary>
    ///   <para>Flag set holding the pressed modifier keys (Alt, Control, Shift, Windows/Command).</para>
    /// </summary>
    EventModifiers modifiers { get; }

    /// <summary>
    ///   <para>The mouse position in the panel coordinate system.</para>
    /// </summary>
    Vector2 mousePosition { get; }

    /// <summary>
    ///   <para>The mouse position in the current target coordinate system.</para>
    /// </summary>
    Vector2 localMousePosition { get; }

    /// <summary>
    ///   <para>Mouse position difference between the last mouse event and this one.</para>
    /// </summary>
    Vector2 mouseDelta { get; }

    /// <summary>
    ///   <para>Number of clicks.</para>
    /// </summary>
    int clickCount { get; }

    /// <summary>
    ///   <para>Integer representing the pressed mouse button: 0 is left, 1 is right, 2 is center.</para>
    /// </summary>
    int button { get; }

    /// <summary>
    ///   <para>Return true if the Shift key is pressed.</para>
    /// </summary>
    bool shiftKey { get; }

    /// <summary>
    ///   <para>Return true if the Control key is pressed.</para>
    /// </summary>
    bool ctrlKey { get; }

    /// <summary>
    ///   <para>Return true if the Windows/Command key is pressed.</para>
    /// </summary>
    bool commandKey { get; }

    /// <summary>
    ///   <para>Return true if the Alt key is pressed.</para>
    /// </summary>
    bool altKey { get; }
  }
}
