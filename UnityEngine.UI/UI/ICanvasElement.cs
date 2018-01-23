// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ICanvasElement
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  public interface ICanvasElement
  {
    /// <summary>
    ///   <para>Rebuild the element for the given stage.</para>
    /// </summary>
    /// <param name="executing">Stage being rebuild.</param>
    void Rebuild(CanvasUpdate executing);

    /// <summary>
    ///   <para>Get the transform associated with the ICanvasElement.</para>
    /// </summary>
    Transform transform { get; }

    /// <summary>
    ///   <para>Callback sent when this ICanvasElement has completed layout.</para>
    /// </summary>
    void LayoutComplete();

    /// <summary>
    ///   <para>Callback sent when this ICanvasElement has completed Graphic rebuild.</para>
    /// </summary>
    void GraphicUpdateComplete();

    /// <summary>
    ///         <para>Return true if the element is considered destroyed.
    /// Used if the native representation has been destroyed.</para>
    ///       </summary>
    bool IsDestroyed();
  }
}
