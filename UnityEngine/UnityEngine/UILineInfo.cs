// Decompiled with JetBrains decompiler
// Type: UnityEngine.UILineInfo
// Assembly: UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D290425A-E4B3-4E49-A420-29F09BB3F974
// Assembly location: C:\Program Files\Unity 5\Editor\Data\Managed\UnityEngine.dll

using UnityEngine.Scripting;

namespace UnityEngine
{
  /// <summary>
  ///   <para>Information about a generated line of text.</para>
  /// </summary>
  [UsedByNativeCode]
  public struct UILineInfo
  {
    /// <summary>
    ///   <para>Index of the first character in the line.</para>
    /// </summary>
    public int startCharIdx;
    /// <summary>
    ///   <para>Height of the line.</para>
    /// </summary>
    public int height;
    /// <summary>
    ///   <para>The upper Y position of the line in pixels. This is used for text annotation such as the caret and selection box in the InputField.</para>
    /// </summary>
    public float topY;
    /// <summary>
    ///   <para>Space in pixels between this line and the next line.</para>
    /// </summary>
    public float leading;
  }
}
