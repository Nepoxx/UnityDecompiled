// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.IClippable
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F4B50A-137D-4022-9C58-045B696814A0
// Assembly location: C:\Program Files\Unity 5\Editor\Data\UnityExtensions\Unity\GUISystem\UnityEngine.UI.dll

namespace UnityEngine.UI
{
  public interface IClippable
  {
    /// <summary>
    ///   <para>GameObject of the IClippable.</para>
    /// </summary>
    GameObject gameObject { get; }

    /// <summary>
    ///   <para>Called when the state of a parent IClippable changes.</para>
    /// </summary>
    void RecalculateClipping();

    /// <summary>
    ///   <para>RectTransform of the clippable.</para>
    /// </summary>
    RectTransform rectTransform { get; }

    /// <summary>
    ///   <para>Clip and cull the IClippable given the clipRect.</para>
    /// </summary>
    /// <param name="clipRect">Rectangle to clip against.</param>
    /// <param name="validRect">Is the Rect valid. If not then the rect has 0 size.</param>
    void Cull(Rect clipRect, bool validRect);

    /// <summary>
    ///   <para>Set the clip rect for the IClippable.</para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validRect"></param>
    void SetClipRect(Rect value, bool validRect);
  }
}
